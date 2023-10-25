// Decompiled with JetBrains decompiler
// Type: FileBrowserTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class FileBrowserTableView : MonoBehaviour, TableView.IDataSource
{
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected FileBrowserTableCell _cellPrefab;
  [SerializeField]
  protected float _cellHeight = 12f;
  protected const string kCellIdentifier = "Cell";
  protected FileBrowserItem[] _items;

  public event System.Action<FileBrowserTableView, FileBrowserItem> didSelectRow;

  public virtual void Init(FileBrowserItem[] items)
  {
    this._items = items;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
    this._tableView.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, false);
    this._tableView.didSelectCellWithIdxEvent -= new System.Action<TableView, int>(this.HandleDidSelectRowEvent);
    this._tableView.didSelectCellWithIdxEvent += new System.Action<TableView, int>(this.HandleDidSelectRowEvent);
  }

  public virtual void SetItems(FileBrowserItem[] items)
  {
    this._items = items;
    this._tableView.ReloadData();
    this._tableView.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, false);
  }

  public virtual bool SelectAndScrollRowToItemWithPath(string folderPath)
  {
    int row = 0;
    foreach (FileBrowserItem fileBrowserItem in this._items)
    {
      if (folderPath == fileBrowserItem.fullPath)
      {
        this.SelectAndScrollRow(row);
        return true;
      }
      ++row;
    }
    return false;
  }

  public virtual float CellSize() => this._cellHeight;

  public virtual int NumberOfCells() => this._items == null ? 0 : this._items.Length;

  public virtual TableCell CellForIdx(TableView tableView, int row)
  {
    FileBrowserTableCell browserTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as FileBrowserTableCell;
    if ((UnityEngine.Object) browserTableCell == (UnityEngine.Object) null)
    {
      browserTableCell = UnityEngine.Object.Instantiate<FileBrowserTableCell>(this._cellPrefab);
      browserTableCell.reuseIdentifier = "Cell";
    }
    FileBrowserItem fileBrowserItem = this._items[row];
    browserTableCell.text = fileBrowserItem.displayName;
    return (TableCell) browserTableCell;
  }

  public virtual void HandleDidSelectRowEvent(TableView tableView, int row)
  {
    if (this.didSelectRow == null)
      return;
    this.didSelectRow(this, this._items[row]);
  }

  public virtual void SelectAndScrollRow(int row)
  {
    this._tableView.SelectCellWithIdx(row, true);
    this._tableView.ScrollToCellWithIdx(row, TableView.ScrollPositionType.Beginning, false);
  }

  public virtual void ClearSelection(bool animated = false, bool scrollToRow0 = true)
  {
    this._tableView.ClearSelection();
    if (!scrollToRow0)
      return;
    this._tableView.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, animated);
  }
}
