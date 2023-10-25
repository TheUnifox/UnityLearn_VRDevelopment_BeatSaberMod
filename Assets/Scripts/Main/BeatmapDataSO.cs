// Decompiled with JetBrains decompiler
// Type: BeatmapDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatmapSaveDataVersion3;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class BeatmapDataSO : PersistentScriptableObject
{
  [HideInInspector]
  [SerializeField]
  public string _jsonData;
  protected IBeatmapDataBasicInfo _beatmapDataBasic;

  public virtual async Task<IBeatmapDataBasicInfo> GetBeatmapDataBasicInfoAsync()
  {
    BeatmapSaveData beatmapSaveData = await this.LoadBeatmapSaveDataAsync();
    IBeatmapDataBasicInfo beatmapDataBasicInfo = (IBeatmapDataBasicInfo) null;
    await this.RunTaskAndLogException((System.Action) (() => beatmapDataBasicInfo = (IBeatmapDataBasicInfo) BeatmapDataLoader.GetBeatmapDataBasicInfoFromSaveData(beatmapSaveData)));
    return beatmapDataBasicInfo;
  }

  public virtual async Task<IReadonlyBeatmapData> GetBeatmapDataAsync(
    BeatmapDifficulty beatmapDifficulty,
    float beatsPerMinute,
    bool loadingForDesignatedEnvironment,
    EnvironmentInfoSO environmentInfo,
    PlayerSpecificSettings playerSpecificSettings)
  {
    BeatmapSaveData beatmapSaveData = await this.LoadBeatmapSaveDataAsync();
    IReadonlyBeatmapData readonlyBeatmapData = (IReadonlyBeatmapData) null;
    await this.RunTaskAndLogException((System.Action) (() => readonlyBeatmapData = (IReadonlyBeatmapData) BeatmapDataLoader.GetBeatmapDataFromSaveData(beatmapSaveData, beatmapDifficulty, beatsPerMinute, loadingForDesignatedEnvironment, environmentInfo, playerSpecificSettings)));
    return readonlyBeatmapData;
  }

  public virtual void SetJsonData(string jsonData) => this._jsonData = jsonData;

  public virtual async Task<BeatmapSaveData> LoadBeatmapSaveDataAsync()
  {
    BeatmapSaveData beatmapSaveData = (BeatmapSaveData) null;
    await this.RunTaskAndLogException((System.Action) (() => beatmapSaveData = BeatmapSaveData.DeserializeFromJSONString(this._jsonData)));
    return beatmapSaveData;
  }

  public virtual async Task RunTaskAndLogException(System.Action action) => await Task.Run((System.Action) (() =>
  {
    try
    {
      action();
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
  }));
}
