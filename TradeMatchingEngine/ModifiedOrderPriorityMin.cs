using Domain.Orders.Entities;

namespace Domain
{
    public class ModifiedOrderPriorityMin : IComparer<Order>
    {
        public int Compare(Order? x, Order? y)
        {
            if (x?.Price == y?.Price)
            {
                return (x?.Id ?? 0) > (y?.Id ?? 0) ? 1 : -1;
            }

            if ((x?.Price ?? 0) > (y?.Price ?? 0))
            {
                return 1;
            }

            return -1;
        }
    }
}