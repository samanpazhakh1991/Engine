using Domain;
using Domain.Events;
using Domain.Orders.Entities;
using Framework.Contracts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit.Abstractions;
using static TradeMatchingEngine.Tests.StockMarketModifyOrderTestData;

namespace TradeMatchingEngine.Tests
{
    public class StockMarketModifyOrderTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new object[]
                {
                    new TestOrderTestData
                    {
                        Price=10,
                        Amount=5,
                        Side=Side.Sell,
                        ExpireTime= DateTime.MaxValue
                    },
                },
                new TestStockMarketExpectedData
                {
                    TradeCount=0,
                    OrderCreated= 2,
                    BuyOrders= 0,
                    SellOrders= 1,
                    MarketState= MarketState.Open
                },

                MarketState.Open,
                new object[]
                {
                    new TestOrderCreated(){
                        Amount=5,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=1,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 5,
                        Price=10,
                        Side=Side.Sell,
                        AggregateVersion=2,
                       MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                    new TestOrderCreated(){
                        Amount=5,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=2,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 5,
                        Price=15,
                        Side=Side.Sell,
                        AggregateVersion=4,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                     new TestOrderCanceled(){
                         Amount=5,
                         ExpireTime =DateTime.MaxValue,
                         HasCompleted = false,
                         Id=1,
                         IsExpired = false,
                         IsFillAndKill = false,
                         IsProcessCompletedForCorrelationId = false,
                         OrderParentId = null,
                         OrderState = OrderStates.Cancel,
                         OriginalAmount = 5,
                         Price=10,
                         Side=Side.Sell,
                         AggregateVersion=3,
                         MarketId=SeedData.FinancialInstrumentStockMarketId
                     },
                },
                new object[]{},
                new object[]
                {
                    new TestStockMarketModifyOrderTestData
                    {
                        OrderId= 1,
                        Price=15,
                        Amount= 5,
                        ExpireTime= DateTime.MaxValue
                    }
                },
            };

            yield return new object[]
            {
                new object[]
                {
                    new TestOrderTestData{
                        Price= 100,
                        Amount= 10,Side= Side.Sell,
                        ExpireTime= DateTime.MaxValue
                    },
                    new TestOrderTestData
                    {
                        Price=90,
                        Amount= 10,
                        Side=Side.Buy,
                        ExpireTime= DateTime.MaxValue
                    },
                },
                new TestStockMarketExpectedData
                {
                   TradeCount=  1,
                   OrderCreated= 2,
                   BuyOrders= 1,
                   SellOrders=1,
                   MarketState= MarketState.Open
                },
                MarketState.Open,
                new object[]
                {
                    new TestOrderCreated(){
                        Amount=10,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=1,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 10,
                        Price=100,
                        Side=Side.Sell,
                        AggregateVersion=2,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                    new TestOrderModified(){
                        Amount=0,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = true,
                        Id=1,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Modified,
                        OriginalAmount = 10,
                        Price=100,
                        Side=Side.Sell,
                        AggregateVersion=8,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                    new TestOrderCreated(){
                        Amount=10,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=2,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 10,
                        Price=90,
                        Side=Side.Buy,
                        AggregateVersion=3,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                     new TestOrderCanceled(){
                         Amount=10,
                         ExpireTime =DateTime.MaxValue,
                         HasCompleted = false,
                         Id=2,
                         IsExpired = false,
                         IsFillAndKill = false,
                         IsProcessCompletedForCorrelationId = false,
                         OrderParentId = null,
                         OrderState = OrderStates.Cancel,
                         OriginalAmount = 10,
                         Price=90,
                         Side=Side.Buy,
                         AggregateVersion=4,
                         MarketId=SeedData.FinancialInstrumentStockMarketId
                     },

                     new TestOrderCreated(){
                        Amount=10,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=3,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 10,
                        Price=100,
                        Side=Side.Buy,
                        AggregateVersion=5,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                      new TestOrderModified(){
                        Amount=0,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = true,
                        Id=3,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Modified,
                        OriginalAmount = 10,
                        Price=100,
                        Side=Side.Buy,
                        AggregateVersion=7,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },
                },
                new object[]{
                     new TestOrderMatched()
                     {
                         Amount=10,
                         BuyOrderId=3,
                         IsProcessCompletedForCorrelationId=false,
                         Price=100,
                         SellOrderId=1,
                         AggregateVersion=6,
                     }
                },
                new object[]
                {
                    new TestStockMarketModifyOrderTestData
                    {
                       OrderId=  2,
                       Price=   100,
                       Amount=    10,
                       ExpireTime= DateTime.MaxValue
                    }
                },
            };

            yield return new object[]
            {
                new object[]
                {
                    new TestOrderTestData
                    {
                        Price=100,
                        Amount= 10,
                        Side= Side.Sell,
                        ExpireTime= DateTime.MaxValue
                    },
                    new TestOrderTestData
                    {
                        Price=90,
                        Amount= 10,
                        Side= Side.Buy,
                        ExpireTime= DateTime.MaxValue
                    },
                },
                new TestStockMarketExpectedData{
                    TradeCount= 1,
                    OrderCreated= 2,
                    BuyOrders= 1,
                   SellOrders= 1,
                    MarketState= MarketState.Open
                },
                MarketState.Open,
                new object[]
                {
                    new TestOrderCreated(){
                        Amount=10,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=1,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 10,
                        Price=100,
                        Side=Side.Sell,
                        AggregateVersion=2,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                    new TestOrderModified(){
                        Amount=5,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=1,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Modified,
                        OriginalAmount = 10,
                        Price=100,
                        Side=Side.Sell,
                        AggregateVersion=8,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                    new TestOrderCreated(){
                        Amount=10,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=2,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 10,
                        Price=90,
                        Side=Side.Buy,
                        AggregateVersion=3,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                     new TestOrderCanceled(){
                         Amount=10,
                         ExpireTime =DateTime.MaxValue,
                         HasCompleted = false,
                         Id=2,
                         IsExpired = false,
                         IsFillAndKill = false,
                         IsProcessCompletedForCorrelationId = false,
                         OrderParentId = null,
                         OrderState = OrderStates.Cancel,
                         OriginalAmount = 10,
                         Price=90,
                         Side=Side.Buy,
                         AggregateVersion=4,
                         MarketId=SeedData.FinancialInstrumentStockMarketId
                     },

                     new TestOrderCreated(){
                        Amount=5,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = false,
                        Id=3,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Register,
                        OriginalAmount = 5,
                        Price=100,
                        Side=Side.Buy,
                        AggregateVersion=5,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },

                      new TestOrderModified(){
                        Amount=0,
                        ExpireTime =DateTime.MaxValue,
                        HasCompleted = true,
                        Id=3,
                        IsExpired = false,
                        IsFillAndKill = false,
                        IsProcessCompletedForCorrelationId = false,
                        OrderParentId = null,
                        OrderState = OrderStates.Modified,
                        OriginalAmount = 5,
                        Price=100,
                        Side=Side.Buy,
                        AggregateVersion=7,
                        MarketId=SeedData.FinancialInstrumentStockMarketId
                    },
                },
                new object[]{
                     new TestOrderMatched()
                     {
                         Amount=5,
                         BuyOrderId=3,
                         IsProcessCompletedForCorrelationId=false,
                         Price=100,
                         SellOrderId=1,
                         AggregateVersion=6
                     }
                },
                new object[]
                {
                    new TestStockMarketModifyOrderTestData
                    {
                        OrderId= 2,
                        Price =100,
                        Amount=5,
                        ExpireTime=DateTime.MaxValue
                    }
                },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class ModifyOrderTestData
        {
            public long OrderId { get; set; }
            public int Price { get; set; }
            public int Amount { get; set; }
            public DateTime? ExpireTime { get; set; }
        }

        public class OrderTestData
        {
            public int Price { get; set; }
            public int Amount { get; set; }
            public Side Side { get; set; }
            public DateTime? ExpireTime { get; set; }
        }

        public class StockMarketExpectedData
        {
            public int TradeCount { get; set; }
            public int OrderCreated { get; set; }
            public int BuyOrders { get; set; }
            public int SellOrders { get; set; }
            public MarketState MarketState { get; set; }
        }
    }

    public class TestStockMarketModifyOrderTestData : ModifyOrderTestData, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            OrderId = info.GetValue<long>("OrderId");
            Price = info.GetValue<int>("Price");
            Amount = info.GetValue<int>("Amount");
            ExpireTime = info.GetValue<DateTime>("ExpireTime");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("OrderId", OrderId);
            info.AddValue("Price", Price);
            info.AddValue("Amount", Amount);
            info.AddValue("ExpireTime", ExpireTime);
        }
    }

    public class TestOrderTestData : OrderTestData, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            Price = info.GetValue<int>("Price");
            Amount = info.GetValue<int>("Amount");
            ExpireTime = info.GetValue<DateTime>("ExpireTime");
            Side = info.GetValue<Side>("Side");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Side", Side);
            info.AddValue("Price", Price);
            info.AddValue("Amount", Amount);
            info.AddValue("ExpireTime", ExpireTime);
        }
    }

    public class TestStockMarketExpectedData : StockMarketExpectedData, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            TradeCount = info.GetValue<int>("TradeCount");
            OrderCreated = info.GetValue<int>("OrderCreated");
            BuyOrders = info.GetValue<int>("BuyOrders");
            SellOrders = info.GetValue<int>("SellOrders");
            MarketState = info.GetValue<MarketState>("MarketState");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("MarketState", MarketState);
            info.AddValue("SellOrders", SellOrders);
            info.AddValue("BuyOrders", BuyOrders);
            info.AddValue("TradeCount", TradeCount);
            info.AddValue("OrderCreated", OrderCreated);
        }
    }

    public class TestOrderCanceled : OrderCanceled, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            Amount = info.GetValue<int>("Amount");
            ExpireTime = info.GetValue<DateTime>("ExpireTime");
            Id = info.GetValue<long>("Id");
            IsExpired = info.GetValue<bool>("IsExpired");
            IsFillAndKill = info.GetValue<bool>("IsFillAndKill");
            OrderParentId = info.GetValue<long?>("OrderParentId");
            OrderState = info.GetValue<OrderStates>("OrderState");
            OriginalAmount = info.GetValue<int>("OriginalAmount");
            Price = info.GetValue<int>("Price");
            Side = info.GetValue<Side>("Side");
            HasCompleted = info.GetValue<bool>("HasCompleted");
            AggregateVersion = info.GetValue<long>("AggregateVersion");
            MarketId = Guid.Parse(info.GetValue<string>("MarketId"));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Amount", Amount);
            info.AddValue("ExpireTime", ExpireTime);
            info.AddValue("Id", Id);
            info.AddValue("IsExpired", IsExpired);
            info.AddValue("IsFillAndKill", IsFillAndKill);
            info.AddValue("OrderParentId", OrderParentId);
            info.AddValue("OrderState", OrderState);
            info.AddValue("OriginalAmount", OriginalAmount);
            info.AddValue("Price", Price);
            info.AddValue("Side", Side);
            info.AddValue("HasCompleted", HasCompleted);
            info.AddValue("AggregateVersion", AggregateVersion);
            info.AddValue("MarketId", MarketId);
            info.AddValue("MarketId", MarketId.ToString("N"));
        }
    }

    public class TestOrderModified : OrderModified, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            Amount = info.GetValue<int>("Amount");
            ExpireTime = info.GetValue<DateTime>("ExpireTime");
            Id = info.GetValue<long>("Id");
            IsExpired = info.GetValue<bool>("IsExpired");
            IsFillAndKill = info.GetValue<bool>("IsFillAndKill");
            OrderParentId = info.GetValue<long?>("OrderParentId");
            OrderState = info.GetValue<OrderStates>("OrderState");
            OriginalAmount = info.GetValue<int>("OriginalAmount");
            Price = info.GetValue<int>("Price");
            Side = info.GetValue<Side>("Side");
            HasCompleted = info.GetValue<bool>("HasCompleted");
            AggregateVersion = info.GetValue<long>("AggregateVersion");
            MarketId = Guid.Parse(info.GetValue<string>("MarketId"));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Amount", Amount);
            info.AddValue("ExpireTime", ExpireTime);
            info.AddValue("Id", Id);
            info.AddValue("IsExpired", IsExpired);
            info.AddValue("IsFillAndKill", IsFillAndKill);
            info.AddValue("OrderParentId", OrderParentId);
            info.AddValue("OrderState", OrderState);
            info.AddValue("OriginalAmount", OriginalAmount);
            info.AddValue("Price", Price);
            info.AddValue("Side", Side);
            info.AddValue("HasCompleted", HasCompleted);
            info.AddValue("AggregateVersion", AggregateVersion);
            info.AddValue("MarketId", MarketId.ToString("N"));
        }
    }

    public class TestOrderCreated : OrderCreated, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            Amount = info.GetValue<int>("Amount");
            ExpireTime = info.GetValue<DateTime>("ExpireTime");
            Id = info.GetValue<long>("Id");
            IsExpired = info.GetValue<bool>("IsExpired");
            IsFillAndKill = info.GetValue<bool>("IsFillAndKill");
            OrderParentId = info.GetValue<long?>("OrderParentId");
            OrderState = info.GetValue<OrderStates>("OrderState");
            OriginalAmount = info.GetValue<int>("OriginalAmount");
            Price = info.GetValue<int>("Price");
            Side = info.GetValue<Side>("Side");
            HasCompleted = info.GetValue<bool>("HasCompleted");
            AggregateVersion = info.GetValue<long>("AggregateVersion");
            MarketId = Guid.Parse(info.GetValue<string>("MarketId"));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Amount", Amount);
            info.AddValue("ExpireTime", ExpireTime);
            info.AddValue("Id", Id);
            info.AddValue("IsExpired", IsExpired);
            info.AddValue("IsFillAndKill", IsFillAndKill);
            info.AddValue("OrderParentId", OrderParentId);
            info.AddValue("OrderState", OrderState);
            info.AddValue("OriginalAmount", OriginalAmount);
            info.AddValue("Price", Price);
            info.AddValue("Side", Side);
            info.AddValue("HasCompleted", HasCompleted);
            info.AddValue("AggregateVersion", AggregateVersion);
            info.AddValue("MarketId", MarketId.ToString("N"));
        }
    }

    public class TestOrderMatched : OrderMatched, IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            Amount = info.GetValue<int>("Amount"); ;
            SellOrderId = info.GetValue<long>("SellOrderId");
            BuyOrderId = info.GetValue<long>("BuyOrderId");
            Price = info.GetValue<int>("Price");
            AggregateVersion = info.GetValue<long>("AggregateVersion");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Amount", Amount);
            info.AddValue("SellOrderId", SellOrderId);
            info.AddValue("BuyOrderId", BuyOrderId);
            info.AddValue("Price", Price);
            info.AddValue("AggregateVersion", AggregateVersion);
        }
    }
}