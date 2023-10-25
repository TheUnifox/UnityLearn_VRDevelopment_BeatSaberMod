// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.ColorEventBoxGroupTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class ColorEventBoxGroupTrackView : EventBoxGroupTrackView<ColorEventMarkerObject>
  {
    [Inject]
    private readonly ColorEventMarkerObject.Pool _colorEventMarkerObjectPool;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;

    public override EventBoxGroupEditorData.EventBoxGroupType type => EventBoxGroupEditorData.EventBoxGroupType.Color;

    protected override ColorEventMarkerObject SpawnEventObject(EventBoxGroupEditorData data)
    {
      ColorEventMarkerObject eventMarkerObject = this._colorEventMarkerObjectPool.Spawn();
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(data.id);
      LightColorBaseEditorData colorBaseEditorData = (LightColorBaseEditorData) null;
      foreach (EventBoxEditorData eventBoxEditorData in byEventBoxGroupId)
      {
        List<BaseEditorData> listByEventBoxId = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id);
        if (listByEventBoxId.Count > 0)
        {
          colorBaseEditorData = (LightColorBaseEditorData) listByEventBoxId[0];
          break;
        }
      }
      if (colorBaseEditorData != (LightColorBaseEditorData) null)
      {
        if (colorBaseEditorData.transitionType == LightColorBaseEditorData.TransitionType.Extension)
          eventMarkerObject.Init(ColorEventMarkerObject.EnvironmentColor.Default, ColorEventMarkerObject.InterpolationType.Instant, 0.0f, 0);
        else
          eventMarkerObject.Init(colorBaseEditorData.colorType.ToObjectColor(), colorBaseEditorData.transitionType.ToObjectInterpolation(), colorBaseEditorData.brightness, colorBaseEditorData.strobeBeatFrequency);
      }
      else
        eventMarkerObject.Init(ColorEventMarkerObject.EnvironmentColor.Default, ColorEventMarkerObject.InterpolationType.Instant, 0.0f, 0);
      return eventMarkerObject;
    }

    protected override void DespawnEventObject(ColorEventMarkerObject eventObject) => this._colorEventMarkerObjectPool.Despawn(eventObject);

    public class Pool : MonoMemoryPool<ColorEventBoxGroupTrackView>
    {
      protected override void OnDespawned(ColorEventBoxGroupTrackView item)
      {
        base.OnDespawned(item);
        item.Clear();
      }
    }
  }
}
