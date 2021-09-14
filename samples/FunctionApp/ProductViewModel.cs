using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    [ParameterBinder(typeof(MyProductVmConverter))]
    public sealed class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public decimal Price { set; get; }
    }

    public sealed class MyProductVmConverter : IConverter
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MyProductVmConverter> _logger;
        public MyProductVmConverter(IHttpClientFactory httpClientFactory, ILogger<MyProductVmConverter> logger)
        {
            this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this._logger = logger;
        }

        public async ValueTask<ParameterBindingResult> ConvertAsync(ConverterContext context)
        {
            if (context.Parameter.Type != typeof(ProductViewModel))
            {
                return ParameterBindingResult.Failed();
            }

            int prodId = 0;
            if (context.FunctionContext.BindingContext.BindingData.TryGetValue("productId", out var productIdValObj))
            {
                prodId = Convert.ToInt32(productIdValObj);
            }

            var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"https://shkr-playground.azurewebsites.net/api/products/{prodId}");
            var client = this._httpClientFactory.CreateClient();

            using var response = await client.SendAsync(reqMsg);
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var productVm = await JsonSerializer.DeserializeAsync<ProductViewModel>(stream, SharedJsonSettings.SerializerOptions);
                this._logger.LogInformation($"Received product info from REST API for {prodId}");

                return ParameterBindingResult.Success(productVm);
            }
        }
    }
}
