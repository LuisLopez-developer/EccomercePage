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
        private readonly CartResumenState _cartResumenState;

        public CartRepository(IAuthService authService, ICartService cartService, CartState cartState, CartResumenState cartResumenState)
        {
            _authService = authService;
            _cartService = cartService;
            _cartState = cartState;
            _cartResumenState = cartResumenState;
        }

        public async Task<bool> ChangeItemQuantity(ChangeItemQuantityDTO dto)
        {
            bool isSuccess = await _cartService.ChangeItemQuantity(dto);

            if (isSuccess)
            {
                // Si la operación fue exitosa, recupera el carrito actualizado y configura el estado
                var userID = await _authService.GetUserIdAsync();
                if (!string.IsNullOrEmpty(userID))
                {
                    _cartResumenState.UpdateCartState(await _cartService.GetCartResumeAsync(userID));
                    _cartState.SetCartItems(await _cartService.GetTotalProductInCartAsync(userID));
                }
            }

            return isSuccess;
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

        public async Task UpdateCartResumeStateAsycn()
        {
            var userID = await _authService.GetUserIdAsync();
            if (string.IsNullOrEmpty(userID))
            {
                return;
            }

            var cartResume = await _cartService.GetCartResumeAsync(userID);

            if (cartResume == null)
            {
                return;
            }

            _cartResumenState.UpdateCartState(cartResume);
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
