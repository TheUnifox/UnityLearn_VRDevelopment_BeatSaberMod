// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapEditorEventBoxGroupInput
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D.DataModels
{
  public struct BeatmapEditorEventBoxGroupInput
  {
    public readonly EventBoxGroupEditorData eventBoxGroup;
    public readonly List<EventBoxEditorData> eventBoxes;
    public readonly List<(BeatmapEditorObjectId, List<BaseEditorData>)> baseLists;

    public BeatmapEditorEventBoxGroupInput(
      EventBoxGroupEditorData eventBoxGroup,
      List<EventBoxEditorData> eventBoxes,
      List<(BeatmapEditorObjectId, List<BaseEditorData>)> baseLists)
    {
      this.eventBoxGroup = eventBoxGroup;
      this.eventBoxes = eventBoxes;
      this.baseLists = baseLists;
    }
  }
}
