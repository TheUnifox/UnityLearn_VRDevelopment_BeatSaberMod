// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.CurrentRegionUpdateBpmCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands
{
  public class CurrentRegionUpdateBpmCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly CurrentRegionUpdateBpmSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private BpmRegion _newRegion;

    public bool shouldAddToHistory => true;

    public int regionIdx { get; private set; }

    public BpmRegion oldRegion { get; private set; }

    public void Execute()
    {
      this.regionIdx = this._bpmEditorState.currentBpmRegionIdx;
      this.oldRegion = this._bpmEditorDataModel.regions[this.regionIdx];
      this._newRegion = new BpmRegion(this.oldRegion.startSampleIndex, this.oldRegion.endSampleIndex, this.oldRegion.startBeat, this.oldRegion.startBeat + AudioTimeHelper.SamplesToBeats(this.oldRegion.sampleCount, this.oldRegion.sampleFrequency, this._signal.newBpm), this.oldRegion.sampleFrequency);
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionIdx, this.oldRegion);
      this._signalBus.Fire<CurrentRegionUpdatedBpmSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionIdx, this._newRegion);
      this._signalBus.Fire<CurrentRegionUpdatedBpmSignal>();
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
