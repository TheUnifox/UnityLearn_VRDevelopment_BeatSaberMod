// Decompiled with JetBrains decompiler
// Type: HMUI.SelectableCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public abstract class SelectableCell : 
    Interactable,
    IPointerClickHandler,
    IEventSystemHandler,
    ISubmitHandler,
    IPointerEnterHandler,
    IPointerExitHandler
  {
    [SerializeField]
    [SignalSender]
    private Signal _wasPressedSignal;

    public event Action<SelectableCell, SelectableCell.TransitionType, object> selectionDidChangeEvent;

    public event Action<SelectableCell, SelectableCell.TransitionType> highlightDidChangeEvent;

    public bool highlighted { get; private set; }

    public bool selected { get; private set; }

    protected virtual void Start()
    {
      this.SelectionDidChange(SelectableCell.TransitionType.Instant);
      this.HighlightDidChange(SelectableCell.TransitionType.Instant);
    }

    public void SetSelected(
      bool value,
      SelectableCell.TransitionType transitionType,
      object changeOwner,
      bool ignoreCurrentValue)
    {
      if (!ignoreCurrentValue && this.selected == value)
        return;
      this.selected = value;
      this.SelectionDidChange(transitionType);
      Action<SelectableCell, SelectableCell.TransitionType, object> selectionDidChangeEvent = this.selectionDidChangeEvent;
      if (selectionDidChangeEvent == null)
        return;
      selectionDidChangeEvent(this, transitionType, changeOwner);
    }

    public void ClearHighlight(SelectableCell.TransitionType transitionType) => this.SetHighlight(false, transitionType, false);

    private void SetHighlight(
      bool value,
      SelectableCell.TransitionType transitionType,
      bool ignoreCurrentValue)
    {
      if (!ignoreCurrentValue && this.highlighted == value)
        return;
      this.highlighted = value;
      this.HighlightDidChange(transitionType);
      Action<SelectableCell, SelectableCell.TransitionType> highlightDidChangeEvent = this.highlightDidChangeEvent;
      if (highlightDidChangeEvent == null)
        return;
      highlightDidChangeEvent(this, transitionType);
    }

    protected abstract void InternalToggle();

    protected virtual void SelectionDidChange(SelectableCell.TransitionType transitionType)
    {
    }

    protected virtual void HighlightDidChange(SelectableCell.TransitionType transitionType)
    {
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left || !this.interactable)
        return;
      this.InternalToggle();
      if (!((UnityEngine.Object) this._wasPressedSignal != (UnityEngine.Object) null))
        return;
      this._wasPressedSignal.Raise();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
      if (!this.interactable)
        return;
      this.InternalToggle();
      if (!((UnityEngine.Object) this._wasPressedSignal != (UnityEngine.Object) null))
        return;
      this._wasPressedSignal.Raise();
    }

    public virtual void OnPointerEnter(PointerEventData eventData) => this.SetHighlight(true, SelectableCell.TransitionType.Animated, false);

    public virtual void OnPointerExit(PointerEventData eventData) => this.SetHighlight(false, SelectableCell.TransitionType.Animated, false);

    public enum TransitionType
    {
      Instant,
      Animated,
    }
  }
}
