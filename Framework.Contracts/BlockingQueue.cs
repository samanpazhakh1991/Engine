using System.Collections.Concurrent;

namespace Framework.Contracts
{
    public partial class BlockingQueue : IAsyncDisposable, IDisposable
    {
        private readonly BlockingCollection<ICommand> blockingCollection;
        private readonly Task mainTask;

        public BlockingQueue()
        {
            blockingCollection = new BlockingCollection<ICommand>();
            mainTask = Task.Run(async () =>
            {
                while (!blockingCollection.IsCompleted)
                {
                    try
                    {
                        var i = blockingCollection.Take();
                        await i.Execute();
                    }
                    catch (Exception ex)
                    {
                        if (ex is ObjectDisposedException or InvalidOperationException)
                            continue;
                    }
                }
            });
        }

        public void Dispose()
        {
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            blockingCollection.CompleteAdding();
            await mainTask;
            blockingCollection.Dispose();
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> function)
        {
            var item = new QueueItem<T>(function);
            blockingCollection.Add(item);

            return item.Completion;
        }
    }
}