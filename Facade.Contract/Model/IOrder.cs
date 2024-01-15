using Domain.Orders.Entities;

namespace Facade.Contract.Model
{
    public interface IOrder : IOrderInfo
    {
        long Id { get; }
    }
}