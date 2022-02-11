using System;
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

                var httpReqBindingData = await  context.BindInput<HttpRequestData>();
                if (httpReqBindingData != null)
                {
                    var newResponse = httpReqBindingData.Value.CreateResponse();
                    await newResponse.WriteAsJsonAsync(new { Status = "Failed", ErrorCode = "function-app-500" });

                    // Update invocation result.
                    context.SetInvocationResult(newResponse);
                }    
                else
                {
                    context.SetInvocationResult(new { ProcessingStatus="Unhealthy" });
                }
            }
        }
    }
}

