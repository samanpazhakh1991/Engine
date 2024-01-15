namespace Framework.Contracts.Events
{
    public interface IDispatcher
    {
        public void Dispatch<T>(T domainEvent) where T : class, IDomainEvent;
    }
}
