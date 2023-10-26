// Decompiled with JetBrains decompiler
// Type: HMUI.InputFieldDataBinder
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HMUI
{
  public class InputFieldDataBinder
  {
    protected List<Tuple<InputField, IObservableChange, UnityAction<string>, Action>> _bindings;

    public InputFieldDataBinder() => this._bindings = new List<Tuple<InputField, IObservableChange, UnityAction<string>, Action>>();

    public virtual void AddBindings<T0, T1>(
      List<Tuple<InputField, T0, Func<string, T1>, Func<T1, string>>> bindingData)
      where T0 : IObservableChange, IValue<T1>
    {
      foreach (Tuple<InputField, T0, Func<string, T1>, Func<T1, string>> tuple in bindingData)
      {
        InputField inputField = tuple.Item1;
        T0 valueItem = tuple.Item2;
        Func<string, T1> toValueConvertor = tuple.Item3;
        Func<T1, string> toStringConvertor = tuple.Item4;
        UnityAction<string> call = (UnityAction<string>) (value =>
        {
          T1 obj = toValueConvertor(value);
          if (obj.Equals((object) valueItem.value))
            inputField.text = toStringConvertor(obj);
          else
            valueItem.value = obj;
        });
        Action action = (Action) (() => inputField.text = toStringConvertor(valueItem.value));
        inputField.onEndEdit.AddListener(call);
        valueItem.didChangeEvent += action;
        this._bindings.Add<InputField, IObservableChange, UnityAction<string>, Action>(tuple.Item1, (IObservableChange) tuple.Item2, call, action);
        action();
      }
    }

    public virtual void AddStringBindings<T>(List<Tuple<InputField, T>> bindingData) where T : IObservableChange, IValue<string>
    {
      List<Tuple<InputField, T, Func<string, string>, Func<string, string>>> tupleList = new List<Tuple<InputField, T, Func<string, string>, Func<string, string>>>();
      Func<string, string> func = (Func<string, string>) (value => value);
      foreach (Tuple<InputField, T> tuple in bindingData)
        tupleList.Add<InputField, T, Func<string, string>, Func<string, string>>(tuple.Item1, tuple.Item2, func, func);
      this.AddBindings<T, string>(tupleList);
    }

    public virtual void ClearBindings()
    {
      if (this._bindings == null)
        return;
      foreach (Tuple<InputField, IObservableChange, UnityAction<string>, Action> binding in this._bindings)
      {
        InputField inputField = binding.Item1;
        IObservableChange observableChange = binding.Item2;
        if ((UnityEngine.Object) inputField != (UnityEngine.Object) null)
          inputField.onEndEdit.RemoveListener(binding.Item3);
        if (observableChange != null)
          observableChange.didChangeEvent -= binding.Item4;
      }
      this._bindings.Clear();
    }
  }
}
