// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventsToolsGroup
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class EventsToolsGroup : MonoBehaviour
  {
    [SerializeField]
    private TrackToolbarType _eventGroup;
    [SerializeField]
    private EventsToolsGroup.EventButtonValue[] _eventButtons;

    public TrackToolbarType eventGroup => this._eventGroup;

    public event Action<TrackToolbarType, int> valueChangedEvent;

    public void SetValue(int value)
    {
      foreach (EventsToolsGroup.EventButtonValue eventButton in this._eventButtons)
        eventButton.button.interactable = value != eventButton.value;
    }

    protected void OnEnable()
    {
      foreach (EventsToolsGroup.EventButtonValue eventButton in this._eventButtons)
      {
        EventsToolsGroup.EventButtonValue eventButtonPair = eventButton;
        eventButtonPair.button.onClick.AddListener((UnityAction) (() =>
        {
          Action<TrackToolbarType, int> valueChangedEvent = this.valueChangedEvent;
          if (valueChangedEvent != null)
            valueChangedEvent(this._eventGroup, eventButtonPair.value);
          this.SetValue(eventButtonPair.value);
        }));
      }
    }

    protected void OnDisable()
    {
      foreach (EventsToolsGroup.EventButtonValue eventButton in this._eventButtons)
        eventButton.button.onClick.RemoveAllListeners();
    }

    [Serializable]
    private struct EventButtonValue
    {
      [SerializeField]
      private Button _button;
      [SerializeField]
      private int _value;

      public Button button => this._button;

      public int value => this._value;
    }
  }
}
