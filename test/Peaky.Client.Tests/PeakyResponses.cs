using System;
using Newtonsoft.Json.Linq;

namespace Peaky.Client.Tests;

public class PeakyResponses
{
    public static string Tests { get; } = @"{
  ""Tests"": [
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/bing_homepage_returned_in_under_5ms"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/homepage_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/images_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/maps_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/rewards_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/sign_in_link_is_present"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""microsoft"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/microsoft/homepage_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""microsoft"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/microsoft/surface_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""microsoft"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/microsoft/windows_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/bing_homepage_returned_in_under_5ms"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/homepage_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/images_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/maps_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/rewards_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/sign_in_link_is_present"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            }
            ]
        }";


    public static string CreateFailedTestResultJson(Exception error, string application, string environment, string testName, string testUrl, params string[] tags)
    {
        var obj = new JObject
        {
            ["Message"] = error.Message,
            ["Passed"] = false,
            ["Exception"] = JToken.FromObject(error),
            ["Test"] = new JObject
            {
                ["Application"] = application,
                ["Environment"] = environment,
                ["Name"] = testName,
                ["Url"] = new Uri(testUrl),
                ["Tags"] = JToken.FromObject(tags ?? Array.Empty<string>())
            }
        };
        return obj.ToString();
    }

    public static string CreateFailedRetriableTestResultJson(Exception error, string application, string environment, string testName, string testUrl, params string[] tags)
    {
        var obj = new JObject
        {
            ["Message"] = error.Message,
            ["Passed"] = false,
            ["SupportsRetry"] = true,
            ["Exception"] = JToken.FromObject(error),
            ["Test"] = new JObject
            {
                ["Application"] = application,
                ["Environment"] = environment,
                ["Name"] = testName,
                ["Url"] = new Uri(testUrl),
                ["Tags"] = JToken.FromObject(tags ?? Array.Empty<string>())
            }
        };
        return obj.ToString();
    }

    public static string CreatePassedTestResultJson(object result, string application, string environment, string testName, string testUrl, params string[] tags)
    {
        var obj = new JObject
        {
            ["ReturnValue"] = JToken.FromObject(result),
            ["Passed"] = true,
            ["Test"] = new JObject
            {
                ["Application"] = application,
                ["Environment"] = environment,
                ["Name"] = testName,
                ["Url"] = new Uri(testUrl),
                ["Tags"] = JToken.FromObject(tags ?? Array.Empty<string>())
            }
        };
        return obj.ToString();
    }
}