using EccomercePage.Data.DTO.CartDTO;

namespace EccomercePage.Api.Repository
{
    public interface ICartRepository
    {
        Task UpdateCartStateAsync();
        Task UpdateCartResumeStateAsycn();
        Task<CartResumeDTO> GetCartResumeAsync();
        Task<bool> ChangeItemQuantity(ChangeItemQuantityDTO dto);
        Task<int> GetTotalAmountAsync();
    }
}
