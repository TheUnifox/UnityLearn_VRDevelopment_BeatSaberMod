// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightEventsToolsGroup
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class LightEventsToolsGroup : MonoBehaviour
  {
    [SerializeField]
    private Button _redButton;
    [SerializeField]
    private Button _blueButton;
    [Space]
    [SerializeField]
    private Button _onButton;
    [SerializeField]
    private Button _offButton;
    [SerializeField]
    private Button _flashButton;
    [SerializeField]
    private Button _fadeOutButton;
    [Space]
    [SerializeField]
    private Button _strobeQuickButton;
    [SerializeField]
    private Button _strobeFaintButton;
    [Space]
    [SerializeField]
    private TMP_InputField _customValueInputField;

    public event Action<LightColor> colorValueChangedEvent;

    public event Action<LightEventType> typeValueChangedEvent;

    public event Action<int> customValueInputFieldChangedEvent;

    public void SetLightColorValue(LightColor color)
    {
      this._redButton.interactable = color != LightColor.Red;
      this._blueButton.interactable = color != LightColor.Blue;
    }

    public void SetLightEventValue(LightEventType type)
    {
      this._onButton.interactable = type != 0;
      this._offButton.interactable = type != LightEventType.Off;
      this._flashButton.interactable = type != LightEventType.Flash;
      this._fadeOutButton.interactable = type != LightEventType.FadeOut;
    }

    protected void OnEnable()
    {
      this._redButton.onClick.AddListener((UnityAction) (() =>
      {
        Action<LightColor> valueChangedEvent = this.colorValueChangedEvent;
        if (valueChangedEvent == null)
          return;
        valueChangedEvent(LightColor.Red);
      }));
      this._blueButton.onClick.AddListener((UnityAction) (() =>
      {
        Action<LightColor> valueChangedEvent = this.colorValueChangedEvent;
        if (valueChangedEvent == null)
          return;
        valueChangedEvent(LightColor.Blue);
      }));
      this._onButton.onClick.AddListener((UnityAction) (() =>
      {
        Action<LightEventType> valueChangedEvent = this.typeValueChangedEvent;
        if (valueChangedEvent == null)
          return;
        valueChangedEvent(LightEventType.On);
      }));
      this._offButton.onClick.AddListener((UnityAction) (() =>
      {
        Action<LightEventType> valueChangedEvent = this.typeValueChangedEvent;
        if (valueChangedEvent == null)
          return;
        valueChangedEvent(LightEventType.Off);
      }));
      this._flashButton.onClick.AddListener((UnityAction) (() =>
      {
        Action<LightEventType> valueChangedEvent = this.typeValueChangedEvent;
        if (valueChangedEvent == null)
          return;
        valueChangedEvent(LightEventType.Flash);
      }));
      this._fadeOutButton.onClick.AddListener((UnityAction) (() =>
      {
        Action<LightEventType> valueChangedEvent = this.typeValueChangedEvent;
        if (valueChangedEvent == null)
          return;
        valueChangedEvent(LightEventType.FadeOut);
      }));
      this._customValueInputField.onEndEdit.AddListener((UnityAction<string>) (text =>
      {
        Action<int> fieldChangedEvent = this.customValueInputFieldChangedEvent;
        if (fieldChangedEvent == null)
          return;
        int result;
        fieldChangedEvent(int.TryParse(text, out result) ? result : 0);
      }));
    }

    protected void OnDisable()
    {
      this._redButton.onClick.RemoveAllListeners();
      this._blueButton.onClick.RemoveAllListeners();
      this._onButton.onClick.RemoveAllListeners();
      this._offButton.onClick.RemoveAllListeners();
      this._flashButton.onClick.RemoveAllListeners();
      this._fadeOutButton.onClick.RemoveAllListeners();
      this._strobeFaintButton.onClick.RemoveAllListeners();
      this._strobeQuickButton.onClick.RemoveAllListeners();
      this._customValueInputField.onEndEdit.RemoveAllListeners();
    }
  }
}
