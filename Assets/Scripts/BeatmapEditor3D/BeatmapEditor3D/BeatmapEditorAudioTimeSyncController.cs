// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorAudioTimeSyncController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public class BeatmapEditorAudioTimeSyncController : IAudioTimeSource
  {
    private readonly BeatmapDataModel _beatmapDataModel;
    private readonly IReadonlyBeatmapState _beatmapState;

    public float songTime => this._beatmapState.beat;

    public float lastFrameDeltaSongTime => this._beatmapState.beat - this._beatmapState.prevBeat;

    public float songEndTime => this._beatmapDataModel.bpmData.SecondsToBeat(this._beatmapDataModel.audioClip.length);

    public float songLength => this._beatmapDataModel.bpmData.SecondsToBeat(this._beatmapDataModel.audioClip.length);

    public bool isReady => true;

    public BeatmapEditorAudioTimeSyncController(
      BeatmapDataModel beatmapDataModel,
      IReadonlyBeatmapState beatmapState)
    {
      this._beatmapDataModel = beatmapDataModel;
      this._beatmapState = beatmapState;
    }
  }
}
