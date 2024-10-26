namespace EccomercePage.Data.DTO.CartDTO
{
    public class CartResumeDTO
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<CartItems> CartItems { get; set; } = [];
    }

    public class CartItems
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int ExistentQuantity { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
