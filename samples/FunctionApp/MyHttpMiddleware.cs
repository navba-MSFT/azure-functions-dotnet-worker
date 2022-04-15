using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public sealed class MyHttpMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            ILogger logger = context.GetLogger<MyHttpMiddleware>();

            logger.LogInformation($"From MyHttpMiddleware {DateTime.Now}");

            await next(context);

            // Stamp a response header.
            context.GetHttpResponseData().Headers.Add("X-REQ-ID", Guid.NewGuid().ToString());
        }
    }
}
