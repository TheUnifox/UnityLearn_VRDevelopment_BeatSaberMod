using System;
using UnityEditor.Networking.PlayerConnection;

// Token: 0x02000040 RID: 64
public interface INetworkPlayer
{
    // Token: 0x1700007B RID: 123
    // (get) Token: 0x060002A9 RID: 681
    string userId { get; }

    // Token: 0x1700007C RID: 124
    // (get) Token: 0x060002AA RID: 682
    string userName { get; }

    // Token: 0x1700007D RID: 125
    // (get) Token: 0x060002AB RID: 683
    bool isMe { get; }

    // Token: 0x1700007E RID: 126
    // (get) Token: 0x060002AC RID: 684
    int currentPartySize { get; }

    // Token: 0x1700007F RID: 127
    // (get) Token: 0x060002AD RID: 685
    bool isMyPartyOwner { get; }

    // Token: 0x17000080 RID: 128
    // (get) Token: 0x060002AE RID: 686
    IConnectedPlayer connectedPlayer { get; }

    // Token: 0x17000081 RID: 129
    // (get) Token: 0x060002AF RID: 687
    GameplayServerConfiguration configuration { get; }

    // Token: 0x17000082 RID: 130
    // (get) Token: 0x060002B0 RID: 688
    BeatmapLevelSelectionMask selectionMask { get; }

    // Token: 0x17000083 RID: 131
    // (get) Token: 0x060002B1 RID: 689
    bool canJoin { get; }

    // Token: 0x060002B2 RID: 690
    void Join();

    // Token: 0x17000084 RID: 132
    // (get) Token: 0x060002B3 RID: 691
    bool requiresPassword { get; }

    // Token: 0x060002B4 RID: 692
    void Join(string password);

    // Token: 0x17000085 RID: 133
    // (get) Token: 0x060002B5 RID: 693
    bool isWaitingOnJoin { get; }

    // Token: 0x17000086 RID: 134
    // (get) Token: 0x060002B6 RID: 694
    bool canInvite { get; }

    // Token: 0x060002B7 RID: 695
    void Invite();

    // Token: 0x17000087 RID: 135
    // (get) Token: 0x060002B8 RID: 696
    bool isWaitingOnInvite { get; }

    // Token: 0x17000088 RID: 136
    // (get) Token: 0x060002B9 RID: 697
    bool canKick { get; }

    // Token: 0x060002BA RID: 698
    void Kick();

    // Token: 0x17000089 RID: 137
    // (get) Token: 0x060002BB RID: 699
    bool canLeave { get; }

    // Token: 0x060002BC RID: 700
    void Leave();

    // Token: 0x1700008A RID: 138
    // (get) Token: 0x060002BD RID: 701
    bool canBlock { get; }

    // Token: 0x060002BE RID: 702
    void Block();

    // Token: 0x1700008B RID: 139
    // (get) Token: 0x060002BF RID: 703
    bool canUnblock { get; }

    // Token: 0x060002C0 RID: 704
    void Unblock();

    // Token: 0x060002C1 RID: 705
    void SendJoinResponse(bool accept);

    // Token: 0x060002C2 RID: 706
    void SendInviteResponse(bool accept);
}
