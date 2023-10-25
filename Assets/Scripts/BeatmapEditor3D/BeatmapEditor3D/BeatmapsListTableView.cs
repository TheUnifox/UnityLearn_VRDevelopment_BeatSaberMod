// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapsListTableView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapsListTableView : MonoBehaviour, TableView.IDataSource
  {
    private const string kCellIdentifier = "Cell";
    [SerializeField]
    private TableView _tableView;
    [SerializeField]
    private float _rowHeight = 8f;
    [Inject]
    private readonly BeatmapsListTableCell.Factory _beatmapsListTableCellFactory;
    private IReadOnlyList<IBeatmapInfoData> _beatmapInfos;

    public event Action<int> openBeatmapEvent;

    public float CellSize() => this._rowHeight;

    public int NumberOfCells() => this._beatmapInfos.Count;

    public TableCell CellForIdx(TableView tableView, int idx)
    {
      BeatmapsListTableCell beatmapsListTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as BeatmapsListTableCell;
      if ((UnityEngine.Object) beatmapsListTableCell == (UnityEngine.Object) null)
      {
        beatmapsListTableCell = this._beatmapsListTableCellFactory.Create();
        beatmapsListTableCell.reuseIdentifier = "Cell";
      }
      beatmapsListTableCell.SetData(idx, this._beatmapInfos[idx]);
      beatmapsListTableCell.openBeatmapButtonPressedEvent -= new Action<int>(this.HandleCellOpenBeatmapButtonPressed);
      beatmapsListTableCell.openBeatmapButtonPressedEvent += new Action<int>(this.HandleCellOpenBeatmapButtonPressed);
      return (TableCell) beatmapsListTableCell;
    }

    public void SetData(IReadOnlyList<IBeatmapInfoData> beatmapInfos)
    {
      this._beatmapInfos = beatmapInfos;
      this._tableView.SetDataSource((TableView.IDataSource) this, true);
    }

    private void HandleCellOpenBeatmapButtonPressed(int idx)
    {
      Action<int> openBeatmapEvent = this.openBeatmapEvent;
      if (openBeatmapEvent == null)
        return;
      openBeatmapEvent(idx);
    }
  }
}
