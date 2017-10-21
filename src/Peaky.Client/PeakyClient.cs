using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pocket;

namespace Peaky.Client
{
    public class PeakyClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public PeakyClient(Uri serviceUri)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = serviceUri
            };

            _disposables.Add(_httpClient);
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

        public async Task<TestResult> GetResultFor(Uri url)
        {
            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            return new TestResult(content, response.StatusCode == HttpStatusCode.OK);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
