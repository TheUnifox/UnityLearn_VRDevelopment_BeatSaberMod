// Decompiled with JetBrains decompiler
// Type: HMUI.ToggleBinder
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HMUI
{
  public class ToggleBinder
  {
    protected List<Tuple<Toggle, UnityAction<bool>>> _bindings;
    protected bool _enabled = true;

    public ToggleBinder() => this.Init();

    public ToggleBinder(List<Tuple<Toggle, Action<bool>>> bindingData)
    {
      this.Init();
      this.AddBindings(bindingData);
    }

    public virtual void Init() => this._bindings = new List<Tuple<Toggle, UnityAction<bool>>>();

    public virtual void AddBindings(List<Tuple<Toggle, Action<bool>>> bindingData)
    {
      foreach (Tuple<Toggle, Action<bool>> tuple in bindingData)
        this.AddBinding(tuple.Item1, tuple.Item2);
    }

    public virtual void AddBinding(Toggle toggle, Action<bool> action)
    {
      UnityAction<bool> call = new UnityAction<bool>(action.Invoke);
      toggle.onValueChanged.AddListener(call);
      this._bindings.Add<Toggle, UnityAction<bool>>(toggle, call);
    }

    public virtual void AddBinding(Toggle toggle, bool enabled, Action action)
    {
      UnityAction<bool> call = (UnityAction<bool>) (b =>
      {
        if (b != enabled)
          return;
        action();
      });
      toggle.onValueChanged.AddListener(call);
      this._bindings.Add<Toggle, UnityAction<bool>>(toggle, call);
    }

    public virtual void ClearBindings()
    {
      if (this._bindings == null)
        return;
      foreach (Tuple<Toggle, UnityAction<bool>> binding in this._bindings)
      {
        Toggle toggle = binding.Item1;
        if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
          toggle.onValueChanged.RemoveListener(binding.Item2);
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
      foreach (Tuple<Toggle, UnityAction<bool>> binding in this._bindings)
      {
        Toggle toggle = binding.Item1;
        if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
          toggle.onValueChanged.RemoveListener(binding.Item2);
      }
    }

    public virtual void Enable()
    {
      if (this._enabled)
        return;
      this._enabled = true;
      if (this._bindings == null)
        return;
      foreach (Tuple<Toggle, UnityAction<bool>> binding in this._bindings)
      {
        Toggle toggle = binding.Item1;
        if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
          toggle.onValueChanged.AddListener(binding.Item2);
      }
    }
  }
}
