// Decompiled with JetBrains decompiler
// Type: HandsMovementMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class HandsMovementMissionObjectiveChecker : SimpleValueMissionObjectiveChecker
{
  [Inject]
  protected SaberActivityCounter _saberActivityCounter;

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._saberActivityCounter != (UnityEngine.Object) null))
      return;
    this._saberActivityCounter.totalDistanceDidChangeEvent -= new System.Action<float>(this.HandleTotalDistanceDidChange);
  }

  public virtual void HandleTotalDistanceDidChange(float distance)
  {
    this.checkedValue = (int) distance;
    this.CheckAndUpdateStatus();
  }

  protected override void Init()
  {
    this._saberActivityCounter.totalDistanceDidChangeEvent += new System.Action<float>(this.HandleTotalDistanceDidChange);
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Equal)
      this.status = MissionObjectiveChecker.Status.NotClearedYet;
    else
      this.status = MissionObjectiveChecker.Status.NotFailedYet;
  }
}
