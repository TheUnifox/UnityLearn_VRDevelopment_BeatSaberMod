// Decompiled with JetBrains decompiler
// Type: AnnotatedBeatmapLevelCollectionsGridView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class AnnotatedBeatmapLevelCollectionsGridView : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler,
  GridView.IDataSource
{
  [SerializeField]
  protected GridView _gridView;
  [SerializeField]
  protected PageControl _pageControl;
  [Space]
  [SerializeField]
  protected AnnotatedBeatmapLevelCollectionsGridViewAnimator _animator;
  [Space]
  [SerializeField]
  protected AnnotatedBeatmapLevelCollectionCell _cellPrefab;
  [SerializeField]
  protected float _cellWidth;
  [SerializeField]
  protected float _cellHeight;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  [Inject]
  protected readonly BeatmapLevelsPromoDataSO _beatmapLevelsPromoData;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  protected bool _isInitialized;
  protected bool _isHovering;
  protected IReadOnlyList<IAnnotatedBeatmapLevelCollection> _annotatedBeatmapLevelCollections;
  protected int _selectedCellIndex;

  public event System.Action didOpenAnnotatedBeatmapLevelCollectionEvent;

  public event System.Action didCloseAnnotatedBeatmapLevelCollectionEvent;

  public event System.Action<IAnnotatedBeatmapLevelCollection> didSelectAnnotatedBeatmapLevelCollectionEvent;

  public virtual void SetData(
    IReadOnlyList<IAnnotatedBeatmapLevelCollection> annotatedBeatmapLevelCollections)
  {
    this._annotatedBeatmapLevelCollections = annotatedBeatmapLevelCollections;
    this._selectedCellIndex = 0;
    this._gridView.SetDataSource((GridView.IDataSource) this, true);
    this._pageControl.SetPagesCount(this._gridView.rowCount);
    this._pageControl.SetSelectedPageIndex(this._selectedCellIndex / this._gridView.columnCount);
    this._animator.Init(this._cellHeight, this._gridView.rowCount);
  }

  public virtual void OnEnable()
  {
    this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    this._vrPlatformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleVRPlatformHelperInputFocusCaptured);
  }

  public virtual void OnDisable()
  {
    this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
    this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleVRPlatformHelperInputFocusCaptured);
  }

  public virtual void Show() => this.gameObject.SetActive(true);

  public virtual void Hide() => this.gameObject.SetActive(false);

  public virtual void CancelAsyncOperations()
  {
    foreach (MonoBehaviour monoBehaviour in this._gridView.GetActiveCellsForIdentifier((MonoBehaviour) this._cellPrefab))
    {
      if (monoBehaviour is AnnotatedBeatmapLevelCollectionCell levelCollectionCell)
        levelCollectionCell.CancelAsyncOperations();
    }
  }

  public virtual void RefreshAvailability()
  {
    if (this._annotatedBeatmapLevelCollections == null)
      return;
    foreach (MonoBehaviour monoBehaviour in this._gridView.cellsEnumerator)
    {
      if (monoBehaviour is AnnotatedBeatmapLevelCollectionCell levelCollectionCell)
        levelCollectionCell.RefreshAvailabilityAsync(this._additionalContentModel);
    }
  }

  public virtual void SelectAndScrollToCellWithIdx(int idx)
  {
    foreach (MonoBehaviour monoBehaviour in this._gridView.cellsEnumerator)
    {
      if (monoBehaviour is AnnotatedBeatmapLevelCollectionCell levelCollectionCell && levelCollectionCell.cellIndex == idx)
      {
        levelCollectionCell.SetSelected(true, SelectableCell.TransitionType.Instant, (object) null, false);
        break;
      }
    }
    this._selectedCellIndex = idx;
    this._pageControl.SetSelectedPageIndex(this._selectedCellIndex / this._gridView.columnCount);
    this._animator.ScrollToRowIdxInstant(this._selectedCellIndex / this._gridView.columnCount);
  }

  public virtual void OnPointerEnter(PointerEventData eventData)
  {
    if (this._gridView.rowCount <= 1)
      return;
    System.Action levelCollectionEvent = this.didOpenAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent != null)
      levelCollectionEvent();
    this._animator.AnimateOpen(true);
  }

  public virtual void OnPointerExit(PointerEventData eventData)
  {
    if (this._gridView.rowCount <= 1)
      return;
    System.Action levelCollectionEvent = this.didCloseAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent != null)
      levelCollectionEvent();
    this._animator.AnimateClose(this._selectedCellIndex / this._gridView.columnCount, true);
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData()
  {
    this._selectedCellIndex = Mathf.Min(this._selectedCellIndex, this.GetNumberOfCells() - 1);
    this._pageControl.SetSelectedPageIndex(this._selectedCellIndex / this._gridView.columnCount);
    this._gridView.ReloadData();
  }

  public virtual void HandleVRPlatformHelperInputFocusCaptured()
  {
    System.Action levelCollectionEvent = this.didCloseAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent != null)
      levelCollectionEvent();
    this._animator.AnimateClose(this._selectedCellIndex / this._gridView.columnCount, false);
  }

  public virtual void HandleCellSelectionDidChange(
    SelectableCell selectableCell,
    SelectableCell.TransitionType transition,
    object changeOwner)
  {
    if (this == (object)changeOwner || !selectableCell.selected)
      return;
    foreach (MonoBehaviour monoBehaviour in this._gridView.cellsEnumerator)
    {
      if (!((UnityEngine.Object) monoBehaviour == (UnityEngine.Object) selectableCell) && monoBehaviour is SelectableCell selectableCell1)
        selectableCell1.SetSelected(false, SelectableCell.TransitionType.Instant, (object) this, false);
    }
    this._selectedCellIndex = ((AnnotatedBeatmapLevelCollectionCell) selectableCell).cellIndex;
    this._pageControl.SetSelectedPageIndex(this._selectedCellIndex / this._gridView.columnCount);
    System.Action<IAnnotatedBeatmapLevelCollection> levelCollectionEvent1 = this.didSelectAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent1 != null)
      levelCollectionEvent1(this._annotatedBeatmapLevelCollections[this._selectedCellIndex]);
    if (this._gridView.rowCount <= 1)
      return;
    System.Action levelCollectionEvent2 = this.didCloseAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent2 != null)
      levelCollectionEvent2();
    this._animator.AnimateClose(this._selectedCellIndex / this._gridView.columnCount, true);
  }

  public virtual int GetNumberOfCells()
  {
    IReadOnlyList<IAnnotatedBeatmapLevelCollection> levelCollections = this._annotatedBeatmapLevelCollections;
    return levelCollections == null ? 0 : levelCollections.Count;
  }

  public virtual float GetCellWidth() => this._cellWidth;

  public virtual float GetCellHeight() => this._cellHeight;

  public virtual MonoBehaviour CellForIdx(GridView gridView, int idx)
  {
    AnnotatedBeatmapLevelCollectionCell reusableCellView = gridView.GetReusableCellView<AnnotatedBeatmapLevelCollectionCell>((MonoBehaviour) this._cellPrefab);
    bool isPromoted = false;
    bool isUpdated = false;
    if (this._annotatedBeatmapLevelCollections[idx] is IBeatmapLevelPack beatmapLevelCollection)
    {
      isPromoted = this._beatmapLevelsPromoData.IsBeatmapLevelPackPromoted(beatmapLevelCollection);
      isUpdated = this._beatmapLevelsPromoData.IsBeatmapLevelPackUpdated(beatmapLevelCollection);
    }
    reusableCellView.cellIndex = idx;
    reusableCellView.SetData(this._annotatedBeatmapLevelCollections[idx], isPromoted, isUpdated);
    reusableCellView.SetSelected(idx == this._selectedCellIndex, SelectableCell.TransitionType.Instant, (object) this, true);
    reusableCellView.selectionDidChangeEvent -= new System.Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleCellSelectionDidChange);
    reusableCellView.selectionDidChangeEvent += new System.Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleCellSelectionDidChange);
    reusableCellView.RefreshAvailabilityAsync(this._additionalContentModel);
    return (MonoBehaviour) reusableCellView;
  }
}
