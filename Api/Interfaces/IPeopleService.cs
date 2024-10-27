using EccomercePage.Api.Services;
using EccomercePage.Data.DTO.Profile;

namespace EccomercePage.Api.Interfaces
{
    public interface IPeopleService
    {
        Task<ApiResponse> AddPeople(AddPeopleDTO dto);
        Task<bool> IsUserLinkedToPerson(string userId);
    }
}
