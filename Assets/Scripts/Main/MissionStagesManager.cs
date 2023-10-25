// Decompiled with JetBrains decompiler
// Type: MissionStagesManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionStagesManager : MonoBehaviour
{
  [SerializeField]
  protected MissionStageLockView _missionStageLockView;
  protected MissionStage[] _missionStages;
  protected MissionStage _firstLockedMissionStage;

  public MissionStage firstLockedMissionStage => this._firstLockedMissionStage;

  public virtual void UpdateFirtsLockedMissionStage(int numberOfClearedMissions)
  {
    if (this._missionStages == null)
      this.InitStages();
    this._firstLockedMissionStage = (MissionStage) null;
    foreach (MissionStage missionStage in this._missionStages)
    {
      this._firstLockedMissionStage = missionStage;
      if (missionStage.minimumMissionsToUnlock > numberOfClearedMissions)
        break;
    }
  }

  public virtual void InitStages()
  {
    this._missionStages = this.GetComponentsInChildren<MissionStage>();
    this._missionStages = ((IEnumerable<MissionStage>) this._missionStages).OrderBy<MissionStage, int>((Func<MissionStage, int>) (stage => stage.minimumMissionsToUnlock)).ToArray<MissionStage>();
  }

  public virtual void UpdateStageLockPosition() => this.UpdateStageLockPositionAnimated(false, 0.0f);

  public virtual void UpdateStageLockPositionAnimated(bool animated, float animationDuration)
  {
    if ((UnityEngine.Object) this.firstLockedMissionStage != (UnityEngine.Object) null)
    {
      this._missionStageLockView.gameObject.SetActive(true);
      this._missionStageLockView.UpdateLocalPositionY(this.firstLockedMissionStage.position.y, animated, animationDuration);
    }
    else
      this._missionStageLockView.gameObject.SetActive(false);
  }

  public virtual void UpdateStageLockText(int numberOfClearedMissions) => this._missionStageLockView.UpdateStageLockText(numberOfClearedMissions.ToString() + " / " + (object) this.firstLockedMissionStage.minimumMissionsToUnlock);
}
