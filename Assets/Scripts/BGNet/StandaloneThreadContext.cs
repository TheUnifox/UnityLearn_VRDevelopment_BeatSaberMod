using System;

// Token: 0x02000082 RID: 130
public class StandaloneThreadContext : IStandaloneThreadRunner
{
    // Token: 0x06000562 RID: 1378 RVA: 0x00002273 File Offset: 0x00000473
    public void Run(IStandaloneThreadRunnable runnable)
    {
    }

    // Token: 0x0400021F RID: 543
    [DoesNotRequireDomainReloadInit]
    public static StandaloneThreadContext instance = new StandaloneThreadContext();
}
