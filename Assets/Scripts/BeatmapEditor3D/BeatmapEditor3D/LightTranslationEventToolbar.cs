// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightTranslationEventToolbar
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
  public class LightTranslationEventToolbar : MonoBehaviour
  {
    [SerializeField]
    private Button _noneEasingButton;
    [SerializeField]
    private Button _linearEasingButton;
    [SerializeField]
    private Button _inQuadEasingButton;
    [SerializeField]
    private Button _outQuadEasingButton;
    [SerializeField]
    private Button _inOutQuadEasingButton;
    [Space]
    [SerializeField]
    private Button _centerButton;
    [SerializeField]
    private Button _neg1Button;
    [SerializeField]
    private Button _pos1Button;
    [SerializeField]
    private Button _neg075Button;
    [SerializeField]
    private Button _pos075Button;
    [SerializeField]
    private Button _neg05Button;
    [SerializeField]
    private Button _pos05Button;
    [SerializeField]
    private Button _neg025Button;
    [SerializeField]
    private Button _pos025Button;
    [Space]
    [SerializeField]
    private Toggle _extensionToggle;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private EaseType _easeType;
    private float _translation;
    private bool _extension;

    protected void OnEnable()
    {
      this._noneEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.None)));
      this._linearEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.Linear)));
      this._inQuadEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.InQuad)));
      this._outQuadEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.OutQuad)));
      this._inOutQuadEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.InOutQuad)));
      this._centerButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(0.0f)));
      this._neg1Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(-1f)));
      this._pos1Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(1f)));
      this._neg075Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(-0.75f)));
      this._pos075Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(0.75f)));
      this._neg05Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(-0.5f)));
      this._pos05Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(0.5f)));
      this._neg025Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(-0.25f)));
      this._pos025Button.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(0.25f)));
      this._extensionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleEnableExtension));
    }

    protected void OnDisable()
    {
      this._noneEasingButton.onClick.RemoveAllListeners();
      this._linearEasingButton.onClick.RemoveAllListeners();
      this._inQuadEasingButton.onClick.RemoveAllListeners();
      this._outQuadEasingButton.onClick.RemoveAllListeners();
      this._inOutQuadEasingButton.onClick.RemoveAllListeners();
      this._centerButton.onClick.RemoveAllListeners();
      this._neg1Button.onClick.RemoveAllListeners();
      this._pos1Button.onClick.RemoveAllListeners();
      this._neg075Button.onClick.RemoveAllListeners();
      this._pos075Button.onClick.RemoveAllListeners();
      this._neg05Button.onClick.RemoveAllListeners();
      this._pos05Button.onClick.RemoveAllListeners();
      this._neg025Button.onClick.RemoveAllListeners();
      this._pos025Button.onClick.RemoveAllListeners();
      this._extensionToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleEnableExtension));
    }

    public void SetValue(EaseType easeType, float translation, bool extension)
    {
      this._easeType = easeType;
      this._translation = translation;
      this._extension = extension;
      this._noneEasingButton.interactable = easeType != 0;
      this._linearEasingButton.interactable = easeType != EaseType.Linear;
      this._inQuadEasingButton.interactable = easeType != EaseType.InQuad;
      this._outQuadEasingButton.interactable = easeType != EaseType.OutQuad;
      this._inOutQuadEasingButton.interactable = easeType != EaseType.InOutQuad;
      this._centerButton.interactable = !Mathf.Approximately(translation, 0.0f);
      this._neg1Button.interactable = !Mathf.Approximately(translation, -1f);
      this._pos1Button.interactable = !Mathf.Approximately(translation, 1f);
      this._neg075Button.interactable = !Mathf.Approximately(translation, -0.75f);
      this._pos075Button.interactable = !Mathf.Approximately(translation, 0.75f);
      this._neg05Button.interactable = !Mathf.Approximately(translation, -0.5f);
      this._pos05Button.interactable = !Mathf.Approximately(translation, 0.5f);
      this._neg025Button.interactable = !Mathf.Approximately(translation, -0.25f);
      this._pos025Button.interactable = !Mathf.Approximately(translation, 0.25f);
      this._extensionToggle.SetIsOnWithoutNotify(this._extension);
    }

    public void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.G, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeEasing(EaseType.None)));
      keyboardBinder.AddBinding(KeyCode.B, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeEasing(EaseType.Linear)));
      keyboardBinder.AddBinding(KeyCode.V, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleToggleQuadEasing()));
      keyboardBinder.AddBinding(KeyCode.F, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeEasing(EaseType.InOutQuad)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(-1f)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(-0.5f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(0.0f)));
      keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(0.5f)));
      keyboardBinder.AddBinding(KeyCode.T, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(1f)));
      keyboardBinder.AddBinding(KeyCode.Alpha4, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleEnableExtension(true)));
    }

    private void HandleChangeEasing(EaseType easeType)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._easeType = easeType;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightTranslationEaseTypeSignal>(new ModifyHoveredLightTranslationEaseTypeSignal(easeType));
      this._signalBus.Fire<ChangeLightTranslationEventSignal>(new ChangeLightTranslationEventSignal(this._easeType, this._translation, false));
    }

    private void HandleToggleQuadEasing()
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._easeType = this._easeType != EaseType.InQuad ? EaseType.InQuad : EaseType.OutQuad;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightTranslationEaseTypeSignal>(new ModifyHoveredLightTranslationEaseTypeSignal(this._easeType));
      this._signalBus.Fire<ChangeLightTranslationEventSignal>(new ChangeLightTranslationEventSignal(this._easeType, this._translation, false));
    }

    private void HandleChangeTranslation(float translation)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._translation = translation;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightTranslationTranslationSignal>(new ModifyHoveredLightTranslationTranslationSignal(this._translation));
      this._signalBus.Fire<ChangeLightTranslationEventSignal>(new ChangeLightTranslationEventSignal(this._easeType, this._translation, false));
    }

    private void HandleEnableExtension(bool isOn)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._extension = true;
      this._signalBus.Fire<ChangeLightTranslationEventSignal>(new ChangeLightTranslationEventSignal(this._easeType, this._translation, isOn));
    }
  }
}
