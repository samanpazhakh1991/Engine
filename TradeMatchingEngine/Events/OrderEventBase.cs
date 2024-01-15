using Domain.Orders.Entities;
using Framework.Contracts.Events;

namespace Domain.Events
{
    public class OrderEventBase : IDomainEvent, IOrderInfo
    {
        public static T Create<T>(IOrder order, bool isMain = false) where T : OrderEventBase
        {
            var @event = Activator.CreateInstance<T>();

            @event.Amount = order.Amount;
            @event.ExpireTime = order.ExpireTime;
            @event.Id = order.Id;
            @event.IsExpired = order.IsExpired;
            @event.IsFillAndKill = order.IsFillAndKill;
            @event.OrderParentId = order.OrderParentId;
            @event.OrderState = order.OrderState;
            @event.OriginalAmount = order.OriginalAmount;
            @event.Price = order.Price;
            @event.Side = order.Side;
            @event.HasCompleted = order.HasCompleted;
            @event.MarketId = order.MarketId;
            @event.IsMain = isMain;
            return @event;
        }

        public int Amount { get; set; }
        public Guid CorrelationId { get; set; }

        public DateTime ExpireTime { get; set; }

        public bool HasCompleted { get; set; }

        public long Id { get; set; }

        public bool IsExpired { get; set; }

        public bool IsFillAndKill { get; set; }

        public bool IsProcessCompletedForCorrelationId { get; set; }
        public long AggregateVersion { get; set; }

        public long? OrderParentId { get; set; }

        public OrderStates? OrderState { get; set; }

        public int? OriginalAmount { get; set; }

        public int Price { get; set; }

        public Side Side { get; set; }

        public Guid MarketId { get; set; }

        public bool IsMain { get; set; }
    }
}