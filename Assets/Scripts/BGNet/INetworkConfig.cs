using System;

// Token: 0x0200003E RID: 62
public interface INetworkConfig
{
    // Token: 0x1700006D RID: 109
    // (get) Token: 0x06000298 RID: 664
    int maxPartySize { get; }

    // Token: 0x1700006E RID: 110
    // (get) Token: 0x06000299 RID: 665
    int discoveryPort { get; }

    // Token: 0x1700006F RID: 111
    // (get) Token: 0x0600029A RID: 666
    int partyPort { get; }

    // Token: 0x17000070 RID: 112
    // (get) Token: 0x0600029B RID: 667
    int multiplayerPort { get; }

    // Token: 0x17000071 RID: 113
    // (get) Token: 0x0600029C RID: 668
    DnsEndPoint masterServerEndPoint { get; }

    // Token: 0x17000072 RID: 114
    // (get) Token: 0x0600029D RID: 669
    string multiplayerStatusUrl { get; }

    // Token: 0x17000073 RID: 115
    // (get) Token: 0x0600029E RID: 670
    string quickPlaySetupUrl { get; }

    // Token: 0x17000074 RID: 116
    // (get) Token: 0x0600029F RID: 671
    string graphUrl { get; }

    // Token: 0x17000075 RID: 117
    // (get) Token: 0x060002A0 RID: 672
    string graphAccessToken { get; }

    // Token: 0x17000076 RID: 118
    // (get) Token: 0x060002A1 RID: 673
    bool forceGameLift { get; }

    // Token: 0x17000077 RID: 119
    // (get) Token: 0x060002A2 RID: 674
    ServiceEnvironment serviceEnvironment { get; }
}
