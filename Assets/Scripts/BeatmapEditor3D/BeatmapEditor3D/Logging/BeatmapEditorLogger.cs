// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Logging.BeatmapEditorLogger
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Globalization;
using UnityEngine;

namespace BeatmapEditor3D.Logging
{
  public class BeatmapEditorLogger : ILogger, ILogHandler
  {
    public ILogHandler logHandler { get; set; }

    public bool logEnabled { get; set; }

    public LogType filterLogType { get; set; }

    public BeatmapEditorLogger(ILogHandler handler, bool enabled, LogType logType)
    {
      this.logHandler = handler;
      this.logEnabled = enabled;
      this.filterLogType = logType;
    }

    public bool IsLogTypeAllowed(LogType logType)
    {
      if (!this.logEnabled)
        return false;
      if (logType == LogType.Exception)
        return true;
      return this.filterLogType != LogType.Exception && logType <= this.filterLogType;
    }

    public void Log(LogType logType, object message)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, (UnityEngine.Object) null, "{0}", (object) BeatmapEditorLogger.GetString(message));
    }

    public void Log(LogType logType, object message, UnityEngine.Object context)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, context, "{0}", (object) BeatmapEditorLogger.GetString(message));
    }

    public void Log(LogType logType, string tag, object message)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, (UnityEngine.Object) null, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, context, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void Log(object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Log))
        return;
      this.logHandler.LogFormat(LogType.Log, (UnityEngine.Object) null, "{0}", (object) BeatmapEditorLogger.GetString(message));
    }

    public void Log(string tag, object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Log))
        return;
      this.logHandler.LogFormat(LogType.Log, (UnityEngine.Object) null, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void Log(string tag, object message, UnityEngine.Object context)
    {
      if (!this.IsLogTypeAllowed(LogType.Log))
        return;
      this.logHandler.LogFormat(LogType.Log, context, "{0}", (object) BeatmapEditorLogger.GetString(message));
    }

    public void LogWarning(string tag, object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Warning))
        return;
      this.logHandler.LogFormat(LogType.Warning, (UnityEngine.Object) null, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context)
    {
      if (!this.IsLogTypeAllowed(LogType.Warning))
        return;
      this.logHandler.LogFormat(LogType.Warning, (UnityEngine.Object) null, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void LogError(string tag, object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Error))
        return;
      this.logHandler.LogFormat(LogType.Error, (UnityEngine.Object) null, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void LogError(string tag, object message, UnityEngine.Object context)
    {
      if (!this.IsLogTypeAllowed(LogType.Error))
        return;
      this.logHandler.LogFormat(LogType.Error, context, "{0}: {1}", (object) tag, (object) BeatmapEditorLogger.GetString(message));
    }

    public void LogFormat(LogType logType, string format, params object[] args)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, (UnityEngine.Object) null, format, args);
    }

    public void LogException(Exception exception)
    {
      if (!this.logEnabled)
        return;
      this.logHandler.LogException(exception, (UnityEngine.Object) null);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
      if (!this.logEnabled)
        return;
      this.logHandler.LogException(exception, context);
    }

    private static string GetString(object message)
    {
      if (message == null)
        return "Null";
      object obj = message;
      return obj != null && obj is IFormattable formattable ? formattable.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture) : message.ToString();
    }
  }
}
