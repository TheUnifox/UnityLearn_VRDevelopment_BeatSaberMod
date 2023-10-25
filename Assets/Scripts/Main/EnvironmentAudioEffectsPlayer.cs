// Decompiled with JetBrains decompiler
// Type: EnvironmentAudioEffectsPlayer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class EnvironmentAudioEffectsPlayer : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;

  public AudioSource audioSource => this._audioSource;

  public virtual void PlayEffect(AudioClip clip, float volume)
  {
    this._audioSource.volume = volume;
    this._audioSource.clip = clip;
    this._audioSource.time = 0.0f;
    this._audioSource.Play();
  }
}
