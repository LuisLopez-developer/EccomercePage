using EccomercePage.Api.Interfaces;
using EccomercePage.Api.Repository.States;

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
