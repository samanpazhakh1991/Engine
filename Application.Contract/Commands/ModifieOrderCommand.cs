namespace Application.Contract.Commands
{
    public class ModifyOrderCommand : ICommand
    {
        public long OrderId { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public DateTime? ExpDate { get; set; }
        public Guid CorrelationId { get; set; }
        public bool DoesMatch { get; set; } = true;
        public long Id { get; set; }
    }
}