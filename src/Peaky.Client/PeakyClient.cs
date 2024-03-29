using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Peaky.Client;

public class PeakyClient
{
    private static readonly Lazy<HttpClient> _staticHttpClient;

    private readonly HttpClient _httpClient;
    private readonly Uri _testRootUri;

    static PeakyClient()
    {
        _staticHttpClient = new Lazy<HttpClient>(() =>
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };
            return new(handler);
        });
    }

    public PeakyClient(Uri testRootUri)
    {
        _testRootUri = testRootUri;
        _httpClient = _staticHttpClient.Value;
    }

    public PeakyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _testRootUri = new(_httpClient.BaseAddress ?? throw new ArgumentException("HttpClient.BaseAddress must be specified"), "/tests");
    }

    public async Task<TestList> GetTestsAsync(
        string application = null,
        string environment = null)
    {
        var uri = _testRootUri.ToString();

        if (environment is not null)
        {
            uri += $"/{environment}";
        }

        if (application is not null)
        {
            uri += $"/{application}";
        }

        var response = await _httpClient.GetAsync(uri);

        var content = await response.Content.ReadAsStringAsync();

        var testResponse = JsonConvert.DeserializeObject<TestsResponse>(content);

        if (testResponse is null)
        {
            return new();
        }

        var tests = testResponse.Tests;
        var testList = new TestList();

        foreach (var test in tests)
        {
            test.PeakyClient = this;
            testList.Add(test);
        }

        return testList;
    }

    public async Task<TestResult> GetTestResultAsync(Test test)
    {
        return await GetTestResultAsync(test.Url);
    }

    public async Task<TestResult> GetTestResultAsync(Uri url)
    {
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var testOutcome = ParseTestOutCome(response.StatusCode, content);

        return new TestResult(content, testOutcome, this);
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
}