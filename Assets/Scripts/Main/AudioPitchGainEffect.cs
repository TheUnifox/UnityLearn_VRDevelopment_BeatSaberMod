// Decompiled with JetBrains decompiler
// Type: AudioPitchGainEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;

public class AudioPitchGainEffect : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _duration = 0.3f;
  [SerializeField]
  protected AnimationCurve _pitchCurve;
  [SerializeField]
  protected AnimationCurve _gainCurve;
  protected Coroutine _currentCoroutine;
  protected float _startPitch;
  protected float _startVolume;

  public virtual void Start() => this._startPitch = this._audioSource.pitch;

  public virtual IEnumerator StartEffectCoroutine(float volumeScale, System.Action finishCallback)
  {
    float startPitch = this._audioSource.pitch;
    float time = 0.0f;
    while ((double) time < (double) this._duration)
    {
      float time1 = time / this._duration;
      this._audioSource.pitch = startPitch * this._pitchCurve.Evaluate(time1);
      this._audioSource.volume = this._gainCurve.Evaluate(time1) * volumeScale;
      time += Time.deltaTime;
      yield return (object) null;
    }
    this._audioSource.pitch = startPitch * this._pitchCurve.Evaluate(1f);
    this._audioSource.volume = this._gainCurve.Evaluate(1f) * volumeScale;
    System.Action action = finishCallback;
    if (action != null)
      action();
  }

  public virtual void StartEffect(float volumeScale, System.Action finishCallback) => this._currentCoroutine = this.StartCoroutine(this.StartEffectCoroutine(volumeScale, finishCallback));

  public virtual void InterruptEffect()
  {
    this._audioSource.pitch = this._startPitch;
    if (this._currentCoroutine == null)
      return;
    this.StopCoroutine(this._currentCoroutine);
    this._currentCoroutine = (Coroutine) null;
  }

  public virtual void SetAudioSource(AudioSource audioSource) => this._audioSource = audioSource;
}
