// Decompiled with JetBrains decompiler
// Type: LeaderboardIdsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardIdsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected List<LeaderboardIdsModelSO.LeaderboardIdData> _leaderboardIds = new List<LeaderboardIdsModelSO.LeaderboardIdData>();
  protected readonly Dictionary<string, string> _leaderboardIdMap = new Dictionary<string, string>();

  public virtual void RebuildMap()
  {
    this._leaderboardIdMap.Clear();
    foreach (LeaderboardIdsModelSO.LeaderboardIdData leaderboardId in this._leaderboardIds)
      this._leaderboardIdMap.Add(leaderboardId.difficultyBeatmapId, leaderboardId.platformLeaderboardId);
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.RebuildMap();
  }

  public virtual bool TryGetPlatformLeaderboardId(
    IDifficultyBeatmap difficultyBeatmap,
    out string platformLeaderboardId)
  {
    platformLeaderboardId = string.Empty;
    return this._leaderboardIdMap.TryGetValue(difficultyBeatmap.SerializedName(), out platformLeaderboardId);
  }

  [Serializable]
  public class LeaderboardIdData
  {
    [SerializeField]
    protected string _difficultyBeatmapId;
    [SerializeField]
    protected string _platformLeaderboardId;

    public string platformLeaderboardId => this._platformLeaderboardId;

    public string difficultyBeatmapId => this._difficultyBeatmapId;

    public LeaderboardIdData(string difficultyBeatmapId, string platformLeaderboardId)
    {
      this._difficultyBeatmapId = difficultyBeatmapId;
      this._platformLeaderboardId = platformLeaderboardId;
    }
  }
}
