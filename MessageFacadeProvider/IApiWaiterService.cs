using Framework.Contracts.Events;

namespace MessageFacadeProvider
{
    public interface IApiWaiterService<T>
        where T : IDomainEvent
    {
        Task<ProcessOrderData> Enqueue(Guid key);

        void Release(T @event);
    }
}