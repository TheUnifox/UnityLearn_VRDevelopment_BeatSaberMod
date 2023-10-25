// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventsToolsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventsToolsView : MonoBehaviour
  {
    [SerializeField]
    private LightEventsToolsGroup _lightEventsToolsGroup;
    [SerializeField]
    private List<EventsToolsGroup> _eventsToolsGroups;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BasicEventsState _basicBasicEventsState;

    public void Refresh()
    {
    }

    protected void OnEnable()
    {
      this._lightEventsToolsGroup.colorValueChangedEvent += new Action<LightColor>(this.HandleLightEventsToolsGroupColorValueChanged);
      this._lightEventsToolsGroup.typeValueChangedEvent += new Action<LightEventType>(this.HandleLightEventsToolsGroupTypeValueChanged);
      this._lightEventsToolsGroup.customValueInputFieldChangedEvent += new Action<int>(this.HandleLightEventsToolsGroupCustomValueChanged);
      foreach (EventsToolsGroup eventsToolsGroup in this._eventsToolsGroups)
      {
        int num = this._basicBasicEventsState.GetSelectedBeatmapTypeValue(eventsToolsGroup.eventGroup).value;
        eventsToolsGroup.SetValue(num);
        eventsToolsGroup.valueChangedEvent += new Action<TrackToolbarType, int>(this.HandleEventToolsGroupHandleValueChanged);
      }
    }

    protected void OnDisable()
    {
      this._lightEventsToolsGroup.colorValueChangedEvent -= new Action<LightColor>(this.HandleLightEventsToolsGroupColorValueChanged);
      this._lightEventsToolsGroup.typeValueChangedEvent -= new Action<LightEventType>(this.HandleLightEventsToolsGroupTypeValueChanged);
      this._lightEventsToolsGroup.customValueInputFieldChangedEvent -= new Action<int>(this.HandleLightEventsToolsGroupCustomValueChanged);
      foreach (EventsToolsGroup eventsToolsGroup in this._eventsToolsGroups)
        eventsToolsGroup.valueChangedEvent -= new Action<TrackToolbarType, int>(this.HandleEventToolsGroupHandleValueChanged);
    }

    private void HandleEventToolsGroupHandleValueChanged(TrackToolbarType group, int value) => this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(group, value));

    private void HandleLightEventsToolsGroupColorValueChanged(LightColor color)
    {
    }

    private void HandleLightEventsToolsGroupTypeValueChanged(LightEventType type)
    {
    }

    private void HandleLightEventsToolsGroupCustomValueChanged(int value)
    {
      this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(TrackToolbarType.CarSelection, value));
      this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(TrackToolbarType.IntValue, value));
    }
  }
}
