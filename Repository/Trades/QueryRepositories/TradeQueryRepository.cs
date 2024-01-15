using Domain.Contract.Trades.Repository.Query;
using Domain.Trades.Entities;
using Infrastructure.GenericServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Trades.QueryRepositories
{
    public class TradeQueryRepository : QueryRepository<Trade, ITrade, long>, ITradeQueryRepository
    {
        private readonly TradeMatchingEngineContext tradeMatchingEngineContext;

        public TradeQueryRepository(TradeMatchingEngineContext dbcontext) : base(dbcontext)
        {
            this.tradeMatchingEngineContext = dbcontext;
        }

        public async Task<long> GetMaxAsync()
        {
            return await tradeMatchingEngineContext.Trades.MaxAsync(t => (long?)t.Id).ConfigureAwait(false) ?? 0;
        }
    }
}