// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmEditorAudioWaveformView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.Views;
using System;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmEditorAudioWaveformView : AudioWaveformView
  {
    [Inject]
    private readonly BpmEditorState _bpmEditorState;

    protected override void SetupWaveform()
    {
      base.SetupWaveform();
      (int start, int end) fullSampleBounds = this.GetFullSampleBounds();
      this.RenderWaveform(fullSampleBounds.start, fullSampleBounds.end);
    }

    protected override void DidActivate()
    {
      base.DidActivate();
      this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.Subscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    protected override void DidDeactivate()
    {
      base.DidDeactivate();
      this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.TryUnsubscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    private (int start, int end) GetFullSampleBounds() => (this._bpmEditorState.sample - this._bpmEditorState.previewHalfSize, this._bpmEditorState.sample + this._bpmEditorState.previewHalfSize);

    private void HandlePlayHeadUpdated()
    {
      (int start, int end) fullSampleBounds = this.GetFullSampleBounds();
      this.RenderWaveform(fullSampleBounds.start, fullSampleBounds.end);
    }

    private void HandlePlayHeadZoomed()
    {
      (int start, int end) fullSampleBounds = this.GetFullSampleBounds();
      this.RenderWaveform(fullSampleBounds.start, fullSampleBounds.end);
    }
  }
}
