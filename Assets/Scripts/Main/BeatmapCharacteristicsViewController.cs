// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class BeatmapCharacteristicsViewController : ViewController
{
  [SerializeField]
  protected BeatmapCharacteristicsTableView _beatmapCharacteristicsTableView;
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  protected int _selectedBeatmapCharacteristicNum;

  public event System.Action<BeatmapCharacteristicSO> didSelectBeatmapCharacteristicEvent;

  public BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection => this._beatmapCharacteristicCollection;

  public BeatmapCharacteristicSO selectedBeatmapCharacteristic => !((UnityEngine.Object) this._beatmapCharacteristicCollection != (UnityEngine.Object) null) || this._beatmapCharacteristicCollection.beatmapCharacteristics == null ? (BeatmapCharacteristicSO) null : this._beatmapCharacteristicCollection.beatmapCharacteristics[this._selectedBeatmapCharacteristicNum];

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!addedToHierarchy)
      return;
    this._beatmapCharacteristicsTableView.didSelectCharacteristic += new System.Action<BeatmapCharacteristicSO>(this.HandleBeatmapCharacteristicsTableViewDidSelecteCharacteristic);
    this._beatmapCharacteristicsTableView.SelectCellWithIdx(this._selectedBeatmapCharacteristicNum);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._beatmapCharacteristicsTableView.didSelectCharacteristic -= new System.Action<BeatmapCharacteristicSO>(this.HandleBeatmapCharacteristicsTableViewDidSelecteCharacteristic);
  }

  public virtual void SetData(
    BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection,
    int selectedCharacteristicNum)
  {
    this._beatmapCharacteristicCollection = beatmapCharacteristicCollection;
    this._beatmapCharacteristicsTableView.SetData(this._beatmapCharacteristicCollection);
    this._selectedBeatmapCharacteristicNum = selectedCharacteristicNum;
    if (!this.isInViewControllerHierarchy)
      return;
    this._beatmapCharacteristicsTableView.SelectCellWithIdx(selectedCharacteristicNum);
  }

  public virtual void HandleBeatmapCharacteristicsTableViewDidSelecteCharacteristic(
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    BeatmapCharacteristicSO[] beatmapCharacteristics = this._beatmapCharacteristicCollection.beatmapCharacteristics;
    for (int index = 0; index < beatmapCharacteristics.Length; ++index)
    {
      if ((UnityEngine.Object) beatmapCharacteristic == (UnityEngine.Object) beatmapCharacteristics[index])
      {
        this._selectedBeatmapCharacteristicNum = index;
        break;
      }
    }
    System.Action<BeatmapCharacteristicSO> characteristicEvent = this.didSelectBeatmapCharacteristicEvent;
    if (characteristicEvent == null)
      return;
    characteristicEvent(beatmapCharacteristic);
  }
}
