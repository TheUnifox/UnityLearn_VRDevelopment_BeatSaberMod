using System;
using System.Diagnostics;
using BGNet.Core;

// Token: 0x0200005C RID: 92
public class MonotonicTimeProvider : BGNet.Core.ITimeProvider
{
    // Token: 0x06000416 RID: 1046 RVA: 0x0000A37C File Offset: 0x0000857C
    private MonotonicTimeProvider()
    {
        this._startTicks = UtcTimeProvider.instance.GetTicks();
        this._stopwatch.Start();
    }

    // Token: 0x06000417 RID: 1047 RVA: 0x0000A3CB File Offset: 0x000085CB
    public long GetTimeMs()
    {
        return this.GetTicks() / 10000L;
    }

    // Token: 0x06000418 RID: 1048 RVA: 0x0000A3DA File Offset: 0x000085DA
    public long GetTicks()
    {
        return this._startTicks + (long)((double)this._stopwatch.ElapsedTicks * this._timeSpanTicksPerStopwatchTick);
    }

    // Token: 0x04000169 RID: 361
    private readonly double _timeSpanTicksPerStopwatchTick = 10000000.0 / (double)Stopwatch.Frequency;

    // Token: 0x0400016A RID: 362
    private readonly long _startTicks;

    // Token: 0x0400016B RID: 363
    private readonly Stopwatch _stopwatch = new Stopwatch();

    // Token: 0x0400016C RID: 364
    [DoesNotRequireDomainReloadInit]
    public static readonly MonotonicTimeProvider instance = new MonotonicTimeProvider();

}
