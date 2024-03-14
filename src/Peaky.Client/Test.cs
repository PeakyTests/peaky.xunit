using System;
using System.Collections.Generic;

namespace Peaky.Client;

public class Test
{
    public Test(string application, string environment, Uri url, IEnumerable<string> tags)
    {
        Application = application;

        Environment = environment;

        Url = url;

        Tags = tags;
    }

    public string Application { get; }

    public string Environment { get; }

    public Uri Url { get; }

    public IEnumerable<string> Tags { get; }
}