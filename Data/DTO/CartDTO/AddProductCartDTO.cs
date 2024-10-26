namespace EccomercePage.Data.DTO.CartDTO
{
    public class AddProductCartDTO
    {
        public string UserId { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }
}
