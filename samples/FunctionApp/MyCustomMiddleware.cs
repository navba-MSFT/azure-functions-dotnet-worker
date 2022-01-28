using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Context.Features;
using Microsoft.Azure.Functions.Worker.Grpc.Features;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{

    public class MyCustomMiddleware : IFunctionsWorkerMiddleware
    {
        // read any input/trigger data
        // update any input/trigger data
        /// read/update invocationresult.
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var httpRequest = context.GetHttpRequestData();

            // Augment request
            httpRequest.Headers.Add("x-foo","bar");

            if (!httpRequest.Cookies.Any(a=>a.Name=="my-auth-token"))
            {
                var response = context.CreateHttpResponse(System.Net.HttpStatusCode.OK);
                response.Headers.Add("x-azf-rid", "qwerty");
                response.Body = new MemoryStream(Encoding.ASCII.GetBytes("hello world"));

                var feature = context.Features.Get<IFunctionBindingsFeature>();
                feature.InvocationResult = response;

                return;
            }

            await next(context);

        }
    }
}
