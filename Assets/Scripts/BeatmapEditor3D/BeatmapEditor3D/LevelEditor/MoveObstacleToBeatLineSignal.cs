﻿// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.MoveObstacleToBeatLineSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.LevelEditor
{
  public class MoveObstacleToBeatLineSignal
  {
    public readonly BeatmapEditorObjectId id;
    public readonly BeatmapObjectCellData cellData;

    public MoveObstacleToBeatLineSignal(BeatmapEditorObjectId id, BeatmapObjectCellData cellData)
    {
      this.id = id;
      this.cellData = cellData;
    }
  }
}