// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SimpleTextEditorDropdownView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using HMUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class SimpleTextEditorDropdownView : DropdownEditorView, TableView.IDataSource
  {
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private SimpleTextTableCell _cellPrefab;
    [SerializeField]
    private float _cellSize = 30f;
    [Inject]
    private readonly DiContainer _container;
    private const string kCellReuseIdentifier = "Cell";
    private IReadOnlyList<string> _texts;
    private bool _initialized;

    private void LazyInit()
    {
      if (this._initialized)
        return;
      this._initialized = true;
      this.didSelectCellWithIdxEvent += new Action<DropdownEditorView, int>(this.HandleDidSelectCellWithIdx);
      this.Init((TableView.IDataSource) this);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.didSelectCellWithIdxEvent -= new Action<DropdownEditorView, int>(this.HandleDidSelectCellWithIdx);
    }

    public void SetTexts(IReadOnlyList<string> texts)
    {
      this.LazyInit();
      this._texts = texts;
      this._text.text = this._texts.Count > this.selectedIndex ? this._texts[this.selectedIndex] : "";
      this.ReloadData();
      this.SelectCellWithIdx(this.selectedIndex);
    }

    public override void SelectCellWithIdx(int idx)
    {
      base.SelectCellWithIdx(idx);
      if (this._texts == null || this._texts != null && (this._texts.Count == 0 || this._texts.Count <= idx))
        this._text.text = "";
      else
        this._text.text = this._texts[idx];
    }

    public float CellSize() => this._cellSize;

    public int NumberOfCells()
    {
      IReadOnlyList<string> texts = this._texts;
      return texts == null ? 0 : texts.Count;
    }

    public TableCell CellForIdx(TableView tableView, int idx)
    {
      SimpleTextTableCell simpleTextTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as SimpleTextTableCell;
      if ((UnityEngine.Object) simpleTextTableCell == (UnityEngine.Object) null)
      {
        simpleTextTableCell = this._container.InstantiatePrefabForComponent<SimpleTextTableCell>((UnityEngine.Object) this._cellPrefab);
        simpleTextTableCell.reuseIdentifier = "Cell";
      }
      simpleTextTableCell.text = this._texts[idx];
      return (TableCell) simpleTextTableCell;
    }

    private void HandleDidSelectCellWithIdx(DropdownEditorView dropdownWithTableView, int idx)
    {
      if (this._texts == null || this._texts.Count == 0)
        return;
      this._text.text = this._texts[idx];
    }
  }
}
