// Decompiled with JetBrains decompiler
// Type: BaseStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;

public abstract class BaseStateTransition : MonoBehaviour
{
  [SerializeField]
  protected SelectableStateController _selectableStateController;

  protected TimeTweeningManager tweeningManager => this._selectableStateController.tweeningManager;

  protected abstract BaseTransitionSO transition { get; }

  public void SetState(SelectableStateController.ViewState viewState)
  {
    switch (viewState)
    {
      case SelectableStateController.ViewState.Highlighted:
        this.SetHighlightedState();
        break;
      case SelectableStateController.ViewState.Pressed:
        this.SetPressedState();
        break;
      case SelectableStateController.ViewState.Disabled:
        this.SetDisabledState();
        break;
      case SelectableStateController.ViewState.Selected:
        this.SetSelectedState();
        break;
      case SelectableStateController.ViewState.SelectedAndHighlighted:
        this.SetSelectedAndHighlightedState();
        break;
      default:
        this.SetNormalState();
        break;
    }
  }

  protected void OnEnable()
  {
    this._selectableStateController.stateDidChangeEvent += new System.Action<SelectableStateController.ViewState, bool>(this.HandleSelectableStateControllerStateDidChange);
    this.SetState(this._selectableStateController.viewState);
  }

  protected void OnDisable() => this._selectableStateController.stateDidChangeEvent -= new System.Action<SelectableStateController.ViewState, bool>(this.HandleSelectableStateControllerStateDidChange);

  protected void OnDestroy()
  {
    if (!((UnityEngine.Object) this.tweeningManager != (UnityEngine.Object) null))
      return;
    this.tweeningManager.KillAllTweens((object) this);
  }

  private void HandleSelectableStateControllerStateDidChange(
    SelectableStateController.ViewState state,
    bool animated)
  {
    switch (state)
    {
      case SelectableStateController.ViewState.Highlighted:
        this.TransitionToHighlightedState();
        break;
      case SelectableStateController.ViewState.Pressed:
        this.TransitionToPressedState();
        break;
      case SelectableStateController.ViewState.Disabled:
        this.TransitionToDisabledState();
        break;
      case SelectableStateController.ViewState.Selected:
        this.TransitionToSelectedState();
        break;
      case SelectableStateController.ViewState.SelectedAndHighlighted:
        this.TransitionToSelectedAndHighlightedState();
        break;
      default:
        this.TransitionToNormalState();
        break;
    }
  }

  protected virtual void TransitionToNormalState()
  {
  }

  protected virtual void TransitionToHighlightedState()
  {
  }

  protected virtual void TransitionToPressedState()
  {
  }

  protected virtual void TransitionToDisabledState()
  {
  }

  protected virtual void TransitionToSelectedState()
  {
  }

  protected virtual void TransitionToSelectedAndHighlightedState()
  {
  }

  protected virtual void SetNormalState()
  {
  }

  protected virtual void SetHighlightedState()
  {
  }

  protected virtual void SetPressedState()
  {
  }

  protected virtual void SetDisabledState()
  {
  }

  protected virtual void SetSelectedState()
  {
  }

  protected virtual void SetSelectedAndHighlightedState()
  {
  }
}
