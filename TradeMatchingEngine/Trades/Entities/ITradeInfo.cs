namespace Domain.Trades.Entities
{
    public interface ITradeInfo
    {
        int Amount { get; }
        long BuyOrderId { get; }
        int Price { get; }
        long SellOrderId { get; }
    }
}