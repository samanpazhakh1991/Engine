using Framework.Contracts.Common;

namespace Domain.Trades.Entities
{
    public class Trade : ITrade
    {
        internal Trade(long id, long buyOrderId, long sellOrderId, int amount, int price)
        {
            Id = id;
            BuyOrderId = buyOrderId;
            SellOrderId = sellOrderId;
            Amount = amount;
            Price = price;
        }

        public long Id { get; }

        public long BuyOrderId { get; }

        public long SellOrderId { get; }

        public int Amount { get; }

        public int Price { get; }
    }
}