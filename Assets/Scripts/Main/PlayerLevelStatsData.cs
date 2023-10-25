// Decompiled with JetBrains decompiler
// Type: PlayerLevelStatsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class PlayerLevelStatsData
{
  [SerializeField]
  protected int _highScore;
  [SerializeField]
  protected int _maxCombo;
  [SerializeField]
  protected bool _fullCombo;
  [SerializeField]
  protected RankModel.Rank _maxRank;
  [SerializeField]
  protected bool _validScore;
  [SerializeField]
  protected int _playCount;
  [SerializeField]
  protected string _levelID;
  [SerializeField]
  protected BeatmapDifficulty _difficulty;
  [SerializeField]
  protected BeatmapCharacteristicSO _beatmapCharacteristic;

  public string levelID => this._levelID;

  public BeatmapDifficulty difficulty => this._difficulty;

  public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

  public int highScore => this._highScore;

  public int maxCombo => this._maxCombo;

  public bool fullCombo => this._fullCombo;

  public RankModel.Rank maxRank => this._maxRank;

  public bool validScore => this._validScore;

  public int playCount => this._playCount;

  public PlayerLevelStatsData(
    string levelID,
    BeatmapDifficulty difficulty,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    this._levelID = levelID;
    this._difficulty = difficulty;
    this._beatmapCharacteristic = beatmapCharacteristic;
  }

  public PlayerLevelStatsData(
    string levelID,
    BeatmapDifficulty difficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    int highScore,
    int maxCombo,
    bool fullCombo,
    RankModel.Rank maxRank,
    bool validScore,
    int playCount)
  {
    this._levelID = levelID;
    this._difficulty = difficulty;
    this._beatmapCharacteristic = beatmapCharacteristic;
    this._highScore = highScore;
    this._maxCombo = maxCombo;
    this._fullCombo = fullCombo;
    this._maxRank = maxRank;
    this._validScore = validScore;
    this._playCount = playCount;
  }

  public virtual void UpdateScoreData(
    int score,
    int maxCombo,
    bool fullCombo,
    RankModel.Rank rank)
  {
    this._highScore = Mathf.Max(this._highScore, score);
    this._maxCombo = Mathf.Max(this._maxCombo, maxCombo);
    this._fullCombo |= fullCombo;
    this._maxRank = (RankModel.Rank) Mathf.Max((int) this._maxRank, (int) rank);
    this._validScore = true;
  }

  public virtual void IncreaseNumberOfGameplays() => ++this._playCount;
}
