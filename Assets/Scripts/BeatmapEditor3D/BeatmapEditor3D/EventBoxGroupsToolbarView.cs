// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupsToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventBoxGroupsToolbarView : MonoBehaviour
  {
    [SerializeField]
    private LightColorEventToolbar _lightColorEventToolbar;
    [SerializeField]
    private LightRotationEventSmallToolbar _lightRotationEventSmallToolbar;
    [SerializeField]
    private LightTranslationEventSmallToolbar _lightTranslationEventSmallToolbar;
    [SerializeField]
    private Toggle _extensionToggle;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    protected void OnEnable()
    {
      this._lightColorEventToolbar.Init(false);
      this._lightColorEventToolbar.SetValue(this._eventBoxGroupsState.lightColorType, this._eventBoxGroupsState.lightColorTransitionType, this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightStrobeBeatFrequency, this._eventBoxGroupsState.eventBoxGroupExtension);
      this._lightRotationEventSmallToolbar.SetValue(this._eventBoxGroupsState.lightRotation);
      this._lightTranslationEventSmallToolbar.SetValue(this._eventBoxGroupsState.lightTranslation);
      this._lightColorEventToolbar.SetKeyBindings(this._keyboardBinder);
      this._lightRotationEventSmallToolbar.SetKeyBindings(this._keyboardBinder);
      this._lightTranslationEventSmallToolbar.SetKeyBindings(this._keyboardBinder);
      this._keyboardBinder.AddBinding(KeyCode.Alpha4, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleEnableExtension(true)));
      this._extensionToggle.SetIsOnWithoutNotify(this._eventBoxGroupsState.eventBoxGroupExtension);
      this._extensionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleEnableExtension));
      this._signalBus.Subscribe<LightColorEventChangedSignal>(new Action(this.HandleLightColorEventChanged));
      this._signalBus.Subscribe<LightRotationEventChangedSignal>(new Action(this.HandleLightRotationEventChanged));
      this._signalBus.Subscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
      this._signalBus.Subscribe<EventBoxGroupExtensionChangedSignal>(new Action(this.HandleEventBoxGroupExtensionChanged));
    }

    protected void OnDisable()
    {
      this._keyboardBinder.ClearBindings();
      this._extensionToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleEnableExtension));
      this._signalBus.TryUnsubscribe<LightColorEventChangedSignal>(new Action(this.HandleLightColorEventChanged));
      this._signalBus.TryUnsubscribe<LightRotationEventChangedSignal>(new Action(this.HandleLightRotationEventChanged));
      this._signalBus.TryUnsubscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
      this._signalBus.TryUnsubscribe<EventBoxGroupExtensionChangedSignal>(new Action(this.HandleEventBoxGroupExtensionChanged));
    }

    private void HandleEnableExtension(bool isOn)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeEventBoxGroupExtensionSignal>(new ChangeEventBoxGroupExtensionSignal());
    }

    protected void Update() => this._keyboardBinder.ManualUpdate();

    private void HandleLightColorEventChanged() => this._lightColorEventToolbar.SetValue(this._eventBoxGroupsState.lightColorType, this._eventBoxGroupsState.lightColorTransitionType, this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightStrobeBeatFrequency, this._eventBoxGroupsState.eventBoxGroupExtension);

    private void HandleLightRotationEventChanged() => this._lightRotationEventSmallToolbar.SetValue(this._eventBoxGroupsState.lightRotation);

    private void HandleLightTranslationEventChanged() => this._lightTranslationEventSmallToolbar.SetValue(this._eventBoxGroupsState.lightTranslation);

    private void HandleEventBoxGroupExtensionChanged() => this._extensionToggle.SetIsOnWithoutNotify(this._eventBoxGroupsState.eventBoxGroupExtension);
  }
}
