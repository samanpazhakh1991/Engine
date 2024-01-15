using Facade.Contract.Model;
using Framework.Contracts.Common;

namespace Facade.Contract
{
    public interface IOrderQueryFacade
    {
        Task<OrderVM> Get(long id);

        Task<PageResult<OrderVM>> GetAllWithPaging(int page, int pageSize, int currentPage = 1, long lastId = 0);
    }
}