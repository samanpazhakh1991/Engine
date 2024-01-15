using Domain.Orders.Entities;
using Framework.Contracts.GenericRepositories;

namespace Domain.Contract.Orders.Repository.Command
{
    public interface IOrderCommandRepository : ICommandRepository<Order, IOrder, long>
    {
    }
}