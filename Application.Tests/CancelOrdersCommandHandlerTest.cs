using Application.Contract.Commands;
using Application.OrderService.OrderCommandHandlers;
using Application.Tests.Constants;
using Domain;
using Domain.Orders.Entities;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class CancelOrdersCommandHandlerTest : CommandHandlerTest<CancelOrderCommandHandler, CancelOrderCommand>
    {
        private IStockMarketMatchingEngineProcessContext processContext;

        public CancelOrdersCommandHandlerTest()
        {
            processContext = Substitute.For<IStockMarketMatchingEngineProcessContext>();

            StockMarketFactoryMock.GetStockMarket(
                OrderQueryRepositoryMock,
                TradeQueryRepositoryMock,
                StockMarketQueryRepositoryMock
                ).Returns(StockMarket);

            StockMarket
           .CancelOrder(
               CancelOrderCommandConstants.SOME_ORDER_ID,
               Arg.Any<bool>(),
               Arg.Any<Func<long, Task<IOrder>>>()
              ).Returns(processContext);
        }

        [Fact]
        public async Task Handle_Should_Call_CancelOrderAsync_When_DoesMatch_Is_True()
        {
            //Arrange
            var cancelOrderCommand = MakeSomeTCommand();

            AssertBuilder.ShouldReceiveCancelOrder(1);
            AssertBuilder.ShouldReceiveFindStockMarket(1);

            processContext.IsStockMarcketChanged.Returns(true);

            //Act
            await Sut.Handle(cancelOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        [Fact]
        public async Task Handle_Should_Call_CancelOrder_When_Does_Match_Is_False()
        {
            //Arrange
            var cancelOrderCommand = new CancelOrderCommand()
            {
                Id = CancelOrderCommandConstants.SOME_ORDER_ID,
                DoesMatch = CancelOrderCommandConstants.SOME_DOES_MATCH_IS_FALSE
            };

            AssertBuilder
                .ShouldReceiveCancelOrder(1);

            processContext.IsStockMarcketChanged.Returns(false);

            //Act
            await Sut.Handle(cancelOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        protected override CancelOrderCommand MakeSomeTCommand()
        {
            return new CancelOrderCommand()
            {
                Id = CancelOrderCommandConstants.SOME_ORDER_ID,
            };
        }
    }
}