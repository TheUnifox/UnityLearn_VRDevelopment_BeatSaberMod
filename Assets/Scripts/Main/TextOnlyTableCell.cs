// Decompiled with JetBrains decompiler
// Type: TextOnlyTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;

public class TextOnlyTableCell : TableCell
{
  [SerializeField]
  protected Color _selectedHighlightColor = new Color(0.0f, 0.7529412f, 1f, 1f);
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected ImageView _bgImage;
  [SerializeField]
  protected ImageView _highlightImage;

  public string text
  {
    get => this._text.text;
    set => this._text.text = value;
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    if (this.selected)
    {
      this._highlightImage.enabled = false;
      this._bgImage.enabled = true;
      this._text.color = this.highlighted ? this._selectedHighlightColor : Color.black;
    }
    else
    {
      this._highlightImage.enabled = this.highlighted;
      this._bgImage.enabled = false;
      this._text.color = Color.white;
    }
  }
}
