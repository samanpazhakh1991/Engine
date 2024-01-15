using Framework.Contracts.Common;
using Framework.Contracts.Events;

namespace Domain
{
    public interface IStockMarketMatchEngine : IAggregateRoot<Guid>
    {
        int BuyOrderCount { get; }
        int SellOrderCount { get; }
        Guid FinancialInstrumentId { get; }

        void RemoveDomainEvent(IDomainEvent @event);

        void UpdateBy(IStockMarketMatchEngine stockMarketMatchEngine, bool clearOriginalEvents = true);
    }
}