// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicsDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatmapCharacteristicsDropdown : MonoBehaviour
{
  [SerializeField]
  protected SimpleTextDropdown _simpleTextDropdown;
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;

  public event System.Action<BeatmapCharacteristicSO> didSelectCellWithIdxEvent;

  public virtual void Start()
  {
    this._simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
    this._simpleTextDropdown.SelectCellWithIdx(0);
    this._simpleTextDropdown.SetTexts((IReadOnlyList<string>) ((IEnumerable<BeatmapCharacteristicSO>) this._beatmapCharacteristicCollection.beatmapCharacteristics).Select<BeatmapCharacteristicSO, string>((Func<BeatmapCharacteristicSO, string>) (x => Localization.Get(x.characteristicNameLocalizationKey))).ToArray<string>());
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._simpleTextDropdown == (UnityEngine.Object) null)
      return;
    this._simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  public virtual BeatmapCharacteristicSO GetSelectedBeatmapCharacteristic() => this._beatmapCharacteristicCollection.beatmapCharacteristics[this._simpleTextDropdown.selectedIndex];

  public virtual void SelectCellWithBeatmapCharacteristic(
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    int idx = ((IReadOnlyList<BeatmapCharacteristicSO>) this._beatmapCharacteristicCollection.beatmapCharacteristics).IndexOf<BeatmapCharacteristicSO>(beatmapCharacteristic);
    if (idx < 0)
      idx = 0;
    this._simpleTextDropdown.SelectCellWithIdx(idx);
  }

  public virtual void HandleSimpleTextDropdownDidSelectCellWithIdx(
    DropdownWithTableView dropdownWithTableView,
    int idx)
  {
    System.Action<BeatmapCharacteristicSO> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
    if (cellWithIdxEvent == null)
      return;
    cellWithIdxEvent(this._beatmapCharacteristicCollection.beatmapCharacteristics[idx]);
  }
}
