using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;

// Token: 0x0200003D RID: 61
public interface IMultiplayerSessionManager
{
    // Token: 0x17000060 RID: 96
    // (get) Token: 0x06000268 RID: 616
    IConnectedPlayer localPlayer { get; }

    // Token: 0x17000061 RID: 97
    // (get) Token: 0x06000269 RID: 617
    bool isConnectionOwner { get; }

    // Token: 0x17000062 RID: 98
    // (get) Token: 0x0600026A RID: 618
    float syncTime { get; }

    // Token: 0x17000063 RID: 99
    // (get) Token: 0x0600026B RID: 619
    bool isSyncTimeInitialized { get; }

    // Token: 0x17000064 RID: 100
    // (get) Token: 0x0600026C RID: 620
    int maxPlayerCount { get; }

    // Token: 0x17000065 RID: 101
    // (get) Token: 0x0600026D RID: 621
    int connectedPlayerCount { get; }

    // Token: 0x17000066 RID: 102
    // (get) Token: 0x0600026E RID: 622
    bool isConnectingOrConnected { get; }

    // Token: 0x17000067 RID: 103
    // (get) Token: 0x0600026F RID: 623
    bool isConnected { get; }

    // Token: 0x17000068 RID: 104
    // (get) Token: 0x06000270 RID: 624
    bool isConnecting { get; }

    // Token: 0x17000069 RID: 105
    // (get) Token: 0x06000271 RID: 625
    bool isDisconnecting { get; }

    // Token: 0x1700006A RID: 106
    // (get) Token: 0x06000272 RID: 626
    bool isSpectating { get; }

    // Token: 0x1700006B RID: 107
    // (get) Token: 0x06000273 RID: 627
    IReadOnlyList<IConnectedPlayer> connectedPlayers { get; }

    // Token: 0x1700006C RID: 108
    // (get) Token: 0x06000274 RID: 628
    IConnectedPlayer connectionOwner { get; }

    // Token: 0x14000060 RID: 96
    // (add) Token: 0x06000275 RID: 629
    // (remove) Token: 0x06000276 RID: 630
    event Action connectedEvent;

    // Token: 0x14000061 RID: 97
    // (add) Token: 0x06000277 RID: 631
    // (remove) Token: 0x06000278 RID: 632
    event Action<ConnectionFailedReason> connectionFailedEvent;

    // Token: 0x14000062 RID: 98
    // (add) Token: 0x06000279 RID: 633
    // (remove) Token: 0x0600027A RID: 634
    event Action<IConnectedPlayer> playerConnectedEvent;

    // Token: 0x14000063 RID: 99
    // (add) Token: 0x0600027B RID: 635
    // (remove) Token: 0x0600027C RID: 636
    event Action<IConnectedPlayer> playerDisconnectedEvent;

    // Token: 0x14000064 RID: 100
    // (add) Token: 0x0600027D RID: 637
    // (remove) Token: 0x0600027E RID: 638
    event Action<IConnectedPlayer> playerAvatarChangedEvent;

    // Token: 0x14000065 RID: 101
    // (add) Token: 0x0600027F RID: 639
    // (remove) Token: 0x06000280 RID: 640
    event Action<IConnectedPlayer> playerStateChangedEvent;

    // Token: 0x14000066 RID: 102
    // (add) Token: 0x06000281 RID: 641
    // (remove) Token: 0x06000282 RID: 642
    event Action<IConnectedPlayer> connectionOwnerStateChangedEvent;

    // Token: 0x14000067 RID: 103
    // (add) Token: 0x06000283 RID: 643
    // (remove) Token: 0x06000284 RID: 644
    event Action<DisconnectedReason> disconnectedEvent;

    // Token: 0x14000068 RID: 104
    // (add) Token: 0x06000285 RID: 645
    // (remove) Token: 0x06000286 RID: 646
    event Action pollUpdateEvent;

    // Token: 0x06000287 RID: 647
    void SetMaxPlayerCount(int maxPlayerCount);

    // Token: 0x06000288 RID: 648
    void StartSession(MultiplayerSessionManager.SessionType sessionType, ConnectedPlayerManager connectedPlayerManager);

    // Token: 0x06000289 RID: 649
    void EndSession();

    // Token: 0x0600028A RID: 650
    IConnectedPlayer GetPlayerByUserId(string userId);

    // Token: 0x0600028B RID: 651
    IConnectedPlayer GetConnectedPlayer(int index);

    // Token: 0x0600028C RID: 652
    void KickPlayer(string userId);

    // Token: 0x0600028D RID: 653
    void Disconnect();

    // Token: 0x0600028E RID: 654
    void Send<T>(T message) where T : INetSerializable;

    // Token: 0x0600028F RID: 655
    void SendToPlayer<T>(T message, IConnectedPlayer player) where T : INetSerializable;

    // Token: 0x06000290 RID: 656
    void SendUnreliable<T>(T message) where T : INetSerializable;

    // Token: 0x06000291 RID: 657
    void SendUnreliableEncryptedToPlayer<T>(T message, IConnectedPlayer player) where T : INetSerializable;

    // Token: 0x06000292 RID: 658
    void RegisterCallback<T>(MultiplayerSessionManager.MessageType serializerType, Action<T, IConnectedPlayer> callback, Func<T> constructor) where T : INetSerializable;

    // Token: 0x06000293 RID: 659
    void UnregisterCallback<T>(MultiplayerSessionManager.MessageType serializerType) where T : INetSerializable;

    // Token: 0x06000294 RID: 660
    void SetLocalPlayerState(string state, bool hasState);

    // Token: 0x06000295 RID: 661
    bool LocalPlayerHasState(string state);

    // Token: 0x06000296 RID: 662
    void RegisterSerializer(MultiplayerSessionManager.MessageType serializerType, INetworkPacketSubSerializer<IConnectedPlayer> subSerializer);

    // Token: 0x06000297 RID: 663
    void UnregisterSerializer(MultiplayerSessionManager.MessageType serializerType, INetworkPacketSubSerializer<IConnectedPlayer> subSerializer);
}
