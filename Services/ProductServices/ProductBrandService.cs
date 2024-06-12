using EccomercePage.Interfaces.ProductInterfaces;
using EccomercePage.Models;
using System.Net.Http.Json;

namespace EccomercePage.Services.ProductServices
{
    public class ProductBrandService : IProductBrand
    {
        private readonly string api = "api/ProductBrand";
        private readonly HttpClient _httpClient;

        public ProductBrandService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public async Task<List<BasicFilterModel>> GetAllAsync()
        {
            var productBrand = await _httpClient.GetFromJsonAsync<List<BasicFilterModel>>($"{api}/GetAllPublic");
            return productBrand ?? new List<BasicFilterModel>();
        }

    }
}
