// Decompiled with JetBrains decompiler
// Type: EnvironmentStartEndSongAudioEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class EnvironmentStartEndSongAudioEffect : MonoBehaviour
{
  [SerializeField]
  protected bool _playStartSongForNonZeroStartSongTime;
  [SerializeField]
  protected float _songFinishedAheadTime = 6f;
  [Space]
  [SerializeField]
  protected float _songStartAudioClipVolume = 0.5f;
  [SerializeField]
  protected float _songFinishedAudioClipVolume = 0.5f;
  [SerializeField]
  protected float _songFailedAudioClipVolume = 0.5f;
  [Space]
  [SerializeField]
  protected AudioClip[] _songStartAudioClips;
  [SerializeField]
  protected AudioClip[] _songFinishedAudioClips;
  [SerializeField]
  protected AudioClip[] _songFailedAudioClips;
  [Inject]
  protected readonly ILevelEndActions _levelEndActions;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [Inject]
  protected readonly IGamePause _gamePause;
  [Inject]
  protected readonly EnvironmentAudioEffectsPlayer _audioEffectsPlayer;
  [Inject]
  protected readonly EnvironmentContext _environmentContext;
  protected const float kSmallSongTime = 0.5f;
  protected bool _songFinishedPlayed;
  protected bool _isWaitingToPlayStartAudio;

  public virtual void Start()
  {
    if (this._environmentContext != EnvironmentContext.Gameplay)
    {
      this.enabled = false;
    }
    else
    {
      int num = this._songStartAudioClips.Length == 0 ? 0 : (this._playStartSongForNonZeroStartSongTime ? 1 : (!this._audioTimeSource.isReady ? 0 : ((double) this._audioTimeSource.songTime <= 0.5 ? 1 : 0)));
      bool flag = this._songStartAudioClips.Length != 0 && !this._audioTimeSource.isReady;
      if (num != 0)
        this._audioEffectsPlayer.PlayEffect(this._songStartAudioClips[UnityEngine.Random.Range(0, this._songStartAudioClips.Length)], this._songStartAudioClipVolume);
      else if (flag)
        this._isWaitingToPlayStartAudio = true;
      this._levelEndActions.levelFailedEvent += new System.Action(this.HandleLevelFailed);
      this.enabled = this._isWaitingToPlayStartAudio || this._songFinishedAudioClips.Length != 0;
      this._gamePause.didPauseEvent += new System.Action(this.HandleGamePauseDidPause);
      this._gamePause.didResumeEvent += new System.Action(this.HandleGamePauseDidResume);
    }
  }

  public virtual void OnDestroy()
  {
    if (this._levelEndActions != null)
      this._levelEndActions.levelFailedEvent -= new System.Action(this.HandleLevelFailed);
    if (this._gamePause == null)
      return;
    this._gamePause.didPauseEvent -= new System.Action(this.HandleGamePauseDidPause);
    this._gamePause.didResumeEvent -= new System.Action(this.HandleGamePauseDidResume);
  }

  public virtual void Update()
  {
    if (this._isWaitingToPlayStartAudio && this._audioTimeSource.isReady && (double) this._audioTimeSource.songTime <= 0.5)
    {
      this._audioEffectsPlayer.PlayEffect(this._songStartAudioClips[UnityEngine.Random.Range(0, this._songStartAudioClips.Length)], this._songStartAudioClipVolume);
      this._isWaitingToPlayStartAudio = false;
      this.enabled = this._songFinishedAudioClips.Length != 0;
    }
    if (this._songFinishedPlayed || (double) this._audioTimeSource.songTime + (double) this._songFinishedAheadTime < (double) this._audioTimeSource.songLength)
      return;
    this.LevelWillFinishWithinAheadTime();
    this._songFinishedPlayed = true;
    this._isWaitingToPlayStartAudio = false;
    this.enabled = false;
  }

  public virtual void LevelWillFinishWithinAheadTime() => this._audioEffectsPlayer.PlayEffect(this._songFinishedAudioClips[UnityEngine.Random.Range(0, this._songFinishedAudioClips.Length)], this._songFinishedAudioClipVolume);

  public virtual void HandleLevelFailed()
  {
    this._isWaitingToPlayStartAudio = false;
    this._songFinishedPlayed = true;
    this.enabled = false;
    if (this._songFailedAudioClips.Length != 0)
      this._audioEffectsPlayer.PlayEffect(this._songFailedAudioClips[UnityEngine.Random.Range(0, this._songFailedAudioClips.Length)], this._songFailedAudioClipVolume);
    else
      this._audioEffectsPlayer.audioSource.Stop();
  }

  public virtual void HandleGamePauseDidResume() => this._audioEffectsPlayer.audioSource.UnPause();

  public virtual void HandleGamePauseDidPause() => this._audioEffectsPlayer.audioSource.Pause();
}
