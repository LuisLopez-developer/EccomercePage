using EccomercePage.Api.Interfaces;
using EccomercePage.Data.DTO.CartDTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace EccomercePage.Api.Services
{
    public class PaymentMethodService : IGetRepository<PaymentMethodDTO>
    {
        private readonly string api = "/api/PaymentMethod";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions =
          new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          };

        public PaymentMethodService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IEnumerable<PaymentMethodDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<PaymentMethodDTO>>(api) ?? [];
        }
    }
}
