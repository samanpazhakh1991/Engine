namespace Messages
{
    public class AddOrderCommandMessage : ICommand
    {
        public Side Side { get; set; }

        public int Price { get; set; }

        public int Amount { get; set; }

        public bool? IsFillAndKill { get; set; } = false;

        public DateTime ExpireTime { get; set; }

        public Guid CorrelationId { get; set; }
    }
}