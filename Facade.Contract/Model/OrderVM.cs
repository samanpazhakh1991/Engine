using Domain.Orders.Entities;

namespace Facade.Contract.Model
{
    public class OrderVM// : IOrder
    {
        public long Id { get; set; }

        public Side Side { get; set; }

        public int Price { get; set; }

        public int Amount { get; set; }

        public bool IsFillAndKill { get; set; } = false;

        public DateTime ExpireTime { get; set; }

        public bool HasCompleted { get; set; }

        public bool IsExpired { get; set; }

        public long? OrderParentId { get; set; }

        public OrderStates? OrderState { get; set; }

        public int? OriginalAmount { get; set; }

        public Guid MarketId { get; set; }
    }
}