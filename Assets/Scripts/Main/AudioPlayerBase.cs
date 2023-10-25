// Decompiled with JetBrains decompiler
// Type: AudioPlayerBase
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class AudioPlayerBase : MonoBehaviour
{
  public abstract AudioClip activeAudioClip { get; }

  public abstract void FadeOut(float duration);

  public abstract void PauseCurrentChannel();

  public abstract void UnPauseCurrentChannel();
}
