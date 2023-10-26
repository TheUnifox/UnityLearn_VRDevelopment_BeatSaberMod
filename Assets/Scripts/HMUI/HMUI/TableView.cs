// Decompiled with JetBrains decompiler
// Type: HMUI.TableView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HMUI
{
  [RequireComponent(typeof (ScrollView))]
  public class TableView : MonoBehaviour, ITableCellOwner
  {
    [SerializeField]
    protected ScrollView _scrollView;
    [Space]
    [SerializeField]
    protected bool _scrollToTopOnEnable;
    [SerializeField]
    protected bool _alignToCenter;
    [SerializeField]
    protected TableView.TableType _tableType;
    [Space]
    [SerializeField]
    protected TableViewSelectionType _selectionType = TableViewSelectionType.Single;
    [SerializeField]
    protected bool _canSelectSelectedCell;
    [Space]
    [SerializeField]
    [NullAllowed]
    protected TableView.CellsGroup[] _preallocatedCells;
    protected RectTransform _contentTransform;
    protected RectTransform _viewportTransform;
    protected TableView.IDataSource _dataSource;
    protected int _numberOfCells;
    protected float _cellSize;
    protected readonly List<TableCell> _visibleCells = new List<TableCell>();
    protected Dictionary<string, List<TableCell>> _reusableCells;
    protected HashSet<int> _selectedCellIdxs = new HashSet<int>();
    protected int _prevMinIdx = -1;
    protected int _prevMaxIdx = -1;
    protected bool _isInitialized;
    protected bool _refreshCellsOnEnable;

    public TableViewSelectionType selectionType
    {
      get => this._selectionType;
      set => this._selectionType = value;
    }

    public bool canSelectSelectedCell => this._canSelectSelectedCell;

    public event Action<TableView, int> didSelectCellWithIdxEvent;

    public event Action<TableView> didReloadDataEvent;

    public event Action<TableView> didInsertCellsEvent;

    public event Action<TableView> didDeleteCellsEvent;

    public event Action<TableView> didChangeRectSizeEvent;

    public TableView.IDataSource dataSource => this._dataSource;

    public virtual void SetDataSource(TableView.IDataSource newDataSource, bool reloadData)
    {
      this._dataSource = newDataSource;
      if (!reloadData)
        return;
      this.ReloadData();
    }

    public IEnumerable<TableCell> visibleCells => (IEnumerable<TableCell>) this._visibleCells;

    public RectTransform viewportTransform => this._viewportTransform;

    public RectTransform contentTransform => this._contentTransform;

    public int numberOfCells => this._numberOfCells;

    public float cellSize => this._cellSize;

    public TableView.TableType tableType => this._tableType;

    protected ScrollView scrollView => this._scrollView;

    public virtual void Awake()
    {
      if (this._isInitialized)
        return;
      this.LazyInit();
    }

    public virtual void OnDestroy() => this._scrollView.scrollPositionChangedEvent -= new Action<float>(this.HandleScrollRectValueChanged);

    public virtual void OnEnable()
    {
      if (this._refreshCellsOnEnable)
      {
        this.RefreshCells(true, false);
        this._refreshCellsOnEnable = false;
      }
      this.ClearHighlights();
      if (!this._scrollToTopOnEnable)
        return;
      this.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, false);
    }

    public virtual void LazyInit()
    {
      if (this._isInitialized)
        return;
      this._isInitialized = true;
      this._reusableCells = new Dictionary<string, List<TableCell>>();
      foreach (TableView.CellsGroup preallocatedCell in this._preallocatedCells)
      {
        this._reusableCells[preallocatedCell.reuseIdentifier] = preallocatedCell.cells;
        foreach (TableCell cell in preallocatedCell.cells)
          cell.reuseIdentifier = preallocatedCell.reuseIdentifier;
      }
      this._contentTransform = this._scrollView.contentTransform;
      this._viewportTransform = this._scrollView.viewportTransform;
      this._scrollView._scrollType = ScrollView.ScrollType.FixedCellSize;
      this._scrollView.scrollPositionChangedEvent += new Action<float>(this.HandleScrollRectValueChanged);
      if (this._tableType == TableView.TableType.Vertical)
      {
        this._contentTransform.anchorMin = new Vector2(0.0f, 1f);
        this._contentTransform.anchorMax = new Vector2(1f, 1f);
        this._contentTransform.pivot = new Vector2(0.5f, 1f);
      }
      else
      {
        this._contentTransform.anchorMin = new Vector2(0.0f, 0.0f);
        this._contentTransform.anchorMax = new Vector2(0.0f, 1f);
        this._contentTransform.pivot = new Vector2(0.0f, 0.5f);
      }
      this._contentTransform.sizeDelta = new Vector2(0.0f, 0.0f);
      this._contentTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    }

    public virtual void Hide() => this.gameObject.SetActive(false);

    public virtual void Show() => this.gameObject.SetActive(true);

    public virtual void RefreshContentSize()
    {
      this._contentTransform.sizeDelta = this._tableType != TableView.TableType.Vertical ? new Vector2((float) this._numberOfCells * this._cellSize, 0.0f) : new Vector2(0.0f, (float) this._numberOfCells * this._cellSize);
      this._scrollView.UpdateContentSize();
    }

    public virtual void RefreshCellsContent() => this.RefreshCells(false, true);

    public virtual Tuple<int, int> GetVisibleCellsIdRange()
    {
      Rect rect = this._viewportTransform.rect;
      float num1 = this._tableType == TableView.TableType.Vertical ? rect.height : rect.width;
      double position = (double) this._scrollView.position;
      int num2 = Mathf.FloorToInt((float) (position / (double) this._cellSize + (double) this._cellSize * (1.0 / 1000.0)));
      if (num2 < 0)
        num2 = 0;
      int num3 = Mathf.FloorToInt((float) (position + (double) num1 - (double) this._cellSize * (1.0 / 1000.0)) / this._cellSize);
      if (num3 > this._numberOfCells - 1)
        num3 = this._numberOfCells - 1;
      return new Tuple<int, int>(num2, num3);
    }

    public virtual void RefreshCells(bool forcedVisualsRefresh, bool forcedContentRefresh)
    {
      this.LazyInit();
      int num1;
      int num2;
      TupleExtensions.Deconstruct<int, int>(this.GetVisibleCellsIdRange(), out num1, out num2);
      int num3 = num1;
      int num4 = num2;
      if (num3 == this._prevMinIdx && num4 == this._prevMaxIdx && !forcedVisualsRefresh && !forcedContentRefresh)
        return;
      for (int index = this._visibleCells.Count - 1; index >= 0; --index)
      {
        TableCell visibleCell = this._visibleCells[index];
        if (((visibleCell.idx < num3 ? 1 : (visibleCell.idx > num4 ? 1 : 0)) | (forcedContentRefresh ? 1 : 0)) != 0)
        {
          visibleCell.gameObject.SetActive(false);
          this._visibleCells.RemoveAt(index);
          this.AddCellToReusableCells(visibleCell);
        }
      }
      Rect rect = this._viewportTransform.rect;
      float num5 = this._tableType == TableView.TableType.Vertical ? rect.height : rect.width;
      float offset = 0.0f;
      if (this._alignToCenter && (double) this._scrollView.scrollableSize == 0.0)
        offset = (float) (((double) num5 - (double) this._numberOfCells * (double) this._cellSize) * 0.5);
      for (int idx = num3; idx <= num4; ++idx)
      {
        TableCell cell = (TableCell) null;
        for (int index = 0; index < this._visibleCells.Count; ++index)
        {
          if (this._visibleCells[index].idx == idx)
          {
            cell = this._visibleCells[index];
            break;
          }
        }
        if (!((UnityEngine.Object) cell != (UnityEngine.Object) null) || forcedVisualsRefresh || forcedContentRefresh)
        {
          bool ignoreCurrentValue = false;
          if ((UnityEngine.Object) cell == (UnityEngine.Object) null)
          {
            ignoreCurrentValue = true;
            cell = this._dataSource.CellForIdx(this, idx);
            this._visibleCells.Add(cell);
          }
          cell.gameObject.SetActive(true);
          cell.TableViewSetup((ITableCellOwner) this, idx);
          cell.selectionDidChangeEvent -= new Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleCellSelectionDidChange);
          cell.selectionDidChangeEvent += new Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleCellSelectionDidChange);
          if (ignoreCurrentValue)
            cell.ClearHighlight(SelectableCell.TransitionType.Instant);
          cell.SetSelected(this._selectedCellIdxs.Contains(idx), SelectableCell.TransitionType.Instant, (object) this, ignoreCurrentValue);
          if ((UnityEngine.Object) cell.transform.parent != (UnityEngine.Object) this._contentTransform)
            cell.transform.SetParent((Transform) this._contentTransform, false);
          this.LayoutCellForIdx(cell, idx, offset);
          if (this._visibleCells.Count == num4 - num3 + 1 && !forcedVisualsRefresh)
            break;
        }
      }
      this._prevMinIdx = num3;
      this._prevMaxIdx = num4;
    }

    public virtual void LayoutCellForIdx(TableCell cell, int idx, float offset)
    {
      RectTransform transform = (RectTransform) cell.transform;
      if (this._tableType == TableView.TableType.Vertical)
      {
        transform.anchorMax = new Vector2(1f, 1f);
        transform.anchorMin = new Vector2(0.0f, 1f);
        transform.pivot = new Vector2(0.5f, 1f);
        transform.sizeDelta = new Vector2(0.0f, this._cellSize);
        transform.anchoredPosition = new Vector2(0.0f, (float) -idx * this._cellSize - offset);
      }
      else
      {
        transform.anchorMax = new Vector2(0.0f, 1f);
        transform.anchorMin = new Vector2(0.0f, 0.0f);
        transform.pivot = new Vector2(0.0f, 0.5f);
        transform.sizeDelta = new Vector2(this._cellSize, 0.0f);
        transform.anchoredPosition = new Vector2((float) idx * this._cellSize + offset, 0.0f);
      }
    }

    public virtual void AddCellToReusableCells(TableCell cell)
    {
      List<TableCell> tableCellList;
      if (!this._reusableCells.TryGetValue(cell.reuseIdentifier, out tableCellList))
      {
        tableCellList = new List<TableCell>();
        this._reusableCells.Add(cell.reuseIdentifier, tableCellList);
        cell.__WasPreparedForReuse();
      }
      tableCellList.Add(cell);
    }

    public virtual void HandleScrollRectValueChanged(float f) => this.RefreshCells(false, false);

    public virtual void HandleCellSelectionDidChange(
      SelectableCell selectableCell,
      SelectableCell.TransitionType transitionType,
      object changeOwner)
    {
      if (this == (object)changeOwner || this.selectionType == TableViewSelectionType.None)
        return;
      TableCell tableCell = (TableCell) selectableCell;
      if (this.selectionType != TableViewSelectionType.Multiple)
      {
        foreach (TableCell visibleCell in this._visibleCells)
        {
          if (!((UnityEngine.Object) tableCell == (UnityEngine.Object) visibleCell))
            visibleCell.SetSelected(false, SelectableCell.TransitionType.Instant, (object) this, false);
        }
      }
      if (tableCell.selected)
      {
        if (this.selectionType != TableViewSelectionType.Multiple)
          this._selectedCellIdxs.Clear();
        this._selectedCellIdxs.Add(tableCell.idx);
        this.DidSelectCellWithIdx(tableCell.idx);
      }
      else
        this._selectedCellIdxs.Remove(tableCell.idx);
    }

    protected virtual void DidSelectCellWithIdx(int idx)
    {
      Action<TableView, int> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
      if (cellWithIdxEvent == null)
        return;
      cellWithIdxEvent(this, idx);
    }

    public virtual void ReloadDataKeepingPosition()
    {
      Vector2 anchoredPosition = this._contentTransform.anchoredPosition;
      this.ReloadData();
      double num = (double) Mathf.Min(this._tableType == TableView.TableType.Horizontal ? anchoredPosition.x : anchoredPosition.y, this._cellSize * (float) this._numberOfCells);
      this._scrollView.ScrollTo(anchoredPosition.y, false);
    }

    public virtual void ReloadData()
    {
      if (!this._isInitialized)
        this.LazyInit();
      foreach (TableCell visibleCell in this._visibleCells)
      {
        visibleCell.gameObject.SetActive(false);
        this.AddCellToReusableCells(visibleCell);
      }
      this._visibleCells.Clear();
      if (this._dataSource != null)
      {
        this._numberOfCells = this._dataSource.NumberOfCells();
        this._cellSize = this._dataSource.CellSize();
      }
      else
      {
        this._numberOfCells = 0;
        this._cellSize = 1f;
      }
      this._scrollView._fixedCellSize = this._cellSize;
      this.RefreshContentSize();
      if (!this.gameObject.activeInHierarchy)
        this._refreshCellsOnEnable = true;
      else
        this.RefreshCells(true, false);
      Action<TableView> didReloadDataEvent = this.didReloadDataEvent;
      if (didReloadDataEvent == null)
        return;
      didReloadDataEvent(this);
    }

    public virtual void InsertCells(int idx, int count)
    {
      foreach (TableCell visibleCell in this._visibleCells)
      {
        if (visibleCell.idx >= idx)
          visibleCell.MoveIdx(count);
      }
      HashSet<int> intSet = new HashSet<int>();
      foreach (int num in this._selectedCellIdxs)
      {
        if (num >= idx)
          intSet.Add(num + count);
        else
          intSet.Add(num);
      }
      this._selectedCellIdxs = intSet;
      int numberOfCells = this._numberOfCells;
      this._numberOfCells = this._dataSource.NumberOfCells();
      this.RefreshContentSize();
      this.RefreshCells(true, false);
      Action<TableView> insertCellsEvent = this.didInsertCellsEvent;
      if (insertCellsEvent == null)
        return;
      insertCellsEvent(this);
    }

    public virtual void DeleteCells(int idx, int count)
    {
      for (int index = this._visibleCells.Count - 1; index >= 0; --index)
      {
        TableCell visibleCell = this._visibleCells[index];
        if (visibleCell.idx >= idx && visibleCell.idx < idx + count)
        {
          visibleCell.gameObject.SetActive(false);
          this._visibleCells.RemoveAt(index);
          this.AddCellToReusableCells(visibleCell);
        }
        else if (visibleCell.idx >= idx + count)
          visibleCell.MoveIdx(-count);
      }
      HashSet<int> intSet = new HashSet<int>();
      foreach (int num in this._selectedCellIdxs)
      {
        if (num >= idx + count)
          intSet.Add(num - count);
        else if (num < idx)
          intSet.Add(num);
      }
      this._selectedCellIdxs = intSet;
      int numberOfCells = this._numberOfCells;
      this._numberOfCells = this._dataSource.NumberOfCells();
      this.RefreshContentSize();
      this.RefreshCells(true, false);
      Action<TableView> deleteCellsEvent = this.didDeleteCellsEvent;
      if (deleteCellsEvent == null)
        return;
      deleteCellsEvent(this);
    }

    public virtual TableCell DequeueReusableCellForIdentifier(string identifier)
    {
      TableCell tableCell = (TableCell) null;
      List<TableCell> tableCellList;
      if (this._reusableCells.TryGetValue(identifier, out tableCellList) && tableCellList.Count > 0)
      {
        tableCell = tableCellList[0];
        tableCellList.RemoveAt(0);
      }
      return tableCell;
    }

    public virtual void SelectCellWithIdx(int idx, bool callbackTable = false)
    {
      foreach (SelectableCell visibleCell in this._visibleCells)
        visibleCell.SetSelected(false, SelectableCell.TransitionType.Instant, (object) this, false);
      this._selectedCellIdxs.Clear();
      this._selectedCellIdxs.Add(idx);
      this.RefreshCells(true, false);
      if (!callbackTable)
        return;
      this.DidSelectCellWithIdx(idx);
    }

    public virtual void ClearSelection()
    {
      foreach (TableCell visibleCell in this._visibleCells)
      {
        visibleCell.SetSelected(false, SelectableCell.TransitionType.Instant, (object) this, false);
        visibleCell.ClearHighlight(SelectableCell.TransitionType.Instant);
      }
      this._selectedCellIdxs.Clear();
      this.RefreshCells(true, false);
    }

    public virtual void ClearHighlights()
    {
      foreach (SelectableCell visibleCell in this._visibleCells)
        visibleCell.ClearHighlight(SelectableCell.TransitionType.Instant);
    }

    public virtual void ScrollToCellWithIdx(
      int idx,
      TableView.ScrollPositionType scrollPositionType,
      bool animated)
    {
      Tuple<int, int> visibleCellsIdRange = this.GetVisibleCellsIdRange();
      int num = visibleCellsIdRange.Item2 - visibleCellsIdRange.Item1;
      switch (scrollPositionType)
      {
        case TableView.ScrollPositionType.Center:
          idx -= num / 2;
          break;
        case TableView.ScrollPositionType.End:
          if (idx >= num - 1)
          {
            idx = num - 1;
            break;
          }
          break;
      }
      if (idx < 0)
        idx = 0;
      this._scrollView.ScrollTo((float) idx * this._cellSize, animated);
      if (animated)
        return;
      this.RefreshCells(true, false);
    }

    public virtual void ChangeRectSize(RectTransform.Axis axis, float size)
    {
      ((RectTransform) this.transform).SetSizeWithCurrentAnchors(axis, size);
      Action<TableView> changeRectSizeEvent = this.didChangeRectSizeEvent;
      if (changeRectSizeEvent == null)
        return;
      changeRectSizeEvent(this);
    }

    public enum TableType
    {
      Vertical,
      Horizontal,
    }

    public enum ScrollPositionType
    {
      Beginning,
      Center,
      End,
    }

    [Serializable]
    public class CellsGroup
    {
      [SerializeField]
      protected string _reuseIdentifier;
      [SerializeField]
      protected List<TableCell> _cells;

      public string reuseIdentifier => this._reuseIdentifier;

      public List<TableCell> cells => this._cells;
    }

    public interface IDataSource
    {
      float CellSize();

      int NumberOfCells();

      TableCell CellForIdx(TableView tableView, int idx);
    }
  }
}
