// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.MoveObstacleToBeatLineCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class MoveObstacleToBeatLineCommand : MoveBeatmapObjectToBeatLineCommand
  {
    [Inject]
    private readonly MoveObstacleToBeatLineSignal _signal;
    private ObstacleEditorData _originalObstacle;
    private ObstacleEditorData _newObstacle;

    protected override (BeatmapEditorObjectId id, BeatmapObjectType type) GetDraggedData() => (this._signal.id, BeatmapObjectType.Obstacle);

    protected override BeatmapObjectCellData GetObjectCellData() => this._signal.cellData;

    protected override void GatherBeatmapObject(float beat, int column, int row)
    {
      this._originalObstacle = this.beatmapLevelDataModel.GetObstacleById(this._signal.id);
      if (this._originalObstacle == (ObstacleEditorData) null)
        return;
      column = Mathf.Clamp(column, 0, 4 - this._originalObstacle.width);
      this._newObstacle = ObstacleEditorData.CopyWithModifications(this._originalObstacle, beat: new float?(beat), column: new int?(column));
    }

    protected override (float, int, int, float, int, int) GetNewRectangle(
      float beat,
      int column,
      int row)
    {
      return (beat, column, this._originalObstacle.row, beat + this._originalObstacle.duration, column + this._originalObstacle.column, this._originalObstacle.row + this._originalObstacle.height);
    }

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
