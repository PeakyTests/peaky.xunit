using System.Collections.Generic;

namespace Peaky.Client;

public class TestsResponse
{
    public TestsResponse(IEnumerable<Test> tests)
    {
        Tests = tests;
    }

    public IEnumerable<Test> Tests { get; }
}