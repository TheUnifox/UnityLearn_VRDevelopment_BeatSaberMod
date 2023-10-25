// Decompiled with JetBrains decompiler
// Type: AudioListenerController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class AudioListenerController : MonoBehaviour
{
  protected bool _startAudioListenerPauseState;

  public bool isPaused => AudioListener.pause;

  public virtual void Awake() => this._startAudioListenerPauseState = AudioListener.pause;

  public virtual void OnDestroy() => AudioListener.pause = this._startAudioListenerPauseState;

  public virtual void Pause() => AudioListener.pause = true;

  public virtual void Resume() => AudioListener.pause = false;
}
