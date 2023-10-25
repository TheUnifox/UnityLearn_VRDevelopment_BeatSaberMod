// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightTranslationEventBoxTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class LightTranslationEventBoxTrackView : 
    EventTrackView<BeatmapEditorObjectId, LightTranslationBaseEditorData, TextOnlyEventMarkerObject>
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly TextOnlyEventMarkerObject.Pool _textOnlyEventMarkerObjectPool;
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

    protected override List<LightTranslationBaseEditorData> GetItemsList() => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightTranslationBaseEditorData>(this.id);

    protected override Vector3 GetEventObjectPosition(LightTranslationBaseEditorData data) => new Vector3(0.0f, 0.0f, this.beatmapObjectPlacementHelper.BeatToPosition(this.startBeat + data.beat, this.beatmapState.beat));

    protected override TextOnlyEventMarkerObject SpawnEventObject(
      LightTranslationBaseEditorData data)
    {
      TextOnlyEventMarkerObject eventMarkerObject = this._textOnlyEventMarkerObjectPool.Spawn();
      eventMarkerObject.Init(data.usePreviousEventTranslationValue ? "" : string.Format("{0} / {1}", (object) (float) ((double) data.translation * 100.0), (object) data.easeType));
      return eventMarkerObject;
    }

    protected override void DespawnEventObject(TextOnlyEventMarkerObject eventObject) => this._textOnlyEventMarkerObjectPool.Despawn(eventObject);

    protected override (bool onBeat, bool pastBeat, bool selected) GetEventState(
      LightTranslationBaseEditorData data)
    {
      float a = data.beat - (this.beatmapState.beat - this.startBeat);
      return (AudioTimeHelper.IsBeatSame(a, 0.0f), (double) a < 0.0, this._selectionState.IsSelected(this.id, data.id));
    }

    protected override void UpdateTimeScale()
    {
    }

    public class Pool : MonoMemoryPool<LightTranslationEventBoxTrackView>
    {
    }
  }
}
