namespace EccomercePage.Data.DTO.CartDTO
{
    public class CartItemsPaymentDTO
    {
        public int Id { get; set; }
        public List<CartItemPaymentDTO> CartItems { get; set; }
    }

    public class CartItemPaymentDTO
    {
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
