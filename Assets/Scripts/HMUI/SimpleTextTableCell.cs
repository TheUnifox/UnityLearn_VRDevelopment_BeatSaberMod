// Decompiled with JetBrains decompiler
// Type: SimpleTextTableCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using HMUI;
using TMPro;
using UnityEngine;

public class SimpleTextTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _text;

  public string text
  {
    set => this._text.text = value;
    get => this._text.text;
  }
}
