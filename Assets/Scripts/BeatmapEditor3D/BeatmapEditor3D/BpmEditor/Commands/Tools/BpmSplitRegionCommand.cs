// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmSplitRegionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmSplitRegionCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly BpmSplitRegionSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private int _regionIdx;
    private BpmRegion _oldRegion;
    private BpmRegion _leftRegion;
    private BpmRegion _rightRegion;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._signal.splitSampleIdx <= 0)
        return;
      IReadOnlyList<BpmRegion> regions = this._bpmEditorDataModel.regions;
      int hoverBpmRegionIdx = this._bpmEditorState.hoverBpmRegionIdx;
      this._regionIdx = PlayHeadHelpers.FindIndex(regions, hoverBpmRegionIdx, this._signal.splitSampleIdx);
      this._oldRegion = regions[this._regionIdx];
      if (this._signal.splitSampleIdx == this._oldRegion.startSampleIndex || this._signal.splitSampleIdx == this._oldRegion.endSampleIndex)
        return;
      float num1 = this._oldRegion.SampleToBeat(this._signal.splitSampleIdx);
      int startSampleIndex = this._signal.splitSampleIdx;
      float startBeat = this._oldRegion.startBeat;
      float endBeat = this._oldRegion.endBeat;
      switch (this._bpmEditorState.bpmToolSnapType)
      {
        case BpmEditorToolSnapType.Round:
          float beat1 = Mathf.Round(num1);
          startSampleIndex = AudioTimeHelper.BeatToSample(this._oldRegion.startSampleIndex, this._oldRegion.endSampleIndex, this._oldRegion.startBeat, this._oldRegion.endBeat, beat1);
          num1 = beat1;
          break;
        case BpmEditorToolSnapType.Floor:
          float beat2 = Mathf.Floor(num1);
          startSampleIndex = AudioTimeHelper.BeatToSample(this._oldRegion.startSampleIndex, this._oldRegion.endSampleIndex, this._oldRegion.startBeat, this._oldRegion.endBeat, beat2);
          num1 = beat2;
          break;
        case BpmEditorToolSnapType.Ceil:
          float beat3 = Mathf.Ceil(num1);
          startSampleIndex = AudioTimeHelper.BeatToSample(this._oldRegion.startSampleIndex, this._oldRegion.endSampleIndex, this._oldRegion.startBeat, this._oldRegion.endBeat, beat3);
          num1 = beat3;
          break;
      }
      if (this._oldRegion.startSampleIndex == startSampleIndex || startSampleIndex == this._oldRegion.endSampleIndex)
        return;
      if (this._bpmEditorState.splitRegionSingleBeat)
      {
        float num2 = endBeat - num1;
        num1 = startBeat + 1f;
        endBeat = num1 + num2;
      }
      this._leftRegion = new BpmRegion(this._oldRegion.startSampleIndex, startSampleIndex - 1, startBeat, num1, this._oldRegion.sampleFrequency);
      this._rightRegion = new BpmRegion(startSampleIndex, this._oldRegion.endSampleIndex, num1, endBeat, this._oldRegion.sampleFrequency);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceSplitRegionAtIndex(this._regionIdx, this._oldRegion);
      this._signalBus.Fire<PlayHeadUpdatePreviewSignal>();
      this._signalBus.Fire<BpmRegionSplitSignal>(new BpmRegionSplitSignal(this._regionIdx));
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceRegionAtIndexWithSplit(this._regionIdx, this._leftRegion, this._rightRegion);
      this._signalBus.Fire<PlayHeadUpdatePreviewSignal>();
      this._signalBus.Fire<BpmRegionSplitSignal>(new BpmRegionSplitSignal(this._regionIdx));
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }
  }
}
