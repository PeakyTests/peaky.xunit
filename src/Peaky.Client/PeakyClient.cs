﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pocket;

namespace Peaky.Client
{
    public class PeakyClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public PeakyClient(Uri serviceUri)
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = serviceUri
            };

            _disposables.Add(_httpClient);
            _disposables.Add(handler);
        }

        public PeakyClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var response = await _httpClient.GetAsync("");

            var content = await response.Content.ReadAsStringAsync();

            var testResponse = JsonConvert.DeserializeObject<TestsResponse>(content);

            if (testResponse == null)
            {
                return Array.Empty<Test>();
            }

            return testResponse.Tests;
        }

        public async Task<TestResult> GetResultFor(Test test)
        {
            return await GetResultFor(test.Url);
        }

        public async Task<TestResult> GetResultFor(Uri url, TimeSpan? maxIntervalForRetrial = null)
        {
            var currentAttempt = 1;
             const int maxAttempts = 10;
            var random = new Random();
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var testOutcome = ParseTestOutCome(response.StatusCode, content);
            while (response.StatusCode == HttpStatusCode.ServiceUnavailable && currentAttempt < maxAttempts)
            {
                var parsed = JObject.Parse(content);
                var supportsRetry = (parsed["SupportsRetry"] ?? parsed["supportsRetry"])?.Value<bool>() == true;

                if (!supportsRetry)
                {
                    return new TestResult(content, testOutcome);
                }

                var maxMinutes = (int) (maxIntervalForRetrial?? TimeSpan.FromMinutes(10)).TotalMinutes;
                if (maxMinutes > 0)
                {
                    var waitInterval = random.Next(1, maxMinutes);
                    await Task.Delay(TimeSpan.FromMinutes(waitInterval));
                }

                currentAttempt++;
                response = await _httpClient.GetAsync(url);
                content = await response.Content.ReadAsStringAsync();
            }

            return new TestResult(content, testOutcome);
        }

        private TestOutcome ParseTestOutCome(HttpStatusCode responseStatusCode, string content)
        {
            TestOutcome outcome;
            try
            {
                var parsed = JObject.Parse(content);

                var outComeString = (parsed["Test"] ?? parsed["test"])?.Value<string>();
                if (!Enum.TryParse(outComeString, true, out outcome))
                {
                    outcome = responseStatusCode == HttpStatusCode.OK ? TestOutcome.Passed : TestOutcome.Failed;
                }
            }
            catch
            {
                outcome = responseStatusCode == HttpStatusCode.OK ? TestOutcome.Passed : TestOutcome.Failed;
            }
            return outcome;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
