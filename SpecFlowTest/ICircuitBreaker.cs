namespace SpecFlowTest
{
    public interface ICircuitBreaker
    {
        Task<TOutput> ExecuteService<TInput, TOutput>(TInput input, Func<TInput, Task<TOutput>> func);
    }
}
