using System;

namespace Peaky.Client
{
    public class TestResult
    {
        public TestResult(string content, bool passed)
        {
            Content = content;

            Passed = passed;
        }

        public string Content { get; }

        public bool Passed { get; }
    }
}