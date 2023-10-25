// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.TransportControlsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class TransportControlsView : MonoBehaviour
  {
    [SerializeField]
    private Button _rewindButton;
    [SerializeField]
    private Button _stopButton;
    [SerializeField]
    private Button _playPauseButton;
    [SerializeField]
    private Button _fastForwardButton;
    private readonly ButtonBinder _buttonBinder = new ButtonBinder();

    public event Action<TransportControlsView.ButtonType> buttonPressedEvent;

    public void ShowPlayingState(bool isPlaying)
    {
    }

    protected void Start()
    {
      this._buttonBinder.AddBinding(this._rewindButton, (Action) (() =>
      {
        Action<TransportControlsView.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
        if (buttonPressedEvent == null)
          return;
        buttonPressedEvent(TransportControlsView.ButtonType.Rewind);
      }));
      this._buttonBinder.AddBinding(this._stopButton, (Action) (() =>
      {
        Action<TransportControlsView.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
        if (buttonPressedEvent == null)
          return;
        buttonPressedEvent(TransportControlsView.ButtonType.Stop);
      }));
      this._buttonBinder.AddBinding(this._playPauseButton, (Action) (() =>
      {
        Action<TransportControlsView.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
        if (buttonPressedEvent == null)
          return;
        buttonPressedEvent(TransportControlsView.ButtonType.PlayPause);
      }));
      this._buttonBinder.AddBinding(this._fastForwardButton, (Action) (() =>
      {
        Action<TransportControlsView.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
        if (buttonPressedEvent == null)
          return;
        buttonPressedEvent(TransportControlsView.ButtonType.FastForward);
      }));
    }

    protected void OnDestroy() => this._buttonBinder.ClearBindings();

    public enum ButtonType
    {
      Rewind,
      Stop,
      PlayPause,
      FastForward,
    }
  }
}
