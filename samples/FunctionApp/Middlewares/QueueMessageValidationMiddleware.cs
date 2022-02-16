using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace FunctionApp
{
    public sealed class QueueMessageValidationMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var bookInputBinding = await context.BindInputAsync<Book>();

            if (bookInputBinding != null)
            {
                if (bookInputBinding.name.Contains("Secret"))
                {
                    return;
                }
            }

            var inputs = context.GetInputData();
            var triggerMetaData = context.GetTriggerMetadata();

            await next(context);
        }
    }
}

