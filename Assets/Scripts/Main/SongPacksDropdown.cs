// Decompiled with JetBrains decompiler
// Type: SongPacksDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongPacksDropdown : MonoBehaviour
{
  [SerializeField]
  protected SimpleTextDropdown _simpleTextDropdown;
  [Space]
  [SerializeField]
  protected SongPackMaskModelSO _songPackMaskModel;
  protected bool _initialized;
  protected List<string> _songPackSerializedNames;

  public event System.Action<int> didSelectCellWithIdxEvent;

  public virtual void LazyInit()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    if (this._songPackSerializedNames == null)
      this._songPackSerializedNames = this._songPackMaskModel.defaultSongPackMaskItems;
    this._simpleTextDropdown.SetTexts((IReadOnlyList<string>) this._songPackSerializedNames.Select<string, string>((Func<string, string>) (serializedName => Localization.Get(serializedName))).ToList<string>());
  }

  public virtual void Start()
  {
    this.LazyInit();
    this._simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._simpleTextDropdown != (UnityEngine.Object) null))
      return;
    this._simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  public virtual void SetOverrideSongPacks(List<string> songPackSerializedNames) => this._songPackSerializedNames = songPackSerializedNames;

  public virtual SongPackMask GetSelectedSongPackMask()
  {
    this.LazyInit();
    return this._songPackMaskModel.ToSongPackMask(this._songPackSerializedNames[this._simpleTextDropdown.selectedIndex]);
  }

  public virtual void SelectCellWithSongPackMask(SongPackMask songPackMask)
  {
    this.LazyInit();
    int idxForSongPackMask = this.GetIdxForSongPackMask(songPackMask);
    if (idxForSongPackMask < 0 || idxForSongPackMask > this._songPackSerializedNames.Count - 1)
      this._simpleTextDropdown.SelectCellWithIdx(0);
    this._simpleTextDropdown.SelectCellWithIdx(idxForSongPackMask);
  }

  public virtual int GetIdxForSongPackMask(SongPackMask songPackMask) => Mathf.Clamp(this._songPackSerializedNames.IndexOf(this._songPackMaskModel.ToSerializedName(songPackMask)), 0, this._songPackSerializedNames.Count - 1);

  public virtual void HandleSimpleTextDropdownDidSelectCellWithIdx(
    DropdownWithTableView dropdownWithTableView,
    int idx)
  {
    System.Action<int> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
    if (cellWithIdxEvent == null)
      return;
    cellWithIdxEvent(idx);
  }
}
