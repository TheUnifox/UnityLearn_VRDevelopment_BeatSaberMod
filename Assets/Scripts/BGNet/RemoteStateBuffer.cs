using System;
using LiteNetLib.Utils;

// Token: 0x0200008F RID: 143
public class RemoteStateBuffer<TStateTable, TType, TState> : StateBuffer<TStateTable, TType, TState> where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable> where TType : struct, IConvertible where TState : struct
{
    // Token: 0x060005CC RID: 1484 RVA: 0x0000FD10 File Offset: 0x0000DF10
    public override void Clear()
    {
        base.Clear();
        for (int i = 0; i < 4; i++)
        {
            this._receivedStates[i].isValid = false;
        }
        for (int j = 0; j < 64; j++)
        {
            this._receivedDeltas[j].isValid = false;
        }
        this._receivedStateIndex = 0;
        this._receivedStateCount = 0;
        this._receivedDeltaIndex = 0;
        this._receivedDeltaCount = 0;
    }

    // Token: 0x060005CD RID: 1485 RVA: 0x0000FD7C File Offset: 0x0000DF7C
    public void PushState(SyncStateId id, TStateTable state, float time)
    {
        this._receivedStates[this._receivedStateIndex] = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable(id, time, state);
        this._receivedStateIndex = (this._receivedStateIndex + 1) % 4;
        if (this._receivedStateCount < 4)
        {
            this._receivedStateCount++;
        }
        base.InsertState(state, time);
        this.ProcessQueue();
    }

    // Token: 0x060005CE RID: 1486 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
    public void PushDelta(SyncStateId baseId, TStateTable delta, float timeOffset)
    {
        SyncStateId id = this._receivedStates[(this._receivedStateIndex + 4 - this._receivedStateCount) % 4].id;
        if (this._receivedStateCount > 0 && baseId.Before(id))
        {
            return;
        }
        for (int i = 0; i < this._receivedStateCount; i++)
        {
            if (!(this._receivedStates[i].id != baseId))
            {
                TStateTable tstateTable = this._receivedStates[i].state.ApplyDelta(delta);
                float time = this._receivedStates[i].time + timeOffset;
                base.InsertState(tstateTable, time);
                return;
            }
        }
        if (!this._receivedDeltas[this._receivedDeltaIndex].isValid)
        {
            this._receivedDeltaCount++;
        }
        this._receivedDeltas[this._receivedDeltaIndex] = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable(baseId, timeOffset, delta);
        this._receivedDeltaIndex = (this._receivedDeltaIndex + 1) % 64;
    }

    // Token: 0x060005CF RID: 1487 RVA: 0x0000FED4 File Offset: 0x0000E0D4
    private void ProcessQueue()
    {
        SyncStateId id = this._receivedStates[(this._receivedStateIndex + 4 - this._receivedStateCount) % 4].id;
        bool flag = true;
        int num = 64 + this._receivedDeltaIndex - this._receivedDeltaCount;
        int i = 0;
        int receivedDeltaCount = this._receivedDeltaCount;
        while (i < receivedDeltaCount)
        {
            int num2 = (num + i) % 64;
            if (!this._receivedDeltas[num2].isValid)
            {
                if (flag)
                {
                    this._receivedDeltaCount--;
                }
            }
            else if (this._receivedDeltas[num2].id.Before(id))
            {
                this._receivedDeltas[num2].isValid = false;
                if (flag)
                {
                    this._receivedDeltaCount--;
                }
            }
            else
            {
                bool flag2 = false;
                for (int j = 0; j < this._receivedStateCount; j++)
                {
                    if (!(this._receivedStates[j].id != this._receivedDeltas[num2].id))
                    {
                        TStateTable tstateTable = this._receivedStates[j].state.ApplyDelta(this._receivedDeltas[num2].state);
                        float time = this._receivedStates[j].time + this._receivedDeltas[num2].time;
                        base.InsertState(tstateTable, time);
                        this._receivedDeltas[num2].isValid = false;
                        if (flag)
                        {
                            this._receivedDeltaCount--;
                        }
                        flag2 = true;
                        break;
                    }
                }
                flag = (flag && flag2);
            }
            i++;
        }
    }

    // Token: 0x060005D0 RID: 1488 RVA: 0x00010088 File Offset: 0x0000E288
    public RemoteStateBuffer(int size, StateBuffer<TStateTable, TType, TState>.InterpolationDelegate interpolator, StateBuffer<TStateTable, TType, TState>.SmoothingDelegate smoother = null) : base(size, interpolator, smoother)
    {
    }

    // Token: 0x04000242 RID: 578
    private const int kMaxStateQueueSize = 4;

    // Token: 0x04000243 RID: 579
    private const int kMaxDeltaQueueSize = 64;

    // Token: 0x04000244 RID: 580
    private StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[] _receivedStates = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[4];

    // Token: 0x04000245 RID: 581
    private StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[] _receivedDeltas = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[64];

    // Token: 0x04000246 RID: 582
    private int _receivedStateIndex;

    // Token: 0x04000247 RID: 583
    private int _receivedStateCount;

    // Token: 0x04000248 RID: 584
    private int _receivedDeltaIndex;

    // Token: 0x04000249 RID: 585
    private int _receivedDeltaCount;
}
