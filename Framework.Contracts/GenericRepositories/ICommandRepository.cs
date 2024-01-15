namespace Framework.Contracts.GenericRepositories
{
    public interface ICommandRepository<in T, TInterface, TId> where T : TInterface
    {
        Task<TInterface> Find(TId id);

        Task Add(T entity);

        Task Add(TInterface entity);

        Task Delete(TId id);

        void Attach(T entity);
    }
}