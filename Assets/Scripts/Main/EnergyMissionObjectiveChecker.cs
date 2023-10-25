// Decompiled with JetBrains decompiler
// Type: EnergyMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class EnergyMissionObjectiveChecker : MissionObjectiveChecker
{
  [Inject]
  protected GameEnergyCounter _energyCounter;

  public virtual void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this._energyCounter)
      return;
    this._energyCounter.gameEnergyDidChangeEvent -= new System.Action<float>(this.HandleEnergyDidChange);
  }

  public virtual void HandleEnergyDidChange(float energy)
  {
    this.checkedValue = (int) ((double) energy * 100.0);
    this.CheckAndUpdateStatus();
  }

  protected override void Init()
  {
    this._energyCounter.gameEnergyDidChangeEvent -= new System.Action<float>(this.HandleEnergyDidChange);
    this._energyCounter.gameEnergyDidChangeEvent += new System.Action<float>(this.HandleEnergyDidChange);
    this.checkedValue = (int) ((double) this._energyCounter.energy * 100.0);
    this.CheckAndUpdateStatus();
  }

  public virtual void CheckAndUpdateStatus()
  {
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min && this.checkedValue >= this._missionObjective.referenceValue || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Max && this.checkedValue <= this._missionObjective.referenceValue || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Equal && this.checkedValue == this._missionObjective.referenceValue)
      this.status = MissionObjectiveChecker.Status.NotFailedYet;
    else
      this.status = MissionObjectiveChecker.Status.NotClearedYet;
  }
}
