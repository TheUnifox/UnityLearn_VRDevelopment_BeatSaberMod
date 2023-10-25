// Decompiled with JetBrains decompiler
// Type: HMMainThreadDispatcher
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections;
using System.Collections.Generic;

public class HMMainThreadDispatcher : PersistentSingleton<HMMainThreadDispatcher>
{
  protected static Queue<System.Action> _mainThreadExecutionQueue = new Queue<System.Action>();

  public virtual void Update()
  {
    lock (HMMainThreadDispatcher._mainThreadExecutionQueue)
    {
      while (HMMainThreadDispatcher._mainThreadExecutionQueue.Count > 0)
        HMMainThreadDispatcher._mainThreadExecutionQueue.Dequeue()();
    }
  }

  public virtual void Enqueue(IEnumerator action)
  {
    lock (HMMainThreadDispatcher._mainThreadExecutionQueue)
      HMMainThreadDispatcher._mainThreadExecutionQueue.Enqueue((System.Action) (() => this.StartCoroutine(action)));
  }

  public virtual void Enqueue(System.Action action) => this.Enqueue(this.ActionCoroutine(action));

  public virtual IEnumerator ActionCoroutine(System.Action action)
  {
    action();
    yield return (object) null;
  }
}
