// Decompiled with JetBrains decompiler
// Type: GenericSignal`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public class GenericSignal<T> : Signal
{
  protected System.Action<T> _floatEvent;

  public override void Raise()
  {
    base.Raise();
    if (this._floatEvent == null)
      return;
    this._floatEvent(default (T));
  }

  public virtual void Raise(T f)
  {
    base.Raise();
    if (this._floatEvent == null)
      return;
    this._floatEvent(f);
  }

  public virtual void Subscribe(System.Action<T> foo)
  {
    this._floatEvent -= foo;
    this._floatEvent += foo;
  }

  public virtual void Unsubscribe(System.Action<T> foo) => this._floatEvent -= foo;
}
