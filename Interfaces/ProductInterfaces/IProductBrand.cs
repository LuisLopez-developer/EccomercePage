using EccomercePage.Models;

namespace EccomercePage.Interfaces.ProductInterfaces
{
    public interface IProductBrand
    {
        Task<List<BasicFilterModel>> GetAllAsync();
    }
}
