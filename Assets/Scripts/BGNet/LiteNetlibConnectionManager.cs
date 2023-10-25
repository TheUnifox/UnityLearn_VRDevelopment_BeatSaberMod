using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;
using BGNet.Logging;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class LiteNetLibConnectionManager : INetworkConnectionManager, IConnectionManager, IPollable, IDisposable, IUnconnectedConnectionManager, IUnconnectedMessageSender, INetEventListener
{
    // Token: 0x14000070 RID: 112
    // (add) Token: 0x06000301 RID: 769 RVA: 0x00006F40 File Offset: 0x00005140
    // (remove) Token: 0x06000302 RID: 770 RVA: 0x00006F78 File Offset: 0x00005178
    public event Action onInitializedEvent;

    // Token: 0x14000071 RID: 113
    // (add) Token: 0x06000303 RID: 771 RVA: 0x00006FB0 File Offset: 0x000051B0
    // (remove) Token: 0x06000304 RID: 772 RVA: 0x00006FE8 File Offset: 0x000051E8
    public event Action onConnectedEvent;

    // Token: 0x14000072 RID: 114
    // (add) Token: 0x06000305 RID: 773 RVA: 0x00007020 File Offset: 0x00005220
    // (remove) Token: 0x06000306 RID: 774 RVA: 0x00007058 File Offset: 0x00005258
    public event Action<DisconnectedReason> onDisconnectedEvent;

    // Token: 0x14000073 RID: 115
    // (add) Token: 0x06000307 RID: 775 RVA: 0x00007090 File Offset: 0x00005290
    // (remove) Token: 0x06000308 RID: 776 RVA: 0x000070C8 File Offset: 0x000052C8
    public event Action<ConnectionFailedReason> onConnectionFailedEvent;

    // Token: 0x14000074 RID: 116
    // (add) Token: 0x06000309 RID: 777 RVA: 0x00007100 File Offset: 0x00005300
    // (remove) Token: 0x0600030A RID: 778 RVA: 0x00007138 File Offset: 0x00005338
    public event Action<IConnection> onConnectionConnectedEvent;

    // Token: 0x14000075 RID: 117
    // (add) Token: 0x0600030B RID: 779 RVA: 0x00007170 File Offset: 0x00005370
    // (remove) Token: 0x0600030C RID: 780 RVA: 0x000071A8 File Offset: 0x000053A8
    public event Action<IConnection, DisconnectedReason> onConnectionDisconnectedEvent;

    // Token: 0x14000076 RID: 118
    // (add) Token: 0x0600030D RID: 781 RVA: 0x000071E0 File Offset: 0x000053E0
    // (remove) Token: 0x0600030E RID: 782 RVA: 0x00007218 File Offset: 0x00005418
    public event Action<IConnection, NetDataReader, BGNet.Core.DeliveryMethod> onReceivedDataEvent;

    // Token: 0x14000077 RID: 119
    // (add) Token: 0x0600030F RID: 783 RVA: 0x00007250 File Offset: 0x00005450
    // (remove) Token: 0x06000310 RID: 784 RVA: 0x00007288 File Offset: 0x00005488
    public event Action<IPEndPoint, NetDataReader> onReceiveUnconnectedDataEvent;

    // Token: 0x14000078 RID: 120
    // (add) Token: 0x06000311 RID: 785 RVA: 0x000072C0 File Offset: 0x000054C0
    // (remove) Token: 0x06000312 RID: 786 RVA: 0x000072F8 File Offset: 0x000054F8
    public event NetworkStatisticsState.NetworkStatisticsUpdateDelegate onStatisticsUpdatedEvent;

    // Token: 0x17000098 RID: 152
    // (get) Token: 0x06000313 RID: 787 RVA: 0x0000732D File Offset: 0x0000552D
    public string userId
    {
        get
        {
            return this._userId;
        }
    }

    // Token: 0x17000099 RID: 153
    // (get) Token: 0x06000314 RID: 788 RVA: 0x00007335 File Offset: 0x00005535
    public string userName
    {
        get
        {
            return this._userName;
        }
    }

    // Token: 0x1700009A RID: 154
    // (get) Token: 0x06000315 RID: 789 RVA: 0x0000733D File Offset: 0x0000553D
    public bool isConnected
    {
        get
        {
            return this._connectionState == LiteNetLibConnectionManager.ConnectionState.Connected;
        }
    }

    // Token: 0x1700009B RID: 155
    // (get) Token: 0x06000316 RID: 790 RVA: 0x00007348 File Offset: 0x00005548
    public bool isConnecting
    {
        get
        {
            return this._connectionState == LiteNetLibConnectionManager.ConnectionState.Connecting;
        }
    }

    // Token: 0x1700009C RID: 156
    // (get) Token: 0x06000317 RID: 791 RVA: 0x00007353 File Offset: 0x00005553
    public bool isDisconnecting
    {
        get
        {
            return this._connectionState == LiteNetLibConnectionManager.ConnectionState.Disconnecting;
        }
    }

    // Token: 0x1700009D RID: 157
    // (get) Token: 0x06000318 RID: 792 RVA: 0x0000735E File Offset: 0x0000555E
    public bool isConnectionOwner
    {
        get
        {
            return this._mode == LiteNetLibConnectionManager.NetworkMode.Server;
        }
    }

    // Token: 0x1700009E RID: 158
    // (get) Token: 0x06000319 RID: 793 RVA: 0x00007369 File Offset: 0x00005569
    public bool hasConnectionOwner
    {
        get
        {
            return this._connections.Find((LiteNetLibConnectionManager.NetPeerConnection c) => c.isConnectionOwner) != null;
        }
    }

    // Token: 0x1700009F RID: 159
    // (get) Token: 0x0600031A RID: 794 RVA: 0x0000735E File Offset: 0x0000555E
    public bool isServer
    {
        get
        {
            return this._mode == LiteNetLibConnectionManager.NetworkMode.Server;
        }
    }

    // Token: 0x170000A0 RID: 160
    // (get) Token: 0x0600031B RID: 795 RVA: 0x00007398 File Offset: 0x00005598
    public bool isClient
    {
        get
        {
            return this._mode == LiteNetLibConnectionManager.NetworkMode.Client;
        }
    }

    // Token: 0x170000A1 RID: 161
    // (get) Token: 0x0600031C RID: 796 RVA: 0x000073A3 File Offset: 0x000055A3
    public bool isDisposed
    {
        get
        {
            return this._mode == LiteNetLibConnectionManager.NetworkMode.None;
        }
    }

    // Token: 0x170000A2 RID: 162
    // (get) Token: 0x0600031D RID: 797 RVA: 0x000073AE File Offset: 0x000055AE
    public int connectionCount
    {
        get
        {
            return this._connections.Count;
        }
    }

    // Token: 0x170000A3 RID: 163
    // (get) Token: 0x0600031E RID: 798 RVA: 0x000073BB File Offset: 0x000055BB
    public IConnectionRequestHandler connectionRequestHandler
    {
        get
        {
            return this._connectionRequestHandler;
        }
    }

    // Token: 0x170000A4 RID: 164
    // (get) Token: 0x0600031F RID: 799 RVA: 0x000073C3 File Offset: 0x000055C3
    public int port
    {
        get
        {
            return this._netManager.LocalPort;
        }
    }

    // Token: 0x170000A5 RID: 165
    // (get) Token: 0x06000320 RID: 800 RVA: 0x000073D0 File Offset: 0x000055D0
    public byte[] unconnectedPacketHeader
    {
        get
        {
            return this._unconnectedPacketHeader;
        }
    }

    // Token: 0x170000A6 RID: 166
    // (get) Token: 0x06000321 RID: 801 RVA: 0x000073D8 File Offset: 0x000055D8
    public PacketEncryptionLayer encryptionLayer
    {
        get
        {
            return this._encryptionLayer;
        }
    }

    // Token: 0x06000322 RID: 802 RVA: 0x000073E0 File Offset: 0x000055E0
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void NoDomainReloadInit()
    {
        NetDebug.Logger = new NetLogger();
    }

    // Token: 0x06000323 RID: 803 RVA: 0x000073E0 File Offset: 0x000055E0
    static LiteNetLibConnectionManager()
    {
        NetDebug.Logger = new NetLogger();
    }

    // Token: 0x06000324 RID: 804 RVA: 0x000073EC File Offset: 0x000055EC
    public LiteNetLibConnectionManager() : this(DefaultTimeProvider.instance, DefaultTaskUtility.instance)
    {
    }

    // Token: 0x06000325 RID: 805 RVA: 0x00007400 File Offset: 0x00005600
    public LiteNetLibConnectionManager(BGNet.Core.ITimeProvider timeProvider, ITaskUtility taskUtility)
    {
        this._taskUtility = taskUtility;
        this._encryptionLayer = new PacketEncryptionLayer(timeProvider, taskUtility);
        this._netManager = new NetManager(this, this._encryptionLayer);
        this._netManager.ReconnectDelay = 200;
        this._netManager.MaxConnectAttempts = 10;
        this._netManager.ThreadPriority = System.Threading.ThreadPriority.AboveNormal;
    }

    // Token: 0x06000326 RID: 806 RVA: 0x0000749E File Offset: 0x0000569E
    public void SendToAll(NetDataWriter writer, BGNet.Core.DeliveryMethod deliveryMethod)
    {
        this._netManager.SendToAll(writer, LiteNetLibConnectionManager.ToLiteNetDeliveryMethod(deliveryMethod));
    }

    // Token: 0x06000327 RID: 807 RVA: 0x000074B2 File Offset: 0x000056B2
    public void SendToAll(NetDataWriter writer, BGNet.Core.DeliveryMethod deliveryMethod, IConnection excludingConnection)
    {
        this._netManager.SendToAll(writer, LiteNetLibConnectionManager.ToLiteNetDeliveryMethod(deliveryMethod), ((LiteNetLibConnectionManager.NetPeerConnection)excludingConnection).netPeer);
    }

    // Token: 0x06000328 RID: 808 RVA: 0x000074D1 File Offset: 0x000056D1
    public void SendUnconnectedMessage(IPEndPoint remoteEndPoint, NetDataWriter writer)
    {
        this._netManager.SendUnconnectedMessage(writer, remoteEndPoint);
    }

    // Token: 0x06000329 RID: 809 RVA: 0x000074E1 File Offset: 0x000056E1
    public void RegisterReceiver(IUnconnectedMessageReceiver receiver)
    {
        this._unconnectedMessageReceiver = receiver;
    }

    // Token: 0x0600032A RID: 810 RVA: 0x000074EA File Offset: 0x000056EA
    public void UnregisterReceiver(IUnconnectedMessageReceiver receiver)
    {
        if (receiver == this._unconnectedMessageReceiver)
        {
            this._unconnectedMessageReceiver = null;
        }
    }

    // Token: 0x0600032B RID: 811 RVA: 0x000074FC File Offset: 0x000056FC
    public void PollUpdate()
    {
        this.CheckSentryState();
        this._lastPollUpdateTime = DateTime.UtcNow.Ticks;
        this._netManager.PollEvents();
        IUnconnectedMessageReceiver unconnectedMessageReceiver = this._unconnectedMessageReceiver;
        if (unconnectedMessageReceiver != null)
        {
            unconnectedMessageReceiver.PollUpdate();
        }
        this.UpdateStatistics();
    }

    // Token: 0x0600032C RID: 812 RVA: 0x00007544 File Offset: 0x00005744
    public bool Init<T>(IConnectionInitParams<T> initParams) where T : IConnectionManager
    {
        if (this._mode != LiteNetLibConnectionManager.NetworkMode.None)
        {
            this.Disconnect(DisconnectedReason.UserInitiated);
        }
        LiteNetLibConnectionManager.LiteNetLibConnectionParamsBase liteNetLibConnectionParamsBase;
        if ((liteNetLibConnectionParamsBase = (initParams as LiteNetLibConnectionManager.LiteNetLibConnectionParamsBase)) != null)
        {
            this._connectionRequestHandler = liteNetLibConnectionParamsBase.connectionRequestHandler;
            this._encryptionLayer.filterUnencryptedTraffic = liteNetLibConnectionParamsBase.filterUnencryptedTraffic;
            this._encryptionLayer.enableStatistics = liteNetLibConnectionParamsBase.enableStatistics;
            this._netManager.UnconnectedMessagesEnabled = liteNetLibConnectionParamsBase.enableUnconnectedMessages;
            this._netManager.EnableStatistics = liteNetLibConnectionParamsBase.enableStatistics;
            this._netManager.DisconnectTimeout = liteNetLibConnectionParamsBase.disconnectTimeoutMs;
            if (!this.TryStartNetManager(liteNetLibConnectionParamsBase.port, liteNetLibConnectionParamsBase.enableBackgroundSentry))
            {
                return false;
            }
        }
        LiteNetLibConnectionManager.StartServerParams startServerParams;
        if ((startServerParams = (initParams as LiteNetLibConnectionManager.StartServerParams)) != null)
        {
            this._mode = LiteNetLibConnectionManager.NetworkMode.Server;
            this._connectionState = LiteNetLibConnectionManager.ConnectionState.Connected;
            this._userId = (startServerParams.userId ?? string.Empty);
            this._userName = (startServerParams.userName ?? string.Empty);
            Action action = this.onInitializedEvent;
            if (action != null)
            {
                action();
            }
            Action action2 = this.onConnectedEvent;
            if (action2 != null)
            {
                action2();
            }
            return true;
        }
        LiteNetLibConnectionManager.ConnectToServerParams connectToServerParams;
        if ((connectToServerParams = (initParams as LiteNetLibConnectionManager.ConnectToServerParams)) != null)
        {
            this._mode = LiteNetLibConnectionManager.NetworkMode.Client;
            this.ConnectToEndPoint(connectToServerParams.userId, connectToServerParams.userName, connectToServerParams.endPoint, connectToServerParams.serverUserId, connectToServerParams.serverUserName, connectToServerParams.serverIsConnectionOwner);
            Action action3 = this.onInitializedEvent;
            if (action3 != null)
            {
                action3();
            }
            return true;
        }
        if (initParams is LiteNetLibConnectionManager.StartClientParams)
        {
            this._mode = LiteNetLibConnectionManager.NetworkMode.Client;
            Action action4 = this.onInitializedEvent;
            if (action4 != null)
            {
                action4();
            }
            return true;
        }
        return false;
    }

    // Token: 0x0600032D RID: 813 RVA: 0x000076B8 File Offset: 0x000058B8
    public void ConnectToEndPoint(string userId, string userName, IPEndPoint remoteEndPoint, string remoteUserId, string remoteUserName, bool remoteUserIsConnectionOwner)
    {
        if (this._connectionState != LiteNetLibConnectionManager.ConnectionState.Connected)
        {
            this._connectionState = LiteNetLibConnectionManager.ConnectionState.Connecting;
        }
        this._userId = (userId ?? string.Empty);
        this._userName = (userName ?? string.Empty);
        this.CreatePendingConnection(this._netManager.Connect(remoteEndPoint, this.GetConnectionMessage()), remoteUserId, remoteUserName, remoteUserIsConnectionOwner);
    }

    // Token: 0x0600032E RID: 814 RVA: 0x00007713 File Offset: 0x00005913
    public void Dispose()
    {
        this.DisposeInternal();
        this._netManager.Stop();
    }

    // Token: 0x0600032F RID: 815 RVA: 0x00007728 File Offset: 0x00005928
    public Task DisposeAsync()
    {
        this.DisposeInternal();
        if (!this._netManager.IsRunning)
        {
            return Task.CompletedTask;
        }
        return this._taskUtility.Run(delegate ()
        {
            this._netManager.Stop();
        }, default(CancellationToken));
    }

    // Token: 0x06000330 RID: 816 RVA: 0x0000776E File Offset: 0x0000596E
    private void DisposeInternal()
    {
        this.Disconnect(DisconnectedReason.UserInitiated);
        this._mode = LiteNetLibConnectionManager.NetworkMode.None;
        CancellationTokenSource backgroundSentryShutdownCts = this._backgroundSentryShutdownCts;
        if (backgroundSentryShutdownCts != null)
        {
            backgroundSentryShutdownCts.Cancel();
        }
        this._encryptionLayer.RemoveAllEndpoints();
    }

    // Token: 0x06000331 RID: 817 RVA: 0x0000779A File Offset: 0x0000599A
    public void Disconnect(DisconnectedReason disconnectedReason = DisconnectedReason.UserInitiated)
    {
        this.DisconnectInternal(disconnectedReason, ConnectionFailedReason.ConnectionCanceled);
    }

    // Token: 0x06000332 RID: 818 RVA: 0x000077A4 File Offset: 0x000059A4
    private void DisconnectInternal(DisconnectedReason disconnectedReason = DisconnectedReason.UserInitiated, ConnectionFailedReason connectionFailedReason = ConnectionFailedReason.Unknown)
    {
        if (this._connectionState == LiteNetLibConnectionManager.ConnectionState.Unconnected || this._connectionState == LiteNetLibConnectionManager.ConnectionState.Disconnecting)
        {
            return;
        }
        bool flag = this._connectionState == LiteNetLibConnectionManager.ConnectionState.Connecting;
        this._connectionState = LiteNetLibConnectionManager.ConnectionState.Disconnecting;
        CancellationTokenSource backgroundSentryDisconnectCts = this._backgroundSentryDisconnectCts;
        if (backgroundSentryDisconnectCts != null)
        {
            backgroundSentryDisconnectCts.Cancel();
        }
        this._netManager.DisconnectAll();
        this._netManager.PollEvents();
        this._connectionState = LiteNetLibConnectionManager.ConnectionState.Unconnected;
        if (flag)
        {
            Action<ConnectionFailedReason> action = this.onConnectionFailedEvent;
            if (action == null)
            {
                return;
            }
            action(connectionFailedReason);
            return;
        }
        else
        {
            Action<DisconnectedReason> action2 = this.onDisconnectedEvent;
            if (action2 == null)
            {
                return;
            }
            action2(disconnectedReason);
            return;
        }
    }

    // Token: 0x06000333 RID: 819 RVA: 0x00007828 File Offset: 0x00005A28
    private bool TryStartNetManager(int port, bool enableBackgroundSentry)
    {
        if (this._netManager.IsRunning && (this._netManager.LocalPort == port || port == 0))
        {
            if (enableBackgroundSentry)
            {
                this.StartBackgroundSentry();
            }
            return true;
        }
        this._netManager.Stop();
        if (!this._netManager.Start(port))
        {
            return false;
        }
        if (port != 0)
        {
            this._netManager.SendBroadcast(new byte[1], port);
        }
        if (enableBackgroundSentry)
        {
            this.StartBackgroundSentry();
        }
        return true;
    }

    // Token: 0x06000334 RID: 820 RVA: 0x0000789C File Offset: 0x00005A9C
    private void StartBackgroundSentry()
    {
        this._lastPollUpdateTime = DateTime.UtcNow.Ticks;
        this._sentryDisconnected = false;
        CancellationTokenSource backgroundSentryDisconnectCts = this._backgroundSentryDisconnectCts;
        if (backgroundSentryDisconnectCts != null)
        {
            backgroundSentryDisconnectCts.Cancel();
        }
        CancellationTokenSource backgroundSentryShutdownCts = this._backgroundSentryShutdownCts;
        if (backgroundSentryShutdownCts != null)
        {
            backgroundSentryShutdownCts.Cancel();
        }
        this._backgroundSentryDisconnectCts = new CancellationTokenSource();
        this._backgroundSentryShutdownCts = new CancellationTokenSource();
        this._taskUtility.Run(new Func<Task>(this.BackgroundDisconnectSentry), default(CancellationToken));
        this._taskUtility.Run(new Func<Task>(this.BackgroundShutdownSentry), default(CancellationToken));
    }

    // Token: 0x06000335 RID: 821 RVA: 0x0000793D File Offset: 0x00005B3D
    private void CheckSentryState()
    {
        if (this._sentryDisconnected)
        {
            this.DisconnectInternal(DisconnectedReason.Timeout, ConnectionFailedReason.ServerUnreachable);
            this._sentryDisconnected = false;
        }
        if (this._sentryShutdown)
        {
            this._mode = LiteNetLibConnectionManager.NetworkMode.None;
            this._encryptionLayer.RemoveAllEndpoints();
            this._sentryShutdown = false;
        }
    }

    // Token: 0x06000336 RID: 822 RVA: 0x00007978 File Offset: 0x00005B78
    private void UpdateStatistics()
    {
        if (!this._netManager.EnableStatistics)
        {
            return;
        }
        long ticks = DateTime.UtcNow.Ticks;
        if (this._lastStatisticsUpdateTime >= ticks - 300000000L)
        {
            return;
        }
        this._lastStatisticsUpdateTime = ticks;
        NetworkStatisticsState.NetworkStatisticsUpdateDelegate networkStatisticsUpdateDelegate = this.onStatisticsUpdatedEvent;
        if (networkStatisticsUpdateDelegate == null)
        {
            return;
        }
        NetworkStatisticsState networkStatisticsState = new NetworkStatisticsState(this._netManager.Statistics.PacketsSent, this._netManager.Statistics.PacketsReceived, this._netManager.Statistics.BytesSent, this._netManager.Statistics.BytesReceived, this._netManager.Statistics.PacketLoss, this._encryptionLayer.statistics.packetsSentEncrypted, this._encryptionLayer.statistics.packetsSentPlaintext, this._encryptionLayer.statistics.packetsSentRejected, this._encryptionLayer.statistics.packetsReceivedEncrypted, this._encryptionLayer.statistics.packetsReceivedPlaintext, this._encryptionLayer.statistics.packetsReceivedRejected, this._encryptionLayer.statistics.encryptionProcessingTime, this._encryptionLayer.statistics.decryptionProcessingTime);
        networkStatisticsUpdateDelegate(networkStatisticsState);
    }

    // Token: 0x06000337 RID: 823 RVA: 0x00007AA1 File Offset: 0x00005CA1
    public IConnection GetConnection(int index)
    {
        return this._connections[index];
    }

    // Token: 0x06000338 RID: 824 RVA: 0x00007AB0 File Offset: 0x00005CB0
    private bool IsConnectedToUser(string userId)
    {
        for (int i = 0; i < this._connections.Count; i++)
        {
            if (this._connections[i].userId == userId)
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06000339 RID: 825 RVA: 0x00007AF0 File Offset: 0x00005CF0
    private bool HasConnectionToEndPoint(IPEndPoint endPoint)
    {
        for (int i = 0; i < this._connections.Count; i++)
        {
            if (object.Equals(this._connections[i].netPeer.EndPoint, endPoint))
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x0600033A RID: 826 RVA: 0x00007B34 File Offset: 0x00005D34
    private bool HasPendingConnectionToEndPoint(IPEndPoint endPoint)
    {
        for (int i = 0; i < this._pendingConnections.Count; i++)
        {
            if (object.Equals(this._pendingConnections[i].netPeer.EndPoint, endPoint))
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x0600033B RID: 827 RVA: 0x00007B78 File Offset: 0x00005D78
    void INetEventListener.OnPeerConnected(NetPeer peer)
    {
        int i = 0;
        while (i < this._pendingConnections.Count)
        {
            if (this._pendingConnections[i].netPeer == peer)
            {
                LiteNetLibConnectionManager.NetPeerConnection netPeerConnection = this._pendingConnections[i];
                this._pendingConnections.RemoveAt(i);
                for (int j = 0; j < this._connections.Count; j++)
                {
                    if (this._connections[j].userId == netPeerConnection.userId)
                    {
                        netPeerConnection.Disconnect();
                        return;
                    }
                }
                this._connections.Add(netPeerConnection);
                if (this._connectionState == LiteNetLibConnectionManager.ConnectionState.Connecting)
                {
                    this._connectionState = LiteNetLibConnectionManager.ConnectionState.Connected;
                    Action action = this.onConnectedEvent;
                    if (action != null)
                    {
                        action();
                    }
                }
                Action<IConnection> action2 = this.onConnectionConnectedEvent;
                if (action2 == null)
                {
                    return;
                }
                action2(netPeerConnection);
                return;
            }
            else
            {
                i++;
            }
        }
        peer.Disconnect();
    }

    // Token: 0x0600033C RID: 828 RVA: 0x00007C51 File Offset: 0x00005E51
    void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        if (socketError == SocketError.NetworkUnreachable)
        {
            this.DisconnectInternal(DisconnectedReason.NetworkDisconnected, ConnectionFailedReason.NetworkNotConnected);
        }
    }

    // Token: 0x0600033D RID: 829 RVA: 0x00002273 File Offset: 0x00000473
    void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latencyMs)
    {
    }

    // Token: 0x0600033E RID: 830 RVA: 0x00007C68 File Offset: 0x00005E68
    void INetEventListener.OnConnectionRequest(ConnectionRequest request)
    {
        this._pendingReconnections.Remove(request.RemoteEndPoint);
        string userName = null;
        string userId = null;
        bool isConnectionOwner;
        if (this._connectionState != LiteNetLibConnectionManager.ConnectionState.Unconnected && this._connectionState != LiteNetLibConnectionManager.ConnectionState.Disconnecting && this.connectionRequestHandler.ValidateConnectionMessage(request.Data, out userId, out userName, out isConnectionOwner))
        {
            this.TryAccept(request, userId, userName, isConnectionOwner);
            return;
        }
        request.Reject();
        this.TryDisconnect(DisconnectReason.ConnectionRejected);
    }

    // Token: 0x0600033F RID: 831 RVA: 0x00007CCD File Offset: 0x00005ECD
    void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        if (disconnectInfo.Reason != DisconnectReason.Reconnect && disconnectInfo.Reason != DisconnectReason.PeerToPeerConnection)
        {
            this._encryptionLayer.RemoveEncryptedEndpoint(peer.EndPoint, null);
        }
        this.RemoveConnection(peer, disconnectInfo.Reason);
    }

    // Token: 0x06000340 RID: 832 RVA: 0x00007D04 File Offset: 0x00005F04
    void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, LiteNetLib.DeliveryMethod deliveryMethod)
    {
        if (this._connectionState == LiteNetLibConnectionManager.ConnectionState.Unconnected || this._connectionState == LiteNetLibConnectionManager.ConnectionState.Disconnecting)
        {
            reader.Recycle();
            return;
        }
        LiteNetLibConnectionManager.NetPeerConnection connection = this.GetConnection(peer);
        if (connection != null)
        {
            Action<IConnection, NetDataReader, BGNet.Core.DeliveryMethod> action = this.onReceivedDataEvent;
            if (action != null)
            {
                action(connection, reader, LiteNetLibConnectionManager.FromLiteNetDeliveryMethod(deliveryMethod));
            }
        }
        reader.Recycle();
    }

    // Token: 0x06000341 RID: 833 RVA: 0x00007D53 File Offset: 0x00005F53
    void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        IUnconnectedMessageReceiver unconnectedMessageReceiver = this._unconnectedMessageReceiver;
        if (unconnectedMessageReceiver != null)
        {
            unconnectedMessageReceiver.ReceiveUnconnectedMessage(remoteEndPoint, reader);
        }
        Action<IPEndPoint, NetDataReader> action = this.onReceiveUnconnectedDataEvent;
        if (action != null)
        {
            action(remoteEndPoint, reader);
        }
        reader.Recycle();
    }

    // Token: 0x06000342 RID: 834 RVA: 0x00007D84 File Offset: 0x00005F84
    private LiteNetLibConnectionManager.NetPeerConnection GetConnection(NetPeer peer)
    {
        for (int i = 0; i < this._connections.Count; i++)
        {
            if (this._connections[i].netPeer == peer)
            {
                return this._connections[i];
            }
        }
        return null;
    }

    // Token: 0x06000343 RID: 835 RVA: 0x00007DCC File Offset: 0x00005FCC
    private void AcceptAllPendingRequests()
    {
        for (int i = 0; i < this._pendingRequests.Count; i++)
        {
            LiteNetLibConnectionManager.NetPeerConnectionRequest netPeerConnectionRequest = this._pendingRequests[i];
            this.CreatePendingConnection(netPeerConnectionRequest.Accept(), netPeerConnectionRequest.userId, netPeerConnectionRequest.userName, netPeerConnectionRequest.isConnectionOwner);
        }
        this._pendingRequests.Clear();
    }

    // Token: 0x06000344 RID: 836 RVA: 0x00007E25 File Offset: 0x00006025
    private void TryAccept(ConnectionRequest request, string userId, string userName, bool isConnectionOwner)
    {
        this.CreatePendingConnection(request.Accept(), userId, userName, isConnectionOwner);
    }

    // Token: 0x06000345 RID: 837 RVA: 0x00007E37 File Offset: 0x00006037
    private void CreatePendingConnection(NetPeer peer, string userId, string userName, bool isConnectionOwner)
    {
        if (peer != null && !this.HasConnectionToEndPoint(peer.EndPoint) && !this.HasPendingConnectionToEndPoint(peer.EndPoint))
        {
            this._pendingConnections.Add(new LiteNetLibConnectionManager.NetPeerConnection(peer, userId, userName, isConnectionOwner));
        }
    }

    // Token: 0x06000346 RID: 838 RVA: 0x00007E70 File Offset: 0x00006070
    private void RemoveConnection(NetPeer netPeer, DisconnectReason reason)
    {
        if (reason == DisconnectReason.Reconnect || reason == DisconnectReason.PeerToPeerConnection)
        {
            this._pendingReconnections.Add(netPeer.EndPoint);
        }
        for (int i = 0; i < this._pendingConnections.Count; i++)
        {
            if (this._pendingConnections[i].netPeer == netPeer)
            {
                this._pendingConnections.RemoveAt(i);
                this.TryDisconnect(reason);
                return;
            }
        }
        for (int j = 0; j < this._connections.Count; j++)
        {
            if (this._connections[j].netPeer == netPeer)
            {
                LiteNetLibConnectionManager.NetPeerConnection arg = this._connections[j];
                this._connections.RemoveAt(j);
                Action<IConnection, DisconnectedReason> action = this.onConnectionDisconnectedEvent;
                if (action != null)
                {
                    action(arg, this.ToDisconnectedReason(reason));
                }
                this.TryDisconnect(reason);
                return;
            }
        }
    }

    // Token: 0x06000347 RID: 839 RVA: 0x00007F3C File Offset: 0x0000613C
    private void TryDisconnect(DisconnectReason reason)
    {
        if (this.isClient && this._pendingConnections.Count == 0 && this._connections.Count == 0 && this._pendingReconnections.Count == 0)
        {
            this.DisconnectInternal(this.ToDisconnectedReason(reason), this.ToConnectionFailedReason(reason));
        }
    }

    // Token: 0x06000348 RID: 840 RVA: 0x00007F8C File Offset: 0x0000618C
    private DisconnectedReason ToDisconnectedReason(DisconnectReason disconnectReason)
    {
        if (disconnectReason == DisconnectReason.Timeout)
        {
            return DisconnectedReason.Timeout;
        }
        if (disconnectReason != DisconnectReason.RemoteConnectionClose)
        {
            return DisconnectedReason.Unknown;
        }
        if (!this.isClient)
        {
            return DisconnectedReason.ClientConnectionClosed;
        }
        return DisconnectedReason.ServerConnectionClosed;
    }

    // Token: 0x06000349 RID: 841 RVA: 0x00007FA7 File Offset: 0x000061A7
    private ConnectionFailedReason ToConnectionFailedReason(DisconnectReason disconnectReason)
    {
        if (disconnectReason == DisconnectReason.DisconnectPeerCalled)
        {
            return ConnectionFailedReason.ServerAtCapacity;
        }
        return ConnectionFailedReason.ServerUnreachable;
    }

    // Token: 0x0600034A RID: 842 RVA: 0x00007FB0 File Offset: 0x000061B0
    private NetDataWriter GetConnectionMessage()
    {
        NetDataWriter netDataWriter = new NetDataWriter();
        this._connectionRequestHandler.GetConnectionMessage(netDataWriter, this.userId, this.userName, this.isConnectionOwner);
        return netDataWriter;
    }

    // Token: 0x0600034B RID: 843 RVA: 0x00007FE4 File Offset: 0x000061E4
    private async Task BackgroundDisconnectSentry()
    {
        CancellationToken cancellationToken = this._backgroundSentryDisconnectCts.Token;
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                long num = this._lastPollUpdateTime + 1200000000L - DateTime.UtcNow.Ticks;
                if (num <= 0L)
                {
                    this._netManager.DisconnectAll();
                    this._sentryDisconnected = true;
                    break;
                }
                await this._taskUtility.Delay(TimeSpan.FromTicks(num + 10000L), cancellationToken);
            }
        }
        catch (Exception)
        {
        }
    }

    // Token: 0x0600034C RID: 844 RVA: 0x0000802C File Offset: 0x0000622C
    private async Task BackgroundShutdownSentry()
    {
        CancellationToken cancellationToken = this._backgroundSentryShutdownCts.Token;
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                long num = this._lastPollUpdateTime + 9000000000L - DateTime.UtcNow.Ticks;
                if (num <= 0L)
                {
                    this._netManager.Stop();
                    this._sentryShutdown = true;
                    break;
                }
                await this._taskUtility.Delay(TimeSpan.FromTicks(num + 10000L), cancellationToken);
            }
        }
        catch (Exception)
        {
        }
    }

    // Token: 0x0600034D RID: 845 RVA: 0x00008071 File Offset: 0x00006271
    private static LiteNetLib.DeliveryMethod ToLiteNetDeliveryMethod(BGNet.Core.DeliveryMethod deliveryMethod)
    {
        if (deliveryMethod != BGNet.Core.DeliveryMethod.Unreliable && deliveryMethod == BGNet.Core.DeliveryMethod.ReliableOrdered)
        {
            return LiteNetLib.DeliveryMethod.ReliableOrdered;
        }
        return LiteNetLib.DeliveryMethod.Unreliable;
    }

    // Token: 0x0600034E RID: 846 RVA: 0x0000807D File Offset: 0x0000627D
    private static BGNet.Core.DeliveryMethod FromLiteNetDeliveryMethod(LiteNetLib.DeliveryMethod deliveryMethod)
    {
        if (deliveryMethod != LiteNetLib.DeliveryMethod.ReliableOrdered)
        {
            if (deliveryMethod != LiteNetLib.DeliveryMethod.Unreliable)
            {
            }
            return BGNet.Core.DeliveryMethod.Unreliable;
        }
        return BGNet.Core.DeliveryMethod.ReliableOrdered;
    }

    // Token: 0x0600034F RID: 847 RVA: 0x0000808C File Offset: 0x0000628C
    [Conditional("VERBOSE_LOGGING")]
    private void Log(string msg)
    {
        BGNet.Logging.Debug.Log("[LNLCM] " + msg);
    }

    // Token: 0x06000350 RID: 848 RVA: 0x0000809E File Offset: 0x0000629E
    private void LogError(string msg)
    {
        BGNet.Logging.Debug.LogError("[LNLCM] " + msg);
    }

    // Token: 0x06000351 RID: 849 RVA: 0x000080B0 File Offset: 0x000062B0
    private static string GetLogFormatConnection(LiteNetLibConnectionManager.NetPeerConnection netPeerConnection)
    {
        return LiteNetLibConnectionManager.GetLogFormatUserInfo(netPeerConnection.userName, netPeerConnection.userId, netPeerConnection.netPeer.EndPoint);
    }

    // Token: 0x06000352 RID: 850 RVA: 0x000080CE File Offset: 0x000062CE
    private static string GetLogFormatUserInfo(string userName, string userId, IPEndPoint ipEndPoint)
    {
        return string.Format("hashed user id({0} - {1})", userId, ipEndPoint);
    }

    // Token: 0x0400010C RID: 268
    private const long kBackgroundDisconnectTimeout = 1200000000L;

    // Token: 0x0400010D RID: 269
    private const long kBackgroundShutdownTimeout = 9000000000L;

    // Token: 0x0400010E RID: 270
    private const long kStatisticsUpdateInterval = 300000000L;

    // Token: 0x0400010F RID: 271
    private readonly byte[] _unconnectedPacketHeader = new byte[]
    {
        8
    };

    // Token: 0x04000110 RID: 272
    private readonly NetManager _netManager;

    // Token: 0x04000111 RID: 273
    private readonly PacketEncryptionLayer _encryptionLayer;

    // Token: 0x04000112 RID: 274
    private readonly ITaskUtility _taskUtility;

    // Token: 0x04000113 RID: 275
    private readonly List<LiteNetLibConnectionManager.NetPeerConnection> _connections = new List<LiteNetLibConnectionManager.NetPeerConnection>();

    // Token: 0x04000114 RID: 276
    private readonly List<LiteNetLibConnectionManager.NetPeerConnection> _pendingConnections = new List<LiteNetLibConnectionManager.NetPeerConnection>();

    // Token: 0x04000115 RID: 277
    private readonly List<LiteNetLibConnectionManager.NetPeerConnectionRequest> _pendingRequests = new List<LiteNetLibConnectionManager.NetPeerConnectionRequest>();

    // Token: 0x04000116 RID: 278
    private readonly HashSet<IPEndPoint> _pendingReconnections = new HashSet<IPEndPoint>();

    // Token: 0x04000117 RID: 279
    private string _userId;

    // Token: 0x04000118 RID: 280
    private string _userName;

    // Token: 0x04000119 RID: 281
    private IConnectionRequestHandler _connectionRequestHandler;

    // Token: 0x0400011A RID: 282
    private IUnconnectedMessageReceiver _unconnectedMessageReceiver;

    // Token: 0x0400011B RID: 283
    private LiteNetLibConnectionManager.NetworkMode _mode;

    // Token: 0x0400011C RID: 284
    private LiteNetLibConnectionManager.ConnectionState _connectionState;

    // Token: 0x0400011D RID: 285
    private CancellationTokenSource _backgroundSentryDisconnectCts;

    // Token: 0x0400011E RID: 286
    private CancellationTokenSource _backgroundSentryShutdownCts;

    // Token: 0x0400011F RID: 287
    private bool _sentryDisconnected;

    // Token: 0x04000120 RID: 288
    private bool _sentryShutdown;

    // Token: 0x04000121 RID: 289
    private long _lastPollUpdateTime;

    // Token: 0x04000122 RID: 290
    private long _lastStatisticsUpdateTime;

    // Token: 0x020000F8 RID: 248
    private enum NetworkMode
    {
        // Token: 0x04000385 RID: 901
        None,
        // Token: 0x04000386 RID: 902
        Client,
        // Token: 0x04000387 RID: 903
        Server
    }

    // Token: 0x020000F9 RID: 249
    private enum ConnectionState
    {
        // Token: 0x04000389 RID: 905
        Unconnected,
        // Token: 0x0400038A RID: 906
        Connecting,
        // Token: 0x0400038B RID: 907
        Connected,
        // Token: 0x0400038C RID: 908
        Disconnecting
    }

    // Token: 0x020000FA RID: 250
    public abstract class LiteNetLibConnectionParamsBase : IConnectionInitParams<LiteNetLibConnectionManager>
    {
        // Token: 0x0400038D RID: 909
        public IConnectionRequestHandler connectionRequestHandler;

        // Token: 0x0400038E RID: 910
        public int port;

        // Token: 0x0400038F RID: 911
        public bool filterUnencryptedTraffic;

        // Token: 0x04000390 RID: 912
        public bool enableUnconnectedMessages;

        // Token: 0x04000391 RID: 913
        public bool enableBackgroundSentry;

        // Token: 0x04000392 RID: 914
        public bool enableStatistics;

        // Token: 0x04000393 RID: 915
        public int disconnectTimeoutMs = 5000;
    }

    // Token: 0x020000FB RID: 251
    public class StartServerParams : LiteNetLibConnectionManager.LiteNetLibConnectionParamsBase
    {
        // Token: 0x04000394 RID: 916
        public string userId;

        // Token: 0x04000395 RID: 917
        public string userName;
    }

    // Token: 0x020000FC RID: 252
    public class StartClientParams : LiteNetLibConnectionManager.LiteNetLibConnectionParamsBase
    {
    }

    // Token: 0x020000FD RID: 253
    public class ConnectToServerParams : LiteNetLibConnectionManager.LiteNetLibConnectionParamsBase
    {
        // Token: 0x04000396 RID: 918
        public string userId;

        // Token: 0x04000397 RID: 919
        public string userName;

        // Token: 0x04000398 RID: 920
        public IPEndPoint endPoint;

        // Token: 0x04000399 RID: 921
        public string serverUserId;

        // Token: 0x0400039A RID: 922
        public string serverUserName;

        // Token: 0x0400039B RID: 923
        public bool serverIsConnectionOwner = true;
    }

    // Token: 0x020000FE RID: 254
    private class NetPeerConnectionRequest
    {
        // Token: 0x1700014E RID: 334
        // (get) Token: 0x060007B1 RID: 1969 RVA: 0x00014893 File Offset: 0x00012A93
        public string userId
        {
            get
            {
                return this._userId;
            }
        }

        // Token: 0x1700014F RID: 335
        // (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001489B File Offset: 0x00012A9B
        public string userName
        {
            get
            {
                return this._userName;
            }
        }

        // Token: 0x17000150 RID: 336
        // (get) Token: 0x060007B3 RID: 1971 RVA: 0x000148A3 File Offset: 0x00012AA3
        public bool isConnectionOwner
        {
            get
            {
                return this._isConnectionOwner;
            }
        }

        // Token: 0x17000151 RID: 337
        // (get) Token: 0x060007B4 RID: 1972 RVA: 0x000148AB File Offset: 0x00012AAB
        public IPEndPoint endPoint
        {
            get
            {
                return this._request.RemoteEndPoint;
            }
        }

        // Token: 0x060007B5 RID: 1973 RVA: 0x000148B8 File Offset: 0x00012AB8
        public NetPeerConnectionRequest(ConnectionRequest request, string userId, string userName, bool isConnectionOwner)
        {
            this._request = request;
            this._userId = userId;
            this._userName = userName;
            this._isConnectionOwner = isConnectionOwner;
        }

        // Token: 0x060007B6 RID: 1974 RVA: 0x000148DD File Offset: 0x00012ADD
        public NetPeer Accept()
        {
            return this._request.Accept();
        }

        // Token: 0x0400039C RID: 924
        private readonly string _userId;

        // Token: 0x0400039D RID: 925
        private readonly string _userName;

        // Token: 0x0400039E RID: 926
        private readonly bool _isConnectionOwner;

        // Token: 0x0400039F RID: 927
        private readonly ConnectionRequest _request;
    }

    // Token: 0x020000FF RID: 255
    private class NetPeerConnection : IConnection, IEquatable<LiteNetLibConnectionManager.NetPeerConnection>
    {
        // Token: 0x17000152 RID: 338
        // (get) Token: 0x060007B7 RID: 1975 RVA: 0x000148EA File Offset: 0x00012AEA
        public string userId
        {
            get
            {
                return this._userId;
            }
        }

        // Token: 0x17000153 RID: 339
        // (get) Token: 0x060007B8 RID: 1976 RVA: 0x000148F2 File Offset: 0x00012AF2
        public string userName
        {
            get
            {
                return this._userName;
            }
        }

        // Token: 0x17000154 RID: 340
        // (get) Token: 0x060007B9 RID: 1977 RVA: 0x000148FA File Offset: 0x00012AFA
        public bool isConnectionOwner
        {
            get
            {
                return this._isConnectionOwner;
            }
        }

        // Token: 0x060007BA RID: 1978 RVA: 0x00014902 File Offset: 0x00012B02
        public NetPeerConnection(NetPeer netPeer, string userId, string userName, bool isConnectionOwner)
        {
            this.netPeer = netPeer;
            this._userId = userId;
            this._userName = userName;
            this._isConnectionOwner = isConnectionOwner;
        }

        // Token: 0x060007BB RID: 1979 RVA: 0x00014927 File Offset: 0x00012B27
        public void Send(NetDataWriter writer, BGNet.Core.DeliveryMethod deliveryMethod)
        {
            this.netPeer.Send(writer, LiteNetLibConnectionManager.ToLiteNetDeliveryMethod(deliveryMethod));
        }

        // Token: 0x060007BC RID: 1980 RVA: 0x0001493B File Offset: 0x00012B3B
        public bool Equals(LiteNetLibConnectionManager.NetPeerConnection other)
        {
            return other != null && (this == other || object.Equals(this.netPeer, other.netPeer));
        }

        // Token: 0x060007BD RID: 1981 RVA: 0x00014959 File Offset: 0x00012B59
        public override bool Equals(object obj)
        {
            return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((LiteNetLibConnectionManager.NetPeerConnection)obj)));
        }

        // Token: 0x060007BE RID: 1982 RVA: 0x00014987 File Offset: 0x00012B87
        public override int GetHashCode()
        {
            return this.netPeer.GetHashCode();
        }

        // Token: 0x060007BF RID: 1983 RVA: 0x00014994 File Offset: 0x00012B94
        public void Disconnect()
        {
            this.netPeer.Disconnect();
        }

        // Token: 0x040003A0 RID: 928
        private readonly string _userId;

        // Token: 0x040003A1 RID: 929
        private readonly string _userName;

        // Token: 0x040003A2 RID: 930
        private readonly bool _isConnectionOwner;

        // Token: 0x040003A3 RID: 931
        public readonly NetPeer netPeer;
    }
}
