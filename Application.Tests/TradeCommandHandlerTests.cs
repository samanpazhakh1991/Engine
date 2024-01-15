using Application.Contract.Commands;
using Application.Tests.Constants;
using Application.TradeService;
using NSubstitute;
using Xunit;

namespace Application.Tests
{
    public class TradeCommandHandlerTests : CommandHandlerTest<TradeCommandHandler, CreateTradeCommand>
    {
        public TradeCommandHandlerTests()
        {
            StockMarketFactoryMock.GetStockMarket
                (OrderQueryRepositoryMock,
                TradeQueryRepositoryMock,
                StockMarketQueryRepositoryMock).Returns(StockMarket);
        }

        [Fact]
        public async void TradeCommandHandler_Should_Call_CreateTrade_And_AddTrade()
        {
            //Arrange
            var createTradeCommand = MakeSomeTCommand();
            AssertBuilder.ShouldReceiveAddTrade(1);
            AssertBuilder.ShouldReceiveCreateTrade(1);

            //Act
            await Sut.Handle(createTradeCommand);

            //Assert
            AssertBuilder.Assert();
        }

        protected override CreateTradeCommand MakeSomeTCommand()
        {
            return new CreateTradeCommand()
            {
                Amount = TradeCommandConstants.SOME_AMOUNT,
                BuyOrderId = TradeCommandConstants.SMOE_BUY_ORDER_ID,
                Price = TradeCommandConstants.SOME_PRICE,
                SellOrderId = TradeCommandConstants.SOME_SELL_ORDER_ID
            };
        }
    }
}