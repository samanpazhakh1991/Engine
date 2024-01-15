using Domain.Orders.Entities;
using Domain.Trades.Entities;
using Framework.Contracts;
using System.Diagnostics;

namespace Domain
{
    public class StockMarketBlockingQueueDecorator : StockMarketMatchEngine
    {
        private readonly BlockingQueue queue;

        public StockMarketBlockingQueueDecorator(
            Guid financialInstrumentId,
            Guid id,
            List<Order>? orders = null,
            long lastOrderId = 0,
            long lastTradeId = 0,
            int sellOrderCount = 0,
            int buyOrderCount = 0,
            long version = 1) : base(financialInstrumentId,
                id,
                orders,
                lastOrderId,
                lastTradeId,
                sellOrderCount,
                buyOrderCount,
                version)
        {
            queue = new BlockingQueue();
        }

        private async Task<T?> executeAsync<T>(Func<Task<T>> function)
        {
            return await queue.ExecuteAsync(async () => await function().ConfigureAwait(false)).ConfigureAwait(false);
        }

        public virtual async Task<IStockMarketMatchingEngineProcessContext> CancelOrder(long orderId)
        {
            return await executeAsync(() => Task.FromResult(base.CancelOrder(orderId))).ConfigureAwait(false);
        }

        public virtual async Task<IStockMarketMatchingEngineProcessContext> CancelOrder(IOrder order)
        {
            return await executeAsync(() => Task.FromResult(base.CancelOrder(order))).ConfigureAwait(false);
        }

        public virtual async Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price,
            int amount, DateTime? expirationDate)
        {
            return await executeAsync(() => Task.FromResult(base.ModifyOrder(orderId, price, amount, expirationDate))).ConfigureAwait(false);
        }

        public virtual async Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(IOrder order, int price, int amount, DateTime? expireTime)
        {
            return await executeAsync(() => Task.FromResult(base.ModifyOrder(order, price, amount, expireTime))).ConfigureAwait(false);
        }

        public virtual async Task<IStockMarketMatchingEngineProcessContext> PreModifyOrder(long orderId, int price,
            int amount, DateTime? expirationDate)
        {
            return await executeAsync(() => Task.FromResult(base.PreModifyOrder(orderId, price, amount, expirationDate))).ConfigureAwait(false);
        }

        public virtual async Task<ITrade> CreateTrade(long buyOrderId, long sellOrderId, int amount, int price) => await executeAsync(() => Task.FromResult(base.CreateTrade(buyOrderId, sellOrderId, amount, price))).ConfigureAwait(false);

        public virtual async Task<IStockMarketMatchingEngineProcessContext> ProcessOrderAsync(int price, int amount,
            Side side, DateTime? expireTime = null, bool fillAndKill = false, long? orderParentId = null, bool doesMatch = true, long orderId = 0)
        {
            return await executeAsync(() => Task.FromResult(base.ProcessOrderAsync(price, amount, side, expireTime, fillAndKill, orderParentId, doesMatch, orderId))).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await queue.DisposeAsync().ConfigureAwait(false);
        }
    }
}