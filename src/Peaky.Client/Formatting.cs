using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Web;
using Newtonsoft.Json;
using static System.Net.WebUtility;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Peaky.Client;

[AttributeUsage(AttributeTargets.Class)]
internal class TypeFormatterSourceAttribute : Attribute
{
    public TypeFormatterSourceAttribute(Type formatterSourceType)
    {
        FormatterSourceType = formatterSourceType;
    }

    public Type FormatterSourceType { get; }
}

internal class TypeFormatterSource
{
    public IEnumerable<object> CreateTypeFormatters()
    {
        yield return new TestResultPlainTextFormatter();
        yield return new TestResultHtmlFormatter();

        yield return new TestHtmlFormatter();

        yield return new TestListHtmlFormatter();
    }
}

internal class TestListHtmlFormatter
{
    public string MimeType => "text/html";

    public bool Format(object instance, TextWriter writer)
    {
        if (instance is not TestList testList)
        {
            return false;
        }

        WriteTestList(testList, writer);

        return true;
    }

    public static void WriteTestList(TestList tests, TextWriter writer)
    {
        foreach (var environment in tests.Select(t => t.Environment).Distinct())
        {
            var testsForEnvironment = tests.Where(t => t.Environment == environment).ToArray();

            var testCount = testsForEnvironment.Length switch
            {
                1 => "1 test",
                _ => testsForEnvironment.Length + " tests"
            };

            writer.WriteLine($"""
                <div>
                    <h2>{environment} ({testCount})</h2>
                """);

            foreach (var test in testsForEnvironment)
            {
                TestHtmlFormatter.WriteTest(test, writer);
            }

            writer.WriteLine("""
                </div>
                """);
        }
    }
}

internal class TestHtmlFormatter
{
    public string MimeType => "text/html";

    public bool Format(object instance, TextWriter writer)
    {
        if (instance is not Test test)
        {
            return false;
        }

        WriteTest(test, writer);

        return true;
    }

    public static void WriteTest(Test test, TextWriter writer)
    {
        var urlString = test.Url.ToString();

        writer.Write($"""
            <details>
                <summary>🧪<a href="{HttpUtility.HtmlAttributeEncode(urlString)}">{HtmlEncode(urlString)}</a></summary>
                <div style="display:inline-block;margin-left:3em;">
                    Application: {test.Application}
                    <br/>
                    Environment: {test.Environment}
                    <br/>
                    Tags: {string.Join(",", test.Tags)}
                </div>
            </details>
            """);
    }
}

internal class TestResultPlainTextFormatter
{
    public string MimeType => "text/plain";

    public bool Format(object instance, TextWriter writer)
    {
        if (instance is not TestResult result)
        {
            return false;
        }

        if (result.Passed)
        {
            writer.Write($"""
                ✅ Passed: {result.Test.Name}
                """);
        }
        else
        {
            writer.Write($"""
                ❌ Failed: {result.Test.Name}
                """);
        }

        return true;
    }
}

internal class TestResultHtmlFormatter
{
    public string MimeType => "text/html";

    public bool Format(object instance, TextWriter writer)
    {
        if (instance is not TestResult result)
        {
            return false;
        }

        var summary = result.Passed
                          ? $"""
                ✅ Passed: {result.Test.Name}
                """
                          : $"""
                ❌ Failed: {result.Test.Name}
                """;

        writer.Write("<details>");
        writer.Write($"<summary>{HtmlEncode(summary)}</summary>");

        TestHtmlFormatter.WriteTest(result.Test, writer);

        var content = result.Content;

        if (TryIndentJson(content, out var formattedJson))
        {
            content = formattedJson;
        }

        writer.Write($"<pre>{HtmlEncode(content)}</pre>");

        writer.Write("</details>");

        return true;

        static bool TryIndentJson(string content, out string formattedJson)
        {
            try
            {
                using var json = JsonDocument.Parse(content);

                formattedJson = JsonSerializer.Serialize(
                    json.RootElement,
                    new
                        JsonSerializerOptions
                        {
                            WriteIndented = true
                        });

                return true;
            }
            catch
            {
                formattedJson = null;
                return false;
            }

        }
    }
}