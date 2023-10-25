// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditBpmGridViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.UI;
using BeatmapEditor3D.Views;
using HMUI;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EditBpmGridViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private TransportControlsView _transportControlsView;
    [SerializeField]
    private BpmCurrentMarkerView _bpmCurrentMarkerView;
    [SerializeField]
    private BpmHoverMarkerView _bpmFullRegionsHoverMarkerView;
    [SerializeField]
    private BpmPreviewMarkerView _bpmPreviewMarkerView;
    [SerializeField]
    private BpmEditorTimingLabelView _bpmEditorTimingLabelView;
    [SerializeField]
    private AudioWaveformView _fullAudioWaveformView;
    [SerializeField]
    private AudioWaveformView _zoomedAudioWaveformView;
    [Inject]
    private readonly BpmEditorSongPreviewController _songPreviewController;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    protected void Update() => this._keyboardBinder.ManualUpdate();

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (!addedToHierarchy)
        return;
      this._fullAudioWaveformView.SetAudioClip(this._songPreviewController.audioClip);
      this._zoomedAudioWaveformView.SetAudioClip(this._songPreviewController.audioClip);
      this._bpmCurrentMarkerView.Setup(0, this._songPreviewController.audioClip.samples, this._bpmEditorState.sample);
      this._bpmPreviewMarkerView.Setup(0, this._songPreviewController.audioClip.samples, this._bpmEditorState.sample);
      this._bpmFullRegionsHoverMarkerView.Setup(0, this._songPreviewController.audioClip.samples, this._bpmEditorState.hoverSample);
      this._bpmEditorTimingLabelView.Setup();
      this._transportControlsView.buttonPressedEvent += new Action<TransportControlsView.ButtonType>(this.HandleTransportControlsViewButtonPressed);
      this._keyboardBinder.AddBinding(KeyCode.Space, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleSpacePlayPausePressed));
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this._transportControlsView.buttonPressedEvent -= new Action<TransportControlsView.ButtonType>(this.HandleTransportControlsViewButtonPressed);
      this._keyboardBinder.ClearBindings();
    }

    private void HandleTransportControlsViewButtonPressed(TransportControlsView.ButtonType type)
    {
      switch (type)
      {
        case TransportControlsView.ButtonType.Stop:
          this.Stop();
          break;
        case TransportControlsView.ButtonType.PlayPause:
          this.PlayPause();
          break;
      }
    }

    private void HandleSpacePlayPausePressed(bool _) => this.PlayPause();

    private void Stop() => this._signalBus.Fire<PlayHeadControlStopSignal>();

    private void PlayPause()
    {
      if (this._songPreviewController.isPlaying)
        this._signalBus.Fire<PlayHeadControlPauseSignal>();
      else
        this._signalBus.Fire<PlayHeadControlPlaySignal>(new PlayHeadControlPlaySignal(this._bpmEditorState.sample));
    }
  }
}
