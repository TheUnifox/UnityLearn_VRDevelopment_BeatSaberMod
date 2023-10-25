// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalInactivePlayerSongSyncController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerLocalInactivePlayerSongSyncController : 
  MonoBehaviour,
  IStartSeekSongController
{
  [SerializeField]
  protected CrossFadeAudioSource _audioSource;
  [Space]
  [SerializeField]
  protected FloatSO _audioLatency;
  [Inject]
  protected readonly MultiplayerLocalInactivePlayerSongSyncController.InitData _initData;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  protected float _timeScale;
  protected float _startSongTime;
  protected float _songTimeOffset;
  protected float _songTime;
  protected bool _audioStarted;
  protected bool _currentObservableIsFailed;
  protected IMultiplayerObservable _observable;
  protected float _lastLatencyOffsetTime;
  protected const float kReSyncThreshold = 0.05f;

  public WaitUntil waitUntilIsReadyToStartTheSong => new WaitUntil((Func<bool>) (() => this.isAudioLoaded));

  private bool isAudioLoaded => (UnityEngine.Object) this._initData.audioClip == (UnityEngine.Object) null || this._initData.audioClip.loadState == AudioDataLoadState.Loaded;

  public virtual void Awake()
  {
    this._timeScale = this._initData.timeScale;
    this._audioSource.clip = this._initData.audioClip;
    this._audioSource.pitch = this._initData.timeScale;
    this._startSongTime = this._initData.startSongTime;
    this._songTimeOffset = this._initData.songTimeOffset + (float) (ObservableVariableSO<float>) this._audioLatency;
    if ((bool) (UnityEngine.Object) this._initData.audioClip)
      this._initData.audioClip.LoadAudioData();
    this.enabled = false;
  }

  public virtual void Start() => this._vrPlatformHelper.vrFocusWasReleasedEvent += new System.Action(this.HandleVrFocusWasReleased);

  public virtual void OnDestroy()
  {
    if (this._vrPlatformHelper == null)
      return;
    this._vrPlatformHelper.vrFocusWasReleasedEvent -= new System.Action(this.HandleVrFocusWasReleased);
  }

  public virtual void Update()
  {
    if ((UnityEngine.Object) this._audioSource.clip == (UnityEngine.Object) null)
      return;
    float num = Time.deltaTime * this._timeScale;
    if (Time.captureFramerate != 0)
    {
      this._songTime += num;
      this._songTime = Mathf.Min(this._songTime, this._audioSource.clip.length - 0.01f);
      this._audioSource.time = this._songTime;
    }
    else
    {
      this._songTime += num;
      bool flag = (double) this._songTime > 0.0;
      if (flag && !this._audioStarted)
        this._audioSource.Play();
      if (!flag && this._audioStarted)
        this._audioSource.Stop();
      this._audioStarted = flag;
      if (!this._audioSource.isPlaying || this._observable == null)
        return;
      if (!this._currentObservableIsFailed && this._observable.isFailed)
      {
        this._currentObservableIsFailed = true;
        this._audioSource.PlayPitchGainEffect(1f);
      }
      if (this._currentObservableIsFailed)
        return;
      this.UpdateOffsetSyncTime(this._observable.offsetSyncTime, true, false);
    }
  }

  public virtual void StartSong(float offsetTime = 0.0f)
  {
    this.enabled = true;
    this.SeekTo(offsetTime, false, 1f);
    if (this._observable == null)
      return;
    this.UpdateOffsetSyncTime(this._observable.offsetSyncTime, true, true);
  }

  public virtual void FollowOffsetSyncTime(
    IMultiplayerObservable observable,
    bool crossFade,
    bool forceUpdate)
  {
    if (this._currentObservableIsFailed)
      this._audioSource.InterruptLastPitchGainEffect();
    forceUpdate = forceUpdate || this._currentObservableIsFailed != observable.isFailed;
    this._observable = observable;
    this._currentObservableIsFailed = observable.isFailed;
    if (!this.enabled)
      return;
    this.UpdateOffsetSyncTime(observable.offsetSyncTime, crossFade, forceUpdate);
  }

  public virtual void UpdateOffsetSyncTime(float offsetSyncTime, bool crossFade, bool forceUpdate)
  {
    float num1 = offsetSyncTime - this._multiplayerSessionManager.syncTime;
    if (!((double) Mathf.Abs(num1 - this._lastLatencyOffsetTime) > 0.05000000074505806 | forceUpdate))
      return;
    float num2 = num1 - this._lastLatencyOffsetTime;
    this._lastLatencyOffsetTime = num1;
    this.SeekTo((this._songTime + num2) / this._timeScale, crossFade, this._currentObservableIsFailed ? 0.0f : 1f);
  }

  public virtual void SeekTo(float offsetTime) => this.SeekTo(offsetTime, false, 1f);

  public virtual void SeekTo(float offsetTime, bool crossFade, float toVolume)
  {
    if ((UnityEngine.Object) this._audioSource.clip == (UnityEngine.Object) null)
      return;
    this._songTime = this._startSongTime + offsetTime * this._timeScale;
    float num = this._songTime + this._songTimeOffset;
    bool flag = (double) num >= 0.0;
    if (crossFade && (double) num > 0.0)
      this._audioSource.CrossFade(Mathf.Clamp(num, 0.0f, this._audioSource.clip.length - 0.01f), toVolume);
    else
      this._audioSource.time = Mathf.Clamp(num, 0.0f, this._audioSource.clip.length - 0.01f);
    if (flag && !this._audioStarted)
      this._audioSource.Play();
    if (!flag && this._audioStarted)
      this._audioSource.Stop();
    this._audioStarted = flag;
  }

  public virtual void HandleVrFocusWasReleased()
  {
  }

  [CompilerGenerated]
  public virtual bool m_Cget_waitUntilIsReadyToStartTheSongm_Eb__7_0() => this.isAudioLoaded;

  public class InitData
  {
    public readonly AudioClip audioClip;
    public readonly float startSongTime;
    public readonly float songTimeOffset;
    public readonly float timeScale;

    public InitData(
      AudioClip audioClip,
      float startSongTime,
      float songTimeOffset,
      float timeScale)
    {
      this.audioClip = audioClip;
      this.startSongTime = startSongTime;
      this.songTimeOffset = songTimeOffset;
      this.timeScale = timeScale;
    }
  }
}
