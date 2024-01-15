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
    public class ModifyOrderCommandHandlerTest : CommandHandlerTest<ModifyOrderCommandHandler, ModifyOrderCommand>
    {
        private IStockMarketMatchingEngineProcessContext processContext;

        public ModifyOrderCommandHandlerTest()
        {
            processContext = Substitute.For<IStockMarketMatchingEngineProcessContext>();

            StockMarketFactoryMock.GetStockMarket(
                OrderQueryRepositoryMock,
                TradeQueryRepositoryMock,
                StockMarketQueryRepositoryMock).Returns(StockMarket);

            StockMarket
           .ModifyOrder(
               ModifyOrderCommandConstants.SOME_ORDER_ID,
               ModifyOrderCommandConstants.SOME_PRICE,
               ModifyOrderCommandConstants.SOME_AMOUNT,
               ModifyOrderCommandConstants.SOME_EXPIRATION_DATE,
               ModifyOrderCommandConstants.SOME_DOES_MATCH_IS_TRUE,
               Arg.Any<Func<long, Task<IOrder>>>()
              )
             .Returns(processContext);
        }

        [Fact]
        public async Task Handle_Should_Call_ModifyOrder_And_Find_StockMarket_When_DoesMatch_Is_True()
        {
            //Arrange
            var modifyOrderCommand = MakeSomeTCommand();

            AssertBuilder.ShouldReceiveModifyOrder(1);
            AssertBuilder.ShouldReceiveFindStockMarket(1);

            processContext.IsStockMarcketChanged.Returns(true);

            //Act
            await Sut.Handle(modifyOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        [Fact]
        public async Task Handle_Should_Call_ModifyOrder_And_Find_StockMarket_When_DoesMatch_Is_False()
        {
            //Arrange
            var modifyOrderCommand = new ModifyOrderCommand()
            {
                Amount = ModifyOrderCommandConstants.SOME_AMOUNT,
                ExpDate = ModifyOrderCommandConstants.SOME_EXPIRATION_DATE,
                OrderId = ModifyOrderCommandConstants.SOME_ORDER_ID,
                Price = ModifyOrderCommandConstants.SOME_PRICE,
                DoesMatch = ModifyOrderCommandConstants.SOME_DOES_MATCH_IS_FALSE,
                Id = ModifyOrderCommandConstants.SOME_ORDER_ID,
            };

            AssertBuilder.ShouldReceiveModifyOrder(1);

            processContext.IsStockMarcketChanged.Returns(false);

            //Act
            await Sut.Handle(modifyOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        protected override ModifyOrderCommand MakeSomeTCommand()
        {
            return new ModifyOrderCommand()
            {
                Amount = ModifyOrderCommandConstants.SOME_AMOUNT,
                ExpDate = ModifyOrderCommandConstants.SOME_EXPIRATION_DATE,
                OrderId = ModifyOrderCommandConstants.SOME_ORDER_ID,
                Price = ModifyOrderCommandConstants.SOME_PRICE,
                DoesMatch = ModifyOrderCommandConstants.SOME_DOES_MATCH_IS_TRUE,
                Id = ModifyOrderCommandConstants.SOME_ORDER_ID
            };
        }
    }
}