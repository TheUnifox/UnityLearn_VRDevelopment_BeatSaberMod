// Decompiled with JetBrains decompiler
// Type: MissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class MissionObjectiveChecker : MonoBehaviour
{
  [SerializeField]
  private MissionObjectiveTypeSO _missionObjectiveType;
  private MissionObjectiveChecker.Status _status;
  private int _checkedValue;
  protected MissionObjective _missionObjective;
  private bool _disableChecking;

  public event System.Action<MissionObjectiveChecker> statusDidChangeEvent;

  public event System.Action<MissionObjectiveChecker> checkedValueDidChangeEvent;

  public MissionObjectiveTypeSO missionObjectiveType => this._missionObjectiveType;

  public MissionObjective missionObjective => this._missionObjective;

  public bool disableChecking
  {
    get => this._disableChecking;
    set => this._disableChecking = value;
  }

  public MissionObjectiveChecker.Status status
  {
    get => this._status;
    protected set
    {
      if (this._disableChecking || value == this._status)
        return;
      this._status = value;
      System.Action<MissionObjectiveChecker> statusDidChangeEvent = this.statusDidChangeEvent;
      if (statusDidChangeEvent == null)
        return;
      statusDidChangeEvent(this);
    }
  }

  public int checkedValue
  {
    get => this._checkedValue;
    protected set
    {
      if (this._disableChecking || value == this._checkedValue)
        return;
      this._checkedValue = value;
      System.Action<MissionObjectiveChecker> valueDidChangeEvent = this.checkedValueDidChangeEvent;
      if (valueDidChangeEvent == null)
        return;
      valueDidChangeEvent(this);
    }
  }

  public void SetCheckedMissionObjective(MissionObjective missionObjective)
  {
    this._missionObjective = missionObjective;
    this.Init();
  }

  protected abstract void Init();

  public enum Status
  {
    None,
    NotClearedYet,
    NotFailedYet,
    Cleared,
    Failed,
  }
}
