using Domain;
using Domain.Contract.StockMarkets.Repository.Query;
using Infrastructure.GenericServices;

namespace Infrastructure.StockMarkets.QueryRepository
{
    public class StockMarketQueryRepository :
        QueryRepository<StockMarketMatchEngine, IStockMarketMatchEngine, Guid>,
        IStockMarketQueryRepository
    {
        private readonly TradeMatchingEngineContext tradeMatchingEngineContext;

        public StockMarketQueryRepository(TradeMatchingEngineContext tradeMatchingEngineContext) : base(tradeMatchingEngineContext)
        {
            this.tradeMatchingEngineContext = tradeMatchingEngineContext;
        }
    }
}