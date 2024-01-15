using Autofac;
using Autofac.Extensions.DependencyInjection;
using Facade.Contract;
using FacadeProvider.OrderFacadeProviders;
using FacadeProvider.TradeFacadeProvider;
using Framework.Contracts;
using Framework.Contracts.Events;
using Infrastructure;
using MessageNServiceBus;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using NServiceBus;
using NServiceBus.Extensions.Logging;

namespace StockMarketApi
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
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                builder.Host.ConfigureContainer<ContainerBuilder>(c =>
                {
                    c.DependencyHolder(connectionString);
                });

                //NLog
                builder.Logging.ClearProviders();
                builder.Logging.AddNLog();

                builder.Host.UseNServiceBus(ctx =>
                {
                    NServiceBus.Logging.LogManager.UseFactory(loggerFactory: new ExtensionsLoggerFactory(
                        builder.Services.BuildServiceProvider().
                        GetRequiredService<ILoggerFactory>())
                        );

                    var endpointConfiguration = new EndpointConfiguration("StockMarketService");
                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningEventsAs(type => typeof(IDomainEvent).IsAssignableFrom(type));

                    var transport = endpointConfiguration.UseTransport<LearningTransport>();

                    return endpointConfiguration;
                });

                builder.Services.DependencyHolder();
                builder.Services.AddScoped<IMessageService, MessageSender>();
                builder.Services.AddScoped<IOrderCommandFacade, OrderCommandFacade>();
                builder.Services.AddScoped<IOrderQueryFacade, OrderQueryFacade>();
                builder.Services.AddScoped<ITradeQueryFacade, TradeQueryFacade>();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddMvc();

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