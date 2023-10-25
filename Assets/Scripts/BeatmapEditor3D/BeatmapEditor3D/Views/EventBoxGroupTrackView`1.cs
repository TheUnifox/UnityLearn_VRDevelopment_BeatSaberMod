// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxGroupTrackView`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public abstract class EventBoxGroupTrackView<TEventObject> : 
    EventTrackView<LightGroupSO, EventBoxGroupEditorData, TEventObject>
    where TEventObject : Component, IEventMarkerObject
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;

    public abstract EventBoxGroupEditorData.EventBoxGroupType type { get; }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.signalBus.Subscribe<EventBoxGroupsSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxGroupsSelectionStateUpdated));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.signalBus.TryUnsubscribe<EventBoxGroupsSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxGroupsSelectionStateUpdated));
    }

    protected override List<EventBoxGroupEditorData> GetItemsList() => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupsInterval(this.id.groupId, this.type, this.beatmapState.beat - 5f, this.beatmapState.beat + 16f);

    protected override Vector3 GetEventObjectPosition(EventBoxGroupEditorData data) => new Vector3(0.0f, 0.0f, this.beatmapObjectPlacementHelper.BeatToPosition(data.beat, this.beatmapState.beat));

    protected override (bool onBeat, bool pastBeat, bool selected) GetEventState(
      EventBoxGroupEditorData data)
    {
      float a = data.beat - this.beatmapState.beat;
      return (AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, this._selectionState.IsSelected(data.id));
    }

    protected override void UpdateTimeScale()
    {
    }

    private void HandleEventBoxGroupsSelectionStateUpdated() => this.UpdateTime();
  }
}
