namespace Facade.Contract.Model
{
    public class TradeVM : ITrade
    {
        public long Id { get; set; }

        public int Amount { get; set; }

        public long BuyOrderId { get; set; }

        public int Price { get; set; }

        public long SellOrderId { get; set; }
    }
}