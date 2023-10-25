// Decompiled with JetBrains decompiler
// Type: UIKeyboard
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIKeyboard : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProButton _keyButtonPrefab;
  protected Button _okButton;
  protected Button _cancelButton;
  protected bool _okButtonInteractivity;
  protected bool _hideCancelButton;

  public event System.Action<char> textKeyWasPressedEvent;

  public event System.Action deleteButtonWasPressedEvent;

  public event System.Action okButtonWasPressedEvent;

  public event System.Action cancelButtonWasPressedEvent;

  public bool enableOkButtonInteractivity
  {
    set
    {
      this._okButtonInteractivity = value;
      if (!((UnityEngine.Object) this._okButton != (UnityEngine.Object) null))
        return;
      this._okButton.interactable = value;
    }
  }

  public bool hideCancelButton
  {
    set
    {
      this._hideCancelButton = value;
      if (!((UnityEngine.Object) this._cancelButton != (UnityEngine.Object) null))
        return;
      this._cancelButton.gameObject.SetActive(!value);
    }
  }

  public virtual void Awake()
  {
    string[] strArray = new string[30]
    {
      "q",
      "w",
      "e",
      "r",
      "t",
      "y",
      "u",
      "i",
      "o",
      "p",
      "a",
      "s",
      "d",
      "f",
      "g",
      "h",
      "j",
      "k",
      "l",
      "z",
      "x",
      "c",
      "v",
      "b",
      "n",
      "m",
      "<-",
      "space",
      Localization.Get("BUTTON_OK"),
      Localization.Get("BUTTON_CANCEL")
    };
    for (int index = 0; index < strArray.Length; ++index)
    {
      TextMeshProButton textMeshProButton = UnityEngine.Object.Instantiate<TextMeshProButton>(this._keyButtonPrefab, (Transform) (this.transform.GetChild(index) as RectTransform));
      textMeshProButton.text.text = strArray[index];
      RectTransform transform = textMeshProButton.transform as RectTransform;
      transform.localPosition = (Vector3) Vector2.zero;
      transform.localScale = Vector3.one;
      transform.anchoredPosition = Vector2.zero;
      transform.anchorMin = Vector2.zero;
      transform.anchorMax = (Vector2) Vector3.one;
      transform.offsetMin = Vector2.zero;
      transform.offsetMax = Vector2.zero;
      Navigation navigation = textMeshProButton.button.navigation with
      {
        mode = Navigation.Mode.None
      };
      textMeshProButton.button.navigation = navigation;
      if (index < strArray.Length - 4)
      {
        string key = strArray[index];
        textMeshProButton.button.onClick.AddListener((UnityAction) (() =>
        {
          System.Action<char> keyWasPressedEvent = this.textKeyWasPressedEvent;
          if (keyWasPressedEvent == null)
            return;
          keyWasPressedEvent(key[0]);
        }));
      }
      else if (index == strArray.Length - 4)
        textMeshProButton.button.onClick.AddListener((UnityAction) (() =>
        {
          System.Action buttonWasPressedEvent = this.deleteButtonWasPressedEvent;
          if (buttonWasPressedEvent == null)
            return;
          buttonWasPressedEvent();
        }));
      else if (index == strArray.Length - 1)
      {
        this._cancelButton = textMeshProButton.button;
        this._cancelButton.gameObject.SetActive(!this._hideCancelButton);
        textMeshProButton.button.onClick.AddListener((UnityAction) (() => this.cancelButtonWasPressedEvent()));
      }
      else if (index == strArray.Length - 2)
      {
        this._okButton = textMeshProButton.button;
        this._okButton.interactable = this._okButtonInteractivity;
        textMeshProButton.button.onClick.AddListener((UnityAction) (() => this.okButtonWasPressedEvent()));
      }
      else
        textMeshProButton.button.onClick.AddListener((UnityAction) (() =>
        {
          System.Action<char> keyWasPressedEvent = this.textKeyWasPressedEvent;
          if (keyWasPressedEvent == null)
            return;
          keyWasPressedEvent(' ');
        }));
    }
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__21_0()
  {
    System.Action buttonWasPressedEvent = this.deleteButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__21_1() => this.cancelButtonWasPressedEvent();

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__21_2() => this.okButtonWasPressedEvent();

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__21_3()
  {
    System.Action<char> keyWasPressedEvent = this.textKeyWasPressedEvent;
    if (keyWasPressedEvent == null)
      return;
    keyWasPressedEvent(' ');
  }
}
