using Application.Contract.Commands;
using Domain.Events;
using Facade.Contract;
using Framework.Contracts.Events;
using Messages;
using NServiceBus.Logging;

namespace MessageHandlers
{
    public class OrderCommandsHandler :
        IHandleMessages<AddOrderCommandMessage>,
        IHandleMessages<ModifyOrderCommandMessage>,
        IHandleMessages<CancelOrderCommandMessage>,
        IHandleMessages<CancelAllOrderCommandMessage>

    {
        private readonly IOrderCommandFacade orderFacade;
        private readonly IOrderQueryFacade _orderQueryFacade;
        private static ILog log = LogManager.GetLogger<OrderCommandsHandler>();

        public OrderCommandsHandler

            (
            IOrderCommandFacade orderFacade,
            IOrderQueryFacade orderQueryFacade
            )
        {
            this.orderFacade = orderFacade;
            _orderQueryFacade = orderQueryFacade;
        }

        public async Task Handle(AddOrderCommandMessage message, IMessageHandlerContext context)
        {
            log.InfoFormat("Handle {CorollationId}", message.CorrelationId);

            var command = new Application.Contract.Commands.AddOrderCommand()
            {
                Amount = message.Amount,
                ExpDate = message.ExpireTime,
                Side = message.Side.ToDomain(),
                Price = message.Price,
                IsFillAndKill = (bool)message.IsFillAndKill,
                CorrelationId = message.CorrelationId
            };

            _ = await orderFacade.ProcessOrder(command).ConfigureAwait(false);
        }

        public async Task Handle(ModifyOrderCommandMessage message, IMessageHandlerContext context)
        {
            var command = new ModifyOrderCommand()
            {
                OrderId = message.OrderId,
                Amount = message.Amount,
                Price = message.Price,
                ExpDate = message.ExpDate,
                CorrelationId = message.CorrelationId
            };

            _ = await orderFacade.ModifyOrder(command).ConfigureAwait(false);
        }

        public async Task Handle(CancelOrderCommandMessage message, IMessageHandlerContext context)
        {
            var command = new CancelOrderCommand()
            {
                Id = message.Id,
                CorrelationId = message.CorrelationId
            };

            _ = await orderFacade.CancelOrder(command).ConfigureAwait(false);
        }

        public async Task Handle(CancelAllOrderCommandMessage message, IMessageHandlerContext context)
        {
            var command = new CancelAllOrderCommand() { CorrelationId = message.CorrelationId };
            _ = await orderFacade.CancelAllOrders(command).ConfigureAwait(false);
        }
    }
}