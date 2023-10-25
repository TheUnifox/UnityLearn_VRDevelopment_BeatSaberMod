using System;

// Token: 0x02000012 RID: 18
public class CustomNetworkConfig : INetworkConfig
{
    // Token: 0x17000011 RID: 17
    // (get) Token: 0x0600009B RID: 155 RVA: 0x0000446C File Offset: 0x0000266C
    public int maxPartySize { get; }

    // Token: 0x17000012 RID: 18
    // (get) Token: 0x0600009C RID: 156 RVA: 0x00004474 File Offset: 0x00002674
    public int discoveryPort { get; }

    // Token: 0x17000013 RID: 19
    // (get) Token: 0x0600009D RID: 157 RVA: 0x0000447C File Offset: 0x0000267C
    public int partyPort { get; }

    // Token: 0x17000014 RID: 20
    // (get) Token: 0x0600009E RID: 158 RVA: 0x00004484 File Offset: 0x00002684
    public int multiplayerPort { get; }

    // Token: 0x17000015 RID: 21
    // (get) Token: 0x0600009F RID: 159 RVA: 0x0000448C File Offset: 0x0000268C
    public DnsEndPoint masterServerEndPoint { get; }

    // Token: 0x17000016 RID: 22
    // (get) Token: 0x060000A0 RID: 160 RVA: 0x00004494 File Offset: 0x00002694
    public string multiplayerStatusUrl { get; }

    // Token: 0x17000017 RID: 23
    // (get) Token: 0x060000A1 RID: 161 RVA: 0x0000449C File Offset: 0x0000269C
    public string quickPlaySetupUrl
    {
        get
        {
            return string.Empty;
        }
    }

    // Token: 0x17000018 RID: 24
    // (get) Token: 0x060000A2 RID: 162 RVA: 0x000044A3 File Offset: 0x000026A3
    public string graphUrl { get; }

    // Token: 0x17000019 RID: 25
    // (get) Token: 0x060000A3 RID: 163 RVA: 0x000044AB File Offset: 0x000026AB
    public string graphAccessToken { get; }

    // Token: 0x1700001A RID: 26
    // (get) Token: 0x060000A4 RID: 164 RVA: 0x000044B3 File Offset: 0x000026B3
    public bool forceGameLift { get; }

    // Token: 0x1700001B RID: 27
    // (get) Token: 0x060000A5 RID: 165 RVA: 0x000044BB File Offset: 0x000026BB
    public ServiceEnvironment serviceEnvironment { get; }

    // Token: 0x060000A6 RID: 166 RVA: 0x000044C4 File Offset: 0x000026C4
    public CustomNetworkConfig(INetworkConfig fromNetworkConfig, string customServerHostName, int port, bool forceGameLift)
    {
        this.maxPartySize = fromNetworkConfig.maxPartySize;
        this.discoveryPort = fromNetworkConfig.discoveryPort;
        this.partyPort = fromNetworkConfig.partyPort;
        this.multiplayerPort = fromNetworkConfig.multiplayerPort;
        this.graphAccessToken = fromNetworkConfig.graphAccessToken;
        this.graphUrl = fromNetworkConfig.graphUrl;
        this.masterServerEndPoint = fromNetworkConfig.masterServerEndPoint;
        this.multiplayerStatusUrl = fromNetworkConfig.multiplayerStatusUrl;
        if (!forceGameLift)
        {
            this.masterServerEndPoint = new DnsEndPoint(customServerHostName, port);
            this.multiplayerStatusUrl = "https://status." + customServerHostName;
        }
        else
        {
            ServiceEnvironment serviceEnvironment;
            this.serviceEnvironment = (Enum.TryParse<ServiceEnvironment>(customServerHostName, true, out serviceEnvironment) ? serviceEnvironment : fromNetworkConfig.serviceEnvironment);
        }
        this.forceGameLift = forceGameLift;
    }
}
