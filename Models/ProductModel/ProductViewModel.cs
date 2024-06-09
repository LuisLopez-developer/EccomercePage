namespace EccomercePage.Models.ProductModel
{
    public class ProductViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
    }
}
