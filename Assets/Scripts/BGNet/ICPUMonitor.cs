using System;

// Token: 0x02000030 RID: 48
public interface ICPUMonitor : IPollable
{
    // Token: 0x17000046 RID: 70
    // (get) Token: 0x06000192 RID: 402
    float utilization { get; }
}
