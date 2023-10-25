// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapBpmDataSaver
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.SerializedData.Bpm;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapBpmDataSaver
  {
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;

    public BpmInfoSerializedDataV2 SaveFromBeatmapDataModel() => this.Save(this._beatmapDataModel.bpmData.sampleCount, this._beatmapDataModel.bpmData.frequency, (IEnumerable<BpmRegion>) this._beatmapDataModel.bpmData.regions);

    public BpmInfoSerializedDataV2 SaveFromBpmDataModel() => this.Save(this._beatmapDataModel.bpmData.sampleCount, this._beatmapDataModel.bpmData.frequency, (IEnumerable<BpmRegion>) this._bpmEditorDataModel.regions);

    private BpmInfoSerializedDataV2 Save(
      int samples,
      int frequency,
      IEnumerable<BpmRegion> regions)
    {
      return new BpmInfoSerializedDataV2(samples, frequency, regions.Select<BpmRegion, BpmInfoSerializedDataV2.BpmRegionSerializedData>((Func<BpmRegion, BpmInfoSerializedDataV2.BpmRegionSerializedData>) (bpmRegion => new BpmInfoSerializedDataV2.BpmRegionSerializedData(bpmRegion.startSampleIndex, bpmRegion.endSampleIndex, bpmRegion.startBeat, bpmRegion.endBeat))).ToList<BpmInfoSerializedDataV2.BpmRegionSerializedData>());
    }
  }
}
