using Application.Contract.CommandHandlerContracts;
using Application.Contract.Commands;
using Application.EventHandlers;
using Application.Factories;
using Application.OrderService.OrderCommandHandlers;
using Application.TradeService;
using Autofac;
using Autofac.Core;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;
using Domain.Events;
using Framework.Contracts;
using Framework.Contracts.Common;
using Framework.Contracts.Events;
using Framework.Contracts.UnitOfWork;
using Infrastructure.GenericServices;
using Infrastructure.Orders.CommandRepositories;
using Infrastructure.Orders.QueryRepositories;
using Infrastructure.StockMarkets.CommandRepositories;
using Infrastructure.StockMarkets.QueryRepository;
using Infrastructure.Trades.CommandRepositories;
using Infrastructure.Trades.QueryRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class BusinessDependencies
    {
        private const string INTERNAL_CMD_HANDLER_NAME = "internalCmdHadler";

        public static void DependencyHolder(this ContainerBuilder container, string? connectionString)
        {
            int eventhandlername = 1;
            container.RegisterType<DomainEventHandler>()
                .As<IDomainEventHandler<OrderCreated>>()
                .As<IDomainEventHandler<OrderMatched>>()
                .As<IDomainEventHandler<OrderCanceled>>()
                .As<IDomainEventHandler<OrderModified>>()
                .InstancePerLifetimeScope();

            foreach (var item in new Dictionary<Type, (Type, Type)> {
                {
                    typeof(ICommandHandler<AddOrderCommand>),
                    (typeof(AddOrderCommandHandler),typeof(AddOrderCommand))
                },
                {
                    typeof(ICommandHandler<ModifyOrderCommand>),
                    (typeof(ModifyOrderCommandHandler), typeof(ModifyOrderCommand))
                },
                {
                    typeof(ICommandHandler<CancelOrderCommand>),
                    (typeof(CancelOrderCommandHandler), typeof(CancelOrderCommand))
                },
                {
                    typeof(ICommandHandler<CancelAllOrderCommand>),
                    (typeof(CancelAllOrdersCommandHandler), typeof(CancelAllOrderCommand))
                },
                {
                    typeof(ICommandHandler<CreateTradeCommand>),
                    (typeof(TradeCommandHandler), typeof(CreateTradeCommand))
                },
                })
            {
                regiterCommandHandlerWithDecorator(container,
                    commandType: item.Value.Item2,
                    handlerInterface: item.Key,
                    handlerConcreteType: item.Value.Item1);
            }
            container.RegisterType<DbConnectionManager>().As<IDbConnectionService>().As<ITransactionService>()
                .WithParameter(TypedParameter.From<string>(connectionString))
                .InstancePerLifetimeScope();
        }

        public static IServiceCollection DependencyHolder(this IServiceCollection services)
        {
            services.AddDbContextFactory<TradeMatchingEngineContext>((sp, ob) =>
            {
                ob.UseSqlServer(sp.GetRequiredService<IDbConnectionService>().GetConnectionAsync().ConfigureAwait(false).GetAwaiter().GetResult());
            }, ServiceLifetime.Scoped);

            services.AddScoped<IOrderCommandRepository, OrderCommandRepository>();
            services.AddScoped<ITradeCommandRepository, TradeCommandRepository>();
            services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
            services.AddScoped<ITradeQueryRepository, TradeQueryRepository>();
            services.AddScoped<IStockMarketQueryRepository, StockMarketQueryRepository>();
            services.AddScoped<IStockMarketCommandRepository, StockMarketCommandRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IStockMarketFactory, StockMarketFactory>();
            services.AddScoped<IDispatcher, GenericDispatcher>();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<ITransactionCounter, TransactionCounter>();
            return services;
        }

        private static void regiterCommandHandlerWithDecorator(ContainerBuilder container,
           Type commandType, Type handlerInterface, Type handlerConcreteType)
        {
            var t = typeof(TransactionalCommandHandler<>).MakeGenericType(new Type[1] { commandType });
            container.RegisterType(handlerConcreteType).Named(INTERNAL_CMD_HANDLER_NAME + handlerConcreteType.Name, handlerInterface)
                .InstancePerLifetimeScope();
            container.RegisterType(t).As(handlerInterface)
                .WithParameter(
                    new ResolvedParameter(
                        (p, c) => p.ParameterType == handlerInterface,
                        (p, c) => c.ResolveNamed(INTERNAL_CMD_HANDLER_NAME + handlerConcreteType.Name, p.ParameterType)))
                .InstancePerLifetimeScope();
        }
    }
}