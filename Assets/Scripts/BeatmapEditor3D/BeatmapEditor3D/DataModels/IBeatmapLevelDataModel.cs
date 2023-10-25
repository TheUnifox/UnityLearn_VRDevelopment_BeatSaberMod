// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IBeatmapLevelDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D.DataModels
{
  public interface IBeatmapLevelDataModel : IReadonlyBeatmapLevelDataModel
  {
    new bool beatmapLevelLoaded { get; set; }

    void UpdateWith(
      BeatmapCharacteristicSO beatmapCharacteristic = null,
      BeatmapDifficulty? beatmapDifficulty = null,
      List<NoteEditorData> notes = null,
      List<WaypointEditorData> waypoints = null,
      List<ObstacleEditorData> obstacles = null,
      List<ArcEditorData> arcs = null,
      List<ChainEditorData> chains = null);

    void Clear();

    void ClearDirty();
  }
}
