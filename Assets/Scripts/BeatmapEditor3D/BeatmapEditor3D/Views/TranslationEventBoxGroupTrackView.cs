// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.TranslationEventBoxGroupTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class TranslationEventBoxGroupTrackView : EventBoxGroupTrackView<TextOnlyEventMarkerObject>
  {
    [Inject]
    private readonly TextOnlyEventMarkerObject.Pool _textOnlyEventMarkerObjectPool;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;

    public override EventBoxGroupEditorData.EventBoxGroupType type => EventBoxGroupEditorData.EventBoxGroupType.Translation;

    protected override TextOnlyEventMarkerObject SpawnEventObject(EventBoxGroupEditorData data)
    {
      TextOnlyEventMarkerObject eventMarkerObject = this._textOnlyEventMarkerObjectPool.Spawn();
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(data.id);
      LightTranslationBaseEditorData translationBaseEditorData = (LightTranslationBaseEditorData) null;
      for (int index = 0; index < byEventBoxGroupId.Count; ++index)
      {
        List<BaseEditorData> listByEventBoxId = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(byEventBoxGroupId[index].id);
        if (listByEventBoxId.Count > 0)
        {
          translationBaseEditorData = (LightTranslationBaseEditorData) listByEventBoxId[0];
          break;
        }
      }
      if (translationBaseEditorData != (LightTranslationBaseEditorData) null && !translationBaseEditorData.usePreviousEventTranslationValue)
        eventMarkerObject.Init(string.Format("{0} / {1}", (object) (float) ((double) translationBaseEditorData.translation * 100.0), (object) translationBaseEditorData.easeType));
      else
        eventMarkerObject.Init("");
      return eventMarkerObject;
    }

    protected override void DespawnEventObject(TextOnlyEventMarkerObject eventObject) => this._textOnlyEventMarkerObjectPool.Despawn(eventObject);

    public class Pool : MonoMemoryPool<TranslationEventBoxGroupTrackView>
    {
      protected override void OnDespawned(TranslationEventBoxGroupTrackView item)
      {
        base.OnDespawned(item);
        item.Clear();
      }
    }
  }
}
