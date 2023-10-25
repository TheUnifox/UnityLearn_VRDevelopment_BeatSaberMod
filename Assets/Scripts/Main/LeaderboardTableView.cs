// Decompiled with JetBrains decompiler
// Type: LeaderboardTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeaderboardTableView : MonoBehaviour, TableView.IDataSource
{
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected LeaderboardTableCell _cellPrefab;
  [SerializeField]
  protected float _rowHeight = 7f;
  protected const string kCellIdentifier = "Cell";
  protected List<LeaderboardTableView.ScoreData> _scores;
  protected int _specialScorePos;

  public virtual float CellSize() => this._rowHeight;

  public virtual int NumberOfCells() => this._scores == null ? 0 : this._scores.Count;

  public virtual TableCell CellForIdx(TableView tableView, int row)
  {
    LeaderboardTableCell leaderboardTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as LeaderboardTableCell;
    if ((Object) leaderboardTableCell == (Object) null)
    {
      leaderboardTableCell = Object.Instantiate<LeaderboardTableCell>(this._cellPrefab);
      leaderboardTableCell.reuseIdentifier = "Cell";
    }
    LeaderboardTableView.ScoreData score = this._scores[row];
    leaderboardTableCell.rank = score.rank;
    leaderboardTableCell.playerName = score.playerName;
    leaderboardTableCell.score = score.score;
    leaderboardTableCell.showFullCombo = score.fullCombo;
    leaderboardTableCell.showSeparator = row != this._scores.Count - 1;
    leaderboardTableCell.specialScore = this._specialScorePos == row;
    return (TableCell) leaderboardTableCell;
  }

  public virtual void SetScores(List<LeaderboardTableView.ScoreData> scores, int specialScorePos)
  {
    this._scores = scores;
    this._specialScorePos = specialScorePos;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
  }

  public class ScoreData
  {
    [CompilerGenerated]
    protected int m_Cscore;
    [CompilerGenerated]
    protected string m_CplayerName;
    [CompilerGenerated]
    protected int m_Crank;
    [CompilerGenerated]
    protected bool m_CfullCombo;

    public int score
    {
      get => this.m_Cscore;
      private set => this.m_Cscore = value;
    }

    public string playerName
    {
      get => this.m_CplayerName;
      private set => this.m_CplayerName = value;
    }

    public int rank
    {
      get => this.m_Crank;
      private set => this.m_Crank = value;
    }

    public bool fullCombo
    {
      get => this.m_CfullCombo;
      private set => this.m_CfullCombo = value;
    }

    public ScoreData(int score, string playerName, int rank, bool fullCombo)
    {
      this.score = score;
      this.playerName = playerName;
      this.rank = rank;
      this.fullCombo = fullCombo;
    }
  }
}
