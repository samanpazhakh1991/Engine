using Framework.Contracts.Events;

namespace Framework.Contracts.Common
{
    public interface IAggregateRoot
    {
        IEnumerable<IDomainEvent> DomainEvents { get; }

        void AddDomainEvent(IDomainEvent eventItem);

        void ClearEvents();

        void RemoveDomainEvent(IDomainEvent eventItem);

        long Version { get; }
    }

    public interface IAggregateRoot<TId> : IAggregateRoot, IEntity<TId>
    {
    }
}