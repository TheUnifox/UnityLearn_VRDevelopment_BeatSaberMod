// Decompiled with JetBrains decompiler
// Type: VRTextEntryController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class VRTextEntryController : MonoBehaviour
{
  [SerializeField]
  protected UIKeyboard _uiKeyboard;
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected TextMeshProUGUI _cursorText;
  [SerializeField]
  protected int _maxLength = 20;
  [SerializeField]
  protected bool _allowBlank;
  protected bool _stopBlinkingCursor;

  public event System.Action<string> textDidChangeEvent;

  public event System.Action okButtonWasPressedEvent;

  public event System.Action cancelButtonWasPressedEvent;

  public bool hideCancelButton
  {
    set => this._uiKeyboard.hideCancelButton = value;
  }

  public string text
  {
    get => this._text.text;
    set
    {
      this._text.text = value;
      System.Action<string> textDidChangeEvent = this.textDidChangeEvent;
      if (textDidChangeEvent != null)
        textDidChangeEvent(this._text.text);
      if (this._allowBlank)
        return;
      this._uiKeyboard.enableOkButtonInteractivity = this._text.text.Length > 0;
    }
  }

  public virtual void Awake()
  {
    this._uiKeyboard.textKeyWasPressedEvent += new System.Action<char>(this.HandleUIKeyboardTextKeyWasPressed);
    this._uiKeyboard.deleteButtonWasPressedEvent += new System.Action(this.HandleUIKeyboardDeleteButtonWasPressed);
    this._uiKeyboard.okButtonWasPressedEvent += (System.Action) (() =>
    {
      System.Action buttonWasPressedEvent = this.okButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    });
    this._uiKeyboard.cancelButtonWasPressedEvent += (System.Action) (() =>
    {
      System.Action buttonWasPressedEvent = this.cancelButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    });
    this._uiKeyboard.enableOkButtonInteractivity = this._allowBlank;
  }

  public virtual void OnEnable()
  {
    this._stopBlinkingCursor = false;
    this.StartCoroutine(this.BlinkCursor());
  }

  public virtual void OnDisable() => this._stopBlinkingCursor = true;

  public virtual IEnumerator BlinkCursor()
  {
    Color cursorColor = this._cursorText.color;
    while (!this._stopBlinkingCursor)
    {
      this._cursorText.color = Color.clear;
      yield return (object) new WaitForSeconds(0.4f);
      this._cursorText.color = cursorColor;
      yield return (object) new WaitForSeconds(0.4f);
    }
  }

  public virtual void HandleUIKeyboardTextKeyWasPressed(char key)
  {
    if (this._text.text.Length < this._maxLength)
    {
      this._text.text += key.ToString().ToUpper();
      System.Action<string> textDidChangeEvent = this.textDidChangeEvent;
      if (textDidChangeEvent != null)
        textDidChangeEvent(this._text.text);
    }
    this._uiKeyboard.enableOkButtonInteractivity = true;
  }

  public virtual void HandleUIKeyboardDeleteButtonWasPressed()
  {
    if (this._text.text.Length > 0)
    {
      this._text.text = this._text.text.Remove(this._text.text.Length - 1);
      System.Action<string> textDidChangeEvent = this.textDidChangeEvent;
      if (textDidChangeEvent != null)
        textDidChangeEvent(this._text.text);
    }
    if (this._allowBlank)
      return;
    this._uiKeyboard.enableOkButtonInteractivity = this._text.text.Length > 0;
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__20_0()
  {
    System.Action buttonWasPressedEvent = this.okButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__20_1()
  {
    System.Action buttonWasPressedEvent = this.cancelButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }
}
