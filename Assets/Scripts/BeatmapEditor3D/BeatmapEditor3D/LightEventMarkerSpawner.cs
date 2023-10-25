// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightEventMarkerSpawner
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Visuals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class LightEventMarkerSpawner : 
    IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>
  {
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IReadonlyEventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly BasicEventMarker.Pool _eventMarkerPool;
    [Inject]
    private readonly LightEventMarkerObject.Pool _lightEventMarkerObjectPool;
    [Inject]
    private readonly DurationEventMarkerObject.Pool _durationEventMarkerObjectPool;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    private readonly List<BasicEventMarker> _eventMarkers = new List<BasicEventMarker>();
    private readonly List<LightEventMarkerObject> _eventMarkerObjects = new List<LightEventMarkerObject>();
    private readonly List<DurationEventMarkerObject> _durationEventMarkerObjects = new List<DurationEventMarkerObject>();

    public void UpdateDurationEvents()
    {
      foreach (DurationEventMarkerObject eventMarkerObject in this._durationEventMarkerObjects)
      {
        BasicEventEditorData basicEventData = eventMarkerObject.basicEventData;
        float position = this._beatmapObjectPlacementHelper.BeatToPosition(basicEventData.beat);
        eventMarkerObject.SetScaleZ(this._beatmapObjectPlacementHelper.BeatToPosition(basicEventData.endBeat) - position);
      }
    }

    public void RemoveSpawnedEvents(
      BasicBeatmapEventType type,
      List<BasicEventEditorData> currentEvents)
    {
      if (currentEvents == null || currentEvents.Count <= 0)
        return;
      for (int index = this._eventMarkers.Count - 1; index >= 0; --index)
      {
        if (!(currentEvents.Find(new Predicate<BasicEventEditorData>(((BaseEditorData) this._eventMarkers[index].data).InstanceEquals)) == (BasicEventEditorData) null) && this._eventMarkers[index].data.type == type)
        {
          this._eventMarkerPool.Despawn(this._eventMarkers[index]);
          this._eventMarkers.RemoveAt(index);
        }
      }
      for (int index = this._eventMarkerObjects.Count - 1; index >= 0; --index)
      {
        if (!(currentEvents.Find(new Predicate<BasicEventEditorData>(((BaseEditorData) this._eventMarkerObjects[index].basicEventData).InstanceEquals)) == (BasicEventEditorData) null) && this._eventMarkerObjects[index].basicEventData.type == type)
        {
          this._lightEventMarkerObjectPool.Despawn(this._eventMarkerObjects[index]);
          this._eventMarkerObjects.RemoveAt(index);
        }
      }
      for (int index = this._durationEventMarkerObjects.Count - 1; index >= 0; --index)
      {
        if (!(currentEvents.Find(new Predicate<BasicEventEditorData>(((BaseEditorData) this._durationEventMarkerObjects[index].basicEventData).InstanceEquals)) == (BasicEventEditorData) null) && this._durationEventMarkerObjects[index].basicEventData.type == type)
        {
          this._durationEventMarkerObjectPool.Despawn(this._durationEventMarkerObjects[index]);
          this._durationEventMarkerObjects.RemoveAt(index);
        }
      }
    }

    public void UpdateSpawnedEvents(float currentBeat)
    {
      for (int index = 0; index < this._eventMarkerObjects.Count; ++index)
      {
        float beat = this._eventMarkerObjects[index].isEnd ? this._eventMarkerObjects[index].basicEventData.endBeat : this._eventMarkerObjects[index].basicEventData.beat;
        float a = beat - currentBeat;
        Vector3 localPosition = this._eventMarkerObjects[index].transform.localPosition with
        {
          z = this._beatmapObjectPlacementHelper.BeatToPosition(beat, currentBeat)
        };
        this._eventMarkerObjects[index].transform.localPosition = localPosition;
        bool selected = this._eventsSelectionState.IsSelected(this._eventMarkerObjects[index].basicEventData.id);
        this._eventMarkerObjects[index].SetState(AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, selected);
      }
      for (int index = 0; index < this._durationEventMarkerObjects.Count; ++index)
      {
        Vector3 localPosition = this._durationEventMarkerObjects[index].transform.localPosition with
        {
          z = this._beatmapObjectPlacementHelper.BeatToPosition(this._durationEventMarkerObjects[index].basicEventData.beat, currentBeat)
        };
        this._durationEventMarkerObjects[index].transform.localPosition = localPosition;
      }
    }

    public void SpawnAt(BasicEventEditorData data, float xPos, float currentBeat)
    {
      BasicEventMarker basicEventMarker = this._eventMarkerPool.Spawn();
      this._eventMarkers.Add(basicEventMarker);
      LightEventMarkerObject eventMarkerObject1 = this._lightEventMarkerObjectPool.Spawn();
      this._eventMarkerObjects.Add(eventMarkerObject1);
      basicEventMarker.Init(data);
      Color eventObjectColor = EventObjectViewColorHelper.GetLightEventObjectColor(data.value, data.floatValue);
      float position1 = this._beatmapObjectPlacementHelper.BeatToPosition(data.beat, currentBeat);
      bool selected = this._eventsSelectionState.IsSelected(data.id);
      eventMarkerObject1.Init(data, eventObjectColor);
      eventMarkerObject1.SetState(AudioTimeHelper.IsBeatSame(position1, 0.0f), (double) position1 < 0.0, selected);
      eventMarkerObject1.SetEventValue(data.value, data.floatValue);
      eventMarkerObject1.transform.localPosition = new Vector3(xPos, 0.0f, position1);
      if (!data.hasEndTime)
        return;
      Color colorB = eventObjectColor;
      switch (data.endValue)
      {
        case 3:
        case 7:
          colorB = Color.clear;
          break;
        case 4:
        case 8:
        case 12:
          colorB = EventObjectViewColorHelper.GetLightEventObjectColor(data.endValue, data.endFloatValue);
          break;
      }
      float position2 = this._beatmapObjectPlacementHelper.BeatToPosition(data.endBeat, currentBeat);
      DurationEventMarkerObject eventMarkerObject2 = this._durationEventMarkerObjectPool.Spawn();
      this._durationEventMarkerObjects.Add(eventMarkerObject2);
      eventMarkerObject2.Init(data, eventObjectColor, colorB);
      eventMarkerObject2.SetScaleZ(position2 - position1);
      eventMarkerObject2.transform.localPosition = new Vector3(xPos, -0.125f, position1);
    }

    public void ClearSpawnedEventObjects()
    {
      for (int index = 0; index < this._eventMarkers.Count; ++index)
        this._eventMarkerPool.Despawn(this._eventMarkers[index]);
      this._eventMarkers.Clear();
      for (int index = 0; index < this._eventMarkerObjects.Count; ++index)
        this._lightEventMarkerObjectPool.Despawn(this._eventMarkerObjects[index]);
      this._eventMarkerObjects.Clear();
      for (int index = 0; index < this._durationEventMarkerObjects.Count; ++index)
        this._durationEventMarkerObjectPool.Despawn(this._durationEventMarkerObjects[index]);
      this._durationEventMarkerObjects.Clear();
    }

    private float GetFadeScale(
      BasicEventEditorData basicEventData,
      BasicEventEditorData nextBasicEventData,
      float currentBeat,
      float startPosition)
    {
      float a = this._beatmapObjectPlacementHelper.BeatToPosition(this._beatmapDataModel.bpmData.FadeEndBeat(basicEventData.beat, 2f), currentBeat) - startPosition;
      if (nextBasicEventData != (BasicEventEditorData) null)
      {
        float position = this._beatmapObjectPlacementHelper.BeatToPosition(nextBasicEventData.beat, currentBeat);
        a = Mathf.Min(a, position - startPosition);
      }
      return a;
    }
  }
}
