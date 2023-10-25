// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventBoxToolbarView : MonoBehaviour
  {
    [SerializeField]
    private LightColorEventToolbar _lightColorEventToolbar;
    [SerializeField]
    private LightRotationEventToolbar _lightRotationEventToolbar;
    [SerializeField]
    private LightTranslationEventToolbar _lightTranslationEventToolbar;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    protected void OnEnable()
    {
      if (this._eventBoxGroupsState.eventBoxGroupContext == (EventBoxGroupEditorData) null)
        return;
      EventBoxGroupEditorData.EventBoxGroupType type = this._eventBoxGroupsState.eventBoxGroupContext.type;
      this._lightColorEventToolbar.gameObject.SetActive(type == EventBoxGroupEditorData.EventBoxGroupType.Color);
      this._lightRotationEventToolbar.gameObject.SetActive(type == EventBoxGroupEditorData.EventBoxGroupType.Rotation);
      this._lightTranslationEventToolbar.gameObject.SetActive(type == EventBoxGroupEditorData.EventBoxGroupType.Translation);
      this._lightColorEventToolbar.Init(true);
      switch (type)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Color:
          this._lightColorEventToolbar.SetValue(this._eventBoxGroupsState.lightColorType, this._eventBoxGroupsState.lightColorTransitionType, this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightStrobeBeatFrequency, this._eventBoxGroupsState.eventBoxGroupExtension);
          this._lightColorEventToolbar.SetKeyBindings(this._keyboardBinder);
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          this._lightRotationEventToolbar.SetValue(this._eventBoxGroupsState.lightRotationEaseType, this._eventBoxGroupsState.lightRotationLoopCount, this._eventBoxGroupsState.lightRotation, this._eventBoxGroupsState.lightRotationDirection, this._eventBoxGroupsState.eventBoxGroupExtension);
          this._lightRotationEventToolbar.SetKeyBindings(this._keyboardBinder);
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          this._lightTranslationEventToolbar.SetValue(this._eventBoxGroupsState.lightTranslationEaseType, this._eventBoxGroupsState.lightTranslation, this._eventBoxGroupsState.eventBoxGroupExtension);
          this._lightTranslationEventToolbar.SetKeyBindings(this._keyboardBinder);
          break;
      }
      this._signalBus.Subscribe<LightColorEventChangedSignal>(new Action(this.HandleLightColorEventChanged));
      this._signalBus.Subscribe<LightRotationEventChangedSignal>(new Action(this.HandleLightRotationEventChanged));
      this._signalBus.Subscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
    }

    protected void OnDisable()
    {
      this._keyboardBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<LightColorEventChangedSignal>(new Action(this.HandleLightColorEventChanged));
      this._signalBus.TryUnsubscribe<LightRotationEventChangedSignal>(new Action(this.HandleLightRotationEventChanged));
      this._signalBus.TryUnsubscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
    }

    protected void Update() => this._keyboardBinder.ManualUpdate();

    private void HandleLightColorEventChanged() => this._lightColorEventToolbar.SetValue(this._eventBoxGroupsState.lightColorType, this._eventBoxGroupsState.lightColorTransitionType, this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightStrobeBeatFrequency, this._eventBoxGroupsState.eventBoxGroupExtension);

    private void HandleLightRotationEventChanged() => this._lightRotationEventToolbar.SetValue(this._eventBoxGroupsState.lightRotationEaseType, this._eventBoxGroupsState.lightRotationLoopCount, this._eventBoxGroupsState.lightRotation, this._eventBoxGroupsState.lightRotationDirection, this._eventBoxGroupsState.eventBoxGroupExtension);

    private void HandleLightTranslationEventChanged() => this._lightTranslationEventToolbar.SetValue(this._eventBoxGroupsState.lightTranslationEaseType, this._eventBoxGroupsState.lightTranslation, this._eventBoxGroupsState.eventBoxGroupExtension);
  }
}
