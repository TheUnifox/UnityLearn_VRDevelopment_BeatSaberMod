// Decompiled with JetBrains decompiler
// Type: Signal
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class Signal : ScriptableObject
{
  private event System.Action _event;

  public virtual void Raise()
  {
    System.Action action = this._event;
    if (action == null)
      return;
    action();
  }

  public virtual void Subscribe(System.Action foo)
  {
    this._event -= foo;
    this._event += foo;
  }

  public virtual void Unsubscribe(System.Action foo) => this._event -= foo;
}
