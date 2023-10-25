using System;
using BGNet.Core;

// Token: 0x02000028 RID: 40
public class GameplayServerFiniteStateMachine
{
    // Token: 0x1700003C RID: 60
    // (get) Token: 0x06000175 RID: 373 RVA: 0x0000698B File Offset: 0x00004B8B
    // (set) Token: 0x06000176 RID: 374 RVA: 0x00006993 File Offset: 0x00004B93
    public ITaskUtility taskUtility { get; private set; }

    // Token: 0x1700003D RID: 61
    // (get) Token: 0x06000177 RID: 375 RVA: 0x0000699C File Offset: 0x00004B9C
    // (set) Token: 0x06000178 RID: 376 RVA: 0x000069A4 File Offset: 0x00004BA4
    public IMultiplayerSessionManager multiplayerSessionManager { get; private set; }

    // Token: 0x1700003E RID: 62
    // (get) Token: 0x06000179 RID: 377 RVA: 0x000069AD File Offset: 0x00004BAD
    // (set) Token: 0x0600017A RID: 378 RVA: 0x000069B5 File Offset: 0x00004BB5
    public string ownerUserId { get; private set; }

    // Token: 0x1700003F RID: 63
    // (get) Token: 0x0600017B RID: 379 RVA: 0x000069BE File Offset: 0x00004BBE
    // (set) Token: 0x0600017C RID: 380 RVA: 0x000069C6 File Offset: 0x00004BC6
    public BeatmapLevelSelectionMask selectionMask { get; private set; }

    // Token: 0x17000040 RID: 64
    // (get) Token: 0x0600017D RID: 381 RVA: 0x000069CF File Offset: 0x00004BCF
    // (set) Token: 0x0600017E RID: 382 RVA: 0x000069D7 File Offset: 0x00004BD7
    public GameplayServerConfiguration configuration { get; private set; }

    // Token: 0x17000041 RID: 65
    // (get) Token: 0x0600017F RID: 383 RVA: 0x000069E0 File Offset: 0x00004BE0
    // (set) Token: 0x06000180 RID: 384 RVA: 0x000069E8 File Offset: 0x00004BE8
    public IServerBeatmapProvider beatmapProvider { get; private set; }

    // Token: 0x17000042 RID: 66
    // (get) Token: 0x06000181 RID: 385 RVA: 0x000069F1 File Offset: 0x00004BF1
    // (set) Token: 0x06000182 RID: 386 RVA: 0x000069F9 File Offset: 0x00004BF9
    public MenuRpcManager menuRpcManager { get; private set; }

    // Token: 0x17000043 RID: 67
    // (get) Token: 0x06000183 RID: 387 RVA: 0x00006A02 File Offset: 0x00004C02
    // (set) Token: 0x06000184 RID: 388 RVA: 0x00006A0A File Offset: 0x00004C0A
    public GameplayRpcManager gameplayRpcManager { get; private set; }

    // Token: 0x06000185 RID: 389 RVA: 0x00006A14 File Offset: 0x00004C14
    public GameplayServerFiniteStateMachine(GameplayServerFiniteStateMachine.InitParams initParams)
    {
        this.taskUtility = initParams.taskUtility;
        this.multiplayerSessionManager = initParams.multiplayerSessionManager;
        this.ownerUserId = initParams.creatorId;
        this.selectionMask = initParams.selectionMask;
        this.configuration = initParams.configuration;
        this.beatmapProvider = initParams.beatmapProvider;
        this.menuRpcManager = new MenuRpcManager(initParams.multiplayerSessionManager);
        this.gameplayRpcManager = new GameplayRpcManager(initParams.multiplayerSessionManager);
    }

    // Token: 0x040000F3 RID: 243
    protected GameState state;

    // Token: 0x040000F4 RID: 244
    protected bool enteringState;

    // Token: 0x020000F7 RID: 247
    public readonly struct InitParams
    {
        // Token: 0x060007AC RID: 1964 RVA: 0x0001483A File Offset: 0x00012A3A
        public InitParams(ITaskUtility taskUtility, IMultiplayerSessionManager multiplayerSessionManager, string creatorId, BeatmapLevelSelectionMask selectionMask, GameplayServerConfiguration configuration, IServerBeatmapProvider beatmapProvider)
        {
            this.taskUtility = taskUtility;
            this.multiplayerSessionManager = multiplayerSessionManager;
            this.creatorId = creatorId;
            this.selectionMask = selectionMask;
            this.configuration = configuration;
            this.beatmapProvider = beatmapProvider;
        }

        // Token: 0x0400037E RID: 894
        public readonly ITaskUtility taskUtility;

        // Token: 0x0400037F RID: 895
        public readonly IMultiplayerSessionManager multiplayerSessionManager;

        // Token: 0x04000380 RID: 896
        public readonly string creatorId;

        // Token: 0x04000381 RID: 897
        public readonly BeatmapLevelSelectionMask selectionMask;

        // Token: 0x04000382 RID: 898
        public readonly GameplayServerConfiguration configuration;

        // Token: 0x04000383 RID: 899
        public readonly IServerBeatmapProvider beatmapProvider;
    }
}
