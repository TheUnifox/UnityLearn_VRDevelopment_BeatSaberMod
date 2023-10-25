// Decompiled with JetBrains decompiler
// Type: IBeatSaberLogger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

public interface IBeatSaberLogger
{
  void Log(string message);

  void Log(string message, object context);

  void LogWarning(string message);

  void LogWarning(string message, object context);

  void LogError(string message);

  void LogError(string message, object context);

  void LogException(Exception exception);

  void LogException(Exception exception, object context);
}
