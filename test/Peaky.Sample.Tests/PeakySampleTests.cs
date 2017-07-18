using System;
using Xunit;
using Peaky.XUnit;
using FluentAssertions;
using Peaky.Client;

namespace Peaky.Sample.Tests
{
    public class PeakySampleTests : PeakyXunitTestBase, IDisposable
    {
        private readonly PeakyClient _peakyClient = new PeakyClient(new Uri("https://peaky.azurewebsites.net/tests"));

        public override PeakyClient PeakyClient => _peakyClient;
        

        [Theory]
        [ClassData(typeof(PeakySampleTests))]
        public async void The_peaky_test_passes(Uri url)
        {
            var result = await PeakyClient.GetResultFor(url);
            
            result.Passed.Should().BeTrue();
        }
        
        public void Dispose()
        {
            _peakyClient.Dispose();
        }
    }
}
