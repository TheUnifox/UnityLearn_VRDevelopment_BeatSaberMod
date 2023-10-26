// Decompiled with JetBrains decompiler
// Type: SimpleTextWithIconTableCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTextWithIconTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Image _icon;

  public Image icon
  {
    set => this._icon = value;
    get => this._icon;
  }

  public string text
  {
    set => this._text.text = value;
    get => this._text.text;
  }
}
