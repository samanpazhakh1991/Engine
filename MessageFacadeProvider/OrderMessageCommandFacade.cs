using Application.Contract.Commands;
using Domain;
using Facade.Contract;
using Framework.Contracts;
using Framework.Contracts.Events;
using Messages;

namespace MessageFacadeProvider
{
    public class OrderMessageCommandFacade : IOrderCommandFacade, IAsyncDisposable, IDisposable
    {
        private readonly IMessageService messageService;
        private readonly IApiWaiterService<IDomainEvent> apiWaiterService;

        public OrderMessageCommandFacade(IMessageService messageService, IApiWaiterService<IDomainEvent> apiWaiterService)
        {
            this.messageService = messageService;
            this.apiWaiterService = apiWaiterService;
        }

        public async Task<ProcessedOrder?> ProcessOrder(AddOrderCommand command)
        {
            var message = new AddOrderCommandMessage()
            {
                Side = command.Side.ToMessage(),
                Amount = command.Amount,
                Price = command.Price,
                IsFillAndKill = command.IsFillAndKill,
                ExpireTime = (DateTime)command.ExpDate,
                CorrelationId = command.CorrelationId,
            };

            await messageService.SendMessageAsync(message).ConfigureAwait(false);

            var result = await apiWaiterService.Enqueue(command.CorrelationId);

            return new ProcessedOrder()
            {
                CanceledOrders = result.CancelledOrders,
                OrderId = result.OrderId,
                Trades = result.Trades,
                ModifiedOrders = result?.ModifiedOrder
            };
        }

        public async Task<ProcessedOrder?> CancelAllOrders(CancelAllOrderCommand command)
        {
            var message = new CancelAllOrderCommandMessage()
            {
                CorrelationId = command.CorrelationId
            };

            await messageService.SendMessageAsync(message).ConfigureAwait(false);

            var resultTask = apiWaiterService.Enqueue(command.CorrelationId);

            await Task.WhenAny(resultTask, Task.Delay(3000));

            if (resultTask.IsCompleted)
            {
                return new ProcessedOrder()
                {
                    CanceledOrders = resultTask.Result.CancelledOrders,
                    OrderId = resultTask.Result.OrderId,
                    Trades = resultTask.Result.Trades
                };
            }
            return new ProcessedOrder();
        }

        public async Task<ProcessedOrder?> CancelOrder(CancelOrderCommand command)
        {
            var message = new CancelOrderCommandMessage()
            {
                Id = command.Id,
                CorrelationId = command.CorrelationId
            };

            await messageService.SendMessageAsync(message).ConfigureAwait(false);

            var result = await apiWaiterService.Enqueue(command.CorrelationId);

            return new ProcessedOrder()
            {
                CanceledOrders = result.CancelledOrders,
                OrderId = result.OrderId,
                Trades = result.Trades
            };
        }

        public void Dispose()
        {
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<ProcessedOrder?> ModifyOrder(ModifyOrderCommand command)
        {
            var message = new ModifyOrderCommandMessage()
            {
                OrderId = command.OrderId,
                Amount = command.Amount,
                ExpDate = command.ExpDate,
                Price = command.Price,
                CorrelationId = command.CorrelationId
            };
            await messageService.SendMessageAsync(message).ConfigureAwait(false);

            var result = await apiWaiterService.Enqueue(command.CorrelationId);

            return new ProcessedOrder()
            {
                CanceledOrders = result.CancelledOrders,
                OrderId = result.OrderId,
                Trades = result.Trades,
                ModifiedOrders = result?.ModifiedOrder
            };
        }
    }
}