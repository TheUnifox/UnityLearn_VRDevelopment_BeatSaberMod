// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxObjectsContainer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using BeatmapEditor3D.Visuals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventBoxObjectsContainer : MonoBehaviour
  {
    [SerializeField]
    private BeatGridContainer _beatGridContainer;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly LightColorEventBoxTrackView.Pool _lightColorEventBoxTrackViewPool;
    [Inject]
    private readonly LightRotationEventBoxTrackView.Pool _lightRotationEventBoxTrackViewPool;
    [Inject]
    private readonly LightTranslationEventBoxTrackView.Pool _lightTranslationEventBoxTrackViewPool;
    private readonly List<EventTrackView> _spawnedTracks = new List<EventTrackView>();
    private EventBoxGroupEditorData _eventBoxGroup;
    private List<EventBoxEditorData> _eventBoxes;

    protected void OnEnable()
    {
      this.SetEventBoxGroupData();
      if (this._eventBoxGroup == (EventBoxGroupEditorData) null)
        return;
      this._signalBus.Subscribe<EditingEventBoxGroupChangedSignal>(new Action(this.HandleEditingEventBoxGroupChanged));
      this._signalBus.Subscribe<EventBoxesUpdatedSignal>(new Action(this.HandleEventBoxesUpdated));
      this._beatGridContainer.Enable();
    }

    protected void OnDisable()
    {
      if (this._eventBoxGroup == (EventBoxGroupEditorData) null)
        return;
      this._signalBus.TryUnsubscribe<EditingEventBoxGroupChangedSignal>(new Action(this.HandleEditingEventBoxGroupChanged));
      this._signalBus.TryUnsubscribe<EventBoxesUpdatedSignal>(new Action(this.HandleEventBoxesUpdated));
      this._beatGridContainer.Disable();
    }

    private void HandleEditingEventBoxGroupChanged() => this.SetEventBoxGroupData();

    private void HandleEventBoxesUpdated() => this.SetEventBoxGroupData();

    private void SetEventBoxGroupData()
    {
      this._eventBoxGroup = this._eventBoxGroupsState.eventBoxGroupContext;
      if (this._eventBoxGroup == (EventBoxGroupEditorData) null)
        return;
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroup.id);
      this.ClearEventBoxTracks();
      this.SpawnEventBoxTracks();
      this._beatGridContainer.SetDataToBeatContainers((float) this._eventBoxes.Count, 0.5f);
      this._beatGridContainer.ForceUpdate();
    }

    private void SpawnEventBoxTracks()
    {
      float num = (float) ((double) -(this._eventBoxes.Count - 1) * 0.5 * 0.5);
      for (int index = 0; index < this._eventBoxes.Count; ++index)
      {
        EventBoxEditorData eventBox = this._eventBoxes[index];
        EventTrackView eventTrackView = (EventTrackView) null;
        switch (eventBox)
        {
          case LightColorEventBoxEditorData _:
            LightColorEventBoxTrackView eventBoxTrackView1 = this._lightColorEventBoxTrackViewPool.Spawn();
            eventBoxTrackView1.Initialize(eventBox.id, string.Format("Color Event Box ({0})", (object) index));
            eventTrackView = (EventTrackView) eventBoxTrackView1;
            break;
          case LightRotationEventBoxEditorData _:
            LightRotationEventBoxTrackView eventBoxTrackView2 = this._lightRotationEventBoxTrackViewPool.Spawn();
            eventBoxTrackView2.Initialize(eventBox.id, string.Format("Rotation Event Box ({0})", (object) index));
            eventTrackView = (EventTrackView) eventBoxTrackView2;
            break;
          case LightTranslationEventBoxEditorData _:
            LightTranslationEventBoxTrackView eventBoxTrackView3 = this._lightTranslationEventBoxTrackViewPool.Spawn();
            eventBoxTrackView3.Initialize(eventBox.id, string.Format("Translation Event Box ({0})", (object) index));
            eventTrackView = (EventTrackView) eventBoxTrackView3;
            break;
        }
        if (!((UnityEngine.Object) eventTrackView == (UnityEngine.Object) null))
        {
          Transform transform = eventTrackView.transform;
          transform.SetParent(this.transform, false);
          transform.localPosition = Vector3.right * (num + (float) index * 0.5f);
          this._spawnedTracks.Add(eventTrackView);
        }
      }
    }

    private void ClearEventBoxTracks()
    {
      foreach (EventTrackView spawnedTrack in this._spawnedTracks)
      {
        switch (spawnedTrack)
        {
          case LightColorEventBoxTrackView eventBoxTrackView1:
            this._lightColorEventBoxTrackViewPool.Despawn(eventBoxTrackView1);
            continue;
          case LightRotationEventBoxTrackView eventBoxTrackView2:
            this._lightRotationEventBoxTrackViewPool.Despawn(eventBoxTrackView2);
            continue;
          case LightTranslationEventBoxTrackView eventBoxTrackView3:
            this._lightTranslationEventBoxTrackViewPool.Despawn(eventBoxTrackView3);
            continue;
          default:
            continue;
        }
      }
      this._spawnedTracks.Clear();
    }
  }
}
