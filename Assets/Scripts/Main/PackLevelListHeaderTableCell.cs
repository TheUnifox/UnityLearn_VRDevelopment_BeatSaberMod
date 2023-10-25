// Decompiled with JetBrains decompiler
// Type: PackLevelListHeaderTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackLevelListHeaderTableCell : TableCell
{
  [SerializeField]
  protected Color _selectedHighlightElementsColor = new Color(0.0f, 0.7529412f, 1f, 1f);
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Image _bgImage;
  [SerializeField]
  protected Image _highlightImage;
  [SerializeField]
  protected Image _arrowImage;
  protected CancellationTokenSource _cancellationTokenSource;

  public string text
  {
    set => this._text.text = value;
    get => this._text.text;
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    this._highlightImage.enabled = this.highlighted;
    this._bgImage.enabled = !this.highlighted;
    this._text.color = Color.white;
    this._arrowImage.color = this._selectedHighlightElementsColor;
  }
}
