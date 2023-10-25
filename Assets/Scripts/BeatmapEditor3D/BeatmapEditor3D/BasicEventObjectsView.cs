// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BasicEventObjectsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BasicEventObjectsView : BaseObjectsView
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly EventMarkerSpawnerProvider _eventMarkerSpawnerProvider;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly BasicEventMarkerSpawner _basicEventMarkerSpawner;
    [Inject]
    private readonly DurationEventMarkerSpawner _durationEventMarkerSpawner;
    [Inject]
    private readonly LightEventMarkerSpawner _lightEventMarkerSpawner;
    private readonly Dictionary<BasicBeatmapEventType, List<BasicEventEditorData>> _prevEvents = new Dictionary<BasicBeatmapEventType, List<BasicEventEditorData>>();

    protected override void OnEnable()
    {
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.Subscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.Subscribe<EventsSelectionStateUpdatedSignal>(new Action(this.HandleEventsSelectionStateUpdated));
      this._signalBus.Subscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChanged));
      this.RefreshView();
      base.OnEnable();
    }

    protected override void OnDisable()
    {
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.TryUnsubscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.TryUnsubscribe<EventsSelectionStateUpdatedSignal>(new Action(this.HandleEventsSelectionStateUpdated));
      this._signalBus.TryUnsubscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChanged));
      base.OnDisable();
    }

    private void ClearView()
    {
      this._prevEvents.Clear();
      this._basicEventMarkerSpawner.ClearSpawnedEventObjects();
      this._lightEventMarkerSpawner.ClearSpawnedEventObjects();
      this._durationEventMarkerSpawner.ClearSpawnedEventObjects();
    }

    private void RefreshView()
    {
      float from = this._beatmapState.beat - 5f;
      float to = this._beatmapState.beat + 16f;
      List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo> basicEventTrackInfoList = this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage];
      int count = basicEventTrackInfoList.Count;
      for (int index = 0; index < basicEventTrackInfoList.Count; ++index)
      {
        EnvironmentTracksDefinitionSO.BasicEventTrackInfo basicEventTrack = basicEventTrackInfoList[index];
        List<BasicEventEditorData> currentEvents = this._beatmapBasicEventsDataModel.GetBasicEventsInterval(basicEventTrack.basicBeatmapEventType, from, to);
        List<BasicEventEditorData> prevEvents = this._prevEvents.ContainsKey(basicEventTrack.basicBeatmapEventType) ? this._prevEvents[basicEventTrack.basicBeatmapEventType] : new List<BasicEventEditorData>();
        List<BasicEventEditorData> list1 = currentEvents.Where<BasicEventEditorData>((Func<BasicEventEditorData, bool>) (evt => prevEvents.All<BasicEventEditorData>((Func<BasicEventEditorData, bool>) (prevEvt => prevEvt != evt)))).ToList<BasicEventEditorData>();
        List<BasicEventEditorData> list2 = prevEvents.Where<BasicEventEditorData>((Func<BasicEventEditorData, bool>) (prevEvt => currentEvents.All<BasicEventEditorData>((Func<BasicEventEditorData, bool>) (evt => evt != prevEvt)))).ToList<BasicEventEditorData>();
        this.RemoveEventObjects(basicEventTrack.basicBeatmapEventType, list2);
        this.AddEventObjects(basicEventTrack, index, count, list1);
        this._prevEvents[basicEventTrack.basicBeatmapEventType] = currentEvents;
      }
      this.UpdateEventObjects();
    }

    private void UpdateDurationEventsTimeScale()
    {
      this._basicEventMarkerSpawner.UpdateDurationEvents();
      this._lightEventMarkerSpawner.UpdateDurationEvents();
      this._durationEventMarkerSpawner.UpdateDurationEvents();
    }

    private void RemoveEventObjects(
      BasicBeatmapEventType basicBeatmapEventType,
      List<BasicEventEditorData> eventsToRemove)
    {
      this._basicEventMarkerSpawner.RemoveSpawnedEvents(basicBeatmapEventType, eventsToRemove);
      this._lightEventMarkerSpawner.RemoveSpawnedEvents(basicBeatmapEventType, eventsToRemove);
      this._durationEventMarkerSpawner.RemoveSpawnedEvents(basicBeatmapEventType, eventsToRemove);
    }

    private void UpdateEventObjects()
    {
      this._basicEventMarkerSpawner.UpdateSpawnedEvents(this._beatmapState.beat);
      this._lightEventMarkerSpawner.UpdateSpawnedEvents(this._beatmapState.beat);
      this._durationEventMarkerSpawner.UpdateSpawnedEvents(this._beatmapState.beat);
    }

    private void AddEventObjects(
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo basicEventTrack,
      int i,
      int trackCount,
      List<BasicEventEditorData> eventsToAdd)
    {
      if (eventsToAdd == null || eventsToAdd.Count == 0)
        return;
      IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData> markerSpawner = this._eventMarkerSpawnerProvider.GetMarkerSpawner(basicEventTrack.trackDefinition.markerType);
      if (markerSpawner == null)
      {
        Debug.Log((object) string.Format("Null MarkerSpawner for {0}", (object) basicEventTrack.trackDefinition.markerType));
      }
      else
      {
        EventEditorDataComparer<BasicEventEditorData> editorDataComparer = new EventEditorDataComparer<BasicEventEditorData>();
        float position = TrackPlacementHelper.TrackToPosition(i, trackCount);
        eventsToAdd.Sort(new Comparison<BasicEventEditorData>(editorDataComparer.Compare));
        foreach (BasicEventEditorData data in eventsToAdd)
          markerSpawner.SpawnAt(data, position, this._beatmapState.beat);
      }
    }

    private void HandleBeatmapLevelStateTimeUpdated() => this.RefreshView();

    private void HandleBeatmapTimeScaleChanged()
    {
      this.RefreshView();
      this.UpdateDurationEventsTimeScale();
    }

    private void HandleBeatmapLevelUpdated() => this.RefreshView();

    private void HandleEventsSelectionStateUpdated() => this.RefreshView();

    private void HandleEventsPageChanged()
    {
      this.ClearView();
      this.RefreshView();
    }
  }
}
