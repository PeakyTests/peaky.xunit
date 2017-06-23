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
    }
}