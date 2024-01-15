using Application.Contract.CommandHandlerContracts;
using Domain;
using Framework.Contracts;
using Framework.Contracts.Events;
using Framework.Contracts.UnitOfWork;
using System.Reflection;
using ICommand = Application.Contract.Commands.ICommand;

namespace Application.OrderService.OrderCommandHandlers
{
    public class TransactionalCommandHandler<T> : ICommandHandler<T>, IAsyncDisposable, IDisposable
        where T : ICommand
    {
        private readonly ICommandHandler<T> handler;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDispatcher dispatcher;
        private readonly IMessageService messageService;
        private readonly ITransactionCounter transactionCounter;
        private ITransactionService transactionService;
        public Guid Id { get; set; }

        public TransactionalCommandHandler(ICommandHandler<T> handler,
                                           IUnitOfWork unitOfWork,
                                           IDispatcher dispatcher,
                                           IMessageService messageService,
                                           ITransactionCounter transactionCounter)
        {
            this.handler = handler;
            this.unitOfWork = unitOfWork;
            this.dispatcher = dispatcher;
            this.messageService = messageService;
            this.transactionCounter = transactionCounter;
            Id = Guid.NewGuid();
        }

        public async ValueTask DisposeAsync()
        {
            if (transactionCounter.Counter > 0) return;
            await unitOfWork.DisposeAsync();

            if (transactionService != null)
                await transactionService.DisposeAsync().ConfigureAwait(false);
        }

        public async Task<ProcessedOrder?> Handle(T command)
        {
            await beginTransaction()
                .ConfigureAwait(false);

            var result = await handler.Handle(command)
                                      .ConfigureAwait(false);

            var aggregateRoots = unitOfWork
                .GetModifiedAggregateRoots();

            await unitOfWork
                .SaveChange()
                .ConfigureAwait(false);

            var events = new List<object>();
            foreach (var aggregateRoot in aggregateRoots)
            {
                foreach (var domainEvent in aggregateRoot.DomainEvents.ToList())
                {
                    events.Add(domainEvent);
                    aggregateRoot.RemoveDomainEvent(domainEvent);

                    dispatcher
                        .GetType()
                        .MakeGenericMethodByName("Dispatch", domainEvent.GetType())
                        .Invoke(dispatcher, new object[] { domainEvent });
                }
            }

            await commitTransaction().ConfigureAwait(false);

            if (events.Any())
            {
                (events.Last() as IDomainEvent)!.IsProcessCompletedForCorrelationId = true;
            }

            foreach (IDomainEvent @event in events)
            {
                @event.CorrelationId = command.CorrelationId;

                await messageService.PublishMessageAsync(@event).ConfigureAwait(false);
            }

            return result;
        }

        private async Task commitTransaction()
        {
            var counter = transactionCounter.Decrement();
            if (counter == 0)
                await transactionService.CommitAsync().ConfigureAwait(false);
        }

        private async Task beginTransaction()
        {
            var counter = transactionCounter.Increment();
            if (transactionService != null) return;

            var lockObj = new SemaphoreSlim(1);
            await lockObj.WaitAsync().ConfigureAwait(false);
            if (transactionService != null) return;
            transactionService = await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    public static class ReflectionHelper
    {
        public static MethodInfo MakeGenericMethodByName(this Type type, string methodName, Type genericArgumentType)
        {
            var dispatchMethodInfo = type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(mi => mi.Name == methodName);

            return dispatchMethodInfo.MakeGenericMethod(genericArgumentType);
        }
    }
}