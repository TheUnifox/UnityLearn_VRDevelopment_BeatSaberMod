// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightColorEventToolbar
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
  public class LightColorEventToolbar : MonoBehaviour
  {
    private const float kMaxStrobePower = 5f;
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
    [Space]
    [SerializeField]
    private Button _offStrobeButton;
    [SerializeField]
    private Button _2StrobeButton;
    [SerializeField]
    private Button _4StrobeButton;
    [SerializeField]
    private Button _8StrobeButton;
    [SerializeField]
    private Button _16StrobeButton;
    [SerializeField]
    private Button _32StrobeButton;
    [Space]
    [SerializeField]
    private GameObject _extensionToggleWrapper;
    [SerializeField]
    private Toggle _extensionToggle;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private LightColorBaseEditorData.EnvironmentColorType _colorType;
    private LightColorBaseEditorData.TransitionType _transitionType;
    private float _brightness = 1f;
    private int _strobeFrequency;
    private bool _extension;
    private bool _showExtensionToggle;

    public void Init(bool showExtensionToggle)
    {
      this._showExtensionToggle = showExtensionToggle;
      this._extensionToggleWrapper.SetActive(this._showExtensionToggle);
    }

    public void SetValue(
      LightColorBaseEditorData.EnvironmentColorType colorType,
      LightColorBaseEditorData.TransitionType transitionType,
      float brightness,
      int strobeFrequency,
      bool extension)
    {
      this._colorType = colorType;
      this._transitionType = transitionType;
      this._brightness = brightness;
      this._strobeFrequency = strobeFrequency;
      this._extension = extension;
      this._redButton.interactable = colorType != 0;
      this._blueButton.interactable = colorType != LightColorBaseEditorData.EnvironmentColorType.Color1;
      this._whiteButton.interactable = colorType != LightColorBaseEditorData.EnvironmentColorType.ColorW;
      this._offButton.interactable = transitionType != LightColorBaseEditorData.TransitionType.Instant || !Mathf.Approximately(brightness, 0.0f);
      this._on05Button.interactable = transitionType != LightColorBaseEditorData.TransitionType.Instant || !Mathf.Approximately(brightness, 0.5f);
      this._onButton.interactable = transitionType != LightColorBaseEditorData.TransitionType.Instant || !Mathf.Approximately(brightness, 1f);
      this._fade0Button.interactable = transitionType != LightColorBaseEditorData.TransitionType.Interpolate || !Mathf.Approximately(brightness, 0.0f);
      this._fade05Button.interactable = transitionType != LightColorBaseEditorData.TransitionType.Interpolate || !Mathf.Approximately(brightness, 0.5f);
      this._fade1Button.interactable = transitionType != LightColorBaseEditorData.TransitionType.Interpolate || !Mathf.Approximately(brightness, 1f);
      this._offStrobeButton.interactable = strobeFrequency != 0;
      this._2StrobeButton.interactable = strobeFrequency != 2;
      this._4StrobeButton.interactable = strobeFrequency != 4;
      this._8StrobeButton.interactable = strobeFrequency != 8;
      this._16StrobeButton.interactable = strobeFrequency != 16;
      this._32StrobeButton.interactable = strobeFrequency != 32;
      if (!this._showExtensionToggle)
        return;
      this._extensionToggle.SetIsOnWithoutNotify(this._extension);
    }

    public void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.Alpha1, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColorBaseEditorData.EnvironmentColorType.Color0)));
      keyboardBinder.AddBinding(KeyCode.Alpha2, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColorBaseEditorData.EnvironmentColorType.Color1)));
      keyboardBinder.AddBinding(KeyCode.Alpha3, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColorBaseEditorData.EnvironmentColorType.ColorW)));
      if (this._showExtensionToggle)
        keyboardBinder.AddBinding(KeyCode.Alpha4, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleEnableExtension(true)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Instant, 0.0f)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Instant, 0.5f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Instant, 1f)));
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Interpolate, 0.0f)));
      keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Interpolate, 0.5f)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Interpolate, 1f)));
      keyboardBinder.AddBinding(KeyCode.Keypad0, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.0f)));
      keyboardBinder.AddBinding(KeyCode.Keypad1, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.1f)));
      keyboardBinder.AddBinding(KeyCode.Keypad2, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.2f)));
      keyboardBinder.AddBinding(KeyCode.Keypad3, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.3f)));
      keyboardBinder.AddBinding(KeyCode.Keypad4, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.4f)));
      keyboardBinder.AddBinding(KeyCode.Keypad5, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.5f)));
      keyboardBinder.AddBinding(KeyCode.Keypad6, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.6f)));
      keyboardBinder.AddBinding(KeyCode.Keypad7, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.7f)));
      keyboardBinder.AddBinding(KeyCode.Keypad8, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.8f)));
      keyboardBinder.AddBinding(KeyCode.Keypad9, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(0.9f)));
      keyboardBinder.AddBinding(KeyCode.KeypadDivide, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(1f)));
      keyboardBinder.AddBinding(KeyCode.KeypadMultiply, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(1.2f)));
      keyboardBinder.AddBinding(KeyCode.KeypadMinus, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeBrightness(1.5f)));
      keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeStrobeFrequency(0)));
      keyboardBinder.AddBinding(KeyCode.F, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleIncreaseStrobeFrequency()));
    }

    protected void OnEnable()
    {
      this._redButton.onClick.AddListener((UnityAction) (() => this.HandleChangeColor(LightColorBaseEditorData.EnvironmentColorType.Color0)));
      this._blueButton.onClick.AddListener((UnityAction) (() => this.HandleChangeColor(LightColorBaseEditorData.EnvironmentColorType.Color1)));
      this._whiteButton.onClick.AddListener((UnityAction) (() => this.HandleChangeColor(LightColorBaseEditorData.EnvironmentColorType.ColorW)));
      this._offButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Instant, 0.0f)));
      this._on05Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Instant, 0.5f)));
      this._onButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Instant, 1f)));
      this._fade0Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Interpolate, 0.0f)));
      this._fade05Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Interpolate, 0.5f)));
      this._fade1Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTransitionTypeAndBrightness(LightColorBaseEditorData.TransitionType.Interpolate, 1f)));
      this._offStrobeButton.onClick.AddListener((UnityAction) (() => this.HandleChangeStrobeFrequency(0)));
      this._2StrobeButton.onClick.AddListener((UnityAction) (() => this.HandleChangeStrobeFrequency(2)));
      this._4StrobeButton.onClick.AddListener((UnityAction) (() => this.HandleChangeStrobeFrequency(4)));
      this._8StrobeButton.onClick.AddListener((UnityAction) (() => this.HandleChangeStrobeFrequency(8)));
      this._16StrobeButton.onClick.AddListener((UnityAction) (() => this.HandleChangeStrobeFrequency(16)));
      this._32StrobeButton.onClick.AddListener((UnityAction) (() => this.HandleChangeStrobeFrequency(32)));
      if (!this._showExtensionToggle)
        return;
      this._extensionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleEnableExtension));
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
      this._offStrobeButton.onClick.RemoveAllListeners();
      this._2StrobeButton.onClick.RemoveAllListeners();
      this._4StrobeButton.onClick.RemoveAllListeners();
      this._8StrobeButton.onClick.RemoveAllListeners();
      this._16StrobeButton.onClick.RemoveAllListeners();
      this._32StrobeButton.onClick.RemoveAllListeners();
      if (!this._showExtensionToggle)
        return;
      this._extensionToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleEnableExtension));
    }

    private void HandleChangeColor(
      LightColorBaseEditorData.EnvironmentColorType colorType)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._colorType = colorType;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightColorSignal>(new ModifyHoveredLightColorSignal(this._colorType));
      this._signalBus.Fire<ChangeLightColorEventSignal>(new ChangeLightColorEventSignal(this._colorType, this._transitionType, this._brightness, this._strobeFrequency, false));
    }

    private void HandleEnableExtension(bool isOn)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._extension = true;
      this._signalBus.Fire<ChangeLightColorEventSignal>(new ChangeLightColorEventSignal(this._colorType, this._transitionType, this._brightness, this._strobeFrequency, isOn));
    }

    private void HandleChangeTransitionTypeAndBrightness(
      LightColorBaseEditorData.TransitionType transitionType,
      float brightness)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._transitionType = transitionType;
      this._brightness = brightness;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightColorTransitionTypeSignal>(new ModifyHoveredLightColorTransitionTypeSignal(this._transitionType, this._brightness));
      this._signalBus.Fire<ChangeLightColorEventSignal>(new ChangeLightColorEventSignal(this._colorType, this._transitionType, this._brightness, this._strobeFrequency, false));
    }

    private void HandleChangeBrightness(float brightness)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._brightness = brightness;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightColorBrightnessSignal>(new ModifyHoveredLightColorBrightnessSignal(this._brightness));
      this._signalBus.Fire<ChangeLightColorEventSignal>(new ChangeLightColorEventSignal(this._colorType, this._transitionType, this._brightness, this._strobeFrequency, false));
    }

    private void HandleChangeStrobeFrequency(int strobeFrequency)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._strobeFrequency = strobeFrequency;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightColorStrobeFrequencySignal>(new ModifyHoveredLightColorStrobeFrequencySignal(this._strobeFrequency));
      this._signalBus.Fire<ChangeLightColorEventSignal>(new ChangeLightColorEventSignal(this._colorType, this._transitionType, this._brightness, this._strobeFrequency, false));
    }

    private void HandleIncreaseStrobeFrequency()
    {
      if (this._beatmapState.cameraMoving)
        return;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightColorCycleStrobeFrequencySignal>(new ModifyHoveredLightColorCycleStrobeFrequencySignal());
      int f = Mathf.ClosestPowerOfTwo(this._eventBoxGroupsState.lightStrobeBeatFrequency);
      this._strobeFrequency = (int) Mathf.Pow(2f, (float) (((f != 0 ? (double) Mathf.Log((float) f, 2f) : 0.0) + 1.0) % 5.0));
      this._signalBus.Fire<ChangeLightColorEventSignal>(new ChangeLightColorEventSignal(this._colorType, this._transitionType, this._brightness, this._strobeFrequency, false));
    }
  }
}
