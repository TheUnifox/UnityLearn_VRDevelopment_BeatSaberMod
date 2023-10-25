// Decompiled with JetBrains decompiler
// Type: ServerPlayerListViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ServerPlayerListViewController : ViewController
{
  [SerializeField]
  protected GameServerPlayersTableView _gameServerPlayersTableView;
  [Header("Button")]
  [SerializeField]
  protected Button _invitePlayerButton;
  [Space]
  [SerializeField]
  protected HoverHint _cantInvitePlayerHoverHint;
  [Inject]
  protected readonly IInvitePlatformHandler _invitePlatformHandler;
  [Inject]
  protected readonly ILobbyPlayersDataModel _lobbyPlayersDataModel;
  [Inject]
  protected readonly ILobbyStateDataModel _lobbyStateDataModel;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;
  [Inject]
  protected readonly ILobbyGameStateController _lobbyGameStateController;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();

  public event System.Action<PreviewDifficultyBeatmap> selectSuggestedBeatmapEvent;

  public event System.Action<GameplayModifiers> selectSuggestedGameplayModifiersEvent;

  public event System.Action<string> kickPlayerEvent;

  public event System.Action didOpenInvitePanelEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._lobbyPlayersDataModel.didChangeEvent += new System.Action<string>(this.HandleLobbyPlayersDataDidChange);
    this._lobbyPlayerPermissionsModel.permissionsChangedEvent += new System.Action(this.HandleLobbyPlayerPermissionChanged);
    this._lobbyGameStateController.lobbyStateChangedEvent += new System.Action<MultiplayerLobbyState>(this.HandleLobbyGameStateControllerLobbyStateChanged);
    this._gameServerPlayersTableView.selectSuggestedLevelEvent += new System.Action<PreviewDifficultyBeatmap>(this.HandleSelectSuggestedLevel);
    this._gameServerPlayersTableView.selectSuggestedGameplayModifiersEvent += new System.Action<GameplayModifiers>(this.HandleSelectSuggestedGameplayModifiers);
    this._gameServerPlayersTableView.kickPlayerEvent += new System.Action<string>(this.HandleKickPlayer);
    this._buttonBinder.AddBinding(this._invitePlayerButton, new System.Action(this.HandleOpenPlatformInvitePanel));
    this.TrySetInviteButtonEnabled();
    this.SetDataToTable();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._lobbyPlayersDataModel.didChangeEvent -= new System.Action<string>(this.HandleLobbyPlayersDataDidChange);
    this._lobbyPlayerPermissionsModel.permissionsChangedEvent -= new System.Action(this.HandleLobbyPlayerPermissionChanged);
    this._lobbyGameStateController.lobbyStateChangedEvent -= new System.Action<MultiplayerLobbyState>(this.HandleLobbyGameStateControllerLobbyStateChanged);
    this._gameServerPlayersTableView.selectSuggestedLevelEvent -= new System.Action<PreviewDifficultyBeatmap>(this.HandleSelectSuggestedLevel);
    this._gameServerPlayersTableView.selectSuggestedGameplayModifiersEvent -= new System.Action<GameplayModifiers>(this.HandleSelectSuggestedGameplayModifiers);
    this._gameServerPlayersTableView.kickPlayerEvent -= new System.Action<string>(this.HandleKickPlayer);
    this._buttonBinder.ClearBindings();
  }

  public virtual void HandleLobbyPlayersDataDidChange(string userId) => this.SetDataToTable();

  public virtual void HandleLobbyGameStateControllerLobbyStateChanged(MultiplayerLobbyState _) => this.SetDataToTable();

  public virtual void HandleLobbyPlayerPermissionChanged() => this.SetDataToTable();

  public virtual void SetDataToTable()
  {
    this._gameServerPlayersTableView.SetData(this._lobbyStateDataModel.connectedPlayers, this._lobbyPlayersDataModel, this._lobbyPlayerPermissionsModel.hasKickVotePermission, this._lobbyGameStateController.state == MultiplayerLobbyState.LobbySetup || this._lobbyGameStateController.state == MultiplayerLobbyState.LobbyCountdown, this._lobbyPlayerPermissionsModel.hasRecommendBeatmapPermission, this._lobbyPlayerPermissionsModel.hasRecommendModifiersPermission);
    this.TrySetInviteButtonEnabled();
  }

  public virtual void HandleSelectSuggestedLevel(PreviewDifficultyBeatmap beatmapLevel)
  {
    System.Action<PreviewDifficultyBeatmap> suggestedBeatmapEvent = this.selectSuggestedBeatmapEvent;
    if (suggestedBeatmapEvent == null)
      return;
    suggestedBeatmapEvent(beatmapLevel);
  }

  public virtual void HandleSelectSuggestedGameplayModifiers(GameplayModifiers gameplayModifiers)
  {
    System.Action<GameplayModifiers> gameplayModifiersEvent = this.selectSuggestedGameplayModifiersEvent;
    if (gameplayModifiersEvent == null)
      return;
    gameplayModifiersEvent(gameplayModifiers);
  }

  public virtual void HandleKickPlayer(string userId)
  {
    System.Action<string> kickPlayerEvent = this.kickPlayerEvent;
    if (kickPlayerEvent == null)
      return;
    kickPlayerEvent(userId);
  }

  public virtual void HandleOpenPlatformInvitePanel()
  {
    this._invitePlatformHandler.OpenInvitePanel();
    System.Action invitePanelEvent = this.didOpenInvitePanelEvent;
    if (invitePanelEvent == null)
      return;
    invitePanelEvent();
  }

  public virtual void TrySetInviteButtonEnabled()
  {
    if (!this._invitePlatformHandler.isSupported || this._lobbyStateDataModel.configuration.invitePolicy == InvitePolicy.NobodyCanInvite)
    {
      this._invitePlayerButton.gameObject.SetActive(false);
    }
    else
    {
      this._invitePlayerButton.gameObject.SetActive(true);
      bool flag = true;
      if (!this._lobbyPlayerPermissionsModel.hasInvitePermission)
      {
        this._cantInvitePlayerHoverHint.text = Localization.Get("LABEL_CANT_INVITE_PLAYERS_NOT_LOBBY_OWNER");
        flag = false;
      }
      else if (this._lobbyStateDataModel.connectedPlayers.Count >= this._lobbyStateDataModel.configuration.maxPlayerCount)
      {
        this._cantInvitePlayerHoverHint.text = Localization.Get("LABEL_CANT_INVITE_PLAYERS_AT_MAX_SIZE");
        flag = false;
      }
      this._invitePlayerButton.interactable = flag;
      this._cantInvitePlayerHoverHint.enabled = !flag;
    }
  }
}
