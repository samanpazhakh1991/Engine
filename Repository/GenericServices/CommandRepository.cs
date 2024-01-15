using Framework.Contracts.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.GenericServices
{
    public class CommandRepository<T, TInterface, TId> : ICommandRepository<T, TInterface, TId> where T : class, TInterface
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public CommandRepository(DbContext dbcontext)
        {
            _dbContext = dbcontext;
            _dbSet = dbcontext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task Add(TInterface order)
        {
            await Add((T)order).ConfigureAwait(false);
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public async Task Delete(TId id)
        {
            var entiryToRemove = await _dbSet.FindAsync(id).ConfigureAwait(false);
            _dbSet.Remove(entiryToRemove);
        }

        public async Task<TInterface> Find(TId id)
        {
            var res = await _dbSet.FindAsync(id).ConfigureAwait(false);

            return res;
        }
    }
}