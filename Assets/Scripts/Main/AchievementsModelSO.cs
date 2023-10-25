// Decompiled with JetBrains decompiler
// Type: AchievementsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AchievementsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected PlatformAchievementsModelSO _platformAchievementsModel;
  protected HashSet<string> _unlockedAchievementIds = new HashSet<string>();
  protected bool _initialized;

  public virtual void Initialize()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._platformAchievementsModel.Initialize();
    this._platformAchievementsModel.GetUnlockedAchievements((PlatformAchievementsModelSO.GetUnlockedAchievementsCompletionHandler) ((result, achievementIds) =>
    {
      if (result != PlatformAchievementsModelSO.GetUnlockedAchievementsResult.OK)
        return;
      foreach (string achievementId in achievementIds)
        this._unlockedAchievementIds.Add(achievementId);
    }));
  }

  public virtual void UnlockAchievement(AchievementSO achievement)
  {
    if (!this._initialized)
      this.Initialize();
    string achievementId = achievement.achievementId;
    if (this._unlockedAchievementIds.Contains(achievementId))
      return;
    this._platformAchievementsModel.UnlockAchievement(achievementId, (PlatformAchievementsModelSO.UnlockAchievementCompletionHandler) (result =>
    {
      if (result != PlatformAchievementsModelSO.UnlockAchievementResult.OK)
        return;
      this._unlockedAchievementIds.Add(achievementId);
    }));
  }

  [CompilerGenerated]
  public virtual void m_CInitializem_Eb__3_0(
    PlatformAchievementsModelSO.GetUnlockedAchievementsResult result,
    string[] achievementIds)
  {
    if (result != PlatformAchievementsModelSO.GetUnlockedAchievementsResult.OK)
      return;
    foreach (string achievementId in achievementIds)
      this._unlockedAchievementIds.Add(achievementId);
  }
}
