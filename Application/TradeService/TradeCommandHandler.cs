using Application.Contract.Commands;
using Application.Factories;
using Application.OrderService.OrderCommandHandlers;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;

namespace Application.TradeService
{
    public class TradeCommandHandler : StockMarketCommandHandler<CreateTradeCommand>
    {
        public TradeCommandHandler(IStockMarketFactory stockMarketFactory,
            IOrderCommandRepository orderCommandRepository,
            IOrderQueryRepository orderQueryRepository,
            ITradeCommandRepository tradeCommandRepository,
            ITradeQueryRepository tradeQueryRepository,
            IStockMarketQueryRepository stockMarketQueryRepository,
            IStockMarketCommandRepository stockMarketCommandRepository
            ) :
            base(stockMarketFactory,
                orderCommandRepository,
                orderQueryRepository,
                tradeCommandRepository,
                tradeQueryRepository,
                stockMarketQueryRepository,
                stockMarketCommandRepository
                )
        {
        }

        protected override async Task<ProcessedOrder?> SpecificHandle(CreateTradeCommand command)
        {

            var trade = await _stockMarketMatchEngine.CreateTrade
                (command.BuyOrderId,
                command.SellOrderId,
                command.Amount,
                command.Price).ConfigureAwait(false);

            await _tradeCommandRepository.Add(trade);
            return new ProcessedOrder() { Trades = new List<long>() { trade.Id } };
        }
    }
}