using Domain.Orders.Entities;
using Domain.Trades.Entities;

namespace Domain
{
    public interface IStockMarketMatchEngineProxy : IAsyncDisposable
    {
        Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price, int amount,
            DateTime? expirationDate, bool doseMatch, Func<long, Task<IOrder>> findOrder);

        Task<IStockMarketMatchingEngineProcessContext> CancelOrder(long orderId, bool doseMatch, Func<long, Task<IOrder>> findOrder);

        Task<IStockMarketMatchingEngineProcessContext> ProcessOrderAsync(
            int price,
            int amount,
            Side side,
            DateTime? expireTime = null,
            bool fillAndKill = false,
            long? orderParentId = null,
            bool doesMath = true,
            long orderId = 0
            );

        Task<ITrade> CreateTrade(long buyOrderId, long sellOrderId, int amount, int price);
    }
}