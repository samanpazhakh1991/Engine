using Application.Contract.CommandHandlerContracts;
using Application.Contract.Commands;
using Domain.Events;
using Framework.Contracts.Events;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class DomainEventHandler :
        IDomainEventHandler<OrderCreated>,
        IDomainEventHandler<OrderMatched>,
        IDomainEventHandler<OrderCanceled>,
        IDomainEventHandler<OrderModified>

    {
        private readonly ILogger logger;
        private readonly ICommandHandler<AddOrderCommand> addOrderCommandHandler;
        private readonly ICommandHandler<ModifyOrderCommand> modifyOrderCommandHandler;
        private readonly ICommandHandler<CancelOrderCommand> cancelOrderCommandHandler;
        private readonly ICommandHandler<CreateTradeCommand> createTradeCommdandHandler;

        public DomainEventHandler(
            ILogger<DomainEventHandler> logger,
            ICommandHandler<AddOrderCommand> addOrderCommandHandler,
            ICommandHandler<ModifyOrderCommand> modifyOrderCommandHandler,
            ICommandHandler<CancelOrderCommand> cancelOrderCommandHandler, ICommandHandler<CreateTradeCommand> createTradeCommdandHandler)
        {
            this.logger = logger;
            this.addOrderCommandHandler = addOrderCommandHandler;
            this.modifyOrderCommandHandler = modifyOrderCommandHandler;
            this.cancelOrderCommandHandler = cancelOrderCommandHandler;
            this.createTradeCommdandHandler = createTradeCommdandHandler;
        }

        public void Handle(OrderCreated @event)
        {
            logger.LogCritical("Order Created Event Handled By FirstOrder{} With ID: {} \n", nameof(DomainEventHandler), @event.Id);

            addOrderCommandHandler.Handle(new AddOrderCommand()
            {
                Amount = @event.Amount,
                CorrelationId = @event.CorrelationId,
                ExpDate = @event.ExpireTime,
                IsFillAndKill = @event.IsFillAndKill,
                OrderParentId = @event.OrderParentId,
                Price = @event.Price,
                Side = @event.Side,
                DoesMatch = false,
                Id = @event.Id
            }).GetAwaiter().GetResult();
        }

        public void Handle(OrderMatched @event)
        {
            logger.LogCritical($"Trade Created: \n");
            createTradeCommdandHandler.Handle(new CreateTradeCommand()
            {
                Amount = @event.Amount,
                CorrelationId = @event.CorrelationId,
                Price = @event.Price,
                BuyOrderId = @event.BuyOrderId,
                SellOrderId = @event.SellOrderId,
            }).GetAwaiter().GetResult();
        }

        public void Handle(OrderCanceled @event)
        {
            logger.LogCritical("Order Cancelled Event Handled By FirstOrder{} With ID: {} \n", nameof(DomainEventHandler), @event.Id);

            cancelOrderCommandHandler.Handle(
                new CancelOrderCommand()
                {
                    DoesMatch = false,
                    Id = @event.Id
                }).GetAwaiter().GetResult();
        }

        public void Handle(OrderModified @event)
        {
            logger.LogCritical("Order modified Event Handled By FirstOrder{} With ID: {} \n", nameof(DomainEventHandler), @event.Id);

            modifyOrderCommandHandler.Handle(new ModifyOrderCommand()
            {
                Amount = @event.Amount,
                CorrelationId = @event.CorrelationId,
                ExpDate = @event.ExpireTime,
                OrderId = @event.Id,
                Price = @event.Price,
                DoesMatch = false
            }).GetAwaiter().GetResult();
        }
    }
}