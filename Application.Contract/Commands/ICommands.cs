namespace Application.Contract.Commands
{
    public interface ICommand
    {
        Guid CorrelationId { get; set; }
    }
}