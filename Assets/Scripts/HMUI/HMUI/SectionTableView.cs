// Decompiled with JetBrains decompiler
// Type: HMUI.SectionTableView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class SectionTableView : TableView, TableView.IDataSource
  {
    [SerializeField]
    protected bool _unfoldSectionsByDefault;
    protected new SectionTableView.IDataSource _dataSource;
    protected SectionTableView.Section[] _sections;

    public event Action<SectionTableView, int, int> didSelectRowInSectionEvent;

    public event Action<SectionTableView, int> didSelectHeaderEvent;

    public new SectionTableView.IDataSource dataSource
    {
      get => this._dataSource;
      set
      {
        if (this._dataSource == value)
          return;
        this._dataSource = value;
        base._dataSource = (TableView.IDataSource) this;
        this.ReloadData();
      }
    }

    public virtual bool IsSectionUnfolded(int section) => this._sections[section].unfolded;

    public virtual float CellSize() => this._dataSource.RowHeight();

    public virtual int NumberOfCells() => this._sections == null || this._sections.Length == 0 ? 0 : this._sections[this._sections.Length - 1].startBaseRow + this._sections[this._sections.Length - 1].numberOfBaseRows;

    public virtual TableCell CellForIdx(TableView tableView, int baseRow)
    {
      int section;
      int row;
      bool isSectionHeader;
      this.SectionAndRowForBaseRow(baseRow, out section, out row, out isSectionHeader);
      return isSectionHeader ? this._dataSource.CellForSectionHeader(section, this._sections[section].unfolded) : this._dataSource.CellForRowInSection(section, row);
    }

    public override void ReloadData() => this.ReloadData(true);

    public virtual void ReloadData(bool resetFoldState)
    {
      int length = this._dataSource.NumberOfSections();
      if (this._sections == null || this._sections.Length != length)
        this._sections = new SectionTableView.Section[length];
      if (resetFoldState)
      {
        for (int index = 0; index < this._sections.Length; ++index)
          this._sections[index].unfolded = this._unfoldSectionsByDefault;
      }
      int num1 = 0;
      for (int section = 0; section < this._sections.Length; ++section)
      {
        this._sections[section].startBaseRow = num1;
        int num2 = this._dataSource.NumberOfRowsInSection(section);
        this._sections[section].numberOfBaseRows = !this._sections[section].unfolded ? 1 : num2 + 1;
        num1 += this._sections[section].numberOfBaseRows;
      }
      base.ReloadData();
    }

    protected override void DidSelectCellWithIdx(int baseRow)
    {
      int section;
      int row;
      bool isSectionHeader;
      this.SectionAndRowForBaseRow(baseRow, out section, out row, out isSectionHeader);
      if (isSectionHeader)
      {
        Action<SectionTableView, int> selectHeaderEvent = this.didSelectHeaderEvent;
        if (selectHeaderEvent == null)
          return;
        selectHeaderEvent(this, section);
      }
      else
      {
        Action<SectionTableView, int, int> rowInSectionEvent = this.didSelectRowInSectionEvent;
        if (rowInSectionEvent == null)
          return;
        rowInSectionEvent(this, row, section);
      }
    }

    public virtual void UnfoldAllSections()
    {
      for (int index = 0; index < this._sections.Length; ++index)
        this._sections[index].unfolded = true;
      this.ReloadData(false);
    }

    public virtual void FoldAll()
    {
      for (int index = 0; index < this._sections.Length; ++index)
        this._sections[index].unfolded = false;
      this.ReloadData(false);
    }

    public virtual void UnfoldSection(int section)
    {
      if (this._sections[section].unfolded)
        return;
      this._sections[section].unfolded = true;
      int count = this._dataSource.NumberOfRowsInSection(section);
      this._sections[section].numberOfBaseRows = count + 1;
      for (int index = section + 1; index < this._sections.Length; ++index)
        this._sections[index].startBaseRow += count;
      this.InsertCells(this._sections[section].startBaseRow + 1, count);
    }

    public virtual void FoldSection(int section)
    {
      if (!this._sections[section].unfolded)
        return;
      int numberOfBaseRows = this._sections[section].numberOfBaseRows;
      this._sections[section].unfolded = false;
      this._sections[section].numberOfBaseRows = 1;
      for (int index = section + 1; index < this._sections.Length; ++index)
        this._sections[index].startBaseRow -= numberOfBaseRows - 1;
      this.DeleteCells(this._sections[section].startBaseRow + 1, numberOfBaseRows - 1);
    }

    public virtual void ScrollToRow(
      int section,
      int row,
      TableView.ScrollPositionType scrollPositionType,
      bool animated)
    {
      this.ScrollToCellWithIdx(this._sections[section].startBaseRow + row, scrollPositionType, animated);
    }

    public virtual void SectionAndRowForBaseRow(
      int baseRow,
      out int section,
      out int row,
      out bool isSectionHeader)
    {
      int num1 = 0;
      int num2 = this._sections.Length - 1;
      section = this._sections.Length / 2;
      bool flag;
      do
      {
        flag = false;
        if (baseRow < this._sections[section].startBaseRow)
        {
          section = (section + num1) / 2;
          num2 = section - 1;
          flag = true;
        }
        else if (baseRow >= this._sections[section].startBaseRow + this._sections[section].numberOfBaseRows)
        {
          section = (section + 1 + num2) / 2;
          num1 = section + 1;
          flag = true;
        }
      }
      while (flag);
      row = baseRow - this._sections[section].startBaseRow;
      if (row == 0)
      {
        isSectionHeader = true;
      }
      else
      {
        isSectionHeader = false;
        --row;
      }
    }

    public new interface IDataSource
    {
      float RowHeight();

      int NumberOfSections();

      int NumberOfRowsInSection(int section);

      TableCell CellForSectionHeader(int section, bool unfolded);

      TableCell CellForRowInSection(int section, int row);
    }

    public struct Section
    {
      public bool unfolded;
      public int startBaseRow;
      public int numberOfBaseRows;
    }
  }
}
