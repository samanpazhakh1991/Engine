using Domain.Trades.Entities;
using Framework.Contracts.GenericRepositories;

namespace Domain.Contract.Trades.Repository.Query
{
    public interface ITradeQueryRepository : IQueryRepository<Trade, ITrade, long>
    {
        Task<long> GetMaxAsync();
    }
}