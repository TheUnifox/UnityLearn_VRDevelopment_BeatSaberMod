using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class MockPlayerFiniteStateMachine : IDisposable
{
    // Token: 0x06000035 RID: 53 RVA: 0x00002136 File Offset: 0x00000336
    public MockPlayerFiniteStateMachine(IMultiplayerSessionManager multiplayerSessionManager, IGameplayRpcManager gameplayRpcManager, IMenuRpcManager menuRpcManager, IMockBeatmapDataProvider beatmapDataProvider, MockPlayerLobbyPoseGenerator lobbyPoseGenerator, MockPlayerGamePoseGenerator gamePoseGenerator)
    {
    }

    // Token: 0x17000015 RID: 21
    // (get) Token: 0x06000036 RID: 54 RVA: 0x00002369 File Offset: 0x00000569
    // (set) Token: 0x06000037 RID: 55 RVA: 0x00002370 File Offset: 0x00000570
    public Color saberAColor
    {
        get
        {
            return Color.red;
        }
        set { }
    }

    // Token: 0x17000016 RID: 22
    // (get) Token: 0x06000038 RID: 56 RVA: 0x00002375 File Offset: 0x00000575
    // (set) Token: 0x06000039 RID: 57 RVA: 0x00002370 File Offset: 0x00000570
    public Color saberBColor
    {
        get
        {
            return Color.blue;
        }
        set { }
    }

    // Token: 0x17000017 RID: 23
    // (get) Token: 0x0600003A RID: 58 RVA: 0x0000237C File Offset: 0x0000057C
    // (set) Token: 0x0600003B RID: 59 RVA: 0x00002370 File Offset: 0x00000570
    public Color obstaclesColor
    {
        get
        {
            return Color.white;
        }
        set { }
    }

    // Token: 0x17000018 RID: 24
    // (get) Token: 0x0600003C RID: 60 RVA: 0x00002383 File Offset: 0x00000583
    // (set) Token: 0x0600003D RID: 61 RVA: 0x00002370 File Offset: 0x00000570
    public bool leftHanded
    {
        get
        {
            return false;
        }
        set { }
    }

    // Token: 0x17000019 RID: 25
    // (get) Token: 0x0600003E RID: 62 RVA: 0x00002383 File Offset: 0x00000583
    // (set) Token: 0x0600003F RID: 63 RVA: 0x00002370 File Offset: 0x00000570
    public bool inactiveByDefault
    {
        get
        {
            return false;
        }
        set { }
    }

    // Token: 0x1700001A RID: 26
    // (get) Token: 0x06000040 RID: 64 RVA: 0x00002386 File Offset: 0x00000586
    public MockPlayerGamePoseGenerator gamePoseGenerator
    {
        get
        {
            return null;
        }
    }

    // Token: 0x06000041 RID: 65 RVA: 0x000021E3 File Offset: 0x000003E3
    public void Tick()
    {
    }

    // Token: 0x06000042 RID: 66 RVA: 0x000021E3 File Offset: 0x000003E3
    public void Dispose()
    {
    }

    // Token: 0x06000043 RID: 67 RVA: 0x000021E3 File Offset: 0x000003E3
    public void SetIsReady(bool isReady)
    {
    }
}
