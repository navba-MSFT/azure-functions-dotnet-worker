using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class QeueueMessageValidationMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                //// To read input/trigger meta data.
                //var inputs = context.GetInputData();
                //var triggerMetaData = context.GetTriggerMetadata();


                //var bindingData = context.BindingContext.BindingData;

                //var ipBindings = context.FunctionDefinition.InputBindings;

                //BindingData<HttpRequestData> httpReq = await context.BindInput<HttpRequestData>();

                ////httpReq.Value =9;

                //var firstBinding = ipBindings.First();

                //object b = await context.BindInput(firstBinding.Value);

                var book =  await context.BindInputAsync<Book>();

                if (book != null)
                {

                }
                //BindingData<Book> b2 = await context.BindInput<Book>();

                //if (ipBindings.TryGetValue("myQueueItem", out var queueBindingMetadata))
                //{
                //    object b3 = await context.BindInput(queueBindingMetadata);
                //}


                await next(context);

            }
            catch (Exception ex)
            {
                context.GetLogger(nameof(QueueMessageValidationMiddleware)).LogError(ex, "error in function invocation");

                //BindingData<HttpRequestData> httpReq = context.BindInput<HttpRequestData>();

                //BindingData<Blob> b = context.BindInput<Blob>();

                //context.BindInput<string>("productId"); // productId is read from inputbinding ref

                // context needs to expose inputbinding in easy way
                // context.BindInput overload can take this input binding ref
                // cache the conversion result.
                //

                // Get http request(null for non http invocations)
                var httpRequest = await context.BindInputAsync<HttpRequestData>();

                if (httpRequest != null)
                {
                    var newResponse = httpRequest.CreateResponse();
                    await newResponse.WriteAsJsonAsync(new { Status = "Failed", ErrorCode = "function-app-500" });

                    // Update invocation result.
                    context.SetInvocationResult(newResponse);

                    // OR Read the output bindings and update as needed
                    IEnumerable<OutputBindingData> outputBindings = context.GetOutputBindings();

                    // Update the output for queue binding.
                    var queueOutputData = outputBindings.FirstOrDefault(a => a.BindingType == "queue");
                    if (queueOutputData != null)
                    {
                        queueOutputData.Value = "Custom value from middleware";
                    }
                }
            }
        }
    }
}

