// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PlaceObstacleObjectCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class PlaceObstacleObjectCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly PlaceObstacleObjectSignal _signal;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private ObstacleEditorData _obstacle;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this.shouldAddToHistory = false;
      float num = this._beatmapObjectPlacementHelper.RoundBeatIfPlaying(this._signal.beat);
      if (this._beatmapLevelDataModel.AnyBeatmapObjectExists(num, this._signal.x, this._signal.y, num + this._readonlyBeatmapObjectsState.obstacleDuration, this._signal.x + this._signal.width, this._signal.y + this._signal.height))
        return;
      this._obstacle = ObstacleEditorData.CreateNew(num, this._signal.x, this._signal.y, this._readonlyBeatmapObjectsState.obstacleDuration, this._signal.width, this._signal.height);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveObstacle(this._obstacle);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.AddObstacle(this._obstacle);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
