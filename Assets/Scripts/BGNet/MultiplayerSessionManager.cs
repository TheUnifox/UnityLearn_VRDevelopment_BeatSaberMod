using System;
using System.Collections.Generic;
using LiteNetLib.Utils;

// Token: 0x02000060 RID: 96
public class MultiplayerSessionManager : StandaloneMonobehavior, IMultiplayerSessionManager
{
    // Token: 0x140000A1 RID: 161
    // (add) Token: 0x06000425 RID: 1061 RVA: 0x0000A53C File Offset: 0x0000873C
    // (remove) Token: 0x06000426 RID: 1062 RVA: 0x0000A574 File Offset: 0x00008774
    public event Action connectedEvent;

    // Token: 0x140000A2 RID: 162
    // (add) Token: 0x06000427 RID: 1063 RVA: 0x0000A5AC File Offset: 0x000087AC
    // (remove) Token: 0x06000428 RID: 1064 RVA: 0x0000A5E4 File Offset: 0x000087E4
    public event Action<ConnectionFailedReason> connectionFailedEvent;

    // Token: 0x140000A3 RID: 163
    // (add) Token: 0x06000429 RID: 1065 RVA: 0x0000A61C File Offset: 0x0000881C
    // (remove) Token: 0x0600042A RID: 1066 RVA: 0x0000A654 File Offset: 0x00008854
    public event Action<IConnectedPlayer> playerConnectedEvent;

    // Token: 0x140000A4 RID: 164
    // (add) Token: 0x0600042B RID: 1067 RVA: 0x0000A68C File Offset: 0x0000888C
    // (remove) Token: 0x0600042C RID: 1068 RVA: 0x0000A6C4 File Offset: 0x000088C4
    public event Action<IConnectedPlayer> playerDisconnectedEvent;

    // Token: 0x140000A5 RID: 165
    // (add) Token: 0x0600042D RID: 1069 RVA: 0x0000A6FC File Offset: 0x000088FC
    // (remove) Token: 0x0600042E RID: 1070 RVA: 0x0000A734 File Offset: 0x00008934
    public event Action<IConnectedPlayer> playerAvatarChangedEvent;

    // Token: 0x140000A6 RID: 166
    // (add) Token: 0x0600042F RID: 1071 RVA: 0x0000A76C File Offset: 0x0000896C
    // (remove) Token: 0x06000430 RID: 1072 RVA: 0x0000A7A4 File Offset: 0x000089A4
    public event Action<IConnectedPlayer> playerStateChangedEvent;

    // Token: 0x140000A7 RID: 167
    // (add) Token: 0x06000431 RID: 1073 RVA: 0x0000A7DC File Offset: 0x000089DC
    // (remove) Token: 0x06000432 RID: 1074 RVA: 0x0000A814 File Offset: 0x00008A14
    public event Action<IConnectedPlayer> connectionOwnerStateChangedEvent;

    // Token: 0x140000A8 RID: 168
    // (add) Token: 0x06000433 RID: 1075 RVA: 0x0000A84C File Offset: 0x00008A4C
    // (remove) Token: 0x06000434 RID: 1076 RVA: 0x0000A884 File Offset: 0x00008A84
    public event Action<DisconnectedReason> disconnectedEvent;

    // Token: 0x140000A9 RID: 169
    // (add) Token: 0x06000435 RID: 1077 RVA: 0x0000A8BC File Offset: 0x00008ABC
    // (remove) Token: 0x06000436 RID: 1078 RVA: 0x0000A8F4 File Offset: 0x00008AF4
    public event Action pollUpdateEvent;

    // Token: 0x170000B3 RID: 179
    // (get) Token: 0x06000437 RID: 1079 RVA: 0x0000A929 File Offset: 0x00008B29
    public bool isConnectionOwner
    {
        get
        {
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            return connectedPlayerManager == null || connectedPlayerManager.isConnectionOwner;
        }
    }

    // Token: 0x170000B4 RID: 180
    // (get) Token: 0x06000438 RID: 1080 RVA: 0x0000A93C File Offset: 0x00008B3C
    // (set) Token: 0x06000439 RID: 1081 RVA: 0x0000A944 File Offset: 0x00008B44
    public IConnectedPlayer connectionOwner { get; private set; }

    // Token: 0x170000B5 RID: 181
    // (get) Token: 0x0600043A RID: 1082 RVA: 0x0000A950 File Offset: 0x00008B50
    public bool isSpectating
    {
        get
        {
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            bool? flag;
            if (connectedPlayerManager == null)
            {
                flag = null;
            }
            else
            {
                IConnectedPlayer localPlayer = connectedPlayerManager.localPlayer;
                flag = ((localPlayer != null) ? new bool?(localPlayer.HasState("spectating")) : null);
            }
            return flag ?? false;
        }
    }

    // Token: 0x170000B6 RID: 182
    // (get) Token: 0x0600043B RID: 1083 RVA: 0x0000A9A8 File Offset: 0x00008BA8
    public bool isConnectingOrConnected
    {
        get
        {
            return this.isConnecting || this.isConnected;
        }
    }

    // Token: 0x170000B7 RID: 183
    // (get) Token: 0x0600043C RID: 1084 RVA: 0x0000A9BA File Offset: 0x00008BBA
    public bool isConnected
    {
        get
        {
            return this._connectionState == MultiplayerSessionManager.ConnectionState.Connected;
        }
    }

    // Token: 0x170000B8 RID: 184
    // (get) Token: 0x0600043D RID: 1085 RVA: 0x0000A9C5 File Offset: 0x00008BC5
    public bool isConnecting
    {
        get
        {
            return this._connectionState == MultiplayerSessionManager.ConnectionState.Connecting;
        }
    }

    // Token: 0x170000B9 RID: 185
    // (get) Token: 0x0600043E RID: 1086 RVA: 0x0000A9D0 File Offset: 0x00008BD0
    public bool isDisconnecting
    {
        get
        {
            return this._connectionState == MultiplayerSessionManager.ConnectionState.Disconnecting;
        }
    }

    // Token: 0x170000BA RID: 186
    // (get) Token: 0x0600043F RID: 1087 RVA: 0x0000A9DB File Offset: 0x00008BDB
    public IReadOnlyList<IConnectedPlayer> connectedPlayers
    {
        get
        {
            return this._connectedPlayers;
        }
    }

    // Token: 0x170000BB RID: 187
    // (get) Token: 0x06000440 RID: 1088 RVA: 0x0000A9E3 File Offset: 0x00008BE3
    public int connectedPlayerCount
    {
        get
        {
            return this._connectedPlayers.Count;
        }
    }

    // Token: 0x170000BC RID: 188
    // (get) Token: 0x06000441 RID: 1089 RVA: 0x0000A9F0 File Offset: 0x00008BF0
    public float syncTime
    {
        get
        {
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            if (connectedPlayerManager == null)
            {
                return 0f;
            }
            return connectedPlayerManager.syncTime;
        }
    }

    // Token: 0x170000BD RID: 189
    // (get) Token: 0x06000442 RID: 1090 RVA: 0x0000AA07 File Offset: 0x00008C07
    public bool isSyncTimeInitialized
    {
        get
        {
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            return connectedPlayerManager != null && connectedPlayerManager.syncTimeInitialized;
        }
    }

    // Token: 0x170000BE RID: 190
    // (get) Token: 0x06000443 RID: 1091 RVA: 0x0000AA1A File Offset: 0x00008C1A
    public IConnectedPlayer localPlayer
    {
        get
        {
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            if (connectedPlayerManager == null)
            {
                return null;
            }
            return connectedPlayerManager.localPlayer;
        }
    }

    // Token: 0x170000BF RID: 191
    // (get) Token: 0x06000444 RID: 1092 RVA: 0x0000AA2D File Offset: 0x00008C2D
    public ConnectedPlayerManager connectedPlayerManager
    {
        get
        {
            return this._connectedPlayerManager;
        }
    }

    // Token: 0x170000C0 RID: 192
    // (get) Token: 0x06000445 RID: 1093 RVA: 0x0000AA35 File Offset: 0x00008C35
    public int maxPlayerCount
    {
        get
        {
            return this._maxPlayerCount;
        }
    }

    // Token: 0x06000446 RID: 1094 RVA: 0x0000AA3D File Offset: 0x00008C3D
    protected override void Start()
    {
        this.SetLocalPlayerState("multiplayer_session", true);
    }

    // Token: 0x06000447 RID: 1095 RVA: 0x0000AA4B File Offset: 0x00008C4B
    protected override void Update()
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager != null)
        {
            connectedPlayerManager.PollUpdate(base.frameCount);
        }
        Action action = this.pollUpdateEvent;
        if (action == null)
        {
            return;
        }
        action();
    }

    // Token: 0x06000448 RID: 1096 RVA: 0x0000AA74 File Offset: 0x00008C74
    protected void LateUpdate()
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.PollLateUpdateOptional();
    }

    // Token: 0x06000449 RID: 1097 RVA: 0x0000AA86 File Offset: 0x00008C86
    protected override void OnDestroy()
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.OnDestroy, DisconnectedReason.UserInitiated, ConnectionFailedReason.ConnectionCanceled);
        this.EndSession();
    }

    // Token: 0x0600044A RID: 1098 RVA: 0x0000AA98 File Offset: 0x00008C98
    protected override void OnApplicationPause(bool pauseStatus)
    {
        this.SetLocalPlayerState("backgrounded", pauseStatus);
    }

    // Token: 0x0600044B RID: 1099 RVA: 0x0000AAA6 File Offset: 0x00008CA6
    public void RegisterSerializer(MultiplayerSessionManager.MessageType serializerType, INetworkPacketSubSerializer<IConnectedPlayer> subSerializer)
    {
        this._packetSerializer.RegisterSubSerializer(serializerType, subSerializer);
    }

    // Token: 0x0600044C RID: 1100 RVA: 0x0000AAA6 File Offset: 0x00008CA6
    public void UnregisterSerializer(MultiplayerSessionManager.MessageType serializerType, INetworkPacketSubSerializer<IConnectedPlayer> subSerializer)
    {
        this._packetSerializer.RegisterSubSerializer(serializerType, subSerializer);
    }

    // Token: 0x0600044D RID: 1101 RVA: 0x0000AAB5 File Offset: 0x00008CB5
    public void RegisterCallback<T>(MultiplayerSessionManager.MessageType serializerType, Action<T, IConnectedPlayer> callback, Func<T> constructor) where T : INetSerializable
    {
        this._packetSerializer.RegisterCallback<T>(serializerType, callback, constructor);
    }

    // Token: 0x0600044E RID: 1102 RVA: 0x0000AAC5 File Offset: 0x00008CC5
    public void UnregisterCallback<T>(MultiplayerSessionManager.MessageType serializerType) where T : INetSerializable
    {
        this._packetSerializer.UnregisterCallback<T>(serializerType);
    }

    // Token: 0x0600044F RID: 1103 RVA: 0x0000AAD4 File Offset: 0x00008CD4
    public void StartSession(MultiplayerSessionManager.SessionType sessionType, ConnectedPlayerManager connectedPlayerManager)
    {
        if (connectedPlayerManager == this._connectedPlayerManager)
        {
            return;
        }
        if (this._connectedPlayerManager != null)
        {
            this.EndSession();
        }
        this._connectedPlayerManager = connectedPlayerManager;
        this._connectedPlayerManager.connectedEvent += this.HandleConnected;
        this._connectedPlayerManager.initializedEvent += this.HandleInitialized;
        this._connectedPlayerManager.connectionFailedEvent += this.HandleConnectionFailed;
        this._connectedPlayerManager.syncTimeInitializedEvent += this.HandleSyncTimeInitialized;
        this._connectedPlayerManager.playerConnectedEvent += this.HandlePlayerConnected;
        this._connectedPlayerManager.playerDisconnectedEvent += this.HandlePlayerDisconnected;
        this._connectedPlayerManager.playerAvatarChangedEvent += this.HandlePlayerAvatarChanged;
        this._connectedPlayerManager.playerStateChangedEvent += this.HandlePlayerStateChanged;
        this._connectedPlayerManager.playerOrderChangedEvent += this.HandlePlayerOrderChanged;
        this._connectedPlayerManager.playerLatencyInitializedEvent += this.HandlePlayerLatencyInitialized;
        this._connectedPlayerManager.disconnectedEvent += this.HandleDisconnected;
        this._connectedPlayerManager.RegisterSerializer(ConnectedPlayerManager.MessageType.MultiplayerSession, this._packetSerializer);
        foreach (string state in this._localPlayerState)
        {
            this._connectedPlayerManager.SetLocalPlayerState(state, true);
        }
        this.SetLocalPlayerState("player", sessionType == MultiplayerSessionManager.SessionType.Player);
        this.SetLocalPlayerState("dedicated_server", sessionType == MultiplayerSessionManager.SessionType.DedicatedServer);
        this.SetLocalPlayerState("spectating", sessionType == MultiplayerSessionManager.SessionType.Spectator);
        this.SetLocalPlayerState("wants_to_play_next_level", true);
        this.UpdateConnectionState(UpdateConnectionStateReason.Init, DisconnectedReason.Unknown, ConnectionFailedReason.Unknown);
    }

    // Token: 0x06000450 RID: 1104 RVA: 0x0000ACA0 File Offset: 0x00008EA0
    public void SetMaxPlayerCount(int maxPlayerCount)
    {
        this._maxPlayerCount = maxPlayerCount;
    }

    // Token: 0x06000451 RID: 1105 RVA: 0x0000ACAC File Offset: 0x00008EAC
    public void EndSession()
    {
        this.Disconnect();
        if (this._connectedPlayerManager != null)
        {
            this._connectedPlayerManager.connectedEvent -= this.HandleConnected;
            this._connectedPlayerManager.initializedEvent -= this.HandleInitialized;
            this._connectedPlayerManager.connectionFailedEvent -= this.HandleConnectionFailed;
            this._connectedPlayerManager.syncTimeInitializedEvent -= this.HandleSyncTimeInitialized;
            this._connectedPlayerManager.playerConnectedEvent -= this.HandlePlayerConnected;
            this._connectedPlayerManager.playerDisconnectedEvent -= this.HandlePlayerDisconnected;
            this._connectedPlayerManager.playerStateChangedEvent -= this.HandlePlayerStateChanged;
            this._connectedPlayerManager.playerOrderChangedEvent -= this.HandlePlayerOrderChanged;
            this._connectedPlayerManager.playerLatencyInitializedEvent -= this.HandlePlayerLatencyInitialized;
            this._connectedPlayerManager.disconnectedEvent -= this.HandleDisconnected;
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            if (connectedPlayerManager != null)
            {
                connectedPlayerManager.UnregisterSerializer(ConnectedPlayerManager.MessageType.MultiplayerSession, this._packetSerializer);
            }
            foreach (string state in this._localPlayerState)
            {
                this._connectedPlayerManager.SetLocalPlayerState(state, false);
            }
            this._connectedPlayerManager = null;
        }
    }

    // Token: 0x06000452 RID: 1106 RVA: 0x0000AE1C File Offset: 0x0000901C
    public void Disconnect()
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.ManualDisconnect, DisconnectedReason.UserInitiated, ConnectionFailedReason.ConnectionCanceled);
    }

    // Token: 0x06000453 RID: 1107 RVA: 0x0000AE27 File Offset: 0x00009027
    public void Send<T>(T message) where T : INetSerializable
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.Send<T>(message);
    }

    // Token: 0x06000454 RID: 1108 RVA: 0x0000AE3A File Offset: 0x0000903A
    public void SendToPlayer<T>(T message, IConnectedPlayer player) where T : INetSerializable
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.SendToPlayer<T>(message, player);
    }

    // Token: 0x06000455 RID: 1109 RVA: 0x0000AE4E File Offset: 0x0000904E
    public void SendUnreliable<T>(T message) where T : INetSerializable
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.SendUnreliable<T>(message);
    }

    // Token: 0x06000456 RID: 1110 RVA: 0x0000AE61 File Offset: 0x00009061
    public void SendUnreliableEncryptedToPlayer<T>(T message, IConnectedPlayer player) where T : INetSerializable
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.SendUnreliableEncryptedToPlayer<T>(message, player);
    }

    // Token: 0x06000457 RID: 1111 RVA: 0x0000AE75 File Offset: 0x00009075
    private void HandleInitialized()
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.Init, DisconnectedReason.Unknown, ConnectionFailedReason.Unknown);
    }

    // Token: 0x06000458 RID: 1112 RVA: 0x0000AE80 File Offset: 0x00009080
    private void HandleConnected()
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.Connected, DisconnectedReason.Unknown, ConnectionFailedReason.Unknown);
    }

    // Token: 0x06000459 RID: 1113 RVA: 0x0000AE8B File Offset: 0x0000908B
    private void HandleDisconnected(DisconnectedReason disconnectedReason)
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.RemoteDisconnect, disconnectedReason, ConnectionFailedReason.Unknown);
    }

    // Token: 0x0600045A RID: 1114 RVA: 0x0000AE96 File Offset: 0x00009096
    private void HandleConnectionFailed(ConnectionFailedReason reason)
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.ConnectionFailed, DisconnectedReason.Unknown, reason);
    }

    // Token: 0x0600045B RID: 1115 RVA: 0x0000AEA1 File Offset: 0x000090A1
    private void HandleSyncTimeInitialized()
    {
        this.UpdateConnectionState(UpdateConnectionStateReason.SyncTimeInitialized, DisconnectedReason.Unknown, ConnectionFailedReason.Unknown);
    }

    // Token: 0x0600045C RID: 1116 RVA: 0x0000AEAC File Offset: 0x000090AC
    private void HandlePlayerConnected(IConnectedPlayer player)
    {
        if (player.isConnectionOwner)
        {
            this.connectionOwner = player;
        }
        this.TryUpdateConnectedPlayer(player, true);
    }

    // Token: 0x0600045D RID: 1117 RVA: 0x0000AEC6 File Offset: 0x000090C6
    private void HandlePlayerDisconnected(IConnectedPlayer player)
    {
        if (player.isConnectionOwner)
        {
            this.connectionOwner = null;
        }
        this.TryUpdateConnectedPlayer(player, false);
    }

    // Token: 0x0600045E RID: 1118 RVA: 0x0000AEE0 File Offset: 0x000090E0
    private void HandlePlayerStateChanged(IConnectedPlayer player)
    {
        if (player.isConnectionOwner)
        {
            if (this.isConnected)
            {
                Action<IConnectedPlayer> action = this.connectionOwnerStateChangedEvent;
                if (action != null)
                {
                    action(player);
                }
            }
            else if (this.isConnecting && player.HasState("terminating"))
            {
                this.UpdateConnectionState(UpdateConnectionStateReason.ManualDisconnect, DisconnectedReason.UserInitiated, ConnectionFailedReason.ServerIsTerminating);
            }
        }
        if (this.TryUpdateConnectedPlayer(player, true))
        {
            Action<IConnectedPlayer> action2 = this.playerStateChangedEvent;
            if (action2 == null)
            {
                return;
            }
            action2(player);
        }
    }

    // Token: 0x0600045F RID: 1119 RVA: 0x0000AF4B File Offset: 0x0000914B
    private void HandlePlayerAvatarChanged(IConnectedPlayer player)
    {
        if (this._connectedPlayers.Contains(player))
        {
            Action<IConnectedPlayer> action = this.playerAvatarChangedEvent;
            if (action == null)
            {
                return;
            }
            action(player);
        }
    }

    // Token: 0x06000460 RID: 1120 RVA: 0x0000AF6C File Offset: 0x0000916C
    private void HandlePlayerOrderChanged(IConnectedPlayer player)
    {
        if (player == this._connectedPlayerManager.localPlayer)
        {
            this.UpdateConnectionState(UpdateConnectionStateReason.PlayerOrderChanged, DisconnectedReason.Unknown, ConnectionFailedReason.Unknown);
            return;
        }
        this.TryUpdateConnectedPlayer(player, true);
    }

    // Token: 0x06000461 RID: 1121 RVA: 0x0000AF8F File Offset: 0x0000918F
    private void HandlePlayerLatencyInitialized(IConnectedPlayer player)
    {
        this.TryUpdateConnectedPlayer(player, true);
    }

    // Token: 0x06000462 RID: 1122 RVA: 0x0000AF9C File Offset: 0x0000919C
    public IConnectedPlayer GetPlayerByUserId(string userId)
    {
        IConnectedPlayer localPlayer = this.localPlayer;
        if (((localPlayer != null) ? localPlayer.userId : null) == userId)
        {
            return this.localPlayer;
        }
        return this._connectedPlayers.Find((IConnectedPlayer player) => player.userId == userId);
    }

    // Token: 0x06000463 RID: 1123 RVA: 0x0000AFF3 File Offset: 0x000091F3
    public IConnectedPlayer GetConnectedPlayer(int i)
    {
        return this._connectedPlayers[i];
    }

    // Token: 0x06000464 RID: 1124 RVA: 0x0000B001 File Offset: 0x00009201
    public void SetLocalPlayerState(string state, bool hasState)
    {
        if (hasState)
        {
            this._localPlayerState.Add(state);
        }
        else
        {
            this._localPlayerState.Remove(state);
        }
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.SetLocalPlayerState(state, hasState);
    }

    // Token: 0x06000465 RID: 1125 RVA: 0x0000B034 File Offset: 0x00009234
    public void KickPlayer(string userId)
    {
        ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
        if (connectedPlayerManager == null)
        {
            return;
        }
        connectedPlayerManager.KickPlayer(userId, DisconnectedReason.Kicked);
    }

    // Token: 0x06000466 RID: 1126 RVA: 0x0000B048 File Offset: 0x00009248
    public bool LocalPlayerHasState(string state)
    {
        IConnectedPlayer localPlayer = this.localPlayer;
        if (localPlayer == null)
        {
            return this._localPlayerState.Contains(state);
        }
        return localPlayer.HasState(state);
    }

    // Token: 0x06000467 RID: 1127 RVA: 0x0000B068 File Offset: 0x00009268
    private void UpdateConnectionState(UpdateConnectionStateReason updateReason, DisconnectedReason disconnectedReason = DisconnectedReason.Unknown, ConnectionFailedReason connectionFailedReason = ConnectionFailedReason.Unknown)
    {
        if (updateReason != UpdateConnectionStateReason.ManualDisconnect && updateReason != UpdateConnectionStateReason.OnDestroy && this._connectedPlayerManager != null && this._connectedPlayerManager.isConnected && this._connectedPlayerManager.syncTimeInitialized && (this.isConnectionOwner || !this.LocalPlayerHasState("player") || this._connectedPlayerManager.localPlayer.sortIndex != -1))
        {
            if (this._connectionState == MultiplayerSessionManager.ConnectionState.Connected)
            {
                return;
            }
            this._connectionState = MultiplayerSessionManager.ConnectionState.Connected;
            if (this.isConnectionOwner && this.LocalPlayerHasState("player"))
            {
                this._connectedPlayerManager.SetLocalPlayerSortIndex(0);
            }
            if (this.isConnectionOwner)
            {
                this.connectionOwner = this.localPlayer;
            }
            Action action = this.connectedEvent;
            if (action != null)
            {
                action();
            }
            for (int i = 0; i < this._connectedPlayerManager.connectedPlayerCount; i++)
            {
                this.HandlePlayerConnected(this._connectedPlayerManager.GetConnectedPlayer(i));
            }
            return;
        }
        else
        {
            if (updateReason != UpdateConnectionStateReason.ManualDisconnect && updateReason != UpdateConnectionStateReason.OnDestroy && this._connectedPlayerManager != null && this._connectedPlayerManager.isConnectedOrConnecting)
            {
                this._connectionState = MultiplayerSessionManager.ConnectionState.Connecting;
                return;
            }
            if (this._connectionState == MultiplayerSessionManager.ConnectionState.Disconnected || this._connectionState == MultiplayerSessionManager.ConnectionState.Disconnecting)
            {
                return;
            }
            if (updateReason != UpdateConnectionStateReason.OnDestroy && this._connectedPlayerManager != null && this._connectedPlayerManager.isDisconnecting)
            {
                return;
            }
            bool flag = this._connectionState == MultiplayerSessionManager.ConnectionState.Connecting;
            this._connectionState = MultiplayerSessionManager.ConnectionState.Disconnecting;
            ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
            if (connectedPlayerManager != null)
            {
                connectedPlayerManager.Disconnect(disconnectedReason);
            }
            for (int j = this._connectedPlayers.Count - 1; j >= 0; j--)
            {
                this.HandlePlayerDisconnected(this._connectedPlayers[j]);
            }
            this.connectionOwner = null;
            ConnectedPlayerManager connectedPlayerManager2 = this._connectedPlayerManager;
            if (connectedPlayerManager2 != null)
            {
                connectedPlayerManager2.SetLocalPlayerSortIndex(-1);
            }
            this._freeSortIndices.Clear();
            this._connectionState = MultiplayerSessionManager.ConnectionState.Disconnected;
            if (updateReason == UpdateConnectionStateReason.OnDestroy)
            {
                return;
            }
            if (flag)
            {
                if (connectionFailedReason == ConnectionFailedReason.Unknown && disconnectedReason != DisconnectedReason.Unknown)
                {
                    switch (disconnectedReason)
                    {
                        case DisconnectedReason.UserInitiated:
                            connectionFailedReason = ConnectionFailedReason.ConnectionCanceled;
                            goto IL_1FC;
                        case DisconnectedReason.Timeout:
                        case DisconnectedReason.Kicked:
                            break;
                        case DisconnectedReason.ServerAtCapacity:
                            connectionFailedReason = ConnectionFailedReason.ServerAtCapacity;
                            goto IL_1FC;
                        case DisconnectedReason.ServerConnectionClosed:
                            connectionFailedReason = ConnectionFailedReason.ServerDoesNotExist;
                            goto IL_1FC;
                        default:
                            if (disconnectedReason == DisconnectedReason.NetworkDisconnected)
                            {
                                connectionFailedReason = ConnectionFailedReason.NetworkNotConnected;
                                goto IL_1FC;
                            }
                            break;
                    }
                    connectionFailedReason = ConnectionFailedReason.ServerUnreachable;
                }
            IL_1FC:
                Action<ConnectionFailedReason> action2 = this.connectionFailedEvent;
                if (action2 == null)
                {
                    return;
                }
                action2(connectionFailedReason);
                return;
            }
            else
            {
                Action<DisconnectedReason> action3 = this.disconnectedEvent;
                if (action3 == null)
                {
                    return;
                }
                action3(disconnectedReason);
                return;
            }
        }
    }

    // Token: 0x06000468 RID: 1128 RVA: 0x0000B294 File Offset: 0x00009494
    private bool TryUpdateConnectedPlayer(IConnectedPlayer player, bool isPlayerConnected = true)
    {
        isPlayerConnected &= (this.isConnected && player.isConnected && !player.isKicked && player.HasState("player") && (this.isConnectionOwner || player.sortIndex != -1) && player.hasValidLatency);
        if (isPlayerConnected && this._connectedPlayers.Contains(player))
        {
            return true;
        }
        if (isPlayerConnected)
        {
            int num = this.LocalPlayerHasState("player") ? (this._maxPlayerCount - 1) : this._maxPlayerCount;
            if (this.isConnectionOwner && this._connectedPlayers.Count >= num)
            {
                this._connectedPlayerManager.KickPlayer(player.userId, DisconnectedReason.ServerAtCapacity);
            }
            else
            {
                if (this.isConnectionOwner)
                {
                    this._connectedPlayerManager.SetPlayerSortIndex(player, this.GetNextAvailableSortIndex());
                }
                this._connectedPlayers.InsertSorted(player, (IConnectedPlayer p) => p.sortIndex);
                Action<IConnectedPlayer> action = this.playerConnectedEvent;
                if (action != null)
                {
                    action(player);
                }
            }
        }
        else if (this._connectedPlayers.Remove(player))
        {
            if (this.isConnectionOwner)
            {
                this._freeSortIndices.Enqueue(player.sortIndex);
                ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
                if (connectedPlayerManager != null)
                {
                    connectedPlayerManager.SetPlayerSortIndex(player, -1);
                }
            }
            Action<IConnectedPlayer> action2 = this.playerDisconnectedEvent;
            if (action2 != null)
            {
                action2(player);
            }
        }
        return false;
    }

    // Token: 0x06000469 RID: 1129 RVA: 0x0000B3F0 File Offset: 0x000095F0
    private int GetNextAvailableSortIndex()
    {
        if (this._freeSortIndices.Count > 0)
        {
            return this._freeSortIndices.Dequeue();
        }
        if (this.LocalPlayerHasState("player"))
        {
            return this._connectedPlayers.Count + 1;
        }
        return this._connectedPlayers.Count;
    }

    // Token: 0x0400017B RID: 379
    private const string kMultiplayerSessionState = "multiplayer_session";

    // Token: 0x0400017C RID: 380
    private readonly NetworkPacketSerializer<MultiplayerSessionManager.MessageType, IConnectedPlayer> _packetSerializer = new NetworkPacketSerializer<MultiplayerSessionManager.MessageType, IConnectedPlayer>();

    // Token: 0x0400017D RID: 381
    private readonly List<IConnectedPlayer> _connectedPlayers = new List<IConnectedPlayer>();

    // Token: 0x0400017E RID: 382
    private readonly HashSet<string> _localPlayerState = new HashSet<string>();

    // Token: 0x0400017F RID: 383
    private int _maxPlayerCount;

    // Token: 0x04000180 RID: 384
    private MultiplayerSessionManager.ConnectionState _connectionState;

    // Token: 0x04000181 RID: 385
    private readonly Queue<int> _freeSortIndices = new Queue<int>();

    // Token: 0x0400018C RID: 396
    private ConnectedPlayerManager _connectedPlayerManager;

    // Token: 0x0200012F RID: 303
    public enum MessageType : byte
    {
        // Token: 0x040003EC RID: 1004
        MenuRpc,
        // Token: 0x040003ED RID: 1005
        GameplayRpc,
        // Token: 0x040003EE RID: 1006
        NodePoseSyncState,
        // Token: 0x040003EF RID: 1007
        ScoreSyncState,
        // Token: 0x040003F0 RID: 1008
        NodePoseSyncStateDelta,
        // Token: 0x040003F1 RID: 1009
        ScoreSyncStateDelta
    }

    // Token: 0x02000130 RID: 304
    public enum SessionType
    {
        // Token: 0x040003F3 RID: 1011
        Player,
        // Token: 0x040003F4 RID: 1012
        Spectator,
        // Token: 0x040003F5 RID: 1013
        DedicatedServer
    }

    // Token: 0x02000131 RID: 305
    private enum ConnectionState
    {
        // Token: 0x040003F7 RID: 1015
        Disconnected,
        // Token: 0x040003F8 RID: 1016
        Connecting,
        // Token: 0x040003F9 RID: 1017
        Connected,
        // Token: 0x040003FA RID: 1018
        Disconnecting
    }
}
