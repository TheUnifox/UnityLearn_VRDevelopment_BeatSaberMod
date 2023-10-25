// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeHoveredChainSquishAmountCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeHoveredChainSquishAmountCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    private const float kSquishDelta = 0.1f;
    [Inject]
    private readonly ChangeHoveredChainSquishAmountSignal _signal;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private ChainEditorData _originalChain;
    private ChainEditorData _changedChain;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._readonlyBeatmapObjectsState.hoveredNoteCellData == null || (double) this._signal.delta == 0.0)
        return;
      this._originalChain = this._beatmapLevelDataModel.GetChainById(this._readonlyBeatmapObjectsState.hoveredBeatmapObjectId);
      if (this._originalChain == (ChainEditorData) null)
        return;
      this._changedChain = ChainEditorData.CopyWithModifications(this._originalChain, squishAmount: new float?(Mathf.Clamp(this._originalChain.squishAmount + Mathf.Sign(this._signal.delta) * 0.1f, 0.5f, 4f)));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveChain(this._changedChain);
      this._beatmapLevelDataModel.AddChain(this._originalChain);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveChain(this._originalChain);
      this._beatmapLevelDataModel.AddChain(this._changedChain);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is ChangeHoveredChainSquishAmountCommand squishAmountCommand && squishAmountCommand._originalChain.id == this._originalChain.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._originalChain = ((ChangeHoveredChainSquishAmountCommand) previousCommand)._originalChain;
    }
  }
}
