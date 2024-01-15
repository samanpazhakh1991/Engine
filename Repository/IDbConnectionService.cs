using System.Data.Common;

namespace Infrastructure
{
    public interface IDbConnectionService : IAsyncDisposable, IDisposable
    {
        Task<DbConnection> GetConnectionAsync();
    }
}
