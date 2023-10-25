// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.MirrorSelectedBeatmapObjectsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class MirrorSelectedBeatmapObjectsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    private List<NoteEditorData> _originalNotes;
    private List<ObstacleEditorData> _originalObstacles;
    private List<WaypointEditorData> _originalWaypoints;
    private List<ChainEditorData> _originalChains;
    private List<ArcEditorData> _originalArcs;
    private List<NoteEditorData> _mirroredNotes;
    private List<ObstacleEditorData> _mirroredObstacles;
    private List<WaypointEditorData> _mirroredWaypoints;
    private List<ChainEditorData> _mirroredChains;
    private List<ArcEditorData> _mirroredArcs;

    public bool shouldAddToHistory => true;

    public void Execute()
    {
      if (!this._beatmapObjectsSelectionState.IsAnythingSelected())
        return;
      this._originalNotes = this._beatmapObjectsSelectionState.notes.Select<BeatmapEditorObjectId, NoteEditorData>((Func<BeatmapEditorObjectId, NoteEditorData>) (noteId => this._beatmapLevelDataModel.GetNoteById(noteId))).ToList<NoteEditorData>();
      this._originalObstacles = this._beatmapObjectsSelectionState.obstacles.Select<BeatmapEditorObjectId, ObstacleEditorData>((Func<BeatmapEditorObjectId, ObstacleEditorData>) (obstacleId => this._beatmapLevelDataModel.GetObstacleById(obstacleId))).ToList<ObstacleEditorData>();
      this._originalWaypoints = this._beatmapObjectsSelectionState.waypoints.Select<BeatmapEditorObjectId, WaypointEditorData>((Func<BeatmapEditorObjectId, WaypointEditorData>) (waypointId => this._beatmapLevelDataModel.GetWaypointById(waypointId))).ToList<WaypointEditorData>();
      this._originalChains = this._beatmapObjectsSelectionState.chains.Select<BeatmapEditorObjectId, ChainEditorData>((Func<BeatmapEditorObjectId, ChainEditorData>) (chainId => this._beatmapLevelDataModel.GetChainById(chainId))).ToList<ChainEditorData>();
      this._originalArcs = this._beatmapObjectsSelectionState.arcs.Select<BeatmapEditorObjectId, ArcEditorData>((Func<BeatmapEditorObjectId, ArcEditorData>) (arcId => this._beatmapLevelDataModel.GetArcById(arcId))).ToList<ArcEditorData>();
      this.MirrorOriginalBeatmapObjects();
      this.ReplaceBeatmapObjects(this._originalNotes, this._mirroredNotes, this._originalObstacles, this._mirroredObstacles, this._originalWaypoints, this._mirroredWaypoints, this._originalChains, this._mirroredChains, this._originalArcs, this._mirroredArcs);
    }

    public void Undo() => this.ReplaceBeatmapObjects(this._mirroredNotes, this._originalNotes, this._mirroredObstacles, this._originalObstacles, this._mirroredWaypoints, this._originalWaypoints, this._mirroredChains, this._originalChains, this._mirroredArcs, this._originalArcs);

    public void Redo() => this.ReplaceBeatmapObjects(this._originalNotes, this._mirroredNotes, this._originalObstacles, this._mirroredObstacles, this._originalWaypoints, this._mirroredWaypoints, this._originalChains, this._mirroredChains, this._originalArcs, this._mirroredArcs);

    private void ReplaceBeatmapObjects(
      List<NoteEditorData> notesToRemove,
      List<NoteEditorData> notesToAdd,
      List<ObstacleEditorData> obstaclesToRemove,
      List<ObstacleEditorData> obstaclesToAdd,
      List<WaypointEditorData> waypointsToRemove,
      List<WaypointEditorData> waypointsToAdd,
      List<ChainEditorData> chainsToRemove,
      List<ChainEditorData> chainsToAdd,
      List<ArcEditorData> arcsToRemove,
      List<ArcEditorData> arcsToAdd)
    {
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) notesToRemove);
      this._beatmapLevelDataModel.RemoveObstacles((IEnumerable<ObstacleEditorData>) obstaclesToRemove);
      this._beatmapLevelDataModel.RemoveWaypoints((IEnumerable<WaypointEditorData>) waypointsToRemove);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) chainsToRemove);
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) arcsToRemove);
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) notesToAdd);
      this._beatmapLevelDataModel.AddObstacles((IEnumerable<ObstacleEditorData>) obstaclesToAdd);
      this._beatmapLevelDataModel.AddWaypoints((IEnumerable<WaypointEditorData>) waypointsToAdd);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) chainsToAdd);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) arcsToAdd);
      this._beatmapObjectsSelectionState.Clear();
      this._beatmapObjectsSelectionState.AddNotes(notesToAdd.Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id)));
      this._beatmapObjectsSelectionState.AddObstacles(obstaclesToAdd.Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (o => o.id)));
      this._beatmapObjectsSelectionState.AddWaypoints(waypointsToAdd.Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (w => w.id)));
      this._beatmapObjectsSelectionState.AddChains(chainsToAdd.Select<ChainEditorData, BeatmapEditorObjectId>((Func<ChainEditorData, BeatmapEditorObjectId>) (ch => ch.id)));
      this._beatmapObjectsSelectionState.AddArcs(arcsToAdd.Select<ArcEditorData, BeatmapEditorObjectId>((Func<ArcEditorData, BeatmapEditorObjectId>) (a => a.id)));
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private void MirrorOriginalBeatmapObjects()
    {
      this._mirroredNotes = new List<NoteEditorData>(this._originalNotes.Count);
      this._mirroredObstacles = new List<ObstacleEditorData>(this._originalObstacles.Count);
      this._mirroredWaypoints = new List<WaypointEditorData>(this._originalWaypoints.Count);
      this._mirroredChains = new List<ChainEditorData>(this._originalChains.Count);
      this._mirroredArcs = new List<ArcEditorData>(this._originalArcs.Count);
      foreach (NoteEditorData originalNote in this._originalNotes)
        this._mirroredNotes.Add(NoteEditorData.CopyWithModifications(originalNote, column: new int?(3 - originalNote.column), type: new ColorType?(originalNote.type.Opposite()), cutDirection: new NoteCutDirection?(originalNote.cutDirection.Mirrored())));
      foreach (ObstacleEditorData originalObstacle in this._originalObstacles)
      {
        int num = 4 - (originalObstacle.column + originalObstacle.width);
        this._mirroredObstacles.Add(ObstacleEditorData.CopyWithModifications(originalObstacle, column: new int?(num)));
      }
      foreach (WaypointEditorData originalWaypoint in this._originalWaypoints)
        this._mirroredWaypoints.Add(WaypointEditorData.CreateNewWithId(originalWaypoint.id, originalWaypoint.beat, 3 - originalWaypoint.column, originalWaypoint.row, originalWaypoint.offsetDirection.OppositeDirection()));
      foreach (ChainEditorData originalChain in this._originalChains)
      {
        List<ChainEditorData> mirroredChains = this._mirroredChains;
        ChainEditorData data = originalChain;
        BeatmapEditorObjectId? id = new BeatmapEditorObjectId?();
        float? beat = new float?();
        int? nullable = new int?(3 - originalChain.column);
        ColorType? colorType = new ColorType?(originalChain.colorType.Opposite());
        int? column = nullable;
        int? row = new int?();
        NoteCutDirection? cutDirection = new NoteCutDirection?(originalChain.cutDirection.Mirrored());
        float? tailBeat = new float?();
        int? tailColumn = new int?(3 - originalChain.tailColumn);
        int? tailRow = new int?();
        int? sliceCount = new int?();
        float? squishAmount = new float?();
        ChainEditorData chainEditorData = ChainEditorData.CopyWithModifications(data, id, beat, colorType, column, row, cutDirection, tailBeat, tailColumn, tailRow, sliceCount, squishAmount);
        mirroredChains.Add(chainEditorData);
      }
      foreach (ArcEditorData originalArc in this._originalArcs)
      {
        List<ArcEditorData> mirroredArcs = this._mirroredArcs;
        ArcEditorData data = originalArc;
        BeatmapEditorObjectId? id = new BeatmapEditorObjectId?();
        ColorType? colorType = new ColorType?(originalArc.colorType.Opposite());
        float? beat = new float?();
        SliderMidAnchorMode? nullable = new SliderMidAnchorMode?(originalArc.midAnchorMode.OppositeDirection());
        int? column = new int?(3 - originalArc.column);
        int? row = new int?();
        NoteCutDirection? cutDirection = new NoteCutDirection?(originalArc.cutDirection.Mirrored());
        float? controlPointLengthMultiplier = new float?();
        float? tailBeat = new float?();
        int? tailColumn = new int?(3 - originalArc.tailColumn);
        int? tailRow = new int?();
        NoteCutDirection? tailCutDirection = new NoteCutDirection?(originalArc.tailCutDirection.Mirrored());
        float? tailControlPointLengthMultiplier = new float?();
        SliderMidAnchorMode? midAnchorMode = nullable;
        ArcEditorData arcEditorData = ArcEditorData.CopyWithModifications(data, id, colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
        mirroredArcs.Add(arcEditorData);
      }
    }
  }
}
