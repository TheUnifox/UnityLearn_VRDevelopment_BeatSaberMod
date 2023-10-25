using System;
using System.Threading.Tasks;

// Token: 0x0200002E RID: 46
public interface IAsyncComputeManager : IDisposable
{
    // Token: 0x0600018D RID: 397
    void BeginOperation(AsyncComputeOperation asyncComputeOperation);

    // Token: 0x0600018E RID: 398
    Task<T> BeginOperation<T>(AsyncComputeOperation<T> asyncComputeOperation);
}
