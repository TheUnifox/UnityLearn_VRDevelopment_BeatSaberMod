// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BasicEventObjectsContainer
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
  public class BasicEventObjectsContainer : MonoBehaviour
  {
    [SerializeField]
    private BeatGridContainer _beatGridContainer;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly EventTrack.Pool _eventTrackPool;
    private readonly List<EventTrack> _spawnedTracks = new List<EventTrack>();

    protected void OnEnable()
    {
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>(new Action(this.HandleBeatmapLevelDataModelLoaded));
      this._signalBus.Subscribe<EventsPageChangedSignal>(new Action<EventsPageChangedSignal>(this.HandleEventsPageChangedSignal));
      this.ClearEventTracks();
      this.SpawnEventTracks();
      this._beatGridContainer.SetDataToBeatContainers((float) this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count, 0.5f);
      this._beatGridContainer.Enable();
    }

    protected void OnDisable()
    {
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>(new Action(this.HandleBeatmapLevelDataModelLoaded));
      this._signalBus.TryUnsubscribe<EventsPageChangedSignal>(new Action<EventsPageChangedSignal>(this.HandleEventsPageChangedSignal));
      this._beatGridContainer.Disable();
    }

    private void HandleBeatmapLevelDataModelLoaded()
    {
      this.ClearEventTracks();
      this.SpawnEventTracks();
      this._beatGridContainer.SetDataToBeatContainers((float) this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count, 0.5f);
      this._beatGridContainer.ForceUpdate();
    }

    private void HandleEventsPageChangedSignal(EventsPageChangedSignal signal)
    {
      this.ClearEventTracks();
      this.SpawnEventTracks();
      this._beatGridContainer.SetDataToBeatContainers((float) this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count, 0.5f);
      this._beatGridContainer.ForceUpdate();
    }

    private void SpawnEventTracks()
    {
      List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo> basicEventTrackInfoList = this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage];
      float num = (float) ((double) -(basicEventTrackInfoList.Count - 1) * 0.5 * 0.5);
      for (int index = 0; index < basicEventTrackInfoList.Count; ++index)
      {
        EnvironmentTracksDefinitionSO.BasicEventTrackInfo basicEventTrackInfo = basicEventTrackInfoList[index];
        EventTrack eventTrack = this._eventTrackPool.Spawn();
        eventTrack.Init(basicEventTrackInfo.trackName, 1f);
        Transform transform = eventTrack.transform;
        transform.SetParent(this.transform, false);
        transform.localPosition = new Vector3(num + (float) index * 0.5f, -0.2f, 0.0f);
        this._spawnedTracks.Add(eventTrack);
      }
    }

    private void ClearEventTracks()
    {
      foreach (EventTrack spawnedTrack in this._spawnedTracks)
        this._eventTrackPool.Despawn(spawnedTrack);
      this._spawnedTracks.Clear();
    }
  }
}
