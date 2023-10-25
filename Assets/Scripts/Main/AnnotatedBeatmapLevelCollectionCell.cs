// Decompiled with JetBrains decompiler
// Type: AnnotatedBeatmapLevelCollectionCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class AnnotatedBeatmapLevelCollectionCell : SelectableCell
{
  [SerializeField]
  protected TextMeshProUGUI _infoText;
  [SerializeField]
  protected ImageView _coverImage;
  [SerializeField]
  protected ImageView _selectionImage;
  [SerializeField]
  protected GameObject _downloadIconObject;
  [Space]
  [SerializeField]
  protected GameObject _newBadgeObject;
  [SerializeField]
  protected GameObject _updatedBadgeObject;
  [Space]
  [SerializeField]
  protected Color _selectedColor0 = new Color(0.0f, 0.7529412f, 1f, 1f);
  [SerializeField]
  protected Color _selectedColor1 = new Color(0.0f, 0.7529412f, 1f, 0.0f);
  [SerializeField]
  protected Color _highlightedColor0 = new Color(0.0f, 0.7529412f, 1f, 1f);
  [SerializeField]
  protected Color _highlightedColor1 = new Color(0.0f, 0.7529412f, 1f, 0.0f);
  [CompilerGenerated]
  protected int m_CcellIndex;
  protected IAnnotatedBeatmapLevelCollection _annotatedBeatmapLevelCollection;
  protected CancellationTokenSource _cancellationTokenSource;

  public int cellIndex
  {
    get => this.m_CcellIndex;
    set => this.m_CcellIndex = value;
  }

  public virtual void SetData(
    IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection,
    bool isPromoted,
    bool isUpdated)
  {
    this._annotatedBeatmapLevelCollection = annotatedBeatmapLevelCollection;
    this._infoText.text = this.GetInfoText(this._annotatedBeatmapLevelCollection.collectionName, annotatedBeatmapLevelCollection.beatmapLevelCollection.beatmapLevels.Count);
    this._coverImage.sprite = annotatedBeatmapLevelCollection.smallCoverImage;
    this._newBadgeObject.SetActive(isPromoted);
    this._updatedBadgeObject.SetActive(isUpdated && !isPromoted);
    this.SetDownloadIconVisible(false);
  }

  protected override void InternalToggle()
  {
    if (this.selected)
      return;
    this.SetSelected(true, SelectableCell.TransitionType.Animated, (object) this, true);
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    if (this.selected || this.highlighted)
    {
      this._selectionImage.enabled = true;
      this._selectionImage.color0 = this.highlighted ? this._highlightedColor0 : this._selectedColor0;
      this._selectionImage.color1 = this.highlighted ? this._highlightedColor1 : this._selectedColor1;
    }
    else
      this._selectionImage.enabled = false;
  }

  public virtual string GetInfoText(string name, int songs, int purchased = -1) => purchased >= 0 ? string.Format("{0}\n\nSongs {1}\nPurchased {2}", (object) name, (object) songs, (object) purchased) : string.Format("{0}\n\nSongs {1}", (object) name, (object) songs);

  public virtual async void RefreshAvailabilityAsync(AdditionalContentModel contentModel)
  {
    try
    {
      this.SetDownloadIconVisible(false);
      this._infoText.text = this.GetInfoText(this._annotatedBeatmapLevelCollection.collectionName, this._annotatedBeatmapLevelCollection.beatmapLevelCollection.beatmapLevels.Count);
      this._cancellationTokenSource?.Cancel();
      this._cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = this._cancellationTokenSource.Token;
      int numberOfOwnedLevels = 0;
      bool error = false;
      foreach (IPreviewBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) this._annotatedBeatmapLevelCollection.beatmapLevelCollection.beatmapLevels)
      {
        AdditionalContentModel.EntitlementStatus entitlementStatusAsync = await contentModel.GetLevelEntitlementStatusAsync(beatmapLevel.levelID, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        switch (entitlementStatusAsync)
        {
          case AdditionalContentModel.EntitlementStatus.Failed:
            error = true;
            goto label_12;
          case AdditionalContentModel.EntitlementStatus.Owned:
            ++numberOfOwnedLevels;
            continue;
          default:
            continue;
        }
      }
label_12:
      IEnumerator<IPreviewBeatmapLevel> enumerator = (IEnumerator<IPreviewBeatmapLevel>) null;
      if (!error && numberOfOwnedLevels == 0)
        this.SetDownloadIconVisible(true);
      if (!error && numberOfOwnedLevels != this._annotatedBeatmapLevelCollection.beatmapLevelCollection.beatmapLevels.Count)
        this._infoText.text = this.GetInfoText(this._annotatedBeatmapLevelCollection.collectionName, this._annotatedBeatmapLevelCollection.beatmapLevelCollection.beatmapLevels.Count, numberOfOwnedLevels);
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public virtual void SetDownloadIconVisible(bool visible)
  {
    this._downloadIconObject.SetActive(visible);
    this._coverImage.gradient = visible;
  }

  public virtual void CancelAsyncOperations() => this._cancellationTokenSource?.Cancel();
}
