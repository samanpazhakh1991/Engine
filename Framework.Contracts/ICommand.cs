namespace Framework.Contracts
{
    public interface ICommand
    {
        Task<object?> Execute();
    }
}
