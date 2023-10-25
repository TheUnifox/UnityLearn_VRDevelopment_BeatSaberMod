// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.RotationEventBoxGroupTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class RotationEventBoxGroupTrackView : EventBoxGroupTrackView<TextOnlyEventMarkerObject>
  {
    [Inject]
    private readonly TextOnlyEventMarkerObject.Pool _textOnlyEventMarkerObjectPool;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;

    public override EventBoxGroupEditorData.EventBoxGroupType type => EventBoxGroupEditorData.EventBoxGroupType.Rotation;

    protected override TextOnlyEventMarkerObject SpawnEventObject(EventBoxGroupEditorData data)
    {
      TextOnlyEventMarkerObject eventMarkerObject = this._textOnlyEventMarkerObjectPool.Spawn();
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(data.id);
      LightRotationBaseEditorData rotationBaseEditorData = (LightRotationBaseEditorData) null;
      for (int index = 0; index < byEventBoxGroupId.Count; ++index)
      {
        List<BaseEditorData> listByEventBoxId = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(byEventBoxGroupId[index].id);
        if (listByEventBoxId.Count > 0)
        {
          rotationBaseEditorData = (LightRotationBaseEditorData) listByEventBoxId[0];
          break;
        }
      }
      if (rotationBaseEditorData != (LightRotationBaseEditorData) null && !rotationBaseEditorData.usePreviousEventRotationValue)
        eventMarkerObject.Init(string.Format("{0}, {1}, {2}", (object) rotationBaseEditorData.easeType, (object) rotationBaseEditorData.loopsCount, (object) rotationBaseEditorData.rotation));
      else
        eventMarkerObject.Init("");
      return eventMarkerObject;
    }

    protected override void DespawnEventObject(TextOnlyEventMarkerObject eventObject) => this._textOnlyEventMarkerObjectPool.Despawn(eventObject);

    public class Pool : MonoMemoryPool<RotationEventBoxGroupTrackView>
    {
      protected override void OnDespawned(RotationEventBoxGroupTrackView item)
      {
        base.OnDespawned(item);
        item.Clear();
      }
    }
  }
}
