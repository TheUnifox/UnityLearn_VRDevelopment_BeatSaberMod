// Decompiled with JetBrains decompiler
// Type: ValueDropdownController`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ValueDropdownController<T> : MonoBehaviour where T : IComparable
{
  [SerializeField]
  private SimpleTextDropdown _simpleTextDropdown;
  private IReadOnlyList<Tuple<T, string>> _namedValues;

  public event System.Action<int, T> didSelectCellWithIdxEvent;

  private IReadOnlyList<Tuple<T, string>> namedValues => this._namedValues ?? (this._namedValues = this.GetNamedValues());

  protected void Start()
  {
    this._simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
    this._simpleTextDropdown.SetTexts((IReadOnlyList<string>) this.namedValues.Select<Tuple<T, string>, string>((Func<Tuple<T, string>, string>) (x => x.Item2)).ToArray<string>());
  }

  protected void OnDestroy()
  {
    if (!((UnityEngine.Object) this._simpleTextDropdown != (UnityEngine.Object) null))
      return;
    this._simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  protected abstract IReadOnlyList<Tuple<T, string>> GetNamedValues();

  public T GetSelectedItemValue() => this.namedValues[this._simpleTextDropdown.selectedIndex].Item1;

  public void SelectCellWithValue(T value) => this._simpleTextDropdown.SelectCellWithIdx(this.GetIdxForValue(value));

  private int GetIdxForValue(T value)
  {
    if (this.namedValues.Count == 0)
      return 0;
    for (int index = 0; index < this.namedValues.Count; ++index)
    {
      if (this.namedValues[index].Item1.CompareTo((object) value) == 0)
        return index;
    }
    return 0;
  }

  private void HandleSimpleTextDropdownDidSelectCellWithIdx(
    DropdownWithTableView dropdownWithTableView,
    int idx)
  {
    System.Action<int, T> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
    if (cellWithIdxEvent == null)
      return;
    cellWithIdxEvent(idx, this._namedValues[idx].Item1);
  }
}
