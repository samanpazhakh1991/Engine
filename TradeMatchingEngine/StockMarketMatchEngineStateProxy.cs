using Domain.Orders.Entities;
using Domain.Trades.Entities;
using System.Diagnostics;

namespace Domain
{
    public class StockMarketMatchEngineStateProxy :
        StockMarketBlockingQueueDecorator,
        IStockMarketMatchEngineWithState
    {
        private StockMarketState state;

        public StockMarketMatchEngineStateProxy(
            Guid financialInstrumentId,
            Guid id,
            List<Order>? orders = null,
            long lastOrderId = 0,
            long lastTradeId = 0,
            int sellOrderCount = 0,
            int buyOrderCount = 0,
            long version = 1
            ) : base(financialInstrumentId,
                id,
                orders,
                lastOrderId,
                lastTradeId,
                sellOrderCount,
                buyOrderCount,
                version)
        {
            state = new Closed(this);
        }

        public MarketState State => state.Code;

        public override Task<IStockMarketMatchingEngineProcessContext> ProcessOrderAsync(int price, int amount,
            Side side, DateTime? expireTime = null, bool fillAndKill = false, long? orderParentId = null, bool doesMatch = true, long orderId = 0) => state.ProcessOrderAsync(price, amount, side, expireTime, fillAndKill, orderParentId, doesMatch, orderId);

        public Task<IStockMarketMatchingEngineProcessContext> CancelOrder(
            long orderId,
            bool doseMatch,
            Func<long, Task<IOrder>> findOrder)
        {
            return state.CancelOrder(orderId, doseMatch, findOrder);
        }

        public Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price, int amount, DateTime? expirationDate, bool doseMatch, Func<long, Task<IOrder>> findOrder)
        {
            return state.ModifyOrder(orderId, price, amount, expirationDate, doseMatch, findOrder);
        }

        public override Task<ITrade> CreateTrade(long buyOrderId, long sellOrderId, int amount, int price)
        {
            return state.CreateTrade(buyOrderId, sellOrderId, amount, price);
        }

        public void Close()
        {
            state.Close();
        }

        public void Open()
        {
            state.Open();
        }

        public void PreOpen()
        {
            state.PreOpen();
        }

        private Task<IStockMarketMatchingEngineProcessContext> processOrderAsync(int price, int amount, Side side, DateTime? expireTime = null, bool fillAndKill = false, long? orderParentId = null, bool doesMatch = true, long orderId = 0)
        {
            return base.ProcessOrderAsync(price, amount, side, expireTime, fillAndKill, orderParentId, doesMatch, orderId);
        }

        private Task<IStockMarketMatchingEngineProcessContext> cancelOrder(long orderId)
        {
            return base.CancelOrder(orderId);
        }

        private Task<IStockMarketMatchingEngineProcessContext> cancelOrder(IOrder order)
        {
            return base.CancelOrder(order);
        }

        private Task<IStockMarketMatchingEngineProcessContext> modifyOrder(IOrder order, int price, int amount, DateTime? expireTime)
        {
            return base.ModifyOrder(order, price, amount, expireTime);
        }

        private Task<IStockMarketMatchingEngineProcessContext> modifyOrder(long orderId, int price, int amount, DateTime? expirationDate)
        {
            return base.ModifyOrder(orderId, price, amount, expirationDate);
        }

        private Task<IStockMarketMatchingEngineProcessContext> preModifyOrder(long orderId, int price, int amount, DateTime? expirationDate)
        {
            return base.PreModifyOrder(orderId, price, amount, expirationDate);
        }

        private Task<ITrade> createTrade(long buyOrderId, long sellOrderId, int amount, int price)
        {
            return base.CreateTrade(buyOrderId, sellOrderId, amount, price);
        }

        private class StockMarketState : IStockMarketMatchEngineProxy
        {
            public MarketState Code { get; protected init; }

            protected readonly StockMarketMatchEngineStateProxy StockMarketMatchEngineProxy;

            protected StockMarketState(StockMarketMatchEngineStateProxy stockMarketMatchEngineProxy)
            {
                StockMarketMatchEngineProxy = stockMarketMatchEngineProxy;
            }

            public virtual void PreOpen()
            {
                throw new NotImplementedException();
            }

            public virtual void Open()
            {
                throw new NotImplementedException();
            }

            public virtual void Close()
            {
                throw new NotImplementedException();
            }

            public virtual Task<IStockMarketMatchingEngineProcessContext> ProcessOrderAsync(int price,
                int amount, Side side, DateTime? expireTime = null, bool fillAndKill = false,
                long? orderParentId = null, bool doesMatch = true, long orderId = 0)
            {
                throw new NotImplementedException();
            }

            protected virtual Task<IStockMarketMatchingEngineProcessContext> CancelOrder(long orderId)
            {
                throw new NotImplementedException();
            }

            protected virtual Task<IStockMarketMatchingEngineProcessContext> CancelOrder(IOrder order)
            {
                throw new NotImplementedException();
            }

            public async Task<IStockMarketMatchingEngineProcessContext> CancelOrder(
                long orderId,
                bool doseMatch,
                Func<long, Task<IOrder>> findOrder)
            {
                if (!doseMatch)
                {
                    var order = await findOrder(orderId);
                    return await CancelOrder(order);
                }

                return await CancelOrder(orderId);
            }

            protected virtual Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price,
                int amount, DateTime? expirationDate)
            {
                throw new NotImplementedException();
            }

            protected virtual Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(IOrder order, int price, int amount, DateTime? expireTime)
            {
                throw new NotImplementedException();
            }

            public async Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price, int amount, DateTime? expirationDate, bool doseMatch, Func<long, Task<IOrder>> findOrder)
            {
                if (!doseMatch)
                {
                    var order = await findOrder(orderId);
                    return await ModifyOrder(order, price, amount, expirationDate);
                }
                return await ModifyOrder(orderId, price, amount, expirationDate);
            }

            async ValueTask IAsyncDisposable.DisposeAsync()
            {
                await StockMarketMatchEngineProxy.DisposeAsync();
            }

            public virtual Task<ITrade> CreateTrade(long buyOrderId, long sellOrderId, int amount, int price)
            {
                throw new NotImplementedException();
            }
        }

        private class Closed : StockMarketState
        {
            public Closed(StockMarketMatchEngineStateProxy stockMarketMatchEngine) : base(stockMarketMatchEngine)
            {
                Code = MarketState.Close;
            }

            public override void PreOpen()
            {
                StockMarketMatchEngineProxy.state = new PreOpened(StockMarketMatchEngineProxy);
            }
        }

        private class Opened : StockMarketState
        {
            public Opened(StockMarketMatchEngineStateProxy stockMarketMatchEngineProxy) : base(stockMarketMatchEngineProxy)
            {
                Code = MarketState.Open;
            }

            public override void PreOpen()
            {
                StockMarketMatchEngineProxy.state = new PreOpened(StockMarketMatchEngineProxy);
            }

            public override async Task<IStockMarketMatchingEngineProcessContext> ProcessOrderAsync(int price,
                int amount, Side side, DateTime? expireTime = null, bool fillAndKill = false,
                long? orderParentId = null, bool doesMatch = true, long orderId = 0)

            {
                return await StockMarketMatchEngineProxy.processOrderAsync(price,
                    amount,
                    side, expireTime, fillAndKill, orderParentId, doesMatch, orderId).ConfigureAwait(false);
            }

            public override async Task<ITrade> CreateTrade(long buyOrderId, long sellOrderId, int amount, int price)
            {
                return await StockMarketMatchEngineProxy.createTrade(buyOrderId, sellOrderId, amount, price);
            }

            protected override async Task<IStockMarketMatchingEngineProcessContext> CancelOrder(long orderId)
            {
                return await StockMarketMatchEngineProxy.cancelOrder(orderId).ConfigureAwait(false);
            }

            protected override Task<IStockMarketMatchingEngineProcessContext> CancelOrder(IOrder order)
            {
                return StockMarketMatchEngineProxy.cancelOrder(order);
            }

            protected override async Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price,
                int amount, DateTime? expirationDate)
            {
                return await StockMarketMatchEngineProxy.modifyOrder(orderId, price, amount, expirationDate).ConfigureAwait(false);
            }

            protected override Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(IOrder order, int price, int amount, DateTime? expireTime)
            {
                return StockMarketMatchEngineProxy.modifyOrder(order, price, amount, expireTime);
            }
        }

        private class PreOpened : StockMarketState
        {
            public PreOpened(StockMarketMatchEngineStateProxy stockMarketMatchEngineProxy) : base(stockMarketMatchEngineProxy)
            {
                Code = MarketState.PreOpen;
            }

            public override void Close()
            {
                StockMarketMatchEngineProxy.state = new Closed(StockMarketMatchEngineProxy);
            }

            public override void Open()
            {
                StockMarketMatchEngineProxy.state = new Opened(StockMarketMatchEngineProxy);
            }

            protected override Task<IStockMarketMatchingEngineProcessContext> CancelOrder(IOrder order)
            {
                return StockMarketMatchEngineProxy.cancelOrder(order);
            }

            protected override async Task<IStockMarketMatchingEngineProcessContext> CancelOrder(long orderId)
            {
                return await StockMarketMatchEngineProxy.CancelOrder(orderId).ConfigureAwait(false);
            }

            public override Task<IStockMarketMatchingEngineProcessContext> ProcessOrderAsync(int price, int amount,
                Side side, DateTime? expireTime = null, bool fillAndKill = false, long? orderParentId = null, bool doesMatch = true, long id = 0)
            {
                var result = StockMarketMatchEngineProxy.PreProcessOrder(price, amount, side, expireTime, fillAndKill);
                return Task.FromResult(result);
            }

            protected override async Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(long orderId, int price,
                int amount, DateTime? expirationDate)
            {
                return await StockMarketMatchEngineProxy.preModifyOrder(orderId, price, amount, expirationDate).ConfigureAwait(false);
            }

            protected override Task<IStockMarketMatchingEngineProcessContext> ModifyOrder(IOrder order, int price, int amount, DateTime? expireTime)
            {
                return StockMarketMatchEngineProxy.modifyOrder(order, price, amount, expireTime);
            }
        }
    }
}