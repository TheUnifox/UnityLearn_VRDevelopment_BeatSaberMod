// Decompiled with JetBrains decompiler
// Type: NoTransitionButtonSelectableStateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;

public class NoTransitionButtonSelectableStateController : 
  SelectableStateController<NoTransitionsButton>
{
  public virtual void OnEnable()
  {
    this._component.selectionStateDidChangeEvent += new System.Action<NoTransitionsButton.SelectionState>(this.HandleNoTransitionButtonSelectionStateDidChange);
    this.ResolveSelectionState(this._component.selectionState, false);
  }

  public virtual void OnDisable() => this._component.selectionStateDidChangeEvent -= new System.Action<NoTransitionsButton.SelectionState>(this.HandleNoTransitionButtonSelectionStateDidChange);

  public virtual void HandleNoTransitionButtonSelectionStateDidChange(
    NoTransitionsButton.SelectionState state)
  {
    this.ResolveSelectionState(state);
  }

  public virtual void ResolveSelectionState(NoTransitionsButton.SelectionState state, bool animated = true)
  {
    switch (state)
    {
      case NoTransitionsButton.SelectionState.Highlighted:
        this.SetState(SelectableStateController.ViewState.Highlighted, animated);
        break;
      case NoTransitionsButton.SelectionState.Pressed:
        this.SetState(SelectableStateController.ViewState.Pressed, animated);
        break;
      case NoTransitionsButton.SelectionState.Disabled:
        this.SetState(SelectableStateController.ViewState.Disabled, animated);
        break;
      default:
        this.SetState(SelectableStateController.ViewState.Normal, animated);
        break;
    }
  }
}
