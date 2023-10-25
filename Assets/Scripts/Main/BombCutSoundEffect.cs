// Decompiled with JetBrains decompiler
// Type: BombCutSoundEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BombCutSoundEffect : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  protected Saber _saber;

  public event System.Action<BombCutSoundEffect> didFinishEvent;

  public virtual void Init(AudioClip audioClip, Saber saber, float volume)
  {
    this.enabled = true;
    this._audioSource.clip = audioClip;
    this._saber = saber;
    this._audioSource.volume = volume;
    this._audioSource.Play();
  }

  public virtual void LateUpdate()
  {
    if (this._audioSource.timeSamples >= this._audioSource.clip.samples - 1)
      this.StopPlayingAndFinish();
    else
      this.transform.position = this._saber.saberBladeTopPos;
  }

  public virtual void StopPlayingAndFinish()
  {
    this.enabled = false;
    this._audioSource.Stop();
    System.Action<BombCutSoundEffect> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }

  public class Pool : MonoMemoryPool<BombCutSoundEffect>
  {
  }
}
