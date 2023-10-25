using System;
using System.Diagnostics;

// Token: 0x0200000A RID: 10
public class CPUMonitor : ICPUMonitor, IPollable
{
    // Token: 0x06000032 RID: 50 RVA: 0x00002844 File Offset: 0x00000A44
    public void PollUpdate()
    {
        long ticks = DateTime.UtcNow.Ticks;
        long ticks2 = this._currentProcess.TotalProcessorTime.Ticks;
        long num = ticks2 - this._lastSampleValue;
        long num2 = ticks - this._lastSampleTime;
        if (num2 <= 0L || (num <= 0L && num2 < 50000000L))
        {
            return;
        }
        if (this._lastSampleTime > 0L)
        {
            double num3 = (double)num / (double)((long)this._processorCount * num2);
            this._utilization.Update((float)num3);
        }
        this._lastSampleTime = ticks;
        this._lastSampleValue = ticks2;
    }

    // Token: 0x17000006 RID: 6
    // (get) Token: 0x06000033 RID: 51 RVA: 0x000028D0 File Offset: 0x00000AD0
    public float utilization
    {
        get
        {
            return this._utilization.currentAverage * 100f;
        }
    }

    // Token: 0x04000014 RID: 20
    private readonly RollingAverage _utilization = new RollingAverage(4);

    // Token: 0x04000015 RID: 21
    private readonly Process _currentProcess = Process.GetCurrentProcess();

    // Token: 0x04000016 RID: 22
    private readonly int _processorCount = Environment.ProcessorCount;

    // Token: 0x04000017 RID: 23
    private long _lastSampleTime;

    // Token: 0x04000018 RID: 24
    private long _lastSampleValue;
}
