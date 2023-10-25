// Decompiled with JetBrains decompiler
// Type: BeatmapEventDataBoxGroupFactory
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public abstract class BeatmapEventDataBoxGroupFactory
{
  public static BeatmapEventDataBoxGroup CreateExtendColorBeatmapEventDataBoxGroup(
    float beat,
    int numberOfElements)
  {
    return new BeatmapEventDataBoxGroup(beat, (IReadOnlyCollection<BeatmapEventDataBox>) new LightColorBeatmapEventDataBox[1]
    {
      new LightColorBeatmapEventDataBox(new IndexFilter(0, numberOfElements - 1, numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 0.0f, BeatmapEventDataBox.DistributionParamType.Step, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, false, EaseType.Linear, (IReadOnlyList<LightColorBaseData>) new LightColorBaseData[1]
      {
        new LightColorBaseData(0.0f, BeatmapEventTransitionType.Extend, EnvironmentColorType.None, 0.0f, 0)
      })
    });
  }

  public static BeatmapEventDataBoxGroup CreateExtendRotationBeatmapEventDataBoxGroup(
    float beat,
    int numberOfElements)
  {
    return new BeatmapEventDataBoxGroup(beat, (IReadOnlyCollection<BeatmapEventDataBox>) new LightRotationBeatmapEventDataBox[2]
    {
      new LightRotationBeatmapEventDataBox(new IndexFilter(0, numberOfElements - 1, numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.X, false, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, false, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[1]
      {
        new LightRotationBaseData(0.0f, true, EaseType.None, 0.0f, 0, LightRotationDirection.Automatic)
      }),
      new LightRotationBeatmapEventDataBox(new IndexFilter(0, numberOfElements - 1, numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.Y, false, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, false, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[1]
      {
        new LightRotationBaseData(0.0f, true, EaseType.None, 0.0f, 0, LightRotationDirection.Automatic)
      })
    });
  }

  public static BeatmapEventDataBoxGroup CreateSingleLightBeatmapEventDataBoxGroup(
    float beat,
    int numberOfElements,
    DefaultEnvironmentEvents.LightGroupEvent lightGroupEvent)
  {
    IndexFilter indexFilter1 = CreateIndexFilter(lightGroupEvent.brightnessFiltering);
    IndexFilter indexFilter2 = CreateIndexFilter(lightGroupEvent.rotationFiltering);
    IndexFilter indexFilter3 = CreateIndexFilter(lightGroupEvent.translationFiltering);
    return new BeatmapEventDataBoxGroup(beat, (IReadOnlyCollection<BeatmapEventDataBox>) new BeatmapEventDataBox[7]
    {
      (BeatmapEventDataBox) new LightColorBeatmapEventDataBox(indexFilter1, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, (float) (lightGroupEvent.brightnessDistribution.useDistribution ? (double) lightGroupEvent.brightnessDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.brightnessDistribution.useDistribution ? (int) lightGroupEvent.brightnessDistribution.distributionParamType : 2), (lightGroupEvent.brightnessDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightColorBaseData>) new LightColorBaseData[1]
      {
        new LightColorBaseData(0.0f, BeatmapEventTransitionType.Instant, lightGroupEvent.environmentColorType, lightGroupEvent.brightness, 0)
      }),
      (BeatmapEventDataBox) new LightRotationBeatmapEventDataBox(indexFilter2, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.X, false, (float) (lightGroupEvent.rotationXDistribution.useDistribution ? (double) lightGroupEvent.rotationXDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.rotationXDistribution.useDistribution ? (int) lightGroupEvent.rotationXDistribution.distributionParamType : 2), (lightGroupEvent.rotationXDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[1]
      {
        new LightRotationBaseData(0.0f, false, EaseType.None, lightGroupEvent.rotationX, 0, LightRotationDirection.Automatic)
      }),
      (BeatmapEventDataBox) new LightRotationBeatmapEventDataBox(indexFilter2, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.Y, false, (float) (lightGroupEvent.rotationYDistribution.useDistribution ? (double) lightGroupEvent.rotationYDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.rotationYDistribution.useDistribution ? (int) lightGroupEvent.rotationYDistribution.distributionParamType : 2), (lightGroupEvent.rotationYDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[1]
      {
        new LightRotationBaseData(0.0f, false, EaseType.None, lightGroupEvent.rotationY, 0, LightRotationDirection.Automatic)
      }),
      (BeatmapEventDataBox) new LightRotationBeatmapEventDataBox(indexFilter2, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.Z, false, (float) (lightGroupEvent.rotationZDistribution.useDistribution ? (double) lightGroupEvent.rotationZDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.rotationZDistribution.useDistribution ? (int) lightGroupEvent.rotationZDistribution.distributionParamType : 2), (lightGroupEvent.rotationZDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[1]
      {
        new LightRotationBaseData(0.0f, false, EaseType.None, lightGroupEvent.rotationZ, 0, LightRotationDirection.Automatic)
      }),
      (BeatmapEventDataBox) new LightTranslationBeatmapEventDataBox(indexFilter3, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.X, false, (float) (lightGroupEvent.translationXDistribution.useDistribution ? (double) lightGroupEvent.translationXDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.translationXDistribution.useDistribution ? (int) lightGroupEvent.translationXDistribution.distributionParamType : 2), (lightGroupEvent.translationXDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightTranslationBaseData>) new LightTranslationBaseData[1]
      {
        new LightTranslationBaseData(0.0f, false, EaseType.None, lightGroupEvent.translationX)
      }),
      (BeatmapEventDataBox) new LightTranslationBeatmapEventDataBox(indexFilter3, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.Y, false, (float) (lightGroupEvent.translationYDistribution.useDistribution ? (double) lightGroupEvent.translationYDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.translationYDistribution.useDistribution ? (int) lightGroupEvent.translationYDistribution.distributionParamType : 2), (lightGroupEvent.translationYDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightTranslationBaseData>) new LightTranslationBaseData[1]
      {
        new LightTranslationBaseData(0.0f, false, EaseType.None, lightGroupEvent.translationY)
      }),
      (BeatmapEventDataBox) new LightTranslationBeatmapEventDataBox(indexFilter3, 0.0f, BeatmapEventDataBox.DistributionParamType.Step, LightAxis.Z, false, (float) (lightGroupEvent.translationZDistribution.useDistribution ? (double) lightGroupEvent.translationZDistribution.distributionParam : 0.0), (BeatmapEventDataBox.DistributionParamType) (lightGroupEvent.translationZDistribution.useDistribution ? (int) lightGroupEvent.translationZDistribution.distributionParamType : 2), (lightGroupEvent.translationZDistribution.useDistribution ? 1 : 0) != 0, EaseType.Linear, (IReadOnlyList<LightTranslationBaseData>) new LightTranslationBaseData[1]
      {
        new LightTranslationBaseData(0.0f, false, EaseType.None, lightGroupEvent.translationZ)
      })
    });

    IndexFilter CreateIndexFilter(
      DefaultEnvironmentEvents.LightGroupFiltering filtering)
    {
      IndexFilter.IndexFilterRandomType random = filtering.useFiltering ? filtering.randomType : IndexFilter.IndexFilterRandomType.NoRandom;
      int seed = filtering.useFiltering ? filtering.seed : 0;
      float limit = filtering.useFiltering ? filtering.limit : 0.0f;
      IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType = filtering.useFiltering ? filtering.alsoAffectType : IndexFilter.IndexFilterLimitAlsoAffectType.None;
      return new IndexFilter(0, numberOfElements - 1, numberOfElements, random, seed, 1, limit, limitAlsoAffectType);
    }
  }
}
