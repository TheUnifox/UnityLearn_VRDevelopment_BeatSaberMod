// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceEventBoxGroupSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public class PlaceEventBoxGroupSignal
  {
    public readonly int groupId;
    public readonly EventBoxGroupEditorData.EventBoxGroupType type;

    public PlaceEventBoxGroupSignal(int groupId, EventBoxGroupEditorData.EventBoxGroupType type)
    {
      this.groupId = groupId;
      this.type = type;
    }
  }
}
