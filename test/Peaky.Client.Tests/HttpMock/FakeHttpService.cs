using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pocket;
using static Pocket.Logger<Peaky.Client.Tests.HttpMock.FakeHttpService>;

namespace Peaky.Client.Tests.HttpMock;

public class FakeHttpService : IDisposable
{
    private readonly IWebHost _host;

    private readonly List<Tuple<Expression<Func<HttpRequest, bool>>, Func<HttpResponse, Task>>> _handlers;

    private readonly List<Test> _testList = new();

    public FakeHttpService()
    {
        _handlers = new List<Tuple<Expression<Func<HttpRequest, bool>>, Func<HttpResponse, Task>>>();
        ServiceId = Guid.NewGuid().ToString();

        FakeHttpServiceRepository.Register(this);

        var config = new ConfigurationBuilder().Build();

        var builder = new WebHostBuilder()
                      .UseConfiguration(config)
                      .UseKestrel()
                      .UseStartup<Startup>()
                      .UseSetting("applicationName", ServiceId)
                      .UseUrls("http://127.0.0.1:0");

        _host = builder.Build();

        _host.Start();

        BaseAddress = new Uri(_host
                              .ServerFeatures.Get<IServerAddressesFeature>()
                              .Addresses.First());

        OnRequest(r => r.GetUri().ToString().EndsWith("/tests"))
            .RespondWith(async response => { await response.WriteAsJsonAsync(new TestsResponse(_testList)); });
    }

    internal FakeHttpService Setup(Expression<Func<HttpRequest, bool>> condition, Func<HttpResponse, Task> response)
    {
        _handlers.Add(new Tuple<Expression<Func<HttpRequest, bool>>, Func<HttpResponse, Task>>(condition, response));

        Log.Info("Setting up condition {condition}", new ConstantMemberEvaluationVisitor().Visit(condition));

        return this;
    }

    public ResponseBuilder OnRequest(Expression<Func<HttpRequest, bool>> condition)
    {
        return new ResponseBuilder(this, condition);
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            foreach (var handler in _handlers)
            {
                if (!handler.Item1.Compile().Invoke(context.Request))
                {
                    continue;
                }

                await handler.Item2(context.Response);

                return;
            }

            context.Response.StatusCode = 404;

            Debug.WriteLine($"No handler for request{Environment.NewLine}{context.Request.Method} {context.Request.Path}");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;

            await context.Response.WriteAsync(e.ToString());
        }
    }

    public Uri BaseAddress { get; }

    public string ServiceId { get; }

    public void Dispose()
    {
        Task.Run(() => _host.Dispose()).Wait();

        FakeHttpServiceRepository.Unregister(this);
    }

    public FakeHttpService WithTestResultAt(
        string relativeUri,
        string content,
        bool passed,
        string applicationName = "MyApplication",
        string environmentName = "MyEnvironment",
        string[] tags = null)
    {
        if (!relativeUri.StartsWith("/"))
        {
            throw new ArgumentException($"{nameof(relativeUri)} must begin with '/'");
        }

        OnRequest(r => r.GetUri().ToString().EndsWith(relativeUri))
            .RespondWith(async r =>
            {
                await r.Body.WriteTextAsUtf8BytesAsync(content);
                r.StatusCode = passed ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
            });

        AddToTestList(relativeUri, applicationName, environmentName, tags);

        return this;
    }

    private void AddToTestList(string relativeUri, string applicationName, string environmentName, string[] tags = null)
    {
        var testUri = new Uri($"{BaseAddress}/tests/{environmentName}/{applicationName}/{relativeUri}");

        _testList.Add(new(applicationName, environmentName, testUri, tags));
    }

    public FakeHttpService WithRetriableTestResultAt(
        string relativeUri,
        string content,
        bool passed,
        Action attemptCallBack,
        string applicationName = "MyApplication",
        string environmentName = "MyEnvironment")
    {
        var attempt = 0;

        OnRequest(r => r.GetUri().ToString().EndsWith(relativeUri))
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

        AddToTestList(relativeUri, applicationName, environmentName);

        return this;
    }

    public override string ToString() => $"\"{ServiceId}\" @ {BaseAddress}";
}