namespace Framework.Contracts.Events
{
    public interface IDomainEvent
    {
        Guid CorrelationId { get; set; }
        public bool IsProcessCompletedForCorrelationId { get; set; }
        public long AggregateVersion { get; set; }
    }
}