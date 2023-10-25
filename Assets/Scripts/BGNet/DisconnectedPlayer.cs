using System;

// Token: 0x02000015 RID: 21
public class DisconnectedPlayer : IConnectedPlayer
{
    // Token: 0x1700001D RID: 29
    // (get) Token: 0x060000B1 RID: 177 RVA: 0x0000476D File Offset: 0x0000296D
    public float offsetSyncTime
    {
        get
        {
            return 0f;
        }
    }

    // Token: 0x1700001E RID: 30
    // (get) Token: 0x060000B2 RID: 178 RVA: 0x00004774 File Offset: 0x00002974
    public bool isFailed
    {
        get
        {
            return true;
        }
    }

    // Token: 0x1700001F RID: 31
    // (get) Token: 0x060000B3 RID: 179 RVA: 0x00004777 File Offset: 0x00002977
    public bool isMe
    {
        get
        {
            return false;
        }
    }

    // Token: 0x17000020 RID: 32
    // (get) Token: 0x060000B4 RID: 180 RVA: 0x0000477A File Offset: 0x0000297A
    // (set) Token: 0x060000B5 RID: 181 RVA: 0x00004782 File Offset: 0x00002982
    public string userId { get; private set; }

    // Token: 0x17000021 RID: 33
    // (get) Token: 0x060000B6 RID: 182 RVA: 0x0000478B File Offset: 0x0000298B
    // (set) Token: 0x060000B7 RID: 183 RVA: 0x00004793 File Offset: 0x00002993
    public string userName { get; private set; }

    // Token: 0x17000022 RID: 34
    // (get) Token: 0x060000B8 RID: 184 RVA: 0x00004777 File Offset: 0x00002977
    public bool hasValidLatency
    {
        get
        {
            return false;
        }
    }

    // Token: 0x17000023 RID: 35
    // (get) Token: 0x060000B9 RID: 185 RVA: 0x0000476D File Offset: 0x0000296D
    public float currentLatency
    {
        get
        {
            return 0f;
        }
    }

    // Token: 0x17000024 RID: 36
    // (get) Token: 0x060000BA RID: 186 RVA: 0x00004777 File Offset: 0x00002977
    public bool isConnected
    {
        get
        {
            return false;
        }
    }

    // Token: 0x17000025 RID: 37
    // (get) Token: 0x060000BB RID: 187 RVA: 0x00004774 File Offset: 0x00002974
    public DisconnectedReason disconnectedReason
    {
        get
        {
            return DisconnectedReason.Unknown;
        }
    }

    // Token: 0x17000026 RID: 38
    // (get) Token: 0x060000BC RID: 188 RVA: 0x00004777 File Offset: 0x00002977
    public bool isConnectionOwner
    {
        get
        {
            return false;
        }
    }

    // Token: 0x17000027 RID: 39
    // (get) Token: 0x060000BD RID: 189 RVA: 0x0000479C File Offset: 0x0000299C
    public int sortIndex { get; }

    // Token: 0x17000028 RID: 40
    // (get) Token: 0x060000BE RID: 190 RVA: 0x000047A4 File Offset: 0x000029A4
    public MultiplayerAvatarData multiplayerAvatarData { get; }

    // Token: 0x17000029 RID: 41
    // (get) Token: 0x060000BF RID: 191 RVA: 0x00004777 File Offset: 0x00002977
    public bool isKicked
    {
        get
        {
            return false;
        }
    }

    // Token: 0x060000C0 RID: 192 RVA: 0x00004777 File Offset: 0x00002977
    public bool HasState(string state)
    {
        return false;
    }

    // Token: 0x060000C1 RID: 193 RVA: 0x000047AC File Offset: 0x000029AC
    public DisconnectedPlayer(string userId, string userName, int sortIndex)
    {
        this.userId = userId;
        this.userName = userName;
        this.sortIndex = sortIndex;
        this.multiplayerAvatarData = default(MultiplayerAvatarData);
    }
}
