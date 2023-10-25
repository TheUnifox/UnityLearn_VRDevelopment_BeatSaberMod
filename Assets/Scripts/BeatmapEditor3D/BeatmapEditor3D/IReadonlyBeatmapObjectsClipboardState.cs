// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.IReadonlyBeatmapObjectsClipboardState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public interface IReadonlyBeatmapObjectsClipboardState
  {
    float startBeat { get; }

    IReadOnlyList<NoteEditorData> notes { get; }

    IReadOnlyList<ObstacleEditorData> obstacles { get; }

    IReadOnlyList<WaypointEditorData> waypoints { get; }

    IReadOnlyList<ChainEditorData> chains { get; }

    IReadOnlyList<ArcEditorData> arcs { get; }
  }
}
