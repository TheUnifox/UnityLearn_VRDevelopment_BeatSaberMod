// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveObstacleOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveObstacleOnGridCommand : MoveBeatmapObjectOnGridCommand
  {
    [Inject]
    private readonly MoveObstacleOnGridSignal _signal;
    private ObstacleEditorData _originalObstacle;
    private ObstacleEditorData _newObstacle;

    public override bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is MoveObstacleOnGridCommand obstacleOnGridCommand && this._originalObstacle.id == obstacleOnGridCommand._newObstacle.id;
    }

    public override void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      if (!(previousCommand is MoveObstacleOnGridCommand obstacleOnGridCommand))
        return;
      this._originalObstacle = obstacleOnGridCommand._originalObstacle;
    }

    protected override bool IsCommandValid() => this._newObstacle != (ObstacleEditorData) null;

    protected override void GatherBeatmapObject()
    {
      this._originalObstacle = this.beatmapLevelDataModel.GetObstacleById(this._signal.id);
      if (this._originalObstacle == (ObstacleEditorData) null)
        return;
      this._newObstacle = ObstacleEditorData.CopyWithModifications(this._originalObstacle, column: new int?(Mathf.Clamp(this._signal.position.x, 0, 4 - this._originalObstacle.width)), row: new int?(Mathf.Clamp(this._signal.position.y, 0, 3 - this._originalObstacle.height)));
    }

    protected override (float, int, int, float, int, int) GetNewRectangle() => (this._newObstacle.beat, this._newObstacle.column, this._newObstacle.row, this._newObstacle.beat + this._newObstacle.duration, this._newObstacle.column + this._newObstacle.width, this._newObstacle.row + this._newObstacle.height);

    protected override BeatmapEditorObjectId GetOriginalId() => this._originalObstacle.id;

    protected override BeatmapObjectCellData GetNewBeatmapObjectCellData() => new BeatmapObjectCellData(this._signal.position, this._originalObstacle.beat);

    protected override void SwapNewWithOld()
    {
      this.beatmapLevelDataModel.RemoveObstacle(this._newObstacle);
      this.beatmapLevelDataModel.AddObstacle(this._originalObstacle);
    }

    protected override void SwapOldWithNew()
    {
      this.beatmapLevelDataModel.RemoveObstacle(this._originalObstacle);
      this.beatmapLevelDataModel.AddObstacle(this._newObstacle);
    }
  }
}
