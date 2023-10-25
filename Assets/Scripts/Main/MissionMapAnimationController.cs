// Decompiled with JetBrains decompiler
// Type: MissionMapAnimationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MissionMapAnimationController : MonoBehaviour
{
  [SerializeField]
  protected MissionNodesManager _missionNodesManager;
  [SerializeField]
  protected ScrollView _mapScrollView;
  [Space]
  [SerializeField]
  protected float _startDelay;
  [SerializeField]
  protected float _stageAnimationStartDelay = 0.3f;
  [SerializeField]
  protected float _missionConnectionAnimationStartDelay = 0.1f;
  [SerializeField]
  protected float _missionConnectionAnimationSeparationTime = 0.1f;
  [SerializeField]
  protected float _stageAnimationDuration = 1f;
  [Inject]
  protected MenuShockwave _shockwaveEffect;

  public bool animatedUpdateIsRequired => (UnityEngine.Object) this._missionNodesManager.GetMissionNodeWithModelClearedStateInconsistency() != (UnityEngine.Object) null;

  public virtual void ScrollToTopMostNotClearedMission()
  {
    MissionNode clearedMissionNode = this._missionNodesManager.GetTopMostNotClearedMissionNode();
    if ((UnityEngine.Object) clearedMissionNode != (UnityEngine.Object) null)
      this._mapScrollView.ScrollToWorldPosition(clearedMissionNode.transform.position, 0.5f, false);
    else
      this._mapScrollView.ScrollToEnd(false);
  }

  public virtual void UpdateMissionMapAfterMissionWasCleared(bool animated, System.Action finishCallback)
  {
    if (!animated)
    {
      this._missionNodesManager.SetupNodeMap();
      if (finishCallback == null)
        return;
      finishCallback();
    }
    else
    {
      MissionNode stateInconsistency = this._missionNodesManager.GetMissionNodeWithModelClearedStateInconsistency();
      if ((UnityEngine.Object) stateInconsistency != (UnityEngine.Object) null)
      {
        this.StartCoroutine(this.UpdateMissionMapCoroutine(stateInconsistency, finishCallback));
      }
      else
      {
        this._missionNodesManager.SetupNodeMap();
        if (finishCallback == null)
          return;
        finishCallback();
      }
    }
  }

  public virtual IEnumerator UpdateMissionMapCoroutine(
    MissionNode lastClearedMissionNode,
    System.Action finishCallback)
  {
    this._mapScrollView.ScrollToWorldPosition(lastClearedMissionNode.transform.position, 0.7f, true);
    yield return (object) new WaitForSeconds(this._startDelay);
    yield return (object) this.UpdateClearedNodeStateCoroutine(lastClearedMissionNode);
    yield return (object) new WaitForSeconds(this._stageAnimationStartDelay);
    yield return (object) this.UpdateStageCoroutine();
    yield return (object) new WaitForSeconds(this._missionConnectionAnimationStartDelay);
    yield return (object) this.UpdateNodesAndConnectionCoroutine();
    System.Action action = finishCallback;
    if (action != null)
      action();
  }

  public virtual IEnumerator UpdateClearedNodeStateCoroutine(MissionNode lastClearedMissionNode)
  {
    if ((UnityEngine.Object) lastClearedMissionNode != (UnityEngine.Object) null)
    {
      lastClearedMissionNode.missionNodeVisualController.SetMissionCleared();
      this._shockwaveEffect.SpawnShockwave(lastClearedMissionNode.transform.position);
    }
    yield return (object) null;
  }

  public virtual IEnumerator UpdateStageCoroutine()
  {
    this._missionNodesManager.UpdateStageLockText();
    if (this._missionNodesManager.DidFirstLockedMissionStageChange())
    {
      this._missionNodesManager.missionStagesManager.UpdateStageLockPositionAnimated(true, this._stageAnimationDuration);
      this._missionNodesManager.UpdateStageLockText();
    }
    yield return (object) null;
  }

  public virtual IEnumerator UpdateNodesAndConnectionCoroutine()
  {
    MissionNodeConnection[] newEnabledConnection = this._missionNodesManager.GetNewEnabledConnection();
    if (((IEnumerable<MissionNodeConnection>) newEnabledConnection).Count<MissionNodeConnection>() == 0)
      yield return (object) null;
    newEnabledConnection = ((IEnumerable<MissionNodeConnection>) newEnabledConnection).OrderBy<MissionNodeConnection, float>((Func<MissionNodeConnection, float>) (connection => connection.childMissionNode.missionNode.position.x)).ThenBy<MissionNodeConnection, float>((Func<MissionNodeConnection, float>) (connection => connection.parentMissionNode.missionNode.position.x)).ToArray<MissionNodeConnection>();
    MissionNodeConnection[] missionNodeConnectionArray = newEnabledConnection;
    for (int index = 0; index < missionNodeConnectionArray.Length; ++index)
    {
      MissionNodeConnection missionNodeConnection = missionNodeConnectionArray[index];
      missionNodeConnection.SetActive(true);
      missionNodeConnection.childMissionNode.SetInteractable();
      yield return (object) new WaitForSeconds(this._missionConnectionAnimationSeparationTime);
    }
    missionNodeConnectionArray = (MissionNodeConnection[]) null;
  }
}
