using EccomercePage.Data.DTO.CartDTO;

namespace EccomercePage.Api.Interfaces
{
    public interface ICartService : IRepository<CartReponseDTO, AddProductCartDTO, UpdateCartDTO>
    {
        Task<int> GetTotalProductInCartAsync(string userId);
        Task<CartResumeDTO> GetCartResumeAsync(string userId);
        Task<bool> ChangeItemQuantity(ChangeItemQuantityDTO dto);
    }
}
