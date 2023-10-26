// Decompiled with JetBrains decompiler
// Type: HMUI.InputFieldViewChangeBinder
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace HMUI
{
  public class InputFieldViewChangeBinder
  {
    protected List<Tuple<InputFieldView, UnityAction<InputFieldView>>> _bindings;
    protected bool _enabled = true;

    public InputFieldViewChangeBinder() => this.Init();

    public virtual void Init() => this._bindings = new List<Tuple<InputFieldView, UnityAction<InputFieldView>>>();

    public virtual void AddBindings(
      List<Tuple<InputFieldView, Action<InputFieldView>>> bindings)
    {
      foreach (Tuple<InputFieldView, Action<InputFieldView>> binding in bindings)
        this.AddBinding(binding.Item1, binding.Item2);
    }

    public virtual void AddBinding(InputFieldView inputField, Action<InputFieldView> action)
    {
      UnityAction<InputFieldView> call = new UnityAction<InputFieldView>(action.Invoke);
      inputField.onValueChanged.AddListener(call);
      this._bindings.Add<InputFieldView, UnityAction<InputFieldView>>(inputField, call);
    }

    public virtual void ClearBindings()
    {
      if (this._bindings == null)
        return;
      foreach (Tuple<InputFieldView, UnityAction<InputFieldView>> binding in this._bindings)
      {
        InputFieldView inputFieldView = binding.Item1;
        if ((UnityEngine.Object) inputFieldView != (UnityEngine.Object) null)
          inputFieldView.onValueChanged.RemoveListener(binding.Item2);
      }
      this._bindings.Clear();
    }

    public virtual void Disable()
    {
      if (!this._enabled)
        return;
      this._enabled = false;
      if (this._bindings == null)
        return;
      foreach (Tuple<InputFieldView, UnityAction<InputFieldView>> binding in this._bindings)
      {
        InputFieldView inputFieldView = binding.Item1;
        if ((UnityEngine.Object) inputFieldView != (UnityEngine.Object) null)
          inputFieldView.onValueChanged.RemoveListener(binding.Item2);
      }
    }

    public virtual void Enable()
    {
      if (this._enabled)
        return;
      this._enabled = true;
      if (this._bindings == null)
        return;
      foreach (Tuple<InputFieldView, UnityAction<InputFieldView>> binding in this._bindings)
      {
        InputFieldView inputFieldView = binding.Item1;
        if ((UnityEngine.Object) inputFieldView != (UnityEngine.Object) null)
          inputFieldView.onValueChanged.AddListener(binding.Item2);
      }
    }
  }
}
