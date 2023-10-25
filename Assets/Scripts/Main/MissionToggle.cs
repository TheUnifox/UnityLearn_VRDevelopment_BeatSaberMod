// Decompiled with JetBrains decompiler
// Type: MissionToggle
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionToggle : 
  UIBehaviour,
  IPointerClickHandler,
  IEventSystemHandler,
  ISubmitHandler,
  IPointerEnterHandler,
  IPointerExitHandler
{
  [SerializeField]
  [SignalSender]
  protected Signal _missionToggleWasPressedSignal;
  [Space]
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Image _lockedImage;
  [SerializeField]
  protected Image _clearedImage;
  [SerializeField]
  protected Image _bgImage;
  [SerializeField]
  protected Image _strokeImage;
  [Space]
  [SerializeField]
  protected Interactable _vrInteractable;
  [Space]
  [SerializeField]
  protected Color _disabledColor = Color.white.ColorWithAlpha(0.1f);
  [SerializeField]
  protected Color _normalColor = Color.white;
  [SerializeField]
  protected Color _invertColor = Color.black;
  [SerializeField]
  protected Color _highlightColor = Color.blue;
  protected bool _selected;
  protected bool _highlighted;
  protected bool _interactable;
  protected bool _missionCleared;

  public event System.Action<MissionToggle> selectionDidChangeEvent;

  public bool missionCleared
  {
    set => this._missionCleared = value;
  }

  public bool selected
  {
    get => this._selected;
    set => this.ChangeSelection(value, true, false);
  }

  public bool interactable
  {
    get => this._interactable;
    set
    {
      this._interactable = value;
      this.RefreshUI();
    }
  }

  public bool highlighted => this._highlighted;

  protected override void Start()
  {
    base.Start();
    this.RefreshUI();
  }

  public virtual void ChangeSelection(
    bool value,
    bool callSelectionDidChange,
    bool ignoreCurrentValue)
  {
    if (!ignoreCurrentValue && this._selected == value)
      return;
    this._selected = value;
    this.RefreshUI();
    if (!callSelectionDidChange || this.selectionDidChangeEvent == null)
      return;
    this.selectionDidChangeEvent(this);
  }

  public virtual void ChangeHighlight(bool value, bool ignoreCurrentValue)
  {
    if (!ignoreCurrentValue && this._highlighted == value)
      return;
    this._highlighted = value;
    this.RefreshUI();
  }

  public virtual void SetText(string text) => this._text.text = text;

  public virtual void InternalToggle()
  {
    if (!this.selected)
      this.selected = true;
    this.RefreshUI();
  }

  public virtual void RefreshUI()
  {
    this._vrInteractable.interactable = this._interactable;
    if (!this._interactable)
    {
      this._bgImage.enabled = false;
      this._lockedImage.enabled = true;
      this._clearedImage.enabled = false;
      this._text.enabled = false;
      this._lockedImage.color = this._disabledColor;
      this._strokeImage.color = this._disabledColor;
      this._strokeImage.rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
    }
    else
    {
      this._lockedImage.enabled = false;
      this._clearedImage.enabled = this._missionCleared;
      this._text.enabled = true;
      this._text.color = this._selected ? this._invertColor : this._normalColor;
      this._clearedImage.color = this._text.color;
      this._strokeImage.color = this._normalColor;
      this._strokeImage.rectTransform.sizeDelta = this._highlighted || this._selected ? new Vector2(1f, 1f) : new Vector2(0.0f, 0.0f);
      this._bgImage.enabled = this._selected || this._highlighted;
      this._bgImage.color = !this._highlighted || this._selected ? this._normalColor : this._highlightColor;
    }
  }

  public virtual void OnPointerClick(PointerEventData eventData)
  {
    if (!this._interactable || eventData.button != PointerEventData.InputButton.Left)
      return;
    this.InternalToggle();
    if (!((UnityEngine.Object) this._missionToggleWasPressedSignal != (UnityEngine.Object) null))
      return;
    this._missionToggleWasPressedSignal.Raise();
  }

  public virtual void OnSubmit(BaseEventData eventData)
  {
    if (!this._interactable)
      return;
    this.InternalToggle();
    if (!((UnityEngine.Object) this._missionToggleWasPressedSignal != (UnityEngine.Object) null))
      return;
    this._missionToggleWasPressedSignal.Raise();
  }

  public virtual void OnPointerEnter(PointerEventData eventData)
  {
    if (!this._interactable)
      return;
    this.ChangeHighlight(true, false);
  }

  public virtual void OnPointerExit(PointerEventData eventData)
  {
    if (!this._interactable)
      return;
    this.ChangeHighlight(false, false);
  }
}
