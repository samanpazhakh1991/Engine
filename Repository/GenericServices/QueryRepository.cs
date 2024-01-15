using Framework.Contracts.Common;
using Framework.Contracts.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Infrastructure.GenericServices
{
    public class QueryRepository<T, TInterface, TId> : IQueryRepository<T, TInterface, TId> where T : class, IEntity<TId>, TInterface
    {
        protected readonly DbContext _dbContext;
        protected readonly IQueryable<T> _querySet;

        public QueryRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _querySet = dbContext.Set<T>().AsNoTracking();
        }

        public async Task<IEnumerable<TInterface>> GetAll(Expression<Func<T, bool>>? predicate = null)
        {
            return await (predicate != null ? _querySet.Where(predicate) : _querySet).ToListAsync().ConfigureAwait(false);
        }

        public async Task<TInterface?> Get(Expression<Func<T, bool>> predicate)
        {
            return await _querySet.Where(predicate).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<TId> GetMax(Expression<Func<T, TId?>> selector)
        {
            if (_querySet.Any())
            {
                return await _querySet.MaxAsync(selector).ConfigureAwait(false);
            }

            return default(TId);
        }

        public async Task<PageResult<T>> GetPaging(int page, int pageSize, int currentPage = 1, long lastId = 0)
        {
            var result = new PageResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;

            var change = Math.Abs(currentPage - page);
            var skip = (page - 1) * pageSize;

            if (change > 1)
            {
                result.RowCount = _querySet.Count();
                var pageCount = (double)result.RowCount / pageSize;
                result.PageCount = (int)Math.Ceiling(pageCount);
                result.Result = await _querySet.OrderBy(i => i.Id).Skip(skip).Take(pageSize).ToListAsync().ConfigureAwait(false);
                return result;
            }

            if (currentPage > page)
                lastId = (currentPage - 2) * pageSize;

            result.Result = await _querySet.OrderBy(i => i.Id).Where(i => (long)(object)i.Id! > lastId).Take(pageSize).ToListAsync().ConfigureAwait(false);

            return result;
        }
    }
}