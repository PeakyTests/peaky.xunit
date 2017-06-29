[![Build status](https://ci.appveyor.com/api/projects/status/kf2mh9ecqlpyxmme?svg=true)](https://ci.appveyor.com/project/piotrpMSFT/peaky-xunit)
[![NuGet Badge](https://buildstats.info/nuget/peaky.xunit?includePreReleases=true)](https://www.nuget.org/packages/peaky.xunit)


# peaky.xunit
Run your [Peaky](https://github.com/PhillipPruett/Peaky) tests to validate new deployments using this simple library. 
Peaky.XUnit makes it easy for xunit to invoke tests from a Peaky endpoint, immediately integrating Peaky test results into your existing test execution infrastructure.
We built Peaky.Xunit to simplify service validation inside our Visual Studio Online Release Management pipeline, and the same library can be used anywhere else you want to validate your Peaky tests.

## Benefits
* Validate Peaky tests in XUnit
* Get Peaky results anywhere XUnit runs, including:
 - Visual Studio, or your favorite IDE   
 - Build/Release scripts via `dotnet test`
 - bash, zsh, powershell, cmd, etc. via `dotnet test`
* Integration into results management systems like VSO's Test Run Explorer and Jenkins' Xunit Plugin when executed as `dotnet test --log:trx`

## Samples
These samples are written targetting the sample Peaky endpoint at http://peaky.azurewebsites.net/tests. Just plug in your own Peaky URI to get started!

### Run all Peaky Tests for a service
```
using System;
using Xunit;
using Peaky.XUnit;
using FluentAssertions;
using Peaky.Client;

namespace Peaky.Sample.Tests
{
    public class PeakySampleTests : PeakyXunitTestBase, IDisposable
    {
        private readonly PeakyClient _peakyClient = new PeakyClient(new Uri("https://peaky.azurewebsites.net"));

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
```

### Run all Peaky Tests for a specific environment
Note that the only change is the URL passed to PeakyClient. Cool!
```
using System;
using Xunit;
using Peaky.XUnit;
using FluentAssertions;
using Peaky.Client;

namespace Peaky.Sample.Tests
{
    public class PeakySampleTests : PeakyXunitTestBase, IDisposable
    {
        private readonly PeakyClient _peakyClient = new PeakyClient(new Uri("https://peaky.azurewebsites.net/prod"));

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
```

### Thanks!
Big thanks to @jonsequitur and @piotroko for their contributions to this project!
