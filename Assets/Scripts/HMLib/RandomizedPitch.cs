// Decompiled with JetBrains decompiler
// Type: RandomizedPitch
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections;
using UnityEngine;

public class RandomizedPitch : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _minPitchMultiplier = 0.8f;
  [SerializeField]
  protected float _maxPitchMultiplier = 1.2f;
  [SerializeField]
  protected bool _playOnAwake;
  protected float _originalPitch = 1f;
  protected Coroutine _restoringCoroutine;

  public virtual void OnEnable()
  {
    if (this._audioSource.playOnAwake)
    {
      this._playOnAwake = true;
      this._audioSource.playOnAwake = false;
    }
    if (!this._playOnAwake)
      return;
    this.Play();
  }

  public virtual void Play()
  {
    if (this._restoringCoroutine != null)
    {
      this.StopCoroutine(this._restoringCoroutine);
      this._audioSource.pitch = this._originalPitch;
    }
    this._originalPitch = this._audioSource.pitch;
    this._audioSource.pitch = this._originalPitch * Random.Range(this._minPitchMultiplier, this._maxPitchMultiplier);
    this._audioSource.Play();
    this._restoringCoroutine = this.StartCoroutine(this.RestorePitchWithDelay(this._audioSource.clip.length));
  }

  public virtual void PlayDelayed(float delay)
  {
    if ((double) delay > 0.0)
      this.StartCoroutine(this.PlayDelayedCoroutine(delay));
    else
      this.Play();
  }

  public virtual IEnumerator PlayDelayedCoroutine(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this.Play();
  }

  public virtual IEnumerator RestorePitchWithDelay(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this._audioSource.pitch = this._originalPitch;
  }
}
