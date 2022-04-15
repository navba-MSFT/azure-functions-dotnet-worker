using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class MyHttpMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // This is added pre-function execution, function will have access to this information
            // in the context.Items dictionary
            context.Items.Add("middlewareitem", "Hello, from middleware");
            ILogger logger = context.GetLogger<MyHttpMiddleware>();

            logger.LogInformation("From MyHttpMiddleware " + DateTime.Now);

            await next(context);

            // This happens after function execution. We can inspect the context after the function
            // was invoked
            if (context.Items.TryGetValue("functionitem", out object value) && value is string message)
            {

            }
        }
    }
}
