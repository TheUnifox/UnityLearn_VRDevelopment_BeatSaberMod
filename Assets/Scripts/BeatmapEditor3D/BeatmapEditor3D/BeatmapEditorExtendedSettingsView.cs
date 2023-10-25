// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorExtendedSettingsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorExtendedSettingsView : MonoBehaviour
  {
    [SerializeField]
    private Toggle _staticLightsToggle;
    [SerializeField]
    private Toggle _autoExposureToggle;
    [SerializeField]
    private Toggle _returnToTimeToggle;
    [SerializeField]
    private Button _copyDifficultyButton;
    [SerializeField]
    private BeatmapEditorToggleGroupView _toggleGroup;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    public event Action copyBeatmapButtonClicked;

    protected void OnEnable()
    {
      this._staticLightsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleStaticLightsToggleValueChanged));
      this._staticLightsToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.staticLights);
      this._autoExposureToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleAutoExposureToggleValueChanged));
      this._autoExposureToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.autoExposure);
      this._returnToTimeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleReturnToTimeToggleValueChanged));
      this._returnToTimeToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.preserveTime);
      this._copyDifficultyButton.onClick.AddListener((UnityAction) (() =>
      {
        Action beatmapButtonClicked = this.copyBeatmapButtonClicked;
        if (beatmapButtonClicked == null)
          return;
        beatmapButtonClicked();
      }));
      this._toggleGroup.SetValueWithoutNotify((int) this._beatmapEditorSettingsDataModel.gameplayUIState);
      this._toggleGroup.onValueChanged += new Action<int>(this.HandleToggleValueChanged);
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action<BeatmapEditingModeSwitched>(this.HandleBeatmapEditingModeSwitched));
      this._signalBus.Subscribe<GameplayUIStateChangedSignal>(new Action<GameplayUIStateChangedSignal>(this.HandleGameplayUIStateChanged));
    }

    protected void OnDisable()
    {
      this._autoExposureToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleAutoExposureToggleValueChanged));
      this._staticLightsToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleStaticLightsToggleValueChanged));
      this._returnToTimeToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleReturnToTimeToggleValueChanged));
      this._copyDifficultyButton.onClick.RemoveAllListeners();
      this._toggleGroup.onValueChanged -= new Action<int>(this.HandleToggleValueChanged);
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action<BeatmapEditingModeSwitched>(this.HandleBeatmapEditingModeSwitched));
      this._signalBus.TryUnsubscribe<GameplayUIStateChangedSignal>(new Action<GameplayUIStateChangedSignal>(this.HandleGameplayUIStateChanged));
    }

    public void SetPanelActive(bool isActive) => this.gameObject.SetActive(isActive);

    private void HandleAutoExposureToggleValueChanged(bool isOn) => this._signalBus.Fire<ToggleAutoExposureSignal>(new ToggleAutoExposureSignal(isOn));

    private void HandleStaticLightsToggleValueChanged(bool isOn) => this._signalBus.Fire<ToggleStaticLightSignal>(new ToggleStaticLightSignal()
    {
      staticLightIsOn = isOn
    });

    private void HandleReturnToTimeToggleValueChanged(bool isOn) => this._signalBus.Fire<TogglePreserveTimeSignal>(new TogglePreserveTimeSignal(isOn));

    private void HandleBeatmapEditingModeSwitched(BeatmapEditingModeSwitched signal)
    {
      this._staticLightsToggle.gameObject.SetActive(signal.mode == BeatmapEditingMode.Objects);
      this._staticLightsToggle.interactable = signal.mode == BeatmapEditingMode.Objects;
    }

    private void HandleGameplayUIStateChanged(GameplayUIStateChangedSignal signal) => this._toggleGroup.value = (int) signal.state;

    private void HandleToggleValueChanged(int newValue) => this._signalBus.Fire<ChangeGameplayUIStateSignal>(new ChangeGameplayUIStateSignal((BeatmapEditor3D.Types.GameplayUIState) newValue));
  }
}
