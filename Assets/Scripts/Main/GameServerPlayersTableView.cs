// Decompiled with JetBrains decompiler
// Type: GameServerPlayersTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GameServerPlayersTableView : MonoBehaviour, TableView.IDataSource
{
  protected const string kCellId = "Cell";
  protected const string kNoSongsCellId = "NoSongCell";
  protected const string kNoModifiersCellId = "NoModifierCell";
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected GameServerPlayerTableCell _gameServerPlayerCellPrefab;
  [SerializeField]
  protected GameServerPlayerTableCell _gameServerPlayerCellWithoutSongsPrefab;
  [SerializeField]
  protected GameServerPlayerTableCell _gameServerPlayerCellWithoutModifiersPrefab;
  [SerializeField]
  protected GameObject _tableHeaderSongGo;
  [SerializeField]
  protected GameObject _tableHeaderModifiersGo;
  [Inject]
  protected readonly DiContainer _container;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  protected bool _initialized;
  protected bool _hasKickPermissions;
  protected bool _allowSelection = true;
  protected bool _showSongSelection = true;
  protected bool _showModifierSelection = true;
  protected IConnectedPlayer _selectedPlayer;
  protected List<IConnectedPlayer> _sortedConnectedPlayers;
  protected ILobbyPlayersDataModel _lobbyPlayersDataModel;

  public event System.Action<PreviewDifficultyBeatmap> selectSuggestedLevelEvent;

  public event System.Action<GameplayModifiers> selectSuggestedGameplayModifiersEvent;

  public event System.Action<string> kickPlayerEvent;

  public virtual float CellSize() => 8.2f;

  public virtual int NumberOfCells() => this._sortedConnectedPlayers.Count;

  private string currentCellId
  {
    get
    {
      if (!this._showSongSelection)
        return "NoSongCell";
      return !this._showModifierSelection ? "NoModifierCell" : "Cell";
    }
  }

  public virtual GameServerPlayerTableCell GetCurrentPrefab()
  {
    switch (this.currentCellId)
    {
      case "NoModifierCell":
        return this._gameServerPlayerCellWithoutModifiersPrefab;
      case "NoSongCell":
        return this._gameServerPlayerCellWithoutSongsPrefab;
      default:
        return this._gameServerPlayerCellPrefab;
    }
  }

  public virtual TableCell CellForIdx(TableView tableView, int idx)
  {
    GameServerPlayerTableCell serverPlayerTableCell = tableView.DequeueReusableCellForIdentifier(this.currentCellId) as GameServerPlayerTableCell;
    if ((UnityEngine.Object) serverPlayerTableCell == (UnityEngine.Object) null)
    {
      serverPlayerTableCell = this._container.InstantiatePrefab((UnityEngine.Object) this.GetCurrentPrefab()).GetComponent<GameServerPlayerTableCell>();
      serverPlayerTableCell.reuseIdentifier = this.currentCellId;
    }
    IConnectedPlayer player;
    ILobbyPlayerData playerData;
    if (!this.TryGetLobbyPlayerData(idx, out player, out playerData))
      return (TableCell) serverPlayerTableCell;
    serverPlayerTableCell.interactable = !player.isMe;
    Task<AdditionalContentModel.EntitlementStatus> entitlementStatusAsync = playerData.beatmapLevel != (PreviewDifficultyBeatmap) null ? this._additionalContentModel.GetLevelEntitlementStatusAsync(playerData.beatmapLevel.beatmapLevel.levelID, new CancellationToken()) : (Task<AdditionalContentModel.EntitlementStatus>) null;
    serverPlayerTableCell.SetData(player, playerData, this._hasKickPermissions, this._allowSelection, entitlementStatusAsync);
    serverPlayerTableCell.kickPlayerEvent -= new System.Action<int>(this.HandleCellKickPlayer);
    serverPlayerTableCell.kickPlayerEvent += new System.Action<int>(this.HandleCellKickPlayer);
    serverPlayerTableCell.useBeatmapEvent -= new System.Action<int>(this.HandleCellUseBeatmap);
    serverPlayerTableCell.useBeatmapEvent += new System.Action<int>(this.HandleCellUseBeatmap);
    serverPlayerTableCell.useModifiersEvent -= new System.Action<int>(this.HandleCellUseModifiers);
    serverPlayerTableCell.useModifiersEvent += new System.Action<int>(this.HandleCellUseModifiers);
    return (TableCell) serverPlayerTableCell;
  }

  public virtual bool TryGetLobbyPlayerData(
    int idx,
    out IConnectedPlayer player,
    out ILobbyPlayerData playerData)
  {
    player = this._sortedConnectedPlayers[idx];
    return this._lobbyPlayersDataModel.TryGetValue(player.userId, out playerData);
  }

  public virtual void HandleCellUseBeatmap(int idx)
  {
    ILobbyPlayerData lobbyPlayerData = this._lobbyPlayersDataModel[this._sortedConnectedPlayers[idx].userId];
    if (lobbyPlayerData?.beatmapLevel == (PreviewDifficultyBeatmap) null)
      return;
    System.Action<PreviewDifficultyBeatmap> suggestedLevelEvent = this.selectSuggestedLevelEvent;
    if (suggestedLevelEvent == null)
      return;
    suggestedLevelEvent(lobbyPlayerData.beatmapLevel);
  }

  public virtual void HandleCellUseModifiers(int idx)
  {
    ILobbyPlayerData lobbyPlayerData = this._lobbyPlayersDataModel[this._sortedConnectedPlayers[idx].userId];
    if (lobbyPlayerData?.gameplayModifiers == null)
      return;
    System.Action<GameplayModifiers> gameplayModifiersEvent = this.selectSuggestedGameplayModifiersEvent;
    if (gameplayModifiersEvent == null)
      return;
    gameplayModifiersEvent(lobbyPlayerData.gameplayModifiers);
  }

  public virtual void HandleCellKickPlayer(int idx)
  {
    IConnectedPlayer sortedConnectedPlayer = this._sortedConnectedPlayers[idx];
    System.Action<string> kickPlayerEvent = this.kickPlayerEvent;
    if (kickPlayerEvent == null)
      return;
    kickPlayerEvent(sortedConnectedPlayer.userId);
  }

  public virtual void SetData(
    List<IConnectedPlayer> sortedPlayers,
    ILobbyPlayersDataModel lobbyPlayersDataModel,
    bool hasKickPermissions,
    bool allowSelection,
    bool showSongSelection,
    bool showModifierSelection,
    bool clearSelection = false)
  {
    if (clearSelection)
      this._tableView.ClearSelection();
    this._sortedConnectedPlayers = sortedPlayers;
    this._lobbyPlayersDataModel = lobbyPlayersDataModel;
    this._hasKickPermissions = hasKickPermissions;
    this._allowSelection = allowSelection;
    this._showSongSelection = showSongSelection;
    this._showModifierSelection = showModifierSelection;
    this._tableHeaderSongGo.SetActive(this._showSongSelection);
    this._tableHeaderModifiersGo.SetActive(this._showModifierSelection);
    this.Init();
    this._tableView.ReloadData();
  }

  public virtual void Init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._tableView.SetDataSource((TableView.IDataSource) this, false);
  }
}
