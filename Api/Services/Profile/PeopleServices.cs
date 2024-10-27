using EccomercePage.Api.Interfaces;
using EccomercePage.Data.DTO.Profile;
using System.Net.Http.Json;
using System.Text.Json;

namespace EccomercePage.Api.Services.Profile
{
    public class PeopleServices : IPeopleService
    {
        private readonly string api = "api/People";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions =
          new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          };

        private readonly IAuthService _authService;

        public PeopleServices(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
            _authService = authService;
        }

        public async Task<ApiResponse> AddPeople(AddPeopleDTO dto)
        {
            dto.UserId = await _authService.GetUserIdAsync(); 

            var response = await _httpClient.PostAsJsonAsync($"{api}", dto, jsonSerializerOptions);

            var errorResponse = await ParseErrorResponseAsync(response);
            return new ApiResponse
            {
                Success = response.IsSuccessStatusCode,
                Errors = errorResponse.Errors
            };
        }

        private async Task<ApiResponse> ParseErrorResponseAsync(HttpResponseMessage response)
        {
            var details = await response.Content.ReadAsStringAsync();
            var errors = JsonDocument.Parse(details)
                .RootElement.GetProperty("errors")
                .EnumerateObject()
                .SelectMany(error => error.Value.ValueKind == JsonValueKind.String
                    ? [error.Value.GetString()]
                    : error.Value.EnumerateArray().Select(e => e.GetString()))
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToArray();

            return new ApiResponse { Errors = errors };
        }

    }
}
