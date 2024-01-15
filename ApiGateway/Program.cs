using Autofac.Extensions.DependencyInjection;
using Controllers.Controller;
using Facade.Contract;
using Framework.Contracts;
using Framework.Contracts.Events;
using MessageFacadeProvider;
using MessageNServiceBus;
using Messages;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using NServiceBus.Extensions.Logging;

namespace ApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                LogManager
                .Setup()
                .LoadConfigurationFromAppSettings();
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

                //NLog
                builder.Logging.ClearProviders();
                builder.Logging.AddNLog();

                builder.Host.UseNServiceBus(ctx =>
                {
                    NServiceBus.Logging.LogManager.UseFactory(
                        loggerFactory: new ExtensionsLoggerFactory(
                            builder.Services.BuildServiceProvider().
                                GetRequiredService<ILoggerFactory>())
                        );

                    var endpointConfiguration = new EndpointConfiguration("ApiGateway");
                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningEventsAs(type => typeof(IDomainEvent).IsAssignableFrom(type));
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(AddOrderCommandMessage).Assembly, "StockMarketService");

                    return endpointConfiguration;
                });

                builder.Services.AddScoped<IMessageService, MessageSender>();
                builder.Services.AddScoped<IOrderQueryFacade, ProxyOrderFacadeQueryService>();
                builder.Services.AddScoped<IOrderCommandFacade, OrderMessageCommandFacade>();
                builder.Services.AddScoped<ITradeQueryFacade, ProxyTradeFacadeQueryService>();
                builder.Services.AddSingleton<IApiWaiterService<IDomainEvent>, ApiWaiterService>();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddMvc();
                builder.Services
                        .AddControllers()
                        .AddApplicationPart(typeof(OrdersController).Assembly)
                        .AddControllersAsServices();

                var app = builder.Build();

                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });

                app.UseStaticFiles();
                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.MapControllers();
                app.Run();
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}