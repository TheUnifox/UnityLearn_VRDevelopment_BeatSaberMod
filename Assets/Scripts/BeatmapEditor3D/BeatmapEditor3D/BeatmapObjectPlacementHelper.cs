// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectPlacementHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectPlacementHelper
  {
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    public float timeToZDistanceScale = 12f;
    public const int kColumnCount = 4;
    public const float kHalfColumnNoteOffset = 1.5f;
    public const float kHalfColumnObstacleOffset = 2f;
    public const float kNoteDistance = 0.8f;
    public const float kObjectsPreviewTime = 16f;
    public const float kObjectsDespawnTime = 5f;
    private const float kDefaultTimeToZDistanceScale = 12f;

    public float BeatToPosition(float beat) => this.BeatToPosition(beat, this._beatmapState.beat);

    public float BeatToPosition(float beat, float currentBeat)
    {
      float seconds = this._beatmapDataModel.bpmData.BeatToSeconds(currentBeat);
      return this.TimeToPosition(this._beatmapDataModel.bpmData.BeatToSeconds(beat) - seconds);
    }

    public float PositionToBeat(float position) => this.PositionToBeat(position, this._beatmapState.beat);

    public float PositionToBeat(float position, float currentBeat)
    {
      float time = this.PositionToTime(position);
      return this._beatmapDataModel.bpmData.SecondsToBeat(this._beatmapDataModel.bpmData.BeatToSeconds(currentBeat) + time);
    }

    public float LocalBeatToPosition(float beat, float bpm) => this.TimeToPosition(AudioTimeHelper.BeatsToSeconds(beat, bpm));

    public float TimeToPosition(float time) => time * this.timeToZDistanceScale;

    public float PositionToTime(float position) => position / this.timeToZDistanceScale;

    public float RoundBeatIfPlaying(float beat) => this._songPreviewController.isPlaying ? AudioTimeHelper.RoundToBeat(beat, this._beatmapState.subdivision) : beat;
  }
}
