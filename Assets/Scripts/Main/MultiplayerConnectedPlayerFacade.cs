// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerFacade
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerFacade : MonoBehaviour
{
  [SerializeField]
  protected GameObject _outroAnimator;
  [SerializeField]
  [NullAllowed]
  protected MultiplayerBigAvatarAnimator _bigAvatarAnimator;
  [Header("Big Avatar Disappear")]
  [SerializeField]
  protected float _bigAvatarDisappearDuration = 0.5f;
  [SerializeField]
  protected EaseType _bigAvatarDisappearEasing = EaseType.InOutCirc;
  [Inject]
  protected readonly MultiplayerConnectedPlayerSongTimeSyncController _songTimeSyncController;
  [Inject]
  protected readonly MultiplayerConnectedPlayerIntroAnimator _introAnimator;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly MultiplayerScoreDiffText _scoreDiffText;
  [Inject]
  protected readonly IConnectedPlayerBeatmapObjectEventManager _beatmapObjectEventManager;

  public MultiplayerConnectedPlayerIntroAnimator introAnimator => this._introAnimator;

  public GameObject outroAnimator => this._outroAnimator;

  public MultiplayerScoreDiffText scoreDiffText => this._scoreDiffText;

  public virtual void SetSongStartSyncTime(float songStartSyncTime) => this._songTimeSyncController.StartSong(songStartSyncTime);

  public virtual void PauseSpawning() => this._beatmapObjectEventManager.Pause();

  public virtual void ResumeSpawning() => this._beatmapObjectEventManager.Resume();

  public virtual void __ForceStopSong()
  {
    this._songTimeSyncController.StopSong();
    this._beatmapObjectManager.DissolveAllObjects();
  }

  public virtual void HideBigAvatar()
  {
    if ((Object) this._bigAvatarAnimator == (Object) null)
      return;
    this._bigAvatarAnimator.Animate(false, this._bigAvatarDisappearDuration, this._bigAvatarDisappearEasing);
  }

  public virtual AvatarPoseController __GetPlayerAvatar() => this.GetComponentInChildren<AvatarPoseController>(true);

  public class Factory : 
    PlaceholderFactory<IConnectedPlayer, MultiplayerPlayerStartState, MultiplayerConnectedPlayerFacade>
  {
  }
}
