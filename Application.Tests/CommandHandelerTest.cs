using Application.Contract.Commands;
using Application.Factories;
using Application.OrderService.OrderCommandHandlers;
using Application.Tests.Builders;
using Application.Tests.Constants;
using Domain;
using Domain.Contract.Orders.Repository.Command;
using Domain.Contract.Orders.Repository.Query;
using Domain.Contract.StockMarkets.Repository.Command;
using Domain.Contract.StockMarkets.Repository.Query;
using Domain.Contract.Trades.Repository.Command;
using Domain.Contract.Trades.Repository.Query;
using NSubstitute;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public abstract class CommandHandlerTest<THandler, TCommand>
        where THandler : StockMarketCommandHandler<TCommand> where TCommand : ICommand
    {
        protected IOrderCommandRepository OrderCommandRepositoryMock;
        protected IOrderQueryRepository OrderQueryRepositoryMock;
        protected ITradeQueryRepository TradeQueryRepositoryMock;
        protected ITradeCommandRepository TradeCommandRepositoryMock;
        protected IStockMarketFactory StockMarketFactoryMock;
        protected IStockMarketMatchEngineWithState StockMarket;
        protected IStockMarketMatchingEngineProcessContext ProcessContext;
        protected IStockMarketQueryRepository StockMarketQueryRepositoryMock;
        protected IStockMarketCommandRepository StockMarketCommandRepositoryMock;
        protected THandler Sut;
        protected AssertBuilder AssertBuilder;

        protected CommandHandlerTest()
        {
            OrderCommandRepositoryMock = Substitute.For<IOrderCommandRepository>();
            OrderQueryRepositoryMock = Substitute.For<IOrderQueryRepository>();
            TradeQueryRepositoryMock = Substitute.For<ITradeQueryRepository>();
            TradeCommandRepositoryMock = Substitute.For<ITradeCommandRepository>();
            StockMarketFactoryMock = Substitute.For<IStockMarketFactory>();
            StockMarket = Substitute.For<IStockMarketMatchEngineWithState>();
            ProcessContext = Substitute.For<IStockMarketMatchingEngineProcessContext>();
            StockMarketQueryRepositoryMock = Substitute.For<IStockMarketQueryRepository>();
            StockMarketCommandRepositoryMock = Substitute.For<IStockMarketCommandRepository>();

            Sut = (THandler)Activator.CreateInstance(typeof(THandler),
               StockMarketFactoryMock,
               OrderCommandRepositoryMock,
               OrderQueryRepositoryMock,
               TradeCommandRepositoryMock,
               TradeQueryRepositoryMock,
               StockMarketQueryRepositoryMock,
               StockMarketCommandRepositoryMock
               )!;

            AssertBuilder = new AssertBuilder(StockMarket,
                ProcessContext,
                TradeCommandRepositoryMock,
                OrderCommandRepositoryMock,
                StockMarketFactoryMock,
                OrderQueryRepositoryMock,
                TradeQueryRepositoryMock,
                StockMarketQueryRepositoryMock,
                StockMarketCommandRepositoryMock
                );
        }

        [Fact]
        public async Task Handle_Should_Call_GetStockMarket_On_StockMarketFactory_TestAsync()
        {
            //Arrange
            setModifiedOrders();
            AssertBuilder.ShouldReceiveGetStockMarket(1);

            //Act
            await Sut.Handle(MakeSomeTCommand());

            //Assert
            AssertBuilder.Assert();
        }

        [Fact]
        public async Task Handle_Should_Call_SpecificHandle_On_Sut_TestAsync()
        {
            //Arrange
            if (!typeof(ICallCounter).IsAssignableFrom(typeof(THandler))) return;

            //Act
            await Sut.Handle(MakeSomeTCommand());

            //Assert
            var callCounter = (ICallCounter)Sut;
            Assert.Equal(1, callCounter.CallCount);
        }

        [Fact]
        public void Test1()
        {
            //Arrange
            var sut = new ConcurrentDictionary<string, int>();
            sut.AddOrUpdate("http://wwww.google.com", key => 10, (key, ov) => ov);
            sut.AddOrUpdate("http://wwww.linkedin.com", key => 20, (key, ov) => ov);
            var url = "http://wwww.google.com/trades";
            //Act

            var actual = sut.First(i => url.StartsWith(i.Key)).Value;

            //Assert
            Assert.Equal(10, actual);
        }

        protected abstract TCommand MakeSomeTCommand();

        protected void setModifiedOrders()
        {
            ProcessContext.ModifiedOrders.Returns(
            new List<TestOrder>()
               {
                    new()
                    {
                        Id = ModifyOrderCommandConstants.SOME_ORDER_ID,
                        Amount = ModifyOrderCommandConstants.SOME_AMOUNT,
                        ExpireTime = ModifyOrderCommandConstants.SOME_EXPIRATION_DATE,
                        IsFillAndKill = ModifyOrderCommandConstants.SOME_IS_FILL_AND_KILL,
                        OrderParentId = ModifyOrderCommandConstants.SOME_ORDER_PARENT_ID,
                        OrderState = ModifyOrderCommandConstants.SOME_ORDER_STATE,
                        Price = ModifyOrderCommandConstants.SOME_PRICE,
                        Side = ModifyOrderCommandConstants.SOME_SIDE
                    }
               });
        }
    }
}