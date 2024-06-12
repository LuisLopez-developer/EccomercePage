using EccomercePage.Interfaces.ProductInterfaces;
using EccomercePage.Models;
using System.Net.Http.Json;

namespace EccomercePage.Services.ProductServices
{
    public class ProductCategoryService : IProductCategory
    {
        private readonly string api = "api/ProductCategory";
        private readonly HttpClient _httpClient;

        public ProductCategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }
        public async Task<List<BasicFilterModel>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<BasicFilterModel>>($"{api}/GetAllPublic");
            return response ?? new List<BasicFilterModel>(); 
        }
    }
}
