// Decompiled with JetBrains decompiler
// Type: SelectableStateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public abstract class SelectableStateController : MonoBehaviour
{
  [Inject]
  private readonly TimeTweeningManager _tweeningManager;

  public event System.Action<SelectableStateController.ViewState, bool> stateDidChangeEvent;

  public TimeTweeningManager tweeningManager => this._tweeningManager;

  public SelectableStateController.ViewState viewState => this.currentViewState;

  private SelectableStateController.ViewState currentViewState { get; set; }

  protected void SetState(SelectableStateController.ViewState state, bool animated)
  {
    this.currentViewState = state;
    System.Action<SelectableStateController.ViewState, bool> stateDidChangeEvent = this.stateDidChangeEvent;
    if (stateDidChangeEvent == null)
      return;
    stateDidChangeEvent(state, animated);
  }

  public enum ViewState
  {
    Normal,
    Highlighted,
    Pressed,
    Disabled,
    Selected,
    SelectedAndHighlighted,
  }
}
