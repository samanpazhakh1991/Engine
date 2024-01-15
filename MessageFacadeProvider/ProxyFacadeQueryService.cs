using Facade.Contract;
using Facade.Contract.Model;
using Framework.Contracts.Common;

namespace MessageFacadeProvider
{
    public class ProxyOrderFacadeQueryService : IOrderQueryFacade
    {
        public async Task<OrderVM> Get(long id)
        {
            var @params = new List<long>
            {
                id
            };

            return await HttpClientRequest.CreateHttpRequest<OrderVM, long>(@params, "Order");
        }

        public async Task<PageResult<OrderVM>> GetAllWithPaging(
            int page,
            int pageSize,
            int currentPage = 1,
            long lastId = 0)
        {
            var @params = new List<long>
            {
                page,
                pageSize,
                currentPage,
                lastId
            };

            return await HttpClientRequest.CreateHttpRequest<PageResult<OrderVM>, long>(@params, "Order");
        }
    }
}