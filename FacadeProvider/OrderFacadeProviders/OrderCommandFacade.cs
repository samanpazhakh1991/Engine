using Application.Contract.CommandHandlerContracts;
using Application.Contract.Commands;
using Domain;
using Facade.Contract;

namespace FacadeProvider.OrderFacadeProviders
{
    public class OrderCommandFacade : IOrderCommandFacade
    {
        private readonly ICommandHandler<AddOrderCommand> addOrderCommandHandlers;
        private readonly ICommandHandler<ModifyOrderCommand> modifyOrderCommandHandlers;
        private readonly ICommandHandler<CancelOrderCommand> cancelOrderCommandHandlers;
        private readonly ICommandHandler<CancelAllOrderCommand> cancelAllOrderCommandHandler;

        public OrderCommandFacade(ICommandHandler<AddOrderCommand> addOrderCommandHandlers,
                                  ICommandHandler<ModifyOrderCommand> modifyOrderCommandHandlers,
                                  ICommandHandler<CancelOrderCommand> cancelOrderCommandHandlers,
                                  ICommandHandler<CancelAllOrderCommand> cancelAllOrderCommandHandler)
        {
            this.addOrderCommandHandlers = addOrderCommandHandlers;
            this.modifyOrderCommandHandlers = modifyOrderCommandHandlers;
            this.cancelOrderCommandHandlers = cancelOrderCommandHandlers;
            this.cancelAllOrderCommandHandler = cancelAllOrderCommandHandler;
        }

        public async Task<ProcessedOrder?> CancelAllOrders(CancelAllOrderCommand command)
        {
            return await cancelAllOrderCommandHandler.Handle(command).ConfigureAwait(false);
        }

        public async Task<ProcessedOrder?> CancelOrder(CancelOrderCommand command)
        {
            return await cancelOrderCommandHandlers.Handle(command).ConfigureAwait(false);
        }

        public async Task<ProcessedOrder?> ModifyOrder(ModifyOrderCommand command)
        {
            return await modifyOrderCommandHandlers.Handle(command).ConfigureAwait(false);
        }

        public async Task<ProcessedOrder?> ProcessOrder(AddOrderCommand command)
        {
            return await addOrderCommandHandlers.Handle(command).ConfigureAwait(false);
        }
    }
}