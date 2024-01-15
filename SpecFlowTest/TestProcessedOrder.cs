using Facade.Contract.Model;

namespace SpecFlowTest
{
    public class GetResult
    {
        public OrderVM State { get; set; }
    }
    public class PostResult
    {
        public TestProcessedOrder State { get; set; }
    }
    public class PutResult
    {
        public TestProcessedOrder State { get; set; }
    }
    public class GetAllTradesResult
    {
        public IEnumerable<TradeVM> State { get; set; }
    }
    public class GetTradeByIdResult
    {
        public TradeVM State { get; set; }
    }

    public class TestProcessedOrder
    {
        public long OrderId { get; set; }
        public IEnumerable<long>? Trades { get; set; }

    }
}
