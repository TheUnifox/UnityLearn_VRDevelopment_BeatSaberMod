// Decompiled with JetBrains decompiler
// Type: LevelCollectionViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelCollectionViewController : ViewController
{
  [SerializeField]
  protected LevelCollectionTableView _levelCollectionTableView;
  [SerializeField]
  protected RectTransform _noDataInfoContainer;
  [Space]
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly DiContainer _container;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  [Inject]
  protected readonly PerceivedLoudnessPerLevelModel _perceivedLoudnessPerLevelModel;
  [Inject]
  protected readonly AudioClipAsyncLoader _audioClipAsyncLoader;
  protected bool _showHeader = true;
  protected string _songPlayerCrossFadingToLevelId;
  protected GameObject _noDataInfoGO;
  protected IPreviewBeatmapLevel _previewBeatmapLevelToBeSelected;

  public event System.Action<LevelCollectionViewController, IPreviewBeatmapLevel> didSelectLevelEvent;

  public event System.Action<LevelCollectionViewController> didSelectHeaderEvent;

  public virtual void SetData(
    IBeatmapLevelCollection beatmapLevelCollection,
    string headerText,
    Sprite headerSprite,
    bool sortLevels,
    GameObject noDataInfoPrefab)
  {
    this._showHeader = !string.IsNullOrEmpty(headerText);
    this._levelCollectionTableView.Init(headerText, headerSprite);
    if ((UnityEngine.Object) this._noDataInfoGO != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._noDataInfoGO);
      this._noDataInfoGO = (GameObject) null;
    }
    if (beatmapLevelCollection != null && beatmapLevelCollection.beatmapLevels.Count > 0)
    {
      this._levelCollectionTableView.gameObject.SetActive(true);
      this._levelCollectionTableView.SetData(beatmapLevelCollection.beatmapLevels, this._playerDataModel.playerData.favoritesLevelIds, sortLevels);
      this._levelCollectionTableView.RefreshLevelsAvailability();
    }
    else
    {
      this._levelCollectionTableView.SetData((IReadOnlyList<IPreviewBeatmapLevel>) new IPreviewBeatmapLevel[0], this._playerDataModel.playerData.favoritesLevelIds, sortLevels);
      if ((UnityEngine.Object) noDataInfoPrefab != (UnityEngine.Object) null)
        this._noDataInfoGO = this._container.InstantiatePrefab((UnityEngine.Object) noDataInfoPrefab, (Transform) this._noDataInfoContainer);
      this._levelCollectionTableView.gameObject.SetActive(false);
    }
    if (!this.isInViewControllerHierarchy)
      return;
    if (this._showHeader)
      this._levelCollectionTableView.SelectLevelPackHeaderCell();
    else
      this._levelCollectionTableView.ClearSelection();
    this._songPreviewPlayer.CrossfadeToDefault();
  }

  public virtual void SelectLevel(IPreviewBeatmapLevel beatmapLevel)
  {
    if (this.isActivated)
    {
      this._levelCollectionTableView.SelectLevel(beatmapLevel);
      this._previewBeatmapLevelToBeSelected = (IPreviewBeatmapLevel) null;
    }
    else
      this._previewBeatmapLevelToBeSelected = beatmapLevel;
  }

  public virtual void RefreshFavorites() => this._levelCollectionTableView.RefreshFavorites(this._playerDataModel.playerData.favoritesLevelIds);

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this._levelCollectionTableView.didSelectLevelEvent += new System.Action<LevelCollectionTableView, IPreviewBeatmapLevel>(this.HandleLevelCollectionTableViewDidSelectLevel);
      this._levelCollectionTableView.didSelectHeaderEvent += new System.Action<LevelCollectionTableView>(this.HandleLevelCollectionTableViewDidSelectPack);
      if (this._showHeader)
        this._levelCollectionTableView.SelectLevelPackHeaderCell();
      else
        this._levelCollectionTableView.ClearSelection();
    }
    this._levelCollectionTableView.RefreshLevelsAvailability();
    this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    if (this._previewBeatmapLevelToBeSelected == null)
      return;
    this._levelCollectionTableView.SelectLevel(this._previewBeatmapLevelToBeSelected);
    this._previewBeatmapLevelToBeSelected = (IPreviewBeatmapLevel) null;
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (removedFromHierarchy)
    {
      this._levelCollectionTableView.didSelectLevelEvent -= new System.Action<LevelCollectionTableView, IPreviewBeatmapLevel>(this.HandleLevelCollectionTableViewDidSelectLevel);
      this._levelCollectionTableView.didSelectHeaderEvent -= new System.Action<LevelCollectionTableView>(this.HandleLevelCollectionTableViewDidSelectPack);
    }
    this._levelCollectionTableView.CancelAsyncOperations();
    this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    this._songPreviewPlayer.CrossfadeToDefault();
  }

  public virtual void HandleLevelCollectionTableViewDidSelectLevel(
    LevelCollectionTableView tableView,
    IPreviewBeatmapLevel level)
  {
    this._previewBeatmapLevelToBeSelected = (IPreviewBeatmapLevel) null;
    this.SongPlayerCrossfadeToLevelAsync(level);
    System.Action<LevelCollectionViewController, IPreviewBeatmapLevel> selectLevelEvent = this.didSelectLevelEvent;
    if (selectLevelEvent == null)
      return;
    selectLevelEvent(this, level);
  }

  public virtual async void SongPlayerCrossfadeToLevelAsync(IPreviewBeatmapLevel level)
  {
    if (this._songPlayerCrossFadingToLevelId == level.levelID)
      return;
    try
    {
      this._songPlayerCrossFadingToLevelId = level.levelID;
      this._songPreviewPlayer.CrossfadeTo(await this._audioClipAsyncLoader.LoadPreview(level), this._perceivedLoudnessPerLevelModel.GetLoudnessCorrectionByLevelId(level.levelID), level.previewStartTime, level.previewDuration, (System.Action) (() => this._audioClipAsyncLoader.UnloadPreview(level)));
      if (!(this._songPlayerCrossFadingToLevelId == level.levelID))
        return;
      this._songPlayerCrossFadingToLevelId = (string) null;
    }
    catch (OperationCanceledException ex)
    {
      if (!(this._songPlayerCrossFadingToLevelId == level.levelID))
        return;
      this._songPlayerCrossFadingToLevelId = (string) null;
    }
  }

  public virtual void HandleLevelCollectionTableViewDidSelectPack(LevelCollectionTableView tableView)
  {
    this._songPreviewPlayer.CrossfadeToDefault();
    System.Action<LevelCollectionViewController> selectHeaderEvent = this.didSelectHeaderEvent;
    if (selectHeaderEvent == null)
      return;
    selectHeaderEvent(this);
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData() => this._levelCollectionTableView.RefreshLevelsAvailability();
}
