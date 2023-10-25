// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapBpmDataVersionedLoader
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.SerializedData.Bpm;
using System;
using System.Linq;

namespace BeatmapEditor3D
{
  public class BeatmapBpmDataVersionedLoader
  {
    private readonly IBeatmapDataModel _beatmapDataModel;
    private readonly Version _version100 = new Version(1, 0, 0);
    private readonly Version _version200 = new Version(2, 0, 0);

    public BeatmapBpmDataVersionedLoader(IBeatmapDataModel beatmapDataModel) => this._beatmapDataModel = beatmapDataModel;

    public void Load(string projectPath)
    {
      if (BeatmapProjectFileHelper.GetVersionedJSONVersion(projectPath, "BPMInfo.dat") == this._version100)
        this.Load_v1(projectPath);
      else
        this.LoadCurrent(projectPath);
    }

    private void LoadCurrent(string projectPath)
    {
      BpmInfoSerializedDataV2 bpmSerializedData = BeatmapProjectFileHelper.LoadBeatmapJsonObject<BpmInfoSerializedDataV2>(projectPath, "BPMInfo.dat");
      if (bpmSerializedData == null)
        return;
      int samples = AudioTimeHelper.SecondsToSamples(this._beatmapDataModel.songTimeOffset, bpmSerializedData.songFrequency);
      this._beatmapDataModel.LoadBpmData(new BpmData(bpmSerializedData.songFrequency, samples, bpmSerializedData.regions.Select<BpmInfoSerializedDataV2.BpmRegionSerializedData, BpmRegion>((Func<BpmInfoSerializedDataV2.BpmRegionSerializedData, BpmRegion>) (region => new BpmRegion(region.startSampleIndex, region.endSampleIndex, region.startBeat, region.endBeat, bpmSerializedData.songFrequency))).ToList<BpmRegion>()), true);
    }

    private void Load_v1(string projectPath)
    {
      BpmInfoSerializedDataV1 bpmSerializedData = BeatmapProjectFileHelper.LoadBeatmapJsonObject<BpmInfoSerializedDataV1>(projectPath, "BPMInfo.dat");
      if (bpmSerializedData == null)
        return;
      int samples = AudioTimeHelper.SecondsToSamples(this._beatmapDataModel.songTimeOffset, bpmSerializedData.songFrequency);
      this._beatmapDataModel.LoadBpmData(new BpmData(bpmSerializedData.songFrequency, samples, bpmSerializedData.regions.Select<BpmInfoSerializedDataV1.BpmRegionSerializedData, BpmRegion>((Func<BpmInfoSerializedDataV1.BpmRegionSerializedData, BpmRegion>) (region => new BpmRegion(region.startSampleIndex, region.endSampleIndex, region.startBeat, region.endBeat + 1f, bpmSerializedData.songFrequency))).ToList<BpmRegion>()), true);
    }
  }
}
