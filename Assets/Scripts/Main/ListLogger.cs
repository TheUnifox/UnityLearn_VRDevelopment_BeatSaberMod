// Decompiled with JetBrains decompiler
// Type: ListLogger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public class ListLogger : IBeatSaberLogger
{
  protected readonly List<ListLogger.LogMessage> _messages;

  public List<ListLogger.LogMessage> messages => this._messages;

  public ListLogger() => this._messages = new List<ListLogger.LogMessage>();

  public virtual void Log(string message) => this.Log(message, (object) null);

  public virtual void Log(string message, object context) => this._messages.Add(new ListLogger.LogMessage(ListLogger.LogType.Info, message, context));

  public virtual void LogWarning(string message) => this.LogWarning(message, (object) null);

  public virtual void LogWarning(string message, object context) => this._messages.Add(new ListLogger.LogMessage(ListLogger.LogType.Warning, message, context));

  public virtual void LogError(string message) => this.LogError(message, (object) null);

  public virtual void LogError(string message, object context) => this._messages.Add(new ListLogger.LogMessage(ListLogger.LogType.Error, message, context));

  public virtual void LogException(Exception exception) => this.LogException(exception, (object) null);

  public virtual void LogException(Exception exception, object context) => this._messages.Add(new ListLogger.LogMessage(ListLogger.LogType.Exception, exception.Message, context));

  public enum LogType
  {
    Info,
    Warning,
    Error,
    Exception,
  }

  public class LogMessage
  {
    public readonly ListLogger.LogType type;
    public readonly string message;
    public readonly object context;

    public LogMessage(ListLogger.LogType type, string message, object context)
    {
      this.type = type;
      this.message = message;
      this.context = context;
    }

    public override string ToString() => string.Format("{0}: {1}", (object) this.type, (object) this.message);
  }
}
