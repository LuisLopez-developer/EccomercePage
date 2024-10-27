using EccomercePage.Api.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace EccomercePage.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly string api = "/api/eccomerce/Order";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions =
          new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          };

        public OrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public async Task<bool> GenerateOrderThroughCart(int cartId, int paymentMethodId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{api}", new { cartId, paymentMethodId }, jsonSerializerOptions);
            return response.IsSuccessStatusCode;       
        }
    }
}
