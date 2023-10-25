// Decompiled with JetBrains decompiler
// Type: ScoreMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class ScoreMissionObjectiveChecker : SimpleValueMissionObjectiveChecker
{
  [Inject]
  protected readonly IScoreController _scoreController;

  public virtual void OnDestroy()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.scoreDidChangeEvent -= new System.Action<int, int>(this.HandleScoreDidChange);
  }

  public virtual void HandleScoreDidChange(int multipliedScore, int modifiedScore)
  {
    this.checkedValue = multipliedScore;
    this.CheckAndUpdateStatus();
  }

  protected override void Init()
  {
    this._scoreController.scoreDidChangeEvent -= new System.Action<int, int>(this.HandleScoreDidChange);
    this._scoreController.scoreDidChangeEvent += new System.Action<int, int>(this.HandleScoreDidChange);
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Equal)
      this.status = MissionObjectiveChecker.Status.NotClearedYet;
    else
      this.status = MissionObjectiveChecker.Status.NotFailedYet;
  }
}
