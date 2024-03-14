using System;
using System.Linq;
using System.Threading.Tasks;
using FakeHttpService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Peaky.Client.Tests;

public class PeakyClientTests
{
    [Fact]
    public async void It_can_load_tests_from_a_Peaky_service_by_BaseAddress()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithContentAt("/tests", PeakyResponses.Tests);
        var client = new PeakyClient( new Uri(server.BaseAddress, "/tests"));

        var tests = await client.GetTests();

        tests.Should().HaveCount(15);
    }
   
    [Fact]
    public async void It_can_load_tests_from_a_Peaky_service_with_specific_environment()
    {
        Uri requestUri = null;
        string requestPath= string.Empty;

        using var server = new FakeHttpService.FakeHttpService().OnRequest(r => true).RespondWith(async (response, uri) =>
        {
            requestUri = uri;
            requestPath = response.HttpContext.Request.Path.Value;
        });
        var client = new PeakyClient(new Uri(server.BaseAddress, @"/tests/test"));

        await client.GetTests();

        requestPath.Should().Be(@"/tests/test");
    }
        
    [Fact]
    public async void It_retrieves_attributes_from_a_Peaky_service()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithContentAt("/tests", PeakyResponses.Tests);
        var client = new PeakyClient(new Uri(server.BaseAddress, @"/tests"));

        var tests = await client.GetTests();

        var test = tests.First();

        test.Application.Should().Be("bing");

        test.Environment.Should().Be("prod");

        test.Url.Should()
            .Be("/tests/prod/bing/bing_homepage_returned_in_under_5ms");

        test.Tags.Should().BeEquivalentTo("LiveSite", "NonSideEffecting");
    }

    [Fact]
    public async void It_retrieves_results_for_a_passing_test()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithTestResultAt("/a_passing_test", PeakyResponses.CreatePassedTestResultsFor("abcdefg", "testApp", "test", "a passing test", "http://server.path/a_passing_test", "a", "b") , true);
        var client = new PeakyClient( new Uri(server.BaseAddress, @"/tests"));

        var test = new Test("", "", new Uri(server.BaseAddress, "/a_passing_test"), null);

        var testResult = await client.GetResultFor(test);

        testResult.Passed.Should().BeTrue();
        testResult.Test.Application.Should().Be("testApp");
        testResult.Test.Environment.Should().Be("test");
        testResult.Test.Tags.Should().BeEquivalentTo("a", "b");
        testResult.Content.Should().Contain("abcdefg");
    }

    [Fact]
    public async void It_retrieves_results_for_a_failing_test()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithTestResultAt("/a_failing_test", PeakyResponses.CreateFailedTestResultsFor(new Exception("jijij"), "testApp", "test", "a failing test", "http://server.path/a_failing_test", "a", "b"), false);
        var client = new PeakyClient( new Uri(server.BaseAddress, @"/tests"));

        var test = new Test("", "", new Uri(server.BaseAddress, "/a_failing_test"), null);

        var testResult = await client.GetResultFor(test);

        testResult.Passed.Should().BeFalse();
        testResult.Test.Application.Should().Be("testApp");
        testResult.Test.Environment.Should().Be("test");
        testResult.Test.Tags.Should().BeEquivalentTo("a", "b");
        testResult.Content.Should().Contain("jijij");
    }
     
    [Fact]
    public async void It_retrieves_results_for_a_passing_test_Url()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithTestResultAt("/a_passing_test", PeakyResponses.CreatePassedTestResultsFor("abcdefg", "testApp", "test", "a passing test", "http://server.path/a_passing_test", "a", "b"), true);
        var client = new PeakyClient( new Uri(server.BaseAddress, @"/tests"));

        var test = new Test("", "", new Uri(server.BaseAddress, "/a_passing_test"), null);

        var testResult = await client.GetResultFor(test.Url);

        testResult.Passed.Should().BeTrue();
        testResult.Passed.Should().BeTrue();
        testResult.Test.Application.Should().Be("testApp");
        testResult.Test.Environment.Should().Be("test");
        testResult.Test.Tags.Should().BeEquivalentTo("a", "b");
        testResult.Content.Should().Contain("abcdefg");
    }

    [Fact]
    public async void It_retrieves_results_for_a_failing_test_Url()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithTestResultAt("/a_failing_test", PeakyResponses.CreateFailedTestResultsFor(new Exception("hijklmnop"), "testApp", "test", "a failing test", "http://server.path/a_failing_test", "a", "b") , false);
        var client = new PeakyClient( new Uri(server.BaseAddress, @"/tests"));

        var test = new Test("", "", new Uri(server.BaseAddress, "/a_failing_test"), null);

        var testResult = await client.GetResultFor(test.Url);

        testResult.Passed.Should().BeFalse();

        testResult.Content.Should().Contain("hijklmnop");
    }

    [Fact]
    public async void It_retrieves_results_for_a_failing_test_Url_and_performs_another_attempt()
    {
        var attempted = false;
        using var server = new FakeHttpService.FakeHttpService()
            .WithRetriableTestResultAt(
                "/a_failing_test", PeakyResponses.CreateFailedRetriableTestResultsFor(new Exception("hijklmnop"), "testApp", "test", "a failing test", "http://server.path/a_failing_test", "a", "b"), false, () => attempted = true);
        var client = new PeakyClient(new Uri(server.BaseAddress, @"/tests"));

        var test = new Test("", "", new Uri(server.BaseAddress, "/a_failing_test"), null);

        var testResult = await client.GetResultFor(test.Url, TimeSpan.Zero);

        attempted.Should().BeTrue();
        testResult.Passed.Should().BeFalse();

        testResult.Content.Should().Contain("hijklmnop");
    }

    [Fact]
    public async void When_the_response_contains_no_peaky_tests_Then_it_returns_an_empty_list()
    {
        using var server = new FakeHttpService.FakeHttpService()
            .WithTestResultAt("/no_test", "hijklmnop", false);
        var client = new PeakyClient(new Uri(server.BaseAddress, @"/tests"));

        var tests = await client.GetTests();

        tests.Should().BeEmpty();
    }

    [Fact]
    public async Task It_returns_the_Peaky_session_cookie_on_subsequent_requests()
    {
        var requestCount = 0;
        var sessionId = Guid.NewGuid().ToString();
        var receivedSessionIdOnSecondRequest = false;

        using (var server = new FakeHttpService.FakeHttpService()
                            .OnRequest(_ => true)
                            .RespondWith(async response =>
                            {   await response.WriteAsync("{}");
                                if (requestCount == 0)
                                {
                                    requestCount++;
                                    response.Cookies.Append("peaky-session", sessionId);
                                     
                                }
                                else
                                {
                                    if (response.HttpContext.Request.Cookies.TryGetValue("peaky-session", out var value))
                                    {
                                        receivedSessionIdOnSecondRequest = value == sessionId;
                                    }
                                }
                            }))
        {
            var client = new PeakyClient(server.BaseAddress);

            await client.GetResultFor(new Uri(server.BaseAddress, "/tests/a_test"));
            await client.GetResultFor(new Uri(server.BaseAddress, "/tests/a_test"));
        }

        receivedSessionIdOnSecondRequest.Should().BeTrue();
    }
}