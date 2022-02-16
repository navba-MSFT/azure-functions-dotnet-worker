using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public sealed class GlobalExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
    {
        ILogger<GlobalExceptionHandlerMiddleware> _logger;

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

                var httpReqBindingData = await context.BindInputAsync<HttpRequestData>();


                if (httpReqBindingData != null)
                {
                    var newResponse = httpReqBindingData.CreateResponse();
                    await newResponse.WriteAsJsonAsync(new { Status = "Failed", ErrorCode = "function-app-500" });

                    // Update invocation result.
                    var httpResponseData = context.GetInvocationResultData<HttpResponseData>();
                    httpResponseData.Value = newResponse;

                    //context.SetInvocationResult(newResponse);

                    // OR Read the output bindings and update as needed
                    IEnumerable<OutputBindingData> outputBindings = context.GetOutputBindings();

                    // Update the output for queue binding.
                    var queueOutputData = outputBindings.FirstOrDefault(a => a.BindingType == "queue");
                    if (queueOutputData != null)
                    {
                        queueOutputData.Value = "Custom value from middleware";
                    }
                }    
                else
                {
                    var httpResponseData = context.GetInvocationResultData<object>();
                    httpResponseData.Value = new { ProcessingStatus = "Unhealthy" };
                    //context.SetInvocationResult(new { ProcessingStatus="Unhealthy" });
                }
            }
        }
    }
}

