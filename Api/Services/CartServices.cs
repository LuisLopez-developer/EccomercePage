using EccomercePage.Api.Interfaces;
using EccomercePage.Data.DTO.CartDTO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace EccomercePage.Api.Services
{
    public class CartServices : ICartService
    {
        private readonly string api = "api/cart";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions =
          new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          };

        public CartServices(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public List<string> Errors => throw new NotImplementedException();

        public async Task<bool> ChangeItemQuantity(ChangeItemQuantityDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{api}/changeItemQuantity", dto, jsonSerializerOptions);
            return response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NoContent;
        }


        public Task<CartReponseDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CartReponseDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CartReponseDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CartItemsPaymentDTO> GetCartItemsPaymentAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<CartItemsPaymentDTO>($"{api}/itemsPayment/{userId}") ?? new CartItemsPaymentDTO();
        }

        public async Task<CartResumeDTO> GetCartResumeAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<CartResumeDTO>($"{api}/resume/{userId}") ?? new CartResumeDTO();
        }

        public async Task<decimal> GetTotalAmountCartAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<decimal>($"{api}/totalAmount/{userId}");
        }

        public async Task<int> GetTotalProductInCartAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<int>($"{api}/total/{userId}");
        }

        public async Task<CartReponseDTO> InsertAsync(AddProductCartDTO insertDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(api, insertDTO, jsonSerializerOptions);
            var CartResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartReponseDTO>(CartResponse, jsonSerializerOptions) ?? new CartReponseDTO();
        }

        public Task<CartReponseDTO> UpdateAsync(int id, UpdateCartDTO updateDTO)
        {
            throw new NotImplementedException();
        }

        public bool Validate(AddProductCartDTO dto)
        {
            throw new NotImplementedException();
        }

        public bool Validate(UpdateCartDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
