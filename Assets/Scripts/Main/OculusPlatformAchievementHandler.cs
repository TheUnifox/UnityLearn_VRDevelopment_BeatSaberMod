// Decompiled with JetBrains decompiler
// Type: OculusPlatformAchievementHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;

public class OculusPlatformAchievementHandler : PlatformAchievementsHandler
{
  protected readonly AchievementIdsModelSO _achievementIdsModel;

  public OculusPlatformAchievementHandler(AchievementIdsModelSO achievementIdsModel) => this._achievementIdsModel = achievementIdsModel;

  public override HMAsyncRequest UnlockAchievement(
    string achievementId,
    PlatformAchievementsModelSO.UnlockAchievementCompletionHandler completionHandler)
  {
    Achievements.Unlock(achievementId).OnComplete((Message<AchievementUpdate>.Callback) (message =>
    {
      if (message.IsError)
        completionHandler(PlatformAchievementsModelSO.UnlockAchievementResult.Failed);
      else
        completionHandler(PlatformAchievementsModelSO.UnlockAchievementResult.OK);
    }));
    return (HMAsyncRequest) null;
  }

  public override HMAsyncRequest GetUnlockedAchievements(
    PlatformAchievementsModelSO.GetUnlockedAchievementsCompletionHandler completionHandler)
  {
    Achievements.GetAllProgress().OnComplete((Message<AchievementProgressList>.Callback) (message =>
    {
      if (message.IsError)
        completionHandler(PlatformAchievementsModelSO.GetUnlockedAchievementsResult.Failed, (string[]) null);
      List<string> stringList = new List<string>();
      foreach (AchievementProgress achievementProgress in (DeserializableList<AchievementProgress>) message.Data)
      {
        AchievementProgress achievement = achievementProgress;
        if (achievement.IsUnlocked && this._achievementIdsModel.achievementsIds.Exists((Predicate<AchievementSO>) (x => x.achievementId == achievement.Name)))
          stringList.Add(achievement.Name);
      }
      completionHandler(PlatformAchievementsModelSO.GetUnlockedAchievementsResult.OK, stringList.ToArray());
    }));
    return (HMAsyncRequest) null;
  }
}
