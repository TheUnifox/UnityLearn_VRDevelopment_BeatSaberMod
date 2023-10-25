using System;

// Token: 0x02000033 RID: 51
public interface IConnectedPlayer
{
    // Token: 0x17000047 RID: 71
    // (get) Token: 0x06000195 RID: 405
    bool isMe { get; }

    // Token: 0x17000048 RID: 72
    // (get) Token: 0x06000196 RID: 406
    string userId { get; }

    // Token: 0x17000049 RID: 73
    // (get) Token: 0x06000197 RID: 407
    string userName { get; }

    // Token: 0x1700004A RID: 74
    // (get) Token: 0x06000198 RID: 408
    bool hasValidLatency { get; }

    // Token: 0x1700004B RID: 75
    // (get) Token: 0x06000199 RID: 409
    float currentLatency { get; }

    // Token: 0x1700004C RID: 76
    // (get) Token: 0x0600019A RID: 410
    bool isConnected { get; }

    // Token: 0x1700004D RID: 77
    // (get) Token: 0x0600019B RID: 411
    DisconnectedReason disconnectedReason { get; }

    // Token: 0x1700004E RID: 78
    // (get) Token: 0x0600019C RID: 412
    bool isConnectionOwner { get; }

    // Token: 0x1700004F RID: 79
    // (get) Token: 0x0600019D RID: 413
    float offsetSyncTime { get; }

    // Token: 0x17000050 RID: 80
    // (get) Token: 0x0600019E RID: 414
    int sortIndex { get; }

    // Token: 0x17000051 RID: 81
    // (get) Token: 0x0600019F RID: 415
    bool isKicked { get; }

    // Token: 0x17000052 RID: 82
    // (get) Token: 0x060001A0 RID: 416
    MultiplayerAvatarData multiplayerAvatarData { get; }

    // Token: 0x060001A1 RID: 417
    bool HasState(string state);
}
