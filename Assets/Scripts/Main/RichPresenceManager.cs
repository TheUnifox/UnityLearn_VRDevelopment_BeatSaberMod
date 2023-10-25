// Decompiled with JetBrains decompiler
// Type: RichPresenceManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class RichPresenceManager : MonoBehaviour
{
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;
  [SerializeField]
  protected ScenesTransitionSetupDataSO _tutorialScenesTransitionSetupData;
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelScenesTransitionSetupData;
  [SerializeField]
  protected MultiplayerLevelScenesTransitionSetupDataSO _multiplayerLevelScenesTransitionSetupData;
  [Inject]
  protected readonly MenuScenesTransitionSetupDataSO _menuScenesTransitionSetupData;
  [Inject]
  protected readonly IRichPresencePlatformHandler _richPresencePlatformHandler;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly LobbyGameStateModel _lobbyGameStateModel;
  [Inject]
  protected readonly IUnifiedNetworkPlayerModel _unifiedNetworkPlayerModel;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;
  protected bool _menuWasLoaded;
  protected bool _isInMultiplayerLobby;
  protected BrowsingMenusRichPresenceData _browsingMenusRichPresenceData;
  protected InMultiplayerRichPresenceData _inMultiplayerRichPresenceData;
  protected PlayingCampaignRichPresenceData _playingCampaignRichPresenceData;
  protected PlayingTutorialPresenceData _playingTutorialPresenceData;
  protected IRichPresenceData _currentPresenceData;

  public virtual void Awake()
  {
    this._gameScenesManager.transitionDidFinishEvent += new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleGameScenesManagerTransitionDidFinish);
    this._lobbyGameStateModel.gameStateDidChangeAlwaysSentEvent += new System.Action<MultiplayerGameState>(this.HandleLobbyGameStateModelDidChange);
    this._playingCampaignRichPresenceData = new PlayingCampaignRichPresenceData();
    this._playingTutorialPresenceData = new PlayingTutorialPresenceData();
    this._browsingMenusRichPresenceData = new BrowsingMenusRichPresenceData();
    this._unifiedNetworkPlayerModel.partySizeChangedEvent += new System.Action<int>(this.HandleMultiplayerPartySizeChanged);
    this._lobbyPlayerPermissionsModel.permissionsChangedEvent += new System.Action(this.HandleLobbyPlayerPermissionChanged);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._gameScenesManager != (UnityEngine.Object) null)
      this._gameScenesManager.transitionDidFinishEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleGameScenesManagerTransitionDidFinish);
    if (this._lobbyGameStateModel != null)
      this._lobbyGameStateModel.gameStateDidChangeAlwaysSentEvent -= new System.Action<MultiplayerGameState>(this.HandleLobbyGameStateModelDidChange);
    if (this._unifiedNetworkPlayerModel != null)
      this._unifiedNetworkPlayerModel.partySizeChangedEvent -= new System.Action<int>(this.HandleMultiplayerPartySizeChanged);
    if (this._lobbyPlayerPermissionsModel == null)
      return;
    this._lobbyPlayerPermissionsModel.permissionsChangedEvent -= new System.Action(this.HandleLobbyPlayerPermissionChanged);
  }

  public virtual void HandleLobbyGameStateModelDidChange(MultiplayerGameState newGameState)
  {
    this._isInMultiplayerLobby = newGameState != 0;
    if (newGameState == MultiplayerGameState.Game)
      return;
    this.SetMenuPresence();
  }

  public virtual void SetMenuPresence()
  {
    if (this._isInMultiplayerLobby)
      this.SetPresence((IRichPresenceData) new InMultiplayerRichPresenceData(this._unifiedNetworkPlayerModel.secret, this._lobbyPlayerPermissionsModel.hasInvitePermission, this._unifiedNetworkPlayerModel.currentPartySize >= this._unifiedNetworkPlayerModel.configuration.maxPlayerCount));
    else
      this.SetPresence((IRichPresenceData) this._browsingMenusRichPresenceData);
  }

  public virtual void HandleGameScenesManagerTransitionDidFinish(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    DiContainer diContainer)
  {
    if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._menuScenesTransitionSetupData)
    {
      this.SetMenuPresence();
      this._menuWasLoaded = true;
    }
    else if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._standardLevelScenesTransitionSetupData)
      this.SetPresence((IRichPresenceData) new PlayingDifficultyBeatmapRichPresenceData(this._standardLevelScenesTransitionSetupData.difficultyBeatmap));
    else if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._multiplayerLevelScenesTransitionSetupData)
      this.SetPresence((IRichPresenceData) new PlayingMultiplayerRichPresenceData(this._multiplayerLevelScenesTransitionSetupData.difficultyBeatmap, this._unifiedNetworkPlayerModel.currentPartySize >= this._unifiedNetworkPlayerModel.configuration.maxPlayerCount));
    else if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._missionLevelScenesTransitionSetupData)
      this.SetPresence((IRichPresenceData) this._playingCampaignRichPresenceData);
    else if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._tutorialScenesTransitionSetupData)
      this.SetPresence((IRichPresenceData) this._playingTutorialPresenceData);
    else if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) null && this._menuWasLoaded)
      this.SetMenuPresence();
    else
      this.Clear();
  }

  public virtual void HandleMultiplayerPartySizeChanged(int currentPartySize)
  {
    bool flag = currentPartySize >= this._unifiedNetworkPlayerModel.configuration.maxPlayerCount;
    if (!(this._currentPresenceData is IMultiplayerRichPresenceData currentPresenceData) || currentPresenceData.atMaxPartySize == flag)
      return;
    currentPresenceData.atMaxPartySize = flag;
    this.SetPresence((IRichPresenceData) currentPresenceData);
  }

  public virtual void HandleLobbyPlayerPermissionChanged()
  {
    bool invitePermission = this._lobbyPlayerPermissionsModel.hasInvitePermission;
    if (!(this._currentPresenceData is IMultiplayerRichPresenceData currentPresenceData) || currentPresenceData.canInvite == invitePermission)
      return;
    currentPresenceData.canInvite = invitePermission;
    this.SetPresence((IRichPresenceData) currentPresenceData);
  }

  public virtual void SetPresence(IRichPresenceData presenceData)
  {
    this._currentPresenceData = presenceData;
    this._richPresencePlatformHandler.SetPresence(presenceData);
  }

  public virtual void Clear()
  {
    this._currentPresenceData = (IRichPresenceData) null;
    this._richPresencePlatformHandler.Clear();
  }
}
