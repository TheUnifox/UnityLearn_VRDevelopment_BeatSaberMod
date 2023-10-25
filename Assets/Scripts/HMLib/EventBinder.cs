// Decompiled with JetBrains decompiler
// Type: EventBinder
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;

public class EventBinder
{
  protected List<System.Action> _unsubscribes = new List<System.Action>();

  public virtual void Bind(System.Action subscribe, System.Action unsubscribe)
  {
    subscribe();
    this._unsubscribes.Add(unsubscribe);
  }

  public virtual void ClearAllBindings()
  {
    foreach (System.Action unsubscribe in this._unsubscribes)
      unsubscribe();
    this._unsubscribes.Clear();
  }
}
