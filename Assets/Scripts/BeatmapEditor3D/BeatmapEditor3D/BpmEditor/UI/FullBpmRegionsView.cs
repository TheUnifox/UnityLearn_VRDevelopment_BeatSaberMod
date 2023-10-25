// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.FullBpmRegionsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using System;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class FullBpmRegionsView : BpmRegionsView
  {
    protected override void DidActivate()
    {
      base.DidActivate();
      this._signalBus.Subscribe<CurrentBpmRegionChangedSignal>(new Action<CurrentBpmRegionChangedSignal>(this.HandleCurrentBpmRegionChanged));
      this._bpmRegionsInputController.SetBounds(0, this.bpmEditorSongPreviewController.audioClip.samples);
      this.DisplayBpmGrid(0, this._bpmEditorDataModel.regions.Count - 1, 0, this.bpmEditorSongPreviewController.samplesCount, false);
    }

    protected override void DidDeactivate()
    {
      base.DidDeactivate();
      this._signalBus.TryUnsubscribe<CurrentBpmRegionChangedSignal>(new Action<CurrentBpmRegionChangedSignal>(this.HandleCurrentBpmRegionChanged));
    }

    protected override void HandleBpmRegionBeatsUpdated(BpmRegionBeatsUpdatedSignal signal)
    {
      BpmRegion region = this._bpmEditorDataModel.regions[signal.regionIndex];
      this.SetDataToRegion(signal.regionIndex, region);
      if (!signal.nextRegionIndex.HasValue)
        return;
      this.SetDataToRegion(signal.nextRegionIndex.Value, this._bpmEditorDataModel.regions[signal.nextRegionIndex.Value]);
    }

    protected override void HandleBpmRegionsMoved(BpmRegionsMovedSignal signal)
    {
      BpmRegion region1 = this._bpmEditorDataModel.regions[signal.regionAIndex];
      BpmRegion region2 = this._bpmEditorDataModel.regions[signal.regionBIndex];
      this.CalculateAndSetRegionSizeAndPosition(this._bpmRegions[signal.regionAIndex], region1);
      this.CalculateAndSetRegionSizeAndPosition(this._bpmRegions[signal.regionBIndex], region2);
      this.CalculateAndSetRegionDividerPosition(this._bpmRegionDividers[signal.regionBIndex], region2.startSampleIndex);
    }

    protected override void HandleBpmRegionSplit(BpmRegionSplitSignal signal) => this.DisplayBpmGrid(0, this._bpmEditorDataModel.regions.Count - 1, 0, this.bpmEditorSongPreviewController.samplesCount, false);

    protected override void HandleBpmRegionsMerged() => this.DisplayBpmGrid(0, this._bpmEditorDataModel.regions.Count - 1, 0, this.bpmEditorSongPreviewController.samplesCount, false);

    protected override void HandleBpmRegionsChanged() => this.DisplayBpmGrid(0, this._bpmEditorDataModel.regions.Count - 1, 0, this.bpmEditorSongPreviewController.samplesCount, false);

    private void HandleCurrentBpmRegionChanged(CurrentBpmRegionChangedSignal signal)
    {
      if (signal.prevRegionIdx < this._bpmRegions.Count)
        this._bpmRegions[signal.prevRegionIdx].SetState(true);
      if (signal.currentRegionIdx >= this._bpmRegions.Count)
        return;
      this._bpmRegions[signal.currentRegionIdx].SetState(false);
    }

    private void SetDataToRegion(int index, BpmRegion region)
    {
      if (this._showSamplesInsteadOfBeats)
        this._bpmRegions[index].SetSampleData(region.startSampleIndex, region.endSampleIndex);
      else
        this._bpmRegions[index].SetBeatData(region.startBeat, AudioTimeHelper.SamplesToBPM(region.sampleCount, region.sampleFrequency, region.beats), region.beats);
    }
  }
}
