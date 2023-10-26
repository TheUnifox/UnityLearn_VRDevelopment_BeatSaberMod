// Decompiled with JetBrains decompiler
// Type: HMUI.SegmentedControl
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HMUI
{
  public class SegmentedControl : MonoBehaviour
  {
    [SerializeField]
    [NullAllowed]
    protected Transform _separatorPrefab;
    protected int _numberOfCells;
    protected readonly List<SegmentedControlCell> _cells = new List<SegmentedControlCell>();
    protected readonly List<GameObject> _separators = new List<GameObject>();
    protected SegmentedControl.IDataSource _dataSource;
    protected int _selectedCellNumber = -1;
    protected Dictionary<int, Action<int>> _callbacks = new Dictionary<int, Action<int>>();

    public event Action<SegmentedControl, int> didSelectCellEvent;

    public SegmentedControl.IDataSource dataSource
    {
      get => this._dataSource;
      set
      {
        this._dataSource = value;
        this.ReloadData();
      }
    }

    public int selectedCellNumber => this._selectedCellNumber;

    public IReadOnlyList<SegmentedControlCell> cells => (IReadOnlyList<SegmentedControlCell>) this._cells;

    public virtual void CreateCells()
    {
      Transform transform1 = this.transform;
      for (int cellNumber = 0; cellNumber < this._numberOfCells; ++cellNumber)
      {
        SegmentedControlCell segmentedControlCell = this._dataSource.CellForCellNumber(cellNumber);
        this._cells.Add(segmentedControlCell);
        segmentedControlCell.gameObject.SetActive(true);
        segmentedControlCell.SegmentedControlSetup(this, cellNumber);
        segmentedControlCell.selectionDidChangeEvent -= new Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleCellSelectionDidChange);
        segmentedControlCell.selectionDidChangeEvent += new Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleCellSelectionDidChange);
        segmentedControlCell.SetSelected(this._selectedCellNumber == cellNumber, SelectableCell.TransitionType.Instant, (object) this, true);
        segmentedControlCell.ClearHighlight(SelectableCell.TransitionType.Instant);
        Transform transform2 = segmentedControlCell.transform;
        if ((UnityEngine.Object) transform2.parent != (UnityEngine.Object) transform1)
          transform2.SetParent(transform1, false);
        transform2.localPosition = Vector3.zero;
        transform2.localRotation = Quaternion.identity;
        if (cellNumber < this._numberOfCells - 1 && (UnityEngine.Object) this._separatorPrefab != (UnityEngine.Object) null)
        {
          Transform transform3 = UnityEngine.Object.Instantiate<Transform>(this._separatorPrefab, transform1, false);
          transform3.localPosition = Vector3.zero;
          transform3.localRotation = Quaternion.identity;
          this._separators.Add(transform3.gameObject);
        }
      }
    }

    public virtual void HandleCellSelectionDidChange(
      SelectableCell selectableCell,
      SelectableCell.TransitionType transitionType,
      object changeOwner)
    {
      if (this == (object)changeOwner)
        return;
      SegmentedControlCell segmentedControlCell = (SegmentedControlCell) selectableCell;
      this._cells[this._selectedCellNumber].SetSelected(false, SelectableCell.TransitionType.Instant, (object) this, false);
      this._selectedCellNumber = segmentedControlCell.cellNumber;
      Action<SegmentedControl, int> didSelectCellEvent = this.didSelectCellEvent;
      if (didSelectCellEvent != null)
        didSelectCellEvent(this, segmentedControlCell.cellNumber);
      Action<int> action;
      if (!this._callbacks.TryGetValue(segmentedControlCell.cellNumber, out action) || action == null)
        return;
      action(segmentedControlCell.cellNumber);
    }

    public virtual void SetCallbackForCell(int cellNumber, Action<int> callback) => this._callbacks[cellNumber] = callback;

    public virtual void ReloadData()
    {
      foreach (SegmentedControlCell cell in this._cells)
      {
        if ((UnityEngine.Object) cell != (UnityEngine.Object) null && (UnityEngine.Object) cell.gameObject != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) cell.gameObject);
      }
      this._cells.Clear();
      foreach (UnityEngine.Object separator in this._separators)
        UnityEngine.Object.Destroy(separator);
      this._separators.Clear();
      this._numberOfCells = this._dataSource.NumberOfCells();
      this._selectedCellNumber = 0;
      this.CreateCells();
    }

    public virtual void SelectCellWithNumber(int selectCellNumber)
    {
      this._selectedCellNumber = selectCellNumber;
      for (int index = 0; index < this._numberOfCells; ++index)
        this._cells[index].SetSelected(index == selectCellNumber, SelectableCell.TransitionType.Instant, (object) this, false);
    }

    public interface IDataSource
    {
      int NumberOfCells();

      SegmentedControlCell CellForCellNumber(int cellNumber);
    }
  }
}
