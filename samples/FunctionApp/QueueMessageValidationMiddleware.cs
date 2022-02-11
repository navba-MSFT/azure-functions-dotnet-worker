using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace FunctionApp
{

    public sealed class QueueMessageValidationMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var bookInputBinding = await context.BindInput<Book>();

            if (bookInputBinding?.Value != null)
            {
                if (bookInputBinding.Value.name.Contains("Secret"))
                {
                    return;
                }
            }

            await next(context);
        }
    }
}

