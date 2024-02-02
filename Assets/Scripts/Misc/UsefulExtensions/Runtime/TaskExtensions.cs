using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulExtensions.Task
{
    public static class TaskExtensions
    {
        public static IEnumerator AsIEnumerator(this System.Threading.Tasks.Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception ?? new Exception(task.Status.ToString());
            }
        }

        // https://devblogs.microsoft.com/pfxteam/how-do-i-cancel-non-cancelable-async-operations/
        public static async System.Threading.Tasks.Task<T> WithCancellation<T>(this System.Threading.Tasks.Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await System.Threading.Tasks.Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            }

            return await task;
        }

        public static async System.Threading.Tasks.Task WithCancellation(this System.Threading.Tasks.Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await System.Threading.Tasks.Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            }

            await task;
        }
    }

}
