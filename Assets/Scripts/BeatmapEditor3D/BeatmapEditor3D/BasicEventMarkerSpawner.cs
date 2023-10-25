// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BasicEventMarkerSpawner
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
  public class BasicEventMarkerSpawner : 
    IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>
  {
    [Inject]
    private readonly IReadonlyEventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly BasicEventMarker.Pool _eventMarkerPool;
    [Inject]
    private readonly TextEventMarkerObject.Pool _textEventMarkerObjectPool;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    private readonly List<BasicEventMarker> _eventMarkers = new List<BasicEventMarker>();
    private readonly List<TextEventMarkerObject> _eventMarkerObjects = new List<TextEventMarkerObject>();

    public void UpdateDurationEvents()
    {
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
    }

    public void UpdateSpawnedEvents(float currentBeat)
    {
      for (int index = 0; index < this._eventMarkerObjects.Count; ++index)
      {
        Vector3 localPosition = this._eventMarkerObjects[index].transform.localPosition with
        {
          z = this._beatmapObjectPlacementHelper.BeatToPosition(this._eventMarkerObjects[index].basicEventData.beat, currentBeat)
        };
        this._eventMarkerObjects[index].transform.localPosition = localPosition;
        bool selected = this._eventsSelectionState.IsSelected(this._eventMarkerObjects[index].basicEventData.id);
        float a = this._eventMarkerObjects[index].basicEventData.beat - currentBeat;
        this._eventMarkerObjects[index].SetState(AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, selected);
      }
    }

    public void SpawnAt(BasicEventEditorData data, float xPos, float currentBeat)
    {
      TextEventMarkerObject eventMarkerObject = this._textEventMarkerObjectPool.Spawn();
      this._eventMarkerObjects.Add(eventMarkerObject);
      bool selected = this._eventsSelectionState.IsSelected(data.id);
      float a = data.beat - currentBeat;
      eventMarkerObject.Init(data);
      eventMarkerObject.SetState(AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, selected);
      eventMarkerObject.SetText(string.Format("{0}/{1}", (object) data.value, (object) data.floatValue));
      eventMarkerObject.transform.localPosition = new Vector3(xPos, 0.0f, this._beatmapObjectPlacementHelper.BeatToPosition(data.beat, currentBeat));
      BasicEventMarker basicEventMarker = this._eventMarkerPool.Spawn();
      this._eventMarkers.Add(basicEventMarker);
      basicEventMarker.Init(data);
    }

    public void ClearSpawnedEventObjects()
    {
      for (int index = 0; index < this._eventMarkers.Count; ++index)
        this._eventMarkerPool.Despawn(this._eventMarkers[index]);
      this._eventMarkers.Clear();
      for (int index = 0; index < this._eventMarkerObjects.Count; ++index)
        this._textEventMarkerObjectPool.Despawn(this._eventMarkerObjects[index]);
      this._eventMarkerObjects.Clear();
    }
  }
}
