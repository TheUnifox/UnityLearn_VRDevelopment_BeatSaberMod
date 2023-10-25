// Decompiled with JetBrains decompiler
// Type: SelectableCellSelectableStateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;

public class SelectableCellSelectableStateController : SelectableStateController<SelectableCell>
{
  public virtual void OnEnable()
  {
    this._component.interactableChangeEvent += new System.Action<Interactable, bool>(this.HandleSelectableCellInteractableDidChange);
    this._component.highlightDidChangeEvent += new System.Action<SelectableCell, SelectableCell.TransitionType>(this.HandleSelectableCellHighlightDidChange);
    this._component.selectionDidChangeEvent += new System.Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleSelectableCellSelectionStateDidChange);
    this.ResolveState(this._component, SelectableCell.TransitionType.Instant);
  }

  public virtual void OnDisable()
  {
    this._component.interactableChangeEvent -= new System.Action<Interactable, bool>(this.HandleSelectableCellInteractableDidChange);
    this._component.highlightDidChangeEvent -= new System.Action<SelectableCell, SelectableCell.TransitionType>(this.HandleSelectableCellHighlightDidChange);
    this._component.selectionDidChangeEvent -= new System.Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleSelectableCellSelectionStateDidChange);
  }

  public virtual void HandleSelectableCellInteractableDidChange(
    Interactable interactableCell,
    bool interactable)
  {
    this.ResolveState((SelectableCell) interactableCell, SelectableCell.TransitionType.Instant);
  }

  public virtual void HandleSelectableCellHighlightDidChange(
    SelectableCell selectableCell,
    SelectableCell.TransitionType transitionType)
  {
    this.ResolveState(selectableCell, transitionType);
  }

  public virtual void HandleSelectableCellSelectionStateDidChange(
    SelectableCell selectableCell,
    SelectableCell.TransitionType transitionType,
    object owner)
  {
    this.ResolveState(selectableCell, transitionType);
  }

  public virtual void ResolveState(
    SelectableCell selectableCell,
    SelectableCell.TransitionType transitionType)
  {
    int num = !selectableCell.interactable ? 1 : 0;
    bool selected = selectableCell.selected;
    bool highlighted = selectableCell.highlighted;
    SelectableStateController.ViewState state = SelectableStateController.ViewState.Normal;
    if (num != 0)
      state = SelectableStateController.ViewState.Disabled;
    else if (selected & highlighted)
      state = SelectableStateController.ViewState.SelectedAndHighlighted;
    else if (selected)
      state = SelectableStateController.ViewState.Selected;
    else if (highlighted)
      state = SelectableStateController.ViewState.Highlighted;
    this.SetState(state, transitionType == SelectableCell.TransitionType.Animated);
  }
}
