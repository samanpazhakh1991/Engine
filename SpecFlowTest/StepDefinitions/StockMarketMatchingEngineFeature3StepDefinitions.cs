using Facade.Contract.Model;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTest.StepDefinitions
{
    [Binding]
    public class StockMarketMatchingEngineFeature3StepDefinitions : Steps
    {
        private readonly ScenarioContext context;

        public StockMarketMatchingEngineFeature3StepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }

        [Given(@"Order '([^']*)' Has Been Registered")]
        public void GivenOrderHasBeenRegistered(string orderSide, Table table)  
        {
            var table1 = new Table("Side", "Price", "Amount", "IsFillAndKill", "ExpireTime");
            foreach (var row in table.Rows)
            {
                table1.AddRow(row[0], row[1], row[2], row[3], row[4]);
            }

            Given("Order 'SellOrder' Has Been Defined", table1);
            When("I Register The Order 'SellOrder'");
            Then("Order 'SellOrder' Should Be Enqueued");
        }

        [Then(@"The following '([^']*)' will be created")]
        public async Task ThenTheFollowingWillBeCreated(string tradeSide, Table table)
        {
            var stockMarketClient = context.Get<StockMarketClient>("smc");

            var order = context.Get<TestProcessedOrder>($"BuyOrderResponse");

            foreach (var tradeId in order.Trades)
            {
                var tradeResult = await stockMarketClient.GetTradeById(tradeId);

                tradeResult.State.Amount.Should().Be(table.CreateInstance<TradeVM>().Amount);

                tradeResult.State.Price.Should().Be(table.CreateInstance<TradeVM>().Price);
            }
        }

        [Then(@"Order '([^']*)' Should Be Modified  like this")]
        public async Task ThenOrderShouldBeModifiedLikeThis(string order, Table table)
        {
            var buyOrderId = context.Get<TestProcessedOrder>($"{order}Response").OrderId;

            var stockMarketClient = context.Get<StockMarketClient>("smc");
            var response = await stockMarketClient.GetOrderById(buyOrderId);

            response.State.Id.Should().Be(buyOrderId);
            response.State.Amount.Should().Be(table.CreateInstance<OrderVM>().Amount);
            response.State.Price.Should().Be(table.CreateInstance<OrderVM>().Price);
        }
    }
}