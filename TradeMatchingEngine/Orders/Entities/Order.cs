namespace Domain.Orders.Entities
{
    public class Order : IOrder
    {
        private OrderStates state;

        internal Order(long id,
            Side side,
            int price,
            int amount,
            DateTime expireTime,
            OrderStates? orderState,
            Guid marketId,
            int? originalAmount = null,
            bool isFillAndKill = false,
            long? orderParentId = null) :
            this(id,
                side,
                price,
                amount,
                expireTime,
                orderState,
                marketId,
                false,
                false,
                originalAmount,
                isFillAndKill,
                orderParentId
                )
        {
        }

        public Order(long id,
                    Side side,
                    int price,
                    int amount,
                    DateTime expireTime,
                    OrderStates? orderState,
                    Guid marketId,
                    bool isNewOrder = false,
                    bool isMainOrder = false,
                    int? originalAmount = null,
                    bool isFillAndKill = false,
                    long? orderParentId = null
            )
        {
            Id = id;
            Side = side;
            Price = price;
            Amount = amount;
            OriginalAmount = originalAmount ?? amount;
            IsFillAndKill = isFillAndKill;
            ExpireTime = expireTime;
            state = orderState ?? OrderStates.Register;
            OrderParentId = orderParentId;
            MarketId = marketId;
        }

        public OrderStates? OrderState
        {
            get => state;
            private set => value = state;
        }

        public long Id { get; }

        public Guid MarketId { get; }

        public Side Side { get; private set; }

        public int Price { get; private set; }

        public int? OriginalAmount { get; private set; }

        public int Amount { get; private set; }

        public bool IsFillAndKill { get; private set; }

        public bool HasCompleted => Amount <= 0;

        public DateTime ExpireTime { get; private set; }

        public bool IsExpired => ExpireTime < DateTime.Now;

        public OrderStates GetOrderState() => state;

        public long? OrderParentId { get; private set; }

        public bool IsGood => !IsExpired &&
                        ExpireTime >= DateTime.Now &&
                        GetOrderState() != OrderStates.Cancel;

        public int DecreaseAmount(int amount)
        {
            Amount -= amount;

            if (Amount <= 0)
            {
                Amount = 0;
            }

            return Amount;
        }

        public void SetStateCancelled()
        {
            state = OrderStates.Cancel;
            OrderState = state;
        }

        public void SetStateRegistered()
        {
            state = OrderStates.Register;
        }

        public void SetStateModified()
        {
            state = OrderStates.Modified;
        }

        public void UpdateBy(IOrder order)
        {
            Price = order.Price;
            OriginalAmount = order.OriginalAmount;
            Amount = order.Amount;
            ExpireTime = order.ExpireTime;
            IsFillAndKill = order.IsFillAndKill;
            Side = order.Side;
            state = (OrderStates)order.OrderState;
            OrderState = state;
        }

        public void UpdateBy(int amount, int price, DateTime? expireTime)
        {
            Price = price;
            OriginalAmount = Amount;
            Amount = amount;
            ExpireTime = expireTime ?? DateTime.MaxValue;
        }

        internal Order Clone(int originalAmount)
        {
            return new Order(
                Id,
                Side,
                Price,
                Amount,
                ExpireTime,
                OrderStates.Modified,
                MarketId,
                false,
                false,
                originalAmount,
                IsFillAndKill,
                OrderParentId);
        }
    }
}