// Decompiled with JetBrains decompiler
// Type: FullComboMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class FullComboMissionObjectiveChecker : MissionObjectiveChecker
{
  [Inject]
  protected readonly ComboController _comboController;

  protected override void Init()
  {
    this.status = MissionObjectiveChecker.Status.NotFailedYet;
    this._comboController.comboBreakingEventHappenedEvent -= new System.Action(this.HandleComboBreakingEventHappened);
    this._comboController.comboBreakingEventHappenedEvent += new System.Action(this.HandleComboBreakingEventHappened);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._comboController != (UnityEngine.Object) null))
      return;
    this._comboController.comboBreakingEventHappenedEvent -= new System.Action(this.HandleComboBreakingEventHappened);
  }

  public virtual void HandleComboBreakingEventHappened() => this.status = MissionObjectiveChecker.Status.Failed;
}
