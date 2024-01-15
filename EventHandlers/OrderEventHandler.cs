using Domain.Events;
using Framework.Contracts.Events;
using MessageFacadeProvider;

namespace EventHandlers
{
    public class OrderEventHandler :
        IHandleMessages<OrderCanceled>,
        IHandleMessages<OrderCreated>,
        IHandleMessages<OrderMatched>,
        IHandleMessages<OrderModified>
    {
        private readonly IApiWaiterService<IDomainEvent> apiWaiterService;

        public OrderEventHandler(
            IApiWaiterService<IDomainEvent> apiWaiterService
            )
        {
            this.apiWaiterService = apiWaiterService;
        }

        public Task Handle(OrderCanceled message, IMessageHandlerContext context)
        {
            apiWaiterService.Release(message);
            return Task.CompletedTask;
        }

        public Task Handle(OrderCreated message, IMessageHandlerContext context)
        {
            apiWaiterService.Release(message);
            return Task.CompletedTask;
        }

        public Task Handle(OrderMatched message, IMessageHandlerContext context)
        {
            apiWaiterService.Release(message);
            return Task.CompletedTask;
        }

        public Task Handle(OrderModified message, IMessageHandlerContext context)
        {
            apiWaiterService.Release(message);
            return Task.CompletedTask;
        }
    }
}