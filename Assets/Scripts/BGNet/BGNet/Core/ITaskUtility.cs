using System;
using System.Threading;
using System.Threading.Tasks;

namespace BGNet.Core
{
    // Token: 0x020000A3 RID: 163
    public interface ITaskUtility
    {
        // Token: 0x0600061A RID: 1562
        Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken = default(CancellationToken));

        // Token: 0x0600061B RID: 1563
        void Wait(Task task);

        // Token: 0x0600061C RID: 1564
        T Wait<T>(Task<T> task);

        // Token: 0x0600061D RID: 1565
        Task<T2> ContinueWith<T1, T2>(Task<T1> task, Func<Task<T1>, Task<T2>> continuation);

        // Token: 0x0600061E RID: 1566
        Task Run(Action action, CancellationToken cancellationToken = default(CancellationToken));

        // Token: 0x0600061F RID: 1567
        Task Run(Func<Task> func, CancellationToken cancellationToken = default(CancellationToken));

        // Token: 0x06000620 RID: 1568
        Task<T> Run<T>(Func<T> func, CancellationToken cancellationToken = default(CancellationToken));

        // Token: 0x06000621 RID: 1569
        CancellationToken CancellationTokenWithDelay(TimeSpan timeSpan);
    }
}
