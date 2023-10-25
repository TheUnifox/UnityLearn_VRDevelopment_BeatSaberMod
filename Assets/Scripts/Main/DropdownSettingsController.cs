// Decompiled with JetBrains decompiler
// Type: DropdownSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropdownSettingsController : MonoBehaviour
{
  [SerializeField]
  private SimpleTextDropdown _dropdown;
  private int _idx;
  private int _numberOfElements;

  public event System.Action dropDownValueDidChangeEvent;

  protected abstract bool GetInitValues(out int idx, out int numberOfElements);

  protected abstract void ApplyValue(int idx);

  protected abstract string TextForValue(int idx);

  protected void OnEnable()
  {
    this._dropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleDropdownValueDidChange);
    if (!this.GetInitValues(out this._idx, out this._numberOfElements))
      return;
    this.RefreshUI();
  }

  protected void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) this._dropdown)
      return;
    this._dropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleDropdownValueDidChange);
  }

  private void HandleDropdownValueDidChange(DropdownWithTableView dropdownWithTableView, int idx)
  {
    this._idx = idx;
    this.ApplyValue(idx);
    System.Action valueDidChangeEvent = this.dropDownValueDidChangeEvent;
    if (valueDidChangeEvent == null)
      return;
    valueDidChangeEvent();
  }

  private void RefreshUI()
  {
    List<string> texts = new List<string>();
    for (int idx = 0; idx < this._numberOfElements; ++idx)
      texts.Add(this.TextForValue(idx));
    this._dropdown.SetTexts((IReadOnlyList<string>) texts);
    this._dropdown.SelectCellWithIdx(this._idx);
  }

  public void Refresh(bool applyValue)
  {
    if (!this.GetInitValues(out this._idx, out this._numberOfElements))
      return;
    this.RefreshUI();
    if (!applyValue)
      return;
    this.ApplyValue(this._idx);
  }
}
