// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmMergeRegionsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmMergeRegionsCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly BpmMergeRegionsSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BpmRegion _mergedRegion;

    public bool shouldAddToHistory { get; private set; }

    public int regionIdx { get; private set; }

    public BpmRegion leftRegion { get; private set; }

    public BpmRegion rightRegion { get; private set; }

    public void Execute()
    {
      if (this._signal.leftRegionIdx == -1 || this._signal.rightRegionIdx == -1)
        return;
      IReadOnlyList<BpmRegion> regions = this._bpmEditorDataModel.regions;
      this.regionIdx = this._signal.leftRegionIdx;
      this.leftRegion = regions[this._signal.leftRegionIdx];
      this.rightRegion = regions[this._signal.rightRegionIdx];
      BpmRegion bpmRegion = regions[this._signal.keepRegionIdx];
      this._mergedRegion = new BpmRegion(this.leftRegion.startSampleIndex, this.rightRegion.endSampleIndex, this.leftRegion.startBeat, this.leftRegion.startBeat + (float) (this.rightRegion.endSampleIndex - this.leftRegion.startSampleIndex) / (float) bpmRegion.samplesPerBeat, bpmRegion.sampleFrequency);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndexWithSplit(this.regionIdx, this.leftRegion, this.rightRegion);
      this._signalBus.Fire<PlayHeadUpdatePreviewSignal>();
      this._signalBus.Fire<BpmRegionsMergedSignal>(new BpmRegionsMergedSignal());
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceSplitRegionAtIndex(this.regionIdx, this._mergedRegion);
      this._signalBus.Fire<PlayHeadUpdatePreviewSignal>();
      this._signalBus.Fire<BpmRegionsMergedSignal>(new BpmRegionsMergedSignal());
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return false;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommandWithHistory)
    {
    }
  }
}
