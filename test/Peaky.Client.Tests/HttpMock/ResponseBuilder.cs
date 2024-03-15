using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Peaky.Client.Tests.HttpMock;

public class ResponseBuilder
{
    private readonly FakeHttpService _fakeHttpService;

    private readonly Expression<Func<HttpRequest, bool>> _requestValidator;

    internal ResponseBuilder(FakeHttpService fakeHttpService, Expression<Func<HttpRequest, bool>> requestValidator)
    {
        _requestValidator = requestValidator;

        _fakeHttpService = fakeHttpService;
    }

    public void RespondWith(Func<HttpResponse, Task> responseConfiguration)
    {
        if (responseConfiguration is null)
        {
            throw new ArgumentNullException(nameof(responseConfiguration));
        }

        _fakeHttpService.Setup(_requestValidator, ResponseFunction);

        async Task ResponseFunction(HttpResponse c)
        {
            await responseConfiguration(c);
        }
    }

    public FakeHttpService RespondWith(Func<HttpResponse, Uri, Task> responseConfiguration)
    {
        if (responseConfiguration == null) throw new ArgumentNullException(nameof(responseConfiguration));

        async Task ResponseFunction(HttpResponse c)
        {
            await responseConfiguration(c, _fakeHttpService.BaseAddress);
        }

        _fakeHttpService.Setup(_requestValidator, ResponseFunction);

        return _fakeHttpService;
    }

    public void Succeed()
    {
        RespondWith(async r =>
        {
            r.StatusCode = 200;
            await Task.Yield();
        });
    }

    public void Fail()
    {
        RespondWith(async r =>
        {
            r.StatusCode = 500;
            await Task.Yield();
        });
    }
}