// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public abstract class EventBoxEditorData
  {
    public readonly BeatmapEditorObjectId id;
    public readonly IndexFilterEditorData indexFilter;
    public readonly BeatmapEventDataBox.DistributionParamType beatDistributionParamType;
    public readonly float beatDistributionParam;

    public EventBoxEditorData(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam)
    {
      this.id = id;
      this.indexFilter = indexFilter;
      this.beatDistributionParamType = beatDistributionParamType;
      this.beatDistributionParam = beatDistributionParam;
    }
  }
}
