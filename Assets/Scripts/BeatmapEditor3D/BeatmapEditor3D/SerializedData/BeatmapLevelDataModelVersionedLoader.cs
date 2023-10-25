// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SerializedData.BeatmapLevelDataModelVersionedLoader
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
  public class BeatmapLevelDataModelVersionedLoader
  {
    [Inject]
    private readonly IBeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    private readonly Version _version400 = new Version(4, 0, 0);
    private readonly Version _version300 = new Version(3, 0, 0);
    private readonly Version _version200 = new Version(2, 0, 0);

    public void LoadToDataModel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      string projectPath,
      string filename)
    {
      Version versionedJsonVersion = BeatmapProjectFileHelper.GetVersionedJSONVersion(projectPath, filename);
      if (versionedJsonVersion < this._version200)
        this.Load_v1();
      else if (versionedJsonVersion < this._version300)
        this.Load_v2(beatmapCharacteristic, beatmapDifficulty, projectPath, filename);
      else
        this.Load_v3(beatmapCharacteristic, beatmapDifficulty, projectPath, filename);
    }

    public void LoadRawData(
      string projectPath,
      string filename,
      out List<NoteEditorData> notes,
      out List<WaypointEditorData> waypoints,
      out List<ObstacleEditorData> obstacles,
      out List<ArcEditorData> arcs,
      out List<ChainEditorData> chains,
      out List<BasicEventEditorData> events,
      out List<BeatmapEditorEventBoxGroupInput> eventBoxGroups)
    {
      Version versionedJsonVersion = BeatmapProjectFileHelper.GetVersionedJSONVersion(projectPath, filename);
      List<NoteEditorData> notes1;
      List<WaypointEditorData> waypoints1;
      List<ObstacleEditorData> obstacles1;
      List<BasicEventEditorData> basicEventEditorDataList;
      List<ArcEditorData> arcEditorDataList;
      List<ChainEditorData> chains1;
      List<BeatmapEditorEventBoxGroupInput> eventBoxGroups1;
      List<BasicEventTypesForKeywordEditorData> basicEventTypesForKeywordEditorData;
      if (versionedJsonVersion < this._version200)
      {
        notes1 = new List<NoteEditorData>();
        waypoints1 = new List<WaypointEditorData>();
        obstacles1 = new List<ObstacleEditorData>();
        basicEventEditorDataList = new List<BasicEventEditorData>();
        arcEditorDataList = new List<ArcEditorData>();
        chains1 = new List<ChainEditorData>();
        eventBoxGroups1 = new List<BeatmapEditorEventBoxGroupInput>();
      }
      else if (versionedJsonVersion < this._version300)
      {
        this.Load_v2Raw(projectPath, filename, out notes1, out waypoints1, out obstacles1, out arcEditorDataList, out basicEventEditorDataList, out basicEventTypesForKeywordEditorData);
        chains1 = new List<ChainEditorData>();
        eventBoxGroups1 = new List<BeatmapEditorEventBoxGroupInput>();
      }
      else
        this.Load_v3Raw(projectPath, filename, out notes1, out waypoints1, out obstacles1, out arcEditorDataList, out chains1, out basicEventEditorDataList, out eventBoxGroups1, out basicEventTypesForKeywordEditorData, out bool _);
      notes = notes1;
      waypoints = waypoints1;
      obstacles = obstacles1;
      events = basicEventEditorDataList;
      arcs = arcEditorDataList;
      chains = chains1;
      eventBoxGroups = eventBoxGroups1;
    }

    private void Load_v3(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      string projectPath,
      string filename)
    {
      List<NoteEditorData> notes;
      List<WaypointEditorData> waypoints;
      List<ObstacleEditorData> obstacles;
      List<ArcEditorData> arcs;
      List<ChainEditorData> chains;
      List<BasicEventEditorData> basicEvents;
      List<BeatmapEditorEventBoxGroupInput> eventBoxGroups;
      List<BasicEventTypesForKeywordEditorData> basicEventTypesForKeywordEditorData;
      bool useNormalEventsAsCompatibleEvents;
      this.Load_v3Raw(projectPath, filename, out notes, out waypoints, out obstacles, out arcs, out chains, out basicEvents, out eventBoxGroups, out basicEventTypesForKeywordEditorData, out useNormalEventsAsCompatibleEvents);
      this._beatmapLevelDataModel.UpdateWith(beatmapCharacteristic, new BeatmapDifficulty?(beatmapDifficulty), notes, waypoints, obstacles, arcs, chains);
      this._beatmapEventsDataModel.UpdateWith(basicEvents, basicEventTypesForKeywordEditorData, useNormalEventsAsCompatibleEvents);
      this._beatmapEventBoxGroupsDataModel.UpdateWith(eventBoxGroups);
      this._beatmapLevelDataModel.beatmapLevelLoaded = true;
    }

    private void Load_v2(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      string projectPath,
      string filename)
    {
      List<NoteEditorData> notes;
      List<WaypointEditorData> waypoints;
      List<ObstacleEditorData> obstacles;
      List<ArcEditorData> sliders;
      List<BasicEventEditorData> events;
      List<BasicEventTypesForKeywordEditorData> basicEventTypesForKeywordEditorData;
      this.Load_v2Raw(projectPath, filename, out notes, out waypoints, out obstacles, out sliders, out events, out basicEventTypesForKeywordEditorData);
      this._beatmapLevelDataModel.UpdateWith(beatmapCharacteristic, new BeatmapDifficulty?(beatmapDifficulty), notes, waypoints, obstacles, sliders, new List<ChainEditorData>());
      this._beatmapEventsDataModel.UpdateWith(events, basicEventTypesForKeywordEditorData, false);
      this._beatmapEventBoxGroupsDataModel.UpdateWith(new List<BeatmapEditorEventBoxGroupInput>());
      this._beatmapLevelDataModel.beatmapLevelLoaded = true;
    }

    private void Load_v1() => throw new NotImplementedException("Load v1 not yet implemented");

    private void Load_v3Raw(
      string projectPath,
      string filename,
      out List<NoteEditorData> notes,
      out List<WaypointEditorData> waypoints,
      out List<ObstacleEditorData> obstacles,
      out List<ArcEditorData> arcs,
      out List<ChainEditorData> chains,
      out List<BasicEventEditorData> basicEvents,
      out List<BeatmapEditorEventBoxGroupInput> eventBoxGroups,
      out List<BasicEventTypesForKeywordEditorData> basicEventTypesForKeywordEditorData,
      out bool useNormalEventsAsCompatibleEvents)
    {
      BeatmapSaveDataVersion3.BeatmapSaveData beatmapSaveData = BeatmapProjectFileHelper.LoadBeatmapLevel<BeatmapSaveDataVersion3.BeatmapSaveData>(projectPath, filename);
      notes = new List<NoteEditorData>().Concat<NoteEditorData>(beatmapSaveData.colorNotes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData, NoteEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData, NoteEditorData>(BeatmapLevelDataModelVersionedLoader.CreateColorNoteEditorData_v3))).Concat<NoteEditorData>(beatmapSaveData.bombNotes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData, NoteEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData, NoteEditorData>(BeatmapLevelDataModelVersionedLoader.CreateBombNoteEditorData_v3))).ToList<NoteEditorData>();
      waypoints = beatmapSaveData.waypoints.Select<BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData, WaypointEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData, WaypointEditorData>(BeatmapLevelDataModelVersionedLoader.CreateWaypointEditorData_v3)).ToList<WaypointEditorData>();
      obstacles = beatmapSaveData.obstacles.Select<BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData, ObstacleEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData, ObstacleEditorData>(BeatmapLevelDataModelVersionedLoader.CreateObstacleEditorData_v3)).ToList<ObstacleEditorData>();
      arcs = beatmapSaveData.sliders.Select<BeatmapSaveDataVersion3.BeatmapSaveData.SliderData, ArcEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.SliderData, ArcEditorData>(BeatmapLevelDataModelVersionedLoader.CreateSliderEditorData_v3)).ToList<ArcEditorData>();
      chains = beatmapSaveData.burstSliders.Select<BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData, ChainEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData, ChainEditorData>(BeatmapLevelDataModelVersionedLoader.CreateBurstSliderEditorData_v3)).ToList<ChainEditorData>();
      basicEvents = new List<BasicEventEditorData>().Concat<BasicEventEditorData>(beatmapSaveData.rotationEvents.Select<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData, BasicEventEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData, BasicEventEditorData>(BeatmapLevelDataModelVersionedLoader.CreateEventEditorDataFromRotation_v3))).Concat<BasicEventEditorData>(beatmapSaveData.basicBeatmapEvents.Select<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData, BasicEventEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData, BasicEventEditorData>(BeatmapLevelDataModelVersionedLoader.CreateEventEditorData_v3))).Concat<BasicEventEditorData>(beatmapSaveData.colorBoostBeatmapEvents.Select<BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData, BasicEventEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData, BasicEventEditorData>(BeatmapLevelDataModelVersionedLoader.CreateEventEditorDataFromColorBoost_v3))).ToList<BasicEventEditorData>();
      eventBoxGroups = new List<BeatmapEditorEventBoxGroupInput>().Concat<BeatmapEditorEventBoxGroupInput>((IEnumerable<BeatmapEditorEventBoxGroupInput>) beatmapSaveData.lightColorEventBoxGroups.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup, BeatmapEditorEventBoxGroupInput>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup, BeatmapEditorEventBoxGroupInput>(BeatmapLevelDataModelVersionedLoader.CreateLightColorEventBoxGroup_v3)).OrderBy<BeatmapEditorEventBoxGroupInput, float>((Func<BeatmapEditorEventBoxGroupInput, float>) (e => e.eventBoxGroup.beat))).Concat<BeatmapEditorEventBoxGroupInput>((IEnumerable<BeatmapEditorEventBoxGroupInput>) beatmapSaveData.lightRotationEventBoxGroups.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup, BeatmapEditorEventBoxGroupInput>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup, BeatmapEditorEventBoxGroupInput>(BeatmapLevelDataModelVersionedLoader.CreateLightRotationEventBoxGroup_v3)).OrderBy<BeatmapEditorEventBoxGroupInput, float>((Func<BeatmapEditorEventBoxGroupInput, float>) (e => e.eventBoxGroup.beat))).Concat<BeatmapEditorEventBoxGroupInput>((IEnumerable<BeatmapEditorEventBoxGroupInput>) beatmapSaveData.lightTranslationEventBoxGroups.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup, BeatmapEditorEventBoxGroupInput>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup, BeatmapEditorEventBoxGroupInput>(BeatmapLevelDataModelVersionedLoader.CreateLightTranslationEventBoxGroup_v3)).OrderBy<BeatmapEditorEventBoxGroupInput, float>((Func<BeatmapEditorEventBoxGroupInput, float>) (e => e.eventBoxGroup.beat))).ToList<BeatmapEditorEventBoxGroupInput>();
      basicEventTypesForKeywordEditorData = beatmapSaveData.basicEventTypesWithKeywords.data.Select<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword, BasicEventTypesForKeywordEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword, BasicEventTypesForKeywordEditorData>(this.CreateBasicEventTypesForKeywordData_v3)).ToList<BasicEventTypesForKeywordEditorData>();
      useNormalEventsAsCompatibleEvents = beatmapSaveData.useNormalEventsAsCompatibleEvents;
    }

    private BasicEventTypesForKeywordEditorData CreateBasicEventTypesForKeywordData_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword data)
    {
      return BasicEventTypesForKeywordEditorData.CreateNew(data.keyword, (IReadOnlyList<BasicBeatmapEventType>) data.eventTypes.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType, BasicBeatmapEventType>((Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType, BasicBeatmapEventType>) (e => (BasicBeatmapEventType) e)).ToList<BasicBeatmapEventType>());
    }

    private static NoteEditorData CreateColorNoteEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.ColorNoteData n) => NoteEditorData.CreateNew(n.beat, n.line, n.layer, n.color == BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA ? ColorType.ColorA : ColorType.ColorB, BeatmapEditor3D.Types.NoteType.Note, n.cutDirection, n.angleOffset);

    private static NoteEditorData CreateBombNoteEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.BombNoteData b) => NoteEditorData.CreateNew(b.beat, b.line, b.layer, ColorType.None, BeatmapEditor3D.Types.NoteType.Bomb, NoteCutDirection.None, 0);

    private static WaypointEditorData CreateWaypointEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.WaypointData w) => WaypointEditorData.CreateNew(w.beat, w.line, w.layer, w.offsetDirection);

    private static ObstacleEditorData CreateObstacleEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.ObstacleData o) => ObstacleEditorData.CreateNew(o.beat, o.line, o.layer, o.duration, o.width, o.height - 2);

    private static ArcEditorData CreateSliderEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.SliderData s) => ArcEditorData.CreateNew(s.colorType == BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA ? ColorType.ColorA : ColorType.ColorB, s.beat, s.headLine, s.headLayer, s.headCutDirection, s.headControlPointLengthMultiplier, s.tailBeat, s.tailLine, s.tailLayer, s.tailCutDirection, s.tailControlPointLengthMultiplier, s.sliderMidAnchorMode);

    private static ChainEditorData CreateBurstSliderEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.BurstSliderData s)
    {
      ColorType colorType = s.colorType == BeatmapSaveDataVersion3.BeatmapSaveData.NoteColorType.ColorA ? ColorType.ColorA : ColorType.ColorB;
      return ChainEditorData.CreateNew(s.beat, colorType, s.headLine, s.headLayer, s.headCutDirection, s.tailBeat, s.tailLine, s.tailLayer, s.sliceCount, s.squishAmount);
    }

    private static BasicEventEditorData CreateEventEditorDataFromRotation_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.RotationEventData r)
    {
      return r.executionTime != BeatmapSaveDataVersion3.BeatmapSaveData.ExecutionTime.Early ? BasicEventEditorData.CreateNew(BasicBeatmapEventType.Event15, r.beat, 0, r.rotation) : BasicEventEditorData.CreateNew(BasicBeatmapEventType.Event14, r.beat, 0, r.rotation);
    }

    private static BasicEventEditorData CreateEventEditorData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.BasicEventData r) => BasicEventEditorData.CreateNew((BasicBeatmapEventType) r.eventType, r.beat, r.value, r.floatValue);

    private static BasicEventEditorData CreateEventEditorDataFromColorBoost_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.ColorBoostEventData r)
    {
      return BasicEventEditorData.CreateNew(BasicBeatmapEventType.Event5, r.beat, r.boost ? 1 : 0, 0.0f);
    }

    private static BeatmapEditorEventBoxGroupInput CreateLightColorEventBoxGroup_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBoxGroup e)
    {
      EventBoxGroupEditorData eventBoxGroup = EventBoxGroupEditorData.CreateNew(e.beat, e.groupId, EventBoxGroupEditorData.EventBoxGroupType.Color);
      List<EventBoxEditorData> list1 = e.eventBoxes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox, EventBoxEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox, EventBoxEditorData>(BeatmapLevelDataModelVersionedLoader.CreateLightColorEventBox_v3)).ToList<EventBoxEditorData>();
      List<List<BaseEditorData>> lightColorEventBoxesLists = e.eventBoxes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>>((Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox, List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>>) (b => b.lightColorBaseDataList)).Select<List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>, List<BaseEditorData>>((Func<List<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData>, List<BaseEditorData>>) (l => l.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData, BaseEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData, BaseEditorData>(BeatmapLevelDataModelVersionedLoader.CreateLightColorBaseData_v3)).OrderBy<BaseEditorData, float>((Func<BaseEditorData, float>) (i => i.beat)).ToList<BaseEditorData>())).ToList<List<BaseEditorData>>();
      List<EventBoxEditorData> eventBoxes = list1;
      List<(BeatmapEditorObjectId, List<BaseEditorData>)> list2 = list1.Select<EventBoxEditorData, (BeatmapEditorObjectId, List<BaseEditorData>)>((Func<EventBoxEditorData, int, (BeatmapEditorObjectId, List<BaseEditorData>)>) ((d, i) => (d.id, lightColorEventBoxesLists[i]))).ToList<(BeatmapEditorObjectId, List<BaseEditorData>)>();
      return new BeatmapEditorEventBoxGroupInput(eventBoxGroup, eventBoxes, list2);
    }

    private static EventBoxEditorData CreateLightColorEventBox_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightColorEventBox b)
    {
      return (EventBoxEditorData) LightColorEventBoxEditorData.CreateNew(BeatmapLevelDataModelVersionedLoader.CreateIndexFilter_v3(b.indexFilter), BeatmapLevelDataModelVersionedLoader.ConvertDistributionParam(b.beatDistributionParamType), b.beatDistributionParam, BeatmapLevelDataModelVersionedLoader.ConvertDistributionParam(b.brightnessDistributionParamType), b.brightnessDistributionParam, b.brightnessDistributionShouldAffectFirstBaseEvent, BeatmapLevelDataModelVersionedLoader.ConvertEaseType(b.brightnessDistributionEaseType));
    }

    private static BaseEditorData CreateLightColorBaseData_v3(BeatmapSaveDataVersion3.BeatmapSaveData.LightColorBaseData d) => (BaseEditorData) LightColorBaseEditorData.CreateNew(d.beat, d.brightness, (LightColorBaseEditorData.TransitionType) d.transitionType, (LightColorBaseEditorData.EnvironmentColorType) d.colorType, d.strobeBeatFrequency);

    private static BeatmapEditorEventBoxGroupInput CreateLightRotationEventBoxGroup_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBoxGroup e)
    {
      EventBoxGroupEditorData eventBoxGroup = EventBoxGroupEditorData.CreateNew(e.beat, e.groupId, EventBoxGroupEditorData.EventBoxGroupType.Rotation);
      List<EventBoxEditorData> list1 = e.eventBoxes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox, EventBoxEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox, EventBoxEditorData>(BeatmapLevelDataModelVersionedLoader.CreateLightRotationEventBox_v3)).ToList<EventBoxEditorData>();
      List<List<BaseEditorData>> lightRotationEventBoxesLists = e.eventBoxes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox, IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>>((Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox, IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>>) (b => b.lightRotationBaseDataList)).Select<IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>, List<BaseEditorData>>((Func<IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData>, List<BaseEditorData>>) (l => l.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData, BaseEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData, BaseEditorData>(BeatmapLevelDataModelVersionedLoader.CreateLightRotationBaseData_v3)).OrderBy<BaseEditorData, float>((Func<BaseEditorData, float>) (i => i.beat)).ToList<BaseEditorData>())).ToList<List<BaseEditorData>>();
      List<EventBoxEditorData> eventBoxes = list1;
      List<(BeatmapEditorObjectId, List<BaseEditorData>)> list2 = list1.Select<EventBoxEditorData, (BeatmapEditorObjectId, List<BaseEditorData>)>((Func<EventBoxEditorData, int, (BeatmapEditorObjectId, List<BaseEditorData>)>) ((d, i) => (d.id, lightRotationEventBoxesLists[i]))).ToList<(BeatmapEditorObjectId, List<BaseEditorData>)>();
      return new BeatmapEditorEventBoxGroupInput(eventBoxGroup, eventBoxes, list2);
    }

    private static EventBoxEditorData CreateLightRotationEventBox_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationEventBox b)
    {
      return (EventBoxEditorData) LightRotationEventBoxEditorData.CreateNew(BeatmapLevelDataModelVersionedLoader.CreateIndexFilter_v3(b.indexFilter), BeatmapLevelDataModelVersionedLoader.ConvertDistributionParam(b.beatDistributionParamType), b.beatDistributionParam, BeatmapLevelDataModelVersionedLoader.ConvertDistributionParam(b.rotationDistributionParamType), b.rotationDistributionParam, b.rotationDistributionShouldAffectFirstBaseEvent, BeatmapLevelDataModelVersionedLoader.ConvertEaseType(b.rotationDistributionEaseType), (LightAxis) b.axis, b.flipRotation);
    }

    private static BaseEditorData CreateLightRotationBaseData_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightRotationBaseData d)
    {
      return (BaseEditorData) LightRotationBaseEditorData.CreateNew(d.beat, BeatmapLevelDataModelVersionedLoader.ConvertEaseType(d.easeType), d.loopsCount, d.rotation, d.usePreviousEventRotationValue, (LightRotationDirection) d.rotationDirection);
    }

    private static BeatmapEditorEventBoxGroupInput CreateLightTranslationEventBoxGroup_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBoxGroup e)
    {
      EventBoxGroupEditorData eventBoxGroup = EventBoxGroupEditorData.CreateNew(e.beat, e.groupId, EventBoxGroupEditorData.EventBoxGroupType.Translation);
      List<EventBoxEditorData> list1 = e.eventBoxes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox, EventBoxEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox, EventBoxEditorData>(BeatmapLevelDataModelVersionedLoader.CreateLightTranslationEventBox_v3)).ToList<EventBoxEditorData>();
      List<List<BaseEditorData>> lightTranslationEventBoxesLists = e.eventBoxes.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox, IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>>((Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox, IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>>) (b => b.lightTranslationBaseDataList)).Select<IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>, List<BaseEditorData>>((Func<IReadOnlyList<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData>, List<BaseEditorData>>) (l => l.Select<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData, BaseEditorData>(new Func<BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData, BaseEditorData>(BeatmapLevelDataModelVersionedLoader.CreateLightTranslationBaseData_v3)).OrderBy<BaseEditorData, float>((Func<BaseEditorData, float>) (i => i.beat)).ToList<BaseEditorData>())).ToList<List<BaseEditorData>>();
      List<EventBoxEditorData> eventBoxes = list1;
      List<(BeatmapEditorObjectId, List<BaseEditorData>)> list2 = list1.Select<EventBoxEditorData, (BeatmapEditorObjectId, List<BaseEditorData>)>((Func<EventBoxEditorData, int, (BeatmapEditorObjectId, List<BaseEditorData>)>) ((d, i) => (d.id, lightTranslationEventBoxesLists[i]))).ToList<(BeatmapEditorObjectId, List<BaseEditorData>)>();
      return new BeatmapEditorEventBoxGroupInput(eventBoxGroup, eventBoxes, list2);
    }

    private static EventBoxEditorData CreateLightTranslationEventBox_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationEventBox b)
    {
      return (EventBoxEditorData) LightTranslationEventBoxEditorData.CreateNew(BeatmapLevelDataModelVersionedLoader.CreateIndexFilter_v3(b.indexFilter), BeatmapLevelDataModelVersionedLoader.ConvertDistributionParam(b.beatDistributionParamType), b.beatDistributionParam, BeatmapLevelDataModelVersionedLoader.ConvertDistributionParam(b.gapDistributionParamType), b.gapDistributionParam, b.gapDistributionShouldAffectFirstBaseEvent, BeatmapLevelDataModelVersionedLoader.ConvertEaseType(b.gapDistributionEaseType), (LightAxis) b.axis, b.flipTranslation);
    }

    private static BaseEditorData CreateLightTranslationBaseData_v3(
      BeatmapSaveDataVersion3.BeatmapSaveData.LightTranslationBaseData d)
    {
      return (BaseEditorData) LightTranslationBaseEditorData.CreateNew(d.beat, BeatmapLevelDataModelVersionedLoader.ConvertEaseType(d.easeType), d.translation, d.usePreviousEventTranslationValue);
    }

    private static IndexFilterEditorData CreateIndexFilter_v3(BeatmapSaveDataVersion3.BeatmapSaveData.IndexFilter f) => IndexFilterEditorData.CreateNew((IndexFilterEditorData.IndexFilterType) f.type, f.param0, f.param1, f.reversed, f.chunks, (IndexFilter.IndexFilterRandomType) f.random, f.seed, f.limit, (IndexFilter.IndexFilterLimitAlsoAffectType) f.limitAlsoAffectsType);

    private void Load_v2Raw(
      string projectPath,
      string filename,
      out List<NoteEditorData> notes,
      out List<WaypointEditorData> waypoints,
      out List<ObstacleEditorData> obstacles,
      out List<ArcEditorData> sliders,
      out List<BasicEventEditorData> events,
      out List<BasicEventTypesForKeywordEditorData> basicEventTypesForKeywordEditorData)
    {
      BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData beatmapSaveData = BeatmapProjectFileHelper.LoadBeatmapLevel<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData>(projectPath, filename);
      Version version1 = new Version(beatmapSaveData.version);
      Version version2 = new Version("2.5.0");
      notes = beatmapSaveData.notes.Where<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData>((Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData, bool>) (note => note.type != BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.GhostNote && note.type != BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.None)).Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData, NoteEditorData>(new Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData, NoteEditorData>(BeatmapLevelDataModelVersionedLoader.CreateNoteEditorData_v2)).ToList<NoteEditorData>();
      waypoints = beatmapSaveData.waypoints.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.WaypointData, WaypointEditorData>(new Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.WaypointData, WaypointEditorData>(BeatmapLevelDataModelVersionedLoader.CreateWaypointEditorData_v2)).ToList<WaypointEditorData>();
      obstacles = beatmapSaveData.obstacles.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleData, ObstacleEditorData>(new Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleData, ObstacleEditorData>(BeatmapLevelDataModelVersionedLoader.CreateObstacleEditorData_v2)).ToList<ObstacleEditorData>();
      sliders = beatmapSaveData.sliders.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SliderData, ArcEditorData>(new Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SliderData, ArcEditorData>(BeatmapLevelDataModelVersionedLoader.CreateSliderEditorData_v2)).ToList<ArcEditorData>();
      Version version3 = version2;
      if (version1.CompareTo(version3) < 0)
        BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ConvertBeatmapSaveDataPreV2_5_0(beatmapSaveData);
      events = beatmapSaveData.events.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.EventData, BasicEventEditorData>(new Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.EventData, BasicEventEditorData>(BeatmapLevelDataModelVersionedLoader.CreateEventEditorData_v2)).ToList<BasicEventEditorData>();
      if (beatmapSaveData.specialEventsKeywordFilters != null && beatmapSaveData.specialEventsKeywordFilters.keywords != null)
        basicEventTypesForKeywordEditorData = beatmapSaveData.specialEventsKeywordFilters.keywords.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SpecialEventsForKeyword, BasicEventTypesForKeywordEditorData>(new Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SpecialEventsForKeyword, BasicEventTypesForKeywordEditorData>(this.CreateBasicEventTypesForKeywordData_v2)).ToList<BasicEventTypesForKeywordEditorData>();
      else
        basicEventTypesForKeywordEditorData = new List<BasicEventTypesForKeywordEditorData>();
    }

    private static NoteEditorData CreateNoteEditorData_v2(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData data)
    {
      BeatmapEditor3D.Types.NoteType noteType = BeatmapEditor3D.Types.NoteType.None;
      ColorType type = ColorType.None;
      switch (data.type)
      {
        case BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.NoteA:
          noteType = BeatmapEditor3D.Types.NoteType.Note;
          type = ColorType.ColorA;
          break;
        case BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.NoteB:
          noteType = BeatmapEditor3D.Types.NoteType.Note;
          type = ColorType.ColorB;
          break;
        case BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.Bomb:
          noteType = BeatmapEditor3D.Types.NoteType.Bomb;
          break;
      }
      return NoteEditorData.CreateNew(data.time, data.lineIndex, (int) data.lineLayer, type, noteType, data.cutDirection, 0);
    }

    private static WaypointEditorData CreateWaypointEditorData_v2(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.WaypointData data) => WaypointEditorData.CreateNew(data.time, data.lineIndex, (int) data.lineLayer, data.offsetDirection);

    private static ObstacleEditorData CreateObstacleEditorData_v2(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleData data)
    {
      int height = data.type == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType.Top ? 1 : 3;
      int row = data.type == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType.Top ? 2 : 0;
      return ObstacleEditorData.CreateNew(data.time, data.lineIndex, row, data.duration, data.width, height);
    }

    private static ArcEditorData CreateSliderEditorData_v2(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SliderData data) => ArcEditorData.CreateNew(data.colorType == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ColorType.ColorA ? ColorType.ColorA : ColorType.ColorB, data.time, data.headLineIndex, (int) data.headLineLayer, data.headCutDirection, 1f, data.tailTime, data.tailLineIndex, (int) data.tailLineLayer, data.tailCutDirection, 1f, data.sliderMidAnchorMode);

    private static BasicEventEditorData CreateEventEditorData_v2(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.EventData data) => BasicEventEditorData.CreateNew((BasicBeatmapEventType) data.type, data.time, data.value, data.floatValue);

    private BasicEventTypesForKeywordEditorData CreateBasicEventTypesForKeywordData_v2(
      BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SpecialEventsForKeyword data)
    {
      return BasicEventTypesForKeywordEditorData.CreateNew(data.keyword, (IReadOnlyList<BasicBeatmapEventType>) data.specialEvents.Select<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType, BasicBeatmapEventType>((Func<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType, BasicBeatmapEventType>) (e => (BasicBeatmapEventType) e)).ToList<BasicBeatmapEventType>());
    }

    private static BeatmapEventDataBox.DistributionParamType ConvertDistributionParam(
      BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType distributionParamType)
    {
      return distributionParamType == BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType.Wave || distributionParamType != BeatmapSaveDataVersion3.BeatmapSaveData.EventBox.DistributionParamType.Step ? BeatmapEventDataBox.DistributionParamType.Wave : BeatmapEventDataBox.DistributionParamType.Step;
    }

    private static EaseType ConvertEaseType(BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType)
    {
      switch (easeType)
      {
        case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None:
          return EaseType.None;
        case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InQuad:
          return EaseType.InQuad;
        case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.OutQuad:
          return EaseType.OutQuad;
        case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InOutQuad:
          return EaseType.InOutQuad;
        default:
          return EaseType.Linear;
      }
    }
  }
}
