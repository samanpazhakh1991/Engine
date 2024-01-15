namespace Application.Contract.Commands
{
    public class CreateTradeCommand : ICommand
    {
        public Guid CorrelationId { get; set; }
        public int Amount { get; set; }
        public long BuyOrderId { get; set; }
        public int Price { get; set; }
        public long SellOrderId { get; set; }
    }
}