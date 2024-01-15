using Domain.Orders.Entities;

namespace Application.Contract.Commands
{
    public class AddOrderCommand : ICommand
    {
        public int Price { get; set; }
        public int Amount { get; set; }
        public Side Side { get; set; }
        public DateTime? ExpDate { get; set; }
        public bool IsFillAndKill { get; set; }
        public long? OrderParentId { get; set; }
        public Guid CorrelationId { get; set; }
        public bool DoesMatch { get; set; } = true;
        public long Id { get; set; }
    }
}