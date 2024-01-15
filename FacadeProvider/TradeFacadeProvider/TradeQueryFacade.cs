using Domain.Contract.Trades.Repository.Query;
using Facade.Contract;
using Facade.Contract.Model;
using Framework.Contracts.Common;
using Mapster;

namespace FacadeProvider.TradeFacadeProvider
{
    public class TradeQueryFacade : ITradeQueryFacade
    {
        private readonly ITradeQueryRepository tradeQuery;

        public TradeQueryFacade(ITradeQueryRepository tradeQuery)
        {
            this.tradeQuery = tradeQuery;
        }

        public async Task<IEnumerable<TradeVM>> GetAllTrades()
        {
            var result = await tradeQuery.GetAll().ConfigureAwait(false);
            return result.Adapt<IEnumerable<TradeVM>>();
        }

        public async Task<PageResult<TradeVM>> GetAllTradesWithPaging(
            int page,
            int pageSize,
            int currentPage,
            long lastId)
        {
            var result = await tradeQuery.GetPaging(
                page,
                pageSize,
                currentPage,
                lastId)
                .ConfigureAwait(false);

            return result.Adapt<PageResult<TradeVM>>();
        }

        public async Task<TradeVM> GetTrade(long id)
        {
            var result = await tradeQuery.Get(t => t.Id == id);

            return result.Adapt<TradeVM>();
        }
    }
}