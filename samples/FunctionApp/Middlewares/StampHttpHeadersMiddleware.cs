using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public sealed class StampHttpHeadersMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var logger = context.GetLogger<StampHttpHeadersMiddleware>();
            var requestId = "azf-" + Guid.NewGuid();

            using (logger.BeginScope("azfunc-requestid:{requestId}", requestId))
            {
                await next(context);
            }

            var httpRequestData = await context.GetHttpRequestDataAsync();
            if (httpRequestData != null)
            {
                var httpResponseData = context.GetInvocationResult();  //.GetHttpResponseData();
                if (httpResponseData != null)
                {
                    if (httpResponseData.Value is HttpResponseData httpResponse)
                    {
                        httpResponse.Headers.Add("x-azfunc-requestid", requestId);
                    }
                }
            }
        }
    }
}

