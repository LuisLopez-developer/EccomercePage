namespace EccomercePage.Api.Interfaces
{
    public interface IRepository<T, TI, TU>
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> InsertAsync(TI insertDTO);
        Task<T> UpdateAsync(int id, TU updateDTO);
        Task<T> DeleteAsync(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);
    }
}
