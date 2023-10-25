// Decompiled with JetBrains decompiler
// Type: GameServerPlayerTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameServerPlayerTableCell : TableCellWithSeparator
{
  [SerializeField]
  protected CurvedTextMeshPro _playerNameText;
  [SerializeField]
  protected Image _localPlayerBackgroundImage;
  [Header("Suggested Level")]
  [SerializeField]
  protected CurvedTextMeshPro _suggestedLevelText;
  [SerializeField]
  protected ImageView _suggestedCharacteristicIcon;
  [SerializeField]
  protected TextMeshProUGUI _suggestedDifficultyText;
  [SerializeField]
  protected CurvedTextMeshPro _emptySuggestedLevelText;
  [Header("Suggested Modifiers")]
  [SerializeField]
  protected GameplayModifierInfoListItemsList _suggestedModifiersList;
  [SerializeField]
  protected CurvedTextMeshPro _emptySuggestedModifiersText;
  [Header("Button")]
  [SerializeField]
  protected Button _mutePlayerButton;
  [SerializeField]
  protected Button _kickPlayerButton;
  [SerializeField]
  protected Button _useBeatmapButton;
  [SerializeField]
  protected Button _useModifiersButton;
  [SerializeField]
  protected HoverHint _useBeatmapButtonHoverHint;
  [Header("Status Icons")]
  [SerializeField]
  protected ButtonSpriteSwapToggle _muteToggle;
  [SerializeField]
  protected ImageView _statusImageView;
  [SerializeField]
  protected Sprite _readyIcon;
  [SerializeField]
  protected Sprite _spectatingIcon;
  [SerializeField]
  protected Sprite _hostIcon;
  [Header("Helpers")]
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiers;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();
  protected CancellationTokenSource _getLevelEntitlementCancellationTokenSource;

  public event System.Action<int> kickPlayerEvent;

  public event System.Action<int> useBeatmapEvent;

  public event System.Action<int> useModifiersEvent;

  public virtual void SetData(
    IConnectedPlayer connectedPlayer,
    ILobbyPlayerData playerData,
    bool hasKickPermissions,
    bool allowSelection,
    Task<AdditionalContentModel.EntitlementStatus> getLevelEntitlementTask)
  {
    this._playerNameText.text = connectedPlayer.userName;
    this._localPlayerBackgroundImage.enabled = connectedPlayer.isMe;
    if (!playerData.isReady && playerData.isActive && !playerData.isPartyOwner)
    {
      this._statusImageView.enabled = false;
    }
    else
    {
      this._statusImageView.enabled = true;
      this._statusImageView.sprite = playerData.isPartyOwner ? this._hostIcon : (!playerData.isActive ? this._spectatingIcon : this._readyIcon);
    }
    bool flag1 = playerData.beatmapLevel != (PreviewDifficultyBeatmap) null;
    this._suggestedLevelText.gameObject.SetActive(flag1);
    this._suggestedCharacteristicIcon.gameObject.SetActive(flag1);
    this._suggestedDifficultyText.gameObject.SetActive(flag1);
    this._emptySuggestedLevelText.gameObject.SetActive(!flag1);
    if (flag1)
    {
      this._suggestedLevelText.text = playerData.beatmapLevel.beatmapLevel.songName;
      this._suggestedCharacteristicIcon.sprite = playerData.beatmapLevel.beatmapCharacteristic.icon;
      this._suggestedDifficultyText.text = playerData.beatmapLevel.beatmapDifficulty.ShortName();
    }
    bool flag2 = playerData?.gameplayModifiers != null && !playerData.gameplayModifiers.IsWithoutModifiers();
    this._suggestedModifiersList.gameObject.SetActive(flag2);
    this._emptySuggestedModifiersText.gameObject.SetActive(!flag2);
    if (flag2)
    {
      List<GameplayModifierParamsSO> modifiersList = this._gameplayModifiers.CreateModifierParamsList(playerData.gameplayModifiers);
      this._emptySuggestedModifiersText.gameObject.SetActive(modifiersList.Count == 0);
      if (modifiersList.Count > 0)
        this._suggestedModifiersList.SetData(modifiersList.Count, (UIItemsList<GameplayModifierInfoListItem>.DataCallback) ((id, listItem) => listItem.SetModifier(modifiersList[id])));
    }
    this._useModifiersButton.interactable = !connectedPlayer.isMe & flag2 & allowSelection;
    this._kickPlayerButton.interactable = !connectedPlayer.isMe & hasKickPermissions & allowSelection;
    this._mutePlayerButton.gameObject.SetActive(false);
    if (getLevelEntitlementTask != null && !connectedPlayer.isMe)
    {
      this._useBeatmapButtonHoverHint.text = Localization.Get("LABEL_CANT_START_GAME_DO_NOT_OWN_SONG");
      this.SetBeatmapUseButtonEnabledAsync(getLevelEntitlementTask);
    }
    else
    {
      this._useBeatmapButtonHoverHint.enabled = false;
      this._useBeatmapButton.interactable = false;
    }
  }

  public virtual void Awake()
  {
    this._buttonBinder.AddBinding(this._kickPlayerButton, new System.Action(this.HandleKickPlayerButtonPressed));
    this._buttonBinder.AddBinding(this._useBeatmapButton, new System.Action(this.HandleUseBeatmapButtonPressed));
    this._buttonBinder.AddBinding(this._useModifiersButton, new System.Action(this.HandleUseModifiersButtonPressed));
  }

  public virtual void HandleKickPlayerButtonPressed()
  {
    System.Action<int> kickPlayerEvent = this.kickPlayerEvent;
    if (kickPlayerEvent == null)
      return;
    kickPlayerEvent(this.idx);
  }

  public virtual void HandleUseBeatmapButtonPressed()
  {
    System.Action<int> useBeatmapEvent = this.useBeatmapEvent;
    if (useBeatmapEvent == null)
      return;
    useBeatmapEvent(this.idx);
  }

  public virtual void HandleUseModifiersButtonPressed()
  {
    System.Action<int> useModifiersEvent = this.useModifiersEvent;
    if (useModifiersEvent == null)
      return;
    useModifiersEvent(this.idx);
  }

  public virtual async void SetBeatmapUseButtonEnabledAsync(
    Task<AdditionalContentModel.EntitlementStatus> getLevelEntitlementTask)
  {
    this._useBeatmapButton.interactable = false;
    this._useBeatmapButtonHoverHint.enabled = false;
    this._getLevelEntitlementCancellationTokenSource?.Cancel();
    this._getLevelEntitlementCancellationTokenSource?.Dispose();
    this._getLevelEntitlementCancellationTokenSource = new CancellationTokenSource();
    CancellationToken cancellationToken = this._getLevelEntitlementCancellationTokenSource.Token;
    try
    {
      int num = (int) await getLevelEntitlementTask;
      cancellationToken.ThrowIfCancellationRequested();
      bool flag = num == 1;
      this._useBeatmapButton.interactable = flag;
      this._useBeatmapButtonHoverHint.enabled = !flag;
    }
    catch (OperationCanceledException ex)
    {
    }
    finally
    {
      this._getLevelEntitlementCancellationTokenSource?.Dispose();
      this._getLevelEntitlementCancellationTokenSource = (CancellationTokenSource) null;
    }
  }
}
