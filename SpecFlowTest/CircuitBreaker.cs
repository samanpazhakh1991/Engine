namespace SpecFlowTest
{
    public class CircuitBreaker : ICircuitBreaker
    {
        private CircuitBreakerState state;
        public CircuitBreaker()
        {
            state = new CircuitBreakerClosed(this);
        }
        public Task<TOutput> ExecuteService<TInput, TOutput>(TInput input, Func<TInput, Task<TOutput>> func)
        {
            return state.ExecuteService(input, func); ;
        }
        private abstract class CircuitBreakerState : ICircuitBreaker
        {
            protected readonly CircuitBreaker Owner;

            protected CircuitBreakerState(CircuitBreaker owner)
            {
                Owner = owner;
            }

            public abstract Task<TOutput> ExecuteService<TInput, TOutput>(TInput input, Func<TInput, Task<TOutput>> func);

        }
        private class CircuitBreakerClosed : CircuitBreakerState
        {

            private int errorCount; 
            public CircuitBreakerClosed(CircuitBreaker owner)
                : base(owner) { }

            public override async Task<TOutput> ExecuteService<TInput, TOutput>(TInput input, Func<TInput, Task<TOutput>> func)
            {
                try
                {
                    return await func(input);
                }
                catch (Exception)
                {
                    _trackErrors();
                    throw;
                }
            }

            private void _trackErrors() 
            {
                errorCount += 1;
                if (errorCount > Config.CircuitClosedErrorLimit)
                {
                    Owner.state = new CircuitBreakerOpen(Owner);
                }
            }
        }

        private class CircuitBreakerOpen : CircuitBreakerState
        {

            public CircuitBreakerOpen(CircuitBreaker owner)
                : base(owner)
            {
                Task.Run(async () => { 
                    await Task.Delay(Config.CircuitOpenTimeout);
                    owner.state = new CircuitBreakerHalfOpen(owner);
                });
            }

            public override Task<TOutput> ExecuteService<TInput, TOutput>(TInput input, Func<TInput, Task<TOutput>> func)
            {
                throw new Exception("Service is not available");
            }

        }

        private class CircuitBreakerHalfOpen : CircuitBreakerState
        {
            private int successCount ;
            private const string Message = "Call failed when circuit half open";
            public CircuitBreakerHalfOpen(CircuitBreaker owner)
                : base(owner) { }
            
            public override async Task<TOutput> ExecuteService<TInput, TOutput>(TInput input, Func<TInput, Task<TOutput>> func)
            {
                try
                {
                    var result = await func(input);
                    successCount += 1;

                    if (successCount > Config.CircuitHalfOpenSuccessLimit)
                    {
                        Owner.state = new CircuitBreakerClosed(Owner);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    Owner.state = new CircuitBreakerOpen(Owner);
                    throw new Exception(Message, e);
                }
            }
        }

    }
}

