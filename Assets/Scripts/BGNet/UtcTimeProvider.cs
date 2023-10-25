using System;
using BGNet.Core;

// Token: 0x02000094 RID: 148
public class UtcTimeProvider : ITimeProvider
{
    // Token: 0x060005E5 RID: 1509 RVA: 0x00010335 File Offset: 0x0000E535
    public long GetTimeMs()
    {
        return this.GetTicks() / 10000L;
    }

    // Token: 0x060005E6 RID: 1510 RVA: 0x00010344 File Offset: 0x0000E544
    public long GetTicks()
    {
        return DateTime.UtcNow.Ticks - UtcTimeProvider._epoch.Ticks;
    }

    // Token: 0x04000258 RID: 600
    [DoesNotRequireDomainReloadInit]
    private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // Token: 0x04000259 RID: 601
    [DoesNotRequireDomainReloadInit]
    public static readonly UtcTimeProvider instance = new UtcTimeProvider();

    public float time => ((ITimeProvider)instance).time;
}