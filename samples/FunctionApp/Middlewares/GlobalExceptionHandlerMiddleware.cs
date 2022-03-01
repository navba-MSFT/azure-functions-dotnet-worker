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
                BindingMetadata sampleBlobBindingMetaData = context.FunctionDefinition
                                                                   .InputBindings.Values.Where(a => a.Type == "blob")
                                                                   .Skip(1)
                                                                   .FirstOrDefault();

                if (sampleBlobBindingMetaData != null)
                {
                    var sampleBlob = await context.BindInputAsync<MyBlob>(sampleBlobBindingMetaData);
                    sampleBlob.Name="edited name";
                }

                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                var httpReqData = await context.GetHttpRequestDataAsync();

                if (httpReqData != null)
                {
                    var newResponse = httpReqData.CreateResponse();
                    await newResponse.WriteAsJsonAsync(new { Status = "Failed", ErrorCode = "function-app-500" });

                    // Update invocation result.
                    var httpInvoationResult = context.GetInvocationResult();
                    httpInvoationResult.Value = newResponse;                   

                    // OR Read the output bindings and update as needed
                    var queueOutputData = context.GetOutputBindings<object>().FirstOrDefault(a => a.BindingType == "queue");
                    if (queueOutputData != null)
                    {
                        queueOutputData.Value = "Custom value from middleware";
                    }
                }
                else
                {
                    context.GetInvocationResult().Value = new { ProcessingStatus = "Unhealthy" };
                }
            }
        }
    }
}

