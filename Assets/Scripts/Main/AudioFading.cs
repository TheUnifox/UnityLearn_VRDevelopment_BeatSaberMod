// Decompiled with JetBrains decompiler
// Type: AudioFading
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class AudioFading : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _smooth = 4f;
  [SerializeField]
  protected bool _fadeInOnStart;
  protected float _targetVolume;

  public virtual void Start()
  {
    if (this._fadeInOnStart)
    {
      this._audioSource.volume = 0.0f;
      this.FadeIn();
    }
    else
      this.enabled = false;
  }

  public virtual void Update()
  {
    if ((double) Mathf.Abs(this._targetVolume - this._audioSource.volume) < 1.0 / 1000.0)
    {
      this._audioSource.volume = this._targetVolume;
      this.enabled = false;
    }
    else
      this._audioSource.volume = Mathf.Lerp(this._audioSource.volume, this._targetVolume, Time.deltaTime * this._smooth);
  }

  public virtual void FadeOut()
  {
    this.enabled = true;
    this._targetVolume = 0.0f;
  }

  public virtual void FadeIn()
  {
    this.enabled = true;
    this._targetVolume = 1f;
  }
}
