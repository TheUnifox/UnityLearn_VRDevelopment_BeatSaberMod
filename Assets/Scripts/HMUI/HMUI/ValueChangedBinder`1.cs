// Decompiled with JetBrains decompiler
// Type: HMUI.ValueChangedBinder`1
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;

namespace HMUI
{
  public class ValueChangedBinder<T>
  {
    protected List<Tuple<IValueChanger<T>, Action<T>>> _bindings;

    public ValueChangedBinder() => this.Init();

    public ValueChangedBinder(IValueChanger<T> valueChanger, Action<T> action)
    {
      this.Init();
      this.AddBinding(valueChanger, action);
    }

    public ValueChangedBinder(
      List<Tuple<IValueChanger<T>, Action<T>>> bindingData)
    {
      this.Init();
      this.AddBindings(bindingData);
    }

    public virtual void Init() => this._bindings = new List<Tuple<IValueChanger<T>, Action<T>>>();

    public virtual void AddBindings(
      List<Tuple<IValueChanger<T>, Action<T>>> bindingData)
    {
      foreach (Tuple<IValueChanger<T>, Action<T>> tuple in bindingData)
      {
        IValueChanger<T> valueChanger;
        Action<T> action;
        TupleExtensions.Deconstruct<IValueChanger<T>, Action<T>>(tuple, out valueChanger, out action);
        this.AddBinding(valueChanger, action);
      }
    }

    public virtual void AddBinding(IValueChanger<T> valueChanger, Action<T> action)
    {
      valueChanger.valueChangedEvent += action;
      this._bindings.Add<IValueChanger<T>, Action<T>>(valueChanger, action);
    }

    public virtual void ClearBindings()
    {
      if (this._bindings == null)
        return;
      foreach (Tuple<IValueChanger<T>, Action<T>> binding in this._bindings)
      {
        IValueChanger<T> valueChanger1;
        Action<T> action1;
        TupleExtensions.Deconstruct<IValueChanger<T>, Action<T>>(binding, out valueChanger1, out action1);
        IValueChanger<T> valueChanger2 = valueChanger1;
        Action<T> action2 = action1;
        if (valueChanger2 != null)
          valueChanger2.valueChangedEvent -= action2;
      }
      this._bindings.Clear();
    }
  }
}
