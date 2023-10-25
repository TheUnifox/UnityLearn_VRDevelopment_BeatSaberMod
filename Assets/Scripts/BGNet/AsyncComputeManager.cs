using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Logging;

// Token: 0x02000002 RID: 2
public class AsyncComputeManager : IAsyncComputeManager, IDisposable
{
    // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    public AsyncComputeManager()
    {
        this._computeThread = new Thread(new ThreadStart(this.ComputeThreadRun))
        {
            Name = "AsyncComputeThread",
            Priority = ThreadPriority.Normal
        };
        this._computeThread.Start();
    }

    // Token: 0x06000002 RID: 2 RVA: 0x000020A2 File Offset: 0x000002A2
    public void Dispose()
    {
        this._disposed = true;
        this._asyncComputeRequests.CompleteAdding();
        this._computeThread.Join();
    }

    // Token: 0x06000003 RID: 3 RVA: 0x000020C1 File Offset: 0x000002C1
    public void BeginOperation(AsyncComputeOperation operation)
    {
        this._asyncComputeRequests.Add(operation);
    }

    // Token: 0x06000004 RID: 4 RVA: 0x000020CF File Offset: 0x000002CF
    public Task<T> BeginOperation<T>(AsyncComputeOperation<T> operation)
    {
        this._asyncComputeRequests.Add(operation);
        return operation.task;
    }

    // Token: 0x06000005 RID: 5 RVA: 0x000020E4 File Offset: 0x000002E4
    private void ComputeThreadRun()
    {
        while (!this._disposed)
        {
            try
            {
                for (; ; )
                {
                    this._asyncComputeRequests.Take().Execute(this._disposed);
                }
            }
            catch (Exception exception)
            {
                if (!this._disposed)
                {
                    Debug.LogException(exception, "Compute Thread threw exception before disposed!");
                }
            }
        }
    }

    // Token: 0x04000001 RID: 1
    private readonly BlockingCollection<AsyncComputeOperation> _asyncComputeRequests = new BlockingCollection<AsyncComputeOperation>();

    // Token: 0x04000002 RID: 2
    private readonly Thread _computeThread;

    // Token: 0x04000003 RID: 3
    private bool _disposed;
}
