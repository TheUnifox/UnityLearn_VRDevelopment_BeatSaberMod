// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BpmVisualFeedbackController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class BpmVisualFeedbackController : BeatmapEditorView
  {
    [SerializeField]
    private bool _tickWithBpm = true;
    [SerializeField]
    private CurrentBpmMarkerAnimation _currentBpmMarkerAnimation;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly SignalBus _signalBus;
    private int _prevBeat = -1;

    protected override void DidActivate() => this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));

    protected override void DidDeactivate() => this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));

    private void HandleBeatmapLevelStateTimeUpdated()
    {
      int playhead = this._beatmapState.previewData.playhead;
      int num = Mathf.FloorToInt(this._beatmapDataModel.bpmData.SampleToBeat(playhead));
      if ((double) Mathf.Abs(num - this._prevBeat) < 1.0)
        return;
      this._prevBeat = num;
      if (!this._tickWithBpm || !this._songPreviewController.isPlaying)
        return;
      this._currentBpmMarkerAnimation.Play(AudioTimeHelper.BeatsToSeconds(1f, this._beatmapDataModel.bpmData.GetRegionAtSample(playhead).bpm));
    }
  }
}
