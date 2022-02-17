using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public sealed class GlobalExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
    {
        readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                var httpReqData = await context.BindInputAsync<HttpRequestData>();

                if (httpReqData != null)
                {
                    var newResponse = httpReqData.CreateResponse();
                    await newResponse.WriteAsJsonAsync(new { Status = "Failed", ErrorCode = "function-app-500" });

                    // Update invocation result.
                    var httpResponseData = context.GetInvocationResult<HttpResponseData>();
                    httpResponseData.Value = newResponse;


                    // OR Read the output bindings and update as needed
                    var queueOutputData = context.GetOutputBindings().FirstOrDefault(a => a.BindingType == "queue");
                    if (queueOutputData != null)
                    {
                        queueOutputData.Value = "Custom value from middleware";
                    }
                }    
                else
                {
                    context.GetInvocationResult<object>().Value = new { ProcessingStatus = "Unhealthy" };
                }
            }
        }
    }
}

