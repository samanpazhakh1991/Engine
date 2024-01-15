namespace Framework.Contracts.Common
{
    public interface IServiceFactory
    {
        IEnumerable<T> GetServices<T>();
    }
}
