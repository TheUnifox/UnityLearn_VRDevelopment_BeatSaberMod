// Decompiled with JetBrains decompiler
// Type: CompositeLogger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public class CompositeLogger : IBeatSaberLogger
{
  protected readonly List<IBeatSaberLogger> _loggers;

  public CompositeLogger() => this._loggers = new List<IBeatSaberLogger>();

  public CompositeLogger(List<IBeatSaberLogger> loggers) => this._loggers = loggers;

  public virtual void AddLogger(IBeatSaberLogger logger) => this._loggers.Add(logger);

  public virtual void Log(string message)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.Log(message);
  }

  public virtual void Log(string message, object context)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.Log(message, context);
  }

  public virtual void LogWarning(string message)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.LogWarning(message);
  }

  public virtual void LogWarning(string message, object context)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.LogWarning(message, context);
  }

  public virtual void LogError(string message)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.LogError(message);
  }

  public virtual void LogError(string message, object context)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.LogError(message, context);
  }

  public virtual void LogException(Exception exception)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.LogException(exception);
  }

  public virtual void LogException(Exception exception, object context)
  {
    foreach (IBeatSaberLogger logger in this._loggers)
      logger.LogException(exception, context);
  }
}
