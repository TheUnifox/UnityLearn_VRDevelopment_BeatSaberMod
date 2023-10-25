// Decompiled with JetBrains decompiler
// Type: LevelPackHeaderTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPackHeaderTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected Image _backgroundImage;
  [Space]
  [SerializeField]
  protected Color _highlightBackgroundColor;
  [SerializeField]
  protected Color _selectedBackgroundColor;
  [SerializeField]
  protected Color _selectedAndHighlightedBackgroundColor;

  public virtual void SetData(string headerText) => this._nameText.text = headerText;

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    if (this.selected && this.highlighted)
      this._backgroundImage.color = this._selectedAndHighlightedBackgroundColor;
    else if (this.highlighted)
      this._backgroundImage.color = this._highlightBackgroundColor;
    else if (this.selected)
      this._backgroundImage.color = this._selectedBackgroundColor;
    this._backgroundImage.enabled = this.selected || this.highlighted;
  }
}
