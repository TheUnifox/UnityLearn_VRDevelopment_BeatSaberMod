// Decompiled with JetBrains decompiler
// Type: MockPlayer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MockPlayer : IConnectedPlayer, INetworkPlayer
{
  [CompilerGenerated]
  protected readonly bool m_CisMe;
  [CompilerGenerated]
  protected bool m_CisConnected;
  [CompilerGenerated]
  protected MultiplayerAvatarData m_CmultiplayerAvatarData;
  [CompilerGenerated]
  protected bool m_CisConnectionOwner;
  [CompilerGenerated]
  protected bool m_CisKicked;
  [CompilerGenerated]
  protected int m_CcurrentPartySize;
  [CompilerGenerated]
  protected BeatmapLevelSelectionMask m_CselectionMask;
  [CompilerGenerated]
  protected GameplayServerConfiguration m_Cconfiguration;
  [CompilerGenerated]
  protected bool m_CisMyPartyOwner;
  [CompilerGenerated]
  protected bool m_CrequiresPassword;
  [CompilerGenerated]
  protected bool m_CisWaitingOnJoin;
  [CompilerGenerated]
  protected bool m_CcanInvite;
  [CompilerGenerated]
  protected bool m_CisWaitingOnInvite;
  [CompilerGenerated]
  protected bool m_CcanKick;
  [CompilerGenerated]
  protected bool m_CcanLeave;
  [CompilerGenerated]
  protected bool m_CcanBlock;
  [CompilerGenerated]
  protected bool m_CcanUnblock;
  protected bool _isReady;
  protected readonly HashSet<string> _playerState = new HashSet<string>();
  protected readonly MockPlayerSettings _settings;
  protected MockPlayerFiniteStateMachine _fsm;
  protected IGameplayRpcManager _gameplayRpcManager;

  public bool isReady
  {
    get => this._isReady;
    set
    {
      this._fsm?.SetIsReady(value);
      this._isReady = value;
    }
  }

  public bool isMe => this.m_CisMe;

  public string userId => this._settings.userId;

  public string userName => this._settings.userName;

  public int sortIndex => this._settings.sortIndex;

  public bool autoConnect => this._settings.autoConnect;

  public bool inactiveByDefault => this._settings.inactiveByDefault;

  public bool isConnected
  {
    get => this.m_CisConnected;
    protected set => this.m_CisConnected = value;
  }

  public MultiplayerAvatarData multiplayerAvatarData
  {
    get => this.m_CmultiplayerAvatarData;
    private set => this.m_CmultiplayerAvatarData = value;
  }

  public bool isConnectionOwner
  {
    get => this.m_CisConnectionOwner;
    set => this.m_CisConnectionOwner = value;
  }

  public DisconnectedReason disconnectedReason => DisconnectedReason.UserInitiated;

  public float offsetSyncTime => (float) ((double) Time.time - (double) this.currentLatency - (this.isMe ? 0.0 : 0.059999998658895493));

  public bool hasValidLatency => true;

  public float currentLatency => this.isMe ? 0.0f : this._settings.latency * (1f / 1000f);

  public bool isKicked
  {
    get => this.m_CisKicked;
    set => this.m_CisKicked = value;
  }

  public int currentPartySize
  {
    get => this.m_CcurrentPartySize;
    set => this.m_CcurrentPartySize = value;
  }

  public BeatmapLevelSelectionMask selectionMask
  {
    get => this.m_CselectionMask;
    set => this.m_CselectionMask = value;
  }

  public GameplayServerConfiguration configuration
  {
    get => this.m_Cconfiguration;
    set => this.m_Cconfiguration = value;
  }

  public bool isMyPartyOwner
  {
    get => this.m_CisMyPartyOwner;
    set => this.m_CisMyPartyOwner = value;
  }

  public IConnectedPlayer connectedPlayer => (IConnectedPlayer) this;

  public virtual bool canJoin => false;

  public bool requiresPassword
  {
    get => this.m_CrequiresPassword;
    set => this.m_CrequiresPassword = value;
  }

  public bool isWaitingOnJoin
  {
    get => this.m_CisWaitingOnJoin;
    set => this.m_CisWaitingOnJoin = value;
  }

  public bool canInvite
  {
    get => this.m_CcanInvite;
    set => this.m_CcanInvite = value;
  }

  public bool isWaitingOnInvite
  {
    get => this.m_CisWaitingOnInvite;
    set => this.m_CisWaitingOnInvite = value;
  }

  public bool canKick
  {
    get => this.m_CcanKick;
    set => this.m_CcanKick = value;
  }

  public bool canLeave
  {
    get => this.m_CcanLeave;
    set => this.m_CcanLeave = value;
  }

  public bool canBlock
  {
    get => this.m_CcanBlock;
    set => this.m_CcanBlock = value;
  }

  public bool canUnblock
  {
    get => this.m_CcanUnblock;
    set => this.m_CcanUnblock = value;
  }

  public bool isPlayer
  {
    get => this.HasState("player");
    set => this.SetState("player", value);
  }

  public bool isDedicatedServer
  {
    get => this.HasState("dedicated_server");
    set => this.SetState("dedicated_server", value);
  }

  public bool wantsToPlayNextLevel
  {
    get => this.HasState("wants_to_play_next_level");
    set => this.SetState("wants_to_play_next_level", value);
  }

  public bool wasActiveAtLevelStart
  {
    get => this.HasState("was_active_at_level_start");
    set => this.SetState("was_active_at_level_start", value);
  }

  public bool isActive
  {
    get => this.HasState("is_active");
    set => this.SetState("is_active", value);
  }

  public bool finishedLevel
  {
    get => this.HasState("finished_level");
    set => this.SetState("finished_level", value);
  }

  public bool isTerminating
  {
    get => this.HasState("terminating");
    set => this.SetState("terminating", value);
  }

  public MockPlayer(MockPlayerSettings settings, bool isLocalPlayer)
  {
    this._settings = settings;
    this.m_CisMe = isLocalPlayer;
    this.isPlayer = true;
    this.isConnected = true;
    this.wantsToPlayNextLevel = true;
  }

  public virtual void Tick() => this._fsm?.Tick();

  public virtual bool SetState(string state, bool value) => !value ? this._playerState.Remove(state) : this._playerState.Add(state);

  public virtual bool HasState(string state) => this._playerState.Contains(state);

  public virtual void Connect(
    IMultiplayerSessionManager multiplayerSessionManager,
    AvatarPartsModel avatarPartsModel,
    BeatmapLevelsModel beatmapLevelsModel,
    NodePoseSyncStateManager nodePoseSyncStateManager)
  {
    this.Disconnect();
    this.isConnected = true;
    if (this.isMe)
      return;
    this.multiplayerAvatarData = new MultiplayerAvatarData(avatarPartsModel.headTopCollection.GetRandom().name, (Color32) UnityEngine.Random.ColorHSV(), (Color32) UnityEngine.Random.ColorHSV(), avatarPartsModel.glassesCollection.GetDefault().name, (Color32) UnityEngine.Random.ColorHSV(), avatarPartsModel.facialHairCollection.GetDefault().name, (Color32) UnityEngine.Random.ColorHSV(), avatarPartsModel.handsCollection.GetRandom().name, (Color32) UnityEngine.Random.ColorHSV(), avatarPartsModel.clothesCollection.GetRandom().name, (Color32) UnityEngine.Random.ColorHSV(), (Color32) UnityEngine.Random.ColorHSV(), (Color32) UnityEngine.Random.ColorHSV(), avatarPartsModel.GetRandomColor().id, avatarPartsModel.eyesCollection.GetRandom().name, "");
    MenuRpcManager menuRpcManager = new MenuRpcManager(multiplayerSessionManager);
    GameplayRpcManager gameplayRpcManager = new GameplayRpcManager(multiplayerSessionManager);
    this._gameplayRpcManager = (IGameplayRpcManager) gameplayRpcManager;
    MockPlayerLobbyPoseGenerator lobbyPoseGenerator;
    MockPlayerGamePoseGenerator gamePoseGenerator;
    switch (this._settings.movementType)
    {
      case MockPlayerMovementType.AI:
        lobbyPoseGenerator = (MockPlayerLobbyPoseGenerator) new MockPlayerLobbyPoseGeneratorAI(multiplayerSessionManager);
        gamePoseGenerator = (MockPlayerGamePoseGenerator) new MockPlayerGamePoseGeneratorAI(multiplayerSessionManager, (IGameplayRpcManager) gameplayRpcManager, (IMockPlayerScoreCalculator) new DeterministicHitChanceScoreCalculator(this._settings.aiCubeHitChance), this._settings.leftHanded);
        break;
      case MockPlayerMovementType.MirrorPlayer:
        lobbyPoseGenerator = (MockPlayerLobbyPoseGenerator) new MockPlayerLobbyPoseGeneratorMirror(multiplayerSessionManager, nodePoseSyncStateManager);
        gamePoseGenerator = (MockPlayerGamePoseGenerator) new MockPlayerGamePoseGeneratorMirror(multiplayerSessionManager, (IGameplayRpcManager) gameplayRpcManager, this._settings.leftHanded, nodePoseSyncStateManager);
        break;
      case MockPlayerMovementType.Recording:
        lobbyPoseGenerator = (MockPlayerLobbyPoseGenerator) new MockPlayerLobbyPoseGeneratorRecording(multiplayerSessionManager);
        gamePoseGenerator = (MockPlayerGamePoseGenerator) new MockPlayerGamePoseGeneratorAI(multiplayerSessionManager, (IGameplayRpcManager) gameplayRpcManager, (IMockPlayerScoreCalculator) new DeterministicHitChanceScoreCalculator(this._settings.aiCubeHitChance), this._settings.leftHanded);
        break;
      default:
        Debug.LogError((object) "Not implemented Mock Player movement generator type, using AI");
        lobbyPoseGenerator = (MockPlayerLobbyPoseGenerator) new MockPlayerLobbyPoseGeneratorAI(multiplayerSessionManager);
        gamePoseGenerator = (MockPlayerGamePoseGenerator) new MockPlayerGamePoseGeneratorAI(multiplayerSessionManager, (IGameplayRpcManager) gameplayRpcManager, (IMockPlayerScoreCalculator) new DeterministicHitChanceScoreCalculator(this._settings.aiCubeHitChance), this._settings.leftHanded);
        break;
    }
    this._fsm = new MockPlayerFiniteStateMachine(multiplayerSessionManager, (IGameplayRpcManager) gameplayRpcManager, (IMenuRpcManager) menuRpcManager, (IMockBeatmapDataProvider) new MockBeatmapLoader(beatmapLevelsModel), lobbyPoseGenerator, gamePoseGenerator);
    this._fsm.leftHanded = this._settings.leftHanded;
    this._fsm.saberAColor = this._settings.saberAColor;
    this._fsm.saberBColor = this._settings.saberBColor;
    this._fsm.obstaclesColor = this._settings.obstaclesColor;
    this._fsm.inactiveByDefault = this._settings.inactiveByDefault;
    this._fsm.SetIsReady(this.isReady);
  }

  public virtual void Disconnect()
  {
    this._fsm?.Dispose();
    this._fsm = (MockPlayerFiniteStateMachine) null;
    this.isConnected = false;
  }

  public virtual void Unblock() => throw new NotImplementedException();

  public virtual void SendJoinResponse(bool accept) => throw new NotImplementedException();

  public virtual void SendInviteResponse(bool accept) => throw new NotImplementedException();

  public virtual void Block() => throw new NotImplementedException();

  public virtual void Leave()
  {
  }

  public virtual void Kick() => throw new NotImplementedException();

  public virtual void Invite() => throw new NotImplementedException();

  public virtual void Join(string password) => throw new NotImplementedException();

  public virtual void Join() => throw new NotImplementedException();

  public virtual void SimulateFail()
  {
    this.isActive = false;
    this._fsm.gamePoseGenerator?.SimulateFail();
  }

  public virtual void SimulateGiveUp()
  {
    this.isActive = false;
    this._fsm.gamePoseGenerator?.SimulateGiveUp();
  }

  public virtual void SimulateReturnToMainMenu() => this._gameplayRpcManager.RequestReturnToMenu();
}
