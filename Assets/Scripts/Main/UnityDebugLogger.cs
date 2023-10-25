// Decompiled with JetBrains decompiler
// Type: UnityDebugLogger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class UnityDebugLogger : IBeatSaberLogger
{
  public virtual void Log(string message) => Debug.Log((object) message);

  public virtual void Log(string message, object context) => Debug.Log((object) message, context as UnityEngine.Object);

  public virtual void LogWarning(string message) => Debug.LogWarning((object) message);

  public virtual void LogWarning(string message, object context) => Debug.LogWarning((object) message, context as UnityEngine.Object);

  public virtual void LogError(string message) => Debug.LogError((object) message);

  public virtual void LogError(string message, object context) => Debug.LogError((object) message, context as UnityEngine.Object);

  public virtual void LogException(Exception exception) => Debug.LogException(exception);

  public virtual void LogException(Exception exception, object context) => Debug.LogException(exception, context as UnityEngine.Object);
}
