// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.SetPlayHeadCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands
{
  public class SetPlayHeadCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SetPlayHeadSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly BpmEditorSongPreviewController _bpmEditorSongPreviewController;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._bpmEditorState.sample = Mathf.Clamp(this._signal.sample, 0, this._bpmEditorSongPreviewController.audioClip.samples);
      int currentBpmRegionIdx = this._bpmEditorState.currentBpmRegionIdx;
      this._bpmEditorState.currentBpmRegionIdx = PlayHeadHelpers.FindIndex(this._bpmEditorDataModel.regions, this._bpmEditorState.currentBpmRegionIdx, this._bpmEditorState.sample);
      if (currentBpmRegionIdx != this._bpmEditorState.currentBpmRegionIdx)
        this._signalBus.Fire<CurrentBpmRegionChangedSignal>(new CurrentBpmRegionChangedSignal(currentBpmRegionIdx, this._bpmEditorState.currentBpmRegionIdx));
      this._signalBus.Fire<PlayHeadUpdatePreviewSignal>();
      this._signalBus.Fire<PlayHeadUpdatedSignal>();
      if (!this._signal.restartPlayback || !this._bpmEditorSongPreviewController.isPlaying)
        return;
      this._bpmEditorSongPreviewController.Stop();
      this._bpmEditorSongPreviewController.PlayFrom(this._bpmEditorState.sample);
    }
  }
}
