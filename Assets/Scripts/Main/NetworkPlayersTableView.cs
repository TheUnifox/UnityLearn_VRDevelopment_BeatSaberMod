// Decompiled with JetBrains decompiler
// Type: NetworkPlayersTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayersTableView : MonoBehaviour, TableView.IDataSource
{
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected NetworkPlayerTableCell _playerCellPrefab;
  [SerializeField]
  protected NetworkPlayerOptionsTableCell _optionsCellPrefab;
  [SerializeField]
  protected LevelPackHeaderTableCell _headerCellPrefab;
  [SerializeField]
  protected float _rowHeight = 7f;
  protected const string kPlayerCellIdentifier = "PlayerCell";
  protected const string kHeaderCellIdentifier = "HeaderCell";
  protected const string kOptionsCellIdentifier = "OptionsCell";
  protected List<NetworkPlayersTableView.CellInfo> _cellInfo = new List<NetworkPlayersTableView.CellInfo>();
  protected int _selectedCellIndex = -1;
  protected string _selectedPlayerID;
  protected bool _selectedCellHasOptions;

  public virtual float CellSize() => this._rowHeight;

  public virtual int NumberOfCells() => this._cellInfo.Count;

  public virtual TableCell CellForIdx(TableView tableView, int row)
  {
    NetworkPlayersTableView.CellInfo cellInfo = this._cellInfo[row];
    switch (cellInfo.type)
    {
      case NetworkPlayersTableView.CellInfo.CellType.Header:
        LevelPackHeaderTableCell packHeaderTableCell = tableView.DequeueReusableCellForIdentifier("HeaderCell") as LevelPackHeaderTableCell;
        if ((Object) packHeaderTableCell == (Object) null)
        {
          packHeaderTableCell = Object.Instantiate<LevelPackHeaderTableCell>(this._headerCellPrefab);
          packHeaderTableCell.reuseIdentifier = "HeaderCell";
        }
        packHeaderTableCell.SetData(cellInfo.headerString);
        return (TableCell) packHeaderTableCell;
      case NetworkPlayersTableView.CellInfo.CellType.Player:
        NetworkPlayerTableCell networkPlayerTableCell1 = tableView.DequeueReusableCellForIdentifier("PlayerCell") as NetworkPlayerTableCell;
        if ((Object) networkPlayerTableCell1 == (Object) null)
        {
          networkPlayerTableCell1 = Object.Instantiate<NetworkPlayerTableCell>(this._playerCellPrefab);
          networkPlayerTableCell1.reuseIdentifier = "PlayerCell";
        }
        INetworkPlayer player = cellInfo.player;
        NetworkPlayerTableCell networkPlayerTableCell2 = networkPlayerTableCell1;
        string userName = player.userName;
        int num1 = player.configuration.discoveryPolicy == DiscoveryPolicy.Public ? 1 : 0;
        IConnectedPlayer connectedPlayer = player.connectedPlayer;
        int num2 = connectedPlayer != null ? (connectedPlayer.WantsToPlayNextLevel() ? 1 : 0) : 0;
        int num3 = player.isMyPartyOwner ? 1 : 0;
        int num4 = player.isMe ? 1 : 0;
        networkPlayerTableCell2.SetData(userName, num1 != 0, num2 != 0, num3 != 0, num4 != 0);
        networkPlayerTableCell1.showSeparator = cellInfo.lastCellInParty;
        return (TableCell) networkPlayerTableCell1;
      case NetworkPlayersTableView.CellInfo.CellType.Options:
        NetworkPlayerOptionsTableCell optionsTableCell = tableView.DequeueReusableCellForIdentifier("OptionsCell") as NetworkPlayerOptionsTableCell;
        if ((Object) optionsTableCell == (Object) null)
        {
          optionsTableCell = Object.Instantiate<NetworkPlayerOptionsTableCell>(this._optionsCellPrefab);
          optionsTableCell.reuseIdentifier = "OptionsCell";
        }
        optionsTableCell.player = cellInfo.player;
        return (TableCell) optionsTableCell;
      default:
        return (TableCell) null;
    }
  }

  public virtual void AddPlayers(IEnumerable<INetworkPlayer> players, string title)
  {
    this._cellInfo.Add(new NetworkPlayersTableView.CellInfo()
    {
      type = NetworkPlayersTableView.CellInfo.CellType.Header,
      headerString = title
    });
    foreach (INetworkPlayer player in players)
    {
      int num = !(player.userId == this._selectedPlayerID) ? 0 : (player.userId != null ? 1 : 0);
      this._cellInfo.Add(new NetworkPlayersTableView.CellInfo()
      {
        type = NetworkPlayersTableView.CellInfo.CellType.Player,
        player = player
      });
      if (num != 0)
      {
        this._selectedCellIndex = this._cellInfo.Count - 1;
        this._selectedCellHasOptions = NetworkPlayersTableView.HasVisibleOptions(player);
        if (this._selectedCellHasOptions)
          this._cellInfo.Add(new NetworkPlayersTableView.CellInfo()
          {
            type = NetworkPlayersTableView.CellInfo.CellType.Options,
            player = player
          });
      }
    }
    for (int index = this._cellInfo.Count - 1; index >= 0; --index)
    {
      if (this._cellInfo[index].type == NetworkPlayersTableView.CellInfo.CellType.Player)
      {
        this._cellInfo[index].lastCellInParty = true;
        break;
      }
    }
  }

  public virtual void SetParties(
    IEnumerable<INetworkPlayer> partyPlayers,
    IEnumerable<INetworkPlayer> otherPlayers,
    string myPartyTitle,
    string otherPlayersTitle)
  {
    Debug.Log((object) "Set Parties");
    this._cellInfo.Clear();
    this._selectedCellIndex = -1;
    this.AddPlayers(partyPlayers, myPartyTitle);
    this.AddPlayers(otherPlayers, otherPlayersTitle);
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
    if (this._selectedCellIndex == -1)
      return;
    this._tableView.SelectCellWithIdx(this._selectedCellIndex);
  }

  public virtual void HandleCellWasPressed(TableView tableView, TableCell tableCell)
  {
  }

  private static bool HasVisibleOptions(INetworkPlayer player) => player.canBlock || player.canUnblock || player.canJoin || player.isWaitingOnJoin || player.canInvite || player.isWaitingOnInvite || player.canKick || player.canLeave;

  public class CellInfo
  {
    public NetworkPlayersTableView.CellInfo.CellType type;
    public string headerString;
    public INetworkPlayer player;
    public bool lastCellInParty;

    public enum CellType
    {
      Header,
      Player,
      Options,
    }
  }
}
