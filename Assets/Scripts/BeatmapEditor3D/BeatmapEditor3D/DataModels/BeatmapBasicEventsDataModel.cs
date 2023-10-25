// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapBasicEventsDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using IntervalTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapBasicEventsDataModel : IBeatmapEventsDataModel, IReadonlyBeatmapEventsDataModel
  {
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EnvironmentDefinitionsListSO _environmentDefinitionsList;
    [Inject]
    private readonly DataTransformerProvider _dataTransformerProvider;
    private readonly IDictionary<BasicBeatmapEventType, IIntervalTree<float, BasicEventEditorData>> _eventsTreeByType = (IDictionary<BasicBeatmapEventType, IIntervalTree<float, BasicEventEditorData>>) new Dictionary<BasicBeatmapEventType, IIntervalTree<float, BasicEventEditorData>>();
    private readonly IDictionary<BeatmapEditorObjectId, BasicEventEditorData> _eventsDict = (IDictionary<BeatmapEditorObjectId, BasicEventEditorData>) new Dictionary<BeatmapEditorObjectId, BasicEventEditorData>();
    private readonly List<BasicEventTypesForKeywordEditorData> _basicEventTypesForKeywordData = new List<BasicEventTypesForKeywordEditorData>();
    private bool _useNormalEventsAsCompatibleEvents;
    private int _eventsCount;
    private bool _isDirty;

    public bool isDirty => this._isDirty;

    public event Action<BasicEventEditorData> didAddBasicEvent;

    public event Action<BasicEventEditorData> didRemoveBasicEvent;

    public LightEventsVersion lightEventsVersion { get; private set; }

    public int Count() => this._eventsCount;

    public void UpdateWith(
      List<BasicEventEditorData> events,
      List<BasicEventTypesForKeywordEditorData> basicEventTypesForKeywordEditorData,
      bool useNormalEventsAsCompatibleEvents)
    {
      this.UpdateBasicEvents(events);
      this.UpdateEventsCount();
      this.UpdateLightEventsVersion();
      foreach (BasicEventTypesForKeywordEditorData keywordEditorData in basicEventTypesForKeywordEditorData)
        this._basicEventTypesForKeywordData.Add(keywordEditorData);
      this._useNormalEventsAsCompatibleEvents = useNormalEventsAsCompatibleEvents;
    }

    public void ClearDirty() => this._isDirty = false;

    public void Clear()
    {
      this._basicEventTypesForKeywordData.Clear();
      this._eventsDict.Clear();
      this._eventsTreeByType.Clear();
    }

    public List<BasicEventEditorData> GetAllEventsAsList()
    {
      EnvironmentTracksDefinitionSO environmentDefinitions = this._environmentDefinitionsList[this._beatmapDataModel.environment];
      List<BasicEventEditorData> allEventsAsList = new List<BasicEventEditorData>();
      if ((UnityEngine.Object) environmentDefinitions == (UnityEngine.Object) null)
        return allEventsAsList;
      foreach (BasicBeatmapEventType key in (IEnumerable<BasicBeatmapEventType>) this._eventsTreeByType.Keys)
      {
        switch (key)
        {
          case BasicBeatmapEventType.Event5:
          case BasicBeatmapEventType.Event14:
          case BasicBeatmapEventType.Event15:
            continue;
          default:
            EventTrackDefinitionSO trackDefinition = environmentDefinitions[key].trackDefinition;
            if (!((UnityEngine.Object) trackDefinition == (UnityEngine.Object) null))
            {
              IEventDataTransformer dataTransformer = this._dataTransformerProvider.GetDataTransformer(trackDefinition.dataTransformation);
              if (dataTransformer != null)
              {
                List<BasicEventEditorData> collection = dataTransformer.TransformFrom(this.GetAllDataIn(key).ToList<BasicEventEditorData>());
                allEventsAsList.InsertRange(0, (IEnumerable<BasicEventEditorData>) collection);
                continue;
              }
              continue;
            }
            continue;
        }
      }
      EventEditorDataComparer<BasicEventEditorData> comparer = new EventEditorDataComparer<BasicEventEditorData>();
      allEventsAsList.Sort((Comparison<BasicEventEditorData>) ((a, b) => comparer.Compare((object) a, (object) b)));
      return allEventsAsList;
    }

    public IEnumerable<BasicEventEditorData> GetAllDataIn(BasicBeatmapEventType eventType) => !this._eventsTreeByType.ContainsKey(eventType) ? (IEnumerable<BasicEventEditorData>) new List<BasicEventEditorData>() : this._eventsTreeByType[eventType].Values;

    public List<BasicEventEditorData> GetBasicEventsInterval(
      BasicBeatmapEventType eventType,
      float from,
      float to)
    {
      return !this._eventsTreeByType.ContainsKey(eventType) ? new List<BasicEventEditorData>() : this._eventsTreeByType[eventType].QueryWithCount(from, to);
    }

    public BasicEventEditorData GetBasicEventAt(BasicBeatmapEventType eventType, float beat) => !this._eventsTreeByType.ContainsKey(eventType) ? (BasicEventEditorData) null : this._eventsTreeByType[eventType].QueryWithCount(beat - 0.00190624991f, beat + 0.00190624991f).FirstOrDefault<BasicEventEditorData>((Func<BasicEventEditorData, bool>) (evt => evt.IsEventValidAtBeat(beat)));

    public BasicEventEditorData GetBasicEventById(BeatmapEditorObjectId id)
    {
      BasicEventEditorData basicEventEditorData;
      return !this._eventsDict.TryGetValue(id, out basicEventEditorData) ? (BasicEventEditorData) null : basicEventEditorData;
    }

    public int GetSpawnRotationEventFor(float beat)
    {
      List<BasicEventEditorData> basicEventsInterval = this.GetBasicEventsInterval(BasicBeatmapEventType.Event15, 0.0f, beat - 0.00190624991f);
      int rotationEventFor = 0;
      float num = 0.0f;
      foreach (BasicEventEditorData basicEventEditorData in basicEventsInterval)
      {
        if ((double) num < (double) basicEventEditorData.beat)
        {
          num = basicEventEditorData.beat;
          rotationEventFor = basicEventEditorData.value;
        }
      }
      return rotationEventFor;
    }

    public IEnumerable<BasicEventTypesForKeywordEditorData> GetBasicEventTypesForKeywordData() => (IEnumerable<BasicEventTypesForKeywordEditorData>) this._basicEventTypesForKeywordData;

    public bool GetUseNormalEventsAsCompatibleEvents() => this._useNormalEventsAsCompatibleEvents;

    public void Insert(BasicEventEditorData basicEventToAdd)
    {
      IEventDataTransformer eventDataTransformer = this.GetBasicEventDataTransformer(basicEventToAdd.type);
      if (!(this.GetBasicEventAt(basicEventToAdd.type, basicEventToAdd.beat) == (BasicEventEditorData) null))
        return;
      IEnumerable<BasicEventEditorData> allDataIn = this.GetAllDataIn(basicEventToAdd.type);
      List<BasicEventEditorData> events = (allDataIn != null ? allDataIn.ToList<BasicEventEditorData>() : (List<BasicEventEditorData>) null) ?? new List<BasicEventEditorData>();
      int index1 = events.FindIndex((Predicate<BasicEventEditorData>) (data => (double) data.beat >= (double) basicEventToAdd.beat));
      int index2 = index1 == -1 ? events.Count : Mathf.Clamp(index1, 0, events.Count);
      events.Insert(index2, basicEventToAdd);
      this.ClearBasicEventsData(basicEventToAdd.type);
      this.AddBasicEventsData(basicEventToAdd.type, (IEnumerable<BasicEventEditorData>) eventDataTransformer.TransformTo(events, this._beatmapDataModel.bpmData));
      Action<BasicEventEditorData> didAddBasicEvent = this.didAddBasicEvent;
      if (didAddBasicEvent != null)
        didAddBasicEvent(basicEventToAdd);
      this.UpdateEventsCount();
      this._isDirty = true;
    }

    public void Insert(IEnumerable<BasicEventEditorData> eventsToAdd)
    {
      foreach (BasicEventEditorData basicEventToAdd in eventsToAdd)
        this.Insert(basicEventToAdd);
    }

    public void Remove(BasicEventEditorData basicEventToRemove)
    {
      BasicEventEditorData basicEventEditorData = this._eventsDict[basicEventToRemove.id];
      IEnumerable<BasicEventEditorData> allDataIn = this.GetAllDataIn(basicEventEditorData.type);
      List<BasicEventEditorData> events = (allDataIn != null ? allDataIn.ToList<BasicEventEditorData>() : (List<BasicEventEditorData>) null) ?? new List<BasicEventEditorData>();
      events.Remove(basicEventEditorData);
      this._eventsDict.Remove(basicEventEditorData.id);
      IEventDataTransformer eventDataTransformer = this.GetBasicEventDataTransformer(basicEventEditorData.type);
      this.ClearBasicEventsData(basicEventEditorData.type);
      this.AddBasicEventsData(basicEventEditorData.type, (IEnumerable<BasicEventEditorData>) eventDataTransformer.TransformTo(events, this._beatmapDataModel.bpmData));
      Action<BasicEventEditorData> removeBasicEvent = this.didRemoveBasicEvent;
      if (removeBasicEvent != null)
        removeBasicEvent(basicEventEditorData);
      this.UpdateEventsCount();
      this._isDirty = true;
    }

    public void Remove(IEnumerable<BasicEventEditorData> eventsToRemove)
    {
      foreach (BasicEventEditorData basicEventToRemove in eventsToRemove)
        this.Remove(basicEventToRemove);
    }

    private IIntervalTree<float, BasicEventEditorData> GetBasicEventTree(BasicBeatmapEventType type)
    {
      if (!this._eventsTreeByType.ContainsKey(type))
        this._eventsTreeByType[type] = (IIntervalTree<float, BasicEventEditorData>) new IntervalTree.IntervalTree<float, BasicEventEditorData>();
      return this._eventsTreeByType[type];
    }

    private void AddBasicEventsData(
      BasicBeatmapEventType type,
      IEnumerable<BasicEventEditorData> eventsData)
    {
      IIntervalTree<float, BasicEventEditorData> basicEventTree = this.GetBasicEventTree(type);
      foreach (BasicEventEditorData basicEventEditorData in eventsData)
      {
        basicEventTree.Add(basicEventEditorData.beat, basicEventEditorData.beat, basicEventEditorData);
        this._eventsDict.Add(basicEventEditorData.id, basicEventEditorData);
      }
    }

    private void ClearBasicEventsData(BasicBeatmapEventType type)
    {
      IIntervalTree<float, BasicEventEditorData> intervalTree;
      if (!this._eventsTreeByType.TryGetValue(type, out intervalTree))
        return;
      foreach (BaseEditorData baseEditorData in intervalTree.Values)
        this._eventsDict.Remove(baseEditorData.id);
      intervalTree.Clear();
    }

    private void UpdateEventsCount() => this._eventsCount = this._eventsDict.Count;

    private IEventDataTransformer GetBasicEventDataTransformer(BasicBeatmapEventType type) => this._dataTransformerProvider.GetDataTransformer(this._beatmapDataModel.environmentTrackDefinition[type].trackDefinition.dataTransformation);

    private void UpdateLightEventsVersion()
    {
      bool flag = false;
      foreach (KeyValuePair<BasicBeatmapEventType, IIntervalTree<float, BasicEventEditorData>> keyValuePair in (IEnumerable<KeyValuePair<BasicBeatmapEventType, IIntervalTree<float, BasicEventEditorData>>>) this._eventsTreeByType)
      {
        BasicBeatmapEventType key = keyValuePair.Key;
        IIntervalTree<float, BasicEventEditorData> intervalTree = keyValuePair.Value;
        if (this._beatmapDataModel.environmentTrackDefinition[key].trackToolbarType == TrackToolbarType.Lights)
          flag |= intervalTree.Values.Any<BasicEventEditorData>(new Func<BasicEventEditorData, bool>(IsV1LightEvt));
      }
      this.lightEventsVersion = flag ? LightEventsVersion.Version1 : LightEventsVersion.Version2;

      bool IsV1LightEvt(BasicEventEditorData evt) => evt.value == 2 || evt.value == 6 || evt.value == 10 || evt.value == 3 || evt.value == 7 || evt.value == 11;
    }

    private void UpdateBasicEvents(List<BasicEventEditorData> events)
    {
      EnvironmentTracksDefinitionSO environmentDefinitions = this._environmentDefinitionsList[this._beatmapDataModel.environment];
      if ((UnityEngine.Object) environmentDefinitions == (UnityEngine.Object) null)
        return;
      Dictionary<BasicBeatmapEventType, List<BasicEventEditorData>> dictionary = new Dictionary<BasicBeatmapEventType, List<BasicEventEditorData>>();
      foreach (BasicEventEditorData basicEventEditorData in events)
      {
        if (basicEventEditorData.type != BasicBeatmapEventType.BpmChange)
        {
          if (!dictionary.ContainsKey(basicEventEditorData.type))
            dictionary[basicEventEditorData.type] = new List<BasicEventEditorData>();
          dictionary[basicEventEditorData.type].Add(basicEventEditorData);
        }
      }
      foreach (KeyValuePair<BasicBeatmapEventType, List<BasicEventEditorData>> keyValuePair in dictionary)
      {
        EventTrackDefinitionSO trackDefinition = environmentDefinitions[keyValuePair.Key]?.trackDefinition;
        if (!((UnityEngine.Object) trackDefinition == (UnityEngine.Object) null))
        {
          IEventDataTransformer dataTransformer = this._dataTransformerProvider.GetDataTransformer(trackDefinition.dataTransformation);
          if (dataTransformer != null)
          {
            List<BasicEventEditorData> eventsData = dataTransformer.TransformTo(keyValuePair.Value, this._beatmapDataModel.bpmData);
            this.AddBasicEventsData(keyValuePair.Key, (IEnumerable<BasicEventEditorData>) eventsData);
          }
        }
      }
    }
  }
}
