using System;
using LiteNetLib.Utils;

// Token: 0x0200008D RID: 141
public abstract class StateBuffer<TStateTable, TType, TState> where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable> where TType : struct, IConvertible where TState : struct
{
    // Token: 0x060005BD RID: 1469 RVA: 0x0000F63B File Offset: 0x0000D83B
    public StateBuffer(int size, StateBuffer<TStateTable, TType, TState>.InterpolationDelegate interpolator, StateBuffer<TStateTable, TType, TState>.SmoothingDelegate smoother = null)
    {
        this._buffer = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[size];
        this._interpolator = interpolator;
        this._smoother = smoother;
    }

    // Token: 0x060005BE RID: 1470 RVA: 0x0000F65D File Offset: 0x0000D85D
    private int GetPreviousIndex(int offset)
    {
        return (this._currentIndex + this._buffer.Length - offset) % this._buffer.Length;
    }

    // Token: 0x060005BF RID: 1471 RVA: 0x0000F67C File Offset: 0x0000D87C
    protected void InsertState(in TStateTable state, float time)
    {
        if (this._buffer.Length == 0 || time < this._buffer[this.GetPreviousIndex(this._buffer.Length - 1)].time)
        {
            return;
        }
        this._currentIndex = (this._currentIndex + 1) % this._buffer.Length;
        int num = 0;
        while (num < this._buffer.Length - 1 && time < this._buffer[this.GetPreviousIndex(num + 1)].time)
        {
            this._buffer[this.GetPreviousIndex(num)] = this._buffer[this.GetPreviousIndex(num + 1)];
            num++;
        }
        this._buffer[this.GetPreviousIndex(num)] = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable(time, state);
    }

    // Token: 0x060005C0 RID: 1472 RVA: 0x0000F744 File Offset: 0x0000D944
    public TState GetState(TType type, float time)
    {
        float num = this._buffer[this.GetPreviousIndex(0)].time;
        TState tstate = this._buffer[this.GetPreviousIndex(0)].state.GetState(type);
        bool flag = false;
        TState tstate2 = default(TState);
        float smooth = 0f;
        for (int i = 1; i < this._buffer.Length; i++)
        {
            int previousIndex = this.GetPreviousIndex(i);
            if (!this._buffer[previousIndex].isValid)
            {
                break;
            }
            float time2 = this._buffer[previousIndex].time;
            TState state = this._buffer[previousIndex].state.GetState(type);
            if (time >= time2)
            {
                if (flag)
                {
                    return this._smoother(this._interpolator(state, time2, tstate, num, time), tstate2, smooth);
                }
                tstate2 = this._interpolator(state, time2, tstate, num, time);
                if (time > num && num > time2)
                {
                    smooth = (time - num) / (num - time2);
                }
                flag = true;
                if (this._smoother == null)
                {
                    return tstate2;
                }
            }
            num = time2;
            tstate = state;
        }
        if (!flag)
        {
            return tstate;
        }
        return tstate2;
    }

    // Token: 0x060005C1 RID: 1473 RVA: 0x0000F877 File Offset: 0x0000DA77
    public TState GetLatestState(TType type)
    {
        return this._buffer[this._currentIndex].state.GetState(type);
    }

    // Token: 0x060005C2 RID: 1474 RVA: 0x0000F89B File Offset: 0x0000DA9B
    public float GetLatestTime()
    {
        return this._buffer[this._currentIndex].time;
    }

    // Token: 0x060005C3 RID: 1475 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
    public virtual void Clear()
    {
        for (int i = 0; i < this._buffer.Length; i++)
        {
            this._buffer[i] = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable(0f, default(TStateTable));
        }
    }

    // Token: 0x04000234 RID: 564
    private int _currentIndex;

    // Token: 0x04000235 RID: 565
    private readonly StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[] _buffer;

    // Token: 0x04000236 RID: 566
    private readonly StateBuffer<TStateTable, TType, TState>.InterpolationDelegate _interpolator;

    // Token: 0x04000237 RID: 567
    private readonly StateBuffer<TStateTable, TType, TState>.SmoothingDelegate _smoother;

    // Token: 0x0200015E RID: 350
    protected struct TimestampedStateTable : IEquatableByReference<StateBuffer<TStateTable, TType, TState>.TimestampedStateTable>
    {
        // Token: 0x06000881 RID: 2177 RVA: 0x0001675D File Offset: 0x0001495D
        public bool Equals(in StateBuffer<TStateTable, TType, TState>.TimestampedStateTable other)
        {
            return this.state.Equals(other.state);
        }

        // Token: 0x06000882 RID: 2178 RVA: 0x00016776 File Offset: 0x00014976
        public TimestampedStateTable(SyncStateId id, float time, TStateTable state)
        {
            this.isValid = true;
            this.id = id;
            this.time = time;
            this.state = state;
        }

        // Token: 0x06000883 RID: 2179 RVA: 0x00016794 File Offset: 0x00014994
        public TimestampedStateTable(float time, TStateTable state)
        {
            this.isValid = true;
            this.id = default(SyncStateId);
            this.time = time;
            this.state = state;
        }

        // Token: 0x06000884 RID: 2180 RVA: 0x000167B7 File Offset: 0x000149B7
        bool IEquatableByReference<StateBuffer<TStateTable, TType, TState>.TimestampedStateTable>.Equals(in StateBuffer<TStateTable, TType, TState>.TimestampedStateTable other)
        {
            return this.Equals(other);
        }

        // Token: 0x04000474 RID: 1140
        public bool isValid;

        // Token: 0x04000475 RID: 1141
        public SyncStateId id;

        // Token: 0x04000476 RID: 1142
        public float time;

        // Token: 0x04000477 RID: 1143
        public TStateTable state;
    }

    // Token: 0x0200015F RID: 351
    // (Invoke) Token: 0x06000886 RID: 2182
    public delegate TState InterpolationDelegate(TState a, float timeA, TState b, float timeB, float time);

    // Token: 0x02000160 RID: 352
    // (Invoke) Token: 0x0600088A RID: 2186
    public delegate TState SmoothingDelegate(TState a, TState b, float smooth);
}
