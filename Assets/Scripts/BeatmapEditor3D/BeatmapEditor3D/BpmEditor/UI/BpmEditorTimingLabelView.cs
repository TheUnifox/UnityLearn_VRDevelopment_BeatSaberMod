// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmEditorTimingLabelView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmEditorTimingLabelView : BeatmapEditorView
  {
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private TextMeshProUGUI _sampleText;
    [SerializeField]
    private TextMeshProUGUI _hoverSampleText;
    [Inject]
    private readonly BpmEditorSongPreviewController _songPreviewController;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.Subscribe<HoverSampleChangedSignal>(new Action(this.HandleHoverSampleChanged));
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.TryUnsubscribe<HoverSampleChangedSignal>(new Action(this.HandleHoverSampleChanged));
    }

    public void Setup() => this.FormatSamples();

    private void HandleHoverSampleChanged() => this.FormatSamples();

    private void HandlePlayHeadUpdated() => this.FormatSamples();

    private void FormatSamples()
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds((double) AudioTimeHelper.SamplesToSeconds(this._bpmEditorState.sample, this._songPreviewController.audioClip.frequency));
      this._timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
      this._sampleText.text = this._bpmEditorState.sample.ToString();
      this._hoverSampleText.text = this._bpmEditorState.hoverSample.ToString();
    }
  }
}
