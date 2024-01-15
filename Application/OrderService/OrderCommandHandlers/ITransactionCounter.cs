namespace Application.OrderService.OrderCommandHandlers
{
    public interface ITransactionCounter
    {
        int Counter { get; }

        int Decrement();

        int Increment();
    }
}