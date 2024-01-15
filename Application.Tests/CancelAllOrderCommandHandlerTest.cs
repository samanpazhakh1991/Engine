using Application.Contract.Commands;
using Application.OrderService.OrderCommandHandlers;
using Application.Tests.Constants;
using Domain.Orders.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class CancelAllOrderCommandHandlerTest : CommandHandlerTest<CancelAllOrdersCommandHandler, CancelAllOrderCommand>
    {
        public CancelAllOrderCommandHandlerTest()
        {
            StockMarketFactoryMock.GetStockMarket(
                OrderQueryRepositoryMock,
                TradeQueryRepositoryMock,
                StockMarketQueryRepositoryMock
                ).Returns(StockMarket);
        }

        [Fact]
        public async Task Handle_Should_Call_CancelOrderAsync()
        {
            //Arrange
            var cancelOrderCommand = MakeSomeTCommand();

            AssertBuilder.ShouldReceiveGetAllOrder(1);
            AssertBuilder.ShouldReceiveCancelOrder(1);
            AssertBuilder.ShouldReceiveFindStockMarket(1);

            OrderQueryRepositoryMock.GetAll(Arg.Any<Expression<Func<Order, bool>>>()).Returns(new List<TestOrder>()
            {
                new(){
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

            //Act
            await Sut.Handle(cancelOrderCommand);

            //Assert
            AssertBuilder.Assert();
        }

        protected override CancelAllOrderCommand MakeSomeTCommand()
        {
            return new CancelAllOrderCommand();
        }
    }
}