// Decompiled with JetBrains decompiler
// Type: FileBrowserTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileBrowserTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Image _bgImage;
  [SerializeField]
  protected Image _highlightImage;

  public string text
  {
    set => this._text.text = value;
    get => this._text.text;
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType)
  {
    if (this.selected)
    {
      this._highlightImage.enabled = false;
      this._bgImage.enabled = true;
      this._text.color = Color.black;
    }
    else
    {
      this._bgImage.enabled = false;
      this._text.color = Color.white;
    }
  }

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType)
  {
    if (this.selected)
      this._highlightImage.enabled = false;
    else
      this._highlightImage.enabled = this.highlighted;
  }
}
