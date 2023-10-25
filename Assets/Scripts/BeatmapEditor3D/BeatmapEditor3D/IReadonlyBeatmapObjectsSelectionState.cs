// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.IReadonlyBeatmapObjectsSelectionState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;
using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public interface IReadonlyBeatmapObjectsSelectionState
  {
    bool showSelection { get; }

    float startBeat { get; }

    float endBeat { get; }

    BeatmapObjectType draggedBeatmapObjectType { get; }

    BeatmapEditorObjectId draggedBeatmapObjectId { get; }

    BeatmapObjectCellData draggedBeatmapObjectCellData { get; }

    bool? draggedHead { get; }

    IReadOnlyList<BeatmapEditorObjectId> notes { get; }

    IReadOnlyList<BeatmapEditorObjectId> obstacles { get; }

    IReadOnlyList<BeatmapEditorObjectId> waypoints { get; }

    IReadOnlyList<BeatmapEditorObjectId> chains { get; }

    IReadOnlyList<BeatmapEditorObjectId> arcs { get; }

    bool IsNoteSelected(BeatmapEditorObjectId item);

    bool IsObstacleSelected(BeatmapEditorObjectId item);

    bool IsWaypointSelected(BeatmapEditorObjectId item);

    bool IsAnythingSelected();
  }
}
