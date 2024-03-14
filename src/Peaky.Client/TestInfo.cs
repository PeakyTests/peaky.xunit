using System;
using Newtonsoft.Json;

namespace Peaky.Client;

public class TestInfo
{
    public string Application { get; }

    public string Name { get; }

    public string Environment { get; }

    public Uri Url { get; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string[] Tags { get; }

    public TestInfo(string application,
        string environment,
        string name,
        Uri url,
        string[] tags = null)
    {

        if (string.IsNullOrWhiteSpace(application))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(application));
        }

        if (string.IsNullOrWhiteSpace(environment))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(environment));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        }

        Application = application;
        Environment = environment;
        Tags = tags;
        Name = name;
        Url = url ?? throw new ArgumentNullException(nameof(url));

    }

}