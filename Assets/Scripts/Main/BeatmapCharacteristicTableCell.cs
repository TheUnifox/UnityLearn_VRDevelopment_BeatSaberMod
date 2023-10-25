// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatmapCharacteristicTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected Image _iconImage;
  [SerializeField]
  protected Image _bgImage;
  [SerializeField]
  protected Image _selectionImage;
  [Space]
  [SerializeField]
  protected Color _bgNormalColor = Color.black;
  [SerializeField]
  protected Color _bgHighlightColor = Color.white;

  public virtual void SetData(BeatmapCharacteristicSO beatmapCharacteristic)
  {
    this._nameText.text = Localization.Get(beatmapCharacteristic.characteristicNameLocalizationKey);
    this._iconImage.sprite = beatmapCharacteristic.icon;
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    this._bgImage.color = this.highlighted ? this._bgHighlightColor : this._bgNormalColor;
    this._selectionImage.enabled = this.selected;
  }
}
