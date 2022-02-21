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
            var requestId = "azfunc-" + Guid.NewGuid();

            using (logger.BeginScope("AzFunc-RequestId:{requestId}", requestId))
            {
                await next(context);
            }

            var httpRequestData = await context.GetHttpRequestData();
            if (httpRequestData != null)
            {
                var httpResponseData = context.GetHttpResponseData();
                if (httpResponseData != null)
                {
                    httpResponseData.Headers.Add("x-azfunc-requestid", requestId);
                }
            }
        }
    }
}

