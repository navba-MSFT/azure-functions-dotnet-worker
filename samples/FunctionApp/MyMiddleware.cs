using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Context.Features;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class MyMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // This is added pre-function execution, function will have access to this information
            // in the context.Items dictionary
            context.Items.Add("middlewareitem", "Hello, from middleware");

            context.BindingContext.BindingData.TryGetValue("id", out var Id);

            var converterContext = new MyConverterContext
            {
                TargetType =  typeof(Customer),
                Source = Id,
                FunctionContext = context,
                //Properties = new Dictionary<string, object>
                //{
                //    // PropertyBagKeys.ConverterType
                //    { "converterType", typeof(MyProductVmConverter) }
                //}
            };
            
            // Get the input conversion feature and call TryConvert
            var conversionFeature = context.Features.Get<IFunctionInputConversionFeature>();
            var conversionResult = await conversionFeature.TryConvertAsync(converterContext);

            //var modelBindingFeature = (DefaultModelBindingFeature)context.Features.Get<IModelBindingFeature>();
            //await modelBindingFeature.BindFunctionInputAsync(context);

            await next(context);

            // This happens after function execution. We can inspect the context after the function
            // was invoked
            if (context.Items.TryGetValue("functionitem", out object value) && value is string message)
            {
                ILogger logger = context.GetLogger<MyMiddleware>();

                logger.LogInformation("From function: {message}", message);
            }
        }
    }

    public class MyConverterContext : ConverterContext
    {
        public override Type TargetType { get ; set; }
        public override IReadOnlyDictionary<string, object> Properties { get ; set; }
        public override object Source { get ; set; }
        public override FunctionContext FunctionContext { get ; set; }
    }
}
