using Domain.Orders.Entities;
using Domain.Trades.Entities;

namespace Domain
{
    public interface IStockMarketMatchingEngineProcessContext
    {
        IEnumerable<ITrade> CreatedTrades { get; }
        IEnumerable<IOrder> ModifiedOrders { get; }
        IOrder? Order { get; }
        IStockMarketMatchEngine StockMarketMatchEngine { get; }
        bool IsStockMarcketChanged { get; }
    }
}