using Application.Contract.Commands;
using Application.Factories;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;
using Domain.Orders.Entities;
using Framework.Contracts.Common;

namespace Application.OrderService.OrderCommandHandlers
{
    public class CancelAllOrdersCommandHandler : StockMarketCommandHandler<CancelAllOrderCommand>
    {
        public CancelAllOrdersCommandHandler(IStockMarketFactory stockMarketFactory,
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

        protected override async Task<ProcessedOrder?> SpecificHandle(CancelAllOrderCommand command)
        {
            var allOrders = await _orderQuery
                .GetAll(x => x.Amount != 0 && x.OrderState != OrderStates.Cancel)
                .ConfigureAwait(false);

            var orderIdList = new List<long>();

            foreach (var item in allOrders)
            {
                var processedOrder = await _stockMarketMatchEngine
                                            .CancelOrder(
                                            item.Id,
                                            command.DoesMatch,
                                            id => _orderCommandRepository.Find(id)
                                            )
                                            .ConfigureAwait(false);

                var stockMarket = await _stockMarketCommandRepository
                                        .Find(SeedData.FinancialInstrumentStockMarketId)
                                        .ConfigureAwait(false);

                stockMarket.UpdateBy(processedOrder.StockMarketMatchEngine);

                orderIdList.Add(item.Id);
            }

            return new ProcessedOrder()
            {
                CanceledOrders = orderIdList
            };
        }
    }
}