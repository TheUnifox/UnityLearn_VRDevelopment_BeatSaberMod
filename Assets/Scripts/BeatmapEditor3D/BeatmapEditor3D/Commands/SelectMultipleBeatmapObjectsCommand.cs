// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SelectMultipleBeatmapObjectsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class SelectMultipleBeatmapObjectsCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectMultipleBeatmapObjectsSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (!this._signal.commit)
      {
        this._beatmapObjectsSelectionState.Clear();
        this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      }
      else
      {
        if (this._signal.clearSelection)
          this._beatmapObjectsSelectionState.Clear();
        List<NoteEditorData> list1 = this._beatmapLevelDataModel.GetNotesInterval(this._beatmapObjectsSelectionState.startBeat, this._beatmapObjectsSelectionState.endBeat).ToList<NoteEditorData>();
        List<ObstacleEditorData> list2 = this._beatmapLevelDataModel.GetObstaclesInterval(this._beatmapObjectsSelectionState.startBeat, this._beatmapObjectsSelectionState.endBeat).ToList<ObstacleEditorData>();
        List<WaypointEditorData> list3 = this._beatmapLevelDataModel.GetWaypointsInterval(this._beatmapObjectsSelectionState.startBeat, this._beatmapObjectsSelectionState.endBeat).ToList<WaypointEditorData>();
        List<ChainEditorData> list4 = this._beatmapLevelDataModel.GetChainsInterval(this._beatmapObjectsSelectionState.startBeat, this._beatmapObjectsSelectionState.endBeat).ToList<ChainEditorData>();
        List<ArcEditorData> list5 = this._beatmapLevelDataModel.GetArcsInterval(this._beatmapObjectsSelectionState.startBeat, this._beatmapObjectsSelectionState.endBeat).ToList<ArcEditorData>();
        foreach (NoteEditorData noteEditorData in list1)
        {
          if (!this._beatmapObjectsSelectionState.IsNoteSelected(noteEditorData.id))
            this._beatmapObjectsSelectionState.AddNote(noteEditorData.id);
        }
        foreach (ObstacleEditorData obstacleEditorData in list2)
        {
          if (!this._beatmapObjectsSelectionState.IsObstacleSelected(obstacleEditorData.id))
            this._beatmapObjectsSelectionState.AddObstacle(obstacleEditorData.id);
        }
        foreach (WaypointEditorData waypointEditorData in list3)
        {
          if (!this._beatmapObjectsSelectionState.IsWaypointSelected(waypointEditorData.id))
            this._beatmapObjectsSelectionState.AddWaypoint(waypointEditorData.id);
        }
        foreach (ChainEditorData chainEditorData in list4)
        {
          if (!this._beatmapObjectsSelectionState.IsChainSelected(chainEditorData.id))
            this._beatmapObjectsSelectionState.AddChain(chainEditorData.id);
        }
        foreach (ArcEditorData arcEditorData in list5)
        {
          if (!this._beatmapObjectsSelectionState.IsArcSelected(arcEditorData.id))
            this._beatmapObjectsSelectionState.AddArc(arcEditorData.id);
        }
        this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      }
    }
  }
}
