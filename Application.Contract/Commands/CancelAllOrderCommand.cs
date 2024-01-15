namespace Application.Contract.Commands
{
    public class CancelAllOrderCommand : ICommand
    {
        public Guid CorrelationId { get; set; }

        public bool DoesMatch { get; set; } = true;
    }
}