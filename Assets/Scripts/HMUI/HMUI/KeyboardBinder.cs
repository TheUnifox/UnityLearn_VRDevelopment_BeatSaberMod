// Decompiled with JetBrains decompiler
// Type: HMUI.KeyboardBinder
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace HMUI
{
  public class KeyboardBinder
  {
    [CompilerGenerated]
    protected bool enabled_k__BackingField;
    protected List<Tuple<KeyCode, KeyboardBinder.KeyBindingType, UnityAction<bool>>> _bindings;

    public bool enabled
    {
      get => this.enabled_k__BackingField;
      set => this.enabled_k__BackingField = value;
    }

    public KeyboardBinder() => this.Init();

    public KeyboardBinder(
      KeyCode keycode,
      KeyboardBinder.KeyBindingType keyBindingType,
      Action<bool> action)
    {
      this.Init();
      this.AddBinding(keycode, keyBindingType, action);
    }

    public KeyboardBinder(
      List<Tuple<KeyCode, KeyboardBinder.KeyBindingType, Action<bool>>> bindingData)
    {
      this.Init();
      this.AddBindings(bindingData);
    }

    public virtual void Init()
    {
      this.enabled = true;
      this._bindings = new List<Tuple<KeyCode, KeyboardBinder.KeyBindingType, UnityAction<bool>>>();
    }

    public virtual void AddBindings(
      List<Tuple<KeyCode, KeyboardBinder.KeyBindingType, Action<bool>>> bindingData)
    {
      foreach (Tuple<KeyCode, KeyboardBinder.KeyBindingType, Action<bool>> tuple in bindingData)
        this.AddBinding(tuple.Item1, tuple.Item2, tuple.Item3);
    }

    public virtual void AddBinding(
      KeyCode keyCode,
      KeyboardBinder.KeyBindingType keyBindingType,
      Action<bool> action)
    {
      UnityAction<bool> unityAction = new UnityAction<bool>(action.Invoke);
      this._bindings.Add(new Tuple<KeyCode, KeyboardBinder.KeyBindingType, UnityAction<bool>>(keyCode, keyBindingType, unityAction));
    }

    public virtual void ClearBindings() => this._bindings?.Clear();

    public virtual void ManualUpdate()
    {
      if (!this.enabled)
        return;
      foreach (Tuple<KeyCode, KeyboardBinder.KeyBindingType, UnityAction<bool>> tuple in this._bindings.ToArray())
      {
        if (tuple.Item2 != KeyboardBinder.KeyBindingType.KeyUp && Input.GetKeyDown(tuple.Item1))
          tuple.Item3(true);
        else if (tuple.Item2 != KeyboardBinder.KeyBindingType.KeyDown && Input.GetKeyUp(tuple.Item1))
          tuple.Item3(false);
      }
    }

    public enum KeyBindingType
    {
      KeyDown,
      KeyUp,
      KeyPress,
    }
  }
}
