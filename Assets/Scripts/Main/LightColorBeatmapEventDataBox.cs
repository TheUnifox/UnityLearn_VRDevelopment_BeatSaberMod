// Decompiled with JetBrains decompiler
// Type: LightColorBeatmapEventDataBox
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class LightColorBeatmapEventDataBox : BeatmapEventDataBox
{
  protected readonly IReadOnlyList<LightColorBaseData> _lightColorBaseDataList;
  protected readonly float _brightnessStep;
  protected readonly float _beatStep;
  protected readonly bool _brightnessDistributionShouldAffectFirstBaseEvent;

  public override int subtypeIdentifier => 0;

  public override float beatStep => this._beatStep;

  public LightColorBeatmapEventDataBox(
    IndexFilter indexFilter,
    float beatDistributionParam,
    BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
    float brightnessDistributionParam,
    BeatmapEventDataBox.DistributionParamType brightnessDistributionParamType,
    bool brightnessDistributionShouldAffectFirstBaseEvent,
    EaseType brightnessDistributionEaseType,
    IReadOnlyList<LightColorBaseData> lightColorBaseDataList)
    : base(indexFilter, beatDistributionParamType, beatDistributionParam, brightnessDistributionParamType, brightnessDistributionParam, brightnessDistributionShouldAffectFirstBaseEvent, brightnessDistributionEaseType)
  {
    this._lightColorBaseDataList = lightColorBaseDataList;
    this._beatStep = this.GetBeatStep(this._lightColorBaseDataList == null || this._lightColorBaseDataList.Count <= 0 ? 0.0f : this._lightColorBaseDataList[this._lightColorBaseDataList.Count - 1].beat);
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
    float num = (float) durationOrderIndex * this._beatStep;
    foreach (LightColorBaseData lightColorBaseData in (IEnumerable<LightColorBaseData>) this._lightColorBaseDataList)
    {
      float distribution = this.GetDistribution(isFirstBaseDataEvent, distributionOrderIndex);
      isFirstBaseDataEvent = false;
      float beat = groupBoxBeat + lightColorBaseData.beat + num;
      if ((double) beat < (double) maxBeat)
      {
        float time = beatToTimeConvertor.ConvertBeatToTime(beat);
        output.Add((BeatmapEventData) new LightColorBeatmapEventData(time, groupId, elementId, lightColorBaseData.transitionType, lightColorBaseData.colorType, lightColorBaseData.brightness + distribution, lightColorBaseData.strobeBeatFrequency));
      }
    }
  }
}
