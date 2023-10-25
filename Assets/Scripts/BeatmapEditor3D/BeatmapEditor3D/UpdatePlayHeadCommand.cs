// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.UpdatePlayHeadCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class UpdatePlayHeadCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly UpdatePlayHeadSignal _signal;

    public void Execute()
    {
      float beat1 = this._signal.updateType == UpdatePlayHeadSignal.UpdateType.Sample ? this._beatmapDataModel.bpmData.SampleToBeat(this._signal.sample) : this._signal.beat;
      switch (this._signal.snapType)
      {
        case UpdatePlayHeadSignal.SnapType.CurrentSubdivision:
          beat1 = AudioTimeHelper.RoundToBeatRelative(beat1, this._beatmapState.beatOffset, this._beatmapState.subdivision);
          break;
        case UpdatePlayHeadSignal.SnapType.SmallestSubdivision:
          beat1 = AudioTimeHelper.RoundToBeatRelative(beat1, this._beatmapState.beatOffset, this._beatmapState.subdivision * 128);
          break;
      }
      float min = this._beatmapState.editingMode == BeatmapEditingMode.EventBoxes ? this._eventBoxGroupsState.eventBoxGroupContext.beat : 0.0f;
      float totalBeats = this._beatmapDataModel.bpmData.totalBeats;
      float beat2 = Mathf.Clamp(beat1, min, totalBeats);
      int sample = this._beatmapDataModel.bpmData.BeatToSample(beat2);
      this._beatmapState.beat = beat2;
      this._beatmapState.previewData.SetPlayHead(sample);
      this._beatmapState.previewData.CenterPreviewStart();
      this._signalBus.Fire<BeatmapLevelStatePreviewUpdated>();
      this._signalBus.Fire<BeatmapLevelStateTimeUpdated>();
      if (!this._beatmapState.autoRotation)
        return;
      int rotationEventFor = this._beatmapEventsDataModel.GetSpawnRotationEventFor(this._beatmapState.beat);
      if (rotationEventFor != this._beatmapState.rotation)
        this._signalBus.Fire<SetViewRotationSignal>(new SetViewRotationSignal()
        {
          rotation = rotationEventFor
        });
      if (!this._signal.restartPlayback || !this._songPreviewController.isPlaying)
        return;
      this._songPreviewController.Stop();
      this._songPreviewController.PlayFrom(this._beatmapState.beat);
    }
  }
}
