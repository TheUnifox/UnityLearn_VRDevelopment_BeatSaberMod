// Decompiled with JetBrains decompiler
// Type: MissionLevelRestartController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MissionLevelRestartController : MonoBehaviour, ILevelRestartController
{
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelSceneSetupData;
  [SerializeField]
  protected PrepareLevelCompletionResults _prepareLevelCompletionResults;
  [SerializeField]
  protected MissionObjectiveCheckersManager _missionObjectiveCheckersManager;

  public virtual void RestartLevel() => this._missionLevelSceneSetupData.Finish(new MissionCompletionResults(this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Incomplete, LevelCompletionResults.LevelEndAction.Restart), this._missionObjectiveCheckersManager.GetResults()));
}
