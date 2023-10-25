// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveNoteOnGridSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.Commands
{
  public class MoveNoteOnGridSignal
  {
    public readonly BeatmapEditorObjectId id;
    public readonly Vector2Int position;

    public MoveNoteOnGridSignal(BeatmapEditorObjectId id, Vector2Int position)
    {
      this.id = id;
      this.position = position;
    }
  }
}
