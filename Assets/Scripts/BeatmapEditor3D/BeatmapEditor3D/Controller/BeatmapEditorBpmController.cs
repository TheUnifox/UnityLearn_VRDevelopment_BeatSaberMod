// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorBpmController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorBpmController : IBpmController
  {
    private readonly BeatmapDataModel _beatmapDataModel;
    private readonly IReadonlyBeatmapState _beatmapState;

    public float currentBpm => this._beatmapDataModel.bpmData.GetRegionAtBeat(this._beatmapState.beat).bpm;

    public BeatmapEditorBpmController(
      IReadonlyBeatmapState beatmapState,
      BeatmapDataModel beatmapDataModel)
    {
      this._beatmapState = beatmapState;
      this._beatmapDataModel = beatmapDataModel;
    }
  }
}
