// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActiveLevelFailController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class MultiplayerLocalActiveLevelFailController : MonoBehaviour
{
  [SerializeField]
  protected LevelFailedTextEffect _levelFailedTextEffect;
  [Inject]
  protected readonly IMultiplayerLevelEndActionsPublisher _levelEndActionsPublisher;
  [Inject]
  protected readonly BeatmapObjectSpawnController _beatmapObjectSpawnController;
  [Inject]
  protected readonly GameSongController _gameSongController;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly MultiplayerPlayersManager _multiplayerPlayersManager;

  public virtual void Start() => this._levelEndActionsPublisher.playerDidFinishEvent += new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);

  public virtual void OnDestroy()
  {
    if (this._levelEndActionsPublisher == null)
      return;
    this._levelEndActionsPublisher.playerDidFinishEvent -= new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
  }

  public virtual IEnumerator PlayerFailedCoroutine()
  {
    this._gameSongController.FailStopSong();
    this._beatmapObjectSpawnController.StopSpawning();
    this._beatmapObjectManager.DissolveAllObjects();
    this._levelFailedTextEffect.ShowEffect();
    yield return (object) new WaitForSeconds(2f);
    this._multiplayerPlayersManager.SwitchLocalPlayerToInactive();
  }

  public virtual void HandlePlayerDidFinish(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    if (!levelCompletionResults.failedOrGivenUp)
      return;
    this.StartCoroutine(this.PlayerFailedCoroutine());
  }
}
