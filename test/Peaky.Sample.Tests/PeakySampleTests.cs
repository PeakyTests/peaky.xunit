using System;
using FluentAssertions;
using Peaky.Client;
using Peaky.XUnit;
using Xunit;

namespace Peaky.Sample.Tests;

public class PeakySampleTests : PeakyXunitTestBase
{
    public override PeakyClient PeakyClient { get; } = new(new Uri("http://peaky-sample.azurewebsites.net/tests"));

    [Theory]
    [ClassData(typeof(PeakySampleTests))]
    public async void The_peaky_test_passes(Uri url)
    {
        var result = await PeakyClient.GetTestResultAsync(url);

        result.Passed.Should().BeTrue();
    }
}