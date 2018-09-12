using Newtonsoft.Json.Linq;

namespace Peaky.Client
{
    public class TestResult
    {
        public TestInfo Test { get; set; }
     
        public TestResult(string content, bool passed)
        {
            Content = content;
            Passed = passed;

            var parsed = JObject.Parse(content);
            
            Test = (parsed["Test"] ?? parsed["test"])?.ToObject<TestInfo>();

        }

        public string Content { get; }

        public bool Passed { get; }
    }
}