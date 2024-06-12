using EccomerceApi.Model;
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

        public async Task<PagedResultModel<ProductCatalogViewModel>> GetProductCatalogWithFiltersAsync(int page, int pageSize, string searchTerm, int? brandId, int? categoryId, string? model, decimal? minimumPrice, decimal? maximunPrice)
        {
            var response = await _httpClient.GetFromJsonAsync<PagedResultModel<ProductCatalogViewModel>>($"{api}/GetProductCatalogWithFilters?page={page}&pageSize={pageSize}&searchTerm={searchTerm}&brandId={brandId}&categoryId={categoryId}&model={model}&minimumPrice={minimumPrice}&maximunPrice={maximunPrice}");
            return response ?? new PagedResultModel<ProductCatalogViewModel>();
        }
    }
}
