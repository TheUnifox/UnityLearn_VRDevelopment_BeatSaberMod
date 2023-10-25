// Decompiled with JetBrains decompiler
// Type: AudioTimeSyncController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class AudioTimeSyncController : MonoBehaviour, IAudioTimeSource
{
  [SerializeField]
  protected float _audioSyncLerpSpeed = 1f;
  [SerializeField]
  protected float _forcedSyncDeltaTime = 0.03f;
  [SerializeField]
  protected float _startSyncDeltaTime = 0.02f;
  [SerializeField]
  protected float _stopSyncDeltaTime = 0.01f;
  [Space]
  [SerializeField]
  protected AudioSource _audioSource;
  [Space]
  [SerializeField]
  protected FloatSO _audioLatency;
  [Inject]
  protected readonly AudioTimeSyncController.InitData _initData;
  [NonSerialized]
  public bool forcedNoAudioSync;
  protected bool _fixingAudioSyncError;
  protected float _audioStartTimeOffsetSinceStart;
  protected int _playbackLoopIndex;
  protected int _prevAudioSamplePos;
  protected float _startSongTime;
  protected float _songTimeOffset;
  protected bool _audioStarted;
  protected float _timeScale;
  protected float _songTime;
  protected double _dspTimeOffset;
  protected AudioTimeSyncController.State _state;
  protected bool _canStartSong;
  protected bool _isReady;
  protected float _lastFrameDeltaSongTime;
  protected bool _forceNoAudioSyncOrAudioSyncErrorFixing;

  public AudioTimeSyncController.State state => this._state;

  public float songTime => this._songTime;

  public float lastFrameDeltaSongTime => this._lastFrameDeltaSongTime;

  public float songLength => !((UnityEngine.Object) this._initData.audioClip != (UnityEngine.Object) null) ? 0.0f : this._initData.audioClip.length;

  public bool isAudioLoaded => this._initData.audioClip.loadState == AudioDataLoadState.Loaded;

  public float songEndTime => this.songLength - (this._songTimeOffset + (float) (ObservableVariableSO<float>) this._audioLatency);

  public float timeScale => this._timeScale;

  public double dspTimeOffset => this._dspTimeOffset;

  public WaitUntil waitUntilAudioIsLoaded => new WaitUntil((Func<bool>) (() => this.isAudioLoaded));

  public bool isReady => this._isReady;

  public float songTimeOffset => this._songTimeOffset;

  public bool forceNoAudioSyncOrAudioSyncErrorFixing
  {
    get => this._forceNoAudioSyncOrAudioSyncErrorFixing;
    set => this._forceNoAudioSyncOrAudioSyncErrorFixing = value;
  }

  public event System.Action stateChangedEvent;

  private float timeSinceStart => Time.timeSinceLevelLoad * this._timeScale;

  public virtual void Awake()
  {
    this._songTime = 0.0f;
    this._audioSource.Stop();
    this._audioSource.clip = (AudioClip) null;
    this._state = AudioTimeSyncController.State.Stopped;
    this._canStartSong = false;
  }

  public virtual void Start()
  {
    this._timeScale = this._initData.timeScale;
    this._audioSource.clip = this._initData.audioClip;
    this._audioSource.pitch = this._initData.timeScale;
    this._startSongTime = this._initData.startSongTime;
    this._songTimeOffset = this._initData.songTimeOffset;
    if ((bool) (UnityEngine.Object) this._initData.audioClip)
      this._initData.audioClip.LoadAudioData();
    this._canStartSong = true;
  }

  public virtual void Update()
  {
    if (this._state == AudioTimeSyncController.State.Stopped)
      return;
    float num1 = Time.deltaTime * this._timeScale;
    this._lastFrameDeltaSongTime = num1;
    if (Time.captureFramerate != 0)
    {
      this._songTime += num1;
      this._songTime = Mathf.Min(this._songTime, this._audioSource.clip.length - 0.01f);
      this._audioSource.time = this._songTime;
      this._dspTimeOffset = AudioSettings.dspTime - (double) this._songTime;
      this._isReady = true;
    }
    else if ((double) this.timeSinceStart < (double) this._audioStartTimeOffsetSinceStart)
    {
      this._songTime += num1;
    }
    else
    {
      if (!this._audioStarted)
      {
        this._audioStarted = true;
        this._audioSource.Play();
      }
      if ((UnityEngine.Object) this._audioSource.clip == (UnityEngine.Object) null || !this._audioSource.isPlaying && !this._forceNoAudioSyncOrAudioSyncErrorFixing)
        return;
      int timeSamples = this._audioSource.timeSamples;
      float time = this._audioSource.time;
      float num2 = this.timeSinceStart - this._audioStartTimeOffsetSinceStart;
      if (this._prevAudioSamplePos > timeSamples)
        ++this._playbackLoopIndex;
      this._prevAudioSamplePos = timeSamples;
      float num3 = time + (float) this._playbackLoopIndex * this._audioSource.clip.length / this._timeScale;
      this._dspTimeOffset = AudioSettings.dspTime - (double) num3 / (double) this._timeScale;
      if (!this._forceNoAudioSyncOrAudioSyncErrorFixing)
      {
        float num4 = Mathf.Abs(num2 - num3);
        if (((double) num4 > (double) this._forcedSyncDeltaTime || this._state == AudioTimeSyncController.State.Paused) && !this.forcedNoAudioSync)
        {
          this._audioStartTimeOffsetSinceStart = this.timeSinceStart - num3;
          num2 = num3;
        }
        else
        {
          if (this._fixingAudioSyncError)
          {
            if ((double) num4 < (double) this._stopSyncDeltaTime)
              this._fixingAudioSyncError = false;
          }
          else if ((double) num4 > (double) this._startSyncDeltaTime)
            this._fixingAudioSyncError = true;
          if (this._fixingAudioSyncError)
            this._audioStartTimeOffsetSinceStart = Mathf.Lerp(this._audioStartTimeOffsetSinceStart, this.timeSinceStart - num3, num1 * this._audioSyncLerpSpeed);
        }
      }
      float num5 = Mathf.Max(this._songTime, num2 - (this._songTimeOffset + (float) (ObservableVariableSO<float>) this._audioLatency));
      this._lastFrameDeltaSongTime = num5 - this._songTime;
      this._songTime = num5;
      this._isReady = true;
    }
  }

  public virtual void StartSong(float startTimeOffset = 0.0f)
  {
    this._state = AudioTimeSyncController.State.Playing;
    System.Action stateChangedEvent = this.stateChangedEvent;
    if (stateChangedEvent != null)
      stateChangedEvent();
    this._playbackLoopIndex = 0;
    this.SeekTo(startTimeOffset);
  }

  public virtual void SeekTo(float startTimeOffset)
  {
    this._songTime = this._startSongTime + startTimeOffset * this.timeScale;
    float a = this._songTime + this._songTimeOffset + (float) (ObservableVariableSO<float>) this._audioLatency;
    bool flag = (double) a >= 0.0;
    this._audioSource.time = Mathf.Max(0.0f, Mathf.Min(a, this._audioSource.clip.length - 0.01f));
    if (this._state == AudioTimeSyncController.State.Playing)
    {
      if (flag && !this._audioStarted)
        this._audioSource.Play();
      if (!flag && this._audioStarted)
        this._audioSource.Stop();
    }
    this._audioStarted = flag;
    this._audioStartTimeOffsetSinceStart = this.timeSinceStart - a;
    this._fixingAudioSyncError = false;
    this._prevAudioSamplePos = (int) ((double) this._audioSource.clip.frequency * (double) a);
    this._dspTimeOffset = AudioSettings.dspTime - (double) a;
  }

  public virtual void StopSong()
  {
    this._audioSource.Stop();
    this._state = AudioTimeSyncController.State.Stopped;
    System.Action stateChangedEvent = this.stateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent();
  }

  public virtual void Pause()
  {
    if (this._state == AudioTimeSyncController.State.Paused || this._state == AudioTimeSyncController.State.Stopped)
      return;
    this._audioSource.Pause();
    this._state = AudioTimeSyncController.State.Paused;
    System.Action stateChangedEvent = this.stateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent();
  }

  public virtual void Resume()
  {
    if (this._state != AudioTimeSyncController.State.Paused)
      return;
    this._state = AudioTimeSyncController.State.Playing;
    System.Action stateChangedEvent = this.stateChangedEvent;
    if (stateChangedEvent != null)
      stateChangedEvent();
    this._audioSource.UnPause();
  }

  public virtual void SetSongTimeIntoAudioTime()
  {
    float num = Mathf.Clamp(this._songTime, 0.0f, this._audioSource.clip.length - 0.01f);
    this._audioSource.time = num;
    this._audioStartTimeOffsetSinceStart = this.timeSinceStart - num;
    this._prevAudioSamplePos = (int) ((double) this._audioSource.clip.frequency * (double) num);
    this._dspTimeOffset = AudioSettings.dspTime - (double) num;
    this._fixingAudioSyncError = false;
  }

  [CompilerGenerated]
  public virtual bool m_Cget_waitUntilAudioIsLoadedm_Eb__27_0() => this.isAudioLoaded;

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

  public enum State
  {
    Playing,
    Paused,
    Stopped,
  }
}
