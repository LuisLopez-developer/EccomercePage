namespace EccomercePage.Api.Repository.States
{
    public class CartState
    {
        public event Action OnChange;

        private int _cartItems;
        public int CartItems
        {
            get => _cartItems;
            private set
            {
                _cartItems = value;
                NotifyStateChanged();
            }
        }

        public void SetCartItems(int count)
        {
            CartItems = count;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
