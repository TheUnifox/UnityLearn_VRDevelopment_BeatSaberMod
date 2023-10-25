// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeHoveredObstacleDurationCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeHoveredObstacleDurationCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly ChangeHoveredObstacleDurationSignal _changeHoveredObstacleDurationSignal;
    private ObstacleEditorData _originalObstacle;
    private ObstacleEditorData _changedObstacle;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this.shouldAddToHistory = false;
      if (this._readonlyBeatmapObjectsState.hoveredNoteCellData == null)
        return;
      this._originalObstacle = this._beatmapLevelDataModel.GetObstacle(this._readonlyBeatmapObjectsState.hoveredNoteCellData);
      if (this._originalObstacle == (ObstacleEditorData) null)
        return;
      this._changedObstacle = ObstacleEditorData.CopyWithModifications(this._originalObstacle, duration: new float?(AudioTimeHelper.ChangeBeatBySubdivision(this._originalObstacle.duration, this._beatmapState.subdivision, this._changeHoveredObstacleDurationSignal.durationDelta, 1)));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo() => this.ChangeObstacleDuration(this._changedObstacle, this._originalObstacle);

    public void Redo() => this.ChangeObstacleDuration(this._originalObstacle, this._changedObstacle);

    private void ChangeObstacleDuration(
      ObstacleEditorData obstacleToRemove,
      ObstacleEditorData obstacleToAdd)
    {
      this._beatmapLevelDataModel.RemoveObstacle(obstacleToRemove);
      this._beatmapLevelDataModel.AddObstacle(obstacleToAdd);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
