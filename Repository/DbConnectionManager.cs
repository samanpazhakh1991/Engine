using Framework.Contracts;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Infrastructure
{
    public class DbConnectionManager : IDbConnectionService, ITransactionService
    {
        public Guid Id { get; } = Guid.NewGuid();
        private readonly SqlConnection connection;
        internal DbTransaction Transaction;

        public DbConnectionManager(string connString)
        {
            connection = new SqlConnection(connString);
        }

        public async Task<ITransactionService> BeginTransactionAsync()
        {
            if (Transaction != null) return this;

            using var lockObj = new SemaphoreSlim(1);
            await lockObj.WaitAsync().ConfigureAwait(false);
            if (Transaction != null) return this;
            Transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);
            return this;
        }

        public async Task CommitAsync()
        {
            await Transaction.CommitAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            if (Transaction != null)
                await Transaction.DisposeAsync().ConfigureAwait(false);

            await connection.CloseAsync().ConfigureAwait(false);

            await connection.DisposeAsync().ConfigureAwait(false);
        }

        public async Task<DbConnection> GetConnectionAsync()
        {
            if (connection.State == ConnectionState.Open) return connection;

            using var lockObj = new SemaphoreSlim(1);
            await lockObj.WaitAsync().ConfigureAwait(false);

            if (connection.State == ConnectionState.Open) return connection;
            await connection.OpenAsync().ConfigureAwait(false);
            return connection;
        }
    }
}