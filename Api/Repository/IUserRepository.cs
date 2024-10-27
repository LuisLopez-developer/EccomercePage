namespace EccomercePage.Api.Repository
{
    public interface IUserRepository
    {
        Task<bool> IsUserLinkedToPerson();
    }
}
