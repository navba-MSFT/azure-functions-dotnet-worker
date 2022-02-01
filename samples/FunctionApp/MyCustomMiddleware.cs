using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Context.Features;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class MyCustomMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {       
                context.GetLogger(nameof(MyCustomMiddleware)).LogError(ex,"error in function invocation");
                
                // Gets the HttpRequestData instance.
                var httpRequest = context.GetHttpRequestData();
                if (httpRequest != null)
                {
                    var response = context.CreateHttpResponse(HttpStatusCode.InternalServerError);
                    await response.WriteAsJsonAsync(new { Status = "Failed", ErrorCode = "function-app-500" });

                    var feature = context.Features.Get<IFunctionBindingsFeature>();

                    // Update invocation response.
                    feature.InvocationResult = response;

                    // Or output binding data in the case of POCO with multiple output attributes
                    feature.SetOutputBindingData("Name","foo-bar");
                    feature.SetOutputBindingData("HttpResponse", response);
                }
            }
        }
    }
}
