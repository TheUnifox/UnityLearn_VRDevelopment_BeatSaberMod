// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightRotationEventBoxTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class LightRotationEventBoxTrackView : 
    EventTrackView<BeatmapEditorObjectId, LightRotationBaseEditorData, TextOnlyEventMarkerObject>
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly TextOnlyEventMarkerObject.Pool _textOnlyEventMarkerObjectPool;
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    private EventBoxGroupEditorData _eventBoxGroupEditorData;

    public float startBeat => this._eventBoxGroupEditorData.beat;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._eventBoxGroupEditorData = this._eventBoxGroupsState.eventBoxGroupContext;
    }

    protected override List<LightRotationBaseEditorData> GetItemsList() => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightRotationBaseEditorData>(this.id);

    protected override Vector3 GetEventObjectPosition(LightRotationBaseEditorData data) => new Vector3(0.0f, 0.0f, this.beatmapObjectPlacementHelper.BeatToPosition(this.startBeat + data.beat, this._beatmapState.beat));

    protected override TextOnlyEventMarkerObject SpawnEventObject(LightRotationBaseEditorData data)
    {
      TextOnlyEventMarkerObject eventMarkerObject = this._textOnlyEventMarkerObjectPool.Spawn();
      eventMarkerObject.Init(data.usePreviousEventRotationValue ? "" : string.Format("{0}, {1}, {2}", (object) data.easeType, (object) data.loopsCount, (object) data.rotation));
      return eventMarkerObject;
    }

    protected override void DespawnEventObject(TextOnlyEventMarkerObject eventObject) => this._textOnlyEventMarkerObjectPool.Despawn(eventObject);

    protected override (bool onBeat, bool pastBeat, bool selected) GetEventState(
      LightRotationBaseEditorData data)
    {
      float a = data.beat - (this.beatmapState.beat - this.startBeat);
      return (AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, this._selectionState.IsSelected(this.id, data.id));
    }

    protected override void UpdateTimeScale()
    {
    }

    public class Pool : MonoMemoryPool<LightRotationEventBoxTrackView>
    {
    }
  }
}
