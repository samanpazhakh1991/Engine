using Application.Contract.Commands;
using Application.Factories;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;
using Framework.Contracts.Common;

namespace Application.OrderService.OrderCommandHandlers
{
    public class AddOrderCommandHandler : StockMarketCommandHandler<AddOrderCommand>
    {
        public AddOrderCommandHandler(IStockMarketFactory stockMarketFactory,
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
                stockMarketCommandRepository)
        {
        }

        protected override async Task<ProcessedOrder> SpecificHandle(AddOrderCommand? command)
        {
            var result = await _stockMarketMatchEngine
                                  .ProcessOrderAsync(
                                   command.Price,
                                   command.Amount,
                                   command.Side,
                                   command.ExpDate,
                                   command.IsFillAndKill,
                                   command.OrderParentId,
                                   command.DoesMatch,
                                   orderId: command.Id
                                   ).ConfigureAwait(false);

            var processedOrder = new ProcessedOrder()
            {
                OrderId = result.Order == null ? 0 : result.Order.Id,
                Trades = result.CreatedTrades.Select(t => t.Id),
                ModifiedOrders = result.ModifiedOrders.Select(o => o.Id)
            };

            if (!result.IsStockMarcketChanged)
            {
                await _orderCommandRepository.Add(result.Order!).ConfigureAwait(false);
                return processedOrder;
            }

            var stockMarket = await _stockMarketCommandRepository
                .Find(SeedData.FinancialInstrumentStockMarketId)
                .ConfigureAwait(false);

            stockMarket.UpdateBy(result.StockMarketMatchEngine);

            return processedOrder;
        }
    }
}