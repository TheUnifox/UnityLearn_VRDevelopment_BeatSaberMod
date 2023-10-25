// Decompiled with JetBrains decompiler
// Type: SongController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class SongController : MonoBehaviour
{
  public event System.Action songDidFinishEvent;

  public void SendSongDidFinishEvent()
  {
    System.Action songDidFinishEvent = this.songDidFinishEvent;
    if (songDidFinishEvent == null)
      return;
    songDidFinishEvent();
  }

  public abstract void StopSong();

  public abstract void PauseSong();

  public abstract void ResumeSong();
}
