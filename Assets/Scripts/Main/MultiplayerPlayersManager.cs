// Decompiled with JetBrains decompiler
// Type: MultiplayerPlayersManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerPlayersManager : MonoBehaviour, IMultiplayerLevelEndActionsPublisher
{
  [SerializeField]
  protected MultiplayerLocalActivePlayerFacade _activeLocalPlayerControllerPrefab;
  [SerializeField]
  protected MultiplayerLocalActivePlayerFacade _activeLocalPlayerDuelControllerPrefab;
  [SerializeField]
  protected MultiplayerLocalInactivePlayerFacade _inactiveLocalPlayerControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerFacade _connectedPlayerControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerFacade _connectedPlayerDuelControllerPrefab;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly BeatmapObjectSpawnCenter _beatmapObjectSpawnCenter;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [Inject]
  protected readonly FadeInOutController _fadeInOutController;
  [Inject]
  protected readonly DiContainer _container;
  [CompilerGenerated]
  protected bool m_CplayerSpawningFinished;
  protected MultiplayerLocalActivePlayerFacade _activeLocalPlayerFacade;
  protected MultiplayerLocalInactivePlayerFacade _inactiveLocalPlayerFacade;
  protected IMultiplayerLevelEndActionsPublisher _currentEventsPublisher;
  protected IStartSeekSongControllerProvider _currentStartSeekSongControllerProvider;
  protected MultiplayerLocalActivePlayerFacade.Factory _activeLocalPlayerFactory;
  protected MultiplayerLocalInactivePlayerFacade.Factory _inactiveLocalPlayerFactory;
  protected MultiplayerConnectedPlayerFacade.Factory _connectedPlayerFactory;
  protected readonly Dictionary<string, MultiplayerConnectedPlayerFacade> _connectedPlayerControllersMap = new Dictionary<string, MultiplayerConnectedPlayerFacade>();
  protected readonly Dictionary<string, float> _connectedPlayerCenterFacingRotationsMap = new Dictionary<string, float>();
  protected IReadOnlyList<IConnectedPlayer> _allActiveAtGameStartPlayers;

  public bool playerSpawningFinished
  {
    get => this.m_CplayerSpawningFinished;
    private set => this.m_CplayerSpawningFinished = value;
  }

  public IReadOnlyList<IConnectedPlayer> allActiveAtGameStartPlayers => this._allActiveAtGameStartPlayers;

  public IStartSeekSongControllerProvider localPlayerStartSeekSongController => this._currentStartSeekSongControllerProvider;

  public MultiplayerLocalActivePlayerFacade activeLocalPlayerFacade => this._activeLocalPlayerFacade;

  public MultiplayerLocalInactivePlayerFacade inactivePlayerFacade => this._inactiveLocalPlayerFacade;

  public Transform localPlayerTransform => !((UnityEngine.Object) this._inactiveLocalPlayerFacade != (UnityEngine.Object) null) ? this._activeLocalPlayerFacade.transform : this._inactiveLocalPlayerFacade.transform;

  public event System.Action playerSpawningDidFinishEvent;

  public event System.Action didSwitchPlayerToInactiveEvent;

  public event System.Action<MultiplayerLevelCompletionResults> playerDidFinishEvent;

  public event System.Action<MultiplayerLevelCompletionResults> playerNetworkDidFailedEvent;

  public virtual void SpawnPlayers(
    MultiplayerPlayerStartState localPlayerStartState,
    IReadOnlyList<IConnectedPlayer> allActiveAtGameStartPlayers)
  {
    this._connectedPlayerControllersMap.Clear();
    this._allActiveAtGameStartPlayers = allActiveAtGameStartPlayers;
    IConnectedPlayer localPlayer = this._multiplayerSessionManager.localPlayer;
    MultiplayerPlayerLayout layout = this._layoutProvider.CalculateLayout(this._allActiveAtGameStartPlayers.Count);
    float spawnCenterPosition = this._beatmapObjectSpawnCenter.CalculateSpawnCenterPosition(this._allActiveAtGameStartPlayers.Count);
    this.BindPlayerFactories(layout);
    if (localPlayer.WasActiveAtLevelStart() && localPlayerStartState == MultiplayerPlayerStartState.InSync)
    {
      this._activeLocalPlayerFacade = this._activeLocalPlayerFactory.Create(localPlayerStartState);
      this._currentEventsPublisher = (IMultiplayerLevelEndActionsPublisher) this._activeLocalPlayerFacade;
      this._currentStartSeekSongControllerProvider = (IStartSeekSongControllerProvider) this._activeLocalPlayerFacade;
    }
    else
    {
      this._inactiveLocalPlayerFacade = this._inactiveLocalPlayerFactory.Create(localPlayerStartState);
      this._currentEventsPublisher = (IMultiplayerLevelEndActionsPublisher) this._inactiveLocalPlayerFacade;
      this._currentStartSeekSongControllerProvider = (IStartSeekSongControllerProvider) this._inactiveLocalPlayerFacade;
      this._inactiveLocalPlayerFacade.spectatorController.SwitchToDefaultSpot();
      System.Action<MultiplayerLevelCompletionResults> playerDidFinishEvent = this.playerDidFinishEvent;
      if (playerDidFinishEvent != null)
        playerDidFinishEvent((MultiplayerLevelCompletionResults) null);
    }
    this._currentEventsPublisher.playerDidFinishEvent += new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
    this._currentEventsPublisher.playerNetworkDidFailedEvent += new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerNetworkDidFailed);
    int localPlayerIndex = this._allActiveAtGameStartPlayers.IndexOf<IConnectedPlayer>(localPlayer);
    if (!localPlayer.WasActiveAtLevelStart())
      localPlayerIndex = this._allActiveAtGameStartPlayers.Count;
    float withEvenAdjustment = MultiplayerPlayerPlacement.GetAngleBetweenPlayersWithEvenAdjustment(this._allActiveAtGameStartPlayers.Count, layout);
    for (int index = 0; index < this._allActiveAtGameStartPlayers.Count; ++index)
    {
      IConnectedPlayer atGameStartPlayer = this._allActiveAtGameStartPlayers[index];
      if (atGameStartPlayer.isMe)
      {
        this._connectedPlayerCenterFacingRotationsMap[atGameStartPlayer.userId] = 0.0f;
      }
      else
      {
        float positionAngleForPlayer = MultiplayerPlayerPlacement.GetOuterCirclePositionAngleForPlayer(index, localPlayerIndex, withEvenAdjustment);
        Vector3 playerWorldPosition = MultiplayerPlayerPlacement.GetPlayerWorldPosition(spawnCenterPosition, positionAngleForPlayer, layout);
        float y = positionAngleForPlayer;
        this._connectedPlayerCenterFacingRotationsMap[atGameStartPlayer.userId] = y;
        if (!atGameStartPlayer.IsFailed() && atGameStartPlayer.WasActiveAtLevelStart())
        {
          MultiplayerConnectedPlayerFacade connectedPlayerFacade = this._connectedPlayerFactory.Create(atGameStartPlayer, localPlayerStartState);
          connectedPlayerFacade.transform.position = playerWorldPosition;
          connectedPlayerFacade.transform.rotation = Quaternion.Euler(0.0f, y, 0.0f);
          this._connectedPlayerControllersMap[atGameStartPlayer.userId] = connectedPlayerFacade;
        }
      }
    }
    this.playerSpawningFinished = true;
    System.Action spawningDidFinishEvent = this.playerSpawningDidFinishEvent;
    if (spawningDidFinishEvent == null)
      return;
    spawningDidFinishEvent();
  }

  public virtual bool TryGetConnectedPlayerController(
    string userId,
    out MultiplayerConnectedPlayerFacade connectedPlayerController)
  {
    return this._connectedPlayerControllersMap.TryGetValue(userId, out connectedPlayerController);
  }

  public virtual bool TryGetConnectedCenterFacingRotation(
    string userId,
    out float centerFacingRotation)
  {
    return this._connectedPlayerCenterFacingRotationsMap.TryGetValue(userId, out centerFacingRotation);
  }

  public virtual void SwitchLocalPlayerToInactive() => this.StartCoroutine(this.SwitchLocalPlayerToInactiveCoroutine());

  public virtual void ReportLocalPlayerNetworkDidFailed(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    System.Action<MultiplayerLevelCompletionResults> networkDidFailedEvent = this.playerNetworkDidFailedEvent;
    if (networkDidFailedEvent == null)
      return;
    networkDidFailedEvent(levelCompletionResults);
  }

  public virtual IEnumerator SwitchLocalPlayerToInactiveCoroutine()
  {
    MultiplayerPlayersManager multiplayerPlayersManager = this;
    multiplayerPlayersManager._fadeInOutController.FadeOut(1.3f);
    yield return (object) new WaitForSeconds(1.3f);
    if (multiplayerPlayersManager._currentEventsPublisher != null)
    {
      multiplayerPlayersManager._currentEventsPublisher.playerDidFinishEvent -= new System.Action<MultiplayerLevelCompletionResults>(multiplayerPlayersManager.HandlePlayerDidFinish);
      multiplayerPlayersManager._currentEventsPublisher.playerNetworkDidFailedEvent -= new System.Action<MultiplayerLevelCompletionResults>(multiplayerPlayersManager.HandlePlayerNetworkDidFailed);
    }
    multiplayerPlayersManager._activeLocalPlayerFacade.DisablePlayer();
    multiplayerPlayersManager._inactiveLocalPlayerFacade = multiplayerPlayersManager._inactiveLocalPlayerFactory.Create(MultiplayerPlayerStartState.Late);
    multiplayerPlayersManager._currentEventsPublisher = (IMultiplayerLevelEndActionsPublisher) multiplayerPlayersManager._inactiveLocalPlayerFacade;
    multiplayerPlayersManager._currentStartSeekSongControllerProvider = (IStartSeekSongControllerProvider) multiplayerPlayersManager._inactiveLocalPlayerFacade;
    multiplayerPlayersManager._inactiveLocalPlayerFacade.spectatorController.SwitchToDefaultSpot();
    multiplayerPlayersManager._currentEventsPublisher.playerDidFinishEvent += new System.Action<MultiplayerLevelCompletionResults>(multiplayerPlayersManager.HandlePlayerDidFinish);
    multiplayerPlayersManager._currentEventsPublisher.playerNetworkDidFailedEvent += new System.Action<MultiplayerLevelCompletionResults>(multiplayerPlayersManager.HandlePlayerNetworkDidFailed);
    System.Action playerToInactiveEvent = multiplayerPlayersManager.didSwitchPlayerToInactiveEvent;
    if (playerToInactiveEvent != null)
      playerToInactiveEvent();
    multiplayerPlayersManager._fadeInOutController.FadeIn(1.3f);
    multiplayerPlayersManager._inactiveLocalPlayerFacade.introPlayableDirector.Play();
  }

  public virtual void BindPlayerFactories(MultiplayerPlayerLayout layout)
  {
    this._container.BindFactory<MultiplayerPlayerStartState, MultiplayerLocalInactivePlayerFacade, MultiplayerLocalInactivePlayerFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab<MultiplayerLocalPlayerInstaller>((UnityEngine.Object) this._inactiveLocalPlayerControllerPrefab);
    if (layout == MultiplayerPlayerLayout.Duel)
    {
      this._container.BindFactory<MultiplayerPlayerStartState, MultiplayerLocalActivePlayerFacade, MultiplayerLocalActivePlayerFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab<MultiplayerLocalPlayerInstaller>((UnityEngine.Object) this._activeLocalPlayerDuelControllerPrefab);
      this._container.BindFactory<IConnectedPlayer, MultiplayerPlayerStartState, MultiplayerConnectedPlayerFacade, MultiplayerConnectedPlayerFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab<MultiplayerConnectedPlayerInstaller>((UnityEngine.Object) this._connectedPlayerDuelControllerPrefab);
    }
    else
    {
      this._container.BindFactory<MultiplayerPlayerStartState, MultiplayerLocalActivePlayerFacade, MultiplayerLocalActivePlayerFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab<MultiplayerLocalPlayerInstaller>((UnityEngine.Object) this._activeLocalPlayerControllerPrefab);
      this._container.BindFactory<IConnectedPlayer, MultiplayerPlayerStartState, MultiplayerConnectedPlayerFacade, MultiplayerConnectedPlayerFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab<MultiplayerConnectedPlayerInstaller>((UnityEngine.Object) this._connectedPlayerControllerPrefab);
    }
    this._activeLocalPlayerFactory = this._container.Resolve<MultiplayerLocalActivePlayerFacade.Factory>();
    this._inactiveLocalPlayerFactory = this._container.Resolve<MultiplayerLocalInactivePlayerFacade.Factory>();
    this._connectedPlayerFactory = this._container.Resolve<MultiplayerConnectedPlayerFacade.Factory>();
  }

  public virtual void HandlePlayerDidFinish(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    System.Action<MultiplayerLevelCompletionResults> playerDidFinishEvent = this.playerDidFinishEvent;
    if (playerDidFinishEvent == null)
      return;
    playerDidFinishEvent(levelCompletionResults);
  }

  public virtual void HandlePlayerNetworkDidFailed(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    this.ReportLocalPlayerNetworkDidFailed(levelCompletionResults);
  }
}
