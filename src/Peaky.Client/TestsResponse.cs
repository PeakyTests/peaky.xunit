using System;
using System.Collections.Generic;

namespace Peaky.Client
{
    public class TestsResponse
    {
        public IEnumerable<Test> Tests { get; }

        public TestsResponse(IEnumerable<Test> tests)
        {
            Tests = tests;
        }
    }
}