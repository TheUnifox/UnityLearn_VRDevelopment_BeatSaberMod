// Decompiled with JetBrains decompiler
// Type: AudioClipQueue
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class AudioClipQueue : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  protected List<AudioClip> _queue = new List<AudioClip>();
  protected float _delay;

  public virtual void Awake() => this._audioSource.loop = false;

  public virtual void Update()
  {
    if ((double) this._delay > 0.0)
      this._delay -= Time.deltaTime;
    else if (this._queue.Count > 0 && !this._audioSource.isPlaying)
    {
      AudioClip audioClip = this._queue[0];
      this._queue.RemoveAt(0);
      this._audioSource.clip = audioClip;
      this._audioSource.Play();
    }
    else
    {
      if (this._queue.Count != 0)
        return;
      this.enabled = false;
    }
  }

  public virtual void PlayAudioClipWithDelay(AudioClip audioClip, float delay)
  {
    this._delay = Mathf.Max(this._delay, delay);
    this._queue.Add(audioClip);
    this.enabled = true;
  }
}
