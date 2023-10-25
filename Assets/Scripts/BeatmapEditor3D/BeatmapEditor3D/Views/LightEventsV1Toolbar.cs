// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightEventsV1Toolbar
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
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class LightEventsV1Toolbar : AbstractBeatmapEditorToolbar
  {
    [Space]
    [SerializeField]
    private TextMeshProUGUI _lightValueText;
    [Space]
    [SerializeField]
    private Button _redButton;
    [SerializeField]
    private Button _blueButton;
    [Space]
    [SerializeField]
    private Button _offButton;
    [SerializeField]
    private Button _onButton;
    [SerializeField]
    private Button _fadeOutButton;
    [SerializeField]
    private Button _flashFadeButton;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private LightEventsPayload _currentLightsData = new LightEventsPayload();

    public override void SetValue(int value, float floatValue, object payload)
    {
      if (!(payload is LightEventsPayload lightEventsPayload))
        lightEventsPayload = new LightEventsPayload();
      this._currentLightsData = lightEventsPayload;
      this._redButton.interactable = this._currentLightsData.color != LightColor.Red;
      this._blueButton.interactable = this._currentLightsData.color != LightColor.Blue;
      this._offButton.interactable = this._currentLightsData.type != LightEventType.Off;
      this._onButton.interactable = this._currentLightsData.type != 0;
      this._fadeOutButton.interactable = this._currentLightsData.type != LightEventType.FadeOut;
      this._flashFadeButton.interactable = this._currentLightsData.type != LightEventType.Flash;
      this.UpdateLightValueText();
    }

    public override void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.Alpha1, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColor.Red)));
      keyboardBinder.AddBinding(KeyCode.Alpha2, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeColor(LightColor.Blue)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeType(LightEventType.On)));
      keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeType(LightEventType.Off)));
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeType(LightEventType.FadeOut)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeType(LightEventType.Flash)));
    }

    private void HandleChangeColor(LightColor color)
    {
      this._currentLightsData.color = color;
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        this._signalBus.Fire<ModifyHoveredLightEventColorSignal>(new ModifyHoveredLightEventColorSignal(color));
      else
        this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._currentLightsData.ToValue(), this._currentLightsData.ToAltValue(), (object) this._currentLightsData));
    }

    private void HandleChangeType(LightEventType type)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._currentLightsData.type = type;
      this._currentLightsData.intensity = 1f;
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
      {
        this._signalBus.Fire<ModifyHoveredLightEventTypeSignal>(new ModifyHoveredLightEventTypeSignal(this._currentLightsData.type));
        this._signalBus.Fire<ModifyHoveredLightEventIntensitySignal>(new ModifyHoveredLightEventIntensitySignal(this._currentLightsData.intensity));
      }
      else
        this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._currentLightsData.ToValue(), this._currentLightsData.ToAltValue(), (object) this._currentLightsData));
    }

    private void UpdateLightValueText() => this._lightValueText.text = this._currentLightsData.ToString();
  }
}
