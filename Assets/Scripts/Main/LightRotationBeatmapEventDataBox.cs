// Decompiled with JetBrains decompiler
// Type: LightRotationBeatmapEventDataBox
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class LightRotationBeatmapEventDataBox : BeatmapEventDataBox
{
  protected readonly IReadOnlyList<LightRotationBaseData> _lightRotationBaseDataList;
  protected readonly LightAxis _axis;
  protected readonly float _rotationDirection;
  protected readonly float _rotationStep;
  protected readonly float _beatStep;

  public override int subtypeIdentifier => (int) this._axis;

  public override float beatStep => this._beatStep;

  public LightRotationBeatmapEventDataBox(
    IndexFilter indexFilter,
    float beatDistributionParam,
    BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
    LightAxis axis,
    bool flipRotation,
    float rotationDistributionParam,
    BeatmapEventDataBox.DistributionParamType rotationDistributionParamType,
    bool rotationDistributionShouldAffectFirstBaseEvent,
    EaseType rotationDistributionEaseType,
    IReadOnlyList<LightRotationBaseData> lightRotationBaseDataList)
    : base(indexFilter, beatDistributionParamType, beatDistributionParam, rotationDistributionParamType, rotationDistributionParam, rotationDistributionShouldAffectFirstBaseEvent, rotationDistributionEaseType)
  {
    this._axis = axis;
    this._lightRotationBaseDataList = lightRotationBaseDataList;
    this._rotationDirection = flipRotation ? -1f : 1f;
    this._beatStep = this.GetBeatStep(this._lightRotationBaseDataList == null || this._lightRotationBaseDataList.Count <= 0 ? 0.0f : this._lightRotationBaseDataList[this._lightRotationBaseDataList.Count - 1].beat);
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
    float num1 = this._beatStep * (float) durationOrderIndex;
    foreach (LightRotationBaseData rotationBaseData in (IEnumerable<LightRotationBaseData>) this._lightRotationBaseDataList)
    {
      float distribution = this.GetDistribution(isFirstBaseDataEvent, distributionOrderIndex);
      isFirstBaseDataEvent = false;
      int num2 = Mathf.FloorToInt(Mathf.Abs(distribution) / 360f);
      float num3 = Mathf.Abs(distribution) % 360f * Mathf.Sign(distribution);
      float beat = groupBoxBeat + rotationBaseData.beat + num1;
      if ((double) beat <= (double) maxBeat)
      {
        float time = beatToTimeConvertor.ConvertBeatToTime(beat);
        output.Add((BeatmapEventData) new LightRotationBeatmapEventData(time, groupId, elementId, rotationBaseData.usePreviousEventRotationValue, rotationBaseData.easeType.FromEaseType(), this._axis, (num3 + rotationBaseData.rotation) * this._rotationDirection, rotationBaseData.loopsCount + num2, rotationBaseData.rotationDirection));
      }
    }
  }
}
