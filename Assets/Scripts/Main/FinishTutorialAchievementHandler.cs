// Decompiled with JetBrains decompiler
// Type: FinishTutorialAchievementHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class FinishTutorialAchievementHandler : MonoBehaviour
{
  [SerializeField]
  protected AchievementsModelSO _achievementsModel;
  [Space]
  [SerializeField]
  protected Signal _tutorialFinishedSignal;
  [SerializeField]
  protected AchievementSO _finishTutorialAchievement;

  public virtual void Start() => this._tutorialFinishedSignal.Subscribe(new System.Action(this.HandleTutorialFinished));

  public virtual void OnDestroy() => this._tutorialFinishedSignal.Unsubscribe(new System.Action(this.HandleTutorialFinished));

  public virtual void HandleTutorialFinished() => this._achievementsModel.UnlockAchievement(this._finishTutorialAchievement);
}
