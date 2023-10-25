using System;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000051 RID: 81
public interface IStandaloneMonobehavior
{
    // Token: 0x060002FB RID: 763
    void Dispatch(Action action);

    // Token: 0x060002FC RID: 764
    Task DispatchAsync(Func<Task> action);

    // Token: 0x060002FD RID: 765
    Task RunAsync(IStandaloneThreadRunner runner, CancellationToken cancellationToken);

    // Token: 0x060002FE RID: 766
    void Stop();
}
