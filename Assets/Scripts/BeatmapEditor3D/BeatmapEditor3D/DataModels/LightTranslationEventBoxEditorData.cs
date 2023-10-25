// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.LightTranslationEventBoxEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public class LightTranslationEventBoxEditorData : EventBoxEditorData
  {
    public readonly BeatmapEventDataBox.DistributionParamType gapDistributionParamType;
    public readonly float gapDistributionParam;
    public readonly bool gapDistributionShouldAffectFirstBaseEvent;
    public readonly EaseType gapDistributionEaseType;
    public readonly LightAxis axis;
    public readonly bool flipTranslation;

    public static LightTranslationEventBoxEditorData CreateNewDefault(LightAxis lightAxis = LightAxis.X) => LightTranslationEventBoxEditorData.CreateNew(IndexFilterEditorData.CreateDivisionIndexFilter(1, 0, false), BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, false, EaseType.Linear, lightAxis, false);

    public static LightTranslationEventBoxEditorData CopyWithoutId(
      LightTranslationEventBoxEditorData data)
    {
      return LightTranslationEventBoxEditorData.CreateNew(IndexFilterEditorData.Copy(data.indexFilter), data.beatDistributionParamType, data.beatDistributionParam, data.gapDistributionParamType, data.gapDistributionParam, data.gapDistributionShouldAffectFirstBaseEvent, data.gapDistributionEaseType, data.axis, data.flipTranslation);
    }

    public static LightTranslationEventBoxEditorData CreateNew(
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType gapDistributionParamType,
      float gapDistributionParam,
      bool gapDistributionShouldAffectFirstBaseEvent,
      EaseType gapDistributionEaseType,
      LightAxis axis,
      bool flipTranslation)
    {
      return LightTranslationEventBoxEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), indexFilter, beatDistributionParamType, beatDistributionParam, gapDistributionParamType, gapDistributionParam, gapDistributionShouldAffectFirstBaseEvent, gapDistributionEaseType, axis, flipTranslation);
    }

    public static LightTranslationEventBoxEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType gapDistributionParamType,
      float gapDistributionParam,
      bool gapDistributionShouldAffectFirstBaseEvent,
      EaseType gapDistributionEaseType,
      LightAxis axis,
      bool flipTranslation)
    {
      return new LightTranslationEventBoxEditorData(id, indexFilter, beatDistributionParamType, beatDistributionParam, gapDistributionParamType, gapDistributionParam, gapDistributionShouldAffectFirstBaseEvent, gapDistributionEaseType, axis, flipTranslation);
    }

    public static LightTranslationEventBoxEditorData CreateExtension(LightAxis axis) => LightTranslationEventBoxEditorData.CreateNew(IndexFilterEditorData.CreateExtensionIndexFilter(), BeatmapEventDataBox.DistributionParamType.Step, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, 0.0f, false, EaseType.Linear, axis, false);

    private LightTranslationEventBoxEditorData(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType gapDistributionParamType,
      float gapDistributionParam,
      bool gapDistributionShouldAffectFirstBaseEvent,
      EaseType gapDistributionEaseType,
      LightAxis axis,
      bool flipTranslation)
      : base(id, indexFilter, beatDistributionParamType, beatDistributionParam)
    {
      this.gapDistributionParamType = gapDistributionParamType;
      this.gapDistributionParam = gapDistributionParam;
      this.gapDistributionShouldAffectFirstBaseEvent = gapDistributionShouldAffectFirstBaseEvent;
      this.gapDistributionEaseType = gapDistributionEaseType;
      this.axis = axis;
      this.flipTranslation = flipTranslation;
    }
  }
}
