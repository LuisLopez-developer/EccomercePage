using EccomercePage.Api.Models;

namespace EccomercePage.Api.Interfaces.ProductInterfaces
{
    public interface IProductCategory
    {
        Task<List<BasicFilterModel>> GetAllAsync();
    }
}
