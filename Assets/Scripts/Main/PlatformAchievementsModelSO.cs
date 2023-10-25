// Decompiled with JetBrains decompiler
// Type: PlatformAchievementsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PlatformAchievementsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected PS4AchievementIdsModelSO _ps4AchievementIdsModel;
  [SerializeField]
  protected AchievementIdsModelSO _achievementIdsModel;
  protected PlatformAchievementsHandler _platformAchievementsHandler;

  private PlatformAchievementsHandler platformAchievementsHandler
  {
    get
    {
      if (this._platformAchievementsHandler != null)
        return this._platformAchievementsHandler;
      this.CreatePlatformAchievementsHandler();
      return this._platformAchievementsHandler;
    }
  }

  public virtual void Initialize() => this.CreatePlatformAchievementsHandler();

  public virtual void CreatePlatformAchievementsHandler()
  {
    if (this._platformAchievementsHandler != null)
      return;
    this._platformAchievementsHandler = (PlatformAchievementsHandler) new OculusPlatformAchievementHandler(this._achievementIdsModel);
  }

  public virtual HMAsyncRequest UnlockAchievement(
    string achievementId,
    PlatformAchievementsModelSO.UnlockAchievementCompletionHandler completionHandler)
  {
    if (this.platformAchievementsHandler != null)
      return this._platformAchievementsHandler.UnlockAchievement(achievementId, completionHandler);
    if (completionHandler != null)
      completionHandler(PlatformAchievementsModelSO.UnlockAchievementResult.Failed);
    return (HMAsyncRequest) null;
  }

  public virtual HMAsyncRequest GetUnlockedAchievements(
    PlatformAchievementsModelSO.GetUnlockedAchievementsCompletionHandler completionHandler)
  {
    if (this.platformAchievementsHandler != null)
      return this._platformAchievementsHandler.GetUnlockedAchievements(completionHandler);
    if (completionHandler != null)
      completionHandler(PlatformAchievementsModelSO.GetUnlockedAchievementsResult.Failed, (string[]) null);
    return (HMAsyncRequest) null;
  }

  public enum UnlockAchievementResult
  {
    OK,
    Failed,
  }

  public enum GetUnlockedAchievementsResult
  {
    OK,
    Failed,
  }

  public delegate void UnlockAchievementCompletionHandler(
    PlatformAchievementsModelSO.UnlockAchievementResult result);

  public delegate void GetUnlockedAchievementsCompletionHandler(
    PlatformAchievementsModelSO.GetUnlockedAchievementsResult result,
    string[] unlockedAchievementsIds);
}
