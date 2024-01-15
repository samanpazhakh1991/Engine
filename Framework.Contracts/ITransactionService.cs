namespace Framework.Contracts
{
    public interface ITransactionService : IAsyncDisposable, IDisposable
    {
        Task<ITransactionService> BeginTransactionAsync();
        Task CommitAsync();
    }
}