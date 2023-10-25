// Decompiled with JetBrains decompiler
// Type: DifficultyTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class DifficultyTableView : MonoBehaviour, TableView.IDataSource
{
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected DifficultyTableCell _cellPrefab;
  [SerializeField]
  protected DifficultyTableCell _nonSelectableCellPrefab;
  [SerializeField]
  protected float _cellHeight = 8f;
  protected const string kCellIdentifier = "Cell";
  protected const string kNonSelectableCellIdentifier = "NonSelectableCell";
  protected IDifficultyBeatmap[] _difficultyBeatmaps;

  public event System.Action<DifficultyTableView, int> didSelectRow;

  public virtual void Init(IDifficultyBeatmap[] difficultyBeatmaps)
  {
    this._difficultyBeatmaps = difficultyBeatmaps;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
    this._tableView.didSelectCellWithIdxEvent -= new System.Action<TableView, int>(this.HandleDidSelectRowEvent);
    this._tableView.didSelectCellWithIdxEvent += new System.Action<TableView, int>(this.HandleDidSelectRowEvent);
  }

  public virtual void SetDifficultyBeatmaps(IDifficultyBeatmap[] difficultyBeatmaps)
  {
    this._difficultyBeatmaps = difficultyBeatmaps;
    this._tableView.ReloadData();
  }

  public virtual float CellSize() => this._cellHeight;

  public virtual int NumberOfCells() => this._difficultyBeatmaps == null || this._difficultyBeatmaps.Length == 0 ? 1 : this._difficultyBeatmaps.Length;

  public virtual TableCell CellForIdx(TableView tableView, int row)
  {
    if (this._difficultyBeatmaps == null || this._difficultyBeatmaps.Length == 0)
    {
      DifficultyTableCell difficultyTableCell = tableView.DequeueReusableCellForIdentifier("NonSelectableCell") as DifficultyTableCell;
      if ((UnityEngine.Object) difficultyTableCell == (UnityEngine.Object) null)
      {
        difficultyTableCell = UnityEngine.Object.Instantiate<DifficultyTableCell>(this._nonSelectableCellPrefab);
        difficultyTableCell.reuseIdentifier = "NonSelectableCell";
      }
      difficultyTableCell.difficultyText = "Empty";
      difficultyTableCell.difficultyValue = 0;
      return (TableCell) difficultyTableCell;
    }
    DifficultyTableCell difficultyTableCell1 = this._tableView.DequeueReusableCellForIdentifier("Cell") as DifficultyTableCell;
    if ((UnityEngine.Object) difficultyTableCell1 == (UnityEngine.Object) null)
    {
      difficultyTableCell1 = UnityEngine.Object.Instantiate<DifficultyTableCell>(this._cellPrefab);
      difficultyTableCell1.reuseIdentifier = "Cell";
    }
    IDifficultyBeatmap difficultyBeatmap = this._difficultyBeatmaps[row];
    difficultyTableCell1.difficultyText = difficultyBeatmap.difficulty.Name();
    difficultyTableCell1.difficultyValue = difficultyBeatmap.difficultyRank;
    return (TableCell) difficultyTableCell1;
  }

  public virtual void HandleDidSelectRowEvent(TableView tableView, int row)
  {
    if (this._difficultyBeatmaps == null || this._difficultyBeatmaps.Length == 0)
    {
      this._tableView.ClearSelection();
    }
    else
    {
      if (this.didSelectRow == null)
        return;
      this.didSelectRow(this, row);
    }
  }

  public virtual void SelectRow(int row, bool callbackTable) => this._tableView.SelectCellWithIdx(row, callbackTable);

  public virtual void SelectRow(IDifficultyBeatmap difficultyBeatmap, bool callbackTable)
  {
    for (int row = 0; row < this._difficultyBeatmaps.Length; ++row)
    {
      if (difficultyBeatmap == this._difficultyBeatmaps[row])
        this.SelectRow(row, callbackTable);
    }
  }

  public virtual void ClearSelection() => this._tableView.ClearSelection();
}
