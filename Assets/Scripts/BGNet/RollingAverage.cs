using System;

// Token: 0x02000078 RID: 120
public class RollingAverage
{
    // Token: 0x170000D4 RID: 212
    // (get) Token: 0x06000512 RID: 1298 RVA: 0x0000D97A File Offset: 0x0000BB7A
    public float currentAverage
    {
        get
        {
            return this._currentAverage;
        }
    }

    // Token: 0x170000D5 RID: 213
    // (get) Token: 0x06000513 RID: 1299 RVA: 0x0000D982 File Offset: 0x0000BB82
    public bool hasValue
    {
        get
        {
            return this._length > 0;
        }
    }

    // Token: 0x06000514 RID: 1300 RVA: 0x0000D98D File Offset: 0x0000BB8D
    public RollingAverage(int window)
    {
        this._buffer = new long[window];
    }

    // Token: 0x06000515 RID: 1301 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
    public void Update(float value)
    {
        long num = this._currentTotal;
        if (this._length == this._buffer.Length)
        {
            num -= this._buffer[this._index];
        }
        long num2 = (long)(value * 1000f);
        this._buffer[this._index] = num2;
        this._index = (this._index + 1) % this._buffer.Length;
        this._length = Math.Min(this._length + 1, this._buffer.Length);
        num += num2;
        this._currentTotal = num;
        this._currentAverage = (float)((double)num / (double)(1000L * (long)this._length));
    }

    // Token: 0x06000516 RID: 1302 RVA: 0x0000DA45 File Offset: 0x0000BC45
    public void Reset()
    {
        this._currentAverage = 0f;
        this._index = 0;
        this._length = 0;
        this._currentTotal = 0L;
    }

    // Token: 0x040001ED RID: 493
    private const long kGranularity = 1000L;

    // Token: 0x040001EE RID: 494
    private long _currentTotal;

    // Token: 0x040001EF RID: 495
    private float _currentAverage;

    // Token: 0x040001F0 RID: 496
    private readonly long[] _buffer;

    // Token: 0x040001F1 RID: 497
    private int _index;

    // Token: 0x040001F2 RID: 498
    private int _length;
}
