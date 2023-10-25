// Decompiled with JetBrains decompiler
// Type: BeatmapEventDataBox
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeatmapEventDataBox
{
  private readonly float _beatDistributionParam;
  private readonly BeatmapEventDataBox.DistributionParamType _beatDistributionParamType;
  private readonly int _eventDistributionCount;
  private readonly BeatmapEventDataBox.DistributionParamType _eventDistributionParamType;
  private readonly float _eventDistributionParam;
  private readonly bool _eventDistributionShouldAffectFirstBaseEvent;
  private readonly EaseType _eventDistributionEaseType;

  public abstract int subtypeIdentifier { get; }

  public abstract float beatStep { get; }

  public IndexFilter indexFilter { get; }

  protected BeatmapEventDataBox(
    IndexFilter indexFilter,
    BeatmapEventDataBox.DistributionParamType beatDistributionParamType,
    float beatDistributionParam,
    BeatmapEventDataBox.DistributionParamType eventDistributionParamType,
    float eventDistributionParam,
    bool eventDistributionShouldAffectFirstBaseEvent,
    EaseType eventDistributionEaseType)
  {
    this.indexFilter = indexFilter;
    this._beatDistributionParam = beatDistributionParam;
    this._beatDistributionParamType = beatDistributionParamType;
    this._eventDistributionCount = indexFilter.limitAlsoAffectType.HasFlag((Enum) IndexFilter.IndexFilterLimitAlsoAffectType.Distribution) ? indexFilter.VisibleCount : indexFilter.Count;
    this._eventDistributionParamType = eventDistributionParamType;
    this._eventDistributionParam = eventDistributionParam;
    this._eventDistributionShouldAffectFirstBaseEvent = eventDistributionShouldAffectFirstBaseEvent;
    this._eventDistributionEaseType = eventDistributionEaseType;
  }

  public abstract void Unpack(
    float groupBoxBeat,
    int groupId,
    int elementId,
    int durationOrderIndex,
    int distributionOrderIndex,
    float maxBeat,
    IBeatToTimeConvertor beatToTimeConvertor,
    List<BeatmapEventData> output);

  protected float GetBeatStep(float lastBaseEventRelativeBeat)
  {
    int count = this.indexFilter.limitAlsoAffectType.HasFlag((Enum) IndexFilter.IndexFilterLimitAlsoAffectType.Duration) ? this.indexFilter.VisibleCount : this.indexFilter.Count;
    return this._beatDistributionParamType == BeatmapEventDataBox.DistributionParamType.Wave ? BeatmapEventDataBox.BeatDistributionParamToStep(Mathf.Max(this._beatDistributionParam - lastBaseEventRelativeBeat, 0.0f), this._beatDistributionParamType, count) : BeatmapEventDataBox.BeatDistributionParamToStep(this._beatDistributionParam, this._beatDistributionParamType, count);
  }

  protected float GetDistribution(bool isFirstBaseDataEvent, int distributionOrderIndex) => !this._eventDistributionShouldAffectFirstBaseEvent && isFirstBaseDataEvent ? 0.0f : BeatmapEventDataBox.EventDistributionParamToStep(distributionOrderIndex, this._eventDistributionParam, this._eventDistributionParamType, this._eventDistributionCount, this._eventDistributionEaseType);

  private static float BeatDistributionParamToStep(
    float distributionParam,
    BeatmapEventDataBox.DistributionParamType distributionParamType,
    int count)
  {
    return distributionParamType == BeatmapEventDataBox.DistributionParamType.Wave ? distributionParam / (float) Mathf.Max(count - 1, 1) : distributionParam;
  }

  private static float EventDistributionParamToStep(
    int index,
    float distributionParam,
    BeatmapEventDataBox.DistributionParamType distributionParamType,
    int count,
    EaseType easeType)
  {
    return distributionParamType == BeatmapEventDataBox.DistributionParamType.Wave ? distributionParam * Interpolation.Interpolate((float) index / (float) Mathf.Max(count - 1, 1), easeType) : distributionParam * Interpolation.Interpolate((float) index / (float) count, easeType) * (float) count;
  }

  public enum DistributionParamType
  {
    Wave = 1,
    Step = 2,
  }
}
