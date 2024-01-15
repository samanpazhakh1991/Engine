using Application.Factories;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;
using Domain.Orders.Entities;
using Domain.Trades.Entities;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Tests.Builders
{
    public class AssertBuilder
    {
        private readonly IOrderCommandRepository orderCommandRepositoryMock;
        private readonly IOrderQueryRepository orderQueryRepositoryMock;
        private readonly ITradeQueryRepository tradeQueryRepositoryMock;
        private readonly ITradeCommandRepository tradeCommandRepositoryMock;
        private readonly IStockMarketFactory stockMarketFactoryMock;
        private readonly IStockMarketMatchEngineWithState stockMarket;
        private readonly IStockMarketMatchingEngineProcessContext processContext;
        private readonly IStockMarketQueryRepository stockMarketQueryRepositoryMock;
        private readonly IStockMarketCommandRepository stockMarketCommandRepositoryMock;

        private int processOrderCount;
        private int orderCount;
        private int tradeCount;
        private int findOrderCount;
        private int createTradeCount;
        private int getStockMarketCount;
        private int cancelOrderCount;
        private int findStockMarketCount;
        private int getAllOrderCount;
        private int modifyOrderCount;

        public AssertBuilder(
            IStockMarketMatchEngineWithState stockMarket,
            IStockMarketMatchingEngineProcessContext processContext,
            ITradeCommandRepository tradeCommandRepositoryMock,
            IOrderCommandRepository orderCommandRepositoryMock,
            IStockMarketFactory stockMarketFactoryMock,
            IOrderQueryRepository orderQueryRepository,
            ITradeQueryRepository tradeQueryRepository,
            IStockMarketQueryRepository stockMarketQueryRepositoryMock,
            IStockMarketCommandRepository stockMarketCommandRepositoryMock
            )
        {
            this.stockMarket = stockMarket;
            this.processContext = processContext;
            this.tradeCommandRepositoryMock = tradeCommandRepositoryMock;
            this.orderCommandRepositoryMock = orderCommandRepositoryMock;
            this.stockMarketFactoryMock = stockMarketFactoryMock;
            orderQueryRepositoryMock = orderQueryRepository;
            tradeQueryRepositoryMock = tradeQueryRepository;
            this.stockMarketCommandRepositoryMock = stockMarketCommandRepositoryMock;
            this.stockMarketQueryRepositoryMock = stockMarketQueryRepositoryMock;

            ShouldReceiveProcessOrder(0);
            ShouldReceiveAddTrade(0);
            ShouldReceiveFindOrder(0);
            ShouldReceiveCancelOrder(0);
            ShouldReceiveModifyOrder(0);
            ShouldReceiveCreateTrade(0);
            ShouldReceiveGetStockMarket(0);
            ShouldReceiveFindStockMarket(0);
            ShouldReceiveGetAllOrder(0);
        }

        public AssertBuilder ShouldReceiveProcessOrder(int receiveCount)
        {
            processOrderCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveAddOrder(int receiveCount)
        {
            orderCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveAddTrade(int receiveCount)
        {
            tradeCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveFindOrder(int receiveCount)
        {
            findOrderCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveCancelOrder(int receiveCount)
        {
            cancelOrderCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveModifyOrder(int receiveCount)
        {
            modifyOrderCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveCreateTrade(int receiveCount)
        {
            createTradeCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveGetStockMarket(int receiveCount)
        {
            getStockMarketCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveFindStockMarket(int receiveCount)
        {
            findStockMarketCount = receiveCount;

            return this;
        }

        public AssertBuilder ShouldReceiveGetAllOrder(int receiveCount)
        {
            getAllOrderCount = receiveCount;

            return this;
        }

        public void Assert()
        {
            if (processOrderCount > 0)
            {
                stockMarket.Received(processOrderCount)
               .ProcessOrderAsync(
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<Side>(),
                    Arg.Any<DateTime>(),
                    Arg.Any<bool>(),
                    Arg.Any<long>(),
                   doesMath: Arg.Any<bool>(),
                   orderId: Arg.Any<long>()
               );
            }

            if (orderCount > 0)
            {
                orderCommandRepositoryMock
                    .Received(orderCount)
                    .Add(processContext.Order!);
            }

            if (tradeCount > 0)
            {
                tradeCommandRepositoryMock
                    .Received(tradeCount)
                    .Add(Arg.Any<ITrade>());
            }

            if (findOrderCount > 0)
            {
                orderCommandRepositoryMock
                    .Received(findOrderCount)
                    .Find(Arg.Any<long>());
            }

            if (cancelOrderCount > 0)
            {
                stockMarket
                   .Received(1)
                   .CancelOrder(
                  orderId: Arg.Any<long>(),
                  doseMatch: Arg.Any<bool>(),
                  findOrder: Arg.Any<Func<long, Task<IOrder>>>()
                  );
            }

            if (createTradeCount > 0)
            {
                stockMarket
                    .Received(createTradeCount)
                    .CreateTrade(
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<int>(),
                    Arg.Any<int>()
                    );
            }

            if (getStockMarketCount > 0)
            {
                stockMarketFactoryMock
                    .Received(getStockMarketCount)
                    .GetStockMarket(
                        orderQueryRepositoryMock,
                        tradeQueryRepositoryMock,
                        stockMarketQueryRepositoryMock
                      );
            }

            if (findStockMarketCount > 0)
            {
                stockMarketCommandRepositoryMock
                  .Received(findStockMarketCount)
                  .Find(Arg.Any<Guid>());
            }

            if (getAllOrderCount > 0)
            {
                orderQueryRepositoryMock
                    .Received(getAllOrderCount)
                    .GetAll(Arg.Any<Expression<Func<Order, bool>>>());
            }

            if (modifyOrderCount > 0)
            {
                stockMarket
                .Received(1)
                .ModifyOrder(
                    Arg.Any<long>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<DateTime>(),
                    Arg.Any<bool>(),
                    Arg.Any<Func<long, Task<IOrder>>>()
                    );
            }
        }
    }
}