using Framework.Contracts.Common;
using System.Linq.Expressions;

namespace Framework.Contracts.GenericRepositories
{
    public interface IQueryRepository<T, TInterface, TId>
    {
        Task<TInterface?> Get(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<TInterface>> GetAll(Expression<Func<T, bool>>? predicate = null);

        Task<TId> GetMax(Expression<Func<T, TId?>> selector);

        Task<PageResult<T>> GetPaging(int page, int pageSize, int currentPage, long lastId);
    }
}