using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BGNet.Logging;

// Token: 0x02000003 RID: 3
public abstract class AsyncComputeOperation
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000006 RID: 6 RVA: 0x0000213C File Offset: 0x0000033C
    public double elapsedTime
    {
        get
        {
            return (double)this._stopwatch.ElapsedTicks / (1.0 * (double)Stopwatch.Frequency);
        }
    }

    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000007 RID: 7 RVA: 0x0000215B File Offset: 0x0000035B
    protected bool hasTimedOut
    {
        get
        {
            return this._stopwatch.ElapsedMilliseconds >= (long)this._timeoutMs;
        }
    }

    // Token: 0x06000008 RID: 8 RVA: 0x00002174 File Offset: 0x00000374
    protected AsyncComputeOperation(int timeoutMs)
    {
        this._timeoutMs = timeoutMs;
        this._stopwatch.Start();
    }

    // Token: 0x06000009 RID: 9
    public abstract void Execute(bool disposed);

    // Token: 0x04000004 RID: 4
    private readonly int _timeoutMs;

    // Token: 0x04000005 RID: 5
    private readonly Stopwatch _stopwatch = new Stopwatch();
}

// Token: 0x02000004 RID: 4
public abstract class AsyncComputeOperation<T> : AsyncComputeOperation
{
    // Token: 0x17000003 RID: 3
    // (get) Token: 0x0600000A RID: 10 RVA: 0x00002199 File Offset: 0x00000399
    public Task<T> task
    {
        get
        {
            return this._tcs.Task;
        }
    }

    // Token: 0x0600000B RID: 11 RVA: 0x000021A6 File Offset: 0x000003A6
    protected AsyncComputeOperation(int timeoutMs) : base(timeoutMs)
    {
    }

    // Token: 0x0600000C RID: 12 RVA: 0x000021BC File Offset: 0x000003BC
    public sealed override void Execute(bool disposed)
    {
        if (disposed)
        {
            this.Cancel();
            return;
        }
        if (!this.IsValidRequest())
        {
            return;
        }
        try
        {
            this.Complete(this.Compute());
        }
        catch (Exception ex)
        {
            BGNet.Logging.Debug.LogException(ex, "Exception thrown performing async compute");
            this.Fail(ex);
        }
        finally
        {
            this.Finally();
        }
    }

    // Token: 0x0600000D RID: 13 RVA: 0x00002224 File Offset: 0x00000424
    private bool IsValidRequest()
    {
        if (!base.hasTimedOut)
        {
            return true;
        }
        this._tcs.TrySetException(new TimeoutException("Took too long to generate signature!"));
        return false;
    }

    // Token: 0x0600000E RID: 14 RVA: 0x00002247 File Offset: 0x00000447
    private void Complete(T computeResult)
    {
        this._tcs.TrySetResult(computeResult);
    }

    // Token: 0x0600000F RID: 15 RVA: 0x00002256 File Offset: 0x00000456
    private void Cancel()
    {
        this._tcs.TrySetCanceled();
    }

    // Token: 0x06000010 RID: 16 RVA: 0x00002264 File Offset: 0x00000464
    private void Fail(Exception ex)
    {
        this._tcs.TrySetException(ex);
    }

    // Token: 0x06000011 RID: 17
    protected abstract T Compute();

    // Token: 0x06000012 RID: 18 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void Finally()
    {
    }

    // Token: 0x04000006 RID: 6
    private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();
}
