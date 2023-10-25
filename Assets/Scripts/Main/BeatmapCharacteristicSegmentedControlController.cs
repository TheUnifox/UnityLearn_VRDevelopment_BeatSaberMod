// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicSegmentedControlController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapCharacteristicSegmentedControlController : MonoBehaviour
{
  [SerializeField]
  protected IconSegmentedControl _segmentedControl;
  protected BeatmapCharacteristicSO _selectedBeatmapCharacteristic;
  protected readonly List<BeatmapCharacteristicSO> _beatmapCharacteristics = new List<BeatmapCharacteristicSO>(5);

  public event System.Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO> didSelectBeatmapCharacteristicEvent;

  public BeatmapCharacteristicSO selectedBeatmapCharacteristic => this._selectedBeatmapCharacteristic;

  public virtual void Awake() => this._segmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleDifficultySegmentedControlDidSelectCell);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._segmentedControl != (UnityEngine.Object) null))
      return;
    this._segmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleDifficultySegmentedControlDidSelectCell);
  }

  public virtual void SetData(
    IReadOnlyList<IDifficultyBeatmapSet> difficultyBeatmapSets,
    BeatmapCharacteristicSO selectedBeatmapCharacteristic)
  {
    this._beatmapCharacteristics.Clear();
    List<IDifficultyBeatmapSet> difficultyBeatmapSetList = new List<IDifficultyBeatmapSet>((IEnumerable<IDifficultyBeatmapSet>) difficultyBeatmapSets);
    difficultyBeatmapSetList.Sort((Comparison<IDifficultyBeatmapSet>) ((a, b) => a.beatmapCharacteristic.sortingOrder.CompareTo(b.beatmapCharacteristic.sortingOrder)));
    difficultyBeatmapSets = (IReadOnlyList<IDifficultyBeatmapSet>) difficultyBeatmapSetList.ToArray();
    IconSegmentedControl.DataItem[] dataItems = new IconSegmentedControl.DataItem[difficultyBeatmapSets.Count];
    int num = 0;
    for (int index = 0; index < difficultyBeatmapSets.Count; ++index)
    {
      BeatmapCharacteristicSO beatmapCharacteristic = difficultyBeatmapSets[index].beatmapCharacteristic;
      dataItems[index] = new IconSegmentedControl.DataItem(beatmapCharacteristic.icon, Localization.Get(beatmapCharacteristic.descriptionLocalizationKey));
      this._beatmapCharacteristics.Add(beatmapCharacteristic);
      if ((UnityEngine.Object) beatmapCharacteristic == (UnityEngine.Object) selectedBeatmapCharacteristic)
        num = index;
    }
    this._segmentedControl.SetData(dataItems);
    this._segmentedControl.SelectCellWithNumber(num);
    this._selectedBeatmapCharacteristic = this._beatmapCharacteristics[num];
  }

  public virtual void HandleDifficultySegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellIdx)
  {
    this._selectedBeatmapCharacteristic = this._beatmapCharacteristics[cellIdx];
    System.Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO> characteristicEvent = this.didSelectBeatmapCharacteristicEvent;
    if (characteristicEvent == null)
      return;
    characteristicEvent(this, this._selectedBeatmapCharacteristic);
  }
}
