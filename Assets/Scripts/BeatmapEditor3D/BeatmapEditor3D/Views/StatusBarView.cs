// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.StatusBarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor.Commands;
using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class StatusBarView : BeatmapEditorView
  {
    [SerializeField]
    private TextMeshProUGUI _noteCountText;
    [SerializeField]
    private TextMeshProUGUI _bombCountText;
    [SerializeField]
    private TextMeshProUGUI _obstaclesCountText;
    [SerializeField]
    private TextMeshProUGUI _chainsCountText;
    [SerializeField]
    private TextMeshProUGUI _arcsCountText;
    [SerializeField]
    private TextMeshProUGUI _eventsCountText;
    [SerializeField]
    private TextMeshProUGUI _groupEventsCountText;
    [Space]
    [SerializeField]
    private Slider _playbackSpeedSlider;
    [SerializeField]
    private Button _playbackSpeedResetButton;
    [SerializeField]
    private Slider _noteFeedbackVolumeSlider;
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private Slider _modifyScrollPrecisionSlider;
    [Space]
    [SerializeField]
    private GameObject _noteFeedbackVolumeGameObject;
    [SerializeField]
    private GameObject _modifyScrollPrecisionGameObject;
    [Space]
    [SerializeField]
    private Toggle _zenModeToggle;
    [SerializeField]
    private Slider _playbackScaleSlider;
    [Space]
    [SerializeField]
    private Button _showExtendedSettingsButton;
    [SerializeField]
    private Button _closeExtendedSettingsButton;
    [Space]
    [SerializeField]
    private LayoutElement _settingsPanelLayoutElement;
    [Space]
    [SerializeField]
    private BeatmapEditorExtendedSettingsView _extendedSettingsView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly EditorAudioFeedbackController _editorAudioFeedbackController;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [DoesNotRequireDomainReloadInit]
    private static readonly float[] _modifyScrollPrecisionFloatValues = new float[4]
    {
      0.02f,
      0.5f,
      1f,
      2f
    };

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action<BeatmapEditingModeSwitched>(this.HandleLevelEditorModeSwitched));
      this._signalBus.Subscribe<BeatmapObjectsCountUpdatedSignal>(new Action<BeatmapObjectsCountUpdatedSignal>(this.HandleBeatmapObjectsCountUpdated));
      this._playbackSpeedSlider.onValueChanged.AddListener((UnityAction<float>) (value => this.HandleSliderValueChanged(StatusBarView.SliderType.PlaybackSpeed, value)));
      this._playbackSpeedResetButton.onClick.AddListener(new UnityAction(this.HandlePlaybackSpeedResetPressed));
      this._musicVolumeSlider.onValueChanged.AddListener((UnityAction<float>) (value => this.HandleSliderValueChanged(StatusBarView.SliderType.MusicVolume, value)));
      this._noteFeedbackVolumeSlider.onValueChanged.AddListener((UnityAction<float>) (value => this.HandleSliderValueChanged(StatusBarView.SliderType.NoteFeedbackVolume, value)));
      this._modifyScrollPrecisionSlider.onValueChanged.AddListener((UnityAction<float>) (value => this.HandleSliderValueChanged(StatusBarView.SliderType.ModifyScrollPrecision, value)));
      this._zenModeToggle.onValueChanged.AddListener((UnityAction<bool>) (isOn => this.HandleToggleValueChanged(StatusBarView.ToggleType.ZenMode, isOn)));
      this._playbackScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandlePlaybackSliderValueChanged));
      this._playbackScaleSlider.SetValueWithoutNotify(this._beatmapObjectPlacementHelper.timeToZDistanceScale);
      this._showExtendedSettingsButton.onClick.AddListener(new UnityAction(this.HandleOpenExtraSettingsButtonClicked));
      this._closeExtendedSettingsButton.onClick.AddListener(new UnityAction(this.HandleCloseExtraSettingsButtonClicked));
      this._zenModeToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.zenMode);
      this._modifyScrollPrecisionSlider.SetValueWithoutNotify((float) ((IReadOnlyList<float>) StatusBarView._modifyScrollPrecisionFloatValues).IndexOf<float>(this._eventBoxGroupsState.scrollPrecision));
      this.SetPlaybackSpeed(Mathf.InverseLerp(0.3f, 2f, this._beatmapEditorSettingsDataModel.playbackSpeed));
      this.SetAudioVolumes(this._editorAudioFeedbackController.sfxVolumeInDb, this._songPreviewController.volume);
      this.SetBeatmapData();
      this.SetStatusViewState(this._beatmapState.editingMode);
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action<BeatmapEditingModeSwitched>(this.HandleLevelEditorModeSwitched));
      this._signalBus.TryUnsubscribe<BeatmapObjectsCountUpdatedSignal>(new Action<BeatmapObjectsCountUpdatedSignal>(this.HandleBeatmapObjectsCountUpdated));
      this._playbackSpeedSlider.onValueChanged.RemoveAllListeners();
      this._playbackSpeedResetButton.onClick.RemoveAllListeners();
      this._musicVolumeSlider.onValueChanged.RemoveAllListeners();
      this._noteFeedbackVolumeSlider.onValueChanged.RemoveAllListeners();
      this._modifyScrollPrecisionSlider.onValueChanged.RemoveAllListeners();
      this._zenModeToggle.onValueChanged.RemoveAllListeners();
      this._playbackScaleSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.HandlePlaybackSliderValueChanged));
      this._showExtendedSettingsButton.onClick.RemoveAllListeners();
    }

    private void SetBeatmapData() => this._signalBus.Fire<CalculateBeatmapObjectsCountSignal>();

    private void SetAudioVolumes(float noteFeedbackVolume, float musicVolume)
    {
      this._musicVolumeSlider.value = musicVolume;
      this._noteFeedbackVolumeSlider.value = noteFeedbackVolume;
    }

    private void SetPlaybackSpeed(float playbackSpeed) => this._playbackSpeedSlider.value = playbackSpeed;

    private void SetStatusViewState(BeatmapEditingMode mode)
    {
      if (mode == BeatmapEditingMode.Objects)
      {
        this._noteFeedbackVolumeGameObject.gameObject.SetActive(true);
        this._modifyScrollPrecisionGameObject.gameObject.SetActive(false);
      }
      else
      {
        this._noteFeedbackVolumeGameObject.gameObject.SetActive(false);
        this._modifyScrollPrecisionGameObject.gameObject.SetActive(true);
      }
    }

    private void HandleBeatmapLevelUpdated() => this.SetBeatmapData();

    private void HandleLevelEditorModeSwitched(BeatmapEditingModeSwitched signal) => this.SetStatusViewState(signal.mode);

    private void HandleBeatmapObjectsCountUpdated(BeatmapObjectsCountUpdatedSignal signal)
    {
      this._noteCountText.text = string.Format("{0}", (object) signal.notesCount);
      this._bombCountText.text = string.Format("{0}", (object) signal.bombsCount);
      this._obstaclesCountText.text = string.Format("{0}", (object) signal.obstaclesCount);
      this._chainsCountText.text = string.Format("{0}", (object) signal.chainsCount);
      this._arcsCountText.text = string.Format("{0}", (object) signal.arcsCount);
      this._eventsCountText.text = string.Format("{0}", (object) signal.eventsCount);
      this._groupEventsCountText.text = string.Format("{0}/{1}", (object) signal.groupsCount, (object) signal.groupsEventsCount);
    }

    private void HandleExtendedViewStateChanged(bool isActive)
    {
      this._showExtendedSettingsButton.gameObject.SetActive(!isActive);
      this._closeExtendedSettingsButton.gameObject.SetActive(isActive);
      this._settingsPanelLayoutElement.minHeight = !isActive ? 40f : 80f;
    }

    private void HandlePlaybackSpeedResetPressed() => this._playbackSpeedSlider.value = Mathf.InverseLerp(0.3f, 2f, 1f);

    private void HandleToggleValueChanged(StatusBarView.ToggleType toggleType, bool isOn)
    {
      switch (toggleType)
      {
        case StatusBarView.ToggleType.StaticLights:
          this._signalBus.Fire<ToggleStaticLightSignal>(new ToggleStaticLightSignal()
          {
            staticLightIsOn = isOn
          });
          break;
        case StatusBarView.ToggleType.ZenMode:
          this._signalBus.Fire<ToggleZenModeSignal>(new ToggleZenModeSignal(isOn));
          break;
        case StatusBarView.ToggleType.AutoExposure:
          this._signalBus.Fire<ToggleAutoExposureSignal>(new ToggleAutoExposureSignal(isOn));
          break;
      }
    }

    private void HandlePlaybackSliderValueChanged(float value) => this._signalBus.Fire<ChangeBeatmapTimeScaleSignal>(new ChangeBeatmapTimeScaleSignal(value));

    private void HandleSliderValueChanged(StatusBarView.SliderType sliderType, float value)
    {
      switch (sliderType)
      {
        case StatusBarView.SliderType.TimeScale:
          this._signalBus.Fire<ChangeBeatmapTimeScaleSignal>(new ChangeBeatmapTimeScaleSignal(value));
          break;
        case StatusBarView.SliderType.PlaybackSpeed:
          this._signalBus.Fire<UpdatePlaybackSpeedSignal>(new UpdatePlaybackSpeedSignal(Mathf.Lerp(0.3f, 2f, value)));
          break;
        case StatusBarView.SliderType.MusicVolume:
          this._songPreviewController.SetVolume(value);
          break;
        case StatusBarView.SliderType.NoteFeedbackVolume:
          this._editorAudioFeedbackController.NotesFeedbackVolume(value);
          break;
        case StatusBarView.SliderType.ModifyScrollPrecision:
          this._signalBus.Fire<UpdateModifyScrollPrecisionSignal>(new UpdateModifyScrollPrecisionSignal(StatusBarView._modifyScrollPrecisionFloatValues[(int) value]));
          break;
      }
    }

    private void HandleOpenExtraSettingsButtonClicked()
    {
      this._extendedSettingsView.SetPanelActive(true);
      this.HandleExtendedViewStateChanged(true);
    }

    private void HandleCloseExtraSettingsButtonClicked()
    {
      this._extendedSettingsView.SetPanelActive(false);
      this.HandleExtendedViewStateChanged(false);
    }

    private enum ToggleType
    {
      StaticLights,
      ZenMode,
      AutoExposure,
    }

    private enum SliderType
    {
      TimeScale,
      PlaybackSpeed,
      MusicVolume,
      NoteFeedbackVolume,
      ModifyScrollPrecision,
    }
  }
}
