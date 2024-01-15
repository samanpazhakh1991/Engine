using Autofac;
using Framework.Contracts.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.GenericServices
{
    public class ServiceFactory : IServiceFactory
    {
        private ILifetimeScope container;

        public ServiceFactory(ILifetimeScope container)
        {
            this.container = container;
        }

        public IEnumerable<T> GetServices<T>()
        {
            var services = container.Resolve<IEnumerable<T>>();
            return services;
        }
    }
}
