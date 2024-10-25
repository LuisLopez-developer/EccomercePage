using EccomercePage.Api.Models;
using EccomercePage.Api.Models.ProductModel;

namespace EccomercePage.Api.Interfaces.ProductInterfaces
{
    public interface IProduct
    {
        // Metodos, enfocados para el eccomerce
        Task<List<ProductCatalogViewModel>> GetMostValuableProductCatalog();
        Task<PagedResultModel<ProductCatalogViewModel>> GetProductCatalogWithFiltersAsync(int page, int pageSize, string searchTerm,
            int? brandId, int? categoryId, string? model, decimal? minimumPrice, decimal? maximunPrice);
    }
}
