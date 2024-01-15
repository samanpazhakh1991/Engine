using Domain;
using Domain.Contract.StockMarkets.Repository.Command;
using Infrastructure.GenericServices;

namespace Infrastructure.StockMarkets.CommandRepositories
{
    public class StockMarketCommandRepository :
        CommandRepository<StockMarketMatchEngine, IStockMarketMatchEngine, Guid>, IStockMarketCommandRepository
    {
        private readonly TradeMatchingEngineContext _tradeMatchingEngineContext;

        public StockMarketCommandRepository(TradeMatchingEngineContext dbcontext) : base(dbcontext)
        {
            _tradeMatchingEngineContext = dbcontext;
        }
    }
}