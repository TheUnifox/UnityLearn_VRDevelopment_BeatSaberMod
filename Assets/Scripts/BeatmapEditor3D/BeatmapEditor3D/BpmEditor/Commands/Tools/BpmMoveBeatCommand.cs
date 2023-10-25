// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmMoveBeatCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmMoveBeatCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly BpmMoveBeatSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private BpmRegion _newRegion;
    private BpmRegion _newNextRegion;

    public int regionIndex { get; private set; }

    public int? nextRegionIndex { get; private set; }

    public BpmRegion oldRegion { get; private set; }

    public BpmRegion oldNextRegion { get; private set; }

    public bool shouldAddToHistory => true;

    public void Execute()
    {
      IReadOnlyList<BpmRegion> regions = this._bpmEditorDataModel.regions;
      this.regionIndex = this._signal.regionIdx;
      this.nextRegionIndex = this.regionIndex < regions.Count - 1 ? new int?(this.regionIndex + 1) : new int?();
      this.oldRegion = regions[this.regionIndex];
      int? nextRegionIndex = this.nextRegionIndex;
      if (nextRegionIndex.HasValue)
      {
        IReadOnlyList<BpmRegion> bpmRegionList = regions;
        nextRegionIndex = this.nextRegionIndex;
        int index = nextRegionIndex.Value;
        this.oldNextRegion = bpmRegionList[index];
      }
      float num1 = (float) (this._signal.newSample - this._signal.region.startSampleIndex - 1) / ((float) this._signal.beat - this._signal.region.startBeat);
      float startBeat = this._signal.region.startBeat;
      float num2 = startBeat + (float) this._signal.region.sampleCount / num1;
      int startSampleIndex = this._signal.region.startSampleIndex;
      int endSampleIndex1 = this._signal.region.endSampleIndex;
      int endSampleIndex2 = endSampleIndex1;
      float num3 = num2;
      switch (this._bpmEditorState.bpmToolSnapType)
      {
        case BpmEditorToolSnapType.Round:
          float beat1 = Mathf.Round(num2);
          endSampleIndex2 = AudioTimeHelper.BeatToSample(startSampleIndex, endSampleIndex1, startBeat, num2, beat1);
          num3 = beat1;
          break;
        case BpmEditorToolSnapType.Floor:
          float beat2 = Mathf.Floor(num2);
          endSampleIndex2 = AudioTimeHelper.BeatToSample(startSampleIndex, endSampleIndex1, startBeat, num2, beat2);
          num3 = beat2;
          break;
        case BpmEditorToolSnapType.Ceil:
          float beat3 = Mathf.Ceil(num2);
          endSampleIndex2 = AudioTimeHelper.BeatToSample(startSampleIndex, endSampleIndex1, startBeat, num2, beat3);
          num3 = beat3;
          break;
      }
      if (this.regionIndex == this._bpmEditorDataModel.regions.Count - 1 && (this._bpmEditorState.bpmToolSnapType == BpmEditorToolSnapType.Round || this._bpmEditorState.bpmToolSnapType == BpmEditorToolSnapType.Floor))
      {
        float num4 = (float) (endSampleIndex2 - startSampleIndex) / (num3 - startBeat);
        num3 = startBeat + (float) (endSampleIndex1 - startSampleIndex) / num4;
        endSampleIndex2 = endSampleIndex1;
      }
      this._newRegion = new BpmRegion(startSampleIndex, endSampleIndex2, startBeat, num3, this.oldRegion.sampleFrequency);
      if (this.oldNextRegion != null)
        this._newNextRegion = new BpmRegion(endSampleIndex2 + 1, this.oldNextRegion.endSampleIndex, num3, num3 + this.oldNextRegion.beats, this.oldNextRegion.sampleFrequency);
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionIndex, this.oldRegion);
      if (this.oldNextRegion != null)
        this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionIndex + 1, this.oldNextRegion);
      this._signalBus.Fire<BpmRegionBeatsUpdatedSignal>(new BpmRegionBeatsUpdatedSignal(this.regionIndex, this.nextRegionIndex));
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionIndex, this._newRegion);
      if (this._newNextRegion != null)
        this._bpmEditorDataModel.ReplaceRegionAtIndex(this.regionIndex + 1, this._newNextRegion);
      this._signalBus.Fire<BpmRegionBeatsUpdatedSignal>(new BpmRegionBeatsUpdatedSignal(this.regionIndex, this.nextRegionIndex));
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is BpmMoveBeatCommand;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      BpmMoveBeatCommand bpmMoveBeatCommand = (BpmMoveBeatCommand) previousCommand;
      this.oldRegion = bpmMoveBeatCommand.oldRegion;
      this.oldNextRegion = bpmMoveBeatCommand.oldNextRegion;
    }
  }
}
