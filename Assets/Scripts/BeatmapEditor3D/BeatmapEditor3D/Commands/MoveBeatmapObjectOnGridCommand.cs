// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveBeatmapObjectOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class MoveBeatmapObjectOnGridCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    protected readonly BeatmapLevelDataModel beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public bool shouldAddToHistory { get; private set; }

    public abstract bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand);

    public abstract void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand);

    protected abstract void GatherBeatmapObject();

    protected abstract bool IsCommandValid();

    protected abstract (float, int, int, float, int, int) GetNewRectangle();

    protected abstract BeatmapEditorObjectId GetOriginalId();

    protected abstract BeatmapObjectCellData GetNewBeatmapObjectCellData();

    protected abstract void SwapNewWithOld();

    protected abstract void SwapOldWithNew();

    public void Execute()
    {
      this.GatherBeatmapObject();
      if (!this.IsCommandValid())
        return;
      (float, int, int, float, int, int) newRectangle = this.GetNewRectangle();
      if (this.beatmapLevelDataModel.AnyBeatmapObjectExistsWithoutIntersecting(newRectangle.Item1, newRectangle.Item2, newRectangle.Item3, newRectangle.Item4, newRectangle.Item5, newRectangle.Item6, this.GetOriginalId()))
        return;
      this._beatmapObjectsSelectionState.draggedBeatmapObjectCellData = this.GetNewBeatmapObjectCellData();
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this.SwapNewWithOld();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this.SwapOldWithNew();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
