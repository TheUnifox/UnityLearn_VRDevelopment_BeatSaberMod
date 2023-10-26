// Decompiled with JetBrains decompiler
// Type: HMUI.SimpleTextDropdown
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HMUI
{
  public class SimpleTextDropdown : DropdownWithTableView, TableView.IDataSource
  {
    [SerializeField]
    protected TextMeshProUGUI _text;
    [SerializeField]
    protected SimpleTextTableCell _cellPrefab;
    [SerializeField]
    protected float _cellSize = 8f;
    protected const string kCellReuseIdentifier = "Cell";
    protected IReadOnlyList<string> _texts;
    protected bool _initialized;

    public virtual void LazyInit()
    {
      if (this._initialized)
        return;
      this._initialized = true;
      this.didSelectCellWithIdxEvent += new Action<DropdownWithTableView, int>(this.HandleDidSelectCellWithIdx);
      base.Init((TableView.IDataSource) this);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.didSelectCellWithIdxEvent -= new Action<DropdownWithTableView, int>(this.HandleDidSelectCellWithIdx);
    }

    public new virtual void Init(TableView.IDataSource initTableViewDataSource) => throw new NotImplementedException();

    public virtual void SetTexts(IReadOnlyList<string> texts)
    {
      this.LazyInit();
      this._texts = texts;
      this._text.text = ((IReadOnlyCollection<string>) this._texts).Count > this.selectedIndex ? this._texts[this.selectedIndex] : "";
      this.ReloadData();
      this.SelectCellWithIdx(this.selectedIndex);
    }

    public override void SelectCellWithIdx(int idx)
    {
      base.SelectCellWithIdx(idx);
      if (this._texts == null || this._texts != null && (((IReadOnlyCollection<string>) this._texts).Count == 0 || ((IReadOnlyCollection<string>) this._texts).Count <= idx))
        this._text.text = "";
      else
        this._text.text = this._texts[idx];
    }

    public virtual float CellSize() => this._cellSize;

    public virtual int NumberOfCells()
    {
      IReadOnlyList<string> texts = this._texts;
      return texts == null ? 0 : ((IReadOnlyCollection<string>) texts).Count;
    }

    public virtual TableCell CellForIdx(TableView tableView, int idx)
    {
      SimpleTextTableCell simpleTextTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as SimpleTextTableCell;
      if ((UnityEngine.Object) simpleTextTableCell == (UnityEngine.Object) null)
      {
        simpleTextTableCell = UnityEngine.Object.Instantiate<SimpleTextTableCell>(this._cellPrefab);
        simpleTextTableCell.reuseIdentifier = "Cell";
      }
      simpleTextTableCell.text = this._texts[idx];
      return (TableCell) simpleTextTableCell;
    }

    public virtual void HandleDidSelectCellWithIdx(
      DropdownWithTableView dropdownWithTableView,
      int idx)
    {
      if (this._texts == null || ((IReadOnlyCollection<string>) this._texts).Count == 0)
        return;
      this._text.text = this._texts[idx];
    }
  }
}
