using Domain.Contract.Orders.Repository.Query;
using Domain.Orders.Entities;
using Infrastructure.GenericServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Orders.QueryRepositories
{
    public class OrderQueryRepository : QueryRepository<Order, IOrder, long>, IOrderQueryRepository
    {
        private readonly TradeMatchingEngineContext tradeMatchingEngineContext;

        public OrderQueryRepository(TradeMatchingEngineContext tradeMatchingEngineContext)
            : base(tradeMatchingEngineContext)
        {
            this.tradeMatchingEngineContext = tradeMatchingEngineContext;
        }

        public async Task<long> GetMaxAsync()
        {
            return await tradeMatchingEngineContext.Orders.MaxAsync(o => (long?)o.Id) ?? 0;
        }
    }
}