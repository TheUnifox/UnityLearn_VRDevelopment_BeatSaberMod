using System;
using LiteNetLib.Utils;

// Token: 0x02000089 RID: 137
public struct StandardScoreSyncState : IStateTable<StandardScoreSyncState, StandardScoreSyncState.Score, int>, INetSerializable, IEquatableByReference<StandardScoreSyncState>
{
    // Token: 0x06000597 RID: 1431 RVA: 0x0000F1FC File Offset: 0x0000D3FC
    public void SetState(StandardScoreSyncState.Score s, int value)
    {
        switch (s)
        {
            case StandardScoreSyncState.Score.ModifiedScore:
                this._modifiedScore = value;
                return;
            case StandardScoreSyncState.Score.MultipliedScore:
                this._multipliedScore = value;
                return;
            case StandardScoreSyncState.Score.ImmediateMaxPossibleMultipliedScore:
                this._immediateMaxPossibleMultipliedScore = value;
                return;
            case StandardScoreSyncState.Score.Combo:
                this._combo = value;
                return;
            case StandardScoreSyncState.Score.Multiplier:
                this._multiplier = value;
                return;
            default:
                return;
        }
    }

    // Token: 0x06000598 RID: 1432 RVA: 0x0000F24C File Offset: 0x0000D44C
    public StandardScoreSyncState GetDelta(in StandardScoreSyncState stateTable)
    {
        return new StandardScoreSyncState
        {
            _modifiedScore = stateTable._modifiedScore - this._modifiedScore,
            _multipliedScore = stateTable._multipliedScore - this._multipliedScore,
            _immediateMaxPossibleMultipliedScore = stateTable._immediateMaxPossibleMultipliedScore - this._immediateMaxPossibleMultipliedScore,
            _combo = stateTable._combo - this._combo,
            _multiplier = stateTable._multiplier - this._multiplier
        };
    }

    // Token: 0x06000599 RID: 1433 RVA: 0x0000F2C8 File Offset: 0x0000D4C8
    public StandardScoreSyncState ApplyDelta(in StandardScoreSyncState delta)
    {
        return new StandardScoreSyncState
        {
            _modifiedScore = delta._modifiedScore + this._modifiedScore,
            _multipliedScore = delta._multipliedScore + this._multipliedScore,
            _immediateMaxPossibleMultipliedScore = delta._immediateMaxPossibleMultipliedScore + this._immediateMaxPossibleMultipliedScore,
            _combo = delta._combo + this._combo,
            _multiplier = delta._multiplier + this._multiplier
        };
    }

    // Token: 0x0600059A RID: 1434 RVA: 0x0000F344 File Offset: 0x0000D544
    public int GetState(StandardScoreSyncState.Score s)
    {
        switch (s)
        {
            case StandardScoreSyncState.Score.ModifiedScore:
                return this._modifiedScore;
            case StandardScoreSyncState.Score.MultipliedScore:
                return this._multipliedScore;
            case StandardScoreSyncState.Score.ImmediateMaxPossibleMultipliedScore:
                return this._immediateMaxPossibleMultipliedScore;
            case StandardScoreSyncState.Score.Combo:
                return this._combo;
            case StandardScoreSyncState.Score.Multiplier:
                return this._multiplier;
            default:
                return 0;
        }
    }

    // Token: 0x0600059B RID: 1435 RVA: 0x0000F391 File Offset: 0x0000D591
    public void Serialize(NetDataWriter writer)
    {
        writer.PutVarInt(this._modifiedScore);
        writer.PutVarInt(this._multipliedScore);
        writer.PutVarInt(this._immediateMaxPossibleMultipliedScore);
        writer.PutVarInt(this._combo);
        writer.PutVarInt(this._multiplier);
    }

    // Token: 0x0600059C RID: 1436 RVA: 0x0000F3CF File Offset: 0x0000D5CF
    public void Deserialize(NetDataReader reader)
    {
        this._modifiedScore = reader.GetVarInt();
        this._multipliedScore = reader.GetVarInt();
        this._immediateMaxPossibleMultipliedScore = reader.GetVarInt();
        this._combo = reader.GetVarInt();
        this._multiplier = reader.GetVarInt();
    }

    // Token: 0x0600059D RID: 1437 RVA: 0x0000F410 File Offset: 0x0000D610
    public bool Equals(in StandardScoreSyncState other)
    {
        return this._modifiedScore == other._modifiedScore && this._multipliedScore == other._multipliedScore && this._immediateMaxPossibleMultipliedScore == other._immediateMaxPossibleMultipliedScore && this._combo == other._combo && this._multiplier == other._multiplier;
    }

    // Token: 0x0600059E RID: 1438 RVA: 0x0000F465 File Offset: 0x0000D665
    public int GetSize()
    {
        return VarIntExtensions.GetSize(this._modifiedScore) + VarIntExtensions.GetSize(this._multipliedScore) + VarIntExtensions.GetSize(this._immediateMaxPossibleMultipliedScore) + VarIntExtensions.GetSize(this._combo) + VarIntExtensions.GetSize(this._multiplier);
    }

    // Token: 0x0600059F RID: 1439 RVA: 0x0000F4A2 File Offset: 0x0000D6A2
    StandardScoreSyncState IStateTable<StandardScoreSyncState, StandardScoreSyncState.Score, int>.GetDelta(in StandardScoreSyncState stateTable)
    {
        return this.GetDelta(stateTable);
    }

    // Token: 0x060005A0 RID: 1440 RVA: 0x0000F4AB File Offset: 0x0000D6AB
    StandardScoreSyncState IStateTable<StandardScoreSyncState, StandardScoreSyncState.Score, int>.ApplyDelta(in StandardScoreSyncState delta)
    {
        return this.ApplyDelta(delta);
    }

    // Token: 0x060005A1 RID: 1441 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
    bool IEquatableByReference<StandardScoreSyncState>.Equals(in StandardScoreSyncState other)
    {
        return this.Equals(other);
    }

    // Token: 0x04000229 RID: 553
    private int _modifiedScore;

    // Token: 0x0400022A RID: 554
    private int _multipliedScore;

    // Token: 0x0400022B RID: 555
    private int _immediateMaxPossibleMultipliedScore;

    // Token: 0x0400022C RID: 556
    private int _combo;

    // Token: 0x0400022D RID: 557
    private int _multiplier;

    // Token: 0x0200015D RID: 349
    public enum Score
    {
        // Token: 0x0400046E RID: 1134
        ModifiedScore,
        // Token: 0x0400046F RID: 1135
        MultipliedScore,
        // Token: 0x04000470 RID: 1136
        ImmediateMaxPossibleMultipliedScore,
        // Token: 0x04000471 RID: 1137
        Combo,
        // Token: 0x04000472 RID: 1138
        Multiplier,
        // Token: 0x04000473 RID: 1139
        Count
    }
}
