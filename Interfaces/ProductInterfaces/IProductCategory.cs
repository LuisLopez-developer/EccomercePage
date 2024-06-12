using EccomercePage.Models;

namespace EccomercePage.Interfaces.ProductInterfaces
{
    public interface IProductCategory
    {
        Task<List<BasicFilterModel>> GetAllAsync();
    }
}
