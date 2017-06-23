using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Peaky.Client
{
    public class PeakyClient :IDisposable
    {
        private readonly HttpClient _httpClient;

        public PeakyClient(Uri serviceUri) :
            this(new HttpClient
            {
                BaseAddress = serviceUri
            })
        {
        }

        public PeakyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var response = await _httpClient.GetAsync("/tests");

            var content = await response.Content.ReadAsStringAsync();

            var testResponse = JsonConvert.DeserializeObject<TestsResponse>(content);

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
            _httpClient.Dispose();
        }
    }
}
