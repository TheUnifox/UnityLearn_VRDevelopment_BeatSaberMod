// Decompiled with JetBrains decompiler
// Type: LeaderboardScoreUploader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeaderboardScoreUploader : MonoBehaviour
{
  protected const string kScoresToUploadFileName = "ScoresToUpload.dat";
  protected List<LeaderboardScoreUploader.ScoreData> _scoresToUpload = new List<LeaderboardScoreUploader.ScoreData>();
  protected List<LeaderboardScoreUploader.ScoreData> _scoresToUploadForCurrentPlayer = new List<LeaderboardScoreUploader.ScoreData>();
  protected LeaderboardScoreUploader.UploadScoreCallback _uploadScoreCallback;
  protected string _playerId;
  protected bool _uploading;

  public event System.Action allScoresDidUploadEvent;

  public virtual void Init(
    LeaderboardScoreUploader.UploadScoreCallback uploadScoreCallback,
    string playerId)
  {
    if (uploadScoreCallback == null || playerId == null)
      return;
    this._uploadScoreCallback = uploadScoreCallback;
    this._playerId = playerId;
    this.StartCoroutine(this.UploadScoresCoroutine());
  }

  public virtual void OnApplicationQuit()
  {
  }

  public virtual IEnumerator UploadScoresCoroutine()
  {
    LeaderboardScoreUploader leaderboardScoreUploader1 = this;
    while (leaderboardScoreUploader1._scoresToUploadForCurrentPlayer.Count > 0)
    {
      LeaderboardScoreUploader leaderboardScoreUploader = leaderboardScoreUploader1;
      LeaderboardScoreUploader.ScoreData scoreData = leaderboardScoreUploader1._scoresToUploadForCurrentPlayer[0];
      ++scoreData.uploadAttemptCount;
      ++scoreData.currentUploadAttemptCount;
      leaderboardScoreUploader1._uploading = true;
      HMAsyncRequest hmAsyncRequest = leaderboardScoreUploader1._uploadScoreCallback(scoreData, (PlatformLeaderboardsModel.UploadScoreCompletionHandler) (result =>
      {
        leaderboardScoreUploader._uploading = false;
        if (result == PlatformLeaderboardsModel.UploadScoreResult.Ok)
        {
          leaderboardScoreUploader._scoresToUploadForCurrentPlayer.RemoveAt(0);
        }
        else
        {
          scoreData = leaderboardScoreUploader._scoresToUploadForCurrentPlayer[0];
          leaderboardScoreUploader._scoresToUploadForCurrentPlayer.RemoveAt(0);
          if (scoreData.currentUploadAttemptCount < 3)
            leaderboardScoreUploader._scoresToUploadForCurrentPlayer.Add(scoreData);
          else
            leaderboardScoreUploader._scoresToUpload.Add(scoreData);
        }
        if (leaderboardScoreUploader._scoresToUploadForCurrentPlayer.Count != 0 || leaderboardScoreUploader.allScoresDidUploadEvent == null)
          return;
        leaderboardScoreUploader.allScoresDidUploadEvent();
      }));
      yield return (object) new WaitUntil(new Func<bool>(leaderboardScoreUploader1.m_CUploadScoresCoroutinem_Eb__14_1));
    }
  }

  public virtual void LoadScoresToUploadFromFile()
  {
    this._scoresToUpload = (List<LeaderboardScoreUploader.ScoreData>) null;
    string path = Application.persistentDataPath + "/ScoresToUpload.dat";
    if (File.Exists(path))
    {
      string json = File.ReadAllText(path);
      if (!string.IsNullOrEmpty(json))
      {
        LeaderboardScoreUploader.ScoresToUploadData scoresToUploadData = (LeaderboardScoreUploader.ScoresToUploadData) null;
        try
        {
          scoresToUploadData = JsonUtility.FromJson<LeaderboardScoreUploader.ScoresToUploadData>(json);
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) string.Format("Exception in ScoresToUploadData json loading:\n{0}", (object) ex));
        }
        if (scoresToUploadData != null)
          this._scoresToUpload = scoresToUploadData.scores;
      }
    }
    if (this._scoresToUpload == null)
      this._scoresToUpload = new List<LeaderboardScoreUploader.ScoreData>();
    this._scoresToUploadForCurrentPlayer = new List<LeaderboardScoreUploader.ScoreData>(this._scoresToUpload.Count);
    for (int index = this._scoresToUpload.Count - 1; index >= 0; --index)
    {
      LeaderboardScoreUploader.ScoreData scoreData = this._scoresToUpload[index];
      if (scoreData.playerId == this._playerId && this._playerId != null)
      {
        this._scoresToUploadForCurrentPlayer.Add(scoreData);
        this._scoresToUpload.RemoveAt(index);
      }
    }
  }

  public virtual void SaveScoresToUploadToFile()
  {
    if (this._scoresToUpload == null)
      return;
    this._scoresToUpload.AddRange((IEnumerable<LeaderboardScoreUploader.ScoreData>) this._scoresToUploadForCurrentPlayer);
    string path = Application.persistentDataPath + "/ScoresToUpload.dat";
    if (this._scoresToUpload.Count > 0)
    {
      string json = JsonUtility.ToJson((object) new LeaderboardScoreUploader.ScoresToUploadData()
      {
        scores = this._scoresToUpload
      });
      File.WriteAllText(path, json);
    }
    else
      File.Delete(path);
  }

  public virtual void AddScore(LeaderboardScoreUploader.ScoreData scoreData)
  {
    if (this._uploadScoreCallback == null || this._playerId == null)
      return;
    if (this._uploading)
      this._scoresToUploadForCurrentPlayer.Insert(1, scoreData);
    else
      this._scoresToUploadForCurrentPlayer.Insert(0, scoreData);
    if (this._scoresToUploadForCurrentPlayer.Count != 1)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine(this.UploadScoresCoroutine());
  }

  [CompilerGenerated]
  public virtual bool m_CUploadScoresCoroutinem_Eb__14_1() => !this._uploading;

  [Serializable]
  public class ScoreData
  {
    [CompilerGenerated]
    protected string m_CplayerId;
    [CompilerGenerated]
    protected IDifficultyBeatmap m_Cbeatmap;
    [CompilerGenerated]
    protected GameplayModifiers m_CgameplayModifiers;
    [CompilerGenerated]
    protected int m_CmultipliedScore;
    [CompilerGenerated]
    protected int m_CmodifiedScore;
    [CompilerGenerated]
    protected bool m_CfullCombo;
    [CompilerGenerated]
    protected int m_CgoodCutsCount;
    [CompilerGenerated]
    protected int m_CbadCutsCount;
    [CompilerGenerated]
    protected int m_CmissedCount;
    [CompilerGenerated]
    protected int m_CmaxCombo;
    public int uploadAttemptCount;
    [NonSerialized]
    public int currentUploadAttemptCount;

    public string playerId
    {
      get => this.m_CplayerId;
      private set => this.m_CplayerId = value;
    }

    public IDifficultyBeatmap beatmap
    {
      get => this.m_Cbeatmap;
      private set => this.m_Cbeatmap = value;
    }

    public GameplayModifiers gameplayModifiers
    {
      get => this.m_CgameplayModifiers;
      private set => this.m_CgameplayModifiers = value;
    }

    public int multipliedScore
    {
      get => this.m_CmultipliedScore;
      private set => this.m_CmultipliedScore = value;
    }

    public int modifiedScore
    {
      get => this.m_CmodifiedScore;
      private set => this.m_CmodifiedScore = value;
    }

    public bool fullCombo
    {
      get => this.m_CfullCombo;
      private set => this.m_CfullCombo = value;
    }

    public int goodCutsCount
    {
      get => this.m_CgoodCutsCount;
      private set => this.m_CgoodCutsCount = value;
    }

    public int badCutsCount
    {
      get => this.m_CbadCutsCount;
      private set => this.m_CbadCutsCount = value;
    }

    public int missedCount
    {
      get => this.m_CmissedCount;
      private set => this.m_CmissedCount = value;
    }

    public int maxCombo
    {
      get => this.m_CmaxCombo;
      private set => this.m_CmaxCombo = value;
    }

    public ScoreData(
      string playerId,
      IDifficultyBeatmap beatmap,
      int multipliedScore,
      int modifiedScore,
      bool fullCombo,
      int goodCutsCount,
      int badCutsCount,
      int missedCount,
      int maxCombo,
      GameplayModifiers gameplayModifiers)
    {
      this.playerId = playerId;
      this.beatmap = beatmap;
      this.multipliedScore = multipliedScore;
      this.modifiedScore = modifiedScore;
      this.gameplayModifiers = gameplayModifiers;
      this.fullCombo = fullCombo;
      this.goodCutsCount = goodCutsCount;
      this.badCutsCount = badCutsCount;
      this.missedCount = missedCount;
      this.maxCombo = maxCombo;
      this.uploadAttemptCount = 0;
      this.currentUploadAttemptCount = 0;
    }
  }

  [Serializable]
  public class ScoresToUploadData
  {
    public List<LeaderboardScoreUploader.ScoreData> scores;
  }

  public delegate HMAsyncRequest UploadScoreCallback(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler);
}
