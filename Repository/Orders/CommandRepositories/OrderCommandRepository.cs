using Domain.Contract.Orders.Repository.Command;
using Domain.Orders.Entities;
using Infrastructure.GenericServices;

namespace Infrastructure.Orders.CommandRepositories
{
    public class OrderCommandRepository : CommandRepository<Order, IOrder, long>, IOrderCommandRepository
    {
        private readonly TradeMatchingEngineContext _tradeMatchingEngineContext;

        public OrderCommandRepository(TradeMatchingEngineContext dbcontext) : base(dbcontext)
        {
            _tradeMatchingEngineContext = dbcontext;
        }
    }
}