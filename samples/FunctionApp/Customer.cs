using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    [InputConverter(typeof(MyCustomerConverter))]
    public sealed class Customer
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public decimal Price { set; get; }
    }

    public sealed class MyCustomerConverter : IFunctionInputConverter
    {
        private readonly ILogger<MyCustomerConverter> _logger;

        public MyCustomerConverter(ILogger<MyCustomerConverter> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._logger.LogInformation($"Custom converter {nameof(MyCustomerConverter)} instance created");
        }

        public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
        {
            var customerIdObj = context.Source;

            if (customerIdObj == null || !int.TryParse(customerIdObj.ToString(), out int customerId))
            {
                return new ValueTask<ConversionResult>(ConversionResult.Failed());
            }

            var customerViewModel = new Customer
            {
                Id = customerId,
                Name = $"From MyCustomerConverter Id:{customerId}"
            };

            var bindingResult = ConversionResult.Success(customerViewModel);
            this._logger.LogInformation($"Successfully bound using Custom converter {nameof(MyCustomerConverter)}");

            return new ValueTask<ConversionResult>(bindingResult);
        }
    }
}
