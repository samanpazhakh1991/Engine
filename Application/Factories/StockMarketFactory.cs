using Domain;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Query;
using Domain.Orders.Entities;
using Framework.Contracts.Common;

namespace Application.Factories
{
    public class StockMarketFactory : IStockMarketFactory
    {
        private readonly SemaphoreSlim locker = new(int.MaxValue);
        private StockMarketMatchEngineStateProxy stockMarketMatchEngine;
        private Dictionary<Guid, StockMarketMatchEngineStateProxy> stockMarketMatchEngineCache;

        public StockMarketFactory()
        {
            stockMarketMatchEngineCache = new Dictionary<Guid, StockMarketMatchEngineStateProxy>();
        }

        public virtual async Task<IStockMarketMatchEngineWithState> GetStockMarket(
            IOrderQueryRepository orderQueryRep,
            ITradeQueryRepository tradeQueryRep,
            IStockMarketQueryRepository stockMarketQueryRepository)
        {
            //TODO:Optimize
            if (stockMarketMatchEngineCache.ContainsKey(SeedData.FinancialInstrumentStockMarketId))
            {
                return stockMarketMatchEngineCache.Values.First();
            }

            await locker.WaitAsync();
            try
            {
                if (stockMarketMatchEngineCache.ContainsKey(SeedData.FinancialInstrumentStockMarketId))
                {
                    return stockMarketMatchEngineCache.Values.First();
                }

                var getOrders = await orderQueryRep.GetAll(x => x.Amount != 0 && x.OrderState != OrderStates.Cancel);

                var lastOrderId = await orderQueryRep.GetMaxAsync();

                var getLastTrade = await tradeQueryRep.GetMaxAsync();

                var getStockMarket = await stockMarketQueryRepository
                                          .Get(s => s.Id == SeedData.FinancialInstrumentStockMarketId);

                stockMarketMatchEngine = new StockMarketMatchEngineStateProxy(
                                            getStockMarket.FinancialInstrumentId,
                                            getStockMarket.Id,
                                            getOrders.Select(x => (Order)x).ToList(),
                                            lastOrderId,
                                            getLastTrade,
                                            version: getStockMarket.Version);

                stockMarketMatchEngineCache.Add(getStockMarket.Id, stockMarketMatchEngine);
                stockMarketMatchEngineCache.Add(getStockMarket.FinancialInstrumentId, stockMarketMatchEngine);

                stockMarketMatchEngine.PreOpen();
                stockMarketMatchEngine.Open();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                locker.Release();
            }

            return stockMarketMatchEngine;
        }
    }
}