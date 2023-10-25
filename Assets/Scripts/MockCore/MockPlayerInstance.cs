using System;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;

// Token: 0x0200000D RID: 13
public class MockPlayerInstance : IStandaloneMonobehavior, IDisposable
{
    // Token: 0x1700001B RID: 27
    // (get) Token: 0x06000059 RID: 89 RVA: 0x000037CD File Offset: 0x000019CD
    public string id
    {
        get
        {
            return this._id;
        }
    }

    // Token: 0x1700001C RID: 28
    // (get) Token: 0x0600005A RID: 90 RVA: 0x000037D5 File Offset: 0x000019D5
    public string userId
    {
        get
        {
            return this._userId;
        }
    }

    // Token: 0x1700001D RID: 29
    // (get) Token: 0x0600005B RID: 91 RVA: 0x000037DD File Offset: 0x000019DD
    public string userName
    {
        get
        {
            return this._userName;
        }
    }

    // Token: 0x0600005C RID: 92 RVA: 0x000037E8 File Offset: 0x000019E8
    public MockPlayerInstance(ITimeProvider timeProvider, ITaskUtility taskUtility, IMockBeatmapDataProvider beatmapDataProvider, IConnectionManager connectionManager)
    {
        this._timeProvider = timeProvider;
        this._taskUtility = taskUtility;
        this._id = Guid.NewGuid().ToString();
        this._userId = NetworkUtility.GetHashedUserId(this._id, AuthenticationToken.Platform.Test);
        this._userName = "Mock Player " + this._id.Substring(4);
        this._multiplayerSessionManager = StandaloneMonobehavior.Create<MultiplayerSessionManager>();
        this._connectedPlayerManager = new ConnectedPlayerManager(this._timeProvider, this._taskUtility, connectionManager);
        this._multiplayerSessionManager.connectionFailedEvent += delegate (ConnectionFailedReason r)
        {
            this.Stop();
        };
        this._multiplayerSessionManager.disconnectedEvent += delegate (DisconnectedReason r)
        {
            this.Stop();
        };
        MenuRpcManager menuRpcManager = new MenuRpcManager(this._multiplayerSessionManager);
        GameplayRpcManager gameplayRpcManager = new GameplayRpcManager(this._multiplayerSessionManager);
        MockPlayerLobbyPoseGeneratorAI lobbyPoseGenerator = new MockPlayerLobbyPoseGeneratorAI(this._multiplayerSessionManager);
        MockPlayerGamePoseGeneratorAI gamePoseGenerator = new MockPlayerGamePoseGeneratorAI(this._multiplayerSessionManager, gameplayRpcManager, new BasicMockPlayerScoreCalculator(0.95f, 66, 110), false);
        this._fsm = new MockPlayerFiniteStateMachine(this._multiplayerSessionManager, gameplayRpcManager, menuRpcManager, beatmapDataProvider, lobbyPoseGenerator, gamePoseGenerator);
    }

    // Token: 0x0600005D RID: 93 RVA: 0x00003904 File Offset: 0x00001B04
    public void Tick()
    {
        this._fsm.Tick();
    }

    // Token: 0x0600005E RID: 94 RVA: 0x00003914 File Offset: 0x00001B14
    public void ConnectToServer<T>(IConnectionInitParams<T> connectionInitParams) where T : class, IConnectionManager
    {
        this._multiplayerSessionManager.Dispatch(delegate
        {
            this._multiplayerSessionManager.StartSession(MultiplayerSessionManager.SessionType.Player, this._connectedPlayerManager);
            this._connectedPlayerManager.GetConnectionManager<T>().Init<T>(connectionInitParams);
            this._fsm.SetIsReady(true);
        });
    }

    // Token: 0x0600005F RID: 95 RVA: 0x0000394C File Offset: 0x00001B4C
    public async Task RunAsync(IStandaloneThreadRunner runner, CancellationToken token)
    {
        token.Register(new Action(this.Stop));
        await this._multiplayerSessionManager.RunAsync(runner, this._cancellationTokenSource.Token);
    }

    // Token: 0x06000060 RID: 96 RVA: 0x000039A1 File Offset: 0x00001BA1
    public void Dispatch(Action action)
    {
        this._multiplayerSessionManager.Dispatch(action);
    }

    // Token: 0x06000061 RID: 97 RVA: 0x000039AF File Offset: 0x00001BAF
    public Task DispatchAsync(Func<Task> action)
    {
        return this._multiplayerSessionManager.DispatchAsync(action);
    }

    // Token: 0x06000062 RID: 98 RVA: 0x000039C0 File Offset: 0x00001BC0
    public async void Stop()
    {
        await this.DisposeAsync();
    }

    // Token: 0x06000063 RID: 99 RVA: 0x000039FC File Offset: 0x00001BFC
    private async Task DisposeAsync()
    {
        if (!this._cancellationTokenSource.IsCancellationRequested)
        {
            await this._multiplayerSessionManager.DispatchAsync(delegate
            {
                this._multiplayerSessionManager.Disconnect();
                this._fsm.Dispose();
                this._connectedPlayerManager.Dispose();
                this._cancellationTokenSource.Cancel();
                return Task.CompletedTask;
            });
        }
    }

    // Token: 0x06000064 RID: 100 RVA: 0x00003A41 File Offset: 0x00001C41
    public void Dispose()
    {
        this._taskUtility.Wait(this.DisposeAsync());
    }

    // Token: 0x0400003F RID: 63
    private readonly string _id;

    // Token: 0x04000040 RID: 64
    private readonly string _userId;

    // Token: 0x04000041 RID: 65
    private readonly string _userName;

    // Token: 0x04000042 RID: 66
    private readonly ITimeProvider _timeProvider;

    // Token: 0x04000043 RID: 67
    private readonly ITaskUtility _taskUtility;

    // Token: 0x04000044 RID: 68
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    // Token: 0x04000045 RID: 69
    private readonly MultiplayerSessionManager _multiplayerSessionManager;

    // Token: 0x04000046 RID: 70
    private readonly ConnectedPlayerManager _connectedPlayerManager;

    // Token: 0x04000047 RID: 71
    private readonly MockPlayerFiniteStateMachine _fsm;
}
