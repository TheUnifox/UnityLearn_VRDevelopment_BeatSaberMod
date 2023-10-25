// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmHoverMarkerView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using System;
using UnityEngine;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmHoverMarkerView : BpmMarkerView
  {
    [SerializeField]
    private bool _autoUpdate;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<HoverSampleChangedSignal>(new Action(this.HandleHoverSampleChanged));
      if (!this._autoUpdate)
        return;
      this.SetupBounds();
      this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.Subscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<HoverSampleChangedSignal>(new Action(this.HandleHoverSampleChanged));
      if (!this._autoUpdate)
        return;
      this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.TryUnsubscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    private void HandleHoverSampleChanged() => this.SetPosition(this._bpmEditorState.hoverSample);

    private void HandlePlayHeadUpdated() => this.SetupBounds();

    private void HandlePlayHeadZoomed() => this.SetupBounds();

    private void SetupBounds() => this.Setup(this._bpmEditorState.sample - this._bpmEditorState.previewHalfSize, this._bpmEditorState.sample + this._bpmEditorState.previewHalfSize, this._bpmEditorState.hoverSample);
  }
}
