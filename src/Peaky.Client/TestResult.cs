using Newtonsoft.Json.Linq;

namespace Peaky.Client;

[TypeFormatterSource(typeof(TypeFormatterSource))]
public class TestResult
{
    public TestResult(string content, TestOutcome outcome, PeakyClient client)
    {
        Content = content;
        Outcome = outcome;
        var parsed = JObject.Parse(content);
        Test = (parsed["Test"] ?? parsed["test"])?.ToObject<Test>();
        Test.PeakyClient = client;
    }

    public TestOutcome Outcome { get; }

    public Test Test { get; }

    public string Content { get; }

    public bool Passed => Outcome == TestOutcome.Passed;
}