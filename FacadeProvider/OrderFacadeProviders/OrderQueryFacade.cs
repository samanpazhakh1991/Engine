using Domain.Contract.Orders.Repository.Query;
using Facade.Contract;
using Facade.Contract.Model;
using Framework.Contracts.Common;
using Mapster;

namespace FacadeProvider.OrderFacadeProviders
{
    public class OrderQueryFacade : IOrderQueryFacade
    {
        private readonly IOrderQueryRepository orderQuery;

        public OrderQueryFacade(IOrderQueryRepository orderQuery)
        {
            this.orderQuery = orderQuery;
        }

        public async Task<OrderVM> Get(long id)
        {
            var result = await orderQuery.Get(o => o.Id == id).ConfigureAwait(false);
            return result.Adapt<OrderVM>();
        }

        public async Task<PageResult<OrderVM>> GetAllWithPaging(
            int page,
            int pageSize,
            int currentPage = 1,
            long lastId = 0)
        {
            var result = await orderQuery
                .GetPaging(page, pageSize, currentPage, lastId)
                .ConfigureAwait(false);

            return result.Adapt<PageResult<OrderVM>>();
        }
    }
}