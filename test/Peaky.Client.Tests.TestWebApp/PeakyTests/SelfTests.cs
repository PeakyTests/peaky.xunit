// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Peaky.Client.Tests.TestWebApp.PeakyTests;

public class SelfTests : IPeakyTest, IApplyToApplication, IHaveTags
{
    private static int _attemptCount = 0;

    private readonly TestTarget _testTarget;

    public SelfTests(TestTarget testTarget)
    {
        _testTarget = testTarget;
    }

    public bool AppliesToApplication(string application)
    {
        return application is "TestWebApp";
    }

    public Task<string> this_test_passes()
    {
        return Task.FromResult("OK!");
    }

    public void this_test_also_passes()
    {
        Console.WriteLine("OK!");
    }

    public void this_test_fails_in_staging()
    {
        if (_testTarget.Environment is "staging")
        {
            throw new Exception("oops!");
        }
    }

    public string[] Tags => new[] { "self" };
}