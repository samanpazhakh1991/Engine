using Domain.Orders.Entities;
using Domain.Trades.Entities;

namespace Domain
{
    public class StockMarketMatchingEngineProcessContext : IStockMarketMatchingEngineProcessContext
    {
        #region Private

        private Order createdOrder;
        private readonly List<Order> modifiedOrders;
        private readonly List<Trade> createdTrades;
        private IStockMarketMatchEngine stockMarket;

        #endregion Private

        public StockMarketMatchingEngineProcessContext()
        {
            modifiedOrders = new List<Order>();
            createdTrades = new List<Trade>();
        }

        #region Public

        public IOrder Order => createdOrder;
        public IEnumerable<IOrder> ModifiedOrders => modifiedOrders;
        public IEnumerable<ITrade> CreatedTrades => createdTrades;

        public IStockMarketMatchEngine StockMarketMatchEngine => stockMarket;

        public bool IsStockMarcketChanged => stockMarket.DomainEvents.Any();

        #endregion Public

        #region Internal

        internal void OrderCreated(Order order)
        {
            createdOrder = order;
        }

        internal void OrderModified(Order order)
        {
            modifiedOrders.Add(order);
        }

        internal void TradeCreated(Trade trade)
        {
            createdTrades.Add(trade);
        }

        internal void StockMarketUpdated(StockMarketMatchEngine stockMarket)
        {
            this.stockMarket = stockMarket;
        }

        #endregion Internal
    }
}