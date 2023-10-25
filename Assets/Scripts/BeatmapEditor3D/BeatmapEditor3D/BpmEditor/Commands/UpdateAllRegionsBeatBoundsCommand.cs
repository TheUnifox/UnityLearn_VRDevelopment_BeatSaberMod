// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.UpdateAllRegionsBeatBoundsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands
{
  public class UpdateAllRegionsBeatBoundsCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    private const float kRoundPrecision = 100f;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    protected List<BpmRegion> _oldRegions;
    private List<BpmRegion> _newRegions;

    public bool shouldAddToHistory => true;

    public void Execute()
    {
      this._oldRegions = new List<BpmRegion>((IEnumerable<BpmRegion>) this._bpmEditorDataModel.regions);
      this._newRegions = new List<BpmRegion>(this._bpmEditorDataModel.regions.Count);
      float num1 = 0.0f;
      for (int index = 0; index < this._oldRegions.Count; ++index)
      {
        float num2 = num1;
        float num3 = Mathf.Round(this._oldRegions[index].beats * 100f) / 100f;
        float num4 = num1 + num3;
        if ((double) num4 > (double) num2 && this._oldRegions[index].endSampleIndex > this._oldRegions[index].startSampleIndex)
        {
          num1 += num3;
          this._newRegions.Insert(index, new BpmRegion(this._oldRegions[index], startBeat: new float?(num2), endBeat: new float?(num4)));
        }
      }
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceAllRegions(this._oldRegions);
      this._signalBus.Fire<BpmRegionsChangedSignal>();
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceAllRegions(this._newRegions);
      this._signalBus.Fire<BpmRegionsChangedSignal>();
      this._signalBus.Fire<BpmEditorDataUpdatedSignal>();
    }

    public virtual bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return false;
    }

    public virtual void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
    }
  }
}
