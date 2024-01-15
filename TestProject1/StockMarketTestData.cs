using Domain;
using Domain.Events;
using Domain.Orders.Entities;
using Framework.Contracts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace TradeMatchingEngine.Tests
{
    public class StockMarketTestData : IEnumerable<object[]>
    {
        private static readonly DateTime SOME_EXPIRE_TIME = DateTime.Now.AddDays(-1);   

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new object[]
                  {
                      new SerializableOrderTestData(OperationType.Process,90, 15),
                      new SerializableOrderTestData(OperationType.Process,110, 15 ),
                      new SerializableOrderTestData(OperationType.Modify,100, 5, Side.Sell, null,null,1),
                      new SerializableOrderTestData(OperationType.Process,100, 15, Side.Buy)
                  },
                  new TestStockMarketExpectedData(){TradeCount= 1,BuyOrders= 1,SellOrders= 2,MarketState = MarketState.Open,OrderCreated = 3},

                  MarketState.Open,

                  new object[]
                  {
                      new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Register, OriginalAmount = 15, Price=90, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                      new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Register, OriginalAmount = 15, Price=110, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                      new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Sell, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId },

                      new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },
                    //  ----------------------------------------------------------------------------------------------------------------------
                      new TestOrderModified(){Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 8,MarketId =SeedData.FinancialInstrumentStockMarketId },
                      new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Modified, OriginalAmount = 5, Price=100, Side=Side.Sell, AggregateVersion = 9,MarketId =SeedData.FinancialInstrumentStockMarketId },
                   //   ---------------------------------------------------------------------------------------------------------------------------------
                      new TestOrderCanceled(){Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                          OrderState = OrderStates.Cancel, OriginalAmount = 15, Price=90, Side=Side.Sell, AggregateVersion = 4,MarketId =SeedData.FinancialInstrumentStockMarketId },

                  },
                  new object[]{ new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 4,SellOrderId = 3, AggregateVersion = 7} }
            };

            yield return new object[]
           {
               new object[]
                      {
                          new SerializableOrderTestData(OperationType.Process,100, 15),
                          new SerializableOrderTestData(OperationType.Cancel,0, 0, Side.Sell, null,false,1),
                          new SerializableOrderTestData(OperationType.Process,100, 15, Side.Buy)
                      },

                      new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 1,SellOrders= 0,MarketState = MarketState.Open,OrderCreated = 2},
                      MarketState.Open,
                      new OrderEventBase[]
                      {
                          new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                              OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                          new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                              OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 4,MarketId =SeedData.FinancialInstrumentStockMarketId },

                         // ---------------------------------------------------------------------------------------------------------------------------------
                          new TestOrderCanceled(){Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                              OrderState = OrderStates.Cancel, OriginalAmount = 15, Price=90, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                      },
                      new OrderMatchedEventBase[]{}
           };

            yield return new object[]
            {
                
                          new object[]
                          {
                              new SerializableOrderTestData(OperationType.Process,100, 10),
                              new SerializableOrderTestData(OperationType.Process,99, 10, Side.Buy)
                          },

                          new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2},
                          MarketState.Open,

                          new OrderEventBase[]
                          {
                              new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                  OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                              new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                              OrderState = OrderStates.Register, OriginalAmount = 10, Price=99, Side=Side.Buy, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId }
                          },
                          new OrderMatchedEventBase[]{}
            };

            yield return new object[]
            {
                
                                 new object[]
                                 {
                                     new SerializableOrderTestData(OperationType.Process,100, 10),
                                     new SerializableOrderTestData(OperationType.Process,100, 15, Side.Buy, null,true),
                                 },

                                 new  TestStockMarketExpectedData(){TradeCount= 1,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2},
                                 MarketState.Open,
                                 new OrderEventBase[]
                                 {
                                     new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                         OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                     new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = true, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                         OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                     new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                         OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                     new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = true, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                     OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                 },
                                 new OrderMatchedEventBase[] { new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 2,SellOrderId = 1, AggregateVersion = 4}}
            };

            yield return new object[]
            {
               
                                new object[]
                                {
                                    new SerializableOrderTestData(OperationType.Process,100, 10),
                                },
                                new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 0,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 1},
                                MarketState.Open,
                                new OrderEventBase[]
                                {
                                    new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                        OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                },
                                new OrderMatchedEventBase[]{}
            };

            yield return new object[]

            {
                
                                           new object[] { new SerializableOrderTestData(OperationType.Process,100, 10) },
                                           new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 0,SellOrders= 1,MarketState = MarketState.PreOpen,OrderCreated = 1},
                                           MarketState.PreOpen,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId }

                                           },
                                           new OrderMatchedEventBase[]{}
            };

            yield return new object[]
            {
                
                                           new object[] { new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy ) },
                                           new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 1,SellOrders= 0,MarketState = MarketState.PreOpen,OrderCreated = 1},
                                           MarketState.PreOpen,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                           },
                                           new OrderMatchedEventBase[]{}
            };

            yield return new object[]
            {
                
                                           new object[] { new SerializableOrderTestData(OperationType.Process,100, 10 ) },
                                           new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 0,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 1},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                           },
                                           new OrderMatchedEventBase[]{}
            };
            yield return new object[]
            {
                
                                           new object[] { new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy) },

                                           new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 1,SellOrders= 0,MarketState = MarketState.Open,OrderCreated = 1},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                           },
                                           new OrderMatchedEventBase[]{}
            };

            yield return new object[]
            {
                
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy)
                                           },

                                           new  TestStockMarketExpectedData(){TradeCount= 1,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                            OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                           },
                                           new OrderMatchedEventBase[] { new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 2,SellOrderId = 1, AggregateVersion = 4}}
            };

            yield return new object[]
            {
                
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy, SOME_EXPIRE_TIME),
                                               new SerializableOrderTestData(OperationType.Process,100, 10)
                                           },

                                           new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =SOME_EXPIRE_TIME, HasCompleted = false, Id=1, IsExpired = true, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                           },
                                           new OrderMatchedEventBase[]{}
            };

            yield return new object[]
            {
                
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Sell, SOME_EXPIRE_TIME),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy)
                                           },
                                           new  TestStockMarketExpectedData(){TradeCount= 0,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =SOME_EXPIRE_TIME, HasCompleted = false, Id=1, IsExpired = true, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                           },
                                           new OrderMatchedEventBase[]{}
            };

            yield return new object[]
            {

                    new object[]
                    {
                                               new SerializableOrderTestData(OperationType.Process,100, 5, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 10)
                                           },

                                           new  TestStockMarketExpectedData(){TradeCount= 1,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount =10, Price=100, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                           },

                                           new OrderMatchedEventBase[] { new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 1,SellOrderId = 2, AggregateVersion = 4}}
            };

            yield return new object[]
            {

                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 5),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy)
                                           },

                                           new  TestStockMarketExpectedData(){TradeCount= 1,BuyOrders= 1,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 2 },
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 5, Price=100, Side=Side.Sell, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                           },

                                           new OrderMatchedEventBase[] { new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 2,SellOrderId = 1, AggregateVersion = 4 }}
            };

            yield return new object[]
            {

                new object[]
                       {
                           new SerializableOrderTestData(OperationType.Process,100, 10),
                           new SerializableOrderTestData(OperationType.Process,100, 5, Side.Buy),
                           new SerializableOrderTestData(OperationType.Process,100, 5, Side.Buy),
                       },
                       new  TestStockMarketExpectedData(){TradeCount= 2,BuyOrders= 2,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 3},
                       MarketState.Open,
                       new OrderEventBase[]
                       {
                           new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                           new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                           new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 7,MarketId =SeedData.FinancialInstrumentStockMarketId },
                         //  ------------------------------------------------------------------------------------------------------------------------------

                           new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                           new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Modified, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId },

                           new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Modified, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 9,MarketId =SeedData.FinancialInstrumentStockMarketId },

                           new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                               OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 10,MarketId =SeedData.FinancialInstrumentStockMarketId }

                       },

                       new OrderMatchedEventBase[]
                       {
                           new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 2,SellOrderId = 1, AggregateVersion = 4},
                           new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 3,SellOrderId = 1, AggregateVersion = 8}
                       }
            };

            yield return new object[]
            {
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 10 ),
                                               new SerializableOrderTestData(OperationType.Process,100, 15, Side.Buy),
                                           },
                                           new  TestStockMarketExpectedData(){TradeCount= 2,BuyOrders= 1,SellOrders= 2,MarketState = MarketState.Open,OrderCreated = 3},

                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 4,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                            //   --------------------------------------------------------------------------------------------------

                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 7,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion =9,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion =10,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                           },

                                           new OrderMatchedEventBase[]
                                           {
                                               new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 3,SellOrderId = 1, AggregateVersion = 5},
                                               new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 3,SellOrderId = 2, AggregateVersion = 8}
                                           }
            };

            yield return new object[]
            {
                
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 15),
                                           },
                                           new  TestStockMarketExpectedData(){TradeCount= 2,BuyOrders= 2,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 3},

                                           MarketState.Open,

                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion=2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion=3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion=4,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                       //    -------------------------------------------------------------------------------
                                           new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion =6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                                new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion =7,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                                  new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion =9,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                                   new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion =10,MarketId =SeedData.FinancialInstrumentStockMarketId }
                                                   },
                                           new OrderMatchedEventBase[]
                                           {
                                               new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 1,SellOrderId =3, AggregateVersion= 5},
                                               new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 2,SellOrderId = 3, AggregateVersion= 8}
                                           }
            };

            yield return new object[]
            {
                
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 10, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 15),
                                           },
                                           new  TestStockMarketExpectedData(){TradeCount= 2,BuyOrders= 4,SellOrders= 1,MarketState = MarketState.Open,OrderCreated = 5},
                                           MarketState.Open,

                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion=2,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion=3,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion=4,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion=5,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion=6,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                            //   -------------------------------------------------------------------------------------------------------------

                                                new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                 OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion =8,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                                 new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                 OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion =9,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                                  new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                 OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion =11,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                                  new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                      OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Buy, AggregateVersion =12,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                           },

                                           new OrderMatchedEventBase[]
                                           {
                                               new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 1,SellOrderId = 5, AggregateVersion=7},
                                               new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 2,SellOrderId =5, AggregateVersion = 10}
                                           }
            };

            yield return new object[]
         {
             
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 15, Side.Buy),
                                           },

                                           new  TestStockMarketExpectedData(){TradeCount= 2,BuyOrders= 1,SellOrders= 4,MarketState = MarketState.Open,OrderCreated = 5},

                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 4,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 6,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                             //  ------------------------------
                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion =8,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion =9,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion =11,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion =12,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                           },

                                           new OrderMatchedEventBase[]
                                           {
                                               new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 5,SellOrderId = 1, AggregateVersion=7},
                                               new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 5, SellOrderId = 2,AggregateVersion = 10}
                                           }
         };

            yield return new object[]
        {
            
                                           new object[]
                                           {
                                               new SerializableOrderTestData(OperationType.Process,100, 10),
                                               new SerializableOrderTestData(OperationType.Process,100, 15),
                                               new SerializableOrderTestData(OperationType.Process,100, 5),
                                               new SerializableOrderTestData(OperationType.Process,100, 15, Side.Buy),
                                               new SerializableOrderTestData(OperationType.Process,100, 5, Side.Buy),
                                           },

                                           new  TestStockMarketExpectedData(){TradeCount= 3,BuyOrders= 2,SellOrders= 3,MarketState = MarketState.Open,OrderCreated = 5},
                                           MarketState.Open,
                                           new OrderEventBase[]
                                           {
                                               new TestOrderCreated(){ Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion = 2,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion = 3,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=3, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Sell, AggregateVersion =4,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=15, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion = 5,MarketId =SeedData.FinancialInstrumentStockMarketId },
                                               new TestOrderCreated(){ Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Register, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion = 12,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                              // --------------------------------------------------------------------------------------------------
                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion =7,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=1, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 10, Price=100, Side=Side.Sell, AggregateVersion =8,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=4, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Buy, AggregateVersion =10,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=10, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion =11,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=0, ExpireTime =DateTime.MaxValue, HasCompleted = true, Id=5, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 5, Price=100, Side=Side.Buy, AggregateVersion =14,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                               new TestOrderModified(){Amount=5, ExpireTime =DateTime.MaxValue, HasCompleted = false, Id=2, IsExpired = false, IsFillAndKill = false, IsProcessCompletedForCorrelationId = false,OrderParentId = null,
                                                   OrderState = OrderStates.Modified, OriginalAmount = 15, Price=100, Side=Side.Sell, AggregateVersion =15,MarketId =SeedData.FinancialInstrumentStockMarketId },

                                           },

                                           new OrderMatchedEventBase[]
                                           {
                                               new TestOrderMatched(){Amount = 10,Price = 100, BuyOrderId = 4,SellOrderId = 1, AggregateVersion = 6},
                                               new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 4,SellOrderId = 2, AggregateVersion = 9},
                                               new TestOrderMatched(){Amount = 5,Price = 100, BuyOrderId = 5,SellOrderId = 2, AggregateVersion = 13}
                                           }
        };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class ProcessOrderTestData
        {
            public OperationType OperationType { get; set; }
            public long? OrderId { get; set; }
            public int Price { get; set; }
            public int Amount { get; set; }
            public Side Side { get; set; }
            public DateTime? ExpireTime { get; set; }
            public bool IsFillAndKill { get; set; }

        }

        public class SerializableOrderTestData : ProcessOrderTestData, IXunitSerializable
        {
            public SerializableOrderTestData()
            {

            }
            public SerializableOrderTestData(OperationType operationType, int price, int amount, Side side = Domain.Orders.Entities.Side.Sell, DateTime? expireTime = null, bool? isFillAndKill = false, long? id = 0)
            {
                OperationType = operationType;
                OrderId = id;
                Price = price;
                Amount = amount;
                Side = side;
                ExpireTime = expireTime;
                IsFillAndKill = isFillAndKill ?? false;

            }
            public void Deserialize(IXunitSerializationInfo info)
            {
                OperationType = info.GetValue<OperationType>("OperationType");
                OrderId = info.GetValue<long?>("OrderId");
                Price = info.GetValue<int>("Price");
                Amount = info.GetValue<int>("Amount");
                ExpireTime = info.GetValue<DateTime?>("ExpireTime");
                IsFillAndKill = info.GetValue<bool>("IsFillAndKill");
                Side = info.GetValue<Side>("Side");

            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue("OrderId", OrderId);
                info.AddValue("Price", Price);
                info.AddValue("Amount", Amount);
                info.AddValue("ExpireTime", ExpireTime);
                info.AddValue("OperationType", OperationType);
                info.AddValue("Side", Side);
                info.AddValue("IsFillAndKill", IsFillAndKill);
            }
        }

        public enum OperationType
        {
            Process,
            Modify,
            Cancel
        }
    }
}