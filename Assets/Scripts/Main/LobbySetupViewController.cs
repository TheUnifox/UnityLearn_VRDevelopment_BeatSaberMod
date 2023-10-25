// Decompiled with JetBrains decompiler
// Type: LobbySetupViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LobbySetupViewController : ViewController
{
  [LocalizationKey]
  protected const string kStartTextKey = "LOBBY_START_GAME";
  [LocalizationKey]
  protected const string kCancelTextKey = "BUTTON_CANCEL";
  [LocalizationKey]
  protected const string kReadyTextKey = "LOBBY_READY";
  [LocalizationKey]
  protected const string kUnreadyTextKey = "BUTTON_UNREADY";
  [LocalizationKey]
  protected const string kRetryTextKey = "BUTTON_RETRY";
  [SerializeField]
  protected Button _startGameReadyButton;
  [SerializeField]
  protected Button _cancelGameUnreadyButton;
  [SerializeField]
  protected LocalizedTextMeshProUGUI _startReadyText;
  [SerializeField]
  protected LocalizedTextMeshProUGUI _cancelUnreadyText;
  [SerializeField]
  protected GameServersFilterText _serverSettings;
  [Space]
  [SerializeField]
  protected GameObject _suggestionHeader;
  [SerializeField]
  protected EditableBeatmapSelectionView _beatmapSelectionView;
  [SerializeField]
  protected EditableModifiersSelectionView _modifiersSelectionView;
  [SerializeField]
  protected HoverHint _cantStartGameHoverHint;
  [SerializeField]
  protected GameObject _playerMissingLevelHoverHintWrapper;
  [SerializeField]
  protected HoverHint _playersMissingLevelHoverHint;
  [SerializeField]
  protected GameObject _spectatorWarningTextWrapper;
  protected readonly ToggleBinder _toggleBinder = new ToggleBinder();
  protected bool _isPartyOwner;
  protected bool _isQuickStart;

  public event System.Action selectBeatmapEvent;

  public event System.Action selectModifiersEvent;

  public event System.Action startGameOrReadyEvent;

  public event System.Action cancelGameOrUnreadyEvent;

  public event System.Action clearSuggestedBeatmapEvent;

  public event System.Action clearSuggestedModifiersEvent;

  public virtual void Setup(
    BeatmapLevelSelectionMask selectionMask,
    bool isPartyOwner,
    bool allowSongSelection,
    bool allowModifierSelection,
    bool isManaged,
    bool isQuickStart)
  {
    this._isPartyOwner = isPartyOwner;
    this._isQuickStart = isQuickStart;
    this._serverSettings.Setup(selectionMask.difficulties, selectionMask.songPacks, !allowModifierSelection);
    this._beatmapSelectionView.Setup(!isPartyOwner);
    this._beatmapSelectionView.SetVisibility(allowSongSelection);
    this._modifiersSelectionView.Setup(!isPartyOwner);
    this._modifiersSelectionView.SetVisibility(allowModifierSelection);
    this._suggestionHeader.SetActive(isManaged && !isPartyOwner);
    this._startReadyText.Key = isPartyOwner ? "LOBBY_START_GAME" : "LOBBY_READY";
    this._cancelUnreadyText.Key = isPartyOwner ? "BUTTON_CANCEL" : (isQuickStart ? "BUTTON_RETRY" : "BUTTON_UNREADY");
    if (!this._isQuickStart)
      return;
    this._startGameReadyButton.gameObject.SetActive(false);
    this._cancelGameUnreadyButton.gameObject.SetActive(true);
  }

  public virtual void SetLobbyPlayerData(ILobbyPlayerData lobbyPlayerData)
  {
    if (lobbyPlayerData == null)
      return;
    this._beatmapSelectionView.SetBeatmap(lobbyPlayerData.beatmapLevel);
    this._modifiersSelectionView.SetGameplayModifiers(lobbyPlayerData.gameplayModifiers);
    this.SetPlayerActiveState(lobbyPlayerData.isActive);
    if (this._isPartyOwner || this._isQuickStart)
      return;
    this._startGameReadyButton.gameObject.SetActive(lobbyPlayerData.isActive && !lobbyPlayerData.isReady);
    this._cancelGameUnreadyButton.gameObject.SetActive(lobbyPlayerData.isActive && lobbyPlayerData.isReady);
  }

  public virtual void SetPlayersMissingLevelText(string playersMissingLevelText)
  {
    this._playerMissingLevelHoverHintWrapper.SetActive(!string.IsNullOrEmpty(playersMissingLevelText));
    if (playersMissingLevelText == null)
      return;
    this._playersMissingLevelHoverHint.text = playersMissingLevelText;
  }

  public virtual void SetPlayerActiveState(bool isActive) => this._spectatorWarningTextWrapper.SetActive(!isActive);

  public virtual void SetStartGameEnabled(CannotStartGameReason cannotStartGameReason)
  {
    this._startGameReadyButton.interactable = cannotStartGameReason == CannotStartGameReason.None;
    this._cantStartGameHoverHint.enabled = cannotStartGameReason != CannotStartGameReason.None;
    this._cantStartGameHoverHint.text = cannotStartGameReason.LocalizedKey();
  }

  public virtual void SetLobbyState(MultiplayerLobbyState lobbyState)
  {
    if (this._isPartyOwner)
    {
      this._startGameReadyButton.interactable = true;
      this._cancelGameUnreadyButton.interactable = lobbyState == MultiplayerLobbyState.LobbyCountdown;
      this._startGameReadyButton.gameObject.SetActive(lobbyState == MultiplayerLobbyState.LobbySetup);
      this._cancelGameUnreadyButton.gameObject.SetActive(lobbyState == MultiplayerLobbyState.LobbyCountdown || lobbyState == MultiplayerLobbyState.GameStarting);
    }
    else
    {
      this._startGameReadyButton.interactable = true;
      this._cancelGameUnreadyButton.interactable = lobbyState == MultiplayerLobbyState.LobbySetup || lobbyState == MultiplayerLobbyState.LobbyCountdown;
    }
    this._beatmapSelectionView.interactable = lobbyState == MultiplayerLobbyState.LobbySetup || lobbyState == MultiplayerLobbyState.LobbyCountdown;
    this._modifiersSelectionView.interactable = lobbyState == MultiplayerLobbyState.LobbySetup || lobbyState == MultiplayerLobbyState.LobbyCountdown;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._beatmapSelectionView.editButton, (System.Action) (() =>
    {
      System.Action selectBeatmapEvent = this.selectBeatmapEvent;
      if (selectBeatmapEvent == null)
        return;
      selectBeatmapEvent();
    }));
    this.buttonBinder.AddBinding(this._modifiersSelectionView.editButton, (System.Action) (() =>
    {
      System.Action selectModifiersEvent = this.selectModifiersEvent;
      if (selectModifiersEvent == null)
        return;
      selectModifiersEvent();
    }));
    this.buttonBinder.AddBinding(this._beatmapSelectionView.clearButton, (System.Action) (() =>
    {
      System.Action suggestedBeatmapEvent = this.clearSuggestedBeatmapEvent;
      if (suggestedBeatmapEvent == null)
        return;
      suggestedBeatmapEvent();
    }));
    this.buttonBinder.AddBinding(this._modifiersSelectionView.clearButton, (System.Action) (() =>
    {
      System.Action suggestedModifiersEvent = this.clearSuggestedModifiersEvent;
      if (suggestedModifiersEvent == null)
        return;
      suggestedModifiersEvent();
    }));
    this.buttonBinder.AddBinding(this._startGameReadyButton, (System.Action) (() =>
    {
      System.Action gameOrReadyEvent = this.startGameOrReadyEvent;
      if (gameOrReadyEvent == null)
        return;
      gameOrReadyEvent();
    }));
    this.buttonBinder.AddBinding(this._cancelGameUnreadyButton, (System.Action) (() =>
    {
      System.Action gameOrUnreadyEvent = this.cancelGameOrUnreadyEvent;
      if (gameOrUnreadyEvent == null)
        return;
      gameOrUnreadyEvent();
    }));
  }

  protected override void OnDestroy() => this._toggleBinder.ClearBindings();

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_0()
  {
    System.Action selectBeatmapEvent = this.selectBeatmapEvent;
    if (selectBeatmapEvent == null)
      return;
    selectBeatmapEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_1()
  {
    System.Action selectModifiersEvent = this.selectModifiersEvent;
    if (selectModifiersEvent == null)
      return;
    selectModifiersEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_2()
  {
    System.Action suggestedBeatmapEvent = this.clearSuggestedBeatmapEvent;
    if (suggestedBeatmapEvent == null)
      return;
    suggestedBeatmapEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_3()
  {
    System.Action suggestedModifiersEvent = this.clearSuggestedModifiersEvent;
    if (suggestedModifiersEvent == null)
      return;
    suggestedModifiersEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_4()
  {
    System.Action gameOrReadyEvent = this.startGameOrReadyEvent;
    if (gameOrReadyEvent == null)
      return;
    gameOrReadyEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_5()
  {
    System.Action gameOrUnreadyEvent = this.cancelGameOrUnreadyEvent;
    if (gameOrUnreadyEvent == null)
      return;
    gameOrUnreadyEvent();
  }
}
