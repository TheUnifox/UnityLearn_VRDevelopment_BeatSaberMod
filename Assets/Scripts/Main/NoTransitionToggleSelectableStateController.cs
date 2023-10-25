// Decompiled with JetBrains decompiler
// Type: NoTransitionToggleSelectableStateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;

public class NoTransitionToggleSelectableStateController : 
  SelectableStateController<NoTransitionsToggle>
{
  public virtual void OnEnable()
  {
    this._component.selectionStateDidChangeEvent += new System.Action<UISelectionState>(this.HandleNoTransitionToggleSelectionStateDidChange);
    this.ResolveSelectionState(this._component.selectionState, false);
  }

  public virtual void OnDisable() => this._component.selectionStateDidChangeEvent -= new System.Action<UISelectionState>(this.HandleNoTransitionToggleSelectionStateDidChange);

  public virtual void HandleNoTransitionToggleSelectionStateDidChange(UISelectionState state) => this.ResolveSelectionState(state);

  public virtual void ResolveSelectionState(UISelectionState state, bool animated = true)
  {
    switch (state)
    {
      case UISelectionState.Normal:
        this.SetState(SelectableStateController.ViewState.Normal, animated);
        break;
      case UISelectionState.Highlighted:
        this.SetState(SelectableStateController.ViewState.Highlighted, animated);
        break;
      case UISelectionState.Pressed:
        this.SetState(SelectableStateController.ViewState.Pressed, animated);
        break;
      case UISelectionState.Selected:
        this.SetState(SelectableStateController.ViewState.Selected, animated);
        break;
      case UISelectionState.Disabled:
        this.SetState(SelectableStateController.ViewState.Disabled, animated);
        break;
    }
  }
}
