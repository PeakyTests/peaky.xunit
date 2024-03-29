// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Peaky.Client.Tests.TestWebApp;
using Xunit;

namespace Peaky.Client.Tests;

public class PeakyClientTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public PeakyClientTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task It_can_load_tests_from_a_Peaky_service_using_HttpClient()
    {
        var peakyClient = new PeakyClient(_httpClient);

        var tests = await peakyClient.GetTestsAsync();

        tests.Should().HaveCount(7);
    }

    [Fact]
    public async void It_retrieves_attributes_from_a_Peaky_service()
    {
        var client = new PeakyClient(_httpClient);

        var tests = await client.GetTestsAsync();

        tests.Should().Contain(t => t.Application == "TestWebApp" &&
                                    t.Environment == "production");
        tests.Should().Contain(t => t.Application == "TestWebApp" &&
                                    t.Environment == "staging");

        tests.Should().Contain(t => t.Tags.Contains("dependencies"));
    }

    [Fact]
    public async void It_retrieves_results_for_a_passing_test()
    {
        var client = new PeakyClient(_httpClient);

        var tests = await client.GetTestsAsync(environment: "staging");

        var test = tests.First(t => t.Url.ToString().EndsWith("/this_test_passes"));

        var testResult = await test.GetResultAsync();

        testResult.Passed.Should().BeTrue();
        testResult.Test.Application.Should().Be("TestWebApp");
        testResult.Test.Environment.Should().Be("staging");
        testResult.Test.Tags.Should().BeEquivalentTo("self");
        testResult.Content.Should().Contain("OK!");
    }

    [Fact]
    public async void It_retrieves_results_for_a_failing_test()
    {
        var client = new PeakyClient(_httpClient);

        var test = (await client.GetTestsAsync(environment: "staging")).Single(t => t.Url.ToString().EndsWith("/this_test_fails_in_staging"));

        var testResult = await test.GetResultAsync();

        testResult.Passed.Should().BeFalse();
        testResult.Test.Application.Should().Be("TestWebApp");
        testResult.Test.Environment.Should().Be("staging");
        testResult.Test.Tags.Should().BeEquivalentTo("self");
        testResult.Content.Should().Contain("oops!");
    }

    [Fact]
    public async void It_retrieves_results_for_a_passing_test_by_url()
    {
        var client = new PeakyClient(_httpClient);

        var tests = await client.GetTestsAsync(environment: "staging");

        var test = tests.First(t => t.Url.ToString().EndsWith("/this_test_passes"));

        var testResult = await client.GetTestResultAsync(test.Url);

        testResult.Passed.Should().BeTrue();
        testResult.Test.Application.Should().Be("TestWebApp");
        testResult.Test.Environment.Should().Be("staging");
        testResult.Test.Tags.Should().BeEquivalentTo("self");
        testResult.Content.Should().Contain("OK!");
    }

    [Fact]
    public async void It_retrieves_results_for_a_failing_test_by_url()
    {
        var client = new PeakyClient(_httpClient);

        var test = (await client.GetTestsAsync(environment: "staging")).Single(t => t.Url.ToString().EndsWith("/this_test_fails_in_staging"));

        var testResult = await client.GetTestResultAsync(test.Url);

        testResult.Passed.Should().BeFalse();
        testResult.Test.Application.Should().Be("TestWebApp");
        testResult.Test.Environment.Should().Be("staging");
        testResult.Test.Tags.Should().BeEquivalentTo("self");
        testResult.Content.Should().Contain("oops!");
    }

    [Fact]
    public async void When_the_response_contains_no_peaky_tests_Then_it_returns_an_empty_list()
    {
        var client = new PeakyClient(_httpClient);

        var tests = await client.GetTestsAsync(application: "nonexistent");

        tests.Should().BeEmpty();
    }

    [Fact]
    public async Task It_can_filter_tests_by_application()
    {
        var client = new PeakyClient(_httpClient);

        var tests = await client.GetTestsAsync(application: "testuri");

        tests.Should().HaveCount(1);
    }

    [Fact]
    public async Task It_can_filter_tests_by_environment()
    {
        var client = new PeakyClient(_httpClient);

        var tests = await client.GetTestsAsync(environment: "production");

        tests.Should().HaveCount(3);
    }
}