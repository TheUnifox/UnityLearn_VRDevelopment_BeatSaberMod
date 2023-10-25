// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerSongTimeSyncController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerSongTimeSyncController : MonoBehaviour, IAudioTimeSource
{
  [SerializeField]
  protected float _audioSyncLerpSpeed = 1f;
  [SerializeField]
  protected float _forcedSyncDeltaTime = 0.03f;
  [SerializeField]
  protected float _startSyncDeltaTime = 0.02f;
  [SerializeField]
  protected float _stopSyncDeltaTime = 0.01f;
  [SerializeField]
  protected FloatSO _audioLatency;
  [Inject]
  protected readonly MultiplayerConnectedPlayerSongTimeSyncController.InitData _initData;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [CompilerGenerated]
  protected float m_CsongTime;
  [CompilerGenerated]
  protected float m_ClastFrameDeltaSongTime;
  protected float _timeScale;
  protected float _startSongSyncTime;
  protected bool _fixingAudioSyncError;
  protected bool _isReady;

  public float songTime
  {
    get => this.m_CsongTime;
    private set => this.m_CsongTime = value;
  }

  public float lastFrameDeltaSongTime
  {
    get => this.m_ClastFrameDeltaSongTime;
    private set => this.m_ClastFrameDeltaSongTime = value;
  }

  public float songEndTime => float.MaxValue;

  public float songLength => float.MaxValue;

  public bool isReady => this._isReady;

  public virtual void Start() => this.enabled = false;

  public virtual void Update()
  {
    float num1 = Time.deltaTime * this._timeScale;
    float b = (this._connectedPlayer.offsetSyncTime - this._startSongSyncTime) * this._timeScale;
    float a = this.songTime + num1;
    float num2 = Mathf.Abs(a - b);
    if ((double) num2 > (double) this._forcedSyncDeltaTime)
    {
      a = b;
    }
    else
    {
      if (this._fixingAudioSyncError)
      {
        if ((double) num2 < (double) this._stopSyncDeltaTime)
          this._fixingAudioSyncError = false;
      }
      else if ((double) num2 > (double) this._startSyncDeltaTime)
        this._fixingAudioSyncError = true;
      if (this._fixingAudioSyncError)
        a = Mathf.Lerp(a, b, num1 * this._audioSyncLerpSpeed);
    }
    this.lastFrameDeltaSongTime = a - this.songTime;
    this.songTime = a;
  }

  public virtual void StartSong(float songStartSyncTime)
  {
    this._timeScale = this._initData.timeScale;
    this._startSongSyncTime = this._initData.startSongTime + this._initData.songTimeOffset + (float) (ObservableVariableSO<float>) this._audioLatency + songStartSyncTime;
    this.songTime = (this._connectedPlayer.offsetSyncTime - this._startSongSyncTime) * this._timeScale;
    this._isReady = true;
    this.enabled = true;
  }

  public virtual void SetConnectedPlayerSongTime(float syncTime, float songTime) => this._startSongSyncTime = syncTime - songTime / this._timeScale;

  public virtual void StopSong() => this.enabled = false;

  public class InitData
  {
    public readonly float startSongTime;
    public readonly float songTimeOffset;
    public readonly float timeScale;

    public InitData(float startSongTime, float songTimeOffset, float timeScale)
    {
      this.startSongTime = startSongTime;
      this.songTimeOffset = songTimeOffset;
      this.timeScale = timeScale;
    }
  }
}
