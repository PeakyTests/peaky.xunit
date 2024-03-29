// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Peaky.Client.Tests.TestWebApp.PeakyTests;

public class DependencyTests : IPeakyTest, IApplyToApplication, IHaveTags
{
    private readonly HttpClient _httpClient;

    public DependencyTests(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public bool AppliesToApplication(string application)
    {
        return application is "testuri";
    }

    public string[] Tags => new[] { "dependencies" };

    public async Task<string> testuri_is_reachable()
    {
        var result = await _httpClient.GetAsync("https://testuri.org");

        result.EnsureSuccessStatusCode();

        return await result.Content.ReadAsStringAsync();
    }
}