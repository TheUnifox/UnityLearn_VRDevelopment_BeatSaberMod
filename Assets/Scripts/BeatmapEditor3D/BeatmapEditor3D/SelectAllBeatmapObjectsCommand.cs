// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SelectAllBeatmapObjectsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class SelectAllBeatmapObjectsCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;

    public void Execute()
    {
      this._beatmapObjectsSelectionState.AddNotes(this._beatmapLevelDataModel.notes.Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id)));
      this._beatmapObjectsSelectionState.AddObstacles(this._beatmapLevelDataModel.obstacles.Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (o => o.id)));
      this._beatmapObjectsSelectionState.AddWaypoints(this._beatmapLevelDataModel.waypoints.Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (w => w.id)));
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
    }
  }
}
