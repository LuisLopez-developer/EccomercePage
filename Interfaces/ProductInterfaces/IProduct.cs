using EccomercePage.Models.ProductModel;

namespace EccomercePage.Interfaces.ProductInterfaces
{
    public interface IProduct
    {
        // Metodos, enfocados para el eccomerce
        Task<List<ProductCatalogViewModel>> GetMostValuableProductCatalog(); 
    }
}
