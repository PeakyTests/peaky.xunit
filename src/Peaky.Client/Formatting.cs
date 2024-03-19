using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using static System.Net.WebUtility;

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
        yield return new TestInfoHtmlFormatter();
    }
}

internal class TestInfoHtmlFormatter
{
    public bool Format(object instance, TextWriter writer)
    {
        if (instance is not TestInfo info)
        {
            return false;
        }

        WriteTestInfo(info, writer);

        return true;
    }

    public static void WriteTestInfo(TestInfo test, TextWriter textWriter)
    {
        var urlString = test.Url.ToString();

        textWriter.Write($"""
            <div>
            <a href="{HttpUtility.HtmlAttributeEncode(urlString)}">{HtmlEncode(urlString)}</a>
            <br/>
            Application: {test.Application}
            <br/>
            Environment: {test.Environment}
            <br/>
            Tags: {string.Join(",", test.Tags)}
            </div>
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

        TestInfoHtmlFormatter.WriteTestInfo(result.Test, writer);

        writer.Write($"<pre>{HtmlEncode(result.Content)}</pre>");

        writer.Write("</details>");

        return true;
    }
}