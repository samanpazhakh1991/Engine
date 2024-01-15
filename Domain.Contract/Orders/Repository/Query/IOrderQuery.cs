using Domain.Orders.Entities;
using Framework.Contracts.GenericRepositories;

namespace Domain.Contract.Orders.Repository.Query
{
    public interface IOrderQueryRepository : IQueryRepository<Order, IOrder, long>
    {
        Task<long> GetMaxAsync();
    }
}