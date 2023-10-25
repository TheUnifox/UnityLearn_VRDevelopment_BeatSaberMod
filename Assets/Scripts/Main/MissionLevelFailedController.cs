// Decompiled with JetBrains decompiler
// Type: MissionLevelFailedController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class MissionLevelFailedController : MonoBehaviour
{
  [SerializeField]
  protected PrepareLevelCompletionResults _prepareLevelCompletionResults;
  [SerializeField]
  protected LevelFailedTextEffect _levelFailedTextEffect;
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelSceneSetupData;
  [SerializeField]
  protected MissionObjectiveCheckersManager _missionObjectiveCheckersManager;
  [Inject]
  protected MissionLevelFailedController.InitData _initData;
  [Inject]
  protected BeatmapObjectSpawnController _beatmapObjectSpawnController;
  [Inject]
  protected GameSongController _gameSongController;
  [Inject]
  protected ILevelEndActions _gameplayManager;
  [Inject]
  protected BeatmapObjectManager _beatmapObjectManager;

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
    MissionCompletionResults missionCompletionResults = new MissionCompletionResults(this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed, this._initData.autoRestart ? LevelCompletionResults.LevelEndAction.Restart : LevelCompletionResults.LevelEndAction.None), this._missionObjectiveCheckersManager.GetResults());
    this._gameSongController.FailStopSong();
    this._beatmapObjectSpawnController.StopSpawning();
    this._beatmapObjectManager.DissolveAllObjects();
    this._levelFailedTextEffect.ShowEffect();
    yield return (object) new WaitForSeconds(2f);
    this._missionLevelSceneSetupData.Finish(missionCompletionResults);
  }

  public class InitData
  {
    public readonly bool autoRestart;

    public InitData(bool autoRestart) => this.autoRestart = autoRestart;
  }
}
