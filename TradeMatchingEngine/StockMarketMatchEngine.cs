using System.Collections.Concurrent;
using Domain.Events;
using Domain.Orders.Entities;
using Domain.Trades.Entities;
using Framework.Contracts.Events;

namespace Domain
{
    public class StockMarketMatchEngine : IStockMarketMatchEngine
    {
        #region PrivateField

        private readonly PriorityQueue<Order, Order> sellOrderQueue, buyOrderQueue;
        private readonly Queue<Order> preOrderQueue;
        private List<Order> orders = new List<Order>();
        private long lastOrderId;
        private long lastTradeId;
        private bool isInitialized;
        private List<IDomainEvent> domainEvents;
        private readonly ConcurrentDictionary<long, IOrder> modifiedAmountOrders;

        #endregion PrivateField

        #region PublicProperties

        public StockMarketMatchEngine(
            Guid id,
            int sellOrderCount,
            int buyOrderCount,
            long version,
            Guid financialInstrumentId
            )
        {
            Id = id;
            SellOrderCount = sellOrderCount;
            BuyOrderCount = buyOrderCount;
            Version = version;
            FinancialInstrumentId = financialInstrumentId;
        }

        #endregion PublicProperties

        public StockMarketMatchEngine(
            Guid financialInstrumentId,
            Guid id,
            List<Order>? orders = null,
            long lastOrderId = 0,
            long lastTradeId = 0,
            int sellOrderCount = 0,
            int buyOrderCount = 0,
            long version = 1)
        {
            Version = 1;
            sellOrderQueue = new PriorityQueue<Order, Order>(new ModifiedOrderPriorityMin());
            buyOrderQueue = new PriorityQueue<Order, Order>(new ModifiedOrderPriorityMax());
            preOrderQueue = new Queue<Order>();

            this.lastOrderId = lastOrderId;
            this.lastTradeId = lastTradeId;

            SellOrderCount = sellOrderCount;
            BuyOrderCount = buyOrderCount;
            Version = version;
            Id = id;
            FinancialInstrumentId = financialInstrumentId;

            foreach (var order in orders ?? new List<Order>())
            {
                this.orders.Add(order);
                processOrder(order);
            }

            domainEvents = new List<IDomainEvent>();
            modifiedAmountOrders = new();
        }

        public int BuyOrderCount { get; private set; }

        public int SellOrderCount { get; private set; }

        public long Version { get; private set; }

        public Guid Id { get; }

        public Guid FinancialInstrumentId { get; private set; }

        public IEnumerable<IDomainEvent> DomainEvents => domainEvents;

        private Order createOrderRequest(
            int price,
            int amount,
            Side side,
            DateTime? expireTime,
            bool fillAndKill,
            Guid marketId,
            long? orderParentId = null,
            bool doesMatch = true,
            long orderId = 0
            )
        {
            var order = new Order(
                doesMatch == true ? setId() : orderId,
                side,
                price,
                amount,
                expireTime ?? DateTime.MaxValue,
                OrderStates.Register,
                marketId: marketId,
                true,
                isFillAndKill: fillAndKill,
                orderParentId: orderParentId);

            if (doesMatch)
            {
                orders.Add(order);
                AddDomainEvent(OrderEventBase.Create<OrderCreated>(order));
            }

            return order;
        }

        private long setId()
        {
            if (lastOrderId != 0) return Interlocked.Increment(ref lastOrderId);

            lastOrderId = 1;

            return lastOrderId;
        }

        protected ITrade CreateTrade(long buyOrderId, long sellOrderId, int amount, int price)
        {
            var trade = new Trade(Interlocked.Increment(
                ref lastTradeId),
                buyOrderId,
                sellOrderId,
                amount,
                price);

            return trade;
        }

        protected IStockMarketMatchingEngineProcessContext PreProcessOrder(int price,
            int amount,
            Side side,
            DateTime? expireTime = null,
            bool fillAndKill = false,
            long? orderParentId = null)
        {
            var preOrder = createOrderRequest(
                price,
                amount,
                side,
                expireTime,
                fillAndKill,
                Id,
                orderParentId,
                true);

            var processContext = new StockMarketMatchingEngineProcessContext();
            processContext.OrderCreated(preOrder);

            if (preOrder.Side == Side.Sell)
            {
                sellOrderQueue.Enqueue(preOrder, preOrder);
                SellOrderCount++;

                return processContext!;
            }
            preOrderQueue.Enqueue(preOrder);
            BuyOrderCount++;
            return processContext;
        }

        protected IStockMarketMatchingEngineProcessContext ProcessOrderAsync(int price,
            int amount,
            Side side,
            DateTime? expireTime = null,
            bool fillAndKill = false,
            long? orderParentId = null,
            bool doesMatch = true,
            long orderId = 0
            )
        {
            var order = createOrderRequest(
                price,
                amount,
                side,
                expireTime,
                fillAndKill,
                Id,
                orderParentId,
                doesMatch,
                orderId
                );

            if (doesMatch)
            {
                return processOrder(order);
            }

            var processContext = new StockMarketMatchingEngineProcessContext();
            processContext.OrderCreated(order);
            processContext.StockMarketUpdated(this);
            return processContext;
        }

        private IStockMarketMatchingEngineProcessContext processOrder(
            Order order)
        {
            PriorityQueue<Order, Order> ordersQueue, otherSideOrdersQueue;
            Func<bool> priceCheck;

            initiateTheQueueSideAndPriceCheck();

            while (order.Amount > 0 && otherSideOrdersQueue.Count > 0 && priceCheck())
            {
                var peekedOrder = otherSideOrdersQueue.Peek();

                if (removeFromQueueIfNotGood(peekedOrder)) continue;

                makeTrade(order, peekedOrder);

                if (!peekedOrder.HasCompleted) continue;

                otherSideOrdersQueue.Dequeue();
                orders.Remove(peekedOrder);

                if (order.Amount <= 0)
                {
                    orders.Remove(order);
                }
            }
            var isFillAndKill = order.IsFillAndKill;
            StockMarketMatchingEngineProcessContext processContext = new();
            if (order.Amount > 0 && !isFillAndKill)
            {
                ordersQueue.Enqueue(order, order);

                processContext = new StockMarketMatchingEngineProcessContext();
                processContext.OrderCreated(order);
                processContext.StockMarketUpdated(this);
                return processContext;
            }

            orders.Remove(order);

            processContext.OrderCreated(order);
            processContext.StockMarketUpdated(this);
            return processContext;

            void initiateTheQueueSideAndPriceCheck()
            {
                if (order.Side == Side.Sell)
                {
                    SellOrderCount++;
                    ordersQueue = sellOrderQueue;
                    otherSideOrdersQueue = buyOrderQueue;
                    priceCheck = () => order.Price <= otherSideOrdersQueue.Peek().Price;
                    return;
                }
                BuyOrderCount++;
                ordersQueue = buyOrderQueue;
                otherSideOrdersQueue = sellOrderQueue;
                priceCheck = () => order.Price >= otherSideOrdersQueue.Peek().Price;
            }

            bool removeFromQueueIfNotGood(Order peekedOrder)
            {
                if (peekedOrder.IsGood)
                    return false;

                orders.Remove(peekedOrder);
                otherSideOrdersQueue.Dequeue();
                return true;
            }
        }

        private void makeTrade(Order order, Order otherSideOrder)
        {
            var amount = otherSideOrder.Amount > order.Amount ? order.Amount : otherSideOrder.Amount;

            AddDomainEvent(OrderMatchedEventBase.Create<OrderMatched>(
                buyOrderId: order.Side == Side.Buy ? order.Id : otherSideOrder.Id,
                sellOrderId: order.Side == Side.Sell ? order.Id : otherSideOrder.Id,
                amount: amount,
                price: order.Side == Side.Sell ? order.Price : otherSideOrder.Price));

            var currentOrderAmount = order.Amount;
            order.DecreaseAmount(otherSideOrder.Amount);
            var currentOrderCloneResult = order.Clone((int)order.OriginalAmount);

            modifiedAmountOrders.AddOrUpdate(currentOrderCloneResult.Id, currentOrderCloneResult, (a, b) => b);

            AddDomainEvent(OrderEventBase.Create<OrderModified>(currentOrderCloneResult));

            otherSideOrder.DecreaseAmount(currentOrderAmount);

            var cloneResult = otherSideOrder.Clone((int)otherSideOrder.OriginalAmount);
            modifiedAmountOrders.AddOrUpdate(cloneResult.Id, cloneResult, (k, v) => v);
            AddDomainEvent(OrderEventBase.Create<OrderModified>(cloneResult));
        }

        protected IStockMarketMatchingEngineProcessContext CancelOrder(
            long orderId)
        {
            var processContext = new StockMarketMatchingEngineProcessContext();

            var findOrder = orders.SingleOrDefault(a => a.Id == orderId);

            if (findOrder == null) throw new Exception(message: "Order Has Not Been Defined");

            findOrder.SetStateCancelled();

            AddDomainEvent(OrderEventBase.Create<OrderCanceled>(findOrder));

            if (findOrder.Side == Side.Sell)
            {
                SellOrderCount--;
            }
            else
            {
                BuyOrderCount--;
            }

            processContext.OrderModified(findOrder);
            processContext.StockMarketUpdated(this);

            return processContext;
        }

        protected IStockMarketMatchingEngineProcessContext CancelOrder(IOrder order)
        {
            var processContext = new StockMarketMatchingEngineProcessContext();
            order.UpdateBy(orders.First(i => i.Id == order.Id));
            processContext.StockMarketUpdated(this);

            return processContext;
        }

        protected IStockMarketMatchingEngineProcessContext ModifyOrder(long orderId,
            int price,
            int amount,
            DateTime? expirationDate)
        {
            CancelOrder(orderId);

            var processContext = new StockMarketMatchingEngineProcessContext();
            var findOrder = orders.Single(o => o.Id == orderId);

            var order = ProcessOrderAsync(price,
                 amount,
                 findOrder.Side,
                 expirationDate,
                 findOrder.IsFillAndKill
                 );

            processContext.OrderModified(findOrder);
            processContext.OrderCreated((Order)order.Order);
            processContext.StockMarketUpdated(this);

            return processContext;
        }

        protected IStockMarketMatchingEngineProcessContext ModifyOrder(IOrder order, int price, int amount, DateTime? expireTime)
        {
            var processContext = new StockMarketMatchingEngineProcessContext();

            order.UpdateBy(amount, price, expireTime);

            processContext.StockMarketUpdated(this);
            return processContext;
        }

        protected IStockMarketMatchingEngineProcessContext PreModifyOrder(
            long orderId,
            int price,
            int amount,
            DateTime? expirationDate
            )
        {
            var processContext = new StockMarketMatchingEngineProcessContext();

            var cancelledOrder = CancelOrder(orderId);

            var orderSide = orders.Single(o => o.Id == orderId).Side;

            PreProcessOrder(
                price,
                amount,
                orderSide,
                expirationDate);

            processContext.StockMarketUpdated(this);
            return processContext;
        }

        public void AddDomainEvent(IDomainEvent @event)
        {
            Version++;
            @event.AggregateVersion = Version;
            domainEvents.Add(@event);
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }

        public void RemoveDomainEvent(IDomainEvent @event)
        {
            domainEvents.Remove(@event);
        }

        public void UpdateBy(IStockMarketMatchEngine stockMarketMatchEngine, bool clearOriginalEvents = true)
        {
            Version = stockMarketMatchEngine.Version;
            SellOrderCount = stockMarketMatchEngine.SellOrderCount;
            BuyOrderCount = stockMarketMatchEngine.BuyOrderCount;
            domainEvents = stockMarketMatchEngine.DomainEvents.ToList();
            if (clearOriginalEvents)
                stockMarketMatchEngine.ClearEvents();
        }
    }
}