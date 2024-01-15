using Framework.Contracts.Common;

namespace Framework.Contracts.Events
{
    public class GenericDispatcher : IDispatcher
    {
        private readonly IServiceFactory serviceFactory;

        public GenericDispatcher(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public void Dispatch<T>(T domainEvent)
            where T : class, IDomainEvent
        {
            var services = serviceFactory.GetServices<IDomainEventHandler<T>>();
            foreach (var service in services)
            {
                service.Handle(domainEvent);
            }
        }
    }
}