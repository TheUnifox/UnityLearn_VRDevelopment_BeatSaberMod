// Decompiled with JetBrains decompiler
// Type: SimpleAudioPlayer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SimpleAudioPlayer : AudioPlayerBase
{
  [SerializeField]
  protected AudioClip _audioClip;
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _targetVolume;
  [SerializeField]
  protected bool _loop = true;
  protected float _fadeSpeed;
  protected bool _fadingIn;

  public override AudioClip activeAudioClip => this._audioClip;

  public virtual void Start()
  {
    this._audioSource.volume = 0.0f;
    this._audioSource.clip = this._audioClip;
    this._audioSource.loop = this._loop;
    this._audioSource.Play();
    this.FadeIn(0.5f);
  }

  public virtual void Update()
  {
    float volume = this._audioSource.volume;
    if (this._fadingIn)
    {
      if ((double) volume < (double) this._targetVolume)
      {
        this._audioSource.volume = Mathf.Min(this._targetVolume, volume + Time.deltaTime * this._fadeSpeed);
      }
      else
      {
        if ((double) volume <= (double) this._targetVolume)
          return;
        this._audioSource.volume = this._targetVolume;
      }
    }
    else
    {
      if ((double) volume <= 0.0)
        return;
      float num = volume - Time.deltaTime * this._fadeSpeed;
      if ((double) num <= 0.0)
      {
        this._audioSource.volume = 0.0f;
        this._audioSource.Stop();
        this.enabled = false;
      }
      else
        this._audioSource.volume = num;
    }
  }

  public virtual void FadeIn(float duration)
  {
    this._fadingIn = true;
    this.enabled = true;
    this._fadeSpeed = 1f / duration;
  }

  public override void FadeOut(float duration)
  {
    this._fadingIn = false;
    this.enabled = true;
    this._fadeSpeed = 1f / duration;
  }

  public override void PauseCurrentChannel() => this._audioSource.Pause();

  public override void UnPauseCurrentChannel() => this._audioSource.UnPause();
}
