using System;

// Token: 0x02000010 RID: 16
public class MockScoreSyncStateSender : IDisposable
{
    // Token: 0x0600006F RID: 111 RVA: 0x00004016 File Offset: 0x00002216
    public MockScoreSyncStateSender(IMultiplayerSessionManager msm)
    {
        this._multiplayerSessionManager = msm;
        this._multiplayerSessionManager.RegisterCallback<StandardScoreSyncStateNetSerializable>(MultiplayerSessionManager.MessageType.ScoreSyncState, new Action<StandardScoreSyncStateNetSerializable, IConnectedPlayer>(this.HandleScoreSyncStateUpdate), new Func<StandardScoreSyncStateNetSerializable>(StandardScoreSyncStateNetSerializable.pool.Obtain));
    }

    // Token: 0x06000070 RID: 112 RVA: 0x0000404E File Offset: 0x0000224E
    public void Dispose()
    {
        this._multiplayerSessionManager.UnregisterCallback<StandardScoreSyncStateNetSerializable>(MultiplayerSessionManager.MessageType.ScoreSyncState);
    }

    // Token: 0x06000071 RID: 113 RVA: 0x0000405C File Offset: 0x0000225C
    public void SendScore(int modifiedScore, int multipliedScore, int immediateMaxPossibleMultipliedScore, int combo, int multiplier)
    {
        StandardScoreSyncState state = default(StandardScoreSyncState);
        state.SetState(StandardScoreSyncState.Score.ModifiedScore, modifiedScore);
        state.SetState(StandardScoreSyncState.Score.MultipliedScore, multipliedScore);
        state.SetState(StandardScoreSyncState.Score.ImmediateMaxPossibleMultipliedScore, immediateMaxPossibleMultipliedScore);
        state.SetState(StandardScoreSyncState.Score.Combo, combo);
        state.SetState(StandardScoreSyncState.Score.Multiplier, multiplier);
        StandardScoreSyncStateNetSerializable standardScoreSyncStateNetSerializable = StandardScoreSyncStateNetSerializable.pool.Obtain();
        standardScoreSyncStateNetSerializable.state = state;
        standardScoreSyncStateNetSerializable.time = this._multiplayerSessionManager.syncTime;
        this._multiplayerSessionManager.SendUnreliable<StandardScoreSyncStateNetSerializable>(standardScoreSyncStateNetSerializable);
    }

    // Token: 0x06000072 RID: 114 RVA: 0x000021E3 File Offset: 0x000003E3
    private void HandleScoreSyncStateUpdate(StandardScoreSyncStateNetSerializable nodePose, IConnectedPlayer connectedPlayer)
    {
    }

    // Token: 0x04000056 RID: 86
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;
}
