// Decompiled with JetBrains decompiler
// Type: PS4AchievementIdsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PS4AchievementIdsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected List<PS4AchievementIdsModelSO.AchievementIdData> _achievementsIds = new List<PS4AchievementIdsModelSO.AchievementIdData>();
  protected Dictionary<string, int> _achievementIdToTrophyId = new Dictionary<string, int>();
  protected Dictionary<int, string> _trophyIdToAchievementId = new Dictionary<int, string>();

  public List<PS4AchievementIdsModelSO.AchievementIdData> achievementsIds => this._achievementsIds;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._achievementIdToTrophyId.Clear();
    this._trophyIdToAchievementId.Clear();
    foreach (PS4AchievementIdsModelSO.AchievementIdData achievementsId in this._achievementsIds)
    {
      this._achievementIdToTrophyId.Add(achievementsId.achievementId, achievementsId.ps4TrophyId);
      this._trophyIdToAchievementId.Add(achievementsId.ps4TrophyId, achievementsId.achievementId);
    }
  }

  public virtual bool GetTrophyId(string achievementId, out int trophyId)
  {
    trophyId = 0;
    return this._achievementIdToTrophyId.TryGetValue(achievementId, out trophyId);
  }

  public virtual bool GetAchievementId(int trophyId, out string achievementId)
  {
    achievementId = "";
    return this._trophyIdToAchievementId.TryGetValue(trophyId, out achievementId);
  }

  [Serializable]
  public class AchievementIdData
  {
    [SerializeField]
    protected int _trophyId;
    [SerializeField]
    protected AchievementSO _achievement;

    public int ps4TrophyId => this._trophyId;

    public string achievementId => this._achievement.achievementId;
  }
}
