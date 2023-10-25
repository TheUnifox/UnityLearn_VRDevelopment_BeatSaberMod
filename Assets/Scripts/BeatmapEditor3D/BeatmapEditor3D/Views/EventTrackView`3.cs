// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventTrackView`3
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public abstract class EventTrackView<TId, TEventData, TEventObject> : EventTrackView
    where TEventData : BaseEditorData
    where TEventObject : Component, IEventMarkerObject
  {
    private HashSet<TEventData> _currentEvents = new HashSet<TEventData>();
    private readonly Dictionary<BeatmapEditorObjectId, TEventObject> _eventObjects = new Dictionary<BeatmapEditorObjectId, TEventObject>();

    public TId id { get; private set; }

    public Dictionary<BeatmapEditorObjectId, TEventObject> eventObjects => this._eventObjects;

    public void Initialize(TId id, string eventLaneName)
    {
      this.id = id;
      this.Initialize(eventLaneName);
    }

    protected virtual void OnEnable()
    {
      this.signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this.signalBus.Subscribe<EventBoxesSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxesSelectionStateUpdated));
      this.signalBus.Subscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this.signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action<BeatmapLevelUpdatedSignal>(this.HandleBeatmapLevelUpdated));
    }

    protected virtual void OnDisable()
    {
      this.signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this.signalBus.TryUnsubscribe<EventBoxesSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxesSelectionStateUpdated));
      this.signalBus.TryUnsubscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this.signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action<BeatmapLevelUpdatedSignal>(this.HandleBeatmapLevelUpdated));
    }

    protected abstract List<TEventData> GetItemsList();

    protected abstract TEventObject SpawnEventObject(TEventData data);

    protected abstract void DespawnEventObject(TEventObject eventObject);

    protected abstract Vector3 GetEventObjectPosition(TEventData data);

    protected abstract (bool onBeat, bool pastBeat, bool selected) GetEventState(TEventData data);

    protected override void UpdateTime()
    {
      List<TEventData> itemsList = this.GetItemsList();
      HashSet<TEventData> newEventsHashSet = new HashSet<TEventData>((IEnumerable<TEventData>) itemsList);
      List<TEventData> list1 = itemsList.Where<TEventData>((Func<TEventData, bool>) (e => !this._currentEvents.Contains(e))).ToList<TEventData>();
      List<TEventData> list2 = this._currentEvents.Where<TEventData>((Func<TEventData, bool>) (e => !newEventsHashSet.Contains(e))).ToList<TEventData>();
      if (list1.Count != 0 || list2.Count != 0)
      {
        this.RemoveEventObjects((IEnumerable<TEventData>) list2);
        this.AddEventObjects((IEnumerable<TEventData>) list1);
        this._currentEvents = newEventsHashSet;
      }
      this.UpdateEventObjects();
    }

    protected void Clear()
    {
      this._currentEvents.Clear();
      foreach (BeatmapEditorObjectId key in this._eventObjects.Keys.ToList<BeatmapEditorObjectId>())
      {
        TEventObject eventObject = this._eventObjects[key];
        this._eventTrackInputMouseInputSource.UnsubscribeFromMouseEvents((Component) eventObject);
        this._eventObjects.Remove(key);
        this.DespawnEventObject(eventObject);
      }
      this._eventObjects.Clear();
    }

    private void RemoveEventObjects(IEnumerable<TEventData> eventsToRemove)
    {
      foreach (TEventData eventData in eventsToRemove)
      {
        TEventObject eventObject = this._eventObjects[eventData.id];
        this._eventTrackInputMouseInputSource.UnsubscribeFromMouseEvents((Component) eventObject);
        this._eventObjects.Remove(eventData.id);
        this.DespawnEventObject(eventObject);
      }
    }

    private void AddEventObjects(IEnumerable<TEventData> eventsToAdd)
    {
      foreach (TEventData data in eventsToAdd)
      {
        TEventObject eventObject = this.SpawnEventObject(data);
        eventObject.transform.SetParent(this.containerTransform, false);
        (bool onBeat, bool pastBeat, bool selected) = this.GetEventState(data);
        eventObject.SetState(onBeat, pastBeat, selected);
        this._eventTrackInputMouseInputSource.SubscribeToMouseEvents(data.id, (Component) eventObject);
        this._eventObjects[data.id] = eventObject;
      }
    }

    private void UpdateEventObjects()
    {
      foreach (TEventData currentEvent in this._currentEvents)
      {
        this._eventObjects[currentEvent.id].transform.localPosition = this.GetEventObjectPosition(currentEvent);
        (bool onBeat, bool pastBeat, bool selected) = this.GetEventState(currentEvent);
        this._eventObjects[currentEvent.id].SetState(onBeat, pastBeat, selected);
      }
    }

    private void HandleBeatmapLevelStateTimeUpdated() => this.UpdateTime();

    private void HandleEventBoxesSelectionStateUpdated() => this.UpdateTime();

    private void HandleBeatmapTimeScaleChanged()
    {
      this.UpdateTime();
      this.UpdateTimeScale();
    }

    private void HandleBeatmapLevelUpdated(BeatmapLevelUpdatedSignal signal)
    {
      if (signal.forceRedraw)
        this.Clear();
      this.UpdateTime();
    }
  }
}
