// Decompiled with JetBrains decompiler
// Type: HMUI.TableCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class TableCell : SelectableCell
  {
    [CompilerGenerated]
    protected int idx_k__BackingField;
    protected string _reuseIdentifier;
    protected ITableCellOwner _tableCellOwner;

    public string reuseIdentifier
    {
      get => this._reuseIdentifier;
      set => this._reuseIdentifier = value;
    }

    public int idx
    {
      get => this.idx_k__BackingField;
      private set => this.idx_k__BackingField = value;
    }

    protected ITableCellOwner tableCellOwner => this._tableCellOwner;

    public virtual void TableViewSetup(ITableCellOwner tableCellOwner, int idx)
    {
      this._tableCellOwner = tableCellOwner;
      this.idx = idx;
    }

    public virtual void MoveIdx(int offset) => this.idx += offset;

    protected override void InternalToggle()
    {
      if (this._tableCellOwner.selectionType == TableViewSelectionType.None)
        return;
      if (this.selected && this._tableCellOwner.selectionType == TableViewSelectionType.Multiple)
      {
        this.SetSelected(!this.selected, SelectableCell.TransitionType.Animated, (object) this, false);
      }
      else
      {
        if (this.selected && !this._tableCellOwner.canSelectSelectedCell)
          return;
        this.SetSelected(true, SelectableCell.TransitionType.Animated, (object) this, true);
      }
    }

    public virtual void __WasPreparedForReuse() => this.WasPreparedForReuse();

    protected virtual void WasPreparedForReuse()
    {
    }
  }
}
