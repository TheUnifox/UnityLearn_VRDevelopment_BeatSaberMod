// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicSelectionViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;

public class BeatmapCharacteristicSelectionViewController : ViewController
{
  [SerializeField]
  protected IconSegmentedControl _beatmapCharacteristicSegmentedControl;
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  protected BeatmapCharacteristicSO _selectedBeatmapCharacteristic;

  public event System.Action<BeatmapCharacteristicSelectionViewController, BeatmapCharacteristicSO> didSelectBeatmapCharacteristicEvent;

  public BeatmapCharacteristicSO selectedBeatmapCharacteristic => this._selectedBeatmapCharacteristic;

  public virtual void Init()
  {
    this._selectedBeatmapCharacteristic = this._beatmapCharacteristicCollection.beatmapCharacteristics[0];
    this._beatmapCharacteristicSegmentedControl.SelectCellWithNumber(0);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      BeatmapCharacteristicSO[] beatmapCharacteristics = this._beatmapCharacteristicCollection.beatmapCharacteristics;
      IconSegmentedControl.DataItem[] dataItems = new IconSegmentedControl.DataItem[beatmapCharacteristics.Length];
      for (int index = 0; index < beatmapCharacteristics.Length; ++index)
      {
        BeatmapCharacteristicSO characteristicSo = beatmapCharacteristics[index];
        dataItems[index] = new IconSegmentedControl.DataItem(characteristicSo.icon, Localization.Get(characteristicSo.descriptionLocalizationKey));
      }
      this._beatmapCharacteristicSegmentedControl.SetData(dataItems);
    }
    if (!addedToHierarchy)
      return;
    this._beatmapCharacteristicSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleBeatmapCharacteristicSegmentedControlDidSelectCell);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._beatmapCharacteristicSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleBeatmapCharacteristicSegmentedControlDidSelectCell);
  }

  public virtual void HandleBeatmapCharacteristicSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellNumber)
  {
    System.Action<BeatmapCharacteristicSelectionViewController, BeatmapCharacteristicSO> characteristicEvent = this.didSelectBeatmapCharacteristicEvent;
    if (characteristicEvent == null)
      return;
    characteristicEvent(this, this._beatmapCharacteristicCollection.beatmapCharacteristics[cellNumber]);
  }
}
