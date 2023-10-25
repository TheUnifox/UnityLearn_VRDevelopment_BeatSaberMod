// Decompiled with JetBrains decompiler
// Type: EnumTableViewDataSource`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using UnityEngine;

public abstract class EnumTableViewDataSource<T> : MonoBehaviour, TableView.IDataSource where T : Enum
{
  [SerializeField]
  private TextOnlyTableCell _cellPrefab;
  [SerializeField]
  private float _cellHeight = 8f;
  private const string kCellReuseIdentifier = "Cell";
  private readonly T[] _values = (T[]) Enum.GetValues(typeof (T));

  public float CellSize() => this._cellHeight;

  public int NumberOfCells() => this._values.Length;

  public TableCell CellForIdx(TableView tableView, int idx)
  {
    TextOnlyTableCell textOnlyTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as TextOnlyTableCell;
    if ((UnityEngine.Object) textOnlyTableCell == (UnityEngine.Object) null)
    {
      textOnlyTableCell = UnityEngine.Object.Instantiate<TextOnlyTableCell>(this._cellPrefab);
      textOnlyTableCell.reuseIdentifier = "Cell";
    }
    textOnlyTableCell.text = this.GetLabelForValue(this._values[idx]);
    return (TableCell) textOnlyTableCell;
  }

  public int GetIdForValue(T value)
  {
    for (int idForValue = 0; idForValue < this._values.Length; ++idForValue)
    {
      if (this._values[idForValue].Equals((object) value))
        return idForValue;
    }
    return 0;
  }

  public string GetLabelForId(int id) => this.GetLabelForValue(this._values[id]);

  public T GetValueForId(int id) => this._values[id];

  public abstract string GetLabelForValue(T value);
}
