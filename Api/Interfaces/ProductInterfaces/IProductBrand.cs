using EccomercePage.Api.Models;

namespace EccomercePage.Api.Interfaces.ProductInterfaces
{
    public interface IProductBrand
    {
        Task<List<BasicFilterModel>> GetAllAsync();
    }
}
