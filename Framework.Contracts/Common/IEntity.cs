namespace Framework.Contracts.Common
{
    public interface IEntity<T>
    {
        public T Id { get; }
    }
}