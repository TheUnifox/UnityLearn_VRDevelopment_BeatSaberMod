// Decompiled with JetBrains decompiler
// Type: LevelCollectionTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelCollectionTableView : MonoBehaviour, TableView.IDataSource
{
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected AlphabetScrollbar _alphabetScrollbar;
  [SerializeField]
  protected LevelListTableCell _levelCellPrefab;
  [SerializeField]
  protected string _levelCellsReuseIdentifier = "LevelCell";
  [SerializeField]
  protected LevelPackHeaderTableCell _packCellPrefab;
  [SerializeField]
  protected string _packCellsReuseIdentifier = "PackCell";
  [SerializeField]
  protected float _cellHeight = 8.5f;
  [SerializeField]
  protected int _showAlphabetScrollbarLevelCountThreshold = 16;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  [Inject]
  protected readonly BeatmapLevelsPromoDataSO _beatmapLevelsPromoData;
  protected bool _isInitialized;
  protected IReadOnlyList<IPreviewBeatmapLevel> _previewBeatmapLevels;
  protected Sprite _headerSprite;
  protected string _headerText;
  protected bool _showLevelPackHeader = true;
  protected HashSet<string> _favoriteLevelIds;
  protected int _selectedRow = -1;
  protected IPreviewBeatmapLevel _selectedPreviewBeatmapLevel;

  public event System.Action<LevelCollectionTableView, IPreviewBeatmapLevel> didSelectLevelEvent;

  public event System.Action<LevelCollectionTableView> didSelectHeaderEvent;

  public virtual void Init(string headerText, Sprite headerSprite)
  {
    this._showLevelPackHeader = !string.IsNullOrEmpty(headerText);
    this._headerText = headerText;
    this._headerSprite = headerSprite;
    this._previewBeatmapLevels = (IReadOnlyList<IPreviewBeatmapLevel>) null;
  }

  public virtual void Init()
  {
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
    this._tableView.didSelectCellWithIdxEvent += new System.Action<TableView, int>(this.HandleDidSelectRowEvent);
  }

  public virtual void SetData(
    IReadOnlyList<IPreviewBeatmapLevel> previewBeatmapLevels,
    HashSet<string> favoriteLevelIds,
    bool beatmapLevelsAreSorted)
  {
    this.Init();
    this._previewBeatmapLevels = previewBeatmapLevels;
    this._favoriteLevelIds = favoriteLevelIds;
    RectTransform transform = (RectTransform) this._tableView.transform;
    if (beatmapLevelsAreSorted && previewBeatmapLevels.Count > this._showAlphabetScrollbarLevelCountThreshold)
    {
      this._alphabetScrollbar.SetData(AlphabetScrollbarInfoBeatmapLevelHelper.CreateData(previewBeatmapLevels, out this._previewBeatmapLevels));
      transform.offsetMin = new Vector2(((RectTransform) this._alphabetScrollbar.transform).rect.size.x + 1f, 0.0f);
      this._alphabetScrollbar.gameObject.SetActive(true);
    }
    else
    {
      transform.offsetMin = new Vector2(0.0f, 0.0f);
      this._alphabetScrollbar.gameObject.SetActive(false);
    }
    this._tableView.ReloadData();
    this._tableView.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, false);
  }

  public virtual void RefreshFavorites(HashSet<string> favoriteLevelIds)
  {
    this._favoriteLevelIds = favoriteLevelIds;
    if (this._previewBeatmapLevels != null && this._previewBeatmapLevels.Count > 0 && this._selectedPreviewBeatmapLevel != null)
    {
      int num = this._previewBeatmapLevels.IndexOf<IPreviewBeatmapLevel>(this._selectedPreviewBeatmapLevel);
      if (num >= 0)
        this._tableView.SelectCellWithIdx(this._showLevelPackHeader ? num + 1 : num);
    }
    this._tableView.RefreshCellsContent();
  }

  public virtual void OnEnable() => this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.HandleAdditionalContentModelDidInvalidateData);

  public virtual void OnDisable() => this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tableView != (UnityEngine.Object) null))
      return;
    this._tableView.didSelectCellWithIdxEvent -= new System.Action<TableView, int>(this.HandleDidSelectRowEvent);
  }

  public virtual float CellSize() => this._cellHeight;

  public virtual int NumberOfCells()
  {
    if (this._previewBeatmapLevels == null)
      return 0;
    return this._showLevelPackHeader ? this._previewBeatmapLevels.Count + 1 : this._previewBeatmapLevels.Count;
  }

  public virtual TableCell CellForIdx(TableView tableView, int row)
  {
    if (row == 0 && this._showLevelPackHeader)
    {
      LevelPackHeaderTableCell packHeaderTableCell = tableView.DequeueReusableCellForIdentifier(this._packCellsReuseIdentifier) as LevelPackHeaderTableCell;
      if ((UnityEngine.Object) packHeaderTableCell == (UnityEngine.Object) null)
      {
        packHeaderTableCell = UnityEngine.Object.Instantiate<LevelPackHeaderTableCell>(this._packCellPrefab);
        packHeaderTableCell.reuseIdentifier = this._packCellsReuseIdentifier;
      }
      packHeaderTableCell.SetData(this._headerText);
      return (TableCell) packHeaderTableCell;
    }
    LevelListTableCell levelListTableCell = tableView.DequeueReusableCellForIdentifier(this._levelCellsReuseIdentifier) as LevelListTableCell;
    if ((UnityEngine.Object) levelListTableCell == (UnityEngine.Object) null)
    {
      levelListTableCell = UnityEngine.Object.Instantiate<LevelListTableCell>(this._levelCellPrefab);
      levelListTableCell.reuseIdentifier = this._levelCellsReuseIdentifier;
    }
    IPreviewBeatmapLevel previewBeatmapLevel = this._previewBeatmapLevels[this._showLevelPackHeader ? row - 1 : row];
    levelListTableCell.SetDataFromLevelAsync(previewBeatmapLevel, this._favoriteLevelIds.Contains(previewBeatmapLevel.levelID), this._beatmapLevelsPromoData.IsBeatmapLevelPromoted(previewBeatmapLevel), this._beatmapLevelsPromoData.IsBeatmapLevelUpdated(previewBeatmapLevel));
    levelListTableCell.RefreshAvailabilityAsync(this._additionalContentModel, previewBeatmapLevel.levelID);
    return (TableCell) levelListTableCell;
  }

  public virtual void HandleDidSelectRowEvent(TableView tableView, int row)
  {
    this._selectedRow = row;
    if (row == 0 && this._showLevelPackHeader)
    {
      this._selectedPreviewBeatmapLevel = (IPreviewBeatmapLevel) null;
      System.Action<LevelCollectionTableView> selectHeaderEvent = this.didSelectHeaderEvent;
      if (selectHeaderEvent == null)
        return;
      selectHeaderEvent(this);
    }
    else
    {
      this._selectedPreviewBeatmapLevel = this._previewBeatmapLevels[this._showLevelPackHeader ? row - 1 : row];
      System.Action<LevelCollectionTableView, IPreviewBeatmapLevel> selectLevelEvent = this.didSelectLevelEvent;
      if (selectLevelEvent == null)
        return;
      selectLevelEvent(this, this._selectedPreviewBeatmapLevel);
    }
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData()
  {
    this._tableView.ReloadDataKeepingPosition();
    this._selectedRow = Math.Min(this._selectedRow, this.NumberOfCells() - 1);
    this._tableView.SelectCellWithIdx(this._selectedRow);
  }

  public virtual void CancelAsyncOperations()
  {
    foreach (TableCell visibleCell in this._tableView.visibleCells)
    {
      LevelListTableCell levelListTableCell = visibleCell as LevelListTableCell;
      if ((bool) (UnityEngine.Object) levelListTableCell)
        levelListTableCell.CancelAsyncOperations();
    }
  }

  public virtual void RefreshLevelsAvailability()
  {
    if (this._previewBeatmapLevels == null || this._previewBeatmapLevels.Count == 0)
      return;
    IReadOnlyList<IPreviewBeatmapLevel> previewBeatmapLevels = this._previewBeatmapLevels;
    foreach (TableCell visibleCell in this._tableView.visibleCells)
    {
      LevelListTableCell levelListTableCell = visibleCell as LevelListTableCell;
      if ((bool) (UnityEngine.Object) levelListTableCell)
      {
        int index = this._showLevelPackHeader ? levelListTableCell.idx - 1 : levelListTableCell.idx;
        levelListTableCell.RefreshAvailabilityAsync(this._additionalContentModel, previewBeatmapLevels[index].levelID);
      }
    }
  }

  public virtual void SelectLevelPackHeaderCell()
  {
    this._selectedRow = 0;
    this._tableView.SelectCellWithIdx(0);
  }

  public virtual void ClearSelection()
  {
    this._selectedRow = -1;
    this._tableView.SelectCellWithIdx(this._selectedRow);
  }

  public virtual void SelectLevel(IPreviewBeatmapLevel beatmapLevel)
  {
    int idx = -1;
    for (int index = 0; index < this._previewBeatmapLevels.Count; ++index)
    {
      if (this._previewBeatmapLevels[index].levelID == beatmapLevel.levelID)
      {
        idx = this._showLevelPackHeader ? index + 1 : index;
        break;
      }
    }
    if (idx < 0)
      return;
    this._selectedRow = idx;
    this._tableView.SelectCellWithIdx(idx, true);
    this._tableView.ScrollToCellWithIdx(idx, TableView.ScrollPositionType.Center, false);
  }
}
