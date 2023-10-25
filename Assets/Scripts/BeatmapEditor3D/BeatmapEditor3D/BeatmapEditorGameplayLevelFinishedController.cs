// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorGameplayLevelFinishedController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorGameplayLevelFinishedController : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorStandardLevelScenesTransitionSetupDataSO _beatmapEditorStandardLevelScenesTransitionSetupData;
    [Inject]
    private readonly PrepareLevelCompletionResults _prepareLevelCompletionResults;
    [Inject]
    private readonly ILevelEndActions _gameplayManager;

    protected void Start() => this._gameplayManager.levelFinishedEvent += new Action(this.HandleLevelFinished);

    protected void OnDestroy() => this._gameplayManager.levelFinishedEvent -= new Action(this.HandleLevelFinished);

    private void HandleLevelFinished() => this._beatmapEditorStandardLevelScenesTransitionSetupData.Finish(this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Cleared, LevelCompletionResults.LevelEndAction.None));
  }
}
