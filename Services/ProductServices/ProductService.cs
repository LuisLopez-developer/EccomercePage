using EccomercePage.Interfaces.ProductInterfaces;
using EccomercePage.Models.ProductModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace EccomercePage.Services.ProductServices
{
    public class ProductService : IProduct
    {
        private readonly string api = "api/Product";
        private readonly HttpClient _httpClient;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        private readonly JsonSerializerOptions jsonSerializerOptions =
          new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          };

        public async Task<List<ProductCatalogViewModel>> GetMostValuableProductCatalog()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductCatalogViewModel>>($"{api}/MostValablesProducts");
            return products ?? [];
        }
    }
}
