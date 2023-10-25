// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmMoveRegionsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmMoveRegionsCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly BpmMoveRegionsSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private BpmRegion _leftRegion;
    private BpmRegion _rightRegion;

    public int regionAIndex { get; private set; }

    public BpmRegion oldLeftRegion { get; private set; }

    public int regionBIndex { get; private set; }

    public BpmRegion oldRightRegion { get; private set; }

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      IReadOnlyList<BpmRegion> regions = this._bpmEditorDataModel.regions;
      if (this._signal.leftRegionIdx < 0 || this._signal.rightRegionIdx >= regions.Count)
        return;
      this.regionAIndex = this._signal.leftRegionIdx;
      this.regionBIndex = this._signal.rightRegionIdx;
      this.oldLeftRegion = regions[this.regionAIndex];
      this.oldRightRegion = regions[this.regionBIndex];
      int num1 = this._signal.sample - 1;
      float num2 = (float) (num1 - this._signal.leftRegion.startSampleIndex) / (float) this._signal.leftRegion.samplesPerBeat;
      int sample = this._signal.sample;
      float num3 = (float) (this._signal.rightRegion.endSampleIndex - sample) / (float) this._signal.rightRegion.samplesPerBeat;
      float num4 = this._signal.leftRegion.startBeat + num2;
      float num5 = num4 + num3;
      int startSampleIndex = this.oldLeftRegion.startSampleIndex;
      if (this._bpmEditorState.stretchRegion)
      {
        num4 = this.oldRightRegion.startBeat;
        num5 = this.oldRightRegion.endBeat;
      }
      this._leftRegion = new BpmRegion(this.oldLeftRegion, new int?(startSampleIndex), new int?(num1), endBeat: new float?(num4));
      this._rightRegion = new BpmRegion(this.oldRightRegion, new int?(sample), startBeat: new float?(num4), endBeat: new float?(num5));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionAIndex, this.oldLeftRegion);
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionBIndex, this.oldRightRegion);
      this._signalBus.Fire<BpmRegionsMovedSignal>(new BpmRegionsMovedSignal(this.regionAIndex, this.regionBIndex));
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionAIndex, this._leftRegion);
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionBIndex, this._rightRegion);
      this._signalBus.Fire<BpmRegionsMovedSignal>(new BpmRegionsMovedSignal(this.regionAIndex, this.regionBIndex));
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is BpmMoveRegionsCommand;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommandWithHistory)
    {
      BpmMoveRegionsCommand moveRegionsCommand = (BpmMoveRegionsCommand) previousCommandWithHistory;
      this.oldLeftRegion = moveRegionsCommand.oldLeftRegion;
      this.oldRightRegion = moveRegionsCommand.oldRightRegion;
    }
  }
}
