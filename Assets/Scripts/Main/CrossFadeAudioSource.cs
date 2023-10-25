// Decompiled with JetBrains decompiler
// Type: CrossFadeAudioSource
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class CrossFadeAudioSource : MonoBehaviour
{
  [SerializeField]
  protected float _duration = 0.45f;
  [SerializeField]
  protected AudioSource _audioSource1;
  [SerializeField]
  protected AudioSource _audioSource2;
  [SerializeField]
  protected AudioPitchGainEffect _audioPitchGainEffect1;
  [SerializeField]
  protected AudioPitchGainEffect _audioPitchGainEffect2;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected AudioSource _activeAudioSource;
  protected AudioSource _inactiveAudioSource;
  protected Tween<float> _volumeDownTween;
  protected Tween<float> _volumeUpTween;
  protected AudioPitchGainEffect _activePitchGainEffect;
  protected AudioPitchGainEffect _inactivePitchGainEffect;

  public AudioClip clip
  {
    get => this._activeAudioSource.clip;
    set
    {
      this._audioSource1.clip = value;
      this._audioSource2.clip = value;
    }
  }

  public float pitch
  {
    set
    {
      this._audioSource1.pitch = value;
      this._audioSource2.pitch = value;
    }
  }

  public float time
  {
    set
    {
      this._audioSource1.time = value;
      this._audioSource2.time = value;
    }
  }

  public bool isPlaying => this._activeAudioSource.isPlaying;

  public virtual void Awake()
  {
    this._volumeDownTween = (Tween<float>) new FloatTween(1f, 0.0f, (System.Action<float>) (val => this._activeAudioSource.volume = val), 0.0f, EaseType.Linear);
    this._volumeUpTween = (Tween<float>) new FloatTween(0.0f, 1f, (System.Action<float>) (val => this._inactiveAudioSource.volume = val), 0.0f, EaseType.Linear);
    this._activeAudioSource = this._audioSource1;
    this._inactiveAudioSource = this._audioSource2;
    this._activePitchGainEffect = this._audioPitchGainEffect1;
    this._inactivePitchGainEffect = this._audioPitchGainEffect2;
    this._activePitchGainEffect.SetAudioSource(this._activeAudioSource);
    this._inactivePitchGainEffect.SetAudioSource(this._inactiveAudioSource);
    this._inactiveAudioSource.volume = 0.0f;
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void PlayPitchGainEffect(float volumeScale) => this._activePitchGainEffect.StartEffect(volumeScale, (System.Action) null);

  public virtual void InterruptLastPitchGainEffect() => this._inactivePitchGainEffect.InterruptEffect();

  public virtual void CrossFade(float toSongTime, float toVolume)
  {
    this._inactiveAudioSource.time = toSongTime;
    this._inactiveAudioSource.Play();
    float volume1 = this._activeAudioSource.volume;
    this._volumeDownTween.fromValue = volume1;
    this._volumeDownTween.duration = this._duration * volume1;
    this._volumeDownTween.onUpdate = (System.Action<float>) (val => this._activeAudioSource.volume = val);
    float volume2 = this._inactiveAudioSource.volume;
    this._volumeUpTween.fromValue = volume2;
    this._volumeUpTween.toValue = toVolume;
    this._volumeUpTween.duration = this._duration * (1f - volume2);
    this._volumeUpTween.onUpdate = (System.Action<float>) (val => this._inactiveAudioSource.volume = val);
    this._volumeUpTween.onCompleted = (System.Action) (() =>
    {
      AudioSource activeAudioSource = this._activeAudioSource;
      this._activeAudioSource = this._inactiveAudioSource;
      this._inactiveAudioSource = activeAudioSource;
      AudioPitchGainEffect activePitchGainEffect = this._activePitchGainEffect;
      this._activePitchGainEffect = this._inactivePitchGainEffect;
      this._inactivePitchGainEffect = activePitchGainEffect;
      this._inactiveAudioSource.Stop();
    });
    this._tweeningManager.RestartTween((Tween) this._volumeUpTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._volumeDownTween, (object) this);
  }

  public virtual void Play() => this._activeAudioSource.Play();

  public virtual void Stop()
  {
    this._volumeUpTween?.Kill();
    this._volumeDownTween?.Kill();
    this._activeAudioSource.Stop();
    this._inactiveAudioSource.Stop();
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__21_0(float val) => this._activeAudioSource.volume = val;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__21_1(float val) => this._inactiveAudioSource.volume = val;

  [CompilerGenerated]
  public virtual void m_CCrossFadem_Eb__25_0(float val) => this._activeAudioSource.volume = val;

  [CompilerGenerated]
  public virtual void m_CCrossFadem_Eb__25_1(float val) => this._inactiveAudioSource.volume = val;

  [CompilerGenerated]
  public virtual void m_CCrossFadem_Eb__25_2()
  {
    AudioSource activeAudioSource = this._activeAudioSource;
    this._activeAudioSource = this._inactiveAudioSource;
    this._inactiveAudioSource = activeAudioSource;
    AudioPitchGainEffect activePitchGainEffect = this._activePitchGainEffect;
    this._activePitchGainEffect = this._inactivePitchGainEffect;
    this._inactivePitchGainEffect = activePitchGainEffect;
    this._inactiveAudioSource.Stop();
  }
}
