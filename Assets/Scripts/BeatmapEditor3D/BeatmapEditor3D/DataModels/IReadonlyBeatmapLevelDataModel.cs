// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IReadonlyBeatmapLevelDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D.DataModels
{
  public interface IReadonlyBeatmapLevelDataModel
  {
    bool beatmapLevelLoaded { get; }

    bool isDirty { get; }

    BeatmapCharacteristicSO beatmapCharacteristic { get; }

    BeatmapDifficulty beatmapDifficulty { get; }

    IReadOnlyList<NoteEditorData> notes { get; }

    IReadOnlyList<WaypointEditorData> waypoints { get; }

    IReadOnlyList<ObstacleEditorData> obstacles { get; }

    IReadOnlyList<ArcEditorData> arcs { get; }

    IReadOnlyList<ChainEditorData> chains { get; }

    NoteEditorData GetNoteById(BeatmapEditorObjectId noteId);

    NoteEditorData GetNote(BeatmapObjectCellData cellData);

    ObstacleEditorData GetObstacleById(BeatmapEditorObjectId obstacleId);

    WaypointEditorData GetWaypointById(BeatmapEditorObjectId waypointId);
  }
}
