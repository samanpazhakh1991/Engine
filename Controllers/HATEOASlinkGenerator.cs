using Domain;
using Facade.Contract.Model;
using Hal;
using Hal.Builders;

namespace Controllers
{
    public static class HATEOASlinkGenerator
    {
        public static Resource ProcessOrderLink(ProcessedOrder order)
        {
            var builder = new ResourceBuilder();

            var resource = builder
                .WithState(order)
                .AddSelfLink().WithLinkItem("/Orders")
                .AddLink("find").WithLinkItem("/Orders/{id}", templated: true)
                .AddLink("get").WithLinkItem($"/Orders/{order.OrderId}", "getOrder", type: "GET")
                .AddLink("modify").WithLinkItem($"/Orders/{order.OrderId}", "modifyOrder", type: "PUT")
                .AddLink("cancel").WithLinkItem($"/Orders/{order.OrderId}", "cancelOrder", type: "DELETE")
                .AddLink("cancel").WithLinkItem($"/Orders", "cancelAllOrders", type: "DELETE")
                .AddEmbedded("trades")
                .Resource(new ResourceBuilder()
                .WithState(order.Trades)
                .AddSelfLink().WithLinkItem("/Trades")
                .AddLink("find").WithLinkItem("/Trades/{id}", templated: true, type: "GET")
                .AddLink("get").WithLinkItem("/Trades", "getAllTrades", type: "GET"))
                .Build();

            return resource;
        }

        public static Resource ModifyOrderLink(ProcessedOrder order)
        {
            var builder = new ResourceBuilder();

            var resource = builder
                .WithState(order)
                .AddSelfLink().WithLinkItem("/Orders")
                .AddLink("find").WithLinkItem("/Orders/{id}", templated: true)
                .AddLink("cancel").WithLinkItem($"/Orders/{order.OrderId}", "cancelOrder", type: "DELETE")
                .AddLink("cancel").WithLinkItem($"/Orders", "cancelAllOrders", type: "DELETE")
                .AddLink("get").WithLinkItem($"/Orders/{order.OrderId}", "getOrder", type: "GET")
                .AddLink("add").WithLinkItem($"/Orders", "processOrder", type: "POST")
                .AddEmbedded("trades")
                .Resource(new ResourceBuilder()
                .WithState(order.Trades)
                .AddSelfLink().WithLinkItem("/Trades")
                .AddLink("find").WithLinkItem("/Trades/{id}", templated: true, type: "GET")
                .AddLink("get").WithLinkItem("/Trades", "getAllTrades", type: "GET"))
                .Build();

            return resource;
        }

        public static Resource CancelOrderLink(ProcessedOrder order)
        {
            var builder = new ResourceBuilder();

            var resource = builder
                .WithState(order)
                .AddSelfLink().WithLinkItem("/Orders")
                .AddLink("find").WithLinkItem("/Orders/{id}", templated: true)
                .AddLink("add").WithLinkItem($"/Orders", "processOrder", type: "POST")
                .Build();

            return resource;
        }

        public static Resource CancelAllOrdersLink(ProcessedOrder order)
        {
            var builder = new ResourceBuilder();

            var resource = builder
                .WithState(order)
                .AddSelfLink().WithLinkItem("/Orders")
                .AddLink("add").WithLinkItem($"/Orders", "processOrder", type: "POST")
                .Build();

            return resource;
        }

        public static Resource GetOrderLink(OrderVM order)
        {
            var builder = new ResourceBuilder();

            var resource = builder
                .WithState(order)
                .AddSelfLink().WithLinkItem("/Orders")
                .AddLink("find").WithLinkItem("/Orders/{id}", templated: true)
                .AddLink("cancel").WithLinkItem($"/Orders/{order.Id}", "cancelOrder", type: "DELETE")
                .AddLink("cancel").WithLinkItem($"/Orders", "cancelAllOrders", type: "DELETE")
                .AddLink("modify").WithLinkItem($"/Orders/{order.Id}", "modifyOrder", type: "PUT")
                .Build();

            return resource;
        }

        public static Resource GetAllTradeLink(IEnumerable<Facade.Contract.Model.ITrade> trades)
        {
            var builder = new ResourceBuilder();
            var states = trades.Select(item => new ResourceBuilder()
                    .WithState(item)
                    .AddSelfLink()
                    .WithLinkItem($"/trade/{item.Id}")
                    .Build())
                    .ToList();

            var resource = builder
                .WithState(states)
                .AddSelfLink()
                .WithLinkItem("/trades")
                .AddLink("ea:find")
                .WithLinkItem("/trades/{?id}", templated: true);

            return resource.Build();
        }

        public static Resource GetTradeLink(Facade.Contract.Model.ITrade trade)
        {
            var builder = new ResourceBuilder();

            var resource = builder
                .WithState(trade)
                .AddSelfLink().WithLinkItem("/Trades")
                .Build();

            return resource;
        }
    }
}