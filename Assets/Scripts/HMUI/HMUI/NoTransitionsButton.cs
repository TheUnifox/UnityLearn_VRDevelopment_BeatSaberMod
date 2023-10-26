// Decompiled with JetBrains decompiler
// Type: HMUI.NoTransitionsButton
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine.UI;

namespace HMUI
{
  public class NoTransitionsButton : Button
  {
    protected NoTransitionsButton.SelectionState _selectionState;

    public NoTransitionsButton.SelectionState selectionState => this._selectionState;

    public event Action<NoTransitionsButton.SelectionState> selectionStateDidChangeEvent;

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      NoTransitionsButton.SelectionState selectionState = NoTransitionsButton.SelectionState.Normal;
      switch (state)
      {
        case Selectable.SelectionState.Normal:
          selectionState = NoTransitionsButton.SelectionState.Normal;
          break;
        case Selectable.SelectionState.Highlighted:
          selectionState = NoTransitionsButton.SelectionState.Highlighted;
          break;
        case Selectable.SelectionState.Pressed:
          selectionState = NoTransitionsButton.SelectionState.Pressed;
          break;
        case Selectable.SelectionState.Disabled:
          selectionState = NoTransitionsButton.SelectionState.Disabled;
          break;
      }
      this._selectionState = selectionState;
      Action<NoTransitionsButton.SelectionState> stateDidChangeEvent = this.selectionStateDidChangeEvent;
      if (stateDidChangeEvent == null)
        return;
      stateDidChangeEvent(selectionState);
    }

    public new enum SelectionState
    {
      Normal,
      Highlighted,
      Pressed,
      Disabled,
    }
  }
}
