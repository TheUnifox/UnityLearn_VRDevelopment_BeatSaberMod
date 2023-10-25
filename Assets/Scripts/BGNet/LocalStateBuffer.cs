using System;
using LiteNetLib.Utils;

// Token: 0x0200008E RID: 142
public class LocalStateBuffer<TStateTable, TType, TState> : StateBuffer<TStateTable, TType, TState> where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable> where TType : struct, IConvertible where TState : struct
{
    // Token: 0x060005C4 RID: 1476 RVA: 0x0000F8F3 File Offset: 0x0000DAF3
    public LocalStateBuffer(float fullStateUpdateFrequency, float deltaUpdateFrequency, int size, StateBuffer<TStateTable, TType, TState>.InterpolationDelegate interpolator, StateBuffer<TStateTable, TType, TState>.SmoothingDelegate smoother = null) : base(size, interpolator, smoother)
    {
        this._fullStateUpdateFrequency = fullStateUpdateFrequency;
        this._deltaUpdateFrequency = deltaUpdateFrequency;
    }

    // Token: 0x060005C5 RID: 1477 RVA: 0x0000F926 File Offset: 0x0000DB26
    public override void Clear()
    {
        base.Clear();
        this.MarkDirty();
    }

    // Token: 0x060005C6 RID: 1478 RVA: 0x0000F934 File Offset: 0x0000DB34
    public void MarkDirty()
    {
        for (int i = 0; i < this._sentStates.Length; i++)
        {
            this._sentStates[i].isValid = false;
        }
        for (int j = 0; j < this._sentDeltas.Length; j++)
        {
            this._sentDeltas[j].isValid = false;
        }
    }

    // Token: 0x060005C7 RID: 1479 RVA: 0x0000F98C File Offset: 0x0000DB8C
    public bool TryGetSerializedState(out TStateTable state, out float time, out SyncStateId id)
    {
        if (this._sentStates[this._sentStateIndex].isValid && (this._sentStates[this._sentStateIndex].time > this._current.time - this._fullStateUpdateFrequency || this._sentStates[this._sentStateIndex].Equals(this._current)))
        {
            state = default(TStateTable);
            time = 0f;
            id = default(SyncStateId);
            return false;
        }
        this._lastSentSyncStateId = this._lastSentSyncStateId.Increment();
        id = this._lastSentSyncStateId;
        state = this._current.state;
        time = this._current.time;
        this._sentStateIndex = (this._sentStateIndex + 1) % 4;
        this._sentStates[this._sentStateIndex] = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable(id, time, state);
        return true;
    }

    // Token: 0x060005C8 RID: 1480 RVA: 0x0000FA80 File Offset: 0x0000DC80
    public bool TryGetSerializedStateDelta(out TStateTable delta, out float timeOffset, out SyncStateId baseId)
    {
        delta = default(TStateTable);
        timeOffset = 0f;
        baseId = default(SyncStateId);
        if ((this._sentStates[this._sentStateIndex].isValid && this._sentStates[this._sentStateIndex].time > this._current.time - this._deltaUpdateFrequency) || (this._sentDeltas[this._sentDeltaIndex].isValid && this._sentDeltas[this._sentDeltaIndex].time > this._current.time - this._deltaUpdateFrequency))
        {
            return false;
        }
        bool flag = false;
        int num = int.MaxValue;
        timeOffset = float.MaxValue;
        for (int i = 0; i < 4; i++)
        {
            if (this._sentStates[i].isValid)
            {
                float num2 = this._current.time - this._sentStates[i].time;
                TStateTable delta2 = this._sentStates[i].state.GetDelta(this._current.state);
                TStateTable tstateTable = default(TStateTable);
                int num3 = tstateTable.Equals(delta2) ? 0 : delta2.GetSize();
                if (num3 <= num && (num3 != num || num2 <= timeOffset))
                {
                    delta = delta2;
                    num = num3;
                    timeOffset = num2;
                    baseId = this._sentStates[i].id;
                    flag = true;
                }
            }
        }
        if (!flag)
        {
            return false;
        }
        bool flag2 = true;
        for (int j = 0; j < 4; j++)
        {
            if (!this._sentDeltas[j].isValid || this._sentDeltas[j].id != baseId || !this._sentDeltas[j].state.Equals(delta))
            {
                flag2 = false;
                break;
            }
        }
        if (flag2)
        {
            return false;
        }
        this._sentDeltaIndex = (this._sentDeltaIndex + 1) % 4;
        this._sentDeltas[this._sentDeltaIndex] = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable(baseId, this._current.time, delta);
        return true;
    }

    // Token: 0x060005C9 RID: 1481 RVA: 0x0000FCBA File Offset: 0x0000DEBA
    public void SetTime(float time)
    {
        this._current.time = time;
        base.InsertState(this._current.state, time);
    }

    // Token: 0x060005CA RID: 1482 RVA: 0x0000FCDA File Offset: 0x0000DEDA
    public void SetState(TType type, TState state)
    {
        this._current.state.SetState(type, state);
    }

    // Token: 0x060005CB RID: 1483 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
    public TState GetState(TType type)
    {
        return this._current.state.GetState(type);
    }

    // Token: 0x04000238 RID: 568
    private StateBuffer<TStateTable, TType, TState>.TimestampedStateTable _current;

    // Token: 0x04000239 RID: 569
    private readonly float _fullStateUpdateFrequency;

    // Token: 0x0400023A RID: 570
    private readonly float _deltaUpdateFrequency;

    // Token: 0x0400023B RID: 571
    private const int kMaxSentStates = 4;

    // Token: 0x0400023C RID: 572
    private const int kMaxSentDeltas = 4;

    // Token: 0x0400023D RID: 573
    private int _sentStateIndex;

    // Token: 0x0400023E RID: 574
    private int _sentDeltaIndex;

    // Token: 0x0400023F RID: 575
    private readonly StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[] _sentStates = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[4];

    // Token: 0x04000240 RID: 576
    private readonly StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[] _sentDeltas = new StateBuffer<TStateTable, TType, TState>.TimestampedStateTable[4];

    // Token: 0x04000241 RID: 577
    private SyncStateId _lastSentSyncStateId;
}
