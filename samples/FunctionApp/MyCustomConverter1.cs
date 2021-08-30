using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;

namespace FunctionApp
{
    public sealed class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { set;get;}
        public decimal Price { set;get;}
    }
    
    public sealed class MyCustomConverter1 : IConverter
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MyCustomConverter1(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {
            if (context.Parameter.Type != typeof(ProductViewModel))
            {
                return await new ValueTask<BindingResult>(BindingResult.Failed());
            }

            int prodId = 0;
            if (context.FunctionContext.BindingContext.BindingData.TryGetValue("productId", out var productIdValObj))
            {
                prodId = Convert.ToInt32(productIdValObj);
            }
            var reqMsg = new HttpRequestMessage(HttpMethod.Get,$"https://shkr-playground.azurewebsites.net/api/products/{prodId}");

            var client = this._httpClientFactory.CreateClient();

            using var response = await client.SendAsync(reqMsg);
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var productVm = await JsonSerializer.DeserializeAsync<ProductViewModel>(stream, SharedJsonSettings.SerializerOptions);

                return await new ValueTask<BindingResult>(BindingResult.Success(productVm));
            }
        }
    }

    internal static class SharedJsonSettings
    {
        public static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}
