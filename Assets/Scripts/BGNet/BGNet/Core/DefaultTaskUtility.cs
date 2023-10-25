using System;
using System.Threading;
using System.Threading.Tasks;

namespace BGNet.Core
{
    // Token: 0x0200009E RID: 158
    public class DefaultTaskUtility : ITaskUtility
    {
        // Token: 0x0600060C RID: 1548 RVA: 0x0001092E File Offset: 0x0000EB2E
        public Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Delay(timeSpan, cancellationToken);
        }

        // Token: 0x0600060D RID: 1549 RVA: 0x00010937 File Offset: 0x0000EB37
        public CancellationToken CancellationTokenWithDelay(TimeSpan timeSpan)
        {
            return new CancellationTokenSource(timeSpan).Token;
        }

        // Token: 0x0600060E RID: 1550 RVA: 0x00010944 File Offset: 0x0000EB44
        public void Wait(Task task)
        {
            task.Wait();
        }

        // Token: 0x0600060F RID: 1551 RVA: 0x0001094C File Offset: 0x0000EB4C
        public T Wait<T>(Task<T> task)
        {
            return task.Result;
        }

        // Token: 0x06000610 RID: 1552 RVA: 0x00010954 File Offset: 0x0000EB54
        public Task<T2> ContinueWith<T1, T2>(Task<T1> task, Func<Task<T1>, Task<T2>> continuation)
        {
            return task.ContinueWith<Task<T2>>(continuation).Unwrap<T2>();
        }

        // Token: 0x06000611 RID: 1553 RVA: 0x00010962 File Offset: 0x0000EB62
        public Task Run(Action action, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(action, cancellationToken);
        }

        // Token: 0x06000612 RID: 1554 RVA: 0x0001096B File Offset: 0x0000EB6B
        public Task Run(Func<Task> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(func, cancellationToken);
        }

        // Token: 0x06000613 RID: 1555 RVA: 0x00010974 File Offset: 0x0000EB74
        public Task<T> Run<T>(Func<T> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run<T>(func, cancellationToken);
        }

        // Token: 0x04000266 RID: 614
        [DoesNotRequireDomainReloadInit]
        public static readonly DefaultTaskUtility instance = new DefaultTaskUtility();
    }
}
