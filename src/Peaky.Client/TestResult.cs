using Newtonsoft.Json.Linq;

namespace Peaky.Client;

[TypeFormatterSource(typeof(TypeFormatterSource))]
public class TestResult
{
    public TestOutcome Outcome { get; }

    public TestInfo Test { get; }

    public TestResult(string content, TestOutcome outcome)
    {
        Content = content;
        Outcome = outcome;
        var parsed = JObject.Parse(content);
        Test = (parsed["Test"] ?? parsed["test"])?.ToObject<TestInfo>();
    }

    public string Content { get; }

    public bool Passed => Outcome == TestOutcome.Passed;
}