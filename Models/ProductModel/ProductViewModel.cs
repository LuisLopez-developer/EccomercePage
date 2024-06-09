namespace EccomercePage.Models.ProductModel
{
    public class ProductViewModel
    {
        public required string ImageSrc { get; set; }
        public required string ImageAlt { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}
