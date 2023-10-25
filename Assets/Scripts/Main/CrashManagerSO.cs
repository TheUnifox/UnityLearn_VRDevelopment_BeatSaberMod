// Decompiled with JetBrains decompiler
// Type: CrashManagerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashManagerSO : PersistentScriptableObject
{
  protected string _logString;
  protected string _stackTrace;

  public string logString => this._logString;

  public string stackTrace => this._stackTrace;

  public virtual void StartCatchingExceptions() => Application.logMessageReceived += new Application.LogCallback(this.HandleLog);

  public virtual void OnDisable() => Application.logMessageReceived -= new Application.LogCallback(this.HandleLog);

  public virtual void HandleLog(string logString, string stackTrace, UnityEngine.LogType type)
  {
    this._logString = logString;
    this._stackTrace = stackTrace;
    if (!Application.CanStreamedLevelBeLoaded("CrashInfo"))
      return;
    SceneManager.LoadScene("CrashInfo");
  }
}
