using EccomercePage.Data.DTO.CartDTO;

namespace EccomercePage.Api.Repository
{
    public interface ICartRepository
    {
        Task UpdateCartStateAsync();
        Task<CartResumeDTO> GetCartResumeAsync();
    }
}
