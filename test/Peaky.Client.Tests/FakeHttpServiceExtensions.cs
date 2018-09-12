using System;
using FakeHttpService;
using Microsoft.AspNetCore.Http;

namespace Peaky.Client.Tests
{
    public static class FakeHttpServiceExtensions
    {
        public static FakeHttpService.FakeHttpService WithTestResultAt(this FakeHttpService.FakeHttpService subject,
            string relativeUri, string content, bool passed)
        {
            return subject
                .OnRequest(r => r.GetUri().ToString().EndsWith(relativeUri))
                .RespondWith(async r =>
                {
                    await r.Body.WriteTextAsUtf8BytesAsync(content);
                    r.StatusCode = passed ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
                });
        }

        public static FakeHttpService.FakeHttpService WithRetriableTestResultAt(this FakeHttpService.FakeHttpService subject,
            string relativeUri, string content, bool passed, Action attemptCallBack)
        {
            var attempt = 0;
            return subject
                .OnRequest(r => r.GetUri().ToString().EndsWith(relativeUri))
                .RespondWith(async r =>
                {
                    

                    await r.Body.WriteTextAsUtf8BytesAsync(content);
                    if (attempt == 0)
                    {
                        attempt++;
                        r.StatusCode = passed ? StatusCodes.Status200OK : StatusCodes.Status503ServiceUnavailable;
                    }
                    else
                    {
                        attemptCallBack?.Invoke();
                        r.StatusCode = passed ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
                    }
                });
        }

    }
}