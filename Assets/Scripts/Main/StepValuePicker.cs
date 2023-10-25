// Decompiled with JetBrains decompiler
// Type: StepValuePicker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StepValuePicker : MonoBehaviour
{
  [SerializeField]
  protected Button _decButton;
  [SerializeField]
  protected Button _incButton;
  [SerializeField]
  protected TextMeshProUGUI _valueText;

  public event System.Action decButtonWasPressedEvent;

  public event System.Action incButtonWasPressedEvent;

  public string text
  {
    get => this._valueText.text;
    set => this._valueText.text = value;
  }

  public bool decButtonInteractable
  {
    set => this._decButton.interactable = value;
  }

  public bool incButtonInteractable
  {
    set => this._incButton.interactable = value;
  }

  protected virtual void OnEnable()
  {
    this._incButton.onClick.AddListener(new UnityAction(this.IncButtonPressed));
    this._decButton.onClick.AddListener(new UnityAction(this.DecButtonPressed));
  }

  public virtual void OnDisable()
  {
    this._incButton.onClick.RemoveListener(new UnityAction(this.IncButtonPressed));
    this._decButton.onClick.RemoveListener(new UnityAction(this.DecButtonPressed));
  }

  public virtual void IncButtonPressed()
  {
    System.Action buttonWasPressedEvent = this.incButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }

  public virtual void DecButtonPressed()
  {
    System.Action buttonWasPressedEvent = this.decButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }
}
