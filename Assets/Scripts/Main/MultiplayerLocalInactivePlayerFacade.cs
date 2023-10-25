// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalInactivePlayerFacade
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class MultiplayerLocalInactivePlayerFacade : 
  MonoBehaviour,
  IMultiplayerLevelEndActionsPublisher,
  IMultiplayerLevelEndActionsListener,
  IStartSeekSongControllerProvider
{
  [Inject]
  protected readonly MultiplayerLocalInactivePlayerSongSyncController _inactivePlayerSongSyncController;
  [Inject]
  protected readonly MultiplayerSpectatorController _spectatorController;
  [SerializeField]
  protected PlayableDirector _introAnimator;
  [Inject]
  protected readonly MultiplayerLocalInactivePlayerOutroAnimator _outroAnimator;

  public IStartSeekSongController songController => (IStartSeekSongController) this._inactivePlayerSongSyncController;

  public MultiplayerSpectatorController spectatorController => this._spectatorController;

  public GameObject introAnimator => this._introAnimator.gameObject;

  public PlayableDirector introPlayableDirector => this._introAnimator;

  public MultiplayerLocalInactivePlayerOutroAnimator outroAnimator => this._outroAnimator;

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

  public class Factory : 
    PlaceholderFactory<MultiplayerPlayerStartState, MultiplayerLocalInactivePlayerFacade>
  {
  }
}
