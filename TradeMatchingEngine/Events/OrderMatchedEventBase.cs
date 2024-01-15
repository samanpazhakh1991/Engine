using Domain.Trades.Entities;
using Framework.Contracts.Events;

namespace Domain.Events
{
    public class OrderMatchedEventBase : IDomainEvent, ITradeInfo
    {
        public static T Create<T>(
            long buyOrderId,
            long sellOrderId,
            int amount,
            int price,
            bool isMain = false) where T : OrderMatchedEventBase
        {
            var @event = Activator.CreateInstance<T>();

            @event.Amount = amount;
            @event.SellOrderId = sellOrderId;
            @event.BuyOrderId = buyOrderId;
            @event.Price = price;
            @event.IsMain = isMain;
            return @event;
        }

        public Guid CorrelationId { get; set; }

        public bool IsProcessCompletedForCorrelationId { get; set; }
        public long AggregateVersion { get; set; }

        public int Amount { get; set; }

        public long BuyOrderId { get; set; }

        public int Price { get; set; }

        public long SellOrderId { get; set; }

        public bool IsMain { get; set; }
    }
}