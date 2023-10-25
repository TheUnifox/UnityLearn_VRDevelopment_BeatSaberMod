// Decompiled with JetBrains decompiler
// Type: SimpleFileLogger
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SimpleFileLogger
{
  protected const string kFilename = "SimpleLog.txt";
  protected static SimpleFileLogger.MonoLogger _monoLogger;

  private static SimpleFileLogger.MonoLogger monoLogger
  {
    get
    {
      if ((UnityEngine.Object) SimpleFileLogger._monoLogger == (UnityEngine.Object) null)
      {
        SimpleFileLogger._monoLogger = new GameObject("MonoLogger").AddComponent<SimpleFileLogger.MonoLogger>();
        SimpleFileLogger._monoLogger.Clear();
      }
      return SimpleFileLogger._monoLogger;
    }
  }

  public static void Log(string text) => SimpleFileLogger.monoLogger.Log(text);

  public static void LogVector(string description, Vector3 vec) => SimpleFileLogger.monoLogger.Log(description + ": " + vec.x.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ", " + vec.y.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ", " + vec.z.ToString((IFormatProvider) CultureInfo.InvariantCulture));

  public static void Clear() => SimpleFileLogger.monoLogger.Clear();

  public class MonoLogger : MonoBehaviour
  {
    protected readonly List<string> _lines = new List<string>();

    public virtual void OnDestroy() => File.AppendAllLines("SimpleLog.txt", (IEnumerable<string>) this._lines);

    public virtual void Log(string text) => this._lines.Add(text);

    public virtual void Clear()
    {
      this._lines.Clear();
      File.Delete("SimpleLog.txt");
    }
  }
}
