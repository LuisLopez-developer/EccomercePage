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
        private ClaimsPrincipal _authenticatedUser = new(new ClaimsIdentity());
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
            {
                Console.WriteLine("Already authenticated.");
                return new AuthenticationState(_authenticatedUser);
            }

            var user = Unauthenticated;
            try
            {
                var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    Console.WriteLine("Access token is null or empty.");
                    return new AuthenticationState(user);
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var userResponse = await _httpClient.GetAsync("/api/eccomerce/Account/GetUserInfo");
                if (!userResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to get user info.");
                    return new AuthenticationState(user);
                }

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

                    _authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(AuthService)));
                    _authenticated = true;

                    // Mensaje de depuración
                    Console.WriteLine($"User authenticated: {userInfo.UserName}, ID: {userInfo.Id}");
                }
                else
                {
                    // Mensaje de depuración
                    Console.WriteLine("UserInfo is null after deserialization.");
                }
            }
            catch (Exception ex)
            {
                // Mensaje de depuración
                Console.WriteLine($"Exception in GetAuthenticationStateAsync: {ex.Message}");
            }

            return new AuthenticationState(_authenticatedUser);
        }

        public async Task LogoutAsync()
        {
            const string Empty = "{}";
            var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync("api/User/logout", emptyContent);

            if (result.IsSuccessStatusCode)
            {
                await _localStorageService.RemoveItemAsync("accessToken");

                // Asegurar que solo una vez se notifique el cambio de estado
                _authenticated = false;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Unauthenticated)));
            }
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
                        _authenticated = true;
                        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                        return new ApiResponse { Success = true };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in LoginAsync: {ex.Message}");
            }

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

        public async Task<string> GetUserIdAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.PrimarySid);
                if (userIdClaim != null)
                {
                    Console.WriteLine($"User ID: {userIdClaim.Value}");
                    return userIdClaim.Value;
                }

            }

            return "";
        }
    }
}