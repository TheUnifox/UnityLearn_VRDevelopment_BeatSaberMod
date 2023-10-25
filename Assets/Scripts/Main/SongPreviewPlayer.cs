// Decompiled with JetBrains decompiler
// Type: SongPreviewPlayer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class SongPreviewPlayer : AudioPlayerBase
{
  [SerializeField]
  [Range(2f, 6f)]
  protected int _channelsCount = 3;
  [SerializeField]
  protected AudioSource _audioSourcePrefab;
  [SerializeField]
  protected AudioClip _defaultAudioClip;
  [SerializeField]
  protected float _volume = 1f;
  [SerializeField]
  protected float _crossFadeToAnotherSongSpeed = 2f;
  [SerializeField]
  protected float _crossFadeToDefaultSpeed = 0.5f;
  [SerializeField]
  protected float _fadeInSpeed = 0.1f;
  [Header("Audio Parameters")]
  [SerializeField]
  protected SongPreviewPlayer.AudioSourceParams _defaultAudioSourceParams;
  [Space]
  [SerializeField]
  protected SongPreviewPlayer.AudioSourceParams _othersAudioSourceParams;
  [Inject]
  protected readonly SongPreviewPlayer.InitData _initData;
  [Inject]
  protected readonly AudioManagerSO _audioManager;
  protected SongPreviewPlayer.AudioSourceVolumeController[] _audioSourceControllers;
  protected int _activeChannel;
  protected float _timeToDefaultAudioTransition;
  protected bool _transitionAfterDelay;
  protected float _volumeScale;
  protected float _fadeSpeed;
  protected float _ambientVolumeScale = 1f;
  protected bool _isActiveChannelPaused;
  protected Dictionary<int, System.Action> _channelToFadeOutCallbackDictionary = new Dictionary<int, System.Action>();

  public override AudioClip activeAudioClip => this._activeChannel >= 0 ? this._audioSourceControllers[this._activeChannel].audioSource.clip : (AudioClip) null;

  public virtual void Awake()
  {
    this._audioSourceControllers = new SongPreviewPlayer.AudioSourceVolumeController[this._channelsCount];
    for (int index = 0; index < this._channelsCount; ++index)
    {
      AudioSource audioSource = UnityEngine.Object.Instantiate<AudioSource>(this._audioSourcePrefab, this.transform);
      audioSource.volume = 0.0f;
      audioSource.loop = false;
      audioSource.reverbZoneMix = 0.0f;
      audioSource.playOnAwake = false;
      audioSource.minDistance = 100f;
      audioSource.dopplerLevel = 0.0f;
      this._audioSourceControllers[index] = new SongPreviewPlayer.AudioSourceVolumeController(audioSource);
    }
  }

  public virtual void Start() => this._ambientVolumeScale = this._initData.ambientVolumeScale;

  public virtual void OnEnable()
  {
    foreach (SongPreviewPlayer.AudioSourceVolumeController sourceController in this._audioSourceControllers)
    {
      if (sourceController != null && (UnityEngine.Object) sourceController.audioSource != (UnityEngine.Object) null)
        sourceController.audioSource.enabled = true;
    }
    this._fadeSpeed = this._fadeInSpeed;
    this.StartCoroutine(this.CrossFadeAfterDelayCoroutine(0.5f));
  }

  public virtual IEnumerator CrossFadeAfterDelayCoroutine(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this.CrossfadeToDefault();
  }

  public virtual void OnDisable()
  {
    foreach (SongPreviewPlayer.AudioSourceVolumeController sourceController in this._audioSourceControllers)
    {
      if (sourceController != null && (UnityEngine.Object) sourceController.audioSource != (UnityEngine.Object) null)
        sourceController.audioSource.enabled = false;
    }
  }

  public virtual void Update()
  {
    for (int channel = 0; channel < this._audioSourceControllers.Length; ++channel)
    {
      SongPreviewPlayer.AudioSourceVolumeController sourceController = this._audioSourceControllers[channel];
      float volume = sourceController.volume;
      if (this._activeChannel == channel)
      {
        float a = this._volume * this._volumeScale;
        if ((double) volume < (double) a)
        {
          sourceController.maxVolume = a;
          sourceController.volume = Mathf.Min(a, volume + Time.deltaTime * this._fadeSpeed);
        }
        else if ((double) volume > (double) a)
        {
          sourceController.maxVolume = a;
          sourceController.volume = a;
        }
      }
      else if ((double) volume > 0.0)
      {
        float num = volume - Time.deltaTime * this._fadeSpeed;
        if ((double) num <= 0.0)
        {
          sourceController.volume = 0.0f;
          sourceController.audioSource.Stop();
          this.ReportChannelDidFadeOut(channel);
        }
        else
          sourceController.volume = num;
      }
    }
    if (!this._transitionAfterDelay || this._isActiveChannelPaused)
      return;
    this._timeToDefaultAudioTransition -= Time.deltaTime;
    if ((double) this._timeToDefaultAudioTransition > 0.0)
      return;
    this.CrossfadeTo(this._defaultAudioClip, -4f, 0.0f, -1f, true, (System.Action) null);
  }

  public virtual void CrossfadeToDefault()
  {
    if (this._audioSourceControllers == null || this._activeChannel >= 0 && (UnityEngine.Object) this._audioSourceControllers[this._activeChannel].audioSource == (UnityEngine.Object) null || !this._transitionAfterDelay && this._activeChannel > 0 && (UnityEngine.Object) this._audioSourceControllers[this._activeChannel].audioSource.clip == (UnityEngine.Object) this._defaultAudioClip)
      return;
    this.CrossfadeTo(this._defaultAudioClip, -4f, Mathf.Max(UnityEngine.Random.Range(0.0f, this._defaultAudioClip.length - 0.1f), 0.0f), -1f, true, (System.Action) null);
  }

  public virtual void CrossfadeToNewDefault(AudioClip audioClip)
  {
    if (!((UnityEngine.Object) this._defaultAudioClip != (UnityEngine.Object) audioClip))
      return;
    this._defaultAudioClip = audioClip;
    this.CrossfadeTo(audioClip, -4f, Mathf.Max(UnityEngine.Random.Range(0.0f, this._defaultAudioClip.length - 0.1f), 0.0f), -1f, true, (System.Action) null);
  }

  public virtual void CrossfadeTo(
    AudioClip audioClip,
    float musicVolume,
    float startTime,
    float duration,
    System.Action onFadeOutCallback)
  {
    this.CrossfadeTo(audioClip, musicVolume, startTime, duration, false, onFadeOutCallback);
  }

  public virtual void CrossfadeTo(
    AudioClip audioClip,
    float musicVolume,
    float startTime,
    float duration,
    bool isDefault,
    System.Action onFadeOutCallback)
  {
    this.StopAllCoroutines();
    this._audioManager.musicVolume = musicVolume;
    this._fadeSpeed = isDefault ? this._crossFadeToDefaultSpeed : this._crossFadeToAnotherSongSpeed;
    this._volumeScale = isDefault ? this._ambientVolumeScale : 1f;
    float num = this._volume;
    int index1 = 0;
    for (int index2 = 0; index2 < this._audioSourceControllers.Length; ++index2)
    {
      float volume = this._audioSourceControllers[index2].volume;
      if ((double) volume <= (double) num)
      {
        index1 = index2;
        num = volume;
      }
    }
    this._activeChannel = index1;
    this.ReportChannelDidFadeOut(index1);
    if (onFadeOutCallback != null)
      this._channelToFadeOutCallbackDictionary[index1] = onFadeOutCallback;
    AudioSource audioSource = this._audioSourceControllers[index1].audioSource;
    audioSource.volume = 0.0f;
    audioSource.clip = audioClip;
    audioSource.time = startTime;
    this._timeToDefaultAudioTransition = Mathf.Max(0.0f, duration - 1f / this._crossFadeToDefaultSpeed);
    SongPreviewPlayer.AudioSourceParams audioSourceParams = isDefault ? this._defaultAudioSourceParams : this._othersAudioSourceParams;
    audioSource.transform.position = audioSourceParams.position;
    audioSource.reverbZoneMix = audioSourceParams.reverbZoneMix;
    audioSource.bypassReverbZones = (double) audioSourceParams.reverbZoneMix <= 0.0099999997764825821;
    audioSource.bypassEffects = (double) audioSourceParams.reverbZoneMix <= 0.0099999997764825821;
    audioSource.spatialBlend = audioSourceParams.spatialBlend;
    audioSource.spread = audioSourceParams.spread;
    this._transitionAfterDelay = (double) duration > 0.0;
    audioSource.loop = !this._transitionAfterDelay;
    audioSource.Play();
  }

  public override void PauseCurrentChannel()
  {
    if (this._activeChannel < 0)
      return;
    AudioSource audioSource = this._audioSourceControllers[this._activeChannel].audioSource;
    if (!audioSource.isPlaying)
      return;
    audioSource.Pause();
    this._isActiveChannelPaused = true;
  }

  public override void UnPauseCurrentChannel()
  {
    if (this._activeChannel < 0)
      return;
    AudioSource audioSource = this._audioSourceControllers[this._activeChannel].audioSource;
    if (audioSource.isPlaying)
      return;
    audioSource.UnPause();
    this._isActiveChannelPaused = false;
  }

  public override void FadeOut(float duration)
  {
    this._fadeSpeed = 1f / duration;
    this._transitionAfterDelay = false;
    this._activeChannel = -1;
  }

  public virtual void ReportChannelDidFadeOut(int channel)
  {
    System.Action action;
    if (!this._channelToFadeOutCallbackDictionary.TryGetValue(channel, out action))
      return;
    if (action != null)
      action();
    this._channelToFadeOutCallbackDictionary.Remove(channel);
  }

  public class InitData
  {
    public readonly float ambientVolumeScale;

    public InitData(float ambientVolumeScale) => this.ambientVolumeScale = ambientVolumeScale;
  }

  [Serializable]
  public class AudioSourceParams
  {
    [SerializeField]
    protected Vector3 _position;
    [SerializeField]
    [Range(0.0f, 1.1f)]
    protected float _reverbZoneMix;
    [SerializeField]
    [Range(0.0f, 1f)]
    protected float _spatialBlend;
    [SerializeField]
    [Range(0.0f, 360f)]
    protected float _spread;

    public Vector3 position => this._position;

    public float reverbZoneMix => this._reverbZoneMix;

    public float spatialBlend => this._spatialBlend;

    public float spread => this._spread;
  }

  public class AudioSourceVolumeController
  {
    public readonly AudioSource audioSource;
    [CompilerGenerated]
    protected float m_CmaxVolume;
    protected float _volume;

    public float volume
    {
      set
      {
        this._volume = value;
        float num = this._volume / this.maxVolume;
        this.audioSource.volume = num * num * this.maxVolume;
      }
      get => this._volume;
    }

    public float maxVolume
    {
      get => this.m_CmaxVolume;
      set => this.m_CmaxVolume = value;
    }

    public AudioSourceVolumeController(AudioSource audioSource)
    {
      this.maxVolume = 1f;
      this.audioSource = audioSource;
    }
  }
}
