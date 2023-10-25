// Decompiled with JetBrains decompiler
// Type: PS4LeaderboardIdsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PS4LeaderboardIdsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected List<PS4LeaderboardIdsModelSO.LeaderboardIdData> _leaderboardIds = new List<PS4LeaderboardIdsModelSO.LeaderboardIdData>();
  protected Dictionary<string, uint> _leaderboardIdToPs4Id = new Dictionary<string, uint>();

  public List<PS4LeaderboardIdsModelSO.LeaderboardIdData> leaderboardIds => this._leaderboardIds;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._leaderboardIdToPs4Id.Clear();
    foreach (PS4LeaderboardIdsModelSO.LeaderboardIdData leaderboardId in this._leaderboardIds)
      this._leaderboardIdToPs4Id.Add(leaderboardId.leaderboardId, leaderboardId.ps4LeaderboardId);
  }

  public virtual bool GetPS4LeaderboardId(
    IDifficultyBeatmap difficultyBeatmap,
    out uint ps4LeaderboardId)
  {
    ps4LeaderboardId = 0U;
    return this._leaderboardIdToPs4Id.TryGetValue(difficultyBeatmap.SerializedName(), out ps4LeaderboardId);
  }

  [Serializable]
  public class LeaderboardIdData
  {
    [SerializeField]
    protected uint _ps4LeaderboardId;
    [SerializeField]
    protected string _leaderboardId;

    public uint ps4LeaderboardId => this._ps4LeaderboardId;

    public string leaderboardId => this._leaderboardId;

    public LeaderboardIdData(uint ps4LeaderboardId, string leaderboardId)
    {
      this._ps4LeaderboardId = ps4LeaderboardId;
      this._leaderboardId = leaderboardId;
    }
  }
}
