using EccomercePage.Api.Interfaces;
using EccomercePage.Api.Repository.States;
using EccomercePage.Data.DTO.CartDTO;

namespace EccomercePage.Api.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IAuthService _authService;
        private readonly ICartService _cartService;
        private readonly CartState _cartState;

        public CartRepository(IAuthService authService, ICartService cartService, CartState cartState)
        {
            _authService = authService;
            _cartService = cartService;
            _cartState = cartState;
        }

        public async Task<CartResumeDTO> GetCartResumeAsync()
        {
            var userID = await _authService.GetUserIdAsync();
            if (string.IsNullOrEmpty(userID))
            {
                return new CartResumeDTO();
            }

            return await _cartService.GetCartResumeAsync(userID);
        }

        public async Task UpdateCartStateAsync()
        {
            var userID = await _authService.GetUserIdAsync();

            if (!string.IsNullOrEmpty(userID))
            {
                var totalItems = await _cartService.GetTotalProductInCartAsync(userID);
                _cartState.SetCartItems(totalItems);
            }
        }
    }
}
