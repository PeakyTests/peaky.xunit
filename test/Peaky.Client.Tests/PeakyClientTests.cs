using System;
using Xunit;

namespace Peaky.Client.Tests
{
    public class PeakyClientTests
    {
        [Fact]
        public void It_can_load_tests_from_a_Peaky_service()
        {
            var client = new PeakyClient("http://peaky.azurewebsites.net/tests");


        }
    }
}
