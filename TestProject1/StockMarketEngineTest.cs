using Domain;
using Domain.Events;
using Domain.Orders.Entities;
using FluentAssertions;
using Framework.Contracts.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static TradeMatchingEngine.Tests.StockMarketTestData;

namespace TradeMatchingEngine.Tests
{
    public class StockMarketEngineTest : IAsyncDisposable
    {
        private readonly StockMarketMatchEngineStateProxy sut;

        public StockMarketEngineTest()
        {
            sut = new StockMarketMatchEngineStateProxy(SeedData.FinancialInstrumentId, SeedData.FinancialInstrumentStockMarketId);
        }

        [Theory, ClassData(typeof(StockMarketTestData))]
        public async Task StockMarket_ProcessOrder_Test(
            object[] orders,
            TestStockMarketExpectedData expectedData,
            MarketState state,
            OrderEventBase[] orderEventExpectedData,
            OrderMatchedEventBase[] tradeExpectedData
            )
        {
            //Arrange
            _setMarketState(state);
            var order = orders.Cast<SerializableOrderTestData>().Last();
            foreach (var item in orders.Cast<SerializableOrderTestData>().Take(orders.Length == 1 ? 0 : orders.Length - 1))
            {
                switch (item.OperationType)
                {
                    case OperationType.Process:
                        {
                            await sut.ProcessOrderAsync(item.Price, item.Amount, item.Side, item.ExpireTime);
                            break;
                        }

                    case OperationType.Modify:
                        {
                            await sut.ModifyOrder((long)item.OrderId!, item.Price, item.Amount, item.ExpireTime);
                            break;
                        }

                    case OperationType.Cancel:
                        {
                            await sut.CancelOrder((long)item.OrderId!);
                            break;
                        }
                }
            }

            //Act
            await sut.ProcessOrderAsync(order.Price, order.Amount, order.Side, order.ExpireTime, order.IsFillAndKill);

            //Assert
            Assert.True(typeof(StockMarketMatchEngine).IsAssignableTo(typeof(IAggregateRoot)));
            Assert.Equal(orderEventExpectedData.OfType<OrderCreated>().Count(), sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(orderEventExpectedData.OfType<OrderModified>().Count(), sut.DomainEvents.Count(x => x is OrderModified));
            Assert.Equal(orderEventExpectedData.OfType<OrderCanceled>().Count(), sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(tradeExpectedData.OfType<OrderMatched>().Count(), sut.DomainEvents.Count(x => x is OrderMatched));
            Assert.Equal(expectedData.BuyOrders, sut.BuyOrderCount);
            Assert.Equal(expectedData.SellOrders, sut.SellOrderCount);
            Assert.Equal(expectedData.MarketState, sut.State);

            foreach (var item in orderEventExpectedData)
            {
                OrderEventBase @event = null;
                switch (item)
                {
                    case TestOrderCreated:
                        {
                            @event = sut.DomainEvents.OfType<OrderCreated>().First(i => i.Id == item.Id);
                            break;
                        }
                    case OrderModified:
                        @event = sut.DomainEvents.OfType<OrderModified>().First(i => i.Id == item.Id && i.AggregateVersion == item.AggregateVersion);
                        break;
                }

                @event?.Should().BeEquivalentTo(item);
            }

            foreach (var item in tradeExpectedData.Cast<TestOrderMatched>())
            {
                var @event = sut.DomainEvents.OfType<OrderMatched>().
                    First(t => t.BuyOrderId == item.BuyOrderId && t.SellOrderId == item.SellOrderId);

                @event.Should().BeEquivalentTo(item);
            }
        }

        [Fact]
        public async Task ProcessOrderAsync_OrderEnters_Then_Is_Cancelled_Then_BuyOrder_Enters_And_No_Trade_Should_Be_Created_When_Is_In_Open_State()
        {
            //Arrange
            sut.PreOpen();
            sut.Open();
            await sut.ProcessOrderAsync(100, 10, Side.Sell);

            //Act
            await sut.CancelOrder(1);

            //Assert
            Assert.Equal(1, sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(1, sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(MarketState.Open, sut.State);
            Assert.Equal(0, sut.BuyOrderCount);
            Assert.Equal(0, sut.SellOrderCount);

            foreach (var item in sut.DomainEvents)
            {
                OrderEventBase @event = null;
                switch (item)
                {
                    case OrderCreated:
                        {
                            @event = sut.DomainEvents.OfType<OrderCreated>().First();
                            break;
                        }
                    case OrderCanceled:
                        @event = sut.DomainEvents.OfType<OrderCanceled>().First();
                        break;
                }

                @event?.Should().BeEquivalentTo(item);
            }
        }

        [Theory]
        [ClassData(typeof(StockMarketModifyOrderTestData))]
        public async Task ModifyOrderAsync_Modify_Order(
            StockMarketModifyOrderTestData.OrderTestData[] orders,
            StockMarketModifyOrderTestData.StockMarketExpectedData expectedData,
            MarketState state,
            OrderEventBase[] orderEventExpectedData,
            OrderMatchedEventBase[] tradeExpectedData,
            StockMarketModifyOrderTestData.ModifyOrderTestData[] modifyOrderTestData
            )
        {
            //Arrange
            _setMarketState(state);

            foreach (var item in orders)
            {
                await sut.ProcessOrderAsync(item.Price, item.Amount, item.Side);
            }

            var modifiedOrder = modifyOrderTestData[0];

            //Act
            await sut.ModifyOrder(
                modifiedOrder.OrderId,
                modifiedOrder.Price,
                modifiedOrder.Amount,
                modifiedOrder.ExpireTime);

            //Assert
            Assert.True(typeof(StockMarketMatchEngine).IsAssignableTo(typeof(IAggregateRoot)));

            Assert.Equal(orderEventExpectedData.OfType<OrderCreated>().Count(), sut.DomainEvents.Count(x => x is OrderCreated));
            Assert.Equal(orderEventExpectedData.OfType<OrderCanceled>().Count(), sut.DomainEvents.Count(x => x is OrderCanceled));
            Assert.Equal(tradeExpectedData.OfType<OrderMatched>().Count(), sut.DomainEvents.Count(x => x is OrderMatched));

            Assert.Equal(expectedData.BuyOrders, sut.BuyOrderCount);
            Assert.Equal(expectedData.SellOrders, sut.SellOrderCount);
            Assert.Equal(expectedData.MarketState, sut.State);

            foreach (var item in orderEventExpectedData)
            {
                OrderEventBase @event = null;
                switch (item)
                {
                    case OrderCreated:
                        {
                            @event = sut.DomainEvents.OfType<OrderCreated>().First(i => i.Id == item.Id);
                            break;
                        }
                    case OrderCanceled:
                        @event = sut.DomainEvents.OfType<OrderCanceled>().First(i => i.Id == item.Id);
                        break;

                    case OrderModified:
                        @event = sut.DomainEvents.OfType<OrderModified>().First(i => i.Id == item.Id);
                        break;
                }

                @event?.Should().BeEquivalentTo(item);
            }

            foreach (var item in tradeExpectedData)
            {
                sut.DomainEvents.OfType<OrderMatched>().First().Should().BeEquivalentTo<OrderMatched>(new()
                {
                    Amount = item.Amount,
                    BuyOrderId = item.BuyOrderId,
                    SellOrderId = item.SellOrderId,
                    Price = item.Price,
                    AggregateVersion = item.AggregateVersion
                });
            }
        }

        [Fact]
        public async Task CreateTrade_Should_Create_Trade()
        {
            //Arrange
            sut.PreOpen();
            sut.Open();

            //Act
            var trade = await sut.CreateTrade(1, 2, 10, 100);

            //Assert
            Assert.NotNull(trade);

            trade.Should().BeEquivalentTo(new
            {
                SellOrderId = 2,
                BuyOrderId = 1,
                Amount = 10,
                Price = 100,
                Id = 1
            });
        }

        [Fact]
        public async Task Set_Cancel_State_Should_Change_Order_State_To_Cancel_Order()
        {
            //Arrange
            _setMarketState(MarketState.Open);
            var result = await sut.ProcessOrderAsync(100, 10, Side.Sell);
            await sut.CancelOrder(result.Order!.Id);

            //Act
            await sut.CancelOrder(result.Order);

            //Assert
            result.Order.Should().BeEquivalentTo(new
            {
                Id = 1,
                Amount = 10,
                Price = 100,
                Side = Side.Sell,
                OrderState = OrderStates.Cancel
            });
        }

        [Fact]
        public async Task Stock_Market_Order_Modify_Test()
        {
            //Arrange
            _setMarketState(MarketState.Open);
            var result = await sut.ProcessOrderAsync(100, 10, Side.Sell);
            var order = await sut.ModifyOrder(result.Order!.Id, 90, 5, DateTime.MaxValue);

            //Act
            await sut.ModifyOrder(order.Order!, 90, 5, DateTime.MaxValue);

            //Assert
            order.Order.Should().BeEquivalentTo(new
            {
                Id = 2,
                Price = 90,
                Amount = 5,
                Side = Side.Sell
            });
        }

        public async ValueTask DisposeAsync()
        {
            await sut.DisposeAsync();
        }

        private void _setMarketState(MarketState marketState)
        {
            switch (marketState)
            {
                case MarketState.Close:
                    sut.Close();
                    break;

                case MarketState.Open:
                    sut.PreOpen();
                    sut.Open();
                    break;

                case MarketState.PreOpen:
                    sut.PreOpen();
                    break;
            }
        }
    }
}