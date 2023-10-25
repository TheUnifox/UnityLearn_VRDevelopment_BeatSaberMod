// Decompiled with JetBrains decompiler
// Type: MissionLevelAnalytics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MissionLevelAnalytics : MonoBehaviour
{
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelScenesTransitionSetupData;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;

  public virtual void Start() => this._missionLevelScenesTransitionSetupData.didFinishEvent += new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelDidFinishEvent);

  public virtual void OnDestroy() => this._missionLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelDidFinishEvent);

  public virtual void HandleMissionLevelDidFinishEvent(
    MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData,
    MissionCompletionResults missionCompletionResults)
  {
    byte maxValue = byte.MaxValue;
    for (int index = 0; index < missionCompletionResults.missionObjectiveResults.Length; ++index)
    {
      if (!missionCompletionResults.missionObjectiveResults[index].cleared)
        maxValue &= (byte) ~(1 << index);
    }
    Dictionary<string, string> eventData = new Dictionary<string, string>(48);
    eventData.Add("mission_id", missionLevelScenesTransitionSetupData.missionId.ToString());
    eventData.Add("mission_objective_results_mask", maxValue.ToString());
    LevelCompletionResultsAnalyticsHelper.FillEventData(missionCompletionResults.levelCompletionResults, eventData);
    this._analyticsModel.LogEvent("Mission Ended", eventData);
  }
}
