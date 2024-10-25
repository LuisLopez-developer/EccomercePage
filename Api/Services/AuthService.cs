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

        private readonly ClaimsPrincipal Unauthenticated =
           new(new ClaimsIdentity());

        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions jsonSerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly ILocalStorageService _localStorageService;

        public AuthService(IHttpClientFactory httpClientFactory,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_authenticated)
            {
                // Si ya está autenticado, no realizar nuevamente la llamada a /manage/info
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // devolver el estado actual
            }

            _authenticated = false;
            var user = Unauthenticated;

            try
            {
                var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return new AuthenticationState(user);
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var userResponse = await _httpClient.GetAsync("manage/info");
                userResponse.EnsureSuccessStatusCode();

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

                if (userInfo != null)
                {
                    var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userInfo.Email),
                new(ClaimTypes.Email, userInfo.Email)
            };

                    claims.AddRange(
                      userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                    .Select(c => new Claim(c.Key, c.Value)));

                    var rolesResponse = await _httpClient.GetAsync($"api/Role/GetuserRole?userEmail={userInfo.Email}");
                    rolesResponse.EnsureSuccessStatusCode();
                    var rolesJson = await rolesResponse.Content.ReadAsStringAsync();

                    var roles = JsonSerializer.Deserialize<string[]>(rolesJson, jsonSerializerOptions);
                    if (roles != null && roles.Length > 0)
                    {
                        foreach (var role in roles)
                        {
                            claims.Add(new(ClaimTypes.Role, role));
                        }
                    }

                    var id = new ClaimsIdentity(claims, nameof(AuthService));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;
                }
            }
            catch (Exception)
            {
                // Manejar la excepción según sea necesario
            }

            return new AuthenticationState(user);
        }

        public async Task LogoutAsync()
        {
            const string Empty = "{}";
            var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("api/user/Logout", emptyContent);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            string[] defaultDetail = ["Un error desconocido impidió que el registro se realizara correctamente."];

            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/eccomerce/Account/register",
                   new { registerDTO.UserName, registerDTO.Email, registerDTO.Password });
                if (result.IsSuccessStatusCode)
                {
                    return new ApiResponse { Success = true };
                }
                var details = await result.Content.ReadAsStringAsync();
                var problemDetails = JsonDocument.Parse(details);

                var errors = new List<string>();
                var errorList = problemDetails.RootElement.GetProperty("errors");

                foreach (var errorEntry in errorList.EnumerateObject())
                {
                    if (errorEntry.Value.ValueKind == JsonValueKind.String)
                    {
                        errors.Add(errorEntry.Value.GetString()!);
                    }
                    else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                    {
                        errors.AddRange(
                            errorEntry.Value.EnumerateArray().Select(
                                e => e.GetString() ?? string.Empty)
                            .Where(e => !string.IsNullOrEmpty(e)));
                    }
                }
                return new ApiResponse
                {
                    Success = false,
                    Errors = problemDetails == null ? defaultDetail : [.. errors]
                };
            }
            catch (Exception)
            {
                return new ApiResponse
                {
                    Success = false,
                    Errors = [.. defaultDetail]
                };
            }
        }

        public async Task<ApiResponse> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync(
                    "login", new
                    {
                        loginDTO.Email,
                        loginDTO.Password
                    });

                if (result.IsSuccessStatusCode)
                {
                    var tokenResponse = await result.Content.ReadAsStringAsync();

                    var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(tokenResponse, jsonSerializerOptions);

                    await _localStorageService.SetItemAsync("accessToken", tokenInfo?.AccessToken);

                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new ApiResponse { Success = true };
                }
            }
            catch (Exception)
            {

            }

            return new ApiResponse
            {
                Success = false,
                Message = "Usuario inválido y/o contraseña."
            };
        }
    }
}
