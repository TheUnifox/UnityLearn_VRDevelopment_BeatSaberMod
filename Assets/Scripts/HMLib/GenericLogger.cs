// Decompiled with JetBrains decompiler
// Type: GenericLogger
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;

[DebuggerStepThrough]
public abstract class GenericLogger
{
  [Conditional("VERBOSE_LOGGING")]
  public static void VerboseLog(string message) => UnityEngine.Debug.Log((object) message);

  public static void Log(string message) => UnityEngine.Debug.Log((object) ("[" + DateTime.Now.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture) + "] " + message));

  public class ScopedStopwatch : IDisposable
  {
    private readonly string _processName;
    private readonly Stopwatch _stopwatch;

    public ScopedStopwatch(string processName)
    {
      this._processName = processName;
      this._stopwatch = Stopwatch.StartNew();
      if (!Application.isBatchMode)
        return;
      GenericLogger.Log(processName + " started");
    }

    public void Dispose()
    {
      this._stopwatch.Stop();
      GenericLogger.Log(string.Format("{0} finished, it took {1}s", (object) this._processName, (object) this._stopwatch.Elapsed.Seconds));
    }
  }
}
