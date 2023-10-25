// Decompiled with JetBrains decompiler
// Type: IGamePause
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IGamePause
{
  bool isPaused { get; }

  event System.Action didPauseEvent;

  event System.Action willResumeEvent;

  event System.Action didResumeEvent;

  void Pause();

  void WillResume();

  void Resume();
}
