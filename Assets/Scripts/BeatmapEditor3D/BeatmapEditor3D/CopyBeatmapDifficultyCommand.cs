// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CopyBeatmapDifficultyCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.SerializedData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class CopyBeatmapDifficultyCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly CopyBeatmapDifficultySignal _signal;
    [Inject]
    private readonly BeatmapProjectManager _beatmapProjectManager;
    [Inject]
    private readonly BeatmapLevelDataModelVersionedLoader _beatmapLevelDataModelVersionedLoader;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicBeatmapBasicEventsDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    private List<NoteEditorData> _prevNotes;
    private List<WaypointEditorData> _prevWaypoints;
    private List<ObstacleEditorData> _prevObstacles;
    private List<ArcEditorData> _prevArcs;
    private List<ChainEditorData> _prevChains;
    private List<BasicEventEditorData> _prevEvents;
    private List<BeatmapEditorEventBoxGroupInput> _prevEventBoxGroups;
    private List<NoteEditorData> _notes;
    private List<WaypointEditorData> _waypoints;
    private List<ObstacleEditorData> _obstacles;
    private List<ArcEditorData> _arcs;
    private List<ChainEditorData> _chains;
    private List<BasicEventEditorData> _events;
    private List<BeatmapEditorEventBoxGroupInput> _eventBoxGroups;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this.shouldAddToHistory = false;
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      IDifficultyBeatmapData difficultyBeatmapData;
      if (!this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(this._signal.beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.TryGetValue(this._signal.beatmapDifficulty, out difficultyBeatmapData))
        return;
      this._beatmapLevelDataModelVersionedLoader.LoadRawData(this._beatmapProjectManager.workingBeatmapProject, difficultyBeatmapData.beatmapFilename, out this._notes, out this._waypoints, out this._obstacles, out this._arcs, out this._chains, out this._events, out this._eventBoxGroups);
      if (this._notes != null && this._signal.copyNotes)
        this._prevNotes = new List<NoteEditorData>((IEnumerable<NoteEditorData>) this._beatmapLevelDataModel.notes);
      if (this._obstacles != null && this._signal.copyObstacles)
        this._prevObstacles = new List<ObstacleEditorData>((IEnumerable<ObstacleEditorData>) this._beatmapLevelDataModel.obstacles);
      if (this._chains != null && this._signal.copyChains)
        this._prevChains = new List<ChainEditorData>((IEnumerable<ChainEditorData>) this._beatmapLevelDataModel.chains);
      if (this._arcs != null && this._signal.copyArcs)
        this._prevArcs = new List<ArcEditorData>((IEnumerable<ArcEditorData>) this._beatmapLevelDataModel.arcs);
      if (this._events != null && this._signal.copyEvents)
        this._prevEvents = new List<BasicEventEditorData>((IEnumerable<BasicEventEditorData>) this._beatmapBasicBeatmapBasicEventsDataModel.GetAllEventsAsList());
      if (this._eventBoxGroups != null && this._signal.copyEvents)
        this._prevEventBoxGroups = new List<BeatmapEditorEventBoxGroupInput>(this._beatmapEventBoxGroupsDataModel.GetAllEventBoxGroups());
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this.SwapItems(this._notes, this._obstacles, this._arcs, this._chains, this._events, this._eventBoxGroups, this._prevNotes, this._prevObstacles, this._prevArcs, this._prevChains, this._prevEvents, this._prevEventBoxGroups);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this.SwapItems(this._prevNotes, this._prevObstacles, this._prevArcs, this._prevChains, this._prevEvents, this._prevEventBoxGroups, this._notes, this._obstacles, this._arcs, this._chains, this._events, this._eventBoxGroups);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private void SwapItems(
      List<NoteEditorData> toRemoveNotes,
      List<ObstacleEditorData> toRemoveObstacles,
      List<ArcEditorData> toRemoveArcs,
      List<ChainEditorData> toRemoveChains,
      List<BasicEventEditorData> toRemoveEvents,
      List<BeatmapEditorEventBoxGroupInput> toRemoveEventBoxGroups,
      List<NoteEditorData> toAddNotes,
      List<ObstacleEditorData> toAddObstacles,
      List<ArcEditorData> toAddArcs,
      List<ChainEditorData> toAddChains,
      List<BasicEventEditorData> toAddEvents,
      List<BeatmapEditorEventBoxGroupInput> toAddEventBoxGroups)
    {
      if (toAddNotes != null && this._signal.copyNotes)
      {
        this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) toRemoveNotes);
        this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) toAddNotes);
      }
      if (toAddObstacles != null && this._signal.copyObstacles)
      {
        this._beatmapLevelDataModel.RemoveObstacles((IEnumerable<ObstacleEditorData>) toRemoveObstacles);
        this._beatmapLevelDataModel.AddObstacles((IEnumerable<ObstacleEditorData>) toAddObstacles);
      }
      if (toAddChains != null && this._signal.copyChains)
      {
        this._beatmapLevelDataModel.RemoveNotes(toAddChains.Select<ChainEditorData, NoteEditorData>((Func<ChainEditorData, NoteEditorData>) (c => this._beatmapLevelDataModel.GetNote(new BeatmapObjectCellData(new Vector2Int(c.column, c.row), c.beat)))).Where<NoteEditorData>((Func<NoteEditorData, bool>) (note => note != (NoteEditorData) null)));
        this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) toRemoveChains);
        this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) toAddChains);
      }
      if (toAddArcs != null && this._signal.copyArcs)
      {
        this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) toRemoveArcs);
        this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) toAddArcs);
      }
      if (toAddEvents != null && this._signal.copyEvents)
      {
        this._beatmapBasicBeatmapBasicEventsDataModel.Remove((IEnumerable<BasicEventEditorData>) toRemoveEvents);
        this._beatmapBasicBeatmapBasicEventsDataModel.Insert((IEnumerable<BasicEventEditorData>) toAddEvents);
      }
      if (toAddEventBoxGroups == null || !this._signal.copyEvents)
        return;
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxGroupsWithData(toRemoveEventBoxGroups);
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxGroupsWithData(toAddEventBoxGroups);
    }
  }
}
