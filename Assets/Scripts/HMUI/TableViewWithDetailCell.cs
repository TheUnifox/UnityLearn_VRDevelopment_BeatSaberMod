// Decompiled with JetBrains decompiler
// Type: TableViewWithDetailCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using HMUI;
using System;

public class TableViewWithDetailCell : TableView, TableView.IDataSource
{
  protected new TableViewWithDetailCell.IDataSource _dataSource;
  protected int _selectedId = -1;

  public event Action<TableViewWithDetailCell, int> didSelectContentCellEvent;

  public event Action<TableViewWithDetailCell, int> didDeselectContentCellEvent;

  public new TableViewWithDetailCell.IDataSource dataSource
  {
    get => this._dataSource;
    set
    {
      if (this._dataSource == value)
        return;
      this._dataSource = value;
      base._dataSource = (TableView.IDataSource) this;
      this.ReloadData();
    }
  }

  public virtual float CellSize() => this._dataSource.CellSize();

  public virtual int NumberOfCells() => this._dataSource.NumberOfCells() + (this._selectedId != -1 ? 1 : 0);

  public virtual TableCell CellForIdx(TableView tableView, int idx)
  {
    if (this._selectedId != -1 && idx == this._selectedId + 1)
      return this._dataSource.CellForDetail(this, idx - 1);
    bool detailOpened = this._selectedId == idx;
    return this._selectedId == -1 || idx <= this._selectedId ? this._dataSource.CellForContent(this, idx, detailOpened) : this._dataSource.CellForContent(this, idx - 1, detailOpened);
  }

  public override void ReloadData() => this.ReloadData(-1);

  public virtual void ReloadData(int currentNewIndex)
  {
    this._selectedId = currentNewIndex;
    if (this._selectedId == -1)
      this.ClearSelection();
    else
      this.SelectCellWithIdx(this._selectedId);
    base.ReloadData();
  }

  protected override void DidSelectCellWithIdx(int idx)
  {
    bool flag = false;
    if (this._selectedId == -1)
    {
      flag = true;
      this._selectedId = idx;
    }
    else if (this._selectedId == idx - 1)
      this._selectedId = -1;
    else if (this._selectedId != idx)
    {
      flag = true;
      if (idx > this._selectedId)
        --idx;
      this._selectedId = idx;
    }
    else
      this._selectedId = -1;
    int num1;
    int num2;
    TupleExtensions.Deconstruct<int, int>(this.GetVisibleCellsIdRange(), out num1, out num2);
    int num3 = num2;
    this.ReloadData(this._selectedId);
    if (this._selectedId >= num3)
      this.scrollView.ScrollTo((float) (this._selectedId + 1) * this.cellSize, true);
    if (flag)
    {
      Action<TableViewWithDetailCell, int> contentCellEvent = this.didSelectContentCellEvent;
      if (contentCellEvent == null)
        return;
      contentCellEvent(this, idx);
    }
    else
    {
      if (this._selectedId != -1)
        return;
      Action<TableViewWithDetailCell, int> contentCellEvent = this.didDeselectContentCellEvent;
      if (contentCellEvent == null)
        return;
      contentCellEvent(this, idx);
    }
  }

  public new interface IDataSource
  {
    float CellSize();

    int NumberOfCells();

    TableCell CellForContent(TableViewWithDetailCell tableView, int idx, bool detailOpened);

    TableCell CellForDetail(TableViewWithDetailCell tableView, int contentIdx);
  }
}
