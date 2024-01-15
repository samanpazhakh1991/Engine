using Application.Factories;
using Application.OrderService.OrderCommandHandlers;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class TestCommandHandler : StockMarketCommandHandler<TestCommand>, ICallCounter
    {
        public int CallCount { get; set; }

        public TestCommandHandler(IStockMarketFactory stockMarketFactory,
            IOrderCommandRepository orderCommandRepository,
            IOrderQueryRepository orderQueryRepository,
            ITradeCommandRepository tradeCommandRepository,
            ITradeQueryRepository tradeQueryRepository,
            IStockMarketQueryRepository stockMarketQueryRepository,
            IStockMarketCommandRepository stockMarketCommandRepository
            ) : base(stockMarketFactory,
                orderCommandRepository,
                orderQueryRepository,
                tradeCommandRepository,
                tradeQueryRepository,
                stockMarketQueryRepository,
                stockMarketCommandRepository
                )
        {
        }

        protected override Task<ProcessedOrder?> SpecificHandle(TestCommand? command)
        {
            CallCount++;
            return Task.FromResult(new ProcessedOrder())!;
        }
    }
}