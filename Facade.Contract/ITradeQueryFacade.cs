using Facade.Contract.Model;
using Framework.Contracts.Common;

namespace Facade.Contract
{
    public interface ITradeQueryFacade
    {
        Task<IEnumerable<TradeVM>> GetAllTrades();

        Task<TradeVM> GetTrade(long id);

        Task<PageResult<TradeVM>> GetAllTradesWithPaging(int page, int pageSize, int currentPage, long lastId);
    }
}