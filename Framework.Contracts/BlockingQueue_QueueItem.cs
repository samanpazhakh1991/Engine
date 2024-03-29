﻿namespace Framework.Contracts
{
    public partial class BlockingQueue
    {
        public class QueueItem<T> : ICommand
        {
            public Task<T> Completion => completionSource.Task;
            private readonly TaskCompletionSource<T> completionSource;
            private readonly Func<Task<T>> action;
            public QueueItem(Func<Task<T>> action)
            {
                completionSource = new TaskCompletionSource<T>();
                this.action = action;
            }

            private async Task<T> execute()
            {
                var res = await action();
                completionSource.SetResult(res);
                return res;
            }

            async Task<object?> ICommand.Execute()
            {
                return await execute();
            }
        }
    }
}
