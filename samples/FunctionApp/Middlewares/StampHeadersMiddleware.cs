using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace FunctionApp
{
    public sealed class StampHeadersMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            await next(context);

            var httpRequestData = await context.GetHttpRequestData();
            if (httpRequestData != null)
            {
                //var r = context.GetInvocationResult();
                var httpResponse = context.GetInvocationResult<HttpResponseData>();
                if (httpResponse != null)
                {
                    httpResponse.Headers.Add("x-azfunc-reqid", DateTime.Now.ToString());
                }
            }
        }
    }
}

