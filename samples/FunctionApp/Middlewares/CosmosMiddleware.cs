using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace FunctionApp
{

    public sealed class CosmosMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var cosmosTriggerBinding = context.FunctionDefinition.InputBindings.Values
                .FirstOrDefault(a=>a.Type== InputBindingTypes.CosmosDBTrigger);

            if (cosmosTriggerBinding != null)
            {
                var bookInputBinding = await context.BindInputAsync<List<MyDocument>>(cosmosTriggerBinding);

                if (bookInputBinding != null)
                {

                }
            }

            await next(context);
        }
    }
}

