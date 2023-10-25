// Decompiled with JetBrains decompiler
// Type: GuestNameButtonsListItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuestNameButtonsListItem : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected Button _button;
  protected System.Action _buttonPressed;

  public string nameText
  {
    set => this._nameText.text = value;
  }

  public System.Action buttonPressed
  {
    set => this._buttonPressed = value;
  }

  public virtual void Awake() => this._button.onClick.AddListener((UnityAction) (() =>
  {
    System.Action buttonPressed = this._buttonPressed;
    if (buttonPressed == null)
      return;
    buttonPressed();
  }));

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__7_0()
  {
    System.Action buttonPressed = this._buttonPressed;
    if (buttonPressed == null)
      return;
    buttonPressed();
  }
}
