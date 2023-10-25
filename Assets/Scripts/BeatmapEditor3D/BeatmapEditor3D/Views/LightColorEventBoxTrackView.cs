// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightColorEventBoxTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class LightColorEventBoxTrackView : 
    EventTrackView<BeatmapEditorObjectId, LightColorBaseEditorData, ColorEventMarkerObject>
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly ColorEventMarkerObject.Pool _colorEventMarkerObjectPool;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    private EventBoxGroupEditorData _eventBoxGroupEditorData;

    public float startBeat => this._eventBoxGroupEditorData.beat;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._eventBoxGroupEditorData = this._eventBoxGroupsState.eventBoxGroupContext;
    }

    protected override List<LightColorBaseEditorData> GetItemsList() => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightColorBaseEditorData>(this.id);

    protected override Vector3 GetEventObjectPosition(LightColorBaseEditorData data) => new Vector3(0.0f, 0.0f, this.beatmapObjectPlacementHelper.BeatToPosition(this.startBeat + data.beat, this.beatmapState.beat));

    protected override ColorEventMarkerObject SpawnEventObject(LightColorBaseEditorData data)
    {
      ColorEventMarkerObject eventMarkerObject = this._colorEventMarkerObjectPool.Spawn();
      if (data.transitionType == LightColorBaseEditorData.TransitionType.Extension)
        eventMarkerObject.Init(ColorEventMarkerObject.EnvironmentColor.Default, ColorEventMarkerObject.InterpolationType.Instant, 0.0f, 0);
      else
        eventMarkerObject.Init(data.colorType.ToObjectColor(), data.transitionType.ToObjectInterpolation(), data.brightness, data.strobeBeatFrequency);
      return eventMarkerObject;
    }

    protected override void DespawnEventObject(ColorEventMarkerObject eventObject) => this._colorEventMarkerObjectPool.Despawn(eventObject);

    protected override (bool onBeat, bool pastBeat, bool selected) GetEventState(
      LightColorBaseEditorData data)
    {
      float a = data.beat - (this.beatmapState.beat - this.startBeat);
      return (AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, this._selectionState.IsSelected(this.id, data.id));
    }

    protected override void UpdateTimeScale()
    {
    }

    public class Pool : MonoMemoryPool<LightColorEventBoxTrackView>
    {
    }
  }
}
