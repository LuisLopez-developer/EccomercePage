using EccomercePage.Models.ProductModel;
using EccomercePage.Models;
using EccomerceApi.Model;

namespace EccomercePage.Interfaces.ProductInterfaces
{
    public interface IProduct
    {
        // Metodos, enfocados para el eccomerce
        Task<List<ProductCatalogViewModel>> GetMostValuableProductCatalog();
        Task<PagedResultModel<ProductCatalogViewModel>> GetProductCatalogWithFiltersAsync(int page, int pageSize, string searchTerm,
            int? brandId, int? categoryId, string? model, decimal? minimumPrice, decimal? maximunPrice);
    }
}
