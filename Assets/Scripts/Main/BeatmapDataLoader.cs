// Decompiled with JetBrains decompiler
// Type: BeatmapDataLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BeatmapDataLoader
{
  protected const int kDefaultNumberOfLines = 4;

  private static BeatmapData GetBeatmapDataFromBeatmapSaveData(
    BeatmapSaveDataVersion3.BeatmapSaveData beatmapSaveData,
    BeatmapDifficulty beatmapDifficulty,
    float startBpm,
    bool loadingForDesignatedEnvironment,
    EnvironmentKeywords environmentKeywords,
    EnvironmentLightGroups environmentLightGroups,
    DefaultEnvironmentEvents defaultEnvironmentEvents,
    PlayerSpecificSettings playerSpecificSettings)
  {
    bool flag = ((loadingForDesignatedEnvironment ? 1 : (!beatmapSaveData.useNormalEventsAsCompatibleEvents ? 0 : (defaultEnvironmentEvents.isEmpty ? 1 : 0))) & (playerSpecificSettings == null ? (true ? 1 : 0) : (playerSpecificSettings.GetEnvironmentEffectsFilterPreset(beatmapDifficulty) != EnvironmentEffectsFilterPreset.NoEffects ? 1 : 0))) != 0;
    BeatmapData beatmapData = new BeatmapData(4);
    beatmapData.InsertBeatmapEventData((BeatmapEventData) new BPMChangeBeatmapEventData(-100f, startBpm));
    List<BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData> bpmEvents = beatmapSaveData.bpmEvents;
    foreach (BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword eventTypesForKeyword in beatmapSaveData.basicEventTypesWithKeywords.data)
      beatmapData.AddSpecialBasicBeatmapEventKeyword(eventTypesForKeyword.keyword);
    BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor = new BeatmapDataLoader.BpmTimeProcessor(startBpm, bpmEvents);
    BeatmapDataLoader.SpecialEventsFilter specialEventsFilter = new BeatmapDataLoader.SpecialEventsFilter(beatmapSaveData.basicEventTypesWithKeywords, environmentKeywords);
    MultipleSortedListsEnumerator<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem> sortedListsEnumerator = new MultipleSortedListsEnumerator<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>(new IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>[6]
    {
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.colorNotes,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.bombNotes,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.obstacles,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.sliders,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.burstSliders,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.waypoints
    });
    DataConvertor<BeatmapObjectData> dataConvertor1 = new DataConvertor<BeatmapObjectData>();
    dataConvertor1.AddConvertor((DataItemConvertor<BeatmapObjectData>) new BeatmapDataLoader.ColorNoteConvertor(bpmTimeProcessor));
    dataConvertor1.AddConvertor((DataItemConvertor<BeatmapObjectData>) new BeatmapDataLoader.BombNoteConvertor(bpmTimeProcessor));
    dataConvertor1.AddConvertor((DataItemConvertor<BeatmapObjectData>) new BeatmapDataLoader.ObstacleConvertor(bpmTimeProcessor));
    dataConvertor1.AddConvertor((DataItemConvertor<BeatmapObjectData>) new BeatmapDataLoader.SliderConvertor(bpmTimeProcessor));
    dataConvertor1.AddConvertor((DataItemConvertor<BeatmapObjectData>) new BeatmapDataLoader.BurstSliderConvertor(bpmTimeProcessor));
    dataConvertor1.AddConvertor((DataItemConvertor<BeatmapObjectData>) new BeatmapDataLoader.WaypointConvertor(bpmTimeProcessor));
    foreach (BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem beatmapSaveDataItem in sortedListsEnumerator)
    {
      BeatmapObjectData beatmapObjectData = dataConvertor1.ProcessItem((object) beatmapSaveDataItem);
      if (beatmapObjectData != null)
        beatmapData.AddBeatmapObjectData(beatmapObjectData);
    }
    bpmTimeProcessor.Reset();
    DataConvertor<BeatmapEventData> dataConvertor2 = new DataConvertor<BeatmapEventData>();
    dataConvertor2.AddConvertor((DataItemConvertor<BeatmapEventData>) new BeatmapDataLoader.BpmEventConvertor(bpmTimeProcessor));
    dataConvertor2.AddConvertor((DataItemConvertor<BeatmapEventData>) new BeatmapDataLoader.RotationEventConvertor(bpmTimeProcessor));
    if (flag)
    {
      dataConvertor2.AddConvertor((DataItemConvertor<BeatmapEventData>) new BeatmapDataLoader.BasicEventConvertor(bpmTimeProcessor, specialEventsFilter));
      dataConvertor2.AddConvertor((DataItemConvertor<BeatmapEventData>) new BeatmapDataLoader.ColorBoostEventConvertor(bpmTimeProcessor));
    }
    else
    {
      dataConvertor2.AddConvertor((DataItemConvertor<BeatmapEventData>) new DefaultDataConvertor<BeatmapEventData, BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData, BasicBeatmapEventData>());
      dataConvertor2.AddConvertor((DataItemConvertor<BeatmapEventData>) new DefaultDataConvertor<BeatmapEventData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData, ColorBoostBeatmapEventData>());
    }
    foreach (BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem beatmapSaveDataItem in new MultipleSortedListsEnumerator<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>(new IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>[4]
    {
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) bpmEvents,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.basicBeatmapEvents,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.colorBoostBeatmapEvents,
      (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem>) beatmapSaveData.rotationEvents
    }))
    {
      BeatmapEventData beatmapEventData = dataConvertor2.ProcessItem((object) beatmapSaveDataItem);
      if (beatmapEventData != null)
        beatmapData.InsertBeatmapEventData(beatmapEventData);
    }
    bpmTimeProcessor.Reset();
    BeatmapEventDataBoxGroupLists beatmapEventDataBoxGroupLists = new BeatmapEventDataBoxGroupLists(beatmapData, (IBeatToTimeConvertor) bpmTimeProcessor, false);
    if (flag)
    {
      BeatmapDataLoader.EventBoxGroupConvertor boxGroupConvertor = new BeatmapDataLoader.EventBoxGroupConvertor(environmentLightGroups);
      foreach (BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup eventBoxGroupSaveData in new MultipleSortedListsEnumerator<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>(new IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>[3]
      {
        (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>) beatmapSaveData.lightColorEventBoxGroups,
        (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>) beatmapSaveData.lightRotationEventBoxGroups,
        (IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>) beatmapSaveData.lightTranslationEventBoxGroups
      }))
      {
        BeatmapEventDataBoxGroup beatmapEventDataBoxGroup = boxGroupConvertor.Convert(eventBoxGroupSaveData);
        if (beatmapEventDataBoxGroup != null)
          beatmapEventDataBoxGroupLists.Insert(eventBoxGroupSaveData.groupId, beatmapEventDataBoxGroup);
      }
    }
    else
      DefaultEnvironmentEventsFactory.InsertDefaultEnvironmentEvents(beatmapData, beatmapEventDataBoxGroupLists, defaultEnvironmentEvents, environmentLightGroups);
    beatmapEventDataBoxGroupLists.SyncWithBeatmapData();
    beatmapData.ProcessRemainingData();
    beatmapData.ProcessAndSortBeatmapData();
    return beatmapData;
  }

  public static BeatmapDataBasicInfo GetBeatmapDataBasicInfoFromSaveData(
    BeatmapSaveDataVersion3.BeatmapSaveData beatmapSaveData)
  {
    if (beatmapSaveData == null)
      return (BeatmapDataBasicInfo) null;
    int count1 = beatmapSaveData.colorNotes.Count;
    int count2 = beatmapSaveData.obstacles.Count;
    int count3 = beatmapSaveData.bombNotes.Count;
    List<string> specialBasicBeatmapEventKeywords = new List<string>();
    foreach (BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword eventTypesForKeyword in beatmapSaveData.basicEventTypesWithKeywords.data)
      specialBasicBeatmapEventKeywords.Add(eventTypesForKeyword.keyword);
    return new BeatmapDataBasicInfo(4, count1, count2, count3, (IEnumerable<string>) specialBasicBeatmapEventKeywords);
  }

  public static BeatmapData GetBeatmapDataFromSaveData(
    BeatmapSaveDataVersion3.BeatmapSaveData beatmapSaveData,
    BeatmapDifficulty beatmapDifficulty,
    float startBpm,
    bool loadingForDesignatedEnvironment,
    EnvironmentInfoSO environmentInfo,
    PlayerSpecificSettings playerSpecificSettings)
  {
    if (beatmapSaveData == null)
      return (BeatmapData) null;
    EnvironmentKeywords environmentKeywords;
    EnvironmentLightGroups environmentLightGroups;
    DefaultEnvironmentEvents defaultEnvironmentEvents;
    if ((UnityEngine.Object) environmentInfo != (UnityEngine.Object) null)
    {
      environmentKeywords = new EnvironmentKeywords(environmentInfo.environmentKeywords);
      environmentLightGroups = environmentInfo.lightGroups;
      defaultEnvironmentEvents = environmentInfo.defaultEnvironmentEvents;
    }
    else
    {
      environmentKeywords = new EnvironmentKeywords((IReadOnlyList<string>) new string[0]);
      environmentLightGroups = new EnvironmentLightGroups();
      defaultEnvironmentEvents = new DefaultEnvironmentEvents();
    }
    return BeatmapDataLoader.GetBeatmapDataFromBeatmapSaveData(beatmapSaveData, beatmapDifficulty, startBpm, loadingForDesignatedEnvironment, environmentKeywords, environmentLightGroups, defaultEnvironmentEvents, playerSpecificSettings);
  }

  private static ColorType ConvertColorType(BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType noteType)
  {
    if (noteType == BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA)
      return ColorType.ColorA;
    return noteType == BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorB ? ColorType.ColorB : ColorType.None;
  }

  private static EnvironmentColorType ConvertColorType(
    BeatmapSaveDataVersion3.BeatmapSaveData.EnvironmentColorType environmentColorType)
  {
    switch (environmentColorType)
    {
      case BeatmapSaveDataVersion3.BeatmapSaveData.EnvironmentColorType.Color0:
        return EnvironmentColorType.Color0;
      case BeatmapSaveDataVersion3.BeatmapSaveData.EnvironmentColorType.Color1:
        return EnvironmentColorType.Color1;
      case BeatmapSaveDataVersion3.BeatmapSaveData.EnvironmentColorType.ColorWhite:
        return EnvironmentColorType.ColorW;
      default:
        return EnvironmentColorType.Color0;
    }
  }

  private static BeatmapEventTransitionType ConvertBeatmapEventTransitionType(
    BeatmapSaveDataVersion3.BeatmapSaveData.TransitionType transitionType)
  {
    switch (transitionType)
    {
      case BeatmapSaveDataVersion3.BeatmapSaveData.TransitionType.Instant:
        return BeatmapEventTransitionType.Instant;
      case BeatmapSaveDataVersion3.BeatmapSaveData.TransitionType.Interpolate:
        return BeatmapEventTransitionType.Interpolate;
      case BeatmapSaveDataVersion3.BeatmapSaveData.TransitionType.Extend:
        return BeatmapEventTransitionType.Extend;
      default:
        return BeatmapEventTransitionType.Instant;
    }
  }

  private static LightAxis ConvertAxis(BeatmapSaveDataVersion3.BeatmapSaveData.Axis axis)
  {
    switch (axis)
    {
      case BeatmapSaveDataVersion3.BeatmapSaveData.Axis.X:
        return LightAxis.X;
      case BeatmapSaveDataVersion3.BeatmapSaveData.Axis.Y:
        return LightAxis.Y;
      case BeatmapSaveDataVersion3.BeatmapSaveData.Axis.Z:
        return LightAxis.Z;
      default:
        return LightAxis.X;
    }
  }

  private static EaseType ConvertEaseType(BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType)
  {
    switch (easeType)
    {
      case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None:
        return EaseType.None;
      case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.Linear:
        return EaseType.Linear;
      case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InQuad:
        return EaseType.InQuad;
      case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.OutQuad:
        return EaseType.OutQuad;
      case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InOutQuad:
        return EaseType.InOutQuad;
      default:
        return EaseType.None;
    }
  }

  private static NoteLineLayer ConvertNoteLineLayer(int layer)
  {
    switch (layer)
    {
      case 0:
        return NoteLineLayer.Base;
      case 1:
        return NoteLineLayer.Upper;
      case 2:
        return NoteLineLayer.Top;
      default:
        return NoteLineLayer.Base;
    }
  }

  private static SliderData.Type ConvertSliderType(BeatmapSaveDataVersion3.BeatmapSaveData.SliderType sliderType) => sliderType == BeatmapSaveDataVersion3.BeatmapSaveData.SliderType.Normal || sliderType != BeatmapSaveDataVersion3.BeatmapSaveData.SliderType.Burst ? SliderData.Type.Normal : SliderData.Type.Burst;

  private static LightRotationDirection ConvertRotationOrientation(
    BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData.RotationDirection rotationDirection)
  {
    switch (rotationDirection)
    {
      case BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData.RotationDirection.Automatic:
        return LightRotationDirection.Automatic;
      case BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData.RotationDirection.Clockwise:
        return LightRotationDirection.Clockwise;
      case BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData.RotationDirection.Counterclockwise:
        return LightRotationDirection.Counterclockwise;
      default:
        return LightRotationDirection.Automatic;
    }
  }

  public class BpmTimeProcessor : IBeatToTimeConvertor
  {
    protected readonly List<BeatmapDataLoader.BpmTimeProcessor.BpmChangeData> _bpmChangeDataList;
    protected int currentBpmChangesDataIdx;

    public BpmTimeProcessor(
      float startBpm,
      List<BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData> bpmEventsSaveData)
    {
      int num = bpmEventsSaveData.Count <= 0 ? 0 : ((double) bpmEventsSaveData[0].beat == 0.0 ? 1 : 0);
      if (num != 0)
        startBpm = bpmEventsSaveData[0].bpm;
      this._bpmChangeDataList = new List<BeatmapDataLoader.BpmTimeProcessor.BpmChangeData>()
      {
        new BeatmapDataLoader.BpmTimeProcessor.BpmChangeData(0.0f, 0.0f, startBpm)
      };
      for (int index = num != 0 ? 1 : 0; index < bpmEventsSaveData.Count; ++index)
      {
        BeatmapDataLoader.BpmTimeProcessor.BpmChangeData bpmChangeData = this._bpmChangeDataList[this._bpmChangeDataList.Count - 1];
        float beat = bpmEventsSaveData[index].beat;
        float bpm = bpmEventsSaveData[index].bpm;
        this._bpmChangeDataList.Add(new BeatmapDataLoader.BpmTimeProcessor.BpmChangeData(bpmChangeData.bpmChangeStartTime + (float) (((double) beat - (double) bpmChangeData.bpmChangeStartBpmTime) / (double) bpmChangeData.bpm * 60.0), beat, bpm));
      }
    }

    public virtual float ConvertBeatToTime(float beat)
    {
      while (this.currentBpmChangesDataIdx > 0 && (double) this._bpmChangeDataList[this.currentBpmChangesDataIdx].bpmChangeStartBpmTime >= (double) beat)
        --this.currentBpmChangesDataIdx;
      while (this.currentBpmChangesDataIdx < this._bpmChangeDataList.Count - 1 && (double) this._bpmChangeDataList[this.currentBpmChangesDataIdx + 1].bpmChangeStartBpmTime < (double) beat)
        ++this.currentBpmChangesDataIdx;
      BeatmapDataLoader.BpmTimeProcessor.BpmChangeData bpmChangeData = this._bpmChangeDataList[this.currentBpmChangesDataIdx];
      return bpmChangeData.bpmChangeStartTime + (float) (((double) beat - (double) bpmChangeData.bpmChangeStartBpmTime) / (double) bpmChangeData.bpm * 60.0);
    }

    public virtual void Reset() => this.currentBpmChangesDataIdx = 0;

    public readonly struct BpmChangeData
    {
      public readonly float bpmChangeStartTime;
      public readonly float bpmChangeStartBpmTime;
      public readonly float bpm;

      public BpmChangeData(float bpmChangeStartTime, float bpmChangeStartBpmTime, float bpm)
      {
        this.bpmChangeStartTime = bpmChangeStartTime;
        this.bpmChangeStartBpmTime = bpmChangeStartBpmTime;
        this.bpm = bpm;
      }
    }
  }

  public class BasicEventConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapEventData, BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData, BasicBeatmapEventData>
  {
    protected readonly BeatmapDataLoader.SpecialEventsFilter _specialEventsFilter;

    public BasicEventConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor,
      BeatmapDataLoader.SpecialEventsFilter specialEventsFilter)
      : base(bpmTimeProcessor)
    {
      this._specialEventsFilter = specialEventsFilter;
    }

    protected override BasicBeatmapEventData Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData basicEventSaveData)
    {
      return !this._specialEventsFilter.IsEventValid(basicEventSaveData.eventType) ? (BasicBeatmapEventData) null : new BasicBeatmapEventData(this.BeatToTime(basicEventSaveData.beat), BeatmapDataLoader.BasicEventConvertor.ConvertFromBeatmapSaveDataBeatmapEventType(basicEventSaveData.eventType), basicEventSaveData.value, basicEventSaveData.floatValue);
    }

    private static BasicBeatmapEventType ConvertFromBeatmapSaveDataBeatmapEventType(
      BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType beatmapEventType)
    {
      return (BasicBeatmapEventType) beatmapEventType;
    }
  }

  public abstract class BeatmapDataItemConvertor<TBase, TIn, TOut> : 
    DataItemConvertor<TBase, TIn, TOut>
    where TBase : BeatmapDataItem
    where TIn : BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem
    where TOut : TBase
  {
    private readonly BeatmapDataLoader.BpmTimeProcessor _bpmTimeProcessor;

    protected BeatmapDataItemConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
    {
      this._bpmTimeProcessor = bpmTimeProcessor;
    }

    protected float BeatToTime(float beat) => this._bpmTimeProcessor.ConvertBeatToTime(beat);
  }

  public class BombNoteConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapObjectData, BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData, NoteData>
  {
    public BombNoteConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override NoteData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData bombNoteSaveData) => NoteData.CreateBombNoteData(this.BeatToTime(bombNoteSaveData.beat), bombNoteSaveData.line, BeatmapDataLoader.ConvertNoteLineLayer(bombNoteSaveData.layer));
  }

  public class BpmEventConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapEventData, BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData, BPMChangeBeatmapEventData>
  {
    public BpmEventConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override BPMChangeBeatmapEventData Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData bpmChangeEventSaveData)
    {
      return new BPMChangeBeatmapEventData(this.BeatToTime(bpmChangeEventSaveData.beat), bpmChangeEventSaveData.bpm);
    }
  }

  public class ColorBoostEventConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapEventData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData, ColorBoostBeatmapEventData>
  {
    public ColorBoostEventConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override ColorBoostBeatmapEventData Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData colorBoostEventSaveData)
    {
      return new ColorBoostBeatmapEventData(this.BeatToTime(colorBoostEventSaveData.beat), colorBoostEventSaveData.boost);
    }
  }

  public class ColorNoteConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapObjectData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData, NoteData>
  {
    public ColorNoteConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override NoteData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData colorNoteSaveData)
    {
      NoteData basicNoteData = NoteData.CreateBasicNoteData(this.BeatToTime(colorNoteSaveData.beat), colorNoteSaveData.line, BeatmapDataLoader.ConvertNoteLineLayer(colorNoteSaveData.layer), BeatmapDataLoader.ConvertColorType(colorNoteSaveData.color), colorNoteSaveData.cutDirection);
      basicNoteData.SetCutDirectionAngleOffset((float) colorNoteSaveData.angleOffset);
      return basicNoteData;
    }
  }

  public abstract class BeatmapEventDataBoxDistributionParamTypeConvertor
  {
    public static BeatmapEventDataBox.DistributionParamType Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType distributionParamType)
    {
      if (distributionParamType == BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType.Wave)
        return BeatmapEventDataBox.DistributionParamType.Wave;
      if (distributionParamType == BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType.Step)
        return BeatmapEventDataBox.DistributionParamType.Step;
      throw new ArgumentOutOfRangeException(nameof (distributionParamType), (object) distributionParamType, (string) null);
    }
  }

  public class EventBoxGroupConvertor
  {
    protected readonly DataConvertor<BeatmapEventDataBox, LightGroupSO> _dataConvertor;
    protected readonly EnvironmentLightGroups _lightGroups;

    public EventBoxGroupConvertor(EnvironmentLightGroups lightGroups)
    {
      this._lightGroups = lightGroups;
      this._dataConvertor = new DataConvertor<BeatmapEventDataBox, LightGroupSO>();
      this._dataConvertor.AddConvertor((DataItemConvertor<BeatmapEventDataBox, LightGroupSO>) new BeatmapDataLoader.LightColorEventBoxConvertor());
      this._dataConvertor.AddConvertor((DataItemConvertor<BeatmapEventDataBox, LightGroupSO>) new BeatmapDataLoader.LightRotationEventBoxConvertor());
      this._dataConvertor.AddConvertor((DataItemConvertor<BeatmapEventDataBox, LightGroupSO>) new BeatmapDataLoader.LightTranslationEventBoxConvertor());
    }

    public virtual BeatmapEventDataBoxGroup Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup eventBoxGroupSaveData)
    {
      List<BeatmapEventDataBox> beatmapEventDataBoxList = new List<BeatmapEventDataBox>(eventBoxGroupSaveData.baseEventBoxes.Count);
      LightGroupSO dataForGroup = this._lightGroups.GetDataForGroup(eventBoxGroupSaveData.groupId);
      if ((UnityEngine.Object) dataForGroup == (UnityEngine.Object) null)
        return (BeatmapEventDataBoxGroup) null;
      foreach (object baseEventBox in (IEnumerable<BeatmapSaveDataVersion3.BeatmapSaveData.EventBox>) eventBoxGroupSaveData.baseEventBoxes)
      {
        BeatmapEventDataBox beatmapEventDataBox = this._dataConvertor.ProcessItem(baseEventBox, dataForGroup);
        if (beatmapEventDataBox != null)
          beatmapEventDataBoxList.Add(beatmapEventDataBox);
      }
      return new BeatmapEventDataBoxGroup(eventBoxGroupSaveData.beat, (IReadOnlyCollection<BeatmapEventDataBox>) beatmapEventDataBoxList);
    }
  }

  public abstract class IndexFilterConvertor
  {
    public static IndexFilter Convert(BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter indexFilter, int groupSize)
    {
      int chunkSize = indexFilter.chunks == 0 ? 1 : Mathf.CeilToInt((float) groupSize / (float) indexFilter.chunks);
      int num1 = Mathf.CeilToInt((float) groupSize / (float) chunkSize);
      switch (indexFilter.type)
      {
        case BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter.IndexFilterType.Division:
          int num2 = indexFilter.param0;
          int num3 = indexFilter.param1;
          int num4 = Mathf.CeilToInt((float) num1 / (float) num2);
          if (indexFilter.reversed)
          {
            int start = num1 - num4 * num3 - 1;
            return new IndexFilter(start, Mathf.Max(0, start - num4 + 1), groupSize, (IndexFilter.IndexFilterRandomType) indexFilter.random, indexFilter.seed, chunkSize, indexFilter.limit, (IndexFilter.IndexFilterLimitAlsoAffectType) indexFilter.limitAlsoAffectsType);
          }
          int start1 = num4 * num3;
          return new IndexFilter(start1, Mathf.Min(num1 - 1, start1 + num4 - 1), groupSize, (IndexFilter.IndexFilterRandomType) indexFilter.random, indexFilter.seed, chunkSize, indexFilter.limit, (IndexFilter.IndexFilterLimitAlsoAffectType) indexFilter.limitAlsoAffectsType);
        case BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter.IndexFilterType.StepAndOffset:
          int start2 = indexFilter.param0;
          int step = indexFilter.param1;
          int num5 = num1 - start2;
          if (num5 <= 0)
            throw new ArgumentOutOfRangeException();
          int count = step == 0 ? 1 : Mathf.CeilToInt((float) num5 / (float) step);
          return indexFilter.reversed ? new IndexFilter(num1 - 1 - start2, -step, count, groupSize, (IndexFilter.IndexFilterRandomType) indexFilter.random, indexFilter.seed, chunkSize, indexFilter.limit, (IndexFilter.IndexFilterLimitAlsoAffectType) indexFilter.limitAlsoAffectsType) : new IndexFilter(start2, step, count, groupSize, (IndexFilter.IndexFilterRandomType) indexFilter.random, indexFilter.seed, chunkSize, indexFilter.limit, (IndexFilter.IndexFilterLimitAlsoAffectType) indexFilter.limitAlsoAffectsType);
        default:
          return (IndexFilter) null;
      }
    }

    public static bool IsIndexFilterValid(BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter indexFilter, int groupSize)
    {
      try
      {
        BeatmapDataLoader.IndexFilterConvertor.Convert(indexFilter, groupSize);
        return true;
      }
      catch (AssertionException ex)
      {
        return false;
      }
    }
  }

  public class LightColorEventBoxConvertor : 
    DataItemConvertor<BeatmapEventDataBox, BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox, LightColorBeatmapEventDataBox, LightGroupSO>
  {
    protected override LightColorBeatmapEventDataBox Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox saveData,
      LightGroupSO lightGroupData)
    {
      IndexFilter indexFilter = BeatmapDataLoader.IndexFilterConvertor.Convert(saveData.indexFilter, lightGroupData.numberOfElements);
      List<LightColorBaseData> lightColorBaseDataList = new List<LightColorBaseData>(saveData.lightColorBaseDataList.Count);
      foreach (BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData lightColorBaseData in saveData.lightColorBaseDataList)
        lightColorBaseDataList.Add(BeatmapDataLoader.LightColoBaseDataConvertor.Convert(lightColorBaseData));
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType = BeatmapDataLoader.BeatmapEventDataBoxDistributionParamTypeConvertor.Convert(saveData.beatDistributionParamType);
      BeatmapEventDataBox.DistributionParamType brightnessDistributionParamType = BeatmapDataLoader.BeatmapEventDataBoxDistributionParamTypeConvertor.Convert(saveData.brightnessDistributionParamType);
      return new LightColorBeatmapEventDataBox(indexFilter, saveData.beatDistributionParam, beatDistributionParamType, saveData.brightnessDistributionParam, brightnessDistributionParamType, saveData.brightnessDistributionShouldAffectFirstBaseEvent, BeatmapDataLoader.ConvertEaseType(saveData.brightnessDistributionEaseType), (IReadOnlyList<LightColorBaseData>) lightColorBaseDataList);
    }
  }

  public abstract class LightColoBaseDataConvertor
  {
    public static LightColorBaseData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData saveData) => new LightColorBaseData(saveData.beat, BeatmapDataLoader.ConvertBeatmapEventTransitionType(saveData.transitionType), BeatmapDataLoader.ConvertColorType(saveData.colorType), saveData.brightness, saveData.strobeBeatFrequency);
  }

  public class LightRotationEventBoxConvertor : 
    DataItemConvertor<BeatmapEventDataBox, BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox, LightRotationBeatmapEventDataBox, LightGroupSO>
  {
    protected override LightRotationBeatmapEventDataBox Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox saveData,
      LightGroupSO lightGroupData)
    {
      IndexFilter indexFilter = BeatmapDataLoader.IndexFilterConvertor.Convert(saveData.indexFilter, lightGroupData.numberOfElements);
      List<LightRotationBaseData> lightRotationBaseDataList = new List<LightRotationBaseData>(saveData.lightRotationBaseDataList.Count);
      foreach (BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData rotationBaseData in (IEnumerable<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>) saveData.lightRotationBaseDataList)
        lightRotationBaseDataList.Add(BeatmapDataLoader.LightRotationBaseDataConvertor.Convert(rotationBaseData));
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType = BeatmapDataLoader.BeatmapEventDataBoxDistributionParamTypeConvertor.Convert(saveData.beatDistributionParamType);
      BeatmapEventDataBox.DistributionParamType rotationDistributionParamType = BeatmapDataLoader.BeatmapEventDataBoxDistributionParamTypeConvertor.Convert(saveData.rotationDistributionParamType);
      return new LightRotationBeatmapEventDataBox(indexFilter, saveData.beatDistributionParam, beatDistributionParamType, BeatmapDataLoader.ConvertAxis(saveData.axis), saveData.flipRotation, saveData.rotationDistributionParam, rotationDistributionParamType, saveData.rotationDistributionShouldAffectFirstBaseEvent, BeatmapDataLoader.ConvertEaseType(saveData.rotationDistributionEaseType), (IReadOnlyList<LightRotationBaseData>) lightRotationBaseDataList);
    }
  }

  public abstract class LightRotationBaseDataConvertor
  {
    public static LightRotationBaseData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData saveData) => new LightRotationBaseData(saveData.beat, saveData.usePreviousEventRotationValue, BeatmapDataLoader.ConvertEaseType(saveData.easeType), saveData.rotation, saveData.loopsCount, BeatmapDataLoader.ConvertRotationOrientation(saveData.rotationDirection));
  }

  public class LightTranslationEventBoxConvertor : 
    DataItemConvertor<BeatmapEventDataBox, BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox, LightTranslationBeatmapEventDataBox, LightGroupSO>
  {
    protected override LightTranslationBeatmapEventDataBox Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox saveData,
      LightGroupSO lightGroupData)
    {
      IndexFilter indexFilter = BeatmapDataLoader.IndexFilterConvertor.Convert(saveData.indexFilter, lightGroupData.numberOfElements);
      List<LightTranslationBaseData> lightTranslationBaseDataList = new List<LightTranslationBaseData>(saveData.lightTranslationBaseDataList.Count);
      foreach (BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData translationBaseData in (IEnumerable<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>) saveData.lightTranslationBaseDataList)
        lightTranslationBaseDataList.Add(BeatmapDataLoader.LightTranslationBaseDataConvertor.Convert(translationBaseData));
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType = BeatmapDataLoader.BeatmapEventDataBoxDistributionParamTypeConvertor.Convert(saveData.beatDistributionParamType);
      BeatmapEventDataBox.DistributionParamType gapDistributionParamType = BeatmapDataLoader.BeatmapEventDataBoxDistributionParamTypeConvertor.Convert(saveData.gapDistributionParamType);
      return new LightTranslationBeatmapEventDataBox(indexFilter, saveData.beatDistributionParam, beatDistributionParamType, BeatmapDataLoader.ConvertAxis(saveData.axis), saveData.flipTranslation, saveData.gapDistributionParam, gapDistributionParamType, saveData.gapDistributionShouldAffectFirstBaseEvent, BeatmapDataLoader.ConvertEaseType(saveData.gapDistributionEaseType), (IReadOnlyList<LightTranslationBaseData>) lightTranslationBaseDataList);
    }
  }

  public abstract class LightTranslationBaseDataConvertor
  {
    public static LightTranslationBaseData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData saveData) => new LightTranslationBaseData(saveData.beat, saveData.usePreviousEventTranslationValue, BeatmapDataLoader.ConvertEaseType(saveData.easeType), saveData.translation);
  }

  public class ObstacleConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapObjectData, BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData, ObstacleData>
  {
    public ObstacleConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override ObstacleData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData obstacleSaveData)
    {
      float beat = obstacleSaveData.beat + obstacleSaveData.duration;
      float time = this.BeatToTime(obstacleSaveData.beat);
      float duration = this.BeatToTime(beat) - time;
      return new ObstacleData(time, obstacleSaveData.line, BeatmapDataLoader.ObstacleConvertor.GetNoteLineLayer(obstacleSaveData.layer), duration, obstacleSaveData.width, obstacleSaveData.height);
    }

    private static NoteLineLayer GetNoteLineLayer(int lineLayer)
    {
      switch (lineLayer)
      {
        case 0:
          return NoteLineLayer.Base;
        case 1:
          return NoteLineLayer.Upper;
        case 2:
          return NoteLineLayer.Top;
        default:
          return NoteLineLayer.Base;
      }
    }
  }

  public class RotationEventConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapEventData, BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData, SpawnRotationBeatmapEventData>
  {
    public RotationEventConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override SpawnRotationBeatmapEventData Convert(
      BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData rotationEventSaveData)
    {
      return new SpawnRotationBeatmapEventData(this.BeatToTime(rotationEventSaveData.beat), BeatmapDataLoader.RotationEventConvertor.SpawnRotationEventType(rotationEventSaveData.executionTime), rotationEventSaveData.rotation);
    }

    private static SpawnRotationBeatmapEventData.SpawnRotationEventType SpawnRotationEventType(
      BeatmapSaveDataVersion3.BeatmapSaveData.ExecutionTime executionTime)
    {
      return executionTime == BeatmapSaveDataVersion3.BeatmapSaveData.ExecutionTime.Early ? SpawnRotationBeatmapEventData.SpawnRotationEventType.Early : SpawnRotationBeatmapEventData.SpawnRotationEventType.Late;
    }
  }

  public class SliderConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapObjectData, BeatmapSaveDataVersion3.BeatmapSaveData.SliderData, SliderData>
  {
    public SliderConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override SliderData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.SliderData sliderSaveData) => SliderData.CreateSliderData(BeatmapDataLoader.ConvertColorType(sliderSaveData.colorType), this.BeatToTime(sliderSaveData.beat), sliderSaveData.headLine, BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.headLayer), BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.headLayer), sliderSaveData.headControlPointLengthMultiplier, sliderSaveData.headCutDirection, this.BeatToTime(sliderSaveData.tailBeat), sliderSaveData.tailLine, BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.tailLayer), BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.tailLayer), sliderSaveData.tailControlPointLengthMultiplier, sliderSaveData.tailCutDirection, sliderSaveData.sliderMidAnchorMode);
  }

  public class BurstSliderConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapObjectData, BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData, SliderData>
  {
    public BurstSliderConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override SliderData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData sliderSaveData) => SliderData.CreateBurstSliderData(BeatmapDataLoader.ConvertColorType(sliderSaveData.colorType), this.BeatToTime(sliderSaveData.beat), sliderSaveData.headLine, BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.headLayer), BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.headLayer), sliderSaveData.headCutDirection, this.BeatToTime(sliderSaveData.tailBeat), sliderSaveData.tailLine, BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.tailLayer), BeatmapDataLoader.ConvertNoteLineLayer(sliderSaveData.tailLayer), NoteCutDirection.Any, sliderSaveData.sliceCount, sliderSaveData.squishAmount);
  }

  public class WaypointConvertor : 
    BeatmapDataLoader.BeatmapDataItemConvertor<BeatmapObjectData, BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData, WaypointData>
  {
    public WaypointConvertor(
      BeatmapDataLoader.BpmTimeProcessor bpmTimeProcessor)
      : base(bpmTimeProcessor)
    {
    }

    protected override WaypointData Convert(BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData waypointSaveData) => new WaypointData(this.BeatToTime(waypointSaveData.beat), waypointSaveData.line, BeatmapDataLoader.ConvertNoteLineLayer(waypointSaveData.layer), waypointSaveData.offsetDirection);
  }

  public class SpecialEventsFilter
  {
    protected readonly HashSet<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType> _eventTypesToFilter;

    public SpecialEventsFilter(
      BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords basicEventTypesWithKeywords,
      EnvironmentKeywords environmentKeywords)
    {
      this._eventTypesToFilter = BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.GetSpecialEventTypes();
      foreach (BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword eventTypesForKeyword in basicEventTypesWithKeywords.data)
      {
        if (environmentKeywords.HasKeyword(eventTypesForKeyword.keyword))
        {
          foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType eventType in eventTypesForKeyword.eventTypes)
            this._eventTypesToFilter.Remove(eventType);
        }
      }
    }

    public virtual bool IsEventValid(
      BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType basicBeatmapEventType)
    {
      return !this._eventTypesToFilter.Contains(basicBeatmapEventType);
    }
  }
}
