namespace EccomercePage.Api.Interfaces
{
    public interface IGetService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
