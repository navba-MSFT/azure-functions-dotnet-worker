using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace FunctionApp
{
    public sealed class ModifyInputMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var http = await context.GetHttpRequestData();
            var myServiceBusMsg = await context.BindInputAsync<MyServiceBusMessage>();

            var ips = context.GetInputData();
            var tr=context.GetTriggerMetadata();

            if (myServiceBusMsg != null)
            {
                myServiceBusMsg.Name+= "-Modified";
            }

            await next(context);
        }
    }
}

