// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerFacade
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalActivePlayerFacade : 
  MonoBehaviour,
  IMultiplayerLevelEndActionsPublisher,
  IMultiplayerLevelEndActionsListener,
  IStartSeekSongControllerProvider
{
  [SerializeField]
  protected GameObject[] _activeOnlyGameObjects;
  [SerializeField]
  protected GameObject _outroAnimator;
  [Inject]
  protected readonly IStartSeekSongController _songController;
  [Inject]
  protected readonly MultiplayerLocalActivePlayerIntroAnimator _introAnimator;
  [Inject]
  protected readonly GameSongController _gameSongController;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly PrepareLevelCompletionResults _prepareLevelCompletionResults;
  [Inject]
  protected readonly BeatmapCallbacksUpdater _beatmapCallbacksUpdater;

  public MultiplayerLocalActivePlayerIntroAnimator introAnimator => this._introAnimator;

  public GameObject outroAnimator => this._outroAnimator;

  public IStartSeekSongController songController => this._songController;

  public LevelCompletionResults currentLocalPlayerLevelCompletionResult => this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Incomplete, LevelCompletionResults.LevelEndAction.None);

  public event System.Action<MultiplayerLevelCompletionResults> playerDidFinishEvent;

  public event System.Action<MultiplayerLevelCompletionResults> playerNetworkDidFailedEvent;

  public virtual void ReportPlayerDidFinish(MultiplayerLevelCompletionResults results)
  {
    System.Action<MultiplayerLevelCompletionResults> playerDidFinishEvent = this.playerDidFinishEvent;
    if (playerDidFinishEvent == null)
      return;
    playerDidFinishEvent(results);
  }

  public virtual void ReportPlayerNetworkDidFailed(MultiplayerLevelCompletionResults results)
  {
    System.Action<MultiplayerLevelCompletionResults> networkDidFailedEvent = this.playerNetworkDidFailedEvent;
    if (networkDidFailedEvent == null)
      return;
    networkDidFailedEvent(results);
  }

  public virtual void DisablePlayer()
  {
    foreach (GameObject activeOnlyGameObject in this._activeOnlyGameObjects)
      activeOnlyGameObject.SetActive(false);
  }

  public virtual void PauseSpawning() => this._beatmapCallbacksUpdater.Pause();

  public virtual void ResumeSpawning() => this._beatmapCallbacksUpdater.Resume();

  public virtual void __ForceStopSong()
  {
    this._gameSongController.StopSong();
    this._beatmapObjectManager.DissolveAllObjects();
  }

  public virtual GameObject[] __GetActiveOnlyGameObjects() => this._activeOnlyGameObjects;

  public class Factory : 
    PlaceholderFactory<MultiplayerPlayerStartState, MultiplayerLocalActivePlayerFacade>
  {
  }
}
