namespace MessageFacadeProvider
{
    public class ProcessOrderData
    {
        public ProcessOrderData()
        {
            CancelledOrders = new List<long>();
            Trades = new List<long>();
            ModifiedOrder = new List<long>();
        }

        public long OrderId { get; set; }
        public List<long> CancelledOrders { get; set; }
        public List<long>? Trades { get; set; }
        public List<long>? ModifiedOrder { get; set; }
    }
}