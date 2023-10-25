// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.ZoomedBpmRegionsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using System;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class ZoomedBpmRegionsView : BpmRegionsView
  {
    protected override void DidActivate()
    {
      base.DidActivate();
      this._signalBus.Subscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
      this.SetBoundsToInputController();
      this.DisplayBpmGrid();
    }

    protected override void DidDeactivate()
    {
      base.DidDeactivate();
      this._signalBus.TryUnsubscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    protected override void HandlePlayHeadUpdated()
    {
      this.SetBoundsToInputController();
      this.DisplayBpmGrid();
    }

    protected override void HandleBpmRegionBeatsUpdated(BpmRegionBeatsUpdatedSignal _) => this.DisplayBpmGrid();

    protected override void HandleBpmRegionsMoved(BpmRegionsMovedSignal _) => this.DisplayBpmGrid();

    protected override void HandleBpmRegionSplit(BpmRegionSplitSignal _) => this.DisplayBpmGrid();

    protected override void HandleBpmRegionsMerged() => this.DisplayBpmGrid();

    protected override void HandleBpmRegionsChanged() => this.DisplayBpmGrid();

    private void HandlePlayHeadZoomed()
    {
      this.SetBoundsToInputController();
      this.DisplayBpmGrid();
    }

    private void DisplayBpmGrid()
    {
      int previewStartSample = this.bpmEditorState.sample - this.bpmEditorState.previewHalfSize;
      int previewEndSample = this.bpmEditorState.sample + this.bpmEditorState.previewHalfSize;
      (int start, int end) previewRegionWindow = this.bpmEditorState.previewRegionWindow;
      this.DisplayBpmGrid(previewRegionWindow.start, previewRegionWindow.end, previewStartSample, previewEndSample, true);
    }

    private void SetBoundsToInputController() => this._bpmRegionsInputController.SetBounds(this.bpmEditorState.sample - this.bpmEditorState.previewHalfSize, this.bpmEditorState.sample + this.bpmEditorState.previewHalfSize);
  }
}
