using Domain;
using Domain.Events;
using Domain.Orders.Entities;
using Framework.Contracts.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TradeMatchingEngine.Tests
{
    public class StockMarketMatchEngineStateTest
    {
        private readonly StockMarketMatchEngineStateProxy sut;

        public StockMarketMatchEngineStateTest()
        {
            sut = new StockMarketMatchEngineStateProxy(SeedData.FinancialInstrumentId, SeedData.FinancialInstrumentStockMarketId);
        }

        [Fact]
        public void StockMarketMatchEngine_Try_To_Open_Market_In_ClosedState_Must_Throw_NotImplemented_Exception()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<NotImplementedException>(() => sut.Open());
        }

        [Fact]
        public void StockMarketMatchEngine_Try_To_Close_Market_In_OpenState_Must_Throw_NotImplemented_Exception()
        {
            //Arrange
            sut.PreOpen();
            sut.Open();

            //Act

            //Assert
            Assert.Throws<NotImplementedException>(() => sut.Close());
        }

        [Fact]
        public void OpenMethod_Should_Change_State_From_PreOpen_To_OpenState()
        {
            //Arrange
            sut.PreOpen();

            //Act
            sut.Open();

            //Assert
            Assert.Equal(MarketState.Open, sut.State);
        }

        [Fact]
        public void CloseMethod_Try_To_Change_MarketState_ToCloseState()
        {
            //Arrange
            sut.PreOpen();

            //Act
            sut.Close();

            //Assert
            Assert.Equal(MarketState.Close, sut.State);
        }

        [Fact]
        public void PreOpen_Try_ToChange_MarketStateOpen_ToPreOpenState()
        {
            //Arrange
            sut.PreOpen();
            sut.Open();

            //Act
            sut.PreOpen();

            //Assert
            Assert.Equal(MarketState.PreOpen, sut.State);
        }

        [Fact]
        public async Task ProcessOrderAsync_Several_SellOrder_Should_Enqueue_When_State_Is_PreOpen()
        {
            //Arrange
            sut.PreOpen();
            await sut.ProcessOrderAsync(100, 10, Side.Sell);
            await sut.ProcessOrderAsync(100, 10, Side.Sell);
            await sut.ProcessOrderAsync(100, 10, Side.Sell);
            await sut.ProcessOrderAsync(120, 10, Side.Sell);

            //Act
            await sut.ProcessOrderAsync(110, 10, Side.Sell);

            //Assert
            Assert.Equal(5, sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderModified));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderMatched));
            Assert.Equal(5, sut.SellOrderCount);
            Assert.Equal(0, sut.BuyOrderCount);
            Assert.Equal(MarketState.PreOpen, sut.State);
        }

        [Fact]
        public async Task ProcessOrderAsync_BuyOrder_Should_Enqueue_When_State_Is_PreOpen()
        {
            //Arrange
            sut.PreOpen();
            sut.Open();
            sut.PreOpen();

            //Act
            await sut.ProcessOrderAsync(10, 5, Side.Buy);

            //Assert
            Assert.Equal(1, sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderModified));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderMatched));
            Assert.Equal(0, sut.SellOrderCount);
            Assert.Equal(1, sut.BuyOrderCount);
            Assert.Equal(MarketState.PreOpen, sut.State);

        }

        [Fact]
        public async Task ProcessOrderAsync_SellOrder_Should_Enqueue_When_State_Is_PreOpen()
        {
            //Arrange
            sut.PreOpen();
            sut.Open();
            sut.PreOpen();

            //Act
            await sut.ProcessOrderAsync(10, 5, Side.Sell);

            //Assert
            Assert.Equal(1, sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderModified));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderMatched));
            Assert.Equal(1, sut.SellOrderCount);
            Assert.Equal(0, sut.BuyOrderCount);
            Assert.Equal(MarketState.PreOpen, sut.State);
        }

        [Fact]
        public async Task ProcessOrderAsync_Call_ModifyOrder_When_State_Is_PreOpen()
        {
            //Arrange
            sut.PreOpen();
            var sellOrderId = await sut.ProcessOrderAsync(100, 10, Side.Sell);

            //Act
            await sut.ModifyOrder(sellOrderId.Order.Id, 110, 10, DateTime.Now.AddDays(1));

            //Assert
            Assert.Equal(2, sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderModified));
            Assert.Equal(1, sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(0, sut.DomainEvents.Count(x => x is OrderMatched));
            Assert.Equal(1, sut.SellOrderCount);
            Assert.Equal(0, sut.BuyOrderCount);
            Assert.Equal(MarketState.PreOpen, sut.State);
        }
    }
}