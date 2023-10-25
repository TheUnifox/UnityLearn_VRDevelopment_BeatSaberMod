using System;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000092 RID: 146
public static class TaskExtensions
{
    // Token: 0x060005E3 RID: 1507 RVA: 0x0001028C File Offset: 0x0000E48C
    public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        if (cancellationToken == CancellationToken.None)
        {
            return task;
        }
        TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        cancellationToken.Register(delegate ()
        {
            tcs.TrySetCanceled(cancellationToken);
        });
        global::TaskExtensions.WaitForTask<T>(task, tcs);
        return tcs.Task;
    }

    // Token: 0x060005E4 RID: 1508 RVA: 0x000102F4 File Offset: 0x0000E4F4
    private static async void WaitForTask<T>(Task<T> task, TaskCompletionSource<T> tcs)
    {
        try
        {
            TaskCompletionSource<T> taskCompletionSource = tcs;
            T result = await task;
            taskCompletionSource.TrySetResult(result);
            taskCompletionSource = null;
        }
        catch (TaskCanceledException)
        {
            tcs.TrySetCanceled();
        }
        catch (Exception exception)
        {
            tcs.TrySetException(exception);
        }
    }
}
