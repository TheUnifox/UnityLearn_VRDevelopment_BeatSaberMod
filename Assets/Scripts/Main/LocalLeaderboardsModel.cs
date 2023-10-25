// Decompiled with JetBrains decompiler
// Type: LocalLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class LocalLeaderboardsModel : PersistentScriptableObject
{
  [SerializeField]
  protected int _maxNumberOfScoresInLeaderboard = 10;
  protected const string kLocalLeaderboardsFileName = "LocalLeaderboards.dat";
  protected const string kLocalDailyLeaderboardsFileName = "LocalDailyLeaderboards.dat";
  protected Dictionary<LocalLeaderboardsModel.LeaderboardType, int> _lastScorePositions = new Dictionary<LocalLeaderboardsModel.LeaderboardType, int>();
  protected string _lastScoreLeaderboardId;
  protected List<LocalLeaderboardsModel.LeaderboardData> _leaderboardsData;
  protected List<LocalLeaderboardsModel.LeaderboardData> _dailyLeaderboardsData;

  public event System.Action<string, LocalLeaderboardsModel.LeaderboardType> newScoreWasAddedToLeaderboardEvent;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.Load();
  }

  public virtual void OnDisable() => this.Save();

  private static void LoadLeaderboardsData(
    string filename,
    out List<LocalLeaderboardsModel.LeaderboardData> leaderboardsData)
  {
    leaderboardsData = (List<LocalLeaderboardsModel.LeaderboardData>) null;
    string path = Application.persistentDataPath + "/" + filename;
    if (File.Exists(path))
    {
      string json = File.ReadAllText(path);
      if (!string.IsNullOrEmpty(json))
      {
        LocalLeaderboardsModel.SavedLeaderboardsData leaderboardsData1 = (LocalLeaderboardsModel.SavedLeaderboardsData) null;
        try
        {
          leaderboardsData1 = JsonUtility.FromJson<LocalLeaderboardsModel.SavedLeaderboardsData>(json);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogWarning((object) string.Format("Exception in SavedLeaderboardsData json loading:\n{0}", (object) ex));
        }
        if (leaderboardsData1 != null)
          leaderboardsData = leaderboardsData1._leaderboardsData;
      }
    }
    if (leaderboardsData != null)
      return;
    leaderboardsData = new List<LocalLeaderboardsModel.LeaderboardData>();
  }

  private static void SaveLeaderboardsData(
    string filename,
    List<LocalLeaderboardsModel.LeaderboardData> leaderboardsData)
  {
    if (leaderboardsData == null)
      return;
    string json = JsonUtility.ToJson((object) new LocalLeaderboardsModel.SavedLeaderboardsData()
    {
      _leaderboardsData = leaderboardsData
    });
    File.WriteAllText(Application.persistentDataPath + "/" + filename, json);
  }

  [Conditional("OCULUS_QUEST_BUILD")]
  private static void AppendLeaderboardScores(
    List<LocalLeaderboardsModel.ScoreData> main,
    List<LocalLeaderboardsModel.ScoreData> tail,
    int maxNumberOfScores)
  {
    main.AddRange((IEnumerable<LocalLeaderboardsModel.ScoreData>) tail);
    main.Sort((Comparison<LocalLeaderboardsModel.ScoreData>) ((a, b) => b._score - a._score));
    if (main.Count <= maxNumberOfScores)
      return;
    main.RemoveRange(maxNumberOfScores, main.Count - maxNumberOfScores);
  }

  [Conditional("OCULUS_QUEST_BUILD")]
  public static void MigrateQuestLeaderboards(
    List<LocalLeaderboardsModel.LeaderboardData> leaderboardDataList,
    int maxNumberOfScores)
  {
    if (!leaderboardDataList.Any<LocalLeaderboardsModel.LeaderboardData>((Func<LocalLeaderboardsModel.LeaderboardData, bool>) (leaderboardData => leaderboardData._leaderboardId.StartsWith("Quest"))))
      return;
    Dictionary<string, LocalLeaderboardsModel.LeaderboardData> dictionary = new Dictionary<string, LocalLeaderboardsModel.LeaderboardData>();
    for (int index = 0; index < leaderboardDataList.Count; ++index)
    {
      LocalLeaderboardsModel.LeaderboardData leaderboardData = leaderboardDataList[index];
      ref string local = ref leaderboardData._leaderboardId;
      if (local.StartsWith("Quest"))
        local = local.Substring("Quest".Length);
      if (!dictionary.TryGetValue(local, out LocalLeaderboardsModel.LeaderboardData _))
        dictionary[local] = leaderboardData;
      else
        leaderboardDataList[index] = (LocalLeaderboardsModel.LeaderboardData) null;
    }
    leaderboardDataList.RemoveAll((Predicate<LocalLeaderboardsModel.LeaderboardData>) (leaderboardData => leaderboardData == null));
  }

  public virtual void Load()
  {
    LocalLeaderboardsModel.LoadLeaderboardsData("LocalLeaderboards.dat", out this._leaderboardsData);
    LocalLeaderboardsModel.LoadLeaderboardsData("LocalDailyLeaderboards.dat", out this._dailyLeaderboardsData);
    for (int index = 0; index < this._dailyLeaderboardsData.Count; ++index)
      this.UpdateDailyLeaderboard(this._dailyLeaderboardsData[index]._leaderboardId);
  }

  public virtual void Save()
  {
    LocalLeaderboardsModel.SaveLeaderboardsData("LocalLeaderboards.dat", this._leaderboardsData);
    LocalLeaderboardsModel.SaveLeaderboardsData("LocalDailyLeaderboards.dat", this._dailyLeaderboardsData);
  }

  public virtual List<LocalLeaderboardsModel.LeaderboardData> GetLeaderboardsData(
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    List<LocalLeaderboardsModel.LeaderboardData> leaderboardsData = (List<LocalLeaderboardsModel.LeaderboardData>) null;
    switch (leaderboardType)
    {
      case LocalLeaderboardsModel.LeaderboardType.AllTime:
        leaderboardsData = this._leaderboardsData;
        break;
      case LocalLeaderboardsModel.LeaderboardType.Daily:
        leaderboardsData = this._dailyLeaderboardsData;
        break;
    }
    return leaderboardsData;
  }

  public virtual LocalLeaderboardsModel.LeaderboardData GetLeaderboardData(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    List<LocalLeaderboardsModel.LeaderboardData> leaderboardsData = this.GetLeaderboardsData(leaderboardType);
    for (int index = 0; index < leaderboardsData.Count; ++index)
    {
      LocalLeaderboardsModel.LeaderboardData leaderboardData = leaderboardsData[index];
      if (leaderboardData._leaderboardId == leaderboardId)
        return leaderboardData;
    }
    return (LocalLeaderboardsModel.LeaderboardData) null;
  }

  public virtual long GetCurrentTimestamp() => (long) DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

  public virtual void UpdateDailyLeaderboard(string leaderboardId)
  {
    LocalLeaderboardsModel.LeaderboardData leaderboardData = this.GetLeaderboardData(leaderboardId, LocalLeaderboardsModel.LeaderboardType.Daily);
    long num = this.GetCurrentTimestamp() - 86400L;
    if (leaderboardData == null)
      return;
    for (int index = leaderboardData._scores.Count - 1; index >= 0; --index)
    {
      if (leaderboardData._scores[index]._timestamp < num)
        leaderboardData._scores.RemoveAt(index);
    }
  }

  public virtual void AddScore(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType,
    string playerName,
    int score,
    bool fullCombo)
  {
    LocalLeaderboardsModel.LeaderboardData leaderboardData = this.GetLeaderboardData(leaderboardId, leaderboardType);
    int index = 0;
    if (leaderboardData != null)
    {
      List<LocalLeaderboardsModel.ScoreData> scores = leaderboardData._scores;
      index = 0;
      while (index < scores.Count && scores[index]._score >= score)
        ++index;
    }
    else
    {
      leaderboardData = new LocalLeaderboardsModel.LeaderboardData();
      leaderboardData._leaderboardId = leaderboardId;
      leaderboardData._scores = new List<LocalLeaderboardsModel.ScoreData>(this._maxNumberOfScoresInLeaderboard);
      this.GetLeaderboardsData(leaderboardType).Add(leaderboardData);
    }
    if (index < this._maxNumberOfScoresInLeaderboard)
    {
      LocalLeaderboardsModel.ScoreData scoreData = new LocalLeaderboardsModel.ScoreData();
      scoreData._score = score;
      scoreData._playerName = playerName;
      scoreData._fullCombo = fullCombo;
      scoreData._timestamp = this.GetCurrentTimestamp();
      List<LocalLeaderboardsModel.ScoreData> scores = leaderboardData._scores;
      scores.Insert(index, scoreData);
      if (scores.Count > this._maxNumberOfScoresInLeaderboard)
        scores.RemoveAt(scores.Count - 1);
    }
    this._lastScorePositions[leaderboardType] = index;
    this._lastScoreLeaderboardId = leaderboardId;
    if (this.newScoreWasAddedToLeaderboardEvent == null)
      return;
    this.newScoreWasAddedToLeaderboardEvent(leaderboardId, leaderboardType);
  }

  public virtual bool WillScoreGoIntoLeaderboard(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType,
    int score)
  {
    if (leaderboardType == LocalLeaderboardsModel.LeaderboardType.Daily)
      this.UpdateDailyLeaderboard(leaderboardId);
    LocalLeaderboardsModel.LeaderboardData leaderboardData = this.GetLeaderboardData(leaderboardId, leaderboardType);
    if (leaderboardData == null)
      return true;
    List<LocalLeaderboardsModel.ScoreData> scores = leaderboardData._scores;
    return scores.Count < this._maxNumberOfScoresInLeaderboard || scores[scores.Count - 1]._score < score;
  }

  public virtual List<LocalLeaderboardsModel.ScoreData> GetScores(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    return this.GetLeaderboardData(leaderboardId, leaderboardType)?._scores;
  }

  public virtual int GetHighScore(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    LocalLeaderboardsModel.LeaderboardData leaderboardData = this.GetLeaderboardData(leaderboardId, leaderboardType);
    return leaderboardData != null && leaderboardData._scores.Count > 0 ? leaderboardData._scores[0]._score : 0;
  }

  public virtual int GetPositionInLeaderboard(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType,
    int score)
  {
    LocalLeaderboardsModel.LeaderboardData leaderboardData = this.GetLeaderboardData(leaderboardId, leaderboardType);
    if (leaderboardData == null)
      return 0;
    List<LocalLeaderboardsModel.ScoreData> scores = leaderboardData._scores;
    int index = 0;
    while (index < scores.Count && scores[index]._score >= score)
      ++index;
    return index < this._maxNumberOfScoresInLeaderboard ? index : -1;
  }

  public virtual int GetLastScorePosition(
    string leaderboardId,
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    int num;
    return this._lastScoreLeaderboardId == leaderboardId && this._lastScorePositions.TryGetValue(leaderboardType, out num) ? num : -1;
  }

  public virtual void ClearLastScorePosition()
  {
    this._lastScorePositions.Clear();
    this._lastScoreLeaderboardId = (string) null;
  }

  public virtual void AddScore(string leaderboardId, string playerName, int score, bool fullCombo)
  {
    this.AddScore(leaderboardId, LocalLeaderboardsModel.LeaderboardType.AllTime, playerName, score, fullCombo);
    this.AddScore(leaderboardId, LocalLeaderboardsModel.LeaderboardType.Daily, playerName, score, fullCombo);
  }

  public virtual bool WillScoreGoIntoLeaderboard(string leaderboardId, int score) => this.WillScoreGoIntoLeaderboard(leaderboardId, LocalLeaderboardsModel.LeaderboardType.AllTime, score) || this.WillScoreGoIntoLeaderboard(leaderboardId, LocalLeaderboardsModel.LeaderboardType.Daily, score);

  public virtual void ClearLeaderboard(string leaderboardId)
  {
    for (int index = 0; index < this._leaderboardsData.Count; ++index)
    {
      if (this._leaderboardsData[index]._leaderboardId == leaderboardId)
      {
        this._leaderboardsData.RemoveAt(index);
        return;
      }
    }
    for (int index = 0; index < this._dailyLeaderboardsData.Count; ++index)
    {
      if (this._dailyLeaderboardsData[index]._leaderboardId == leaderboardId)
      {
        this._dailyLeaderboardsData.RemoveAt(index);
        break;
      }
    }
  }

  public virtual void ClearAllLeaderboards(bool deleteLeaderboardFile)
  {
    this._leaderboardsData.Clear();
    this._dailyLeaderboardsData.Clear();
    if (!deleteLeaderboardFile)
      return;
    File.Delete(Application.persistentDataPath + "/LocalLeaderboards.dat");
    File.Delete(Application.persistentDataPath + "/LocalDailyLeaderboards.dat");
  }

  public enum LeaderboardType
  {
    AllTime,
    Daily,
  }

  [Serializable]
  public class ScoreData
  {
    public int _score;
    public string _playerName;
    public bool _fullCombo;
    public long _timestamp;
  }

  [Serializable]
  public class LeaderboardData
  {
    public string _leaderboardId;
    public List<LocalLeaderboardsModel.ScoreData> _scores;
  }

  [Serializable]
  public class SavedLeaderboardsData
  {
    public List<LocalLeaderboardsModel.LeaderboardData> _leaderboardsData;
  }
}
