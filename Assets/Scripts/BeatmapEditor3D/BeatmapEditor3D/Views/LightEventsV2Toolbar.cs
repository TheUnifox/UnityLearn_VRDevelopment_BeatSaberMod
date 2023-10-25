// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightEventsV2Toolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using HMUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class LightEventsV2Toolbar : AbstractBeatmapEditorToolbar
  {
    [Space]
    [SerializeField]
    private TextMeshProUGUI _lightValueText;
    [Space]
    [SerializeField]
    private Button _redButton;
    [SerializeField]
    private Button _blueButton;
    [SerializeField]
    private Button _whiteButton;
    [Space]
    [SerializeField]
    private Button _offButton;
    [SerializeField]
    private Button _on05Button;
    [SerializeField]
    private Button _onButton;
    [SerializeField]
    private Button _fade0Button;
    [SerializeField]
    private Button _fade05Button;
    [SerializeField]
    private Button _fade1Button;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private LightEventsPayload _currentLightsData = new LightEventsPayload();

    public override void SetValue(int value, float intensity, object payload)
    {
      if (!(payload is LightEventsPayload lightEventsPayload))
        lightEventsPayload = new LightEventsPayload();
      this._currentLightsData = lightEventsPayload;
      this._redButton.interactable = this._currentLightsData.color != LightColor.Red;
      this._blueButton.interactable = this._currentLightsData.color != LightColor.Blue;
      this._whiteButton.interactable = this._currentLightsData.color != LightColor.White;
      this._offButton.interactable = this._currentLightsData.type != LightEventType.Off || !Mathf.Approximately(this._currentLightsData.intensity, 0.0f);
      this._on05Button.interactable = this._currentLightsData.type != LightEventType.On || !Mathf.Approximately(this._currentLightsData.intensity, 0.5f);
      this._onButton.interactable = this._currentLightsData.type != LightEventType.On || !Mathf.Approximately(this._currentLightsData.intensity, 1f);
      this._fade0Button.interactable = this._currentLightsData.type != LightEventType.Fade || !Mathf.Approximately(this._currentLightsData.intensity, 0.0f);
      this._fade05Button.interactable = this._currentLightsData.type != LightEventType.Fade || !Mathf.Approximately(this._currentLightsData.intensity, 0.5f);
      this._fade1Button.interactable = this._currentLightsData.type != LightEventType.Fade || !Mathf.Approximately(this._currentLightsData.intensity, 1f);
      this.UpdateLightValueText();
    }

    public override void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.Alpha1, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColor.Red)));
      keyboardBinder.AddBinding(KeyCode.Alpha2, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColor.Blue)));
      keyboardBinder.AddBinding(KeyCode.Alpha3, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColor.White)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeType(LightEventType.Off, 0.0f)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeType(LightEventType.On, 0.5f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeType(LightEventType.On, 1f)));
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeType(LightEventType.Fade, 0.0f)));
      keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeType(LightEventType.Fade, 0.5f)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeType(LightEventType.Fade, 1f)));
      keyboardBinder.AddBinding(KeyCode.Keypad0, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.0f)));
      keyboardBinder.AddBinding(KeyCode.Keypad1, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.1f)));
      keyboardBinder.AddBinding(KeyCode.Keypad2, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.2f)));
      keyboardBinder.AddBinding(KeyCode.Keypad3, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.3f)));
      keyboardBinder.AddBinding(KeyCode.Keypad4, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.4f)));
      keyboardBinder.AddBinding(KeyCode.Keypad5, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.5f)));
      keyboardBinder.AddBinding(KeyCode.Keypad6, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.6f)));
      keyboardBinder.AddBinding(KeyCode.Keypad7, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.7f)));
      keyboardBinder.AddBinding(KeyCode.Keypad8, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.8f)));
      keyboardBinder.AddBinding(KeyCode.Keypad9, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(0.9f)));
      keyboardBinder.AddBinding(KeyCode.KeypadDivide, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(1f)));
      keyboardBinder.AddBinding(KeyCode.KeypadMultiply, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeIntensity(1.2f)));
    }

    protected void OnEnable()
    {
      this._redButton.onClick.AddListener((UnityAction) (() => this.HandleChangeColor(LightColor.Red)));
      this._blueButton.onClick.AddListener((UnityAction) (() => this.HandleChangeColor(LightColor.Blue)));
      this._whiteButton.onClick.AddListener((UnityAction) (() => this.HandleChangeColor(LightColor.White)));
      this._offButton.onClick.AddListener((UnityAction) (() => this.HandleChangeType(LightEventType.Off, 0.0f)));
      this._on05Button.onClick.AddListener((UnityAction) (() => this.HandleChangeType(LightEventType.On, 0.5f)));
      this._onButton.onClick.AddListener((UnityAction) (() => this.HandleChangeType(LightEventType.On, 1f)));
      this._fade0Button.onClick.AddListener((UnityAction) (() => this.HandleChangeType(LightEventType.Fade, 0.0f)));
      this._fade05Button.onClick.AddListener((UnityAction) (() => this.HandleChangeType(LightEventType.Fade, 0.5f)));
      this._fade1Button.onClick.AddListener((UnityAction) (() => this.HandleChangeType(LightEventType.Fade, 1f)));
    }

    protected void OnDisable()
    {
      this._redButton.onClick.RemoveAllListeners();
      this._blueButton.onClick.RemoveAllListeners();
      this._whiteButton.onClick.RemoveAllListeners();
      this._offButton.onClick.RemoveAllListeners();
      this._on05Button.onClick.RemoveAllListeners();
      this._onButton.onClick.RemoveAllListeners();
      this._fade0Button.onClick.RemoveAllListeners();
      this._fade05Button.onClick.RemoveAllListeners();
      this._fade1Button.onClick.RemoveAllListeners();
    }

    private void HandleChangeColor(LightColor color)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._currentLightsData.color = color;
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        this._signalBus.Fire<ModifyHoveredLightEventColorSignal>(new ModifyHoveredLightEventColorSignal(color));
      else
        this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._currentLightsData.ToValue(), this._currentLightsData.ToAltValue(), (object) this._currentLightsData));
    }

    private void HandleChangeType(LightEventType type, float floatValue)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._currentLightsData.type = type;
      this._currentLightsData.intensity = floatValue;
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
      {
        this._signalBus.Fire<ModifyHoveredLightEventTypeSignal>(new ModifyHoveredLightEventTypeSignal(this._currentLightsData.type));
        this._signalBus.Fire<ModifyHoveredLightEventIntensitySignal>(new ModifyHoveredLightEventIntensitySignal(this._currentLightsData.intensity));
      }
      else
        this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._currentLightsData.ToValue(), this._currentLightsData.ToAltValue(), (object) this._currentLightsData));
    }

    private void HandleChangeIntensity(float intensity)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._currentLightsData.intensity = intensity;
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        this._signalBus.Fire<ModifyHoveredLightEventIntensitySignal>(new ModifyHoveredLightEventIntensitySignal(this._currentLightsData.intensity));
      else
        this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._currentLightsData.ToValue(), this._currentLightsData.ToAltValue(), (object) this._currentLightsData));
    }

    private void UpdateLightValueText() => this._lightValueText.text = this._currentLightsData.ToString();
  }
}
