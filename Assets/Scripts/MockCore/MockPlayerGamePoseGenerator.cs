using System;

// Token: 0x0200000B RID: 11
public abstract class MockPlayerGamePoseGenerator : IDisposable
{
    // Token: 0x06000044 RID: 68 RVA: 0x00002389 File Offset: 0x00000589
    protected MockPlayerGamePoseGenerator(IMultiplayerSessionManager multiplayerSessionManager, IGameplayRpcManager gameplayRpcManager, bool leftHanded)
    {
        this.multiplayerSessionManager = multiplayerSessionManager;
        this.gameplayRpcManager = gameplayRpcManager;
        this.leftHanded = leftHanded;
        this.mockNodePoseSyncStateSender = new MockNodePoseSyncStateSender(multiplayerSessionManager);
        this.mockScoreSyncStateSender = new MockScoreSyncStateSender(multiplayerSessionManager);
    }

    // Token: 0x06000045 RID: 69 RVA: 0x000023BE File Offset: 0x000005BE
    public virtual void Dispose()
    {
        MockNodePoseSyncStateSender mockNodePoseSyncStateSender = this.mockNodePoseSyncStateSender;
        if (mockNodePoseSyncStateSender != null)
        {
            mockNodePoseSyncStateSender.Dispose();
        }
        MockScoreSyncStateSender mockScoreSyncStateSender = this.mockScoreSyncStateSender;
        if (mockScoreSyncStateSender == null)
        {
            return;
        }
        mockScoreSyncStateSender.Dispose();
    }

    // Token: 0x06000046 RID: 70
    public abstract void Init(float introStartTime, MockBeatmapData beatmapData, GameplayModifiers gameplayModifiers, Action onSongFinished);

    // Token: 0x06000047 RID: 71
    public abstract void Tick();

    // Token: 0x06000048 RID: 72 RVA: 0x000023E4 File Offset: 0x000005E4
    public virtual void SimulateFail()
    {
        LevelCompletionResults levelCompletionResults = MockPlayerGamePoseGenerator.CreateEmptyLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed);
        this.gameplayRpcManager.LevelFinished(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Failed, levelCompletionResults));
    }

    // Token: 0x06000049 RID: 73 RVA: 0x0000240C File Offset: 0x0000060C
    public void SimulateGiveUp()
    {
        LevelCompletionResults levelCompletionResults = MockPlayerGamePoseGenerator.CreateEmptyLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Incomplete);
        this.gameplayRpcManager.LevelFinished(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.GivenUp, levelCompletionResults));
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00002434 File Offset: 0x00000634
    private static LevelCompletionResults CreateEmptyLevelCompletionResults(LevelCompletionResults.LevelEndStateType levelEndStateType)
    {
        return new LevelCompletionResults(GameplayModifiers.noModifiers, 0, 0, RankModel.Rank.A, false, 0f, 0f, 0f, 0f, levelEndStateType, LevelCompletionResults.LevelEndAction.None, 1f, 1, 0, 0, 0, 1, 100, 1000, 1, 8f, 100f, 0, 0f);
    }

    // Token: 0x0400001C RID: 28
    protected readonly IMultiplayerSessionManager multiplayerSessionManager;

    // Token: 0x0400001D RID: 29
    protected readonly IGameplayRpcManager gameplayRpcManager;

    // Token: 0x0400001E RID: 30
    protected readonly bool leftHanded;

    // Token: 0x0400001F RID: 31
    protected readonly MockNodePoseSyncStateSender mockNodePoseSyncStateSender;

    // Token: 0x04000020 RID: 32
    protected readonly MockScoreSyncStateSender mockScoreSyncStateSender;
}
