using System;

// Token: 0x0200000E RID: 14
public abstract class MockPlayerLobbyPoseGenerator : IDisposable
{
    // Token: 0x06000068 RID: 104 RVA: 0x00003A8F File Offset: 0x00001C8F
    protected MockPlayerLobbyPoseGenerator(IMultiplayerSessionManager multiplayerSessionManager)
    {
        this.multiplayerSessionManager = multiplayerSessionManager;
        this.mockNodePoseSyncStateSender = new MockNodePoseSyncStateSender(multiplayerSessionManager);
    }

    // Token: 0x06000069 RID: 105 RVA: 0x00003AAA File Offset: 0x00001CAA
    public void Dispose()
    {
        MockNodePoseSyncStateSender mockNodePoseSyncStateSender = this.mockNodePoseSyncStateSender;
        if (mockNodePoseSyncStateSender == null)
        {
            return;
        }
        mockNodePoseSyncStateSender.Dispose();
    }

    // Token: 0x0600006A RID: 106
    public abstract void Init();

    // Token: 0x0600006B RID: 107
    public abstract void Tick();

    // Token: 0x04000048 RID: 72
    protected readonly IMultiplayerSessionManager multiplayerSessionManager;

    // Token: 0x04000049 RID: 73
    protected readonly MockNodePoseSyncStateSender mockNodePoseSyncStateSender;
}
