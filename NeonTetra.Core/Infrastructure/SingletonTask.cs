using System;
using System.Threading.Tasks;

namespace NeonTetra.Core.Infrastructure
{
    /// <summary>
    ///     Ensures that the action never executes concurrently. Multiple concurrent invokations wait for the single action
    ///     execution to complete.
    /// </summary>
    /// <remarks>
    ///     This is based on sample source code suggested by Stephen Cleary:
    ///     https://codereview.stackexchange.com/questions/16150/singleton-task-running-using-tasks-await-peer-review-challenge
    /// </remarks>
    public sealed class SingletonTask
    {
        private readonly object _lockObject;
        private readonly Func<Task> _taskFactory;

        private Task _runningTask;

        public SingletonTask(Func<Task> taskFactory)
        {
            _lockObject = new object();
            _taskFactory = taskFactory;
        }

        public SingletonTask(Action action)
            : this(() => Task.Run(action))
        {
        }

        public Task RunAsync()
        {
            Task result;
            TaskCompletionSource<object> tcs;

            lock (_lockObject)
            {
                if (_runningTask != null) return _runningTask;

                tcs = new TaskCompletionSource<object>();
                _runningTask = result = StartNewTask(tcs.Task);
            }

            tcs.SetResult(null);
            return result;
        }

        private async Task StartNewTask(Task exitLock)
        {
            await exitLock;
            try
            {
                await _taskFactory();
            }
            finally
            {
                lock (_lockObject)
                {
                    _runningTask = null;
                }
            }
        }
    }
}