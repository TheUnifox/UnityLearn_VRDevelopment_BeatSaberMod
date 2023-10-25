// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmEditorPlayBackControlsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmEditorPlayBackControlsView : BeatmapEditorView
  {
    [SerializeField]
    private Slider _zoomSlider;
    [SerializeField]
    private Slider _playbackSpeedSlider;
    [SerializeField]
    private Button _playbackSpeedResetButton;
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private Button _musicVolumeResetButton;
    [SerializeField]
    private Slider _sfxVolumeSlider;
    [SerializeField]
    private Button _sfxVolumeResetButton;
    [SerializeField]
    private Button _normalSubdivisionButton;
    [SerializeField]
    private Button _tripletSubdivisionButton;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected override void DidActivate()
    {
      this._playbackSpeedSlider.minValue = 0.1f;
      this._playbackSpeedSlider.maxValue = 2f;
      this._musicVolumeSlider.minValue = 0.0f;
      this._musicVolumeSlider.maxValue = 2f;
      this._sfxVolumeSlider.minValue = 0.0f;
      this._sfxVolumeSlider.maxValue = 2f;
      this._normalSubdivisionButton.interactable = this._bpmEditorState.bpmSubdivisionType != 0;
      this._tripletSubdivisionButton.interactable = this._bpmEditorState.bpmSubdivisionType != BpmSubdivisionType.Triplet;
      this.SetZoom();
      this.SetPlaybackSpeed();
      this.SetMusicVolume();
      this.SetMetronomeVolume();
      this.SetSubdivision();
      this._zoomSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandleZoomSliderOnValueChanged));
      this._playbackSpeedSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandlePlaybackSpeedSliderOnValueChanged));
      this._musicVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandleMusicVolumeSliderOnValueChanged));
      this._sfxVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandleSfxVolumeSliderOnValueChanged));
      this._playbackSpeedResetButton.onClick.AddListener(new UnityAction(this.HandlePlaybackSpeedReset));
      this._musicVolumeResetButton.onClick.AddListener(new UnityAction(this.HandleMusicVolumeReset));
      this._sfxVolumeResetButton.onClick.AddListener(new UnityAction(this.HandleSfxVolumeReset));
      this._normalSubdivisionButton.onClick.AddListener(new UnityAction(this.HandleNormalSubdivisionChangedButtonOnClick));
      this._tripletSubdivisionButton.onClick.AddListener(new UnityAction(this.HandleTripletSubdivisionChangedButtonOnClick));
      this._signalBus.Subscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    protected override void DidDeactivate()
    {
      this._zoomSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.HandleZoomSliderOnValueChanged));
      this._playbackSpeedSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.HandlePlaybackSpeedSliderOnValueChanged));
      this._musicVolumeSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.HandleMusicVolumeSliderOnValueChanged));
      this._sfxVolumeSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.HandleSfxVolumeSliderOnValueChanged));
      this._playbackSpeedResetButton.onClick.RemoveListener(new UnityAction(this.HandlePlaybackSpeedReset));
      this._musicVolumeResetButton.onClick.RemoveListener(new UnityAction(this.HandleMusicVolumeReset));
      this._sfxVolumeResetButton.onClick.RemoveListener(new UnityAction(this.HandleSfxVolumeReset));
      this._normalSubdivisionButton.onClick.RemoveListener(new UnityAction(this.HandleNormalSubdivisionChangedButtonOnClick));
      this._tripletSubdivisionButton.onClick.RemoveListener(new UnityAction(this.HandleTripletSubdivisionChangedButtonOnClick));
      this._signalBus.TryUnsubscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    private void HandleZoomSliderOnValueChanged(float t) => this._signalBus.Fire<PlayHeadZoomSignal>(new PlayHeadZoomSignal((int) Mathf.Lerp(5000f, 250000f, t) - this._bpmEditorState.previewHalfSize));

    private void HandlePlaybackSpeedSliderOnValueChanged(float t) => this._signalBus.Fire<BpmPlaybackChangeSpeedSignal>(new BpmPlaybackChangeSpeedSignal(t));

    private void HandleMusicVolumeSliderOnValueChanged(float t) => this._signalBus.Fire<BpmPlaybackChangeMusicVolumeSignal>(new BpmPlaybackChangeMusicVolumeSignal(t));

    private void HandleSfxVolumeSliderOnValueChanged(float t) => this._signalBus.Fire<BpmPlaybackChangeMetronomeVolumeSignal>(new BpmPlaybackChangeMetronomeVolumeSignal(t));

    private void HandlePlaybackSpeedReset()
    {
      this._signalBus.Fire<BpmPlaybackChangeSpeedSignal>(new BpmPlaybackChangeSpeedSignal(1f));
      this.SetPlaybackSpeed();
    }

    private void HandleMusicVolumeReset()
    {
      this._signalBus.Fire<BpmPlaybackChangeMusicVolumeSignal>(new BpmPlaybackChangeMusicVolumeSignal(1f));
      this.SetMusicVolume();
    }

    private void HandleSfxVolumeReset()
    {
      this._signalBus.Fire<BpmPlaybackChangeMetronomeVolumeSignal>(new BpmPlaybackChangeMetronomeVolumeSignal(1f));
      this.SetMetronomeVolume();
    }

    private void HandleNormalSubdivisionChangedButtonOnClick()
    {
      this._signalBus.Fire<SwitchBpmSubdivisionSignal>(new SwitchBpmSubdivisionSignal(BpmSubdivisionType.Normal));
      this.SetSubdivision();
    }

    private void HandleTripletSubdivisionChangedButtonOnClick()
    {
      this._signalBus.Fire<SwitchBpmSubdivisionSignal>(new SwitchBpmSubdivisionSignal(BpmSubdivisionType.Triplet));
      this.SetSubdivision();
    }

    private void HandlePlayHeadZoomed() => this.SetZoom();

    private void SetZoom() => this._zoomSlider.SetValueWithoutNotify(Mathf.InverseLerp(5000f, 250000f, (float) this._bpmEditorState.previewHalfSize));

    private void SetPlaybackSpeed() => this._playbackSpeedSlider.SetValueWithoutNotify(this._bpmEditorState.playbackSpeed);

    private void SetMusicVolume() => this._musicVolumeSlider.SetValueWithoutNotify(this._bpmEditorState.musicVolume);

    private void SetMetronomeVolume() => this._sfxVolumeSlider.SetValueWithoutNotify(this._bpmEditorState.metronomeVolume);

    private void SetSubdivision()
    {
      this._normalSubdivisionButton.interactable = this._bpmEditorState.bpmSubdivisionType != 0;
      this._tripletSubdivisionButton.interactable = this._bpmEditorState.bpmSubdivisionType != BpmSubdivisionType.Triplet;
    }
  }
}
