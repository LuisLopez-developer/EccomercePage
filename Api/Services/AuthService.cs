using Blazored.LocalStorage;
using EccomercePage.Api.Interfaces;
using EccomercePage.Api.Models;
using EccomercePage.Data.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EccomercePage.Api.Services
{
    public class AuthService : AuthenticationStateProvider, IAuthService
    {
        private bool _authenticated = false;
        private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly ILocalStorageService _localStorageService;

        public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_authenticated)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var user = Unauthenticated;
            try
            {
                var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");
                if (string.IsNullOrWhiteSpace(accessToken))
                    return new AuthenticationState(user);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var userResponse = await _httpClient.GetAsync("manage/info");
                if (!userResponse.IsSuccessStatusCode) return new AuthenticationState(user);

                var userInfo = JsonSerializer.Deserialize<UserInfo>(
                    await userResponse.Content.ReadAsStringAsync(), jsonSerializerOptions);

                if (userInfo != null)
                {
                    var claims = userInfo.Claims
                        .Select(c => new Claim(c.Key, c.Value))
                        .ToList();

                    claims.Add(new Claim(ClaimTypes.PrimarySid, userInfo.Id));
                    claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));
                    claims.Add(new Claim(ClaimTypes.Email, userInfo.Email));
                    
                    userInfo.Roles?.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

                    user = new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(AuthService)));
                    _authenticated = true;
                }
            }
            catch { }

            return new AuthenticationState(user);
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("api/user/Logout", new StringContent("{}", Encoding.UTF8, "application/json"));
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/eccomerce/Account/register",
                    new { registerDTO.UserName, registerDTO.Email, registerDTO.Password });
                if (response.IsSuccessStatusCode)
                    return new ApiResponse { Success = true };

                var errorResponse = await ParseErrorResponseAsync(response);
                return new ApiResponse { Success = false, Errors = errorResponse.Errors };
            }
            catch
            {
                return new ApiResponse
                {
                    Success = false,
                    Errors = new[] { "Un error desconocido impidió que el registro se realizara correctamente." }
                };
            }
        }

        public async Task<ApiResponse> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/eccomerce/Account/login", new { loginDTO.UserNameOrEmail, loginDTO.Password });

                if (response.IsSuccessStatusCode)
                {
                    var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(
                        await response.Content.ReadAsStringAsync(), jsonSerializerOptions);

                    if (tokenInfo != null)
                    {
                        await _localStorageService.SetItemAsync("accessToken", tokenInfo.AccessToken);
                        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                        return new ApiResponse { Success = true };
                    }
                }
            }
            catch { }

            return new ApiResponse
            {
                Success = false,
                Message = "Usuario inválido y/o contraseña."
            };
        }

        private async Task<ApiResponse> ParseErrorResponseAsync(HttpResponseMessage response)
        {
            var details = await response.Content.ReadAsStringAsync();
            var errors = JsonDocument.Parse(details)
                .RootElement.GetProperty("errors")
                .EnumerateObject()
                .SelectMany(error => error.Value.ValueKind == JsonValueKind.String
                    ? new[] { error.Value.GetString() }
                    : error.Value.EnumerateArray().Select(e => e.GetString()))
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToArray();

            return new ApiResponse { Errors = errors };
        }
    }
}