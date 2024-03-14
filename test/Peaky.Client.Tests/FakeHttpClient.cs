using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Peaky.Client.Tests;

internal class FakeHttpClient : HttpClient
{
    public FakeHttpClient(Func<HttpRequestMessage, HttpResponseMessage> handle) : base(new FakeMessageHandler(handle))
    {
    }

    private class FakeMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> handle;

        public FakeMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handle)
        {
            this.handle = handle;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.Run(() => handle(request));
        }
    }
}