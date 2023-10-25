using System;
using LiteNetLib.Utils;
using UnityEngine.Scripting;

// Token: 0x0200005D RID: 93
public class MultiplayerLevelCompletionResults : INetImmutableSerializable<MultiplayerLevelCompletionResults>, IComparable
{
    // Token: 0x170000AE RID: 174
    // (get) Token: 0x0600041A RID: 1050 RVA: 0x0000A403 File Offset: 0x00008603
    public MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState playerLevelEndState
    {
        get
        {
            return this._playerLevelEndState;
        }
    }

    // Token: 0x170000AF RID: 175
    // (get) Token: 0x0600041B RID: 1051 RVA: 0x0000A40B File Offset: 0x0000860B
    public MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason playerLevelEndReason
    {
        get
        {
            return this._playerLevelEndReason;
        }
    }

    // Token: 0x170000B0 RID: 176
    // (get) Token: 0x0600041C RID: 1052 RVA: 0x0000A413 File Offset: 0x00008613
    public LevelCompletionResults levelCompletionResults
    {
        get
        {
            return this._levelCompletionResults;
        }
    }

    // Token: 0x170000B1 RID: 177
    // (get) Token: 0x0600041D RID: 1053 RVA: 0x0000A41B File Offset: 0x0000861B
    public bool hasAnyResults
    {
        get
        {
            return MultiplayerLevelCompletionResults.HasAnyResult(this._playerLevelEndState);
        }
    }

    // Token: 0x170000B2 RID: 178
    // (get) Token: 0x0600041E RID: 1054 RVA: 0x0000A428 File Offset: 0x00008628
    public bool failedOrGivenUp
    {
        get
        {
            return this._playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.GivenUp || this._playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Failed;
        }
    }

    // Token: 0x0600041F RID: 1055 RVA: 0x000024B7 File Offset: 0x000006B7
    [Preserve]
    public MultiplayerLevelCompletionResults()
    {
    }

    // Token: 0x06000420 RID: 1056 RVA: 0x0000A43E File Offset: 0x0000863E
    public MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState playerLevelEndState, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason playerLevelEndReason, LevelCompletionResults levelCompletionResults)
    {
        this._playerLevelEndState = playerLevelEndState;
        this._playerLevelEndReason = playerLevelEndReason;
        this._levelCompletionResults = levelCompletionResults;
    }

    // Token: 0x06000421 RID: 1057 RVA: 0x0000A45B File Offset: 0x0000865B
    public void Serialize(NetDataWriter writer)
    {
        writer.PutVarInt((int)this._playerLevelEndState);
        writer.PutVarInt((int)this._playerLevelEndReason);
        if (MultiplayerLevelCompletionResults.HasAnyResult(this._playerLevelEndState))
        {
            this.levelCompletionResults.Serialize(writer);
        }
    }

    // Token: 0x06000422 RID: 1058 RVA: 0x0000A490 File Offset: 0x00008690
    public MultiplayerLevelCompletionResults CreateFromSerializedData(NetDataReader reader)
    {
        int varInt = reader.GetVarInt();
        MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason varInt2 = (MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason)reader.GetVarInt();
        LevelCompletionResults levelCompletionResults = MultiplayerLevelCompletionResults.HasAnyResult((MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState)varInt) ? LevelCompletionResults.CreateFromSerializedData(reader) : null;
        return new MultiplayerLevelCompletionResults((MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState)varInt, varInt2, levelCompletionResults);
    }

    // Token: 0x06000423 RID: 1059 RVA: 0x0000A4C4 File Offset: 0x000086C4
    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }
        MultiplayerLevelCompletionResults multiplayerLevelCompletionResults;
        if ((multiplayerLevelCompletionResults = (obj as MultiplayerLevelCompletionResults)) == null)
        {
            throw new ArgumentException("Comparing not comparable data.");
        }
        if (this.levelCompletionResults != null && multiplayerLevelCompletionResults.levelCompletionResults != null)
        {
            return this.levelCompletionResults.CompareTo(multiplayerLevelCompletionResults.levelCompletionResults);
        }
        if (this.levelCompletionResults != null && multiplayerLevelCompletionResults.levelCompletionResults == null)
        {
            return -1;
        }
        if (this.levelCompletionResults == null)
        {
            LevelCompletionResults levelCompletionResults = multiplayerLevelCompletionResults.levelCompletionResults;
            return 1;
        }
        return 1;
    }

    // Token: 0x06000424 RID: 1060 RVA: 0x0000A531 File Offset: 0x00008731
    public static bool HasAnyResult(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState playerLevelEndState)
    {
        return playerLevelEndState == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.SongFinished || playerLevelEndState == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished;
    }

    // Token: 0x0400016D RID: 365
    private readonly MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState _playerLevelEndState;

    // Token: 0x0400016E RID: 366
    private readonly MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason _playerLevelEndReason;

    // Token: 0x0400016F RID: 367
    private readonly LevelCompletionResults _levelCompletionResults;

    // Token: 0x0200012D RID: 301
    public enum MultiplayerPlayerLevelEndState
    {
        // Token: 0x040003DF RID: 991
        SongFinished,
        // Token: 0x040003E0 RID: 992
        NotFinished,
        // Token: 0x040003E1 RID: 993
        NotStarted
    }

    // Token: 0x0200012E RID: 302
    public enum MultiplayerPlayerLevelEndReason
    {
        // Token: 0x040003E3 RID: 995
        Cleared,
        // Token: 0x040003E4 RID: 996
        Failed,
        // Token: 0x040003E5 RID: 997
        GivenUp,
        // Token: 0x040003E6 RID: 998
        Quit,
        // Token: 0x040003E7 RID: 999
        HostEndedLevel,
        // Token: 0x040003E8 RID: 1000
        WasInactive,
        // Token: 0x040003E9 RID: 1001
        StartupFailed,
        // Token: 0x040003EA RID: 1002
        ConnectedAfterLevelEnded
    }
}
