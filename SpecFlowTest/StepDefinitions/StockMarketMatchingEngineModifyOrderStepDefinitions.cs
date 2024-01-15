using Facade.Contract.Model;

namespace SpecFlowTest.StepDefinitions
{
    [Binding]
    public class StockMarketMatchingEngineModifyOrderStepDefinitions
    {
        private readonly ScenarioContext context;

        public StockMarketMatchingEngineModifyOrderStepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }

        [When(@"I Modify The Order '([^']*)' to '([^']*)'")]
        public async Task WhenIModifyTheOrderTo(string sellOrder, string modifiedOrder)
        {
            var orderId = context.Get<TestProcessedOrder>($"{sellOrder}Response").OrderId;

            var modifiedOrderVm = context.Get<OrderVM>($"{modifiedOrder}");

            var orderVm = new ModifiedOrderVM()
            {
                OrderId = orderId,
                Amount = modifiedOrderVm.Amount,
                ExpDate = modifiedOrderVm.ExpireTime,
                Price = modifiedOrderVm.Price,
            };

            var stockMarketClient = context.Get<StockMarketClient>("smc");
            var response = await stockMarketClient.ModifyOrder(orderVm);

            context.Add($"{modifiedOrder}Response", response);
        }

        [Then(@"The order '([^']*)'  Should Be Found like '([^']*)'")]
        public async Task ThenTheOrderShouldBeFoundLike(string orderSide, string modifiedOrder)
        {
            var result = context.Get<PutResult>($"{modifiedOrder}Response");
            var modifiedOrderVm = context.Get<OrderVM>($"{modifiedOrder}");

            var stockMarketClient = context.Get<StockMarketClient>("smc");
            var response = await stockMarketClient.GetOrderById(result.State.OrderId);

            response.State.Amount.Should().Be(modifiedOrderVm.Amount);
            response.State.Price.Should().Be(modifiedOrderVm.Price);
            response.State.Side.Should().Be(modifiedOrderVm.Side);
        }
    }
}