// Decompiled with JetBrains decompiler
// Type: MissionObjectiveCheckersManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MissionObjectiveCheckersManager : MonoBehaviour
{
  [SerializeField]
  protected MissionObjectiveChecker[] _missionObjectiveCheckers;
  [Inject]
  protected MissionObjectiveCheckersManager.InitData _initData;
  [Inject]
  protected ILevelEndActions _gameplayManager;
  protected MissionObjectiveChecker[] _activeMissionObjectiveCheckers = new MissionObjectiveChecker[0];

  public event System.Action objectiveDidFailEvent;

  public event System.Action objectiveWasClearedEvent;

  public event System.Action objectivesListDidChangeEvent;

  public MissionObjectiveChecker[] activeMissionObjectiveCheckers => this._activeMissionObjectiveCheckers;

  public virtual void Start()
  {
    this._gameplayManager.levelFailedEvent += new System.Action(this.HandleLevelFailed);
    this._gameplayManager.levelFinishedEvent += new System.Action(this.HandleLevelFinished);
    MissionObjective[] missionObjectives = this._initData.missionObjectives;
    List<MissionObjectiveChecker> objectiveCheckerList1 = new List<MissionObjectiveChecker>(this._missionObjectiveCheckers.Length);
    List<MissionObjectiveChecker> objectiveCheckerList2 = new List<MissionObjectiveChecker>((IEnumerable<MissionObjectiveChecker>) this._missionObjectiveCheckers);
    foreach (MissionObjective missionObjective in missionObjectives)
    {
      bool flag = false;
      foreach (MissionObjectiveChecker objectiveChecker in objectiveCheckerList2)
      {
        if ((UnityEngine.Object) objectiveChecker.missionObjectiveType == (UnityEngine.Object) missionObjective.type)
        {
          objectiveCheckerList1.Add(objectiveChecker);
          objectiveChecker.SetCheckedMissionObjective(missionObjective);
          objectiveCheckerList2.Remove(objectiveChecker);
          flag = true;
          break;
        }
      }
      if (!flag)
        Debug.LogError((object) "No missionObjectiveCheckers for missionOjective");
    }
    this._activeMissionObjectiveCheckers = objectiveCheckerList1.ToArray();
    foreach (MissionObjectiveChecker objectiveChecker in this._activeMissionObjectiveCheckers)
      objectiveChecker.statusDidChangeEvent += new System.Action<MissionObjectiveChecker>(this.HandleMissionObjectiveCheckerStatusDidChange);
    System.Action listDidChangeEvent = this.objectivesListDidChangeEvent;
    if (listDidChangeEvent == null)
      return;
    listDidChangeEvent();
  }

  public virtual void OnDestroy()
  {
    if (this._gameplayManager == null)
      return;
    this._gameplayManager.levelFailedEvent -= new System.Action(this.HandleLevelFailed);
    this._gameplayManager.levelFinishedEvent -= new System.Action(this.HandleLevelFinished);
  }

  public virtual void HandleMissionObjectiveCheckerStatusDidChange(
    MissionObjectiveChecker missionObjectiveChecker)
  {
    if (missionObjectiveChecker.status == MissionObjectiveChecker.Status.Failed)
    {
      System.Action objectiveDidFailEvent = this.objectiveDidFailEvent;
      if (objectiveDidFailEvent == null)
        return;
      objectiveDidFailEvent();
    }
    else
    {
      if (missionObjectiveChecker.status != MissionObjectiveChecker.Status.Cleared)
        return;
      System.Action objectiveWasClearedEvent = this.objectiveWasClearedEvent;
      if (objectiveWasClearedEvent == null)
        return;
      objectiveWasClearedEvent();
    }
  }

  public virtual MissionObjectiveChecker GetMissionObjectiveChecker(
    MissionObjectiveTypeSO missionObjectiveType)
  {
    foreach (MissionObjectiveChecker objectiveChecker in this._missionObjectiveCheckers)
    {
      if ((UnityEngine.Object) objectiveChecker.missionObjectiveType == (UnityEngine.Object) missionObjectiveType)
        return objectiveChecker;
    }
    return (MissionObjectiveChecker) null;
  }

  public virtual MissionObjectiveResult[] GetResults()
  {
    MissionObjectiveResult[] results = new MissionObjectiveResult[this._activeMissionObjectiveCheckers.Length];
    for (int index = 0; index < this._activeMissionObjectiveCheckers.Length; ++index)
    {
      MissionObjectiveChecker objectiveChecker = this._activeMissionObjectiveCheckers[index];
      objectiveChecker.disableChecking = true;
      objectiveChecker.statusDidChangeEvent -= new System.Action<MissionObjectiveChecker>(this.HandleMissionObjectiveCheckerStatusDidChange);
      bool cleared = objectiveChecker.status == MissionObjectiveChecker.Status.Cleared || objectiveChecker.status == MissionObjectiveChecker.Status.NotFailedYet;
      int checkedValue = objectiveChecker.checkedValue;
      MissionObjectiveResult missionObjectiveResult = new MissionObjectiveResult(objectiveChecker.missionObjective, cleared, checkedValue);
      results[index] = missionObjectiveResult;
    }
    return results;
  }

  public virtual void HandleLevelFailed() => this.StopChecking();

  public virtual void HandleLevelFinished() => this.StopChecking();

  public virtual void StopChecking()
  {
    for (int index = 0; index < this._activeMissionObjectiveCheckers.Length; ++index)
    {
      MissionObjectiveChecker objectiveChecker = this._activeMissionObjectiveCheckers[index];
      objectiveChecker.disableChecking = true;
      objectiveChecker.statusDidChangeEvent -= new System.Action<MissionObjectiveChecker>(this.HandleMissionObjectiveCheckerStatusDidChange);
    }
  }

  public class InitData
  {
    public readonly MissionObjective[] missionObjectives;

    public InitData(MissionObjective[] missionObjectives) => this.missionObjectives = missionObjectives;
  }
}
