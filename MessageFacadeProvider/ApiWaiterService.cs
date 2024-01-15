using Domain.Events;
using Framework.Contracts.Events;
using System.Collections.Concurrent;

namespace MessageFacadeProvider
{
    public class ApiWaiterService : IApiWaiterService<IDomainEvent>
    {
        public ConcurrentDictionary<Guid, Tuple<ProcessOrderData, TaskCompletionSource<ProcessOrderData>>> Works { get; set; }
        
        public ApiWaiterService()
        {
            Works = new ConcurrentDictionary<Guid, Tuple<ProcessOrderData, TaskCompletionSource<ProcessOrderData>>>();
        }

        public Task<ProcessOrderData> Enqueue(Guid key)
        {
            var item = Works.AddOrUpdate(key,
            k =>
            new Tuple<ProcessOrderData, TaskCompletionSource<ProcessOrderData>>(
                new ProcessOrderData(),
            new TaskCompletionSource<ProcessOrderData>()),
            (k, v) => v);

            return item.Item2.Task;
        }

        public void Release(IDomainEvent @event)
        {
            if (!Works.Keys.Contains(@event.CorrelationId)) return;

            switch (@event)
            {
                case OrderCanceled canceled:
                {
                    Works[@event.CorrelationId].Item1.CancelledOrders.Add(canceled.Id);
                    break;
                }
                case OrderModified modified:
                {
                    Works[@event.CorrelationId].Item1.ModifiedOrder?.Add(modified!.Id);
                    break;
                }
                case OrderCreated created:
                {
                    Works[@event.CorrelationId].Item1.OrderId = created.Id;
                    break;
                }
            }

            if (!@event.IsProcessCompletedForCorrelationId) return;
            Works.TryRemove(@event.CorrelationId, out var item);
            item?.Item2.SetResult(item.Item1);
        }
    }
}