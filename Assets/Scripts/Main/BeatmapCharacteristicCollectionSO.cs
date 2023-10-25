// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicCollectionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BeatmapCharacteristicCollectionSO : PersistentScriptableObject
{
  public bool _ignore360MovementCharacteristics;
  [SerializeField]
  protected BeatmapCharacteristicSO[] _beatmapCharacteristics;
  protected BeatmapCharacteristicSO[] _no360beatmapCharacteristics;

  public BeatmapCharacteristicSO[] beatmapCharacteristics => !this._ignore360MovementCharacteristics ? this._beatmapCharacteristics : this._no360beatmapCharacteristics;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.InitData();
  }

  public virtual BeatmapCharacteristicSO GetBeatmapCharacteristicBySerializedName(
    string serializedName)
  {
    foreach (BeatmapCharacteristicSO beatmapCharacteristic in this._beatmapCharacteristics)
    {
      if (beatmapCharacteristic.serializedName == serializedName)
        return beatmapCharacteristic;
    }
    return (BeatmapCharacteristicSO) null;
  }

  public virtual bool ContainsBeatmapCharacteristic(BeatmapCharacteristicSO beatmapCharacteristic)
  {
    foreach (Object beatmapCharacteristic1 in this._beatmapCharacteristics)
    {
      if (beatmapCharacteristic1 == (Object) beatmapCharacteristic)
        return true;
    }
    return false;
  }

  public virtual void InitData()
  {
    if (this._beatmapCharacteristics == null)
      return;
    this._no360beatmapCharacteristics = this.GetCharacteristicsWithout360Movement();
  }

  public virtual BeatmapCharacteristicSO[] GetCharacteristicsWithout360Movement()
  {
    List<BeatmapCharacteristicSO> characteristicSoList = new List<BeatmapCharacteristicSO>((IEnumerable<BeatmapCharacteristicSO>) this._beatmapCharacteristics);
    int index = 0;
    while (index < characteristicSoList.Count)
    {
      if (characteristicSoList[index].requires360Movement)
        characteristicSoList.RemoveAt(index);
      else
        ++index;
    }
    return characteristicSoList.ToArray();
  }
}
