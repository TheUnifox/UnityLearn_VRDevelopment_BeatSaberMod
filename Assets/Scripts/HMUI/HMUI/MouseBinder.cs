// Decompiled with JetBrains decompiler
// Type: HMUI.MouseBinder
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
  public class MouseBinder
  {
    [CompilerGenerated]
    protected bool enabled_k__BackingField;
    protected List<UnityAction<float>> _scrollBindings;
    protected List<(MouseBinder.ButtonType buttonType, MouseBinder.MouseEventType mouseEventType, UnityAction action)> _buttonBindings;

    public bool enabled
    {
      get => this.enabled_k__BackingField;
      set => this.enabled_k__BackingField = value;
    }

    public MouseBinder() => this.Init();

    public virtual void Init()
    {
      this.enabled = true;
      this._scrollBindings = new List<UnityAction<float>>();
      this._buttonBindings = new List<(MouseBinder.ButtonType, MouseBinder.MouseEventType, UnityAction)>();
    }

    public virtual void AddScrollBindings(List<UnityAction<float>> bindingData)
    {
      foreach (UnityAction<float> action in bindingData)
        this.AddScrollBinding(action);
    }

    public virtual void AddScrollBinding(UnityAction<float> action) => this._scrollBindings.Add(action);

    public virtual void RemoveScrollBinding(UnityAction<float> action) => this._scrollBindings.Remove(action);

    public virtual void AddButtonBindings(
      List<Tuple<MouseBinder.ButtonType, MouseBinder.MouseEventType, UnityAction>> bindingData)
    {
      foreach (Tuple<MouseBinder.ButtonType, MouseBinder.MouseEventType, UnityAction> tuple in bindingData)
        this.AddButtonBinding(tuple.Item1, tuple.Item2, tuple.Item3);
    }

    public virtual void AddButtonBinding(
      MouseBinder.ButtonType buttonType,
      MouseBinder.MouseEventType keyBindingType,
      UnityAction action)
    {
      this._buttonBindings.Add((buttonType, keyBindingType, action));
    }

    public virtual void RemoveButtonBinding(
      MouseBinder.ButtonType buttonType,
      MouseBinder.MouseEventType keyBindingType,
      UnityAction action)
    {
      this._buttonBindings.Remove((buttonType, keyBindingType, action));
    }

    public virtual void ClearBindings()
    {
      this._buttonBindings?.Clear();
      this._scrollBindings?.Clear();
    }

    public virtual void ManualUpdate()
    {
      if (!this.enabled)
        return;
      float y = Input.mouseScrollDelta.y;
      if (!Mathf.Approximately(y, 0.0f))
      {
        foreach (UnityAction<float> scrollBinding in this._scrollBindings)
          scrollBinding(y);
      }
      foreach ((MouseBinder.ButtonType buttonType, MouseBinder.MouseEventType mouseEventType, UnityAction action) buttonBinding in this._buttonBindings)
      {
        switch (buttonBinding.mouseEventType)
        {
          case MouseBinder.MouseEventType.ButtonDown:
            if (Input.GetMouseButtonDown((int) buttonBinding.buttonType))
            {
              buttonBinding.action();
              continue;
            }
            continue;
          case MouseBinder.MouseEventType.ButtonUp:
            if (Input.GetMouseButtonUp((int) buttonBinding.buttonType))
            {
              buttonBinding.action();
              continue;
            }
            continue;
          case MouseBinder.MouseEventType.ButtonPress:
            if (Input.GetMouseButton((int) buttonBinding.buttonType))
            {
              buttonBinding.action();
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }

    public enum MouseEventType
    {
      ButtonDown,
      ButtonUp,
      ButtonPress,
    }

    public enum ButtonType
    {
      Primary,
      Secondary,
      Middle,
    }
  }
}
