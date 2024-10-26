namespace EccomercePage.Data.DTO.CartDTO
{
    public class UpdateCartDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
    }
}
