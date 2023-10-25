// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.MoveBeatmapObjectToBeatLineCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public abstract class MoveBeatmapObjectToBeatLineCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    protected readonly BeatmapLevelDataModel beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState beatmapObjectsSelectionState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly SignalBus signalBus;

    public bool shouldAddToHistory { get; private set; }

    protected abstract (BeatmapEditorObjectId id, BeatmapObjectType type) GetDraggedData();

    protected abstract BeatmapObjectCellData GetObjectCellData();

    protected virtual bool IsCommandValid() => true;

    protected abstract void GatherBeatmapObject(float beat, int column, int row);

    protected abstract (float, int, int, float, int, int) GetNewRectangle(
      float beat,
      int column,
      int row);

    protected abstract void SwapNewWithOld();

    protected abstract void SwapOldWithNew();

    public void Execute()
    {
      (BeatmapEditorObjectId id, BeatmapObjectType type) = this.GetDraggedData();
      BeatmapObjectCellData objectCellData = this.GetObjectCellData();
      this.beatmapObjectsSelectionState.draggedBeatmapObjectId = id;
      this.beatmapObjectsSelectionState.draggedBeatmapObjectType = type;
      this.beatmapObjectsSelectionState.draggedBeatmapObjectCellData = objectCellData;
      this.beatmapObjectsSelectionState.draggedHead = new bool?();
      if (AudioTimeHelper.IsBeatSame(objectCellData.beat, this._beatmapState.beat))
        return;
      BeatmapObjectCellData hoveredGridCellData = this._beatmapObjectsState.hoveredGridCellData;
      Vector2Int cellPosition;
      int x;
      if (hoveredGridCellData == null)
      {
        x = objectCellData.cellPosition.x;
      }
      else
      {
        cellPosition = hoveredGridCellData.cellPosition;
        x = cellPosition.x;
      }
      int num1 = x;
      int y;
      if (hoveredGridCellData == null)
      {
        cellPosition = objectCellData.cellPosition;
        y = cellPosition.y;
      }
      else
      {
        cellPosition = hoveredGridCellData.cellPosition;
        y = cellPosition.y;
      }
      int num2 = y;
      BeatmapObjectCellData beatmapObjectCellData = new BeatmapObjectCellData(new Vector2Int(num1, num2), this._beatmapState.beat);
      this.GatherBeatmapObject(beatmapObjectCellData.beat, num1, num2);
      if (!this.IsCommandValid())
        return;
      (float, int, int, float, int, int) newRectangle = this.GetNewRectangle(beatmapObjectCellData.beat, num1, num2);
      if (this.beatmapLevelDataModel.AnyBeatmapObjectExists(newRectangle.Item1, newRectangle.Item2, newRectangle.Item3, newRectangle.Item4, newRectangle.Item5, newRectangle.Item6))
        return;
      this.beatmapObjectsSelectionState.draggedBeatmapObjectCellData = beatmapObjectCellData;
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this.SwapNewWithOld();
      this.signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this.SwapOldWithNew();
      this.signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
