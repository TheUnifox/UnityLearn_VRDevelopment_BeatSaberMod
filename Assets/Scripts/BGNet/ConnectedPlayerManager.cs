using System;
using System.Collections.Generic;
using System.Diagnostics;
using BGNet.Core;
using BGNet.Logging;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class ConnectedPlayerManager : IDisposable
{
    // Token: 0x0600003B RID: 59 RVA: 0x00002AD9 File Offset: 0x00000CD9
    private byte GetNextConnectionId()
    {
        do
        {
            this._lastConnectionId += 1;
            if (this._lastConnectionId == 127)
            {
                this._lastConnectionId = 1;
            }
        }
        while (this.GetPlayer(this._lastConnectionId) != null);
        return this._lastConnectionId;
    }

    // Token: 0x0600003C RID: 60 RVA: 0x00002B0F File Offset: 0x00000D0F
    private void RemoveAllPlayers(DisconnectedReason reason)
    {
        while (this._players.Count > 0)
        {
            this.RemovePlayer(this._players[0], reason);
        }
    }

    // Token: 0x0600003D RID: 61 RVA: 0x00002B34 File Offset: 0x00000D34
    private void RemovePlayer(ConnectedPlayerManager.ConnectedPlayer player, DisconnectedReason reason)
    {
        if (this._players.Remove(player))
        {
            player.Disconnect(reason);
            Action<IConnectedPlayer> action = this.playerDisconnectedEvent;
            if (action != null)
            {
                action(player);
            }
            if (player.isConnectionOwner && this.isConnected)
            {
                this.Disconnect(reason);
            }
        }
    }

    // Token: 0x0600003E RID: 62 RVA: 0x00002B74 File Offset: 0x00000D74
    private void AddPlayer(ConnectedPlayerManager.ConnectedPlayer player)
    {
        this.SendImmediately(ConnectedPlayerManager.PingPacket.pool.Obtain().Init(this.runTime), false);
        this._lastPingTime = this.runTime;
        this.SendImmediatelyExcludingPlayer(player.GetPlayerConnectedPacket(), player, true);
        if (player.isDirectConnection)
        {
            for (int i = 0; i < this._players.Count; i++)
            {
                this.SendImmediatelyToPlayer(this._players[i].GetPlayerConnectedPacket(), player);
                if (this._players[i].sortIndex != -1)
                {
                    this.SendImmediatelyToPlayer(this._players[i].GetPlayerSortOrderPacket(), player);
                }
                this.SendImmediatelyFromPlayerToPlayer(this._players[i].GetPlayerIdentityPacket(), this._players[i], player);
            }
            if (this._localPlayer.sortIndex != -1)
            {
                this.SendImmediatelyToPlayer(this._localPlayer.GetPlayerSortOrderPacket(), player);
            }
            this.SendImmediatelyToPlayer(this._localPlayer.GetPlayerIdentityPacket(), player);
        }
        this._players.Add(player);
        Action<IConnectedPlayer> action = this.playerConnectedEvent;
        if (action == null)
        {
            return;
        }
        action(player);
    }

    // Token: 0x0600003F RID: 63 RVA: 0x00002C90 File Offset: 0x00000E90
    private ConnectedPlayerManager.ConnectedPlayer GetPlayer(byte connectionId)
    {
        for (int i = 0; i < this._players.Count; i++)
        {
            if (this._players[i].connectionId == connectionId)
            {
                return this._players[i];
            }
        }
        return null;
    }

    // Token: 0x06000040 RID: 64 RVA: 0x00002CD8 File Offset: 0x00000ED8
    private ConnectedPlayerManager.ConnectedPlayer GetPlayer(IConnection connection, byte remoteConnectionId)
    {
        for (int i = 0; i < this._players.Count; i++)
        {
            if (object.Equals(this._players[i].connection, connection) && this._players[i].remoteConnectionId == remoteConnectionId)
            {
                return this._players[i];
            }
        }
        return null;
    }

    // Token: 0x06000041 RID: 65 RVA: 0x00002D38 File Offset: 0x00000F38
    private ConnectedPlayerManager.ConnectedPlayer GetPlayer(string userId)
    {
        for (int i = 0; i < this._players.Count; i++)
        {
            if (object.Equals(this._players[i].userId, userId))
            {
                return this._players[i];
            }
        }
        return null;
    }

    // Token: 0x06000042 RID: 66 RVA: 0x00002D82 File Offset: 0x00000F82
    public IConnectedPlayer GetConnectedPlayer(int index)
    {
        return this._players[index];
    }

    // Token: 0x14000001 RID: 1
    // (add) Token: 0x06000043 RID: 67 RVA: 0x00002D90 File Offset: 0x00000F90
    // (remove) Token: 0x06000044 RID: 68 RVA: 0x00002DC8 File Offset: 0x00000FC8
    public event Action connectedEvent;

    // Token: 0x14000002 RID: 2
    // (add) Token: 0x06000045 RID: 69 RVA: 0x00002E00 File Offset: 0x00001000
    // (remove) Token: 0x06000046 RID: 70 RVA: 0x00002E38 File Offset: 0x00001038
    public event Action initializedEvent;

    // Token: 0x14000003 RID: 3
    // (add) Token: 0x06000047 RID: 71 RVA: 0x00002E70 File Offset: 0x00001070
    // (remove) Token: 0x06000048 RID: 72 RVA: 0x00002EA8 File Offset: 0x000010A8
    public event Action<DisconnectedReason> disconnectedEvent;

    // Token: 0x14000004 RID: 4
    // (add) Token: 0x06000049 RID: 73 RVA: 0x00002EE0 File Offset: 0x000010E0
    // (remove) Token: 0x0600004A RID: 74 RVA: 0x00002F18 File Offset: 0x00001118
    public event Action<ConnectionFailedReason> connectionFailedEvent;

    // Token: 0x14000005 RID: 5
    // (add) Token: 0x0600004B RID: 75 RVA: 0x00002F50 File Offset: 0x00001150
    // (remove) Token: 0x0600004C RID: 76 RVA: 0x00002F88 File Offset: 0x00001188
    public event Action<IConnectedPlayer> playerConnectedEvent;

    // Token: 0x14000006 RID: 6
    // (add) Token: 0x0600004D RID: 77 RVA: 0x00002FC0 File Offset: 0x000011C0
    // (remove) Token: 0x0600004E RID: 78 RVA: 0x00002FF8 File Offset: 0x000011F8
    public event Action<IConnectedPlayer> playerDisconnectedEvent;

    // Token: 0x14000007 RID: 7
    // (add) Token: 0x0600004F RID: 79 RVA: 0x00003030 File Offset: 0x00001230
    // (remove) Token: 0x06000050 RID: 80 RVA: 0x00003068 File Offset: 0x00001268
    public event Action<IConnectedPlayer> playerStateChangedEvent;

    // Token: 0x14000008 RID: 8
    // (add) Token: 0x06000051 RID: 81 RVA: 0x000030A0 File Offset: 0x000012A0
    // (remove) Token: 0x06000052 RID: 82 RVA: 0x000030D8 File Offset: 0x000012D8
    public event Action<IConnectedPlayer> playerAvatarChangedEvent;

    // Token: 0x14000009 RID: 9
    // (add) Token: 0x06000053 RID: 83 RVA: 0x00003110 File Offset: 0x00001310
    // (remove) Token: 0x06000054 RID: 84 RVA: 0x00003148 File Offset: 0x00001348
    public event Action<IConnectedPlayer> playerOrderChangedEvent;

    // Token: 0x1400000A RID: 10
    // (add) Token: 0x06000055 RID: 85 RVA: 0x00003180 File Offset: 0x00001380
    // (remove) Token: 0x06000056 RID: 86 RVA: 0x000031B8 File Offset: 0x000013B8
    public event Action<IConnectedPlayer> playerLatencyInitializedEvent;

    // Token: 0x1400000B RID: 11
    // (add) Token: 0x06000057 RID: 87 RVA: 0x000031F0 File Offset: 0x000013F0
    // (remove) Token: 0x06000058 RID: 88 RVA: 0x00003228 File Offset: 0x00001428
    public event Action syncTimeInitializedEvent;

    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000059 RID: 89 RVA: 0x0000325D File Offset: 0x0000145D
    public bool isConnectionOwner
    {
        get
        {
            return this._connectionManager.isConnectionOwner;
        }
    }

    // Token: 0x17000008 RID: 8
    // (get) Token: 0x0600005A RID: 90 RVA: 0x0000326A File Offset: 0x0000146A
    public bool isConnectedOrConnecting
    {
        get
        {
            return this.isConnected || this.isConnecting;
        }
    }

    // Token: 0x17000009 RID: 9
    // (get) Token: 0x0600005B RID: 91 RVA: 0x0000327C File Offset: 0x0000147C
    public bool isConnected
    {
        get
        {
            return this._connectionManager.isConnected;
        }
    }

    // Token: 0x1700000A RID: 10
    // (get) Token: 0x0600005C RID: 92 RVA: 0x00003289 File Offset: 0x00001489
    public bool isConnecting
    {
        get
        {
            return this._connectionManager.isConnecting;
        }
    }

    // Token: 0x1700000B RID: 11
    // (get) Token: 0x0600005D RID: 93 RVA: 0x00003296 File Offset: 0x00001496
    public bool isDisconnecting
    {
        get
        {
            return this._connectionManager.isDisconnecting;
        }
    }

    // Token: 0x1700000C RID: 12
    // (get) Token: 0x0600005E RID: 94 RVA: 0x000032A3 File Offset: 0x000014A3
    public IConnectedPlayer localPlayer
    {
        get
        {
            return this._localPlayer;
        }
    }

    // Token: 0x1700000D RID: 13
    // (get) Token: 0x0600005F RID: 95 RVA: 0x000032AB File Offset: 0x000014AB
    public int connectedPlayerCount
    {
        get
        {
            return this._players.Count;
        }
    }

    // Token: 0x1700000E RID: 14
    // (get) Token: 0x06000060 RID: 96 RVA: 0x000032B8 File Offset: 0x000014B8
    public float syncTime
    {
        get
        {
            return this.runTime + this._syncTimeOffset.currentAverage;
        }
    }

    // Token: 0x1700000F RID: 15
    // (get) Token: 0x06000061 RID: 97 RVA: 0x000032CC File Offset: 0x000014CC
    public bool syncTimeInitialized
    {
        get
        {
            return this.isConnectionOwner || this._syncTimeOffset.hasValue;
        }
    }

    // Token: 0x17000010 RID: 16
    // (get) Token: 0x06000062 RID: 98 RVA: 0x000032E3 File Offset: 0x000014E3
    private float runTime
    {
        get
        {
            return (float)((double)(this._timeProvider.GetTicks() - this._startTicks) / 10000000.0);
        }
    }

    // Token: 0x06000063 RID: 99 RVA: 0x00003303 File Offset: 0x00001503
    public ConnectedPlayerManager(IConnectionManager connectionManager) : this(DefaultTimeProvider.instance, DefaultTaskUtility.instance, connectionManager)
    {
    }

    // Token: 0x06000064 RID: 100 RVA: 0x00003318 File Offset: 0x00001518
    public ConnectedPlayerManager(BGNet.Core.ITimeProvider timeProvider, ITaskUtility taskUtility, IConnectionManager connectionManager)
    {
        this._timeProvider = timeProvider;
        this._taskUtility = taskUtility;
        this._startTicks = this._timeProvider.GetTicks();
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PlayerConnectedPacket>(ConnectedPlayerManager.InternalMessageType.PlayerConnected, new Action<ConnectedPlayerManager.PlayerConnectedPacket, IConnectedPlayer>(this.HandleServerPlayerConnected), new Func<ConnectedPlayerManager.PlayerConnectedPacket>(ConnectedPlayerManager.PlayerConnectedPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PlayerIdentityPacket>(ConnectedPlayerManager.InternalMessageType.PlayerIdentity, new Action<ConnectedPlayerManager.PlayerIdentityPacket, IConnectedPlayer>(this.HandlePlayerIdentityUpdate), new Func<ConnectedPlayerManager.PlayerIdentityPacket>(ConnectedPlayerManager.PlayerIdentityPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PlayerDisconnectedPacket>(ConnectedPlayerManager.InternalMessageType.PlayerDisconnected, new Action<ConnectedPlayerManager.PlayerDisconnectedPacket, IConnectedPlayer>(this.HandleServerPlayerDisconnected), new Func<ConnectedPlayerManager.PlayerDisconnectedPacket>(ConnectedPlayerManager.PlayerDisconnectedPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PlayerSortOrderPacket>(ConnectedPlayerManager.InternalMessageType.PlayerSortOrderUpdate, new Action<ConnectedPlayerManager.PlayerSortOrderPacket, IConnectedPlayer>(this.HandlePlayerSortOrderUpdate), new Func<ConnectedPlayerManager.PlayerSortOrderPacket>(ConnectedPlayerManager.PlayerSortOrderPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.SyncTimePacket>(ConnectedPlayerManager.InternalMessageType.SyncTime, new Action<ConnectedPlayerManager.SyncTimePacket, IConnectedPlayer>(this.HandleSyncTimePacket), new Func<ConnectedPlayerManager.SyncTimePacket>(ConnectedPlayerManager.SyncTimePacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.KickPlayerPacket>(ConnectedPlayerManager.InternalMessageType.KickPlayer, new Action<ConnectedPlayerManager.KickPlayerPacket, IConnectedPlayer>(this.HandleKickPlayerPacket), new Func<ConnectedPlayerManager.KickPlayerPacket>(ConnectedPlayerManager.KickPlayerPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PlayerAvatarPacket>(ConnectedPlayerManager.InternalMessageType.PlayerAvatarUpdate, new Action<ConnectedPlayerManager.PlayerAvatarPacket, IConnectedPlayer>(this.HandlePlayerAvatarUpdate), new Func<ConnectedPlayerManager.PlayerAvatarPacket>(ConnectedPlayerManager.PlayerAvatarPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PlayerStatePacket>(ConnectedPlayerManager.InternalMessageType.PlayerStateUpdate, new Action<ConnectedPlayerManager.PlayerStatePacket, IConnectedPlayer>(this.HandlePlayerStateUpdate), new Func<ConnectedPlayerManager.PlayerStatePacket>(ConnectedPlayerManager.PlayerStatePacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PingPacket>(ConnectedPlayerManager.InternalMessageType.Ping, new Action<ConnectedPlayerManager.PingPacket, IConnectedPlayer>(this.HandlePing), new Func<ConnectedPlayerManager.PingPacket>(ConnectedPlayerManager.PingPacket.pool.Obtain));
        this._messageSerializer.RegisterCallback<ConnectedPlayerManager.PongPacket>(ConnectedPlayerManager.InternalMessageType.Pong, new Action<ConnectedPlayerManager.PongPacket, IConnectedPlayer>(this.HandlePong), new Func<ConnectedPlayerManager.PongPacket>(ConnectedPlayerManager.PongPacket.pool.Obtain));
        this.ResetLocalState();
        this._connectionManager = connectionManager;
        this._connectionManager.onInitializedEvent += this.HandleInitialized;
        this._connectionManager.onConnectedEvent += this.HandleConnected;
        this._connectionManager.onDisconnectedEvent += this.HandleDisconnected;
        this._connectionManager.onConnectionFailedEvent += this.HandleConnectionFailed;
        this._connectionManager.onConnectionConnectedEvent += this.HandleConnectionConnected;
        this._connectionManager.onConnectionDisconnectedEvent += this.HandleConnectionDisconnected;
        this._connectionManager.onReceivedDataEvent += this.HandleNetworkReceive;
        if (this._connectionManager.isConnected)
        {
            this.HandleConnected();
        }
        for (int i = 0; i < this._connectionManager.connectionCount; i++)
        {
            this.HandleConnectionConnected(this._connectionManager.GetConnection(i));
        }
    }

    // Token: 0x06000065 RID: 101 RVA: 0x0000362C File Offset: 0x0000182C
    public void PollUpdate(int frameCount)
    {
        if (this._lastPollFrame == frameCount)
        {
            return;
        }
        this._lastPollFrame = frameCount;
        this._connectionManager.PollUpdate();
        this._lastPollTime = this.runTime;
        if (!this.isConnected)
        {
            return;
        }
        if (this._lastPingTime < this.runTime - 2f)
        {
            this.Send<ConnectedPlayerManager.PingPacket>(ConnectedPlayerManager.PingPacket.pool.Obtain().Init(this.runTime));
            this._lastPingTime = this.runTime;
        }
        this.FlushReliableQueue();
        this.FlushUnreliableQueue();
    }

    // Token: 0x06000066 RID: 102 RVA: 0x000036B1 File Offset: 0x000018B1
    public void PollLateUpdateOptional()
    {
        if (!this.isConnected)
        {
            return;
        }
        this.FlushReliableQueue();
        this.FlushUnreliableQueue();
    }

    // Token: 0x06000067 RID: 103 RVA: 0x000036C8 File Offset: 0x000018C8
    public void RegisterSerializer(ConnectedPlayerManager.MessageType serializerType, INetworkPacketSubSerializer<IConnectedPlayer> subSerializer)
    {
        this._messageSerializer.RegisterSubSerializer((ConnectedPlayerManager.InternalMessageType)serializerType, subSerializer);
    }

    // Token: 0x06000068 RID: 104 RVA: 0x000036D7 File Offset: 0x000018D7
    public void UnregisterSerializer(ConnectedPlayerManager.MessageType serializerType, INetworkPacketSubSerializer<IConnectedPlayer> subSerializer)
    {
        this._messageSerializer.UnregisterSubSerializer((ConnectedPlayerManager.InternalMessageType)serializerType, subSerializer);
    }

    // Token: 0x06000069 RID: 105 RVA: 0x000036E6 File Offset: 0x000018E6
    public T GetConnectionManager<T>() where T : class, IConnectionManager
    {
        return this._connectionManager as T;
    }

    // Token: 0x0600006A RID: 106 RVA: 0x000036F8 File Offset: 0x000018F8
    public void Dispose()
    {
        this.Disconnect(DisconnectedReason.UserInitiated);
        this._connectionManager.onInitializedEvent -= this.HandleInitialized;
        this._connectionManager.onConnectedEvent -= this.HandleConnected;
        this._connectionManager.onDisconnectedEvent -= this.HandleDisconnected;
        this._connectionManager.onConnectionFailedEvent -= this.HandleConnectionFailed;
        this._connectionManager.onConnectionConnectedEvent -= this.HandleConnectionConnected;
        this._connectionManager.onConnectionDisconnectedEvent -= this.HandleConnectionDisconnected;
        this._connectionManager.onReceivedDataEvent -= this.HandleNetworkReceive;
        this._connectionManager.Dispose();
    }

    // Token: 0x0600006B RID: 107 RVA: 0x000037B8 File Offset: 0x000019B8
    public void Disconnect(DisconnectedReason disconnectedReason = DisconnectedReason.UserInitiated)
    {
        this._connectionManager.Disconnect(disconnectedReason);
    }

    // Token: 0x0600006C RID: 108 RVA: 0x000037C8 File Offset: 0x000019C8
    public void KickPlayer(string userId, DisconnectedReason disconnectedReason = DisconnectedReason.Kicked)
    {
        ConnectedPlayerManager.ConnectedPlayer player = this.GetPlayer(userId);
        if (player == null)
        {
            return;
        }
        if (this.isConnectionOwner)
        {
            this.SendImmediatelyToPlayer(ConnectedPlayerManager.KickPlayerPacket.pool.Obtain().Init(disconnectedReason), player);
            player.SetKicked();
            Action<IConnectedPlayer> action = this.playerStateChangedEvent;
            if (action == null)
            {
                return;
            }
            action(player);
        }
    }

    // Token: 0x0600006D RID: 109 RVA: 0x00003818 File Offset: 0x00001A18
    public void SetLocalPlayerState(string state, bool setState)
    {
        bool flag;
        if (setState)
        {
            flag = this._localPlayerState.Add(state);
        }
        else
        {
            flag = this._localPlayerState.Remove(state);
        }
        if (flag && this._localPlayer != null)
        {
            this._localPlayer.SetPlayerState(new PlayerStateHash(this._localPlayerState));
            this.SendImmediately(this._localPlayer.GetPlayerStatePacket(), false);
        }
    }

    // Token: 0x0600006E RID: 110 RVA: 0x00003879 File Offset: 0x00001A79
    public void SetLocalPlayerAvatar(MultiplayerAvatarData multiplayerAvatarData)
    {
        if (!this._localPlayerAvatar.Equals(multiplayerAvatarData))
        {
            this._localPlayerAvatar = multiplayerAvatarData;
            if (this._localPlayer != null)
            {
                this._localPlayer.SetPlayerAvatar(multiplayerAvatarData);
                this.SendImmediately(this._localPlayer.GetPlayerAvatarPacket(), false);
            }
        }
    }

    // Token: 0x0600006F RID: 111 RVA: 0x000038B6 File Offset: 0x00001AB6
    public void SetLocalPlayerSortIndex(int sortIndex)
    {
        this.SetPlayerSortIndex(this._localPlayer, sortIndex);
    }

    // Token: 0x06000070 RID: 112 RVA: 0x000038C8 File Offset: 0x00001AC8
    public void SetPlayerSortIndex(IConnectedPlayer player, int sortIndex)
    {
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer;
        if (!this.isConnectionOwner || (connectedPlayer = (player as ConnectedPlayerManager.ConnectedPlayer)) == null)
        {
            return;
        }
        if (connectedPlayer.UpdateSortIndex(sortIndex) && this.isConnected)
        {
            this.SendImmediately(connectedPlayer.GetPlayerSortOrderPacket(), true);
        }
    }

    // Token: 0x06000071 RID: 113 RVA: 0x00003906 File Offset: 0x00001B06
    private void ResetLocalState()
    {
        this._localPlayer = null;
        this._lastConnectionId = 0;
        this._syncTimeOffset.Reset();
    }

    // Token: 0x06000072 RID: 114 RVA: 0x00003921 File Offset: 0x00001B21
    private void HandleInitialized()
    {
        Action action = this.initializedEvent;
        if (action == null)
        {
            return;
        }
        action();
    }

    // Token: 0x06000073 RID: 115 RVA: 0x00003934 File Offset: 0x00001B34
    private void HandleConnected()
    {
        this._encryptionKeys = DiffieHellmanUtility.GenerateKeys(DiffieHellmanUtility.KeyType.ElipticalCurve);
        this._encryptionRandom = SecureRandomProvider.GetBytes(32);
        this._localPlayer = ConnectedPlayerManager.ConnectedPlayer.CreateLocalPlayer(this, this._connectionManager.userId, this._connectionManager.userName, this._connectionManager.isConnectionOwner, this._encryptionKeys.publicKey, this._encryptionRandom);
        this._localPlayer.SetPlayerState(new PlayerStateHash(this._localPlayerState));
        this._localPlayer.SetPlayerAvatar(this._localPlayerAvatar);
        Action action = this.connectedEvent;
        if (action == null)
        {
            return;
        }
        action();
    }

    // Token: 0x06000074 RID: 116 RVA: 0x000039CF File Offset: 0x00001BCF
    private void HandleDisconnected(DisconnectedReason disconnectedReason)
    {
        this.RemoveAllPlayers(disconnectedReason);
        this.ResetLocalState();
        Action<DisconnectedReason> action = this.disconnectedEvent;
        if (action == null)
        {
            return;
        }
        action(disconnectedReason);
    }

    // Token: 0x06000075 RID: 117 RVA: 0x000039EF File Offset: 0x00001BEF
    private void HandleConnectionFailed(ConnectionFailedReason reason)
    {
        Action<ConnectionFailedReason> action = this.connectionFailedEvent;
        if (action == null)
        {
            return;
        }
        action(reason);
    }

    // Token: 0x06000076 RID: 118 RVA: 0x00003A02 File Offset: 0x00001C02
    private void HandleConnectionConnected(IConnection connection)
    {
        this.AddPlayer(ConnectedPlayerManager.ConnectedPlayer.CreateDirectlyConnectedPlayer(this, this.GetNextConnectionId(), connection));
    }

    // Token: 0x06000077 RID: 119 RVA: 0x00003A18 File Offset: 0x00001C18
    private void HandleConnectionDisconnected(IConnection connection, DisconnectedReason disconnectedReason)
    {
        for (int i = this._players.Count - 1; i >= 0; i--)
        {
            ConnectedPlayerManager.ConnectedPlayer connectedPlayer = this._players[i];
            if (object.Equals(connectedPlayer.connection, connection))
            {
                this.SendImmediatelyFromPlayer(ConnectedPlayerManager.PlayerDisconnectedPacket.pool.Obtain().Init(disconnectedReason), connectedPlayer, false);
                this.RemovePlayer(connectedPlayer, disconnectedReason);
                if (i > this._players.Count)
                {
                    i = this._players.Count;
                }
            }
        }
    }

    // Token: 0x06000078 RID: 120 RVA: 0x00003A94 File Offset: 0x00001C94
    private void HandleNetworkReceive(IConnection connection, NetDataReader reader, DeliveryMethod deliveryMethod)
    {
        byte remoteConnectionId;
        byte b;
        if (!reader.TryGetByte(out remoteConnectionId) || !reader.TryGetByte(out b) || reader.AvailableBytes == 0)
        {
            return;
        }
        ConnectedPlayerManager.ConnectedPlayer player = this.GetPlayer(connection, remoteConnectionId);
        if (player == null)
        {
            return;
        }
        byte b2 = (byte)(b & 0b10000000);
        b &= 127;
        if (b == 127 && b2 != 0)
        {
            return;
        }
        if (b != 0 && this._connectionManager.connectionCount > 1)
        {
            if (b == 127)
            {
                this._temporaryDataWriter.Reset();
                this._temporaryDataWriter.Put(player.connectionId);
                this._temporaryDataWriter.Put(127);
                this._temporaryDataWriter.Put(reader.RawData, reader.Position, reader.AvailableBytes);
                this._connectionManager.SendToAll(this._temporaryDataWriter, deliveryMethod, connection);
            }
            else
            {
                ConnectedPlayerManager.ConnectedPlayer player2 = this.GetPlayer(b);
                if (player2 != null && player2.connection != connection)
                {
                    this._temporaryDataWriter.Reset();
                    this._temporaryDataWriter.Put(player.connectionId);
                    this._temporaryDataWriter.Put(player2.remoteConnectionId | b2);
                    this._temporaryDataWriter.Put(reader.RawData, reader.Position, reader.AvailableBytes);
                    player2.connection.Send(this._temporaryDataWriter, deliveryMethod);
                }
            }
        }
        if (b != 0 && b != 127)
        {
            return;
        }
        if (b2 != 0)
        {
            byte[] rawData = reader.RawData;
            int position = reader.Position;
            int availableBytes = reader.AvailableBytes;
            if (player.encryptionState == null || !player.encryptionState.TryDecryptData(rawData, ref position, ref availableBytes) || availableBytes == 0)
            {
                return;
            }
            reader.SetSource(rawData, position, availableBytes + position);
        }
        try
        {
            this._messageSerializer.ProcessAllPackets(reader, player);
        }
        catch (Exception)
        {
            if (this.isConnectionOwner)
            {
                this.KickPlayer(player.userId, DisconnectedReason.Kicked);
            }
        }
    }

    // Token: 0x06000079 RID: 121 RVA: 0x00003C60 File Offset: 0x00001E60
    public void Send<T>(T message) where T : INetSerializable
    {
        if (!this.isConnected)
        {
            IPoolablePacket poolablePacket;
            if ((poolablePacket = (message as IPoolablePacket)) != null)
            {
                poolablePacket.Release();
            }
            return;
        }
        if (this._reliableDataWriter.Length == 0)
        {
            this._reliableDataWriter.Put(0);
            this._reliableDataWriter.Put(127);
        }
        this.Write(this._reliableDataWriter, message);
    }

    // Token: 0x0600007A RID: 122 RVA: 0x00003CC4 File Offset: 0x00001EC4
    public void SendUnreliable<T>(T message) where T : INetSerializable
    {
        if (!this.isConnected)
        {
            IPoolablePacket poolablePacket;
            if ((poolablePacket = (message as IPoolablePacket)) != null)
            {
                poolablePacket.Release();
            }
            return;
        }
        this._temporaryDataWriter.Reset();
        this.Write(this._temporaryDataWriter, message);
        if (this._temporaryDataWriter.Length + 2 > 412)
        {
            return;
        }
        if (this._unreliableDataWriter.Length + this._temporaryDataWriter.Length > 412)
        {
            this.FlushUnreliableQueue();
        }
        if (this._unreliableDataWriter.Length == 0)
        {
            this._unreliableDataWriter.Put(0);
            this._unreliableDataWriter.Put(127);
        }
        this._unreliableDataWriter.Put(this._temporaryDataWriter.Data, 0, this._temporaryDataWriter.Length);
    }

    // Token: 0x0600007B RID: 123 RVA: 0x00003D90 File Offset: 0x00001F90
    public void SendToPlayer<T>(T message, IConnectedPlayer player) where T : INetSerializable
    {
        if (!this.isConnected)
        {
            IPoolablePacket poolablePacket;
            if ((poolablePacket = (message as IPoolablePacket)) != null)
            {
                poolablePacket.Release();
            }
            return;
        }
        this.SendImmediatelyToPlayer(message, (ConnectedPlayerManager.ConnectedPlayer)player);
    }

    // Token: 0x0600007C RID: 124 RVA: 0x00003DD0 File Offset: 0x00001FD0
    public void SendUnreliableEncryptedToPlayer<T>(T message, IConnectedPlayer player) where T : INetSerializable
    {
        if (!this.isConnected)
        {
            IPoolablePacket poolablePacket;
            if ((poolablePacket = (message as IPoolablePacket)) != null)
            {
                poolablePacket.Release();
            }
            return;
        }
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer = (ConnectedPlayerManager.ConnectedPlayer)player;
        if (connectedPlayer.encryptionState == null)
        {
            return;
        }
        connectedPlayer.connection.Send(this.WriteOneEncrypted(connectedPlayer.encryptionState, this._localPlayer.connectionId, connectedPlayer.remoteConnectionId, message), DeliveryMethod.Unreliable);
    }

    // Token: 0x0600007D RID: 125 RVA: 0x00003E3A File Offset: 0x0000203A
    private void SendImmediately(INetSerializable message, bool onlyFirstDegree = false)
    {
        this._connectionManager.SendToAll(this.WriteOne(this._localPlayer.connectionId, (byte)(onlyFirstDegree ? 0 : 127), message), DeliveryMethod.ReliableOrdered);
    }

    // Token: 0x0600007E RID: 126 RVA: 0x00003E62 File Offset: 0x00002062
    private void SendImmediatelyExcludingPlayer(INetSerializable message, ConnectedPlayerManager.ConnectedPlayer excludingPlayer, bool onlyFirstDegree = false)
    {
        this._connectionManager.SendToAll(this.WriteOne(this._localPlayer.connectionId, (byte)(onlyFirstDegree ? 0 : 127), message), DeliveryMethod.ReliableOrdered, excludingPlayer.connection);
    }

    // Token: 0x0600007F RID: 127 RVA: 0x00003E90 File Offset: 0x00002090
    private void SendImmediatelyToPlayer(INetSerializable message, ConnectedPlayerManager.ConnectedPlayer toPlayer)
    {
        toPlayer.connection.Send(this.WriteOne(this._localPlayer.connectionId, toPlayer.remoteConnectionId, message), DeliveryMethod.ReliableOrdered);
    }

    // Token: 0x06000080 RID: 128 RVA: 0x00003EB6 File Offset: 0x000020B6
    private void SendImmediatelyFromPlayer(INetSerializable message, ConnectedPlayerManager.ConnectedPlayer fromPlayer, bool onlyFirstDegree = false)
    {
        this._connectionManager.SendToAll(this.WriteOne(fromPlayer.connectionId, (byte)(onlyFirstDegree ? 0 : 127), message), DeliveryMethod.ReliableOrdered, fromPlayer.connection);
    }

    // Token: 0x06000081 RID: 129 RVA: 0x00003EDF File Offset: 0x000020DF
    private void SendImmediatelyFromPlayerToPlayer(INetSerializable message, ConnectedPlayerManager.ConnectedPlayer fromPlayer, ConnectedPlayerManager.ConnectedPlayer toPlayer)
    {
        toPlayer.connection.Send(this.WriteOne(fromPlayer.connectionId, toPlayer.remoteConnectionId, message), DeliveryMethod.ReliableOrdered);
    }

    // Token: 0x06000082 RID: 130 RVA: 0x00003F00 File Offset: 0x00002100
    private void FlushReliableQueue()
    {
        if (this._reliableDataWriter.Length <= 0)
        {
            return;
        }
        this._connectionManager.SendToAll(this._reliableDataWriter, DeliveryMethod.ReliableOrdered);
        this._reliableDataWriter.Reset();
    }

    // Token: 0x06000083 RID: 131 RVA: 0x00003F2E File Offset: 0x0000212E
    private void FlushUnreliableQueue()
    {
        if (this._unreliableDataWriter.Length <= 0)
        {
            return;
        }
        this._connectionManager.SendToAll(this._unreliableDataWriter, DeliveryMethod.Unreliable);
        this._unreliableDataWriter.Reset();
    }

    // Token: 0x06000084 RID: 132 RVA: 0x00003F5C File Offset: 0x0000215C
    private NetDataWriter WriteOne(byte senderId, byte receiverId, INetSerializable message)
    {
        this._temporaryDataWriter.Reset();
        this._temporaryDataWriter.Put(senderId);
        this._temporaryDataWriter.Put(receiverId);
        this.Write(this._temporaryDataWriter, message);
        return this._temporaryDataWriter;
    }

    // Token: 0x06000085 RID: 133 RVA: 0x00003F94 File Offset: 0x00002194
    private NetDataWriter WriteOneEncrypted(EncryptionUtility.IEncryptionState encryptionState, byte senderId, byte receiverId, INetSerializable message)
    {
        this._temporaryEncryptedDataWriter.Reset();
        this.Write(this._temporaryEncryptedDataWriter, message);
        this._temporaryEncryptedDataWriter.ResizeIfNeed(this._temporaryEncryptedDataWriter.Length + 62);
        byte[] data = this._temporaryEncryptedDataWriter.Data;
        int offset = 0;
        int length = this._temporaryEncryptedDataWriter.Length;
        encryptionState.EncryptData(data, ref offset, ref length, 0);
        this._temporaryDataWriter.Reset();
        this._temporaryDataWriter.Put(senderId);
        this._temporaryDataWriter.Put(receiverId | 128);
        this._temporaryDataWriter.Put(data, offset, length);
        return this._temporaryDataWriter;
    }

    // Token: 0x06000086 RID: 134 RVA: 0x00004038 File Offset: 0x00002238
    private void Write(NetDataWriter writer, INetSerializable packet)
    {
        this._messageSerializer.SerializePacket(writer, packet);
        IPoolablePacket poolablePacket;
        if ((poolablePacket = (packet as IPoolablePacket)) != null)
        {
            poolablePacket.Release();
        }
    }

    // Token: 0x06000087 RID: 135 RVA: 0x00004064 File Offset: 0x00002264
    private void HandleServerPlayerConnected(ConnectedPlayerManager.PlayerConnectedPacket packet, IConnectedPlayer iPlayer)
    {
        ConnectedPlayerManager.ConnectedPlayer parent = (ConnectedPlayerManager.ConnectedPlayer)iPlayer;
        if (this.GetPlayer(packet.userId) == null)
        {
            this.AddPlayer(ConnectedPlayerManager.ConnectedPlayer.CreateRemoteConnectedPlayer(this, this.GetNextConnectionId(), packet, parent));
        }
        packet.Release();
    }

    // Token: 0x06000088 RID: 136 RVA: 0x000040A0 File Offset: 0x000022A0
    private void HandlePlayerIdentityUpdate(ConnectedPlayerManager.PlayerIdentityPacket packet, IConnectedPlayer iPlayer)
    {
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer = (ConnectedPlayerManager.ConnectedPlayer)iPlayer;
        int num = (connectedPlayer.publicEncryptionKey != null && connectedPlayer.random != null) ? 1 : 0;
        connectedPlayer.UpdateIdentity(packet);
        this.SendImmediatelyFromPlayer(packet, connectedPlayer, true);
        bool flag = connectedPlayer.publicEncryptionKey != null && connectedPlayer.random != null;
        if (num == 0 && flag)
        {
            this.InitializePlayerEncryption(connectedPlayer);
        }
        Action<IConnectedPlayer> action = this.playerAvatarChangedEvent;
        if (action != null)
        {
            action(connectedPlayer);
        }
        Action<IConnectedPlayer> action2 = this.playerStateChangedEvent;
        if (action2 == null)
        {
            return;
        }
        action2(connectedPlayer);
    }

    // Token: 0x06000089 RID: 137 RVA: 0x00004120 File Offset: 0x00002320
    private async void InitializePlayerEncryption(ConnectedPlayerManager.ConnectedPlayer player)
    {
        bool isClient = string.Compare(this._localPlayer.userId, player.userId, StringComparison.Ordinal) < 0;
        byte[] preMasterSecret = await this._encryptionKeys.GetPreMasterSecretAsync(this._taskUtility, player.publicEncryptionKey);
        EncryptionUtility.IEncryptionState encryptionState = await EncryptionUtility.CreateEncryptionStateAsync(this._taskUtility, preMasterSecret, isClient ? player.random : this._encryptionRandom, isClient ? this._encryptionRandom : player.random, isClient);
        if (player.isConnected)
        {
            player.SetEncryptionState(encryptionState);
            Action<IConnectedPlayer> action = this.playerStateChangedEvent;
            if (action != null)
            {
                action(player);
            }
        }
        else
        {
            encryptionState.Dispose();
        }
    }

    // Token: 0x0600008A RID: 138 RVA: 0x00004164 File Offset: 0x00002364
    private void HandlePlayerStateUpdate(ConnectedPlayerManager.PlayerStatePacket packet, IConnectedPlayer iPlayer)
    {
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer = (ConnectedPlayerManager.ConnectedPlayer)iPlayer;
        connectedPlayer.UpdateState(packet);
        Action<IConnectedPlayer> action = this.playerStateChangedEvent;
        if (action == null)
        {
            return;
        }
        action(connectedPlayer);
    }

    // Token: 0x0600008B RID: 139 RVA: 0x00004190 File Offset: 0x00002390
    private void HandlePlayerAvatarUpdate(ConnectedPlayerManager.PlayerAvatarPacket packet, IConnectedPlayer iPlayer)
    {
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer = (ConnectedPlayerManager.ConnectedPlayer)iPlayer;
        connectedPlayer.UpdateAvatar(packet);
        Action<IConnectedPlayer> action = this.playerAvatarChangedEvent;
        if (action == null)
        {
            return;
        }
        action(connectedPlayer);
    }

    // Token: 0x0600008C RID: 140 RVA: 0x000041BC File Offset: 0x000023BC
    private void HandleServerPlayerDisconnected(ConnectedPlayerManager.PlayerDisconnectedPacket packet, IConnectedPlayer iPlayer)
    {
        DisconnectedReason disconnectedReason = packet.disconnectedReason;
        packet.Release();
        ConnectedPlayerManager.ConnectedPlayer player = (ConnectedPlayerManager.ConnectedPlayer)iPlayer;
        this.RemovePlayer(player, disconnectedReason);
    }

    // Token: 0x0600008D RID: 141 RVA: 0x000041E8 File Offset: 0x000023E8
    private void HandleKickPlayerPacket(ConnectedPlayerManager.KickPlayerPacket packet, IConnectedPlayer iPlayer)
    {
        DisconnectedReason disconnectedReason = packet.disconnectedReason;
        packet.Release();
        if (!iPlayer.isConnectionOwner)
        {
            return;
        }
        this.Disconnect(disconnectedReason);
    }

    // Token: 0x0600008E RID: 142 RVA: 0x00004214 File Offset: 0x00002414
    private void HandlePlayerSortOrderUpdate(ConnectedPlayerManager.PlayerSortOrderPacket packet, IConnectedPlayer iPlayer)
    {
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer = (packet.userId == this._localPlayer.userId) ? this._localPlayer : this.GetPlayer(packet.userId);
        int sortIndex = packet.sortIndex;
        packet.Release();
        if (connectedPlayer == null)
        {
            return;
        }
        if (connectedPlayer.UpdateSortIndex(sortIndex))
        {
            Action<IConnectedPlayer> action = this.playerOrderChangedEvent;
            if (action != null)
            {
                action(connectedPlayer);
            }
            this.SendImmediatelyExcludingPlayer(connectedPlayer.GetPlayerSortOrderPacket(), (ConnectedPlayerManager.ConnectedPlayer)iPlayer, true);
        }
    }

    // Token: 0x0600008F RID: 143 RVA: 0x00004290 File Offset: 0x00002490
    private void HandleSyncTimePacket(ConnectedPlayerManager.SyncTimePacket packet, IConnectedPlayer player)
    {
        float syncTime = packet.syncTime;
        packet.Release();
        if (this.runTime - this._lastPollTime > 0.03f)
        {
            return;
        }
        bool flag = !this._syncTimeOffset.hasValue;
        this._syncTimeOffset.Update(syncTime + player.currentLatency - this.runTime);
        if (flag)
        {
            Action action = this.syncTimeInitializedEvent;
            if (action == null)
            {
                return;
            }
            action();
        }
    }

    // Token: 0x06000090 RID: 144 RVA: 0x000042FC File Offset: 0x000024FC
    private void HandlePing(ConnectedPlayerManager.PingPacket packet, IConnectedPlayer player)
    {
        float pingTime = packet.pingTime;
        packet.Release();
        if (this.runTime - this._lastPollTime > 0.03f)
        {
            return;
        }
        this.SendImmediatelyToPlayer(ConnectedPlayerManager.PongPacket.pool.Obtain().Init(pingTime), (ConnectedPlayerManager.ConnectedPlayer)player);
        if (this._connectionManager.isConnectionOwner)
        {
            this.SendImmediatelyToPlayer(ConnectedPlayerManager.SyncTimePacket.pool.Obtain().Init(this.syncTime), (ConnectedPlayerManager.ConnectedPlayer)player);
        }
    }

    // Token: 0x06000091 RID: 145 RVA: 0x00004378 File Offset: 0x00002578
    private void HandlePong(ConnectedPlayerManager.PongPacket packet, IConnectedPlayer player)
    {
        float pingTime = packet.pingTime;
        packet.Release();
        if (this.runTime - this._lastPollTime > 0.03f)
        {
            return;
        }
        ConnectedPlayerManager.ConnectedPlayer connectedPlayer = (ConnectedPlayerManager.ConnectedPlayer)player;
        bool flag = !player.hasValidLatency;
        connectedPlayer.UpdateLatency(this.runTime - pingTime);
        if (flag)
        {
            Action<IConnectedPlayer> action = this.playerLatencyInitializedEvent;
            if (action == null)
            {
                return;
            }
            action(player);
        }
    }

    // Token: 0x06000092 RID: 146 RVA: 0x000043D8 File Offset: 0x000025D8
    [Conditional("VERBOSE_LOGGING")]
    private void Log(string message)
    {
        BGNet.Logging.Debug.Log("[ConnectedPlayerManager]" + message);
    }

    // Token: 0x04000026 RID: 38
    public const int invalidSortIndex = -1;

    // Token: 0x04000027 RID: 39
    private const byte kEncryptedMessageBit = 128;

    // Token: 0x04000028 RID: 40
    private const byte kAllConnectionsId = 127;

    // Token: 0x04000029 RID: 41
    private const byte kLocalConnectionId = 0;

    // Token: 0x0400002A RID: 42
    private const float kTimeSensitiveAllowedReceiveWindow = 0.03f;

    // Token: 0x0400002B RID: 43
    private const int kMaxUnreliableMessageLength = 412;

    // Token: 0x0400002C RID: 44
    private const float kPingUpdateFrequency = 2f;

    // Token: 0x04000038 RID: 56
    private readonly long _startTicks;

    // Token: 0x04000039 RID: 57
    private readonly RollingAverage _syncTimeOffset = new RollingAverage(30);

    // Token: 0x0400003A RID: 58
    private readonly BGNet.Core.ITimeProvider _timeProvider;

    // Token: 0x0400003B RID: 59
    private readonly ITaskUtility _taskUtility;

    // Token: 0x0400003C RID: 60
    private readonly IConnectionManager _connectionManager;

    // Token: 0x0400003D RID: 61
    private readonly NetDataWriter _temporaryDataWriter = new NetDataWriter();

    // Token: 0x0400003E RID: 62
    private readonly NetDataWriter _temporaryEncryptedDataWriter = new NetDataWriter();

    // Token: 0x0400003F RID: 63
    private readonly NetDataWriter _reliableDataWriter = new NetDataWriter();

    // Token: 0x04000040 RID: 64
    private readonly NetDataWriter _unreliableDataWriter = new NetDataWriter();

    // Token: 0x04000041 RID: 65
    private readonly List<ConnectedPlayerManager.ConnectedPlayer> _players = new List<ConnectedPlayerManager.ConnectedPlayer>();

    // Token: 0x04000042 RID: 66
    private readonly HashSet<string> _localPlayerState = new HashSet<string>();

    // Token: 0x04000043 RID: 67
    private MultiplayerAvatarData _localPlayerAvatar;

    // Token: 0x04000044 RID: 68
    private IDiffieHellmanKeyPair _encryptionKeys;

    // Token: 0x04000045 RID: 69
    private byte[] _encryptionRandom;

    // Token: 0x04000046 RID: 70
    private ConnectedPlayerManager.ConnectedPlayer _localPlayer;

    // Token: 0x04000047 RID: 71
    private byte _lastConnectionId;

    // Token: 0x04000048 RID: 72
    private float _lastPollTime;

    // Token: 0x04000049 RID: 73
    private int _lastPollFrame;

    // Token: 0x0400004A RID: 74
    private float _lastPingTime;

    // Token: 0x0400004B RID: 75
    private readonly NetworkPacketSerializer<ConnectedPlayerManager.InternalMessageType, IConnectedPlayer> _messageSerializer = new NetworkPacketSerializer<ConnectedPlayerManager.InternalMessageType, IConnectedPlayer>();

    // Token: 0x020000CB RID: 203
    private class ConnectedPlayer : IConnectedPlayer
    {
        // Token: 0x1700012B RID: 299
        // (get) Token: 0x0600070C RID: 1804 RVA: 0x00013343 File Offset: 0x00011543
        public IConnection connection
        {
            get
            {
                return this._connection;
            }
        }

        // Token: 0x1700012C RID: 300
        // (get) Token: 0x0600070D RID: 1805 RVA: 0x0001334B File Offset: 0x0001154B
        public byte connectionId
        {
            get
            {
                return this._connectionId;
            }
        }

        // Token: 0x1700012D RID: 301
        // (get) Token: 0x0600070E RID: 1806 RVA: 0x00013353 File Offset: 0x00011553
        public byte remoteConnectionId
        {
            get
            {
                return this._remoteConnectionId;
            }
        }

        // Token: 0x1700012E RID: 302
        // (get) Token: 0x0600070F RID: 1807 RVA: 0x0001335B File Offset: 0x0001155B
        public bool isConnected
        {
            get
            {
                return this._isConnected;
            }
        }

        // Token: 0x1700012F RID: 303
        // (get) Token: 0x06000710 RID: 1808 RVA: 0x00013363 File Offset: 0x00011563
        public bool isConnectionOwner
        {
            get
            {
                return this._isConnectionOwner;
            }
        }

        // Token: 0x17000130 RID: 304
        // (get) Token: 0x06000711 RID: 1809 RVA: 0x0001336B File Offset: 0x0001156B
        public bool isKicked
        {
            get
            {
                return this._isKicked;
            }
        }

        // Token: 0x17000131 RID: 305
        // (get) Token: 0x06000712 RID: 1810 RVA: 0x00013373 File Offset: 0x00011573
        public DisconnectedReason disconnectedReason
        {
            get
            {
                return this._disconnectedReason;
            }
        }

        // Token: 0x17000132 RID: 306
        // (get) Token: 0x06000713 RID: 1811 RVA: 0x0001337B File Offset: 0x0001157B
        public int sortIndex
        {
            get
            {
                return this._sortIndex;
            }
        }

        // Token: 0x17000133 RID: 307
        // (get) Token: 0x06000714 RID: 1812 RVA: 0x00013383 File Offset: 0x00011583
        public string userId
        {
            get
            {
                return this._userId;
            }
        }

        // Token: 0x17000134 RID: 308
        // (get) Token: 0x06000715 RID: 1813 RVA: 0x0001338B File Offset: 0x0001158B
        public string userName
        {
            get
            {
                return this._userName;
            }
        }

        // Token: 0x17000135 RID: 309
        // (get) Token: 0x06000716 RID: 1814 RVA: 0x00013393 File Offset: 0x00011593
        public bool isMe
        {
            get
            {
                return this._isMe;
            }
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x06000717 RID: 1815 RVA: 0x0001339B File Offset: 0x0001159B
        public bool hasValidLatency
        {
            get
            {
                return this._isMe || this._latency.hasValue;
            }
        }

        // Token: 0x17000137 RID: 311
        // (get) Token: 0x06000718 RID: 1816 RVA: 0x000133B2 File Offset: 0x000115B2
        public float currentLatency
        {
            get
            {
                return Mathf.Max(this._latency.currentAverage, 0f);
            }
        }

        // Token: 0x17000138 RID: 312
        // (get) Token: 0x06000719 RID: 1817 RVA: 0x000133C9 File Offset: 0x000115C9
        public float offsetSyncTime
        {
            get
            {
                return this._manager.syncTime - (this.isMe ? 0f : (this.currentLatency + 0.033333335f));
            }
        }

        // Token: 0x17000139 RID: 313
        // (get) Token: 0x0600071A RID: 1818 RVA: 0x000133F2 File Offset: 0x000115F2
        public MultiplayerAvatarData multiplayerAvatarData
        {
            get
            {
                return this._playerAvatar;
            }
        }

        // Token: 0x1700013A RID: 314
        // (get) Token: 0x0600071B RID: 1819 RVA: 0x000133FA File Offset: 0x000115FA
        public byte[] publicEncryptionKey
        {
            get
            {
                return this._publicEncryptionKey;
            }
        }

        // Token: 0x1700013B RID: 315
        // (get) Token: 0x0600071C RID: 1820 RVA: 0x00013402 File Offset: 0x00011602
        public byte[] random
        {
            get
            {
                return this._random;
            }
        }

        // Token: 0x1700013C RID: 316
        // (get) Token: 0x0600071D RID: 1821 RVA: 0x0001340A File Offset: 0x0001160A
        public bool isDirectConnection
        {
            get
            {
                return this._parent == null;
            }
        }

        // Token: 0x1700013D RID: 317
        // (get) Token: 0x0600071E RID: 1822 RVA: 0x00013415 File Offset: 0x00011615
        public EncryptionUtility.IEncryptionState encryptionState
        {
            get
            {
                return this._encryptionState;
            }
        }

        // Token: 0x0600071F RID: 1823 RVA: 0x00013420 File Offset: 0x00011620
        private ConnectedPlayer(ConnectedPlayerManager manager, byte connectionId, byte remoteConnectionId, IConnection connection, ConnectedPlayerManager.ConnectedPlayer parent, string userId, string userName, bool isConnectionOwner, bool isMe, byte[] publicEncryptionKey, byte[] random)
        {
            this._manager = manager;
            this._connectionId = connectionId;
            this._remoteConnectionId = remoteConnectionId;
            this._parent = parent;
            this._connection = connection;
            this._userId = userId;
            this._userName = userName;
            this._isConnectionOwner = isConnectionOwner;
            this._isMe = isMe;
            this._publicEncryptionKey = publicEncryptionKey;
            this._random = random;
            this._sortIndex = -1;
        }

        // Token: 0x06000720 RID: 1824 RVA: 0x000134A4 File Offset: 0x000116A4
        public static ConnectedPlayerManager.ConnectedPlayer CreateLocalPlayer(ConnectedPlayerManager manager, string userId, string userName, bool isConnectionOwner, byte[] publicEncryptionKey, byte[] random)
        {
            return new ConnectedPlayerManager.ConnectedPlayer(manager, 0, 0, null, null, userId, userName, isConnectionOwner, true, publicEncryptionKey, random);
        }

        // Token: 0x06000721 RID: 1825 RVA: 0x000134C4 File Offset: 0x000116C4
        public static ConnectedPlayerManager.ConnectedPlayer CreateDirectlyConnectedPlayer(ConnectedPlayerManager manager, byte connectionId, IConnection connection)
        {
            return new ConnectedPlayerManager.ConnectedPlayer(manager, connectionId, 0, connection, null, connection.userId, connection.userName, connection.isConnectionOwner, false, null, null);
        }

        // Token: 0x06000722 RID: 1826 RVA: 0x000134F0 File Offset: 0x000116F0
        public static ConnectedPlayerManager.ConnectedPlayer CreateRemoteConnectedPlayer(ConnectedPlayerManager manager, byte connectionId, ConnectedPlayerManager.PlayerConnectedPacket packet, ConnectedPlayerManager.ConnectedPlayer parent)
        {
            return new ConnectedPlayerManager.ConnectedPlayer(manager, connectionId, packet.remoteConnectionId, parent.connection, parent, packet.userId, packet.userName, packet.isConnectionOwner, false, null, null);
        }

        // Token: 0x06000723 RID: 1827 RVA: 0x00013526 File Offset: 0x00011726
        public ConnectedPlayerManager.PlayerConnectedPacket GetPlayerConnectedPacket()
        {
            return ConnectedPlayerManager.PlayerConnectedPacket.pool.Obtain().Init(this.connectionId, this.userId, this.userName, this.isConnectionOwner);
        }

        // Token: 0x06000724 RID: 1828 RVA: 0x0001354F File Offset: 0x0001174F
        public ConnectedPlayerManager.PlayerIdentityPacket GetPlayerIdentityPacket()
        {
            return ConnectedPlayerManager.PlayerIdentityPacket.pool.Obtain().Init(this._playerState, this._playerAvatar, this._random, this._publicEncryptionKey);
        }

        // Token: 0x06000725 RID: 1829 RVA: 0x00013578 File Offset: 0x00011778
        public ConnectedPlayerManager.PlayerStatePacket GetPlayerStatePacket()
        {
            return ConnectedPlayerManager.PlayerStatePacket.pool.Obtain().Init(this._playerState);
        }

        // Token: 0x06000726 RID: 1830 RVA: 0x0001358F File Offset: 0x0001178F
        public ConnectedPlayerManager.PlayerAvatarPacket GetPlayerAvatarPacket()
        {
            return ConnectedPlayerManager.PlayerAvatarPacket.pool.Obtain().Init(this._playerAvatar);
        }

        // Token: 0x06000727 RID: 1831 RVA: 0x000135A6 File Offset: 0x000117A6
        public ConnectedPlayerManager.PlayerSortOrderPacket GetPlayerSortOrderPacket()
        {
            return ConnectedPlayerManager.PlayerSortOrderPacket.pool.Obtain().Init(this._userId, this._sortIndex);
        }

        // Token: 0x06000728 RID: 1832 RVA: 0x000135C4 File Offset: 0x000117C4
        public void Disconnect(DisconnectedReason disconnectedReason)
        {
            if (this._isConnected)
            {
                this._isConnected = false;
                this._disconnectedReason = (this._isKicked ? DisconnectedReason.Kicked : disconnectedReason);
                if (this.isDirectConnection)
                {
                    IConnection connection = this.connection;
                    if (connection != null)
                    {
                        connection.Disconnect();
                    }
                }
            }
            EncryptionUtility.IEncryptionState encryptionState = this._encryptionState;
            if (encryptionState != null)
            {
                encryptionState.Dispose();
            }
            this._encryptionState = null;
        }

        // Token: 0x06000729 RID: 1833 RVA: 0x00013623 File Offset: 0x00011823
        public void UpdateLatency(float latency)
        {
            this._latency.Update(latency);
        }

        // Token: 0x0600072A RID: 1834 RVA: 0x00013631 File Offset: 0x00011831
        public bool UpdateSortIndex(int index)
        {
            if (this._sortIndex == index)
            {
                return false;
            }
            this._sortIndex = index;
            return true;
        }

        // Token: 0x0600072B RID: 1835 RVA: 0x00013646 File Offset: 0x00011846
        public void SetKicked()
        {
            this._isKicked = true;
        }

        // Token: 0x0600072C RID: 1836 RVA: 0x0001364F File Offset: 0x0001184F
        public void UpdateIdentity(ConnectedPlayerManager.PlayerIdentityPacket packet)
        {
            this._playerState = packet.playerState;
            this._playerAvatar = packet.playerAvatar;
            this._random = packet.random;
            this._publicEncryptionKey = packet.publicEncryptionKey;
        }

        // Token: 0x0600072D RID: 1837 RVA: 0x0001368B File Offset: 0x0001188B
        public void UpdateState(ConnectedPlayerManager.PlayerStatePacket packet)
        {
            this._playerState = packet.playerState;
        }

        // Token: 0x0600072E RID: 1838 RVA: 0x00013699 File Offset: 0x00011899
        public void UpdateAvatar(ConnectedPlayerManager.PlayerAvatarPacket packet)
        {
            this._playerAvatar = packet.playerAvatar;
        }

        // Token: 0x0600072F RID: 1839 RVA: 0x000136A7 File Offset: 0x000118A7
        public void SetEncryptionState(EncryptionUtility.IEncryptionState encryptionState)
        {
            this._encryptionState = encryptionState;
        }

        // Token: 0x06000730 RID: 1840 RVA: 0x000136B0 File Offset: 0x000118B0
        public bool HasState(string state)
        {
            return this._playerState.Contains(state);
        }

        // Token: 0x06000731 RID: 1841 RVA: 0x000136BE File Offset: 0x000118BE
        public void SetPlayerState(PlayerStateHash playerState)
        {
            this._playerState = playerState;
        }

        // Token: 0x06000732 RID: 1842 RVA: 0x000136C7 File Offset: 0x000118C7
        public void SetPlayerAvatar(MultiplayerAvatarData avatarData)
        {
            this._playerAvatar = avatarData;
        }

        // Token: 0x06000733 RID: 1843 RVA: 0x000136D0 File Offset: 0x000118D0
        public override string ToString()
        {
            return string.Format("[ConnectedPlayer {0}({1}) isMe:{2} isConnectionOwner:{3}]", new object[]
            {
                this.userName,
                this.userId,
                this.isMe,
                this.isConnectionOwner
            });
        }

        // Token: 0x040002FB RID: 763
        private const float kFixedSyncTimeOffset = 0.033333335f;

        // Token: 0x040002FC RID: 764
        private readonly string _userId;

        // Token: 0x040002FD RID: 765
        private readonly string _userName;

        // Token: 0x040002FE RID: 766
        private readonly bool _isMe;

        // Token: 0x040002FF RID: 767
        private readonly bool _isConnectionOwner;

        // Token: 0x04000300 RID: 768
        private readonly ConnectedPlayerManager _manager;

        // Token: 0x04000301 RID: 769
        private readonly IConnection _connection;

        // Token: 0x04000302 RID: 770
        private readonly ConnectedPlayerManager.ConnectedPlayer _parent;

        // Token: 0x04000303 RID: 771
        private readonly byte _connectionId;

        // Token: 0x04000304 RID: 772
        private readonly byte _remoteConnectionId;

        // Token: 0x04000305 RID: 773
        private int _sortIndex;

        // Token: 0x04000306 RID: 774
        private bool _isConnected = true;

        // Token: 0x04000307 RID: 775
        private DisconnectedReason _disconnectedReason;

        // Token: 0x04000308 RID: 776
        private bool _isKicked;

        // Token: 0x04000309 RID: 777
        private PlayerStateHash _playerState;

        // Token: 0x0400030A RID: 778
        private MultiplayerAvatarData _playerAvatar;

        // Token: 0x0400030B RID: 779
        private byte[] _publicEncryptionKey;

        // Token: 0x0400030C RID: 780
        private byte[] _random;

        // Token: 0x0400030D RID: 781
        private EncryptionUtility.IEncryptionState _encryptionState;

        // Token: 0x0400030E RID: 782
        private readonly RollingAverage _latency = new RollingAverage(30);
    }

    // Token: 0x020000CC RID: 204
    private enum InternalMessageType : byte
    {
        // Token: 0x04000310 RID: 784
        SyncTime,
        // Token: 0x04000311 RID: 785
        PlayerConnected,
        // Token: 0x04000312 RID: 786
        PlayerIdentity,
        // Token: 0x04000313 RID: 787
        PlayerLatencyUpdate,
        // Token: 0x04000314 RID: 788
        PlayerDisconnected,
        // Token: 0x04000315 RID: 789
        PlayerSortOrderUpdate,
        // Token: 0x04000316 RID: 790
        Party,
        // Token: 0x04000317 RID: 791
        MultiplayerSession,
        // Token: 0x04000318 RID: 792
        KickPlayer,
        // Token: 0x04000319 RID: 793
        PlayerStateUpdate,
        // Token: 0x0400031A RID: 794
        PlayerAvatarUpdate,
        // Token: 0x0400031B RID: 795
        Ping,
        // Token: 0x0400031C RID: 796
        Pong
    }

    // Token: 0x020000CD RID: 205
    public enum MessageType : byte
    {
        // Token: 0x0400031E RID: 798
        Party = 6,
        // Token: 0x0400031F RID: 799
        MultiplayerSession
    }

    // Token: 0x020000CE RID: 206
    private class PlayerConnectedPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x1700013E RID: 318
        // (get) Token: 0x06000734 RID: 1844 RVA: 0x00013710 File Offset: 0x00011910
        public static PacketPool<ConnectedPlayerManager.PlayerConnectedPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PlayerConnectedPacket>.pool;
            }
        }

        // Token: 0x06000735 RID: 1845 RVA: 0x00013717 File Offset: 0x00011917
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.remoteConnectionId);
            writer.Put(this.userId);
            writer.Put(this.userName);
            writer.Put(this.isConnectionOwner);
        }

        // Token: 0x06000736 RID: 1846 RVA: 0x00013749 File Offset: 0x00011949
        public void Deserialize(NetDataReader reader)
        {
            this.remoteConnectionId = reader.GetByte();
            this.userId = reader.GetString();
            this.userName = reader.GetString();
            this.isConnectionOwner = reader.GetBool();
        }

        // Token: 0x06000737 RID: 1847 RVA: 0x0001377B File Offset: 0x0001197B
        public void Release()
        {
            ConnectedPlayerManager.PlayerConnectedPacket.pool.Release(this);
        }

        // Token: 0x06000738 RID: 1848 RVA: 0x00013788 File Offset: 0x00011988
        public ConnectedPlayerManager.PlayerConnectedPacket Init(byte connectionId, string userId, string userName, bool isConnectionOwner)
        {
            this.remoteConnectionId = connectionId;
            this.userId = userId;
            this.userName = userName;
            this.isConnectionOwner = isConnectionOwner;
            return this;
        }

        // Token: 0x04000320 RID: 800
        public byte remoteConnectionId;

        // Token: 0x04000321 RID: 801
        public string userId;

        // Token: 0x04000322 RID: 802
        public string userName;

        // Token: 0x04000323 RID: 803
        public bool isConnectionOwner;
    }

    // Token: 0x020000CF RID: 207
    private class PlayerIdentityPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x1700013F RID: 319
        // (get) Token: 0x0600073A RID: 1850 RVA: 0x000137A8 File Offset: 0x000119A8
        public static PacketPool<ConnectedPlayerManager.PlayerIdentityPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PlayerIdentityPacket>.pool;
            }
        }

        // Token: 0x0600073B RID: 1851 RVA: 0x000137AF File Offset: 0x000119AF
        public void Serialize(NetDataWriter writer)
        {
            this.playerState.Serialize(writer);
            this.playerAvatar.Serialize(writer);
            this.random.Serialize(writer);
            this.publicEncryptionKey.Serialize(writer);
        }

        // Token: 0x0600073C RID: 1852 RVA: 0x000137E1 File Offset: 0x000119E1
        public void Deserialize(NetDataReader reader)
        {
            this.playerState = PlayerStateHash.Deserialize(reader);
            this.playerAvatar = MultiplayerAvatarData.Deserialize(reader);
            this.random.Deserialize(reader);
            this.publicEncryptionKey.Deserialize(reader);
        }

        // Token: 0x0600073D RID: 1853 RVA: 0x00013813 File Offset: 0x00011A13
        public void Release()
        {
            this.random.Clear();
            this.publicEncryptionKey.Clear();
            ConnectedPlayerManager.PlayerIdentityPacket.pool.Release(this);
        }

        // Token: 0x0600073E RID: 1854 RVA: 0x00013836 File Offset: 0x00011A36
        public ConnectedPlayerManager.PlayerIdentityPacket Init(PlayerStateHash states, MultiplayerAvatarData avatar, byte[] random, byte[] publicEncryptionKey)
        {
            this.playerState = states;
            this.playerAvatar = avatar;
            this.random.data = random;
            this.publicEncryptionKey.data = publicEncryptionKey;
            return this;
        }

        // Token: 0x04000324 RID: 804
        public PlayerStateHash playerState;

        // Token: 0x04000325 RID: 805
        public MultiplayerAvatarData playerAvatar;

        // Token: 0x04000326 RID: 806
        public readonly ByteArrayNetSerializable random = new ByteArrayNetSerializable("random", 32, true);

        // Token: 0x04000327 RID: 807
        public readonly ByteArrayNetSerializable publicEncryptionKey = new ByteArrayNetSerializable("publicEncryptionKey", 0, 256, true);
    }

    // Token: 0x020000D0 RID: 208
    private class PlayerAvatarPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000140 RID: 320
        // (get) Token: 0x06000740 RID: 1856 RVA: 0x00013892 File Offset: 0x00011A92
        public static PacketPool<ConnectedPlayerManager.PlayerAvatarPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PlayerAvatarPacket>.pool;
            }
        }

        // Token: 0x06000741 RID: 1857 RVA: 0x00013899 File Offset: 0x00011A99
        public void Serialize(NetDataWriter writer)
        {
            this.playerAvatar.Serialize(writer);
        }

        // Token: 0x06000742 RID: 1858 RVA: 0x000138A7 File Offset: 0x00011AA7
        public void Deserialize(NetDataReader reader)
        {
            this.playerAvatar = MultiplayerAvatarData.Deserialize(reader);
        }

        // Token: 0x06000743 RID: 1859 RVA: 0x000138B5 File Offset: 0x00011AB5
        public void Release()
        {
            ConnectedPlayerManager.PlayerAvatarPacket.pool.Release(this);
        }

        // Token: 0x06000744 RID: 1860 RVA: 0x000138C2 File Offset: 0x00011AC2
        public ConnectedPlayerManager.PlayerAvatarPacket Init(MultiplayerAvatarData avatar)
        {
            this.playerAvatar = avatar;
            return this;
        }

        // Token: 0x04000328 RID: 808
        public MultiplayerAvatarData playerAvatar;
    }

    // Token: 0x020000D1 RID: 209
    private class PlayerStatePacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000141 RID: 321
        // (get) Token: 0x06000746 RID: 1862 RVA: 0x000138CC File Offset: 0x00011ACC
        public static PacketPool<ConnectedPlayerManager.PlayerStatePacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PlayerStatePacket>.pool;
            }
        }

        // Token: 0x06000747 RID: 1863 RVA: 0x000138D3 File Offset: 0x00011AD3
        public void Serialize(NetDataWriter writer)
        {
            this.playerState.Serialize(writer);
        }

        // Token: 0x06000748 RID: 1864 RVA: 0x000138E1 File Offset: 0x00011AE1
        public void Deserialize(NetDataReader reader)
        {
            this.playerState = PlayerStateHash.Deserialize(reader);
        }

        // Token: 0x06000749 RID: 1865 RVA: 0x000138EF File Offset: 0x00011AEF
        public void Release()
        {
            ConnectedPlayerManager.PlayerStatePacket.pool.Release(this);
        }

        // Token: 0x0600074A RID: 1866 RVA: 0x000138FC File Offset: 0x00011AFC
        public ConnectedPlayerManager.PlayerStatePacket Init(PlayerStateHash states)
        {
            this.playerState = states;
            return this;
        }

        // Token: 0x04000329 RID: 809
        public PlayerStateHash playerState;
    }

    // Token: 0x020000D2 RID: 210
    private class PlayerSortOrderPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000142 RID: 322
        // (get) Token: 0x0600074C RID: 1868 RVA: 0x00013906 File Offset: 0x00011B06
        public static PacketPool<ConnectedPlayerManager.PlayerSortOrderPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PlayerSortOrderPacket>.pool;
            }
        }

        // Token: 0x0600074D RID: 1869 RVA: 0x0001390D File Offset: 0x00011B0D
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.userId);
            writer.PutVarInt(this.sortIndex);
        }

        // Token: 0x0600074E RID: 1870 RVA: 0x00013927 File Offset: 0x00011B27
        public void Deserialize(NetDataReader reader)
        {
            this.userId = reader.GetString();
            this.sortIndex = reader.GetVarInt();
        }

        // Token: 0x0600074F RID: 1871 RVA: 0x00013941 File Offset: 0x00011B41
        public void Release()
        {
            ConnectedPlayerManager.PlayerSortOrderPacket.pool.Release(this);
        }

        // Token: 0x06000750 RID: 1872 RVA: 0x0001394E File Offset: 0x00011B4E
        public ConnectedPlayerManager.PlayerSortOrderPacket Init(string userId, int sortIndex)
        {
            this.userId = userId;
            this.sortIndex = sortIndex;
            return this;
        }

        // Token: 0x0400032A RID: 810
        public string userId;

        // Token: 0x0400032B RID: 811
        public int sortIndex;
    }

    // Token: 0x020000D3 RID: 211
    private class PlayerDisconnectedPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000143 RID: 323
        // (get) Token: 0x06000752 RID: 1874 RVA: 0x0001395F File Offset: 0x00011B5F
        public static PacketPool<ConnectedPlayerManager.PlayerDisconnectedPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PlayerDisconnectedPacket>.pool;
            }
        }

        // Token: 0x06000753 RID: 1875 RVA: 0x00013966 File Offset: 0x00011B66
        public ConnectedPlayerManager.PlayerDisconnectedPacket Init(DisconnectedReason disconnectedReason)
        {
            this.disconnectedReason = disconnectedReason;
            return this;
        }

        // Token: 0x06000754 RID: 1876 RVA: 0x00013970 File Offset: 0x00011B70
        public void Serialize(NetDataWriter writer)
        {
            writer.PutVarInt((int)this.disconnectedReason);
        }

        // Token: 0x06000755 RID: 1877 RVA: 0x0001397E File Offset: 0x00011B7E
        public void Deserialize(NetDataReader reader)
        {
            this.disconnectedReason = (DisconnectedReason)reader.GetVarInt();
        }

        // Token: 0x06000756 RID: 1878 RVA: 0x0001398C File Offset: 0x00011B8C
        public void Release()
        {
            ConnectedPlayerManager.PlayerDisconnectedPacket.pool.Release(this);
        }

        // Token: 0x0400032C RID: 812
        public DisconnectedReason disconnectedReason = DisconnectedReason.Unknown;
    }

    // Token: 0x020000D4 RID: 212
    private class KickPlayerPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000144 RID: 324
        // (get) Token: 0x06000758 RID: 1880 RVA: 0x000139A8 File Offset: 0x00011BA8
        public static PacketPool<ConnectedPlayerManager.KickPlayerPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.KickPlayerPacket>.pool;
            }
        }

        // Token: 0x06000759 RID: 1881 RVA: 0x000139AF File Offset: 0x00011BAF
        public ConnectedPlayerManager.KickPlayerPacket Init(DisconnectedReason disconnectedReason)
        {
            this.disconnectedReason = disconnectedReason;
            return this;
        }

        // Token: 0x0600075A RID: 1882 RVA: 0x000139B9 File Offset: 0x00011BB9
        public void Serialize(NetDataWriter writer)
        {
            writer.PutVarInt((int)this.disconnectedReason);
        }

        // Token: 0x0600075B RID: 1883 RVA: 0x000139C7 File Offset: 0x00011BC7
        public void Deserialize(NetDataReader reader)
        {
            this.disconnectedReason = (DisconnectedReason)reader.GetVarInt();
        }

        // Token: 0x0600075C RID: 1884 RVA: 0x000139D5 File Offset: 0x00011BD5
        public void Release()
        {
            ConnectedPlayerManager.KickPlayerPacket.pool.Release(this);
        }

        // Token: 0x0400032D RID: 813
        public DisconnectedReason disconnectedReason;
    }

    // Token: 0x020000D5 RID: 213
    private class SyncTimePacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000145 RID: 325
        // (get) Token: 0x0600075E RID: 1886 RVA: 0x000139E2 File Offset: 0x00011BE2
        public static PacketPool<ConnectedPlayerManager.SyncTimePacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.SyncTimePacket>.pool;
            }
        }

        // Token: 0x0600075F RID: 1887 RVA: 0x000139E9 File Offset: 0x00011BE9
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.syncTime);
        }

        // Token: 0x06000760 RID: 1888 RVA: 0x000139F7 File Offset: 0x00011BF7
        public void Deserialize(NetDataReader reader)
        {
            this.syncTime = reader.GetFloat();
        }

        // Token: 0x06000761 RID: 1889 RVA: 0x00013A05 File Offset: 0x00011C05
        public ConnectedPlayerManager.SyncTimePacket Init(float syncTime)
        {
            this.syncTime = syncTime;
            return this;
        }

        // Token: 0x06000762 RID: 1890 RVA: 0x00013A0F File Offset: 0x00011C0F
        public void Release()
        {
            ConnectedPlayerManager.SyncTimePacket.pool.Release(this);
        }

        // Token: 0x0400032E RID: 814
        public float syncTime;
    }

    // Token: 0x020000D6 RID: 214
    private class PingPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000146 RID: 326
        // (get) Token: 0x06000764 RID: 1892 RVA: 0x00013A1C File Offset: 0x00011C1C
        public static PacketPool<ConnectedPlayerManager.PingPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PingPacket>.pool;
            }
        }

        // Token: 0x06000765 RID: 1893 RVA: 0x00013A23 File Offset: 0x00011C23
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.pingTime);
        }

        // Token: 0x06000766 RID: 1894 RVA: 0x00013A31 File Offset: 0x00011C31
        public void Deserialize(NetDataReader reader)
        {
            this.pingTime = reader.GetFloat();
        }

        // Token: 0x06000767 RID: 1895 RVA: 0x00013A3F File Offset: 0x00011C3F
        public ConnectedPlayerManager.PingPacket Init(float pingTime)
        {
            this.pingTime = pingTime;
            return this;
        }

        // Token: 0x06000768 RID: 1896 RVA: 0x00013A49 File Offset: 0x00011C49
        public void Release()
        {
            ConnectedPlayerManager.PingPacket.pool.Release(this);
        }

        // Token: 0x0400032F RID: 815
        public float pingTime;
    }

    // Token: 0x020000D7 RID: 215
    private class PongPacket : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000147 RID: 327
        // (get) Token: 0x0600076A RID: 1898 RVA: 0x00013A56 File Offset: 0x00011C56
        public static PacketPool<ConnectedPlayerManager.PongPacket> pool
        {
            get
            {
                return ThreadStaticPacketPool<ConnectedPlayerManager.PongPacket>.pool;
            }
        }

        // Token: 0x0600076B RID: 1899 RVA: 0x00013A5D File Offset: 0x00011C5D
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.pingTime);
        }

        // Token: 0x0600076C RID: 1900 RVA: 0x00013A6B File Offset: 0x00011C6B
        public void Deserialize(NetDataReader reader)
        {
            this.pingTime = reader.GetFloat();
        }

        // Token: 0x0600076D RID: 1901 RVA: 0x00013A79 File Offset: 0x00011C79
        public ConnectedPlayerManager.PongPacket Init(float pingTime)
        {
            this.pingTime = pingTime;
            return this;
        }

        // Token: 0x0600076E RID: 1902 RVA: 0x00013A83 File Offset: 0x00011C83
        public void Release()
        {
            ConnectedPlayerManager.PongPacket.pool.Release(this);
        }

        // Token: 0x04000330 RID: 816
        public float pingTime;
    }
}
