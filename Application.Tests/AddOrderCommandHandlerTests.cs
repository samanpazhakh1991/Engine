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
    public class AddOrderCommandHandlerTests : CommandHandlerTest<AddOrderCommandHandler, AddOrderCommand>
    {
        private IStockMarketMatchingEngineProcessContext processContext;

        public AddOrderCommandHandlerTests()
        {
            processContext = Substitute.For<IStockMarketMatchingEngineProcessContext>();

            StockMarketFactoryMock.GetStockMarket(
                OrderQueryRepositoryMock,
                TradeQueryRepositoryMock,
                StockMarketQueryRepositoryMock).Returns(StockMarket);

            StockMarket.ProcessOrderAsync(Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<Side>(),
                    Arg.Any<DateTime>(),
                    Arg.Any<bool>(),
                    Arg.Any<long>(),
                   doesMath: Arg.Any<bool>(),
                   orderId: Arg.Any<long>()).Returns(processContext);
        }

        [Fact]
        public async Task Handle_Should_Call_ProcessOrderAsync_And_Find_When_DoesMatch_Is_True()
        {
            //Arrange
            var addOrderCommand = MakeSomeTCommand();
            AssertBuilder.ShouldReceiveProcessOrder(1);
            AssertBuilder.ShouldReceiveFindStockMarket(1);
            processContext.IsStockMarcketChanged.Returns(true);

            //Act
            await Sut.Handle(addOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        [Fact]
        public async Task Handle_Should_Call_Handle_Should_Call_ProcessOrderAsync_And_AddOrder_When_DoesMatch_Is_False()
        {
            //Arrange
            var addOrderCommand = new AddOrderCommand
            {
                Amount = AddOrderCommandConstants.SOME_AMOUNT,
                ExpDate = AddOrderCommandConstants.SOME_EXPIRATION_DATE,
                IsFillAndKill = AddOrderCommandConstants.SOME_IS_FILL_AND_KILL,
                Price = AddOrderCommandConstants.SOME_PRICE,
                Side = AddOrderCommandConstants.SOME_SIDE,
                OrderParentId = AddOrderCommandConstants.SOME_ORDER_PARENT_ID,
                DoesMatch = AddOrderCommandConstants.SOME_DOES_MATH_FALSE,
                Id = AddOrderCommandConstants.SOME_OTHER_ORDER_ID
            };

            StockMarket.ProcessOrderAsync(
               AddOrderCommandConstants.SOME_PRICE,
               AddOrderCommandConstants.SOME_AMOUNT,
               AddOrderCommandConstants.SOME_SIDE,
               AddOrderCommandConstants.SOME_EXPIRATION_DATE,
               AddOrderCommandConstants.SOME_IS_FILL_AND_KILL,
               AddOrderCommandConstants.SOME_ORDER_PARENT_ID,
               AddOrderCommandConstants.SOME_DOES_MATH_FALSE,
               AddOrderCommandConstants.SOME_OTHER_ORDER_ID
               ).Returns(ProcessContext);

            AssertBuilder.ShouldReceiveAddOrder(1);
            AssertBuilder.ShouldReceiveProcessOrder(1);
            processContext.IsStockMarcketChanged.Returns(false);

            //Act
            await Sut.Handle(addOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        protected override AddOrderCommand MakeSomeTCommand()
        {
            return new AddOrderCommand
            {
                Amount = AddOrderCommandConstants.SOME_AMOUNT,
                ExpDate = AddOrderCommandConstants.SOME_EXPIRATION_DATE,
                IsFillAndKill = AddOrderCommandConstants.SOME_IS_FILL_AND_KILL,
                Price = AddOrderCommandConstants.SOME_PRICE,
                Side = AddOrderCommandConstants.SOME_SIDE,
                OrderParentId = AddOrderCommandConstants.SOME_ORDER_PARENT_ID,
                DoesMatch = AddOrderCommandConstants.SOME_DOES_MATH_TRUE,
                Id = AddOrderCommandConstants.SOME_ORDER_ID,
            };
        }
    }
}