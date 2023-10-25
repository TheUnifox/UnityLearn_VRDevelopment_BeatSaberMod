// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightColorEventBoxEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public class LightColorEventBoxEditorData : EventBoxEditorData
  {
    public readonly BeatmapEventDataBox.DistributionParamType brightnessDistributionParamType;
    public readonly float brightnessDistributionParam;
    public readonly bool brightnessDistributionShouldAffectFirstBaseEvent;
    public readonly EaseType brightnessDistributionEaseType;

    public static LightColorEventBoxEditorData CreateNewDefault() => LightColorEventBoxEditorData.CreateNew(IndexFilterEditorData.CreateDivisionIndexFilter(1, 0, false), BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, false, EaseType.Linear);

    public static LightColorEventBoxEditorData CopyWithoutId(LightColorEventBoxEditorData data) => LightColorEventBoxEditorData.CreateNew(IndexFilterEditorData.Copy(data.indexFilter), data.beatDistributionParamType, data.beatDistributionParam, data.brightnessDistributionParamType, data.brightnessDistributionParam, data.brightnessDistributionShouldAffectFirstBaseEvent, data.brightnessDistributionEaseType);

    public static LightColorEventBoxEditorData CreateNew(
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType brightnessDistributionParamType,
      float brightnessDistributionParam,
      bool brightnessDistributionShouldAffectFirstBaseEvent,
      EaseType brightnessDistributionEaseType)
    {
      return LightColorEventBoxEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), indexFilter, beatDistributionParamType, beatDistributionParam, brightnessDistributionParamType, brightnessDistributionParam, brightnessDistributionShouldAffectFirstBaseEvent, brightnessDistributionEaseType);
    }

    public static LightColorEventBoxEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType brightnessDistributionParamType,
      float brightnessDistributionParam,
      bool brightnessDistributionShouldAffectFirstBaseEvent,
      EaseType brightnessDistributionEaseType)
    {
      return new LightColorEventBoxEditorData(id, indexFilter, beatDistributionParamType, beatDistributionParam, brightnessDistributionParamType, brightnessDistributionParam, brightnessDistributionShouldAffectFirstBaseEvent, brightnessDistributionEaseType);
    }

    public static LightColorEventBoxEditorData CreateExtension() => LightColorEventBoxEditorData.CreateNew(IndexFilterEditorData.CreateExtensionIndexFilter(), BeatmapEventDataBox.DistributionParamType.Step, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, 0.0f, false, EaseType.Linear);

    private LightColorEventBoxEditorData(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType brightnessDistributionParamType,
      float brightnessDistributionParam,
      bool brightnessDistributionShouldAffectFirstBaseEvent,
      EaseType brightnessDistributionEaseType)
      : base(id, indexFilter, beatDistributionParamType, beatDistributionParam)
    {
      this.brightnessDistributionParamType = brightnessDistributionParamType;
      this.brightnessDistributionParam = brightnessDistributionParam;
      this.brightnessDistributionShouldAffectFirstBaseEvent = brightnessDistributionShouldAffectFirstBaseEvent;
      this.brightnessDistributionEaseType = brightnessDistributionEaseType;
    }
  }
}
