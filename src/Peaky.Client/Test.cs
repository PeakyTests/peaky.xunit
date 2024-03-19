using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Peaky.Client;

[TypeFormatterSource(typeof(TypeFormatterSource))]
public class Test
{
    public Test(string application, string environment, Uri url, IEnumerable<string> tags = null)
    {
        Application = application;

        Environment = environment;

        Url = url;

        Tags = tags?.ToArray() ?? Array.Empty<string>();
    }

    public string Application { get; }

    public string Environment { get; }

    public Uri Url { get; }

    public IEnumerable<string> Tags { get; }

    [JsonIgnore] 
    internal PeakyClient PeakyClient { get; set; }

    public async Task<TestResult> GetResultAsync()
    {
        if (PeakyClient is null)
        {
            throw new InvalidOperationException($"{nameof(PeakyClient)} was not set.");
        }

        return await PeakyClient.GetTestResultAsync(Url);
    }
}