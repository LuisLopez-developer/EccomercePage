namespace EccomercePage.Api.Interfaces
{
    public interface IGetRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
