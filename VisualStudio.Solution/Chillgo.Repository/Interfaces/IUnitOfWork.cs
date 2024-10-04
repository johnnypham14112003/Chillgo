namespace Chillgo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IAccountRepository GetAccountRepository();

        IConversationRepository ConversationRepository { get; }

        IMessageRepository MessageRepository { get; }
    }
}
