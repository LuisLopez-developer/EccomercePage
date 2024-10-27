
using EccomercePage.Api.Interfaces;

namespace EccomercePage.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IPeopleService _peopleServce;
        private readonly IAuthService _authService;

        public UserRepository(IPeopleService peopleService, IAuthService authService)
        {
            _peopleServce = peopleService;
            _authService = authService;
        }

        public async Task<bool> IsUserLinkedToPerson()
        {
            var userId = await _authService.GetUserIdAsync();

            return await _peopleServce.IsUserLinkedToPerson(userId);

        }
    }
}
