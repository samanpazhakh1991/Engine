namespace Framework.Contracts.Events
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        void Handle(T @event);
    }
}