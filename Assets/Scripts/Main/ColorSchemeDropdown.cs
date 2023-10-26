// Decompiled with JetBrains decompiler
// Type: ColorSchemeDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorSchemeDropdown : DropdownWithTableView, TableView.IDataSource
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected ColorSchemeView _colorSchemeView;
  [SerializeField]
  protected ColorSchemeTableCell _cellPrefab;
  [SerializeField]
  protected string _cellReuseIdentifier = "Cell";
  [SerializeField]
  protected float _cellSize = 8f;
  protected IReadOnlyList<ColorScheme> _colorSchemes;
  protected bool _initialized;

  public virtual void LazyInit()
  {
    if (this._initialized)
      return;
    this._initialized = true;
        base.didSelectCellWithIdxEvent += this.HandleDidSelectCellWithIdx;
        base.Init(this);
    }

  protected override void OnDestroy()
  {
    base.OnDestroy();
        base.didSelectCellWithIdxEvent -= this.HandleDidSelectCellWithIdx;
    }

  public new virtual void Init(TableView.IDataSource initTableViewDataSource) => throw new NotImplementedException();

  public virtual void SetData(IReadOnlyList<ColorScheme> colorSchemes)
  {
    this.LazyInit();
    this._colorSchemes = colorSchemes;
    if (this._colorSchemes.Count > 0)
      this.RefreshUI(this._colorSchemes[0]);
    this.ReloadData();
    this.SelectCellWithIdx(0);
  }

  public virtual float CellSize() => this._cellSize;

  public virtual int NumberOfCells()
  {
    IReadOnlyList<ColorScheme> colorSchemes = this._colorSchemes;
    return colorSchemes == null ? 0 : colorSchemes.Count;
  }

  public virtual TableCell CellForIdx(TableView tableView, int idx)
  {
    ColorSchemeTableCell colorSchemeTableCell = tableView.DequeueReusableCellForIdentifier(this._cellReuseIdentifier) as ColorSchemeTableCell;
    if ((UnityEngine.Object) colorSchemeTableCell == (UnityEngine.Object) null)
    {
      colorSchemeTableCell = UnityEngine.Object.Instantiate<ColorSchemeTableCell>(this._cellPrefab);
      colorSchemeTableCell.reuseIdentifier = this._cellReuseIdentifier;
    }
    ColorScheme colorScheme = this._colorSchemes[idx];
    colorSchemeTableCell.showEditIcon = colorScheme.isEditable;
    colorSchemeTableCell.text = !colorScheme.useNonLocalizedName ? Localization.Get(colorScheme.colorSchemeNameLocalizationKey) : colorScheme.nonLocalizedName;
    colorSchemeTableCell.SetColors(colorScheme.saberAColor, colorScheme.saberBColor, colorScheme.environmentColor0, colorScheme.environmentColor1, colorScheme.environmentColor0Boost, colorScheme.environmentColor1Boost, colorScheme.obstaclesColor);
    return (TableCell) colorSchemeTableCell;
  }

  public override void SelectCellWithIdx(int idx)
  {
    base.SelectCellWithIdx(idx);
    this.RefreshUI(this._colorSchemes[idx]);
  }

  public virtual void HandleDidSelectCellWithIdx(
    DropdownWithTableView dropdownWithTableView,
    int idx)
  {
    if (this._colorSchemes == null || this._colorSchemes.Count == 0)
      return;
    this.RefreshUI(this._colorSchemes[idx]);
  }

  public virtual void RefreshUI(ColorScheme colorScheme)
  {
    if (colorScheme.useNonLocalizedName)
      this._text.text = colorScheme.nonLocalizedName;
    else
      this._text.text = Localization.Get(colorScheme.colorSchemeNameLocalizationKey);
    this._colorSchemeView.SetColors(colorScheme.saberAColor, colorScheme.saberBColor, colorScheme.environmentColor0, colorScheme.environmentColor1, colorScheme.environmentColor0Boost, colorScheme.environmentColor1Boost, colorScheme.obstaclesColor);
  }
}
