// Decompiled with JetBrains decompiler
// Type: HMUI.NoTransitionsToggle
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HMUI
{
  public class NoTransitionsToggle : Toggle
  {
    protected UISelectionState _selectionState;

    public UISelectionState selectionState => this._selectionState;

    public event Action<UISelectionState> selectionStateDidChangeEvent;

    protected override void Start()
    {
      base.Start();
      this.onValueChanged.AddListener((UnityAction<bool>) (isOn => this.DoStateTransition(this.currentSelectionState, true)));
    }

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      UISelectionState uiSelectionState = UISelectionState.Normal;
      switch (state)
      {
        case Selectable.SelectionState.Normal:
          uiSelectionState = this.isOn ? UISelectionState.Selected : UISelectionState.Normal;
          break;
        case Selectable.SelectionState.Highlighted:
          uiSelectionState = UISelectionState.Highlighted;
          break;
        case Selectable.SelectionState.Pressed:
          uiSelectionState = UISelectionState.Pressed;
          break;
        case Selectable.SelectionState.Selected:
          uiSelectionState = UISelectionState.Selected;
          break;
        case Selectable.SelectionState.Disabled:
          uiSelectionState = UISelectionState.Disabled;
          break;
      }
      if (this._selectionState == uiSelectionState)
        return;
      this._selectionState = uiSelectionState;
      Action<UISelectionState> stateDidChangeEvent = this.selectionStateDidChangeEvent;
      if (stateDidChangeEvent == null)
        return;
      stateDidChangeEvent(uiSelectionState);
    }

    [CompilerGenerated]
    public virtual void Start_b__6_0(bool isOn) => this.DoStateTransition(this.currentSelectionState, true);
  }
}
