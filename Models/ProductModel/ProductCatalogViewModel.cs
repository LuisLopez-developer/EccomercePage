namespace EccomercePage.Models.ProductModel
{
    public class ProductCatalogViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
    }
}
