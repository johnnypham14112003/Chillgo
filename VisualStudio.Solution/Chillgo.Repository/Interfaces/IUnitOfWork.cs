namespace Chillgo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IAccountRepository GetAccountRepository();
    }
}
