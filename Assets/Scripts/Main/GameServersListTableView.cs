// Decompiled with JetBrains decompiler
// Type: GameServersListTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameServersListTableView : MonoBehaviour, TableViewWithDetailCell.IDataSource
{
  protected const string kCellReuseIdentifier = "Cell";
  protected const string kDetailCellReuseIdentifier = "DetailCell";
  [SerializeField]
  protected TableViewWithDetailCell _tableView;
  [SerializeField]
  protected GameServerListTableCell _gameServerListCellPrefab;
  [SerializeField]
  protected GameServerListDetailTableCell _gameServerDetailCellPrefab;
  [Inject]
  protected readonly DiContainer _container;
  protected bool _isInitialized;
  protected INetworkPlayer[] _gamesList;
  protected INetworkPlayer _selectedServer;

  public event System.Action<INetworkPlayer> joinButtonPressedEvent;

  public virtual float CellSize() => 8.2f;

  public virtual int NumberOfCells()
  {
    INetworkPlayer[] gamesList = this._gamesList;
    return gamesList == null ? 0 : gamesList.Length;
  }

  public virtual void Init()
  {
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    this._tableView.dataSource = (TableViewWithDetailCell.IDataSource) this;
    this._tableView.didSelectContentCellEvent += new System.Action<TableViewWithDetailCell, int>(this.HandleTableViewDidSelectCellWithIdx);
    this._tableView.didDeselectContentCellEvent += new System.Action<TableViewWithDetailCell, int>(this.HandleTableViewDidDeselectCellWithIdx);
  }

  public virtual TableCell CellForContent(
    TableViewWithDetailCell tableView,
    int idx,
    bool detailOpened)
  {
    GameServerListTableCell serverListTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as GameServerListTableCell;
    if ((UnityEngine.Object) serverListTableCell == (UnityEngine.Object) null)
    {
      serverListTableCell = this._container.InstantiatePrefab((UnityEngine.Object) this._gameServerListCellPrefab).GetComponent<GameServerListTableCell>();
      serverListTableCell.reuseIdentifier = "Cell";
    }
    serverListTableCell.SetData(this._gamesList[idx]);
    return (TableCell) serverListTableCell;
  }

  public virtual TableCell CellForDetail(TableViewWithDetailCell tableView, int contentIdx)
  {
    GameServerListDetailTableCell listDetailTableCell = tableView.DequeueReusableCellForIdentifier("DetailCell") as GameServerListDetailTableCell;
    if ((UnityEngine.Object) listDetailTableCell == (UnityEngine.Object) null)
    {
      listDetailTableCell = UnityEngine.Object.Instantiate<GameServerListDetailTableCell>(this._gameServerDetailCellPrefab);
      listDetailTableCell.reuseIdentifier = "DetailCell";
    }
    listDetailTableCell.joinServerButtonWasPressedEvent -= new System.Action(this.HandleGameServerListDetailTableCellJoinServerButtonWasPressed);
    listDetailTableCell.joinServerButtonWasPressedEvent += new System.Action(this.HandleGameServerListDetailTableCellJoinServerButtonWasPressed);
    return (TableCell) listDetailTableCell;
  }

  public virtual void SetData(IEnumerable<INetworkPlayer> servers, bool clearSelection)
  {
    if (servers == null)
      return;
    this.Init();
    if (!(servers is INetworkPlayer[] networkPlayerArray1))
      networkPlayerArray1 = servers.ToArray<INetworkPlayer>();
    INetworkPlayer[] networkPlayerArray2 = networkPlayerArray1;
    if (clearSelection)
      this._tableView.ClearSelection();
    this._gamesList = networkPlayerArray2;
    int currentNewIndex = -1;
    if (this._selectedServer != null)
      currentNewIndex = Array.IndexOf<INetworkPlayer>(this._gamesList, this._selectedServer);
    this._selectedServer = currentNewIndex != -1 ? this._selectedServer : (INetworkPlayer) null;
    this._tableView.ReloadData(currentNewIndex);
  }

  public virtual void HandleGameServerListDetailTableCellJoinServerButtonWasPressed()
  {
    if (this._selectedServer == null)
      return;
    System.Action<INetworkPlayer> buttonPressedEvent = this.joinButtonPressedEvent;
    if (buttonPressedEvent == null)
      return;
    buttonPressedEvent(this._selectedServer);
  }

  public virtual void HandleTableViewDidSelectCellWithIdx(TableView tableView, int id) => this._selectedServer = this._gamesList[id];

  public virtual void HandleTableViewDidDeselectCellWithIdx(TableViewWithDetailCell arg1, int arg2) => this._selectedServer = (INetworkPlayer) null;

  public virtual void OnDestroy()
  {
    this._tableView.didSelectCellWithIdxEvent -= new System.Action<TableView, int>(this.HandleTableViewDidSelectCellWithIdx);
    this._tableView.didDeselectContentCellEvent -= new System.Action<TableViewWithDetailCell, int>(this.HandleTableViewDidDeselectCellWithIdx);
  }
}
