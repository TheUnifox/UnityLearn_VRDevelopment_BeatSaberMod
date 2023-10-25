// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupsClipboardHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventBoxGroupsClipboardHelper
  {
    [Inject]
    private readonly EventBoxGroupsSelectionState _eventBoxGroupsSelectionState;
    [Inject]
    private readonly EventBoxGroupsClipboardState _eventBoxGroupsClipboardState;
    [Inject]
    private readonly EventBoxesSelectionState _eventBoxesSelectionState;
    [Inject]
    private readonly EventBoxesClipboardState _eventBoxesClipboardState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;

    public void CopySelectedEventBoxGroups()
    {
      this._eventBoxGroupsClipboardState.Clear();
      this._eventBoxGroupsClipboardState.AddRange(this._eventBoxGroupsSelectionState.eventBoxGroups.Select<BeatmapEditorObjectId, EventBoxGroupEditorData>((Func<BeatmapEditorObjectId, EventBoxGroupEditorData>) (id => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(id))));
      foreach (EventBoxGroupEditorData eventBoxGroup in (IEnumerable<EventBoxGroupEditorData>) this._eventBoxGroupsClipboardState.eventBoxGroups)
      {
        List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(eventBoxGroup.id);
        foreach (EventBoxEditorData eventBoxEditorData in byEventBoxGroupId)
          this._eventBoxGroupsClipboardState.AddRange(eventBoxEditorData.id, (IEnumerable<BaseEditorData>) this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id).ToList<BaseEditorData>());
        this._eventBoxGroupsClipboardState.AddRange(eventBoxGroup.id, (IEnumerable<EventBoxEditorData>) byEventBoxGroupId.ToList<EventBoxEditorData>());
      }
      this._eventBoxGroupsClipboardState.startBeat = this._eventBoxGroupsClipboardState.eventBoxGroups.Min<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, float>) (g => g.beat));
    }

    public void GetAllDataForEventBoxGroups(
      List<EventBoxGroupEditorData> eventBoxGroups,
      out Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBoxes,
      out Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      eventBoxes = new Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>();
      baseDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();
      foreach (EventBoxGroupEditorData eventBoxGroup in eventBoxGroups)
      {
        List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(eventBoxGroup.id);
        foreach (EventBoxEditorData eventBoxEditorData in byEventBoxGroupId)
          baseDataLists[eventBoxEditorData.id] = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id).ToList<BaseEditorData>();
        eventBoxes[eventBoxGroup.id] = byEventBoxGroupId.ToList<EventBoxEditorData>();
      }
    }

    public void InsertEventBoxGroupData(
      List<EventBoxGroupEditorData> eventBoxGroups,
      Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBoxes,
      Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxGroups((IEnumerable<EventBoxGroupEditorData>) eventBoxGroups);
      foreach (KeyValuePair<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBox in eventBoxes)
        this._beatmapEventBoxGroupsDataModel.InsertEventBoxes(eventBox.Key, (IEnumerable<EventBoxEditorData>) eventBox.Value);
      foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> baseDataList in baseDataLists)
        this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(baseDataList.Key, (IEnumerable<BaseEditorData>) baseDataList.Value);
    }

    public void RemoveEventBoxGroupData(
      List<EventBoxGroupEditorData> eventBoxGroups,
      Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBoxes,
      Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> baseDataList in baseDataLists)
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(baseDataList.Key);
      foreach (KeyValuePair<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBox in eventBoxes)
        this._beatmapEventBoxGroupsDataModel.RemoveEventBoxes(eventBox.Key, (IEnumerable<EventBoxEditorData>) eventBox.Value);
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxGroups((IEnumerable<EventBoxGroupEditorData>) eventBoxGroups);
    }

    public void CopyEventBoxesWithBaseLists(
      List<EventBoxEditorData> eventBoxes,
      IReadOnlyDictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseEditorDataLists,
      List<EventBoxEditorData> newEventBoxes,
      Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> newBaseDataLists)
    {
      foreach (EventBoxEditorData eventBox in eventBoxes)
      {
        newEventBoxes.Add(EventBoxGroupsClipboardHelper.CopyEventBoxEditorDataWithoutId(eventBox));
        newBaseDataLists[newEventBoxes[newEventBoxes.Count - 1].id] = baseEditorDataLists[eventBox.id].Select<BaseEditorData, BaseEditorData>((Func<BaseEditorData, BaseEditorData>) (e => EventBoxGroupsClipboardHelper.CopyBaseEditorDataWithoutId(e))).ToList<BaseEditorData>();
      }
    }

    public void CopySelectedEventBoxEvents()
    {
      this._eventBoxesClipboardState.Clear();
      float num1 = float.MaxValue;
      int num2 = int.MaxValue;
      BeatmapEditorObjectId[] array = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id).Select<EventBoxEditorData, BeatmapEditorObjectId>((Func<EventBoxEditorData, BeatmapEditorObjectId>) (e => e.id)).ToArray<BeatmapEditorObjectId>();
      foreach ((BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId beatmapEditorObjectId) in (IEnumerable<(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId)>) this._eventBoxesSelectionState.events)
      {
        int index = ((IReadOnlyList<BeatmapEditorObjectId>) array).IndexOf<BeatmapEditorObjectId>(eventBoxId);
        (BaseEditorData, int) baseEditorDataById = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById(eventBoxId, beatmapEditorObjectId);
        if ((double) baseEditorDataById.Item1.beat < (double) num1)
          num1 = baseEditorDataById.Item1.beat;
        if (index < num2)
          num2 = index;
        this._eventBoxesClipboardState.Add(index, EventBoxGroupsClipboardHelper.CopyBaseEditorDataWithoutId(baseEditorDataById.Item1));
      }
      this._eventBoxesClipboardState.startBeat = num1;
      this._eventBoxesClipboardState.startEventBoxIndex = num2;
      this._eventBoxesClipboardState.eventBoxGroupType = this._eventBoxesSelectionState.eventBoxGroupType;
    }

    public void GetBaseDataListsFromSelection(
      out Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      baseDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();
      foreach ((BeatmapEditorObjectId beatmapEditorObjectId1, BeatmapEditorObjectId beatmapEditorObjectId2) in (IEnumerable<(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId)>) this._eventBoxesSelectionState.events)
      {
        if (!baseDataLists.ContainsKey(beatmapEditorObjectId1))
          baseDataLists[beatmapEditorObjectId1] = new List<BaseEditorData>();
        baseDataLists[beatmapEditorObjectId1].Add(this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById(beatmapEditorObjectId1, beatmapEditorObjectId2).Item1);
      }
    }

    public void InsertEventBoxesBaseLists(
      Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> baseDataList in baseDataLists)
      {
        foreach (BaseEditorData data in baseDataList.Value)
          this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataAtBeat(baseDataList.Key, data);
      }
    }

    public void RemoveEventBoxesBaseLists(
      Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> baseDataList in baseDataLists)
      {
        foreach (BaseEditorData baseEditorData in baseDataList.Value)
          this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(baseDataList.Key, baseEditorData);
      }
    }

    public bool CheckEventBoxesBaseListsCollisions(
      Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists)
    {
      return baseDataLists.Any<KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>>>((Func<KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>>, bool>) (list => list.Value.Any<BaseEditorData>((Func<BaseEditorData, bool>) (baseEvent => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataAt(list.Key, baseEvent.beat) != null))));
    }

    public static EventBoxEditorData CopyEventBoxEditorDataWithoutId(EventBoxEditorData b)
    {
      switch (b)
      {
        case LightColorEventBoxEditorData data1:
          return (EventBoxEditorData) LightColorEventBoxEditorData.CopyWithoutId(data1);
        case LightRotationEventBoxEditorData data2:
          return (EventBoxEditorData) LightRotationEventBoxEditorData.CopyWithoutId(data2);
        case LightTranslationEventBoxEditorData data3:
          return (EventBoxEditorData) LightTranslationEventBoxEditorData.CopyWithoutId(data3);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static BaseEditorData CopyBaseEditorDataWithoutId(BaseEditorData d, float? beat = null)
    {
      switch (d)
      {
        case LightColorBaseEditorData original1:
          BeatmapEditorObjectId? id1 = new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId());
          float? beat1 = beat;
          float? brightness = new float?();
          LightColorBaseEditorData.TransitionType? transitionType = new LightColorBaseEditorData.TransitionType?();
          LightColorBaseEditorData.EnvironmentColorType? colorType = new LightColorBaseEditorData.EnvironmentColorType?();
          int? strobeFrequency = new int?();
          return (BaseEditorData) LightColorBaseEditorData.CopyWithModifications(original1, id1, beat1, brightness, transitionType, colorType, strobeFrequency);
        case LightRotationBaseEditorData original2:
          BeatmapEditorObjectId? id2 = new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId());
          float? beat2 = beat;
          EaseType? easeType1 = new EaseType?();
          int? loopsCount = new int?();
          float? rotation = new float?();
          bool? usePreviousEventRotationValue = new bool?();
          LightRotationDirection? rotationDirection = new LightRotationDirection?();
          return (BaseEditorData) LightRotationBaseEditorData.CopyWithModifications(original2, id2, beat2, easeType1, loopsCount, rotation, usePreviousEventRotationValue, rotationDirection);
        case LightTranslationBaseEditorData original3:
          BeatmapEditorObjectId? id3 = new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId());
          float? beat3 = beat;
          EaseType? easeType2 = new EaseType?();
          float? translation = new float?();
          bool? usePreviousEventTranslationValue = new bool?();
          return (BaseEditorData) LightTranslationBaseEditorData.CopyWithModifications(original3, id3, beat3, easeType2, translation, usePreviousEventTranslationValue);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
