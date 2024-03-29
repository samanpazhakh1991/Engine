using Facade.Contract.Model;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTest.StepDefinitions
{
    [Binding]
    public class StockMarketMatchingEngineFeature2StepDefinitions : Steps
    {
        private readonly ScenarioContext context;

        public StockMarketMatchingEngineFeature2StepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }

        [BeforeScenario]
        public async Task CancelAllOrders() 
        {
            this.context.Add("smc", new StockMarketClient("https://localhost:7001/api"));

            var client = context.Get<StockMarketClient>("smc");
            await client.CancelAllOrders();
        }

        [Given(@"Order '([^']*)' Has Been Defined")]
        public void GivenSellOrderHasBeenDefined(string order, Table table)
        {
            context.Add(order, table.CreateInstance<OrderVM>());
        }

        [When(@"I Register The Order '([^']*)'")]
        public async Task WhenIRegisterTheSellOrder(string order)
        {
            var client = context.Get<StockMarketClient>("smc");
            var result = await client.ProcessOrder(context.Get<OrderVM>(order));
            context.Add($"{order}Response", result.State);
        }

        [Then(@"Order '([^']*)' Should Be Enqueued")]
        public async Task ThenOrderShouldBeEnqueuedAsync(string order)
        {
            var orderId = context.Get<TestProcessedOrder>($"{order}Response").OrderId;
            var client = context.Get<StockMarketClient>("smc");
            var addedOrderId = await client.GetOrderById(orderId);

            addedOrderId.State.Id.Should().Be(orderId);
        }
    }
}