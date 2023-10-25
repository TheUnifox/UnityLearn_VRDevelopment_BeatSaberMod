// Decompiled with JetBrains decompiler
// Type: BeatmapDataStrobeFilterTransform
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class BeatmapDataStrobeFilterTransform
{
  private const float kMaxSecondsToConsiderStrobe = 0.1f;

  public static IReadonlyBeatmapData CreateTransformedData(
    IReadonlyBeatmapData beatmapData,
    EnvironmentIntensityReductionOptions environmentIntensityReductionOptions)
  {
    BeatmapData transformedData = new BeatmapData(beatmapData.numberOfLines);
    bool flag1 = environmentIntensityReductionOptions.compressExpand == EnvironmentIntensityReductionOptions.CompressExpandReductionType.RemoveWithStrobeFilter;
    bool flag2 = environmentIntensityReductionOptions.rotateRings == EnvironmentIntensityReductionOptions.RotateRingsReductionType.RemoveWithStrobeFilter;
    Dictionary<BasicBeatmapEventType, BeatmapDataStrobeFilterTransform.StrobeStreakData> dictionary = new Dictionary<BasicBeatmapEventType, BeatmapDataStrobeFilterTransform.StrobeStreakData>()
    {
      {
        BasicBeatmapEventType.Event0,
        new BeatmapDataStrobeFilterTransform.StrobeStreakData()
      },
      {
        BasicBeatmapEventType.Event1,
        new BeatmapDataStrobeFilterTransform.StrobeStreakData()
      },
      {
        BasicBeatmapEventType.Event2,
        new BeatmapDataStrobeFilterTransform.StrobeStreakData()
      },
      {
        BasicBeatmapEventType.Event3,
        new BeatmapDataStrobeFilterTransform.StrobeStreakData()
      },
      {
        BasicBeatmapEventType.Event4,
        new BeatmapDataStrobeFilterTransform.StrobeStreakData()
      }
    };
    foreach (BeatmapDataItem allBeatmapDataItem in beatmapData.allBeatmapDataItems)
    {
      switch (allBeatmapDataItem)
      {
        case LightColorBeatmapEventData beatmapEventData3:
          beatmapEventData3.DisableStrobe();
          transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) beatmapEventData3);
          continue;
        case BasicBeatmapEventData beatmapEventData4:
          if ((!flag1 || beatmapEventData4.basicBeatmapEventType != BasicBeatmapEventType.Event9) && (!flag2 || beatmapEventData4.basicBeatmapEventType != BasicBeatmapEventType.Event8))
          {
            if (!beatmapEventData4.basicBeatmapEventType.IsCoreLightIntensityChangeEvent())
            {
              transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) beatmapEventData4);
              continue;
            }
            if (beatmapEventData4.basicBeatmapEventType.IsCoreLightIntensityChangeEvent() && beatmapEventData4.HasLightFadeEventDataValue())
            {
              transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) beatmapEventData4);
              continue;
            }
            BeatmapDataStrobeFilterTransform.StrobeStreakData strobeStreakData = dictionary[beatmapEventData4.basicBeatmapEventType];
            if (strobeStreakData.isActive)
            {
              if ((double) beatmapEventData4.time - (double) strobeStreakData.lastSwitchTime < 0.10000000149011612)
              {
                strobeStreakData.AddStrobeData(beatmapEventData4);
                continue;
              }
              if (!Mathf.Approximately(strobeStreakData.strobeStartTime, strobeStreakData.lastSwitchTime))
              {
                int onEventDataValue = BeatmapDataStrobeFilterTransform.GetOnEventDataValue(strobeStreakData.startColorType);
                BasicBeatmapEventData beatmapEventData1 = new BasicBeatmapEventData(strobeStreakData.strobeStartTime, beatmapEventData4.basicBeatmapEventType, onEventDataValue, beatmapEventData4.floatValue);
                transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) beatmapEventData1);
                int num = !strobeStreakData.lastIsOn ? BeatmapDataStrobeFilterTransform.GetFlashAndFadeToBlackEventDataValue(strobeStreakData.lastColorType) : BeatmapDataStrobeFilterTransform.GetOnEventDataValue(strobeStreakData.lastColorType);
                BasicBeatmapEventData beatmapEventData2 = new BasicBeatmapEventData(strobeStreakData.lastSwitchTime, beatmapEventData4.basicBeatmapEventType, num, beatmapEventData4.floatValue);
                transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) beatmapEventData2);
              }
              else
                transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) strobeStreakData.originalBasicBeatmapEventData);
              strobeStreakData.StartPotentialStrobe(beatmapEventData4);
              continue;
            }
            strobeStreakData.StartPotentialStrobe(beatmapEventData4);
            continue;
          }
          continue;
        case BeatmapEventData beatmapEventData5:
          transformedData.InsertBeatmapEventDataInOrder(beatmapEventData5);
          continue;
        case BeatmapObjectData beatmapObjectData:
          transformedData.AddBeatmapObjectDataInOrder(beatmapObjectData);
          continue;
        default:
          continue;
      }
    }
    foreach (KeyValuePair<BasicBeatmapEventType, BeatmapDataStrobeFilterTransform.StrobeStreakData> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.isActive)
        transformedData.InsertBeatmapEventDataInOrder((BeatmapEventData) keyValuePair.Value.originalBasicBeatmapEventData);
    }
    return (IReadonlyBeatmapData) transformedData;
  }

  private static int GetOnEventDataValue(EnvironmentColorType lightColorType)
  {
    switch (lightColorType)
    {
      case EnvironmentColorType.None:
        return 0;
      case EnvironmentColorType.Color0:
        return 1;
      case EnvironmentColorType.Color1:
        return 5;
      case EnvironmentColorType.ColorW:
        return 9;
      default:
        return 1;
    }
  }

  private static int GetFlashAndFadeToBlackEventDataValue(EnvironmentColorType lightColorType)
  {
    switch (lightColorType)
    {
      case EnvironmentColorType.None:
        return -1;
      case EnvironmentColorType.Color0:
        return 3;
      case EnvironmentColorType.Color1:
        return 7;
      case EnvironmentColorType.ColorW:
        return 11;
      default:
        return 3;
    }
  }

  private class StrobeStreakData
  {
    public bool isActive;
    public float strobeStartTime;
    public EnvironmentColorType startColorType;
    public float lastSwitchTime;
    public EnvironmentColorType lastColorType;
    public bool lastIsOn;
    public BasicBeatmapEventData originalBasicBeatmapEventData;
    private bool _foundFirstColoredEventData;

    public void StartPotentialStrobe(BasicBeatmapEventData startBasicBeatmapEventData)
    {
      this.isActive = true;
      bool flag1 = startBasicBeatmapEventData.value != 0;
      bool flag2 = startBasicBeatmapEventData.value == 3 || startBasicBeatmapEventData.value == 7 || startBasicBeatmapEventData.value == 11 || startBasicBeatmapEventData.value == -1;
      this.lastIsOn = flag1 && !flag2;
      this.strobeStartTime = this.lastSwitchTime = startBasicBeatmapEventData.time;
      if (flag1)
      {
        this.startColorType = this.lastColorType = startBasicBeatmapEventData.LightColorTypeFromEventDataValue();
        this._foundFirstColoredEventData = true;
      }
      else
        this._foundFirstColoredEventData = false;
      this.originalBasicBeatmapEventData = startBasicBeatmapEventData;
    }

    public void AddStrobeData(BasicBeatmapEventData basicBeatmapEventData)
    {
      this.lastSwitchTime = basicBeatmapEventData.time;
      this.lastColorType = basicBeatmapEventData.LightColorTypeFromEventDataValue();
      bool flag1 = basicBeatmapEventData.value != 0;
      bool flag2 = basicBeatmapEventData.value == 3 || basicBeatmapEventData.value == 7 || basicBeatmapEventData.value == 11 || basicBeatmapEventData.value == -1;
      this.lastIsOn = flag1 && !flag2;
      if (!(!this._foundFirstColoredEventData & flag1))
        return;
      this.startColorType = this.lastColorType;
      this._foundFirstColoredEventData = true;
    }
  }
}
