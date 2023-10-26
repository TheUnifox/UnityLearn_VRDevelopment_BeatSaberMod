// Decompiled with JetBrains decompiler
// Type: HMUI.UIKeyboard
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class UIKeyboard : MonoBehaviour
  {
    [SerializeField]
    protected Button _okButton;
    protected readonly ButtonBinder _buttonBinder = new ButtonBinder();
    protected bool _shouldCapitalize;
    protected List<TextMeshProUGUI> _letterBtnTexts;

    public event Action okButtonWasPressedEvent;

    public event Action<char> keyWasPressedEvent;

    public event Action deleteButtonWasPressedEvent;

    public virtual void Awake()
    {
      this._buttonBinder.AddBinding(this._okButton, (Action) (() =>
      {
        Action buttonWasPressedEvent = this.okButtonWasPressedEvent;
        if (buttonWasPressedEvent == null)
          return;
        buttonWasPressedEvent();
      }));
      this._letterBtnTexts = new List<TextMeshProUGUI>();
      foreach (UIKeyboardKey componentsInChild in this.GetComponentsInChildren<UIKeyboardKey>())
      {
        UIKeyboardKey key = componentsInChild;
        NoTransitionsButton component = key.GetComponent<NoTransitionsButton>();
        if (key.canBeUppercase)
          this._letterBtnTexts.Add(component.GetComponentInChildren<TextMeshProUGUI>());
        switch (key.keyCode)
        {
          case KeyCode.Backspace:
            this._buttonBinder.AddBinding((Button) component, (Action) (() =>
            {
              Action buttonWasPressedEvent = this.deleteButtonWasPressedEvent;
              if (buttonWasPressedEvent == null)
                return;
              buttonWasPressedEvent();
            }));
            break;
          case KeyCode.CapsLock:
            this._buttonBinder.AddBinding((Button) component, new Action(this.HandleCapsLockPressed));
            break;
          default:
            this._buttonBinder.AddBinding((Button) component, (Action) (() => this.HandleKeyPress(key.keyCode)));
            break;
        }
      }
    }

    public virtual void HandleKeyPress(KeyCode keyCode)
    {
      int num = (int) keyCode;
      if (num > 96 && num < 123)
      {
        char c = (char) num;
        if (this._shouldCapitalize)
          c = char.ToUpper(c);
        Action<char> keyWasPressedEvent = this.keyWasPressedEvent;
        if (keyWasPressedEvent == null)
          return;
        keyWasPressedEvent(c);
      }
      else if (num > (int) byte.MaxValue && num < 266)
      {
        Action<char> keyWasPressedEvent = this.keyWasPressedEvent;
        if (keyWasPressedEvent == null)
          return;
        keyWasPressedEvent((char) (num - 208));
      }
      else
      {
        if (num != 32)
          return;
        Action<char> keyWasPressedEvent = this.keyWasPressedEvent;
        if (keyWasPressedEvent == null)
          return;
        keyWasPressedEvent(' ');
      }
    }

    public virtual void HandleCapsLockPressed()
    {
      this._shouldCapitalize = !this._shouldCapitalize;
      this.SetKeyboardCapitalization(this._shouldCapitalize);
    }

    public virtual void SetKeyboardCapitalization(bool capitalize)
    {
      for (int index = 0; index < this._letterBtnTexts.Count; ++index)
      {
        TextMeshProUGUI letterBtnText = this._letterBtnTexts[index];
        FontStyles fontStyle = this._letterBtnTexts[index].fontStyle;
        if (capitalize && !this.HasFontStyle(letterBtnText, FontStyles.UpperCase))
          fontStyle |= FontStyles.UpperCase;
        else if (!capitalize && this.HasFontStyle(letterBtnText, FontStyles.UpperCase))
          fontStyle ^= FontStyles.UpperCase;
        letterBtnText.fontStyle = fontStyle;
      }
    }

    public virtual bool HasFontStyle(TextMeshProUGUI text, FontStyles style) => (text.fontStyle & style) != 0;

    public virtual void OnEnable()
    {
      this._shouldCapitalize = false;
      this.SetKeyboardCapitalization(this._shouldCapitalize);
    }

    [CompilerGenerated]
    public virtual void Awake_b__13_0()
    {
      Action buttonWasPressedEvent = this.okButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    }

    [CompilerGenerated]
    public virtual void Awake_b__13_1()
    {
      Action buttonWasPressedEvent = this.deleteButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    }
  }
}
