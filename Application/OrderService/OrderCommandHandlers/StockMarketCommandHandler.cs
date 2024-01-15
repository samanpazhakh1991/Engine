using Application.Contract.CommandHandlerContracts;
using Application.Contract.Commands;
using Application.Factories;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;

namespace Application.OrderService.OrderCommandHandlers
{
    public abstract class StockMarketCommandHandler<T> : ICommandHandler<T> where T : ICommand
    {
        protected readonly IStockMarketFactory _stockMarketFactory;
        protected readonly IOrderCommandRepository _orderCommandRepository;
        protected readonly IOrderQueryRepository _orderQuery;
        protected readonly ITradeQueryRepository _tradeQuery;
        protected readonly IStockMarketQueryRepository _stockMarketQueryRepository;
        protected readonly IStockMarketCommandRepository _stockMarketCommandRepository;
        protected readonly ITradeCommandRepository _tradeCommandRepository;
        protected IStockMarketMatchEngineWithState _stockMarketMatchEngine;

        public StockMarketCommandHandler(IStockMarketFactory stockMarketFactory,
            IOrderCommandRepository orderCommandRepository,
            IOrderQueryRepository orderQueryRepository,
            ITradeCommandRepository tradeCommandRepository,
            ITradeQueryRepository tradeQueryRepository,
            IStockMarketQueryRepository stockMarketQueryRepository,
            IStockMarketCommandRepository stockMarketCommandRepository
            )
        {
            _stockMarketFactory = stockMarketFactory;
            _orderCommandRepository = orderCommandRepository;
            _orderQuery = orderQueryRepository;
            _tradeCommandRepository = tradeCommandRepository;
            _tradeQuery = tradeQueryRepository;
            _stockMarketQueryRepository = stockMarketQueryRepository;
            _stockMarketCommandRepository = stockMarketCommandRepository;
        }

        public async Task<ProcessedOrder?> Handle(T command)
        {
            _stockMarketMatchEngine = await
                _stockMarketFactory
                .GetStockMarket(_orderQuery, _tradeQuery, _stockMarketQueryRepository)
                .ConfigureAwait(false);
            return await SpecificHandle(command).ConfigureAwait(false);
        }

        protected abstract Task<ProcessedOrder?> SpecificHandle(T command);
    }
}