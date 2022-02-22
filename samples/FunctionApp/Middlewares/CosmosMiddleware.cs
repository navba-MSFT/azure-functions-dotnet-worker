using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace FunctionApp
{
    public sealed class CosmosMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var bookInputBinding = await context.BindInputAsync<List<MyDocument>>();

            if (bookInputBinding != null)
            {
                
            }

            var inputs = context.GetInputData();
            var triggerMetaData = context.GetTriggerMetadata();

            await next(context);
        }
    }
}

