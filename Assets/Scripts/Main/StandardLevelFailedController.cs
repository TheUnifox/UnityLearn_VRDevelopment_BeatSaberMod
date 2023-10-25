// Decompiled with JetBrains decompiler
// Type: StandardLevelFailedController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class StandardLevelFailedController : MonoBehaviour
{
  [SerializeField]
  protected LevelFailedTextEffect _levelFailedTextEffect;
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _standardLevelSceneSetupData;
  [Inject]
  protected readonly PrepareLevelCompletionResults _prepareLevelCompletionResults;
  [Inject]
  protected readonly StandardLevelFailedController.InitData _initData;
  [Inject]
  protected readonly ILevelEndActions _gameplayManager;
  [Inject]
  protected readonly BeatmapObjectSpawnController _beatmapObjectSpawnController;
  [Inject]
  protected readonly GameSongController _gameSongController;
  [Inject]
  protected readonly EnvironmentSpawnRotation _environmentSpawnRotation;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;

  public virtual void Start() => this._gameplayManager.levelFailedEvent += new System.Action(this.HandleLevelFailed);

  public virtual void OnDestroy()
  {
    if (this._gameplayManager == null)
      return;
    this._gameplayManager.levelFailedEvent -= new System.Action(this.HandleLevelFailed);
  }

  public virtual void HandleLevelFailed() => this.StartCoroutine(this.LevelFailedCoroutine());

  public virtual IEnumerator LevelFailedCoroutine()
  {
    StandardLevelFailedController failedController = this;
    failedController.transform.eulerAngles = new Vector3(0.0f, failedController._environmentSpawnRotation.targetRotation, 0.0f);
    LevelCompletionResults.LevelEndAction levelEndAction = failedController._initData.autoRestart ? LevelCompletionResults.LevelEndAction.Restart : LevelCompletionResults.LevelEndAction.None;
    LevelCompletionResults levelCompletionResults = failedController._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed, levelEndAction);
    failedController._gameSongController.FailStopSong();
    failedController._beatmapObjectSpawnController.StopSpawning();
    failedController._beatmapObjectManager.DissolveAllObjects();
    failedController._levelFailedTextEffect.ShowEffect();
    yield return (object) new WaitForSeconds(2f);
    failedController._standardLevelSceneSetupData.Finish(levelCompletionResults);
  }

  public class InitData
  {
    public readonly bool autoRestart;

    public InitData(bool autoRestart) => this.autoRestart = autoRestart;
  }
}
