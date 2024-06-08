using EccomercePage.Models;

namespace EccomercePage.Interfaces
{
    public interface IAccountManagement
    {
        public Task<FormResultModel> RegisterAsync(string email, string password);
        public Task<FormResultModel> LoginAsync(string email, string password);
        public Task LogoutAsync();
        public Task<bool> CheckAuthenticatedAsync();

    }
}
