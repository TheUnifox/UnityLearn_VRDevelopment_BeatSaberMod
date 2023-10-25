// Decompiled with JetBrains decompiler
// Type: SimpleValueMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class SimpleValueMissionObjectiveChecker : MissionObjectiveChecker
{
  protected void CheckAndUpdateStatus()
  {
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min)
    {
      if (this.checkedValue < this._missionObjective.referenceValue)
        return;
      this.status = MissionObjectiveChecker.Status.Cleared;
    }
    else if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Max)
    {
      if (this.checkedValue <= this._missionObjective.referenceValue)
        return;
      this.status = MissionObjectiveChecker.Status.Failed;
    }
    else
    {
      if (this._missionObjective.referenceValueComparisonType != MissionObjective.ReferenceValueComparisonType.Equal)
        return;
      if (this.status == MissionObjectiveChecker.Status.NotClearedYet)
      {
        if (this.checkedValue != this._missionObjective.referenceValue)
          return;
        this.status = MissionObjectiveChecker.Status.NotFailedYet;
      }
      else
      {
        if (this.status != MissionObjectiveChecker.Status.NotFailedYet || this.checkedValue == this._missionObjective.referenceValue)
          return;
        this.status = MissionObjectiveChecker.Status.Failed;
      }
    }
  }
}
