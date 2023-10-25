// Decompiled with JetBrains decompiler
// Type: ResultsTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;

public class ResultsTableView : MonoBehaviour, TableView.IDataSource
{
  protected const string kCellIdentifier = "Cell";
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected ResultsTableCell _cellPrefab;
  [SerializeField]
  protected float _rowHeight = 6.2f;
  protected IReadOnlyList<MultiplayerPlayerResultsData> _dataList;

  public virtual float CellSize() => this._rowHeight;

  public virtual int NumberOfCells() => this._dataList.Count;

  public virtual TableCell CellForIdx(TableView tableView, int idx)
  {
    ResultsTableCell resultsTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as ResultsTableCell;
    if ((Object) resultsTableCell == (Object) null)
    {
      resultsTableCell = Object.Instantiate<ResultsTableCell>(this._cellPrefab);
      resultsTableCell.reuseIdentifier = "Cell";
    }
    MultiplayerPlayerResultsData data = this._dataList[idx];
    resultsTableCell.SetData(idx + 1, data.connectedPlayer, data.multiplayerLevelCompletionResults.levelCompletionResults);
    return (TableCell) resultsTableCell;
  }

  public virtual void SetData(
    IReadOnlyList<MultiplayerPlayerResultsData> dataList)
  {
    this._dataList = dataList;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
  }
}
