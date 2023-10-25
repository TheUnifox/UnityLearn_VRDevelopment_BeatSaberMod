// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.DeleteObstacleCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class DeleteObstacleCommand : DeleteBeatmapObjectCommand
  {
    [Inject]
    private readonly DeleteObstacleSignal _signal;
    private ObstacleEditorData _obstacle;

    protected override void GatherBeatmapObject() => this._obstacle = this.beatmapLevelDataModel.GetObstacleById(this._signal.id);

    protected override bool ShouldAddToHistory() => this._obstacle != (ObstacleEditorData) null;

    protected override void DeselectBeatmapObjectIfSelected() => this.beatmapObjectsSelectionState.IsObstacleSelected(this._obstacle.id);

    protected override void RemoveFromBeatmapLevelDataModel() => this.beatmapLevelDataModel.RemoveObstacle(this._obstacle);

    protected override void AddToBeatmapLevelDataModel() => this.beatmapLevelDataModel.AddObstacle(this._obstacle);
  }
}
