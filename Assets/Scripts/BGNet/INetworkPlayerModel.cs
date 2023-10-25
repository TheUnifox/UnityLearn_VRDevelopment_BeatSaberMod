using System;
using System.Collections.Generic;

// Token: 0x02000041 RID: 65
public interface INetworkPlayerModel
{
    // Token: 0x1700008C RID: 140
    // (get) Token: 0x060002C3 RID: 707
    // (set) Token: 0x060002C4 RID: 708
    bool discoveryEnabled { get; set; }

    // Token: 0x1700008D RID: 141
    // (get) Token: 0x060002C5 RID: 709
    bool localPlayerIsPartyOwner { get; }

    // Token: 0x1700008E RID: 142
    // (get) Token: 0x060002C6 RID: 710
    bool hasNetworkingFailed { get; }

    // Token: 0x1700008F RID: 143
    // (get) Token: 0x060002C7 RID: 711
    GameplayServerConfiguration configuration { get; }

    // Token: 0x17000090 RID: 144
    // (get) Token: 0x060002C8 RID: 712
    BeatmapLevelSelectionMask selectionMask { get; }

    // Token: 0x17000091 RID: 145
    // (get) Token: 0x060002C9 RID: 713
    int currentPartySize { get; }

    // Token: 0x17000092 RID: 146
    // (get) Token: 0x060002CA RID: 714
    IEnumerable<INetworkPlayer> partyPlayers { get; }

    // Token: 0x17000093 RID: 147
    // (get) Token: 0x060002CB RID: 715
    IEnumerable<INetworkPlayer> otherPlayers { get; }

    // Token: 0x17000094 RID: 148
    // (get) Token: 0x060002CC RID: 716
    ConnectedPlayerManager connectedPlayerManager { get; }

    // Token: 0x1400006A RID: 106
    // (add) Token: 0x060002CD RID: 717
    // (remove) Token: 0x060002CE RID: 718
    event Action<INetworkPlayerModel> connectedPlayerManagerCreatedEvent;

    // Token: 0x1400006B RID: 107
    // (add) Token: 0x060002CF RID: 719
    // (remove) Token: 0x060002D0 RID: 720
    event Action<INetworkPlayerModel> connectedPlayerManagerDestroyedEvent;

    // Token: 0x1400006C RID: 108
    // (add) Token: 0x060002D1 RID: 721
    // (remove) Token: 0x060002D2 RID: 722
    event Action<INetworkPlayerModel> partyChangedEvent;

    // Token: 0x1400006D RID: 109
    // (add) Token: 0x060002D3 RID: 723
    // (remove) Token: 0x060002D4 RID: 724
    event Action<int> partySizeChangedEvent;

    // Token: 0x1400006E RID: 110
    // (add) Token: 0x060002D5 RID: 725
    // (remove) Token: 0x060002D6 RID: 726
    event Action<INetworkPlayer> joinRequestedEvent;

    // Token: 0x1400006F RID: 111
    // (add) Token: 0x060002D7 RID: 727
    // (remove) Token: 0x060002D8 RID: 728
    event Action<INetworkPlayer> inviteRequestedEvent;

    // Token: 0x060002D9 RID: 729
    bool CreatePartyConnection<T>(INetworkPlayerModelPartyConfig<T> config) where T : INetworkPlayerModel;

    // Token: 0x060002DA RID: 730
    void DestroyPartyConnection();
}
