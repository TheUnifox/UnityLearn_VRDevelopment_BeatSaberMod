// Decompiled with JetBrains decompiler
// Type: HMTask
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class HMTask
{
  protected Thread _thread;
  protected System.Action _job;
  protected System.Action _finishCallback;
  protected bool _canceled;
  protected bool _started;
  protected bool _finished;

  public HMTask(System.Action job, System.Action finishCallback = null)
  {
    this._job = job;
    if ((UnityEngine.Object) PersistentSingleton<HMMainThreadDispatcher>.instance != (UnityEngine.Object) null)
      this._finishCallback = finishCallback;
    else
      Debug.LogError((object) "HMMainThreadDispatcher is not available Something went wrong.");
  }

  public virtual void Run()
  {
    if (this._started)
      return;
    this._started = true;
    this._thread = new Thread(new ThreadStart(this.RunJob));
    this._thread.Start();
  }

    public virtual IEnumerator RunCoroutine()
    {
        this.Run();
        WaitUntil waitUntil = new WaitUntil(() => this._finished);
        yield return waitUntil;
        yield break;
    }

    public virtual void RunJob()
  {
    this._job();
    PersistentSingleton<HMMainThreadDispatcher>.instance.Enqueue((System.Action) (() =>
    {
      if (!this._canceled && this._finishCallback != null)
        this._finishCallback();
      this._finished = true;
    }));
  }

  public virtual void Cancel() => this._canceled = true;

}
