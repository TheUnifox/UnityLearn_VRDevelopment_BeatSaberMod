// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CopyBeatmapObjectsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class CopyBeatmapObjectsCommand : 
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly BeatmapObjectsClipboardState _beatmapObjectsClipboardState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (!this._beatmapObjectsSelectionState.IsAnythingSelected())
        return;
      this._beatmapObjectsClipboardState.Clear();
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.notes.Select<BeatmapEditorObjectId, NoteEditorData>((Func<BeatmapEditorObjectId, NoteEditorData>) (noteId => this._beatmapLevelDataModel.GetNoteById(noteId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.obstacles.Select<BeatmapEditorObjectId, ObstacleEditorData>((Func<BeatmapEditorObjectId, ObstacleEditorData>) (obstacleId => this._beatmapLevelDataModel.GetObstacleById(obstacleId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.waypoints.Select<BeatmapEditorObjectId, WaypointEditorData>((Func<BeatmapEditorObjectId, WaypointEditorData>) (waypointId => this._beatmapLevelDataModel.GetWaypointById(waypointId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.chains.Select<BeatmapEditorObjectId, ChainEditorData>((Func<BeatmapEditorObjectId, ChainEditorData>) (chainId => this._beatmapLevelDataModel.GetChainById(chainId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.arcs.Select<BeatmapEditorObjectId, ArcEditorData>((Func<BeatmapEditorObjectId, ArcEditorData>) (arcId => this._beatmapLevelDataModel.GetArcById(arcId))));
      this._beatmapObjectsClipboardState.startBeat = Mathf.Min(this._beatmapObjectsClipboardState.notes.Count > 0 ? this._beatmapObjectsClipboardState.notes.Min<NoteEditorData>((Func<NoteEditorData, float>) (n => n.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.obstacles.Count > 0 ? this._beatmapObjectsClipboardState.obstacles.Min<ObstacleEditorData>((Func<ObstacleEditorData, float>) (o => o.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.waypoints.Count > 0 ? this._beatmapObjectsClipboardState.waypoints.Min<WaypointEditorData>((Func<WaypointEditorData, float>) (w => w.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.chains.Count > 0 ? this._beatmapObjectsClipboardState.chains.Min<ChainEditorData>((Func<ChainEditorData, float>) (c => c.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.arcs.Count > 0 ? this._beatmapObjectsClipboardState.arcs.Min<ArcEditorData>((Func<ArcEditorData, float>) (a => a.beat)) : float.MaxValue);
      this._signalBus.Fire<BeatmapObjectsClipboardStateUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Copied {0} notes, {1} obstacles, {2} waypoints, {3} chains, {4} arcs.", (object) this._beatmapObjectsClipboardState.notes.Count, (object) this._beatmapObjectsClipboardState.obstacles.Count, (object) this._beatmapObjectsClipboardState.waypoints.Count, (object) this._beatmapObjectsClipboardState.chains.Count, (object) this._beatmapObjectsClipboardState.arcs.Count));
  }
}
