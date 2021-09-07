using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public sealed class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public decimal Price { set; get; }
    }

    public sealed class MyCustomerConverter : IConverter
    {
        ILogger<MyCustomerConverter> _logger;

        public MyCustomerConverter(ILogger<MyCustomerConverter> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._logger.LogInformation($"Custom converter {nameof(MyCustomerConverter)} instance created");
        }

        public ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {
            context.FunctionContext.BindingContext.BindingData.TryGetValue("customerId", out var customerIdObj);

            if (customerIdObj == null)
                return new ValueTask<BindingResult>(BindingResult.Failed());

            var customerViewModel = new CustomerViewModel { Id = Convert.ToInt32(customerIdObj), Name ="From Complex Converter" };
            BindingResult result = BindingResult.Success(customerViewModel);
            this._logger.LogInformation($"Successfully bound using Custom converter {nameof(MyCustomerConverter)}");

            return new ValueTask<BindingResult>(result);
        }
    }

    public sealed class MyComplexCustomerConverter : IConverter
    {
        ILogger<MyCustomerConverter> _logger;

        public MyComplexCustomerConverter(ILogger<MyCustomerConverter> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._logger.LogInformation($"Custom converter {nameof(MyCustomerConverter)} instance created");
        }

        public ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {
            context.FunctionContext.BindingContext.BindingData.TryGetValue("customerId", out var customerIdObj);

            if (customerIdObj == null)
                return new ValueTask<BindingResult>(BindingResult.Failed());

            var customerViewModel = new CustomerViewModel { Id = Convert.ToInt32(customerIdObj) , Name ="From Complex Converter" };
            BindingResult result = BindingResult.Success(customerViewModel);
            this._logger.LogInformation($"Successfully bound using Custom converter {nameof(MyCustomerConverter)}");

            return new ValueTask<BindingResult>(result);
        }
    }
}
