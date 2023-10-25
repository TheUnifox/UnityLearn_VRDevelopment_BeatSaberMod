using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;
using BGNet.Core.GameLift;
using BGNet.Logging;
using LiteNetLib.Utils;

// Token: 0x0200001E RID: 30
public class GameLiftConnectionManager : IConnectionManager, IPollable, IDisposable
{
    // Token: 0x1400000C RID: 12
    // (add) Token: 0x060000F6 RID: 246 RVA: 0x000053FC File Offset: 0x000035FC
    // (remove) Token: 0x060000F7 RID: 247 RVA: 0x00005434 File Offset: 0x00003634
    public event Action onInitializedEvent;

    // Token: 0x1400000D RID: 13
    // (add) Token: 0x060000F8 RID: 248 RVA: 0x0000546C File Offset: 0x0000366C
    // (remove) Token: 0x060000F9 RID: 249 RVA: 0x000054A4 File Offset: 0x000036A4
    public event Action onConnectedEvent;

    // Token: 0x1400000E RID: 14
    // (add) Token: 0x060000FA RID: 250 RVA: 0x000054DC File Offset: 0x000036DC
    // (remove) Token: 0x060000FB RID: 251 RVA: 0x00005514 File Offset: 0x00003714
    public event Action<DisconnectedReason> onDisconnectedEvent;

    // Token: 0x1400000F RID: 15
    // (add) Token: 0x060000FC RID: 252 RVA: 0x0000554C File Offset: 0x0000374C
    // (remove) Token: 0x060000FD RID: 253 RVA: 0x00005584 File Offset: 0x00003784
    public event Action<ConnectionFailedReason> onConnectionFailedEvent;

    // Token: 0x14000010 RID: 16
    // (add) Token: 0x060000FE RID: 254 RVA: 0x000055BC File Offset: 0x000037BC
    // (remove) Token: 0x060000FF RID: 255 RVA: 0x000055F4 File Offset: 0x000037F4
    public event Action<IConnection> onConnectionConnectedEvent;

    // Token: 0x14000011 RID: 17
    // (add) Token: 0x06000100 RID: 256 RVA: 0x0000562C File Offset: 0x0000382C
    // (remove) Token: 0x06000101 RID: 257 RVA: 0x00005664 File Offset: 0x00003864
    public event Action<IConnection, DisconnectedReason> onConnectionDisconnectedEvent;

    // Token: 0x14000012 RID: 18
    // (add) Token: 0x06000102 RID: 258 RVA: 0x0000569C File Offset: 0x0000389C
    // (remove) Token: 0x06000103 RID: 259 RVA: 0x000056D4 File Offset: 0x000038D4
    public event Action<IConnection, NetDataReader, DeliveryMethod> onReceivedDataEvent;

    // Token: 0x1700002E RID: 46
    // (get) Token: 0x06000104 RID: 260 RVA: 0x00005709 File Offset: 0x00003909
    public string userId
    {
        get
        {
            if (this._authenticationTokenProviderTask == null || !this._authenticationTokenProviderTask.IsCompleted)
            {
                return null;
            }
            return this._authenticationTokenProviderTask.Result.hashedUserId;
        }
    }

    // Token: 0x1700002F RID: 47
    // (get) Token: 0x06000105 RID: 261 RVA: 0x00005732 File Offset: 0x00003932
    public string userName
    {
        get
        {
            if (this._authenticationTokenProviderTask == null || !this._authenticationTokenProviderTask.IsCompleted)
            {
                return null;
            }
            return this._authenticationTokenProviderTask.Result.userName;
        }
    }

    // Token: 0x17000030 RID: 48
    // (get) Token: 0x06000106 RID: 262 RVA: 0x0000575B File Offset: 0x0000395B
    public bool isConnected
    {
        get
        {
            return this._connectionState == GameLiftConnectionManager.ConnectionState.Connected;
        }
    }

    // Token: 0x17000031 RID: 49
    // (get) Token: 0x06000107 RID: 263 RVA: 0x00005766 File Offset: 0x00003966
    public bool isConnecting
    {
        get
        {
            return this._connectionState == GameLiftConnectionManager.ConnectionState.Connecting;
        }
    }

    // Token: 0x17000032 RID: 50
    // (get) Token: 0x06000108 RID: 264 RVA: 0x00005771 File Offset: 0x00003971
    public bool isDisconnecting
    {
        get
        {
            return this._connectionState == GameLiftConnectionManager.ConnectionState.Disconnecting;
        }
    }

    // Token: 0x17000033 RID: 51
    // (get) Token: 0x06000109 RID: 265 RVA: 0x0000577C File Offset: 0x0000397C
    public int connectionCount
    {
        get
        {
            return this._connectionManager.connectionCount;
        }
    }

    // Token: 0x17000034 RID: 52
    // (get) Token: 0x0600010A RID: 266 RVA: 0x00005789 File Offset: 0x00003989
    public bool isConnectionOwner
    {
        get
        {
            return this._connectionManager.isConnectionOwner;
        }
    }

    // Token: 0x17000035 RID: 53
    // (get) Token: 0x0600010B RID: 267 RVA: 0x00005796 File Offset: 0x00003996
    public bool isDisposed
    {
        get
        {
            return this._connectionManager.isDisposed;
        }
    }

    // Token: 0x17000036 RID: 54
    // (get) Token: 0x0600010C RID: 268 RVA: 0x000057A3 File Offset: 0x000039A3
    public string playerSessionId
    {
        get
        {
            return this._connectionRequestHandler.playerSessionId;
        }
    }

    // Token: 0x17000037 RID: 55
    // (get) Token: 0x0600010D RID: 269 RVA: 0x000057B0 File Offset: 0x000039B0
    public BeatmapLevelSelectionMask selectionMask
    {
        get
        {
            return this._selectionMask;
        }
    }

    // Token: 0x17000038 RID: 56
    // (get) Token: 0x0600010E RID: 270 RVA: 0x000057B8 File Offset: 0x000039B8
    public GameplayServerConfiguration configuration
    {
        get
        {
            return this._configuration;
        }
    }

    // Token: 0x17000039 RID: 57
    // (get) Token: 0x0600010F RID: 271 RVA: 0x000057C0 File Offset: 0x000039C0
    public string code
    {
        get
        {
            return this._code;
        }
    }

    // Token: 0x1700003A RID: 58
    // (get) Token: 0x06000110 RID: 272 RVA: 0x000057C8 File Offset: 0x000039C8
    public string secret
    {
        get
        {
            return this._secret;
        }
    }

    // Token: 0x06000111 RID: 273 RVA: 0x000057D0 File Offset: 0x000039D0
    public void SendToAll(NetDataWriter writer, DeliveryMethod deliveryMethod)
    {
        this._connectionManager.SendToAll(writer, deliveryMethod);
    }

    // Token: 0x06000112 RID: 274 RVA: 0x000057DF File Offset: 0x000039DF
    public void SendToAll(NetDataWriter writer, DeliveryMethod deliveryMethod, IConnection excludingConnection)
    {
        this._connectionManager.SendToAll(writer, deliveryMethod, excludingConnection);
    }

    // Token: 0x06000113 RID: 275 RVA: 0x000057EF File Offset: 0x000039EF
    public void PollUpdate()
    {
        this._connectionManager.PollUpdate();
    }

    // Token: 0x06000114 RID: 276 RVA: 0x000057FC File Offset: 0x000039FC
    public GameLiftConnectionManager() : this(DefaultTimeProvider.instance, DefaultTaskUtility.instance, new LiteNetLibConnectionManager(), new ClientCertificateValidator())
    {
    }

    // Token: 0x06000115 RID: 277 RVA: 0x00005818 File Offset: 0x00003A18
    public GameLiftConnectionManager(BGNet.Core.ITimeProvider timeProvider, ITaskUtility taskUtility, INetworkConnectionManager connectionManager, ICertificateValidator certificateValidator)
    {
        this._timeProvider = timeProvider;
        this._taskUtility = taskUtility;
        this._certificateValidator = certificateValidator;
        this._connectionManager = connectionManager;
        this._connectionManager.onConnectedEvent += this.HandleConnected;
        this._connectionManager.onDisconnectedEvent += this.Disconnect;
        this._connectionManager.onConnectionConnectedEvent += this.HandleConnectionConnected;
        this._connectionManager.onConnectionDisconnectedEvent += this.HandleConnectionDisconnected;
        this._connectionManager.onConnectionFailedEvent += this.HandleConnectionFailed;
        this._connectionManager.onReceivedDataEvent += this.HandleReceivedData;
    }

    // Token: 0x06000116 RID: 278 RVA: 0x000058E0 File Offset: 0x00003AE0
    public bool Init<T>(IConnectionInitParams<T> initParams) where T : IConnectionManager
    {
        this.Disconnect(DisconnectedReason.UserInitiated);
        GameLiftConnectionManager.GameLiftConnectionManagerParamsBase gameLiftConnectionManagerParamsBase;
        if ((gameLiftConnectionManagerParamsBase = (initParams as GameLiftConnectionManager.GameLiftConnectionManagerParamsBase)) != null)
        {
            this._selectionMask = gameLiftConnectionManagerParamsBase.selectionMask;
            this._configuration = gameLiftConnectionManagerParamsBase.configuration;
            this._authenticationTokenProviderTask = gameLiftConnectionManagerParamsBase.authenticationTokenProviderTask;
            this._gameLiftPlayerSessionProvider = gameLiftConnectionManagerParamsBase.gameLiftPlayerSessionProvider;
        }
        GameLiftConnectionManager.ConnectToServerParams connectToServerParams;
        if ((connectToServerParams = (initParams as GameLiftConnectionManager.ConnectToServerParams)) == null)
        {
            if (initParams is GameLiftConnectionManager.StartClientParams)
            {
                this._connectionState = GameLiftConnectionManager.ConnectionState.Unconnected;
                if (this._connectionManager.Init<LiteNetLibConnectionManager>(new LiteNetLibConnectionManager.StartClientParams
                {
                    connectionRequestHandler = this._connectionRequestHandler,
                    filterUnencryptedTraffic = true,
                    enableUnconnectedMessages = true,
                    enableBackgroundSentry = true
                }))
                {
                    Action action = this.onInitializedEvent;
                    if (action != null)
                    {
                        action();
                    }
                    return true;
                }
            }
            return false;
        }
        this._connectionState = GameLiftConnectionManager.ConnectionState.Connecting;
        this._connectionCancellationTokenSource = new CancellationTokenSource();
        if (!this._connectionManager.Init<LiteNetLibConnectionManager>(new LiteNetLibConnectionManager.StartClientParams
        {
            connectionRequestHandler = this._connectionRequestHandler,
            filterUnencryptedTraffic = true,
            enableUnconnectedMessages = true,
            enableBackgroundSentry = true
        }))
        {
            return false;
        }
        this.GameLiftConnectToServer(connectToServerParams.secret, connectToServerParams.code, this._connectionCancellationTokenSource.Token);
        Action action2 = this.onInitializedEvent;
        if (action2 != null)
        {
            action2();
        }
        return true;
    }

    // Token: 0x06000117 RID: 279 RVA: 0x00005A05 File Offset: 0x00003C05
    public Task DisposeAsync()
    {
        return this._connectionManager.DisposeAsync();
    }

    // Token: 0x06000118 RID: 280 RVA: 0x00005A12 File Offset: 0x00003C12
    public void Dispose()
    {
        this._connectionManager.Dispose();
    }

    // Token: 0x06000119 RID: 281 RVA: 0x00005A1F File Offset: 0x00003C1F
    public void Disconnect(DisconnectedReason disconnectedReason = DisconnectedReason.UserInitiated)
    {
        this.DisconnectInternal(disconnectedReason, ConnectionFailedReason.Unknown);
    }

    // Token: 0x0600011A RID: 282 RVA: 0x00005A29 File Offset: 0x00003C29
    private void DisconnectInternal(ConnectionFailedReason connectionFailedReason)
    {
        this.DisconnectInternal(DisconnectedReason.Unknown, connectionFailedReason);
    }

    // Token: 0x0600011B RID: 283 RVA: 0x00005A34 File Offset: 0x00003C34
    private void DisconnectInternal(DisconnectedReason disconnectedReason, ConnectionFailedReason connectionFailedReason)
    {
        if (this._connectionState == GameLiftConnectionManager.ConnectionState.Unconnected || this._connectionState == GameLiftConnectionManager.ConnectionState.Disconnecting)
        {
            return;
        }
        if (this._connectionManager.isDisconnecting)
        {
            return;
        }
        if (connectionFailedReason == ConnectionFailedReason.Unknown && disconnectedReason != DisconnectedReason.Unknown)
        {
            switch (disconnectedReason)
            {
                case DisconnectedReason.UserInitiated:
                    connectionFailedReason = ConnectionFailedReason.ConnectionCanceled;
                    goto IL_63;
                case DisconnectedReason.Timeout:
                case DisconnectedReason.Kicked:
                    break;
                case DisconnectedReason.ServerAtCapacity:
                    connectionFailedReason = ConnectionFailedReason.ServerAtCapacity;
                    goto IL_63;
                case DisconnectedReason.ServerConnectionClosed:
                    connectionFailedReason = ConnectionFailedReason.ServerDoesNotExist;
                    goto IL_63;
                default:
                    if (disconnectedReason == DisconnectedReason.NetworkDisconnected)
                    {
                        connectionFailedReason = ConnectionFailedReason.NetworkNotConnected;
                        goto IL_63;
                    }
                    break;
            }
            connectionFailedReason = ConnectionFailedReason.ServerUnreachable;
        }
    IL_63:
        bool flag = this._connectionState == GameLiftConnectionManager.ConnectionState.Connecting;
        this._connectionState = GameLiftConnectionManager.ConnectionState.Disconnecting;
        CancellationTokenSource connectionCancellationTokenSource = this._connectionCancellationTokenSource;
        if (connectionCancellationTokenSource != null)
        {
            connectionCancellationTokenSource.Cancel();
        }
        GameLiftClientMessageHandler messageHandler = this._messageHandler;
        if (messageHandler != null)
        {
            messageHandler.Dispose();
        }
        this._messageHandler = null;
        this._connectionManager.Disconnect(disconnectedReason);
        this._connectionState = GameLiftConnectionManager.ConnectionState.Unconnected;
        if (flag)
        {
            Debug.Log(string.Format("[GLCM] Connection failed {0}", connectionFailedReason));
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

    // Token: 0x0600011C RID: 284 RVA: 0x00005B2A File Offset: 0x00003D2A
    public IConnection GetConnection(int index)
    {
        return this._connectionManager.GetConnection(index);
    }

    // Token: 0x0600011D RID: 285 RVA: 0x00005B38 File Offset: 0x00003D38
    private void HandleConnected()
    {
        if (this._connectionState == GameLiftConnectionManager.ConnectionState.Connected)
        {
            return;
        }
        this._connectionState = GameLiftConnectionManager.ConnectionState.Connected;
        Action action = this.onConnectedEvent;
        if (action == null)
        {
            return;
        }
        action();
    }

    // Token: 0x0600011E RID: 286 RVA: 0x00005B5B File Offset: 0x00003D5B
    private void HandleConnectionConnected(IConnection connection)
    {
        Action<IConnection> action = this.onConnectionConnectedEvent;
        if (action == null)
        {
            return;
        }
        action(connection);
    }

    // Token: 0x0600011F RID: 287 RVA: 0x00005B6E File Offset: 0x00003D6E
    private void HandleConnectionDisconnected(IConnection connection, DisconnectedReason reason)
    {
        Action<IConnection, DisconnectedReason> action = this.onConnectionDisconnectedEvent;
        if (action == null)
        {
            return;
        }
        action(connection, reason);
    }

    // Token: 0x06000120 RID: 288 RVA: 0x00005B82 File Offset: 0x00003D82
    private void HandleConnectionFailed(ConnectionFailedReason failedReason)
    {
        this.DisconnectInternal(failedReason);
    }

    // Token: 0x06000121 RID: 289 RVA: 0x00005B8B File Offset: 0x00003D8B
    private void HandleReceivedData(IConnection connection, NetDataReader reader, DeliveryMethod deliveryMethod)
    {
        Action<IConnection, NetDataReader, DeliveryMethod> action = this.onReceivedDataEvent;
        if (action == null)
        {
            return;
        }
        action(connection, reader, deliveryMethod);
    }

    // Token: 0x06000122 RID: 290 RVA: 0x00005BA0 File Offset: 0x00003DA0
    private async void GameLiftConnectToServer(string secret, string code, CancellationToken cancellationToken)
    {
        PlayerSessionInfo playerSessionInfo = null;
        IAuthenticationTokenProvider authenticationTokenProvider = null;
        try
        {
            IAuthenticationTokenProvider authenticationTokenProvider2 = await this._authenticationTokenProviderTask;
            authenticationTokenProvider = authenticationTokenProvider2;
            playerSessionInfo = await this._gameLiftPlayerSessionProvider.GetGameLiftPlayerSessionInfo(authenticationTokenProvider, authenticationTokenProvider.hashedUserId, this.selectionMask, this.configuration, secret, code, cancellationToken);
        }
        catch (ConnectionFailedException ex)
        {
            this.DisconnectInternal(ex.reason);
            return;
        }
        catch (TaskCanceledException)
        {
            return;
        }
        catch (Exception)
        {
            this.DisconnectInternal(ConnectionFailedReason.MultiplayerApiUnreachable);
            return;
        }
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                this._messageHandler = new GameLiftClientMessageHandler(this._connectionManager, new DnsEndPoint(playerSessionInfo.dnsName, playerSessionInfo.port), this._timeProvider, this._taskUtility, this._certificateValidator, null);
                await this._messageHandler.AuthenticateWithGameLiftServer(authenticationTokenProvider.hashedUserId, authenticationTokenProvider.userName, playerSessionInfo.playerSessionId);
            }
            catch (ConnectionFailedException ex2)
            {
                this.DisconnectInternal(ex2.reason);
                return;
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (Exception)
            {
                this.DisconnectInternal(ConnectionFailedReason.ServerUnreachable);
                return;
            }
            if (!cancellationToken.IsCancellationRequested)
            {
                this.HandleConnectToServerSuccess(playerSessionInfo.playerSessionId, playerSessionInfo.dnsName, playerSessionInfo.port, playerSessionInfo.gameSessionId, playerSessionInfo.privateGameSecret, playerSessionInfo.privateGameCode, playerSessionInfo.beatmapLevelSelectionMask, playerSessionInfo.gameplayServerConfiguration);
            }
        }
    }

    // Token: 0x06000123 RID: 291 RVA: 0x00005BF4 File Offset: 0x00003DF4
    private void HandleConnectToServerSuccess(string playerSessionId, string hostName, int port, string gameSessionId, string secret, string code, BeatmapLevelSelectionMask selectionMask, GameplayServerConfiguration configuration)
    {
        Debug.Log(string.Format("Successful Connect to Server Response: {0}:{1} isConnectionOwner: {2} isDedicatedServer: {3}", new object[]
        {
            hostName,
            port,
            true,
            true
        }));
        this._selectionMask = selectionMask;
        this._configuration = configuration;
        this._code = code;
        this._secret = (string.IsNullOrEmpty(secret) ? gameSessionId : secret);
        this._connectionRequestHandler.playerSessionId = playerSessionId;
        if (!this._connectionManager.Init<LiteNetLibConnectionManager>(new LiteNetLibConnectionManager.ConnectToServerParams
        {
            connectionRequestHandler = this._connectionRequestHandler,
            userId = this.userId,
            userName = this.userName,
            endPoint = this._messageHandler.endPoint.endPoint,
            serverUserId = gameSessionId,
            serverUserName = string.Empty,
            enableBackgroundSentry = true
        }))
        {
            this.DisconnectInternal(ConnectionFailedReason.Unknown);
        }
    }

    // Token: 0x06000124 RID: 292 RVA: 0x00005CDB File Offset: 0x00003EDB
    public void GetPublicServers(Action<IReadOnlyList<PublicServerInfo>> onSuccess, Action<ConnectionFailedReason> onFailure, BeatmapLevelSelectionMask selectionMask, GameplayServerConfiguration configuration, int offset = 0, int count = 20)
    {
        Debug.LogError("Get Public Servers needs to be implemented");
        throw new NotImplementedException();
    }

    // Token: 0x040000A2 RID: 162
    private readonly BGNet.Core.ITimeProvider _timeProvider;

    // Token: 0x040000A3 RID: 163
    private readonly ITaskUtility _taskUtility;

    // Token: 0x040000A4 RID: 164
    private readonly INetworkConnectionManager _connectionManager;

    // Token: 0x040000A5 RID: 165
    private readonly ICertificateValidator _certificateValidator;

    // Token: 0x040000A6 RID: 166
    private string _code;

    // Token: 0x040000A7 RID: 167
    private string _secret;

    // Token: 0x040000A8 RID: 168
    private BeatmapLevelSelectionMask _selectionMask;

    // Token: 0x040000A9 RID: 169
    private GameplayServerConfiguration _configuration;

    // Token: 0x040000AA RID: 170
    private GameLiftClientMessageHandler _messageHandler;

    // Token: 0x040000AB RID: 171
    private GameLiftConnectionManager.ConnectionState _connectionState;

    // Token: 0x040000AC RID: 172
    private CancellationTokenSource _connectionCancellationTokenSource;

    // Token: 0x040000AD RID: 173
    private Task<IAuthenticationTokenProvider> _authenticationTokenProviderTask;

    // Token: 0x040000AE RID: 174
    private IGameLiftPlayerSessionProvider _gameLiftPlayerSessionProvider;

    // Token: 0x040000AF RID: 175
    private readonly GameLiftClientConnectionRequestHandler _connectionRequestHandler = new GameLiftClientConnectionRequestHandler();

    // Token: 0x020000E2 RID: 226
    private enum ConnectionState
    {
        // Token: 0x04000359 RID: 857
        Unconnected,
        // Token: 0x0400035A RID: 858
        Connecting,
        // Token: 0x0400035B RID: 859
        Connected,
        // Token: 0x0400035C RID: 860
        Disconnecting
    }

    // Token: 0x020000E3 RID: 227
    public abstract class GameLiftConnectionManagerParamsBase : IConnectionInitParams<GameLiftConnectionManager>
    {
        // Token: 0x0400035D RID: 861
        public Task<IAuthenticationTokenProvider> authenticationTokenProviderTask;

        // Token: 0x0400035E RID: 862
        public IGameLiftPlayerSessionProvider gameLiftPlayerSessionProvider;

        // Token: 0x0400035F RID: 863
        public BeatmapLevelSelectionMask selectionMask;

        // Token: 0x04000360 RID: 864
        public GameplayServerConfiguration configuration = new GameplayServerConfiguration(5, DiscoveryPolicy.Public, InvitePolicy.AnyoneCanInvite, GameplayServerMode.Countdown, SongSelectionMode.Vote, GameplayServerControlSettings.None);
    }

    // Token: 0x020000E4 RID: 228
    public class ConnectToServerParams : GameLiftConnectionManager.GameLiftConnectionManagerParamsBase
    {
        // Token: 0x04000361 RID: 865
        public string secret;

        // Token: 0x04000362 RID: 866
        public string code;
    }

    // Token: 0x020000E5 RID: 229
    public class StartClientParams : GameLiftConnectionManager.GameLiftConnectionManagerParamsBase
    {
    }
}