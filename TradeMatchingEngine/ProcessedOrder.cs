namespace Domain
{
    public class ProcessedOrder
    {
        public long OrderId { get; set; }

        public IEnumerable<long>? Trades { get; set; }

        public IEnumerable<long>? ModifiedOrders { get; set; }

        public IEnumerable<long>? CanceledOrders { get; set; }
    }
}