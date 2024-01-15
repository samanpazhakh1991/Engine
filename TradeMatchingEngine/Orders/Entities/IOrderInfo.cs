namespace Domain.Orders.Entities
{
    public interface IOrderInfo
    {
        int Amount { get; }
        DateTime ExpireTime { get; }
        bool HasCompleted { get; }
        bool IsExpired { get; }
        bool IsFillAndKill { get; }
        long? OrderParentId { get; }
        OrderStates? OrderState { get; }
        int? OriginalAmount { get; }
        int Price { get; }
        Side Side { get; }
        Guid MarketId { get; }
    }
}