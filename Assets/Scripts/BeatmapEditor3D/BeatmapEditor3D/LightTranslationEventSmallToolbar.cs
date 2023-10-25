// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightTranslationEventSmallToolbar
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
  public class LightTranslationEventSmallToolbar : MonoBehaviour
  {
    [SerializeField]
    private Button _neg1TranslationButton;
    [SerializeField]
    private Button _neg05TranslationButton;
    [SerializeField]
    private Button _0TranslationButton;
    [SerializeField]
    private Button _pos05TranslationButton;
    [SerializeField]
    private Button _pos1TranslationButton;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private float _translation;

    protected void OnEnable()
    {
      this._neg1TranslationButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(-1f)));
      this._neg05TranslationButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(-0.5f)));
      this._0TranslationButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(0.0f)));
      this._pos05TranslationButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(0.5f)));
      this._pos1TranslationButton.onClick.AddListener((UnityAction) (() => this.HandleChangeTranslation(1f)));
    }

    protected void OnDisable()
    {
      this._neg1TranslationButton.onClick.RemoveAllListeners();
      this._neg05TranslationButton.onClick.RemoveAllListeners();
      this._0TranslationButton.onClick.RemoveAllListeners();
      this._pos05TranslationButton.onClick.RemoveAllListeners();
      this._pos1TranslationButton.onClick.RemoveAllListeners();
    }

    public void SetValue(float translation)
    {
      this._neg1TranslationButton.interactable = !Mathf.Approximately(translation, -1f);
      this._neg05TranslationButton.interactable = !Mathf.Approximately(translation, -0.5f);
      this._0TranslationButton.interactable = !Mathf.Approximately(translation, 0.0f);
      this._pos05TranslationButton.interactable = !Mathf.Approximately(translation, 0.5f);
      this._pos1TranslationButton.interactable = !Mathf.Approximately(translation, 1f);
    }

    public void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(-1f)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(-0.5f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(0.0f)));
      keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(0.5f)));
      keyboardBinder.AddBinding(KeyCode.T, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeTranslation(1f)));
    }

    private void HandleChangeTranslation(float translation)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._translation = translation;
      this._signalBus.Fire<ChangeLightTranslationEventSignal>(new ChangeLightTranslationEventSignal(EaseType.InOutQuad, this._translation, false));
    }
  }
}
