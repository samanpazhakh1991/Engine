

namespace SpecFlowTest.StepDefinitions
{
    [Binding]
    public class StockMarketEngineCancelOrderStepDefinitions
    {
        private readonly ScenarioContext context;
        public StockMarketEngineCancelOrderStepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }

        [When(@"I cancel '([^']*)'")]
        public async Task WhenICancel(string order)
        {
            var stockMarketClient = context.Get<StockMarketClient>("smc");
            await stockMarketClient.CancelOrder(context.Get<TestProcessedOrder>($"{order}Response").OrderId);
        }

        [Then(@"The order '([^']*)'  Should Be Canceled")]
        public async Task ThenTheOrderShouldBeCancelled(string order)
        {
            var stockMarketClient = context.Get<StockMarketClient>("smc");
            var addedOrder = await stockMarketClient.GetOrderById(context.Get<TestProcessedOrder>($"{order}Response").OrderId);

            addedOrder.State.OrderState.Should().Be(Domain.Orders.Entities.OrderStates.Cancel);
        }
    }
}
