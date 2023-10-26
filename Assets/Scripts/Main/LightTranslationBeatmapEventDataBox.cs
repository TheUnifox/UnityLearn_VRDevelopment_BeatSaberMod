// Decompiled with JetBrains decompiler
// Type: LightTranslationBeatmapEventDataBox
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class LightTranslationBeatmapEventDataBox : BeatmapEventDataBox
{
  protected readonly IReadOnlyList<LightTranslationBaseData> _lightTranslationBaseDataList;
  protected readonly LightAxis _axis;
  protected readonly float _translationDirection;
  protected readonly float _beatStep;

  public override int subtypeIdentifier => (int) this._axis;

  public override float beatStep => this._beatStep;

  public LightTranslationBeatmapEventDataBox(
    IndexFilter indexFilter,
    float beatDistributionParam,
    BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
    LightAxis axis,
    bool flipTranslation,
    float gapDistributionParam,
    BeatmapEventDataBox.DistributionParamType gapDistributionParamType,
    bool gapDistributionShouldAffectFirstBaseEvent,
    EaseType gapDistributionEaseType,
    IReadOnlyList<LightTranslationBaseData> lightTranslationBaseDataList)
    : base(indexFilter, beatDistributionParamType, beatDistributionParam, gapDistributionParamType, gapDistributionParam, gapDistributionShouldAffectFirstBaseEvent, gapDistributionEaseType)
  {
    this._axis = axis;
    this._lightTranslationBaseDataList = lightTranslationBaseDataList;
    this._translationDirection = flipTranslation ? -1f : 1f;
    this._beatStep = this.GetBeatStep(this._lightTranslationBaseDataList == null || this._lightTranslationBaseDataList.Count <= 0 ? 0.0f : this._lightTranslationBaseDataList[this._lightTranslationBaseDataList.Count - 1].beat);
  }

  public override void Unpack(
    float groupBoxBeat,
    int groupId,
    int elementId,
    int durationOrderIndex,
    int distributionOrderIndex,
    float maxBeat,
    IBeatToTimeConvertor beatToTimeConvertor,
    List<BeatmapEventData> output)
  {
    bool isFirstBaseDataEvent = true;
    float num = this._beatStep * (float) durationOrderIndex;
    foreach (LightTranslationBaseData translationBaseData in (IEnumerable<LightTranslationBaseData>) this._lightTranslationBaseDataList)
    {
      float distribution = this.GetDistribution(isFirstBaseDataEvent, distributionOrderIndex);
      isFirstBaseDataEvent = false;
      float beat = groupBoxBeat + translationBaseData.beat + num;
      if ((double) beat <= (double) maxBeat)
      {
        float time = beatToTimeConvertor.ConvertBeatToTime(beat);
        output.Add((BeatmapEventData) new LightTranslationBeatmapEventData(time, groupId, elementId, translationBaseData.usePreviousEventTranslationValue, translationBaseData.easeType.FromEaseType(), this._axis, translationBaseData.translation * this._translationDirection, distribution * this._translationDirection));
      }
    }
  }
}
