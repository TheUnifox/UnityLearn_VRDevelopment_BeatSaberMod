// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DurationEventMarkerSpawner
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class DurationEventMarkerSpawner : 
    IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>
  {
    [Inject]
    private readonly IReadonlyEventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly BasicEventMarker.Pool _eventMarkerPool;
    [Inject]
    private readonly TextEventMarkerObject.Pool _textEventMarkerObjectPool;
    [Inject]
    private readonly DurationEventMarkerObject.Pool _durationEventMarkerObjectPool;
    private readonly List<BasicEventMarker> _eventMarkers = new List<BasicEventMarker>();
    private readonly List<TextEventMarkerObject> _eventMarkerObjects = new List<TextEventMarkerObject>();
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
          this._textEventMarkerObjectPool.Despawn(this._eventMarkerObjects[index]);
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
            for (int i = 0; i < this._eventMarkerObjects.Count; i++)
            {
                float num = this._eventMarkerObjects[i].basicEventData.beat - currentBeat;
                Vector3 localPosition = this._eventMarkerObjects[i].transform.localPosition;
                localPosition.z = this._beatmapObjectPlacementHelper.BeatToPosition(this._eventMarkerObjects[i].basicEventData.beat, currentBeat);
                this._eventMarkerObjects[i].transform.localPosition = localPosition;
                bool selected = this._eventsSelectionState.IsSelected(this._eventMarkerObjects[i].basicEventData.id);
                this._eventMarkerObjects[i].SetState(AudioTimeHelper.IsBeatSame(num, 0f), num < 0f, selected);
            }
            for (int j = 0; j < this._durationEventMarkerObjects.Count; j++)
            {
                Vector3 localPosition2 = this._durationEventMarkerObjects[j].transform.localPosition;
                localPosition2.z = this._beatmapObjectPlacementHelper.BeatToPosition(this._durationEventMarkerObjects[j].basicEventData.beat, currentBeat);
                this._durationEventMarkerObjects[j].transform.localPosition = localPosition2;
            }
        }

        public void SpawnAt(BasicEventEditorData data, float xPos, float currentBeat)
    {
      BasicEventMarker basicEventMarker = this._eventMarkerPool.Spawn();
      this._eventMarkers.Add(basicEventMarker);
      TextEventMarkerObject eventMarkerObject1 = this._textEventMarkerObjectPool.Spawn();
      this._eventMarkerObjects.Add(eventMarkerObject1);
      basicEventMarker.Init(data);
      float a = data.beat - currentBeat;
      float position1 = this._beatmapObjectPlacementHelper.BeatToPosition(data.beat, currentBeat);
      bool selected = this._eventsSelectionState.IsSelected(data.id);
      eventMarkerObject1.Init(data);
      eventMarkerObject1.SetState(AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, selected);
      eventMarkerObject1.SetText(data.value.ToString());
      eventMarkerObject1.transform.localPosition = new Vector3(xPos, 0.0f, position1);
      if (!data.hasEndTime)
        return;
      float position2 = this._beatmapObjectPlacementHelper.BeatToPosition(data.endBeat, currentBeat);
      DurationEventMarkerObject eventMarkerObject2 = this._durationEventMarkerObjectPool.Spawn();
      this._durationEventMarkerObjects.Add(eventMarkerObject2);
      eventMarkerObject2.Init(data, Color.gray, Color.gray);
      eventMarkerObject2.transform.localPosition = new Vector3(xPos, -0.125f, position1);
      eventMarkerObject2.SetScaleZ(position2 - position1);
    }

    public void ClearSpawnedEventObjects()
    {
      for (int index = 0; index < this._eventMarkers.Count; ++index)
        this._eventMarkerPool.Despawn(this._eventMarkers[index]);
      this._eventMarkers.Clear();
      for (int index = 0; index < this._eventMarkerObjects.Count; ++index)
        this._textEventMarkerObjectPool.Despawn(this._eventMarkerObjects[index]);
      this._eventMarkerObjects.Clear();
      for (int index = 0; index < this._durationEventMarkerObjects.Count; ++index)
        this._durationEventMarkerObjectPool.Despawn(this._durationEventMarkerObjects[index]);
      this._durationEventMarkerObjects.Clear();
    }
  }
}
