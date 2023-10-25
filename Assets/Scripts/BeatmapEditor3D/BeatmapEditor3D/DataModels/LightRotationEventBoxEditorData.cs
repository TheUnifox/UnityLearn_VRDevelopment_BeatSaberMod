// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.LightRotationEventBoxEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public class LightRotationEventBoxEditorData : EventBoxEditorData
  {
    public readonly BeatmapEventDataBox.DistributionParamType rotationDistributionParamType;
    public readonly float rotationDistributionParam;
    public readonly bool rotationDistributionShouldAffectFirstBaseEvent;
    public readonly EaseType rotationDistributionEaseType;
    public readonly LightAxis axis;
    public readonly bool flipRotation;

    public static LightRotationEventBoxEditorData CreateNewDefault(LightAxis lightAxis = LightAxis.X) => LightRotationEventBoxEditorData.CreateNew(IndexFilterEditorData.CreateDivisionIndexFilter(1, 0, false), BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, false, EaseType.Linear, lightAxis, false);

    public static LightRotationEventBoxEditorData CopyWithoutId(LightRotationEventBoxEditorData data) => LightRotationEventBoxEditorData.CreateNew(IndexFilterEditorData.Copy(data.indexFilter), data.beatDistributionParamType, data.beatDistributionParam, data.rotationDistributionParamType, data.rotationDistributionParam, data.rotationDistributionShouldAffectFirstBaseEvent, data.rotationDistributionEaseType, data.axis, data.flipRotation);

    public static LightRotationEventBoxEditorData CreateNew(
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType rotationDistributionParamType,
      float rotationDistributionParam,
      bool rotationDistributionShouldAffectFirstBaseEvent,
      EaseType rotationDistributionEaseType,
      LightAxis axis,
      bool flipRotation)
    {
      return LightRotationEventBoxEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), indexFilter, beatDistributionParamType, beatDistributionParam, rotationDistributionParamType, rotationDistributionParam, rotationDistributionShouldAffectFirstBaseEvent, rotationDistributionEaseType, axis, flipRotation);
    }

    public static LightRotationEventBoxEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType rotationDistributionParamType,
      float rotationDistributionParam,
      bool rotationDistributionShouldAffectFirstBaseEvent,
      EaseType rotationDistributionEaseType,
      LightAxis axis,
      bool flipRotation)
    {
      return new LightRotationEventBoxEditorData(id, indexFilter, beatDistributionParamType, beatDistributionParam, rotationDistributionParamType, rotationDistributionParam, rotationDistributionShouldAffectFirstBaseEvent, rotationDistributionEaseType, axis, flipRotation);
    }

    public static LightRotationEventBoxEditorData CreateExtension(LightAxis axis) => LightRotationEventBoxEditorData.CreateNew(IndexFilterEditorData.CreateExtensionIndexFilter(), BeatmapEventDataBox.DistributionParamType.Step, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, 0.0f, false, EaseType.Linear, axis, false);

    private LightRotationEventBoxEditorData(
      BeatmapEditorObjectId id,
      IndexFilterEditorData indexFilter,
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
      float beatDistributionParam,
      BeatmapEventDataBox.DistributionParamType rotationDistributionParamType,
      float rotationDistributionParam,
      bool rotationDistributionShouldAffectFirstBaseEvent,
      EaseType rotationDistributionEaseType,
      LightAxis axis,
      bool flipRotation)
      : base(id, indexFilter, beatDistributionParamType, beatDistributionParam)
    {
      this.rotationDistributionParamType = rotationDistributionParamType;
      this.rotationDistributionParam = rotationDistributionParam;
      this.rotationDistributionShouldAffectFirstBaseEvent = rotationDistributionShouldAffectFirstBaseEvent;
      this.rotationDistributionEaseType = rotationDistributionEaseType;
      this.axis = axis;
      this.flipRotation = flipRotation;
    }
  }
}
