// Decompiled with JetBrains decompiler
// Type: LevelListTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelListTableCell : TableCell
{
  [SerializeField]
  protected Image _backgroundImage;
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [Space]
  [SerializeField]
  protected Image _coverImage;
  [SerializeField]
  protected TextMeshProUGUI _songNameText;
  [SerializeField]
  protected TextMeshProUGUI _songAuthorText;
  [SerializeField]
  protected Image _favoritesBadgeImage;
  [SerializeField]
  protected TextMeshProUGUI _songDurationText;
  [SerializeField]
  protected TextMeshProUGUI _songBpmText;
  [Space]
  [SerializeField]
  protected Color _highlightBackgroundColor;
  [SerializeField]
  protected Color _selectedBackgroundColor;
  [SerializeField]
  protected Color _selectedAndHighlightedBackgroundColor;
  [SerializeField]
  protected float _notOwnedAlpha = 0.5f;
  [Space]
  [SerializeField]
  protected GameObject _promoBadgeGo;
  [SerializeField]
  protected GameObject _updatedBadgeGo;
  protected CancellationTokenSource _refreshingAvailabilityCancellationTokenSource;
  protected CancellationTokenSource _settingDataCancellationTokenSource;
  protected bool _notOwned;
  protected string _refreshingAvailabilityLevelID;
  protected string _settingDataFromLevelId;

  public virtual async void SetDataFromLevelAsync(
    IPreviewBeatmapLevel level,
    bool isFavorite,
    bool isPromoted,
    bool isUpdated)
  {
    if (this._settingDataFromLevelId == level.levelID)
      return;
    try
    {
      this._favoritesBadgeImage.enabled = isFavorite;
      this._settingDataFromLevelId = level.levelID;
      this._settingDataCancellationTokenSource?.Cancel();
      this._settingDataCancellationTokenSource = new CancellationTokenSource();
      this._songNameText.text = level.songName + " <size=80%>" + level.songSubName + "</size>";
      this._songAuthorText.text = level.songAuthorName;
      this._songDurationText.text = level.songDuration.MinSecDurationText();
      this._songBpmText.text = level.beatsPerMinute.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      this._coverImage.sprite = (Sprite) null;
      this._coverImage.color = Color.clear;
      this._promoBadgeGo.SetActive(isPromoted);
      this._updatedBadgeGo.SetActive(isUpdated && !isPromoted);
      CancellationToken cancellationToken = this._settingDataCancellationTokenSource.Token;
      Sprite coverImageAsync = await level.GetCoverImageAsync(cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      this._coverImage.sprite = coverImageAsync;
      this._coverImage.color = Color.white;
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException)
    {
    }
    finally
    {
      if (this._settingDataFromLevelId == level.levelID)
        this._settingDataFromLevelId = (string) null;
    }
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    this._canvasGroup.alpha = this._notOwned ? this._notOwnedAlpha : 1f;
    if (this.selected && this.highlighted)
      this._backgroundImage.color = this._selectedAndHighlightedBackgroundColor;
    else if (this.highlighted)
      this._backgroundImage.color = this._highlightBackgroundColor;
    else if (this.selected)
      this._backgroundImage.color = this._selectedBackgroundColor;
    this._backgroundImage.enabled = this.selected || this.highlighted;
  }

  protected override void WasPreparedForReuse() => this.CancelAsyncOperations();

  public virtual async void RefreshAvailabilityAsync(
    AdditionalContentModel contentModel,
    string levelID)
  {
    if (this._refreshingAvailabilityLevelID == levelID)
      return;
    try
    {
      this._notOwned = false;
      this.RefreshVisuals();
      this._refreshingAvailabilityLevelID = levelID;
      this._refreshingAvailabilityCancellationTokenSource?.Cancel();
      this._refreshingAvailabilityCancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = this._refreshingAvailabilityCancellationTokenSource.Token;
      AdditionalContentModel.EntitlementStatus entitlementStatusAsync = await contentModel.GetLevelEntitlementStatusAsync(levelID, cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      this._notOwned = entitlementStatusAsync == AdditionalContentModel.EntitlementStatus.NotOwned;
      this.RefreshVisuals();
      this._refreshingAvailabilityLevelID = (string) null;
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException)
    {
    }
    finally
    {
      if (this._refreshingAvailabilityLevelID == levelID)
        this._refreshingAvailabilityLevelID = (string) null;
    }
  }

  public virtual void CancelAsyncOperations()
  {
    this._refreshingAvailabilityCancellationTokenSource?.Cancel();
    this._refreshingAvailabilityCancellationTokenSource = (CancellationTokenSource) null;
    this._settingDataCancellationTokenSource?.Cancel();
    this._settingDataCancellationTokenSource = (CancellationTokenSource) null;
  }
}
