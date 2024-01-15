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
    public class ModifyOrderCommandHandler : StockMarketCommandHandler<ModifyOrderCommand>
    {
        public ModifyOrderCommandHandler(IStockMarketFactory stockMarketFactory,
                                          IOrderCommandRepository orderCommandRepository,
                                          IOrderQueryRepository orderQueryRepository,
                                          ITradeCommandRepository tradeCommandRepository,
                                          ITradeQueryRepository tradeQueryRepository,
                                          IStockMarketQueryRepository stockMarketQueryRepository,
                                           IStockMarketCommandRepository stockMarketCommandRepository
                                          )
            : base(stockMarketFactory,
                   orderCommandRepository,
                   orderQueryRepository,
                   tradeCommandRepository,
                   tradeQueryRepository,
                   stockMarketQueryRepository,
                   stockMarketCommandRepository
                   )
        {
        }

        protected override async Task<ProcessedOrder?> SpecificHandle(ModifyOrderCommand command)
        {
            var result = await _stockMarketMatchEngine
                        .ModifyOrder
                        (command.OrderId,
                            command.Price,
                            command.Amount,
                            command.ExpDate,
                            command.DoesMatch,
                            id => _orderCommandRepository.Find(id))
                        .ConfigureAwait(false);

            var res = new ProcessedOrder()
            {
                ModifiedOrders =
                    new List<long>()
                    {
                      result?.ModifiedOrders?.FirstOrDefault()?.Id ??0,
                    }
            };

            if (!result.IsStockMarcketChanged) return res;

            var stockMarket = await _stockMarketCommandRepository
               .Find(SeedData.FinancialInstrumentStockMarketId)
               .ConfigureAwait(false);

            stockMarket
                .UpdateBy(result.StockMarketMatchEngine);

            return res;
        }
    }
}