namespace Application.Contract.Commands
{
    public class CancelOrderCommand : ICommand
    {
        public long Id { get; set; }

        public Guid CorrelationId { get; set; }

        public bool DoesMatch { get; set; } = true;
    }
}