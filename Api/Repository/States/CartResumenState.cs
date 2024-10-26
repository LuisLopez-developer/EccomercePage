using EccomercePage.Data.DTO.CartDTO;

namespace EccomercePage.Api.Repository.States
{
    public class CartResumenState
    {
        public event Action OnChange;
        public CartResumeDTO CartResume { get; private set; } = new CartResumeDTO();
        public int CartTotalProducts => CartResume.CartItems.Sum(x => x.Quantity);
        

        // Método para actualizar el estado del carrito y el total
        public void UpdateCartState(CartResumeDTO cartResumeDTO)
        {
            CartResume = cartResumeDTO;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
