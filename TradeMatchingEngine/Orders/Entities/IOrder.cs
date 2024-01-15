using Framework.Contracts.Common;

namespace Domain.Orders.Entities
{
    public interface IOrder : IEntity<long>, IOrderInfo
    {
        int DecreaseAmount(int amount);

        OrderStates GetOrderState();

        void SetStateCancelled();

        void SetStateModified();

        void SetStateRegistered();

        void UpdateBy(IOrder order);

        void UpdateBy(int amount, int price, DateTime? expireTime);
    }
}