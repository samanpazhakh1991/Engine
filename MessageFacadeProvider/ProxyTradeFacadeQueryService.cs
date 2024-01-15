using Facade.Contract;
using Facade.Contract.Model;
using Framework.Contracts.Common;

namespace MessageFacadeProvider
{
    public class ProxyTradeFacadeQueryService : ITradeQueryFacade
    {
        public async Task<IEnumerable<TradeVM>> GetAllTrades()
        {
            return await HttpClientRequest.CreateHttpRequest<List<TradeVM>, List<long>>(null, "Trade");
        }

        public async Task<PageResult<TradeVM>> GetAllTradesWithPaging(int page, int pageSize, int currentPage, long lastId)
        {
            var @params = new List<long>
            {
                page,
                pageSize,
                currentPage,
                lastId
            };

            return await HttpClientRequest.CreateHttpRequest<PageResult<TradeVM>, long>(@params, "Trade");
        }

        public async Task<TradeVM> GetTrade(long id)
        {
            var @params = new List<long>
            {
                id
            };

            return await HttpClientRequest.CreateHttpRequest<TradeVM, long>(@params, "Trade");
        }
    }
}