// Decompiled with JetBrains decompiler
// Type: HMUI.ToggleWithCallbacks
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine.UI;

namespace HMUI
{
  public class ToggleWithCallbacks : Toggle
  {
    public event Action<ToggleWithCallbacks.SelectionState> stateDidChangeEvent;

    public ToggleWithCallbacks.SelectionState selectionState
    {
      get
      {
        if (!this.IsInteractable())
          return ToggleWithCallbacks.SelectionState.Disabled;
        if (this.IsPressed())
          return ToggleWithCallbacks.SelectionState.Pressed;
        return this.IsHighlighted() ? ToggleWithCallbacks.SelectionState.Highlighted : ToggleWithCallbacks.SelectionState.Normal;
      }
    }

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      base.DoStateTransition(state, instant);
      Action<ToggleWithCallbacks.SelectionState> stateDidChangeEvent = this.stateDidChangeEvent;
      if (stateDidChangeEvent == null)
        return;
      stateDidChangeEvent((ToggleWithCallbacks.SelectionState) state);
    }

    public new enum SelectionState
    {
      Normal,
      Highlighted,
      Pressed,
      Selected,
      Disabled,
    }
  }
}
