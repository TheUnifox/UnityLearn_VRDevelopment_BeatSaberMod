// Decompiled with JetBrains decompiler
// Type: MissionLevelFinishedController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MissionLevelFinishedController : MonoBehaviour
{
  [SerializeField]
  protected PrepareLevelCompletionResults _prepareLevelCompletionResults;
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelSceneSetupData;
  [SerializeField]
  protected MissionObjectiveCheckersManager _missionObjectiveCheckersManager;
  [Inject]
  protected ILevelEndActions _gameplayManager;

  public virtual void Start() => this._gameplayManager.levelFinishedEvent += new System.Action(this.HandleLevelFinished);

  public virtual void OnDestroy()
  {
    if (this._gameplayManager == null)
      return;
    this._gameplayManager.levelFinishedEvent -= new System.Action(this.HandleLevelFinished);
  }

  public virtual void HandleLevelFinished() => this.StartLevelFinished();

  public virtual void StartLevelFinished() => this._missionLevelSceneSetupData.Finish(new MissionCompletionResults(this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Cleared, LevelCompletionResults.LevelEndAction.None), this._missionObjectiveCheckersManager.GetResults()));
}
