// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SerializedData.BeatmapLevelDataModelSaver
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.SerializedData
{
  public class BeatmapLevelDataModelSaver
  {
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;

    public bool NeedsSaving() => this._beatmapLevelDataModel.isDirty || this._beatmapEventsDataModel.isDirty || this._beatmapEventBoxGroupsDataModel.isDirty;

    public BeatmapSaveDataVersion3.BeatmapSaveData Save()
    {
      List<BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData> list1 = this._beatmapDataModel.bpmData.regions.Select<BpmRegion, BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData>(new Func<BpmRegion, BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData>(BeatmapLevelDataModelSaver.CreateBpmChangeEventSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData> list2 = new List<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>().Concat<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(this._beatmapEventsDataModel.GetAllDataIn(BasicBeatmapEventType.Event14).Select<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(new Func<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(BeatmapLevelDataModelSaver.CreateRotationEventSaveData))).Concat<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(this._beatmapEventsDataModel.GetAllDataIn(BasicBeatmapEventType.Event15).Select<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(new Func<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(BeatmapLevelDataModelSaver.CreateRotationEventSaveData))).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData> list3 = this._beatmapEventsDataModel.GetAllEventsAsList().Select<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData>(new Func<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData>(BeatmapLevelDataModelSaver.CreateBasicEventSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData> list4 = this._beatmapEventsDataModel.GetAllDataIn(BasicBeatmapEventType.Event5).Select<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData>(new Func<BasicEventEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData>(BeatmapLevelDataModelSaver.CreateColorBoostSaveEventData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData> list5 = new List<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>().Concat<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(this._beatmapLevelDataModel.notes.Where<NoteEditorData>((Func<NoteEditorData, bool>) (note => note.noteType == BeatmapEditor3D.Types.NoteType.Note)).Select<NoteEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(new Func<NoteEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(BeatmapLevelDataModelSaver.CreateColorNoteSaveData))).Concat<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(this._beatmapLevelDataModel.chains.Select<ChainEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(new Func<ChainEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(BeatmapLevelDataModelSaver.CreateColorNoteSaveDataFromChains))).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData> list6 = this._beatmapLevelDataModel.notes.Where<NoteEditorData>((Func<NoteEditorData, bool>) (note => note.noteType == BeatmapEditor3D.Types.NoteType.Bomb)).Select<NoteEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData>(new Func<NoteEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData>(BeatmapLevelDataModelSaver.CreateBombSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData> list7 = this._beatmapLevelDataModel.obstacles.Select<ObstacleEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData>(new Func<ObstacleEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData>(BeatmapLevelDataModelSaver.CreateObstacleSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.SliderData> list8 = this._beatmapLevelDataModel.arcs.Select<ArcEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.SliderData>(new Func<ArcEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.SliderData>(BeatmapLevelDataModelSaver.CreateSliderSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.SliderData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData> list9 = this._beatmapLevelDataModel.chains.Select<ChainEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData>(new Func<ChainEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData>(BeatmapLevelDataModelSaver.CreateBurstSliderSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData>();
      List<BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData> list10 = this._beatmapLevelDataModel.waypoints.Select<WaypointEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData>(new Func<WaypointEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData>(BeatmapLevelDataModelSaver.CreateWaypointSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData>();
      list1.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData>(BeatmapLevelDataModelSaver.SortByBeat));
      list2.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData>(BeatmapLevelDataModelSaver.SortByRotationTypeAndBeat));
      list3.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData>(BeatmapLevelDataModelSaver.SortByEventTypeAndBeat));
      list4.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData>(BeatmapLevelDataModelSaver.SortByBeat));
      list5.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData>(BeatmapLevelDataModelSaver.SortByBeat));
      list6.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData>(BeatmapLevelDataModelSaver.SortByBeat));
      list10.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData>(BeatmapLevelDataModelSaver.SortByBeat));
      list7.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData>(BeatmapLevelDataModelSaver.SortByBeat));
      list8.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.SliderData>(BeatmapLevelDataModelSaver.SortByBeat));
      list9.Sort(new Comparison<BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData>(BeatmapLevelDataModelSaver.SortByBeat));
      (List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup> lightColorEventBoxGroups, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup> lightRotationEventBoxGroups, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup> lightTranslationEventBoxGroups) = this._beatmapEventBoxGroupsDataModel.GetAllEventBoxGroupsAsList().OrderBy<EventBoxGroupEditorData, float>((Func<EventBoxGroupEditorData, float>) (e => e.beat)).Select<EventBoxGroupEditorData, List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>>(new Func<EventBoxGroupEditorData, List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>>(this.CreateEventBoxGroupSaveData)).Aggregate<List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>, (List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup>)>((new List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup>(), new List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup>(), new List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup>()), new Func<(List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup>), List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>, (List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup>)>(BeatmapLevelDataModelSaver.SplitEventBoxGroupsSaveData));
      BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords basicEventTypesWithKeywords = new BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords(this._beatmapEventsDataModel.GetBasicEventTypesForKeywordData().Select<BasicEventTypesForKeywordEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword>(new Func<BasicEventTypesForKeywordEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword>(this.CreateBasicEventTypesForKeywordSaveData)).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword>());
      return new BeatmapSaveDataVersion3.BeatmapSaveData(list1, list2, list5, list6, list7, list8, list9, list10, list3, list4, lightColorEventBoxGroups, lightRotationEventBoxGroups, lightTranslationEventBoxGroups, basicEventTypesWithKeywords, this._beatmapEventsDataModel.GetUseNormalEventsAsCompatibleEvents());
    }

    private BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword CreateBasicEventTypesForKeywordSaveData(
      BasicEventTypesForKeywordEditorData data)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword(data.keyword, data.eventTypes.Select<BasicBeatmapEventType, BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType>((Func<BasicBeatmapEventType, BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType>) (e => (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType) e)).ToList<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType>());
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.SliderData CreateSliderSaveData(
      ArcEditorData a)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.SliderData(a.colorType == ColorType.ColorA ? BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA : BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorB, a.beat, a.column, a.row, a.controlPointLengthMultiplier, a.cutDirection, a.tailBeat, a.tailColumn, a.tailRow, a.tailControlPointLengthMultiplier, a.tailCutDirection, a.midAnchorMode);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData CreateBurstSliderSaveData(
      ChainEditorData c)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData(c.colorType == ColorType.ColorA ? BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA : BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorB, c.beat, c.column, c.row, c.cutDirection, c.tailBeat, c.tailColumn, c.tailRow, c.sliceCount, c.squishAmount);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData CreateBasicEventSaveData(
      BasicEventEditorData e)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData(e.beat, (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType) e.type, e.value, e.floatValue);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData CreateColorNoteSaveData(
      NoteEditorData n)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData(n.beat, n.column, n.row, n.type == ColorType.ColorA ? BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA : BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorB, n.cutDirection, n.angle);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData CreateColorNoteSaveDataFromChains(
      ChainEditorData c)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData(c.beat, c.column, c.row, c.colorType == ColorType.ColorA ? BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA : BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorB, c.cutDirection, 0);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData CreateBombSaveData(
      NoteEditorData n)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData(n.beat, n.column, n.row);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData CreateWaypointSaveData(
      WaypointEditorData w)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData(w.beat, w.column, w.row, w.offsetDirection);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData CreateObstacleSaveData(
      ObstacleEditorData o)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData(o.beat, o.column, o.row, o.duration, o.width, o.height + 2);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData CreateBpmChangeEventSaveData(
      BpmRegion r)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.BpmChangeEventData(r.startBeat, r.bpm);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData CreateRotationEventSaveData(
      BasicEventEditorData e)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData(e.beat, e.type == BasicBeatmapEventType.Event14 ? BeatmapSaveDataVersion3.BeatmapSaveData.ExecutionTime.Early : BeatmapSaveDataVersion3.BeatmapSaveData.ExecutionTime.Late, (float) e.value);
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData CreateColorBoostSaveEventData(
      BasicEventEditorData e)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData(e.beat, e.value == 1);
    }

    private static int SortByBeat(
      BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem itemA,
      BeatmapSaveDataVersion3.BeatmapSaveData.BeatmapSaveDataItem itemB)
    {
      return itemA.beat.CompareTo(itemB.beat);
    }

    private static int SortByEventTypeAndBeat(
      BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData itemA,
      BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData itemB)
    {
      int num = itemA.beat.CompareTo(itemB.beat);
      return num != 0 ? num : itemA.eventType.CompareTo((object) itemB.eventType);
    }

    private static int SortByRotationTypeAndBeat(
      BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData itemA,
      BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData itemB)
    {
      int num = itemA.beat.CompareTo(itemB.beat);
      return num != 0 ? num : itemA.executionTime.CompareTo((object) itemB.executionTime);
    }

    private List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup> CreateEventBoxGroupSaveData(
      EventBoxGroupEditorData e)
    {
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(e.id);
      List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup> boxGroupSaveData = new List<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup>();
      switch (e.type)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Color:
          BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup colorEventBoxGroup = new BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup(e.beat, e.groupId, byEventBoxGroupId.Select<EventBoxEditorData, LightColorEventBoxEditorData>((Func<EventBoxEditorData, LightColorEventBoxEditorData>) (eventBox => (LightColorEventBoxEditorData) eventBox)).Select<LightColorEventBoxEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox>((Func<LightColorEventBoxEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox>) (eventBox => new BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox(BeatmapLevelDataModelSaver.CreateIndexFilter(eventBox.indexFilter), eventBox.beatDistributionParam, (BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType) eventBox.beatDistributionParamType, eventBox.brightnessDistributionParam, eventBox.brightnessDistributionShouldAffectFirstBaseEvent, (BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType) eventBox.brightnessDistributionParamType, BeatmapLevelDataModelSaver.ConvertEaseType(eventBox.brightnessDistributionEaseType), this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightColorBaseEditorData>(eventBox.id).OrderBy<LightColorBaseEditorData, float>((Func<LightColorBaseEditorData, float>) (i => i.beat)).Select<LightColorBaseEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>((Func<LightColorBaseEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>) (data => new BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData(data.beat, (BeatmapSaveDataVersion3.BeatmapSaveData.TransitionType) data.transitionType, (BeatmapSaveDataVersion3.BeatmapSaveData.EnvironmentColorType) data.colorType, data.brightness, data.strobeBeatFrequency))).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>()))).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox>());
          boxGroupSaveData.Add((BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup) colorEventBoxGroup);
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup rotationEventBoxGroup = new BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup(e.beat, e.groupId, byEventBoxGroupId.Select<EventBoxEditorData, LightRotationEventBoxEditorData>((Func<EventBoxEditorData, LightRotationEventBoxEditorData>) (eventBox => (LightRotationEventBoxEditorData) eventBox)).Select<LightRotationEventBoxEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox>((Func<LightRotationEventBoxEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox>) (eventBox => new BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox(BeatmapLevelDataModelSaver.CreateIndexFilter(eventBox.indexFilter), eventBox.beatDistributionParam, (BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType) eventBox.beatDistributionParamType, eventBox.rotationDistributionParam, (BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType) eventBox.rotationDistributionParamType, eventBox.rotationDistributionShouldAffectFirstBaseEvent, BeatmapLevelDataModelSaver.ConvertEaseType(eventBox.rotationDistributionEaseType), (BeatmapSaveDataVersion3.BeatmapSaveData.Axis) eventBox.axis, eventBox.flipRotation, this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightRotationBaseEditorData>(eventBox.id).OrderBy<LightRotationBaseEditorData, float>((Func<LightRotationBaseEditorData, float>) (i => i.beat)).Select<LightRotationBaseEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>((Func<LightRotationBaseEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>) (data =>
          {
            double beat = (double) data.beat;
            BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType = BeatmapLevelDataModelSaver.ConvertEaseType(data.easeType);
            int loopsCount1 = data.loopsCount;
            float rotation1 = data.rotation;
            int num1 = data.usePreviousEventRotationValue ? 1 : 0;
            int num2 = (int) easeType;
            int loopsCount2 = loopsCount1;
            double rotation2 = (double) rotation1;
            int rotationDirection = (int) data.rotationDirection;
            return new BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData((float) beat, num1 != 0, (BeatmapSaveDataVersion3.BeatmapSaveData.EaseType) num2, loopsCount2, (float) rotation2, (BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData.RotationDirection) rotationDirection);
          })).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>()))).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox>());
          boxGroupSaveData.Add((BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup) rotationEventBoxGroup);
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup translationEventBoxGroup = new BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup(e.beat, e.groupId, byEventBoxGroupId.Select<EventBoxEditorData, LightTranslationEventBoxEditorData>((Func<EventBoxEditorData, LightTranslationEventBoxEditorData>) (eventBox => (LightTranslationEventBoxEditorData) eventBox)).Select<LightTranslationEventBoxEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox>((Func<LightTranslationEventBoxEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox>) (eventBox => new BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox(BeatmapLevelDataModelSaver.CreateIndexFilter(eventBox.indexFilter), eventBox.beatDistributionParam, (BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType) eventBox.beatDistributionParamType, eventBox.gapDistributionParam, (BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType) eventBox.gapDistributionParamType, eventBox.gapDistributionShouldAffectFirstBaseEvent, BeatmapLevelDataModelSaver.ConvertEaseType(eventBox.gapDistributionEaseType), (BeatmapSaveDataVersion3.BeatmapSaveData.Axis) eventBox.axis, eventBox.flipTranslation, this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightTranslationBaseEditorData>(eventBox.id).OrderBy<LightTranslationBaseEditorData, float>((Func<LightTranslationBaseEditorData, float>) (i => i.beat)).Select<LightTranslationBaseEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>((Func<LightTranslationBaseEditorData, BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>) (data =>
          {
            double beat = (double) data.beat;
            BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType = BeatmapLevelDataModelSaver.ConvertEaseType(data.easeType);
            float translation1 = data.translation;
            int num3 = data.usePreviousEventTranslationValue ? 1 : 0;
            int num4 = (int) easeType;
            double translation2 = (double) translation1;
            return new BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData((float) beat, num3 != 0, (BeatmapSaveDataVersion3.BeatmapSaveData.EaseType) num4, (float) translation2);
          })).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>()))).ToList<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox>());
          boxGroupSaveData.Add((BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup) translationEventBoxGroup);
          break;
      }
      return boxGroupSaveData;
    }

    public static BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter CreateIndexFilter(
      IndexFilterEditorData f)
    {
      return new BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter((BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter.IndexFilterType) f.type, f.param0, f.param1, f.reversed, (BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilterRandomType) f.randomType, f.seed, f.chunks, f.limit, (BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilterLimitAlsoAffectsType) f.limitAlsoAffectType);
    }

    private static (List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup>, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup>) SplitEventBoxGroupsSaveData(
      (List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup> colors, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup> rotations, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup> translations) acc,
      IEnumerable<BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup> eventBoxGroups)
    {
      foreach (BeatmapSaveDataVersion3.BeatmapSaveData.EventBoxGroup eventBoxGroup in eventBoxGroups)
      {
        switch (eventBoxGroup)
        {
          case BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup colorEventBoxGroup:
            acc.colors.Add(colorEventBoxGroup);
            continue;
          case BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup rotationEventBoxGroup:
            acc.rotations.Add(rotationEventBoxGroup);
            continue;
          case BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup translationEventBoxGroup:
            acc.translations.Add(translationEventBoxGroup);
            continue;
          default:
            continue;
        }
      }
      return acc;
    }

    private static BeatmapSaveDataVersion3.BeatmapSaveData.EaseType ConvertEaseType(
      EaseType easeType)
    {
      switch (easeType)
      {
        case EaseType.None:
          return BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None;
        case EaseType.InQuad:
          return BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InQuad;
        case EaseType.OutQuad:
          return BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.OutQuad;
        case EaseType.InOutQuad:
          return BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InOutQuad;
        default:
          return BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.Linear;
      }
    }
  }
}
