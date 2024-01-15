using Domain.Contract.Trades.Repository.Command;
using Domain.Trades.Entities;
using Infrastructure.GenericServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Trades.CommandRepositories
{
    public class TradeCommandRepository : CommandRepository<Trade, ITrade, long>, ITradeCommandRepository
    {
        private readonly TradeMatchingEngineContext tradeMatchingEngineContext;

        public TradeCommandRepository(TradeMatchingEngineContext dbcontext) : base(dbcontext)
        {
        }
    }
}