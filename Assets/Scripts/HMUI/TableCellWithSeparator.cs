// Decompiled with JetBrains decompiler
// Type: TableCellWithSeparator
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using HMUI;
using UnityEngine;

public class TableCellWithSeparator : TableCell
{
  [SerializeField]
  protected GameObject _separator;

  public override void TableViewSetup(ITableCellOwner tableCellOwner, int idx)
  {
    base.TableViewSetup(tableCellOwner, idx);
    this._separator.SetActive(idx < tableCellOwner.numberOfCells - 1);
  }
}
