// Decompiled with JetBrains decompiler
// Type: ComboMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class ComboMissionObjectiveChecker : SimpleValueMissionObjectiveChecker
{
  [Inject]
  protected readonly ComboController _comboController;

  protected override void Init()
  {
    this._comboController.comboDidChangeEvent -= new System.Action<int>(this.HandleComboDidChange);
    this._comboController.comboDidChangeEvent += new System.Action<int>(this.HandleComboDidChange);
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Equal)
      this.status = MissionObjectiveChecker.Status.NotClearedYet;
    else
      this.status = MissionObjectiveChecker.Status.NotFailedYet;
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._comboController != (UnityEngine.Object) null))
      return;
    this._comboController.comboDidChangeEvent -= new System.Action<int>(this.HandleComboDidChange);
  }

  public virtual void HandleComboDidChange(int combo)
  {
    if (combo <= this.checkedValue)
      return;
    this.checkedValue = combo;
    this.CheckAndUpdateStatus();
  }
}
