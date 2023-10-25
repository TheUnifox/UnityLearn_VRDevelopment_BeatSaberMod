// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicsTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using UnityEngine;

public class BeatmapCharacteristicsTableView : MonoBehaviour, TableView.IDataSource
{
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected BeatmapCharacteristicTableCell _cellPrefab;
  [SerializeField]
  protected string _cellReuseIdentifier = "Cell";
  [SerializeField]
  protected float _cellWidth = 40f;
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  protected bool _isInitialized;
  protected int _selectedColumn;

  public event System.Action<BeatmapCharacteristicSO> didSelectCharacteristic;

  public virtual void Init()
  {
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
    this._tableView.didSelectCellWithIdxEvent += new System.Action<TableView, int>(this.HandleDidSelectColumnEvent);
  }

  public virtual void SetData(
    BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection)
  {
    this.Init();
    this._beatmapCharacteristicCollection = beatmapCharacteristicCollection;
    this._tableView.ReloadData();
    this._tableView.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, false);
  }

  public virtual void OnDestroy() => this._tableView.didSelectCellWithIdxEvent -= new System.Action<TableView, int>(this.HandleDidSelectColumnEvent);

  public virtual float CellSize() => this._cellWidth;

  public virtual int NumberOfCells() => (UnityEngine.Object) this._beatmapCharacteristicCollection == (UnityEngine.Object) null ? 0 : this._beatmapCharacteristicCollection.beatmapCharacteristics.Length;

  public virtual TableCell CellForIdx(TableView tableView, int idx)
  {
    BeatmapCharacteristicTableCell characteristicTableCell = tableView.DequeueReusableCellForIdentifier(this._cellReuseIdentifier) as BeatmapCharacteristicTableCell;
    if ((UnityEngine.Object) characteristicTableCell == (UnityEngine.Object) null)
    {
      characteristicTableCell = UnityEngine.Object.Instantiate<BeatmapCharacteristicTableCell>(this._cellPrefab);
      characteristicTableCell.reuseIdentifier = this._cellReuseIdentifier;
    }
    BeatmapCharacteristicSO beatmapCharacteristic = this._beatmapCharacteristicCollection.beatmapCharacteristics[idx];
    characteristicTableCell.SetData(beatmapCharacteristic);
    return (TableCell) characteristicTableCell;
  }

  public virtual void HandleDidSelectColumnEvent(TableView tableView, int column)
  {
    this._selectedColumn = column;
    System.Action<BeatmapCharacteristicSO> selectCharacteristic = this.didSelectCharacteristic;
    if (selectCharacteristic == null)
      return;
    selectCharacteristic(this._beatmapCharacteristicCollection.beatmapCharacteristics[column]);
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData()
  {
    this._tableView.ReloadData();
    this._selectedColumn = Math.Min(this._selectedColumn, this.NumberOfCells() - 1);
    this._tableView.SelectCellWithIdx(this._selectedColumn);
  }

  public virtual void SelectCellWithIdx(int idx) => this._tableView.SelectCellWithIdx(idx);
}
