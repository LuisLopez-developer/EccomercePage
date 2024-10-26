using EccomercePage.Api.Services;
using EccomercePage.Data.DTO;

namespace EccomercePage.Api.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterDTO registerDTO);
        Task<ApiResponse> LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync();
        Task<bool> CheckAuthenticatedAsync();
        Task<string> GetUserIdAsync();
    }
}
