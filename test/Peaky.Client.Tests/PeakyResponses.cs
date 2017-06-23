namespace Peaky.Client.Tests
{
    public class PeakyResponses
    {
        public static string Tests { get; } = @"{
  ""Tests"": [
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/bing_homepage_returned_in_under_5ms"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/homepage_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/images_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/maps_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/rewards_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/bing/sign_in_link_is_present"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""microsoft"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/microsoft/homepage_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""microsoft"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/microsoft/surface_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""microsoft"",
                ""Environment"": ""prod"",
                ""Url"": ""/tests/prod/microsoft/windows_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/bing_homepage_returned_in_under_5ms"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/homepage_should_return_200OK"",
                ""Tags"": [
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/images_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/maps_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/rewards_should_return_200OK"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            },
            {
                ""Application"": ""bing"",
                ""Environment"": ""test"",
                ""Url"": ""/tests/test/bing/sign_in_link_is_present"",
                ""Tags"": [
                ""LiveSite"",
                ""NonSideEffecting""
                    ]
            }
            ]
        }";

        public static string TestResult { get; } = @"{
        ""Message"": ""Expected a value less than 5, but found 381."",
        ""Exception"": ""{ AssertionFailedException: Message = Expected a value less than 5, but found 381. | Data = {  } | InnerException = [null] | TargetSite = Void Throw(System.String) | StackTrace =    at FluentAssertions.Execution.FallbackTestFramework.Throw(String message)\r\n   at FluentAssertions.Execution.TestFrameworkProvider.Throw(String message)\r\n   at FluentAssertions.Execution.DefaultAssertionStrategy.HandleFailure(String message)\r\n   at FluentAssertions.Execution.AssertionScope.FailWith(String message, Object[] args)\r\n   at FluentAssertions.Numeric.NumericAssertions`1.BeLessThan(T expected, String because, Object[] becauseArgs)\r\n   at Peaky.SampleWebApplication.BingTests.bing_homepage_returned_in_under_5ms()\r\n   at lambda_method(Closure , BingTests )\r\n   at Peaky.TestDefinition`1.Run(HttpActionContext context, Func`2 resolver)\r\n   at Peaky.PeakyTestController.<Run>d__1.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Threading.Tasks.TaskHelpersExtensions.<CastToObject>d__3`1.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ApiControllerActionInvoker.<InvokeActionAsyncCore>d__0.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Filters.ActionFilterAttribute.<CallOnActionExecutedAsync>d__5.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Web.Http.Filters.ActionFilterAttribute.<CallOnActionExecutedAsync>d__5.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Filters.ActionFilterAttribute.<ExecuteActionFilterAsyncCore>d__0.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ActionFilterResult.<ExecuteAsync>d__2.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ExceptionFilterResult.<ExecuteAsync>d__0.MoveNext() | HelpLink = [null] | Source = FluentAssertions | HResult = -2146233088 }""
}";
    }
}