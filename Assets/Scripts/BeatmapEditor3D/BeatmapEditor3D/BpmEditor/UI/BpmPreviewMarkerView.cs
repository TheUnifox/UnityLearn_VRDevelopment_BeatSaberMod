// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmPreviewMarkerView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmPreviewMarkerView : BeatmapEditorView
  {
    [SerializeField]
    [NullAllowed(NullAllowed.Context.Prefab)]
    private RectTransform _containerTransform;
    [SerializeField]
    private RectTransform _previewTransform;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly BpmEditorSongPreviewController _bpmEditorSongPreviewController;
    [Inject]
    private readonly SignalBus _signalBus;
    private int _startSample;
    private int _endSample;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.Subscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.TryUnsubscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
    }

    public void Setup(int startSample, int endSample, int currentSample)
    {
      this._startSample = startSample;
      this._endSample = endSample;
      this.SetPreviewPositionAndSize(currentSample);
    }

    private void HandlePlayHeadUpdated() => this.SetPreviewPositionAndSize(this._bpmEditorState.sample);

    private void HandlePlayHeadZoomed() => this.SetPreviewPositionAndSize(this._bpmEditorState.sample);

        private void SetPreviewPositionAndSize(int sample)
        {
            int num = Mathf.Max(0, sample - this._bpmEditorState.previewHalfSize);
            int regionEndSample = Mathf.Min(this._bpmEditorSongPreviewController.samplesCount, sample + this._bpmEditorState.previewHalfSize);
            Vector2 anchoredPosition = this._previewTransform.anchoredPosition;
            anchoredPosition.x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, num, this._startSample, this._endSample) + 1f;
            this._previewTransform.anchoredPosition = anchoredPosition;
            Vector2 sizeDelta = this._previewTransform.sizeDelta;
            sizeDelta.x = WaveformPlacementHelper.CalculateRegionWidth(this._containerTransform, num, regionEndSample, this._startSample, this._endSample);
            this._previewTransform.sizeDelta = sizeDelta;
        }
    }
}
