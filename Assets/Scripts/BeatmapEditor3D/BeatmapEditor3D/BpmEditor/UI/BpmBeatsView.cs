// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmBeatsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmBeatsView : BeatmapEditorView
  {
    [SerializeField]
    private RectTransform _containerTransform;
    [Inject]
    private readonly BpmEditorSongPreviewController _bpmEditorSongPreviewController;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BpmBeatMarkerView.Pool _bpmBeatMarkerViewPool;
    [Inject]
    private readonly BpmQuarterBeatMarkerView.Pool _bpmQuarterBeatMarkerViewPool;
    private const float kHandleWidth = 300f;
    private const float kShowQuarterBeatLimit = 300f;
    private readonly List<BpmBeatMarkerView> _beatMarkers = new List<BpmBeatMarkerView>();
    private readonly List<BpmQuarterBeatMarkerView> _quarterBeatMarkers = new List<BpmQuarterBeatMarkerView>();

    public event Action<int, int> beatMouseDownEvent;

    public event Action<int, int> beatMouseUpEvent;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.Subscribe<BpmRegionBeatsUpdatedSignal>(new Action(this.HandleBpmRegionBeatsUpdated));
      this._signalBus.Subscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
      this._signalBus.Subscribe<BpmRegionsMovedSignal>(new Action(this.HandleBpmRegionsMoved));
      this._signalBus.Subscribe<BpmRegionsChangedSignal>(new Action(this.HandleBpmRegionsChanged));
      this._signalBus.Subscribe<BpmRegionSplitSignal>(new Action(this.HandleBpmRegionSplit));
      this._signalBus.Subscribe<BpmRegionsMergedSignal>(new Action(this.HandleBpmRegionsMerged));
      this._signalBus.Subscribe<BpmSubdivisionSwitchedSignal>(new Action(this.HandleBpmSubdivisionSwitched));
      this.DisplayBeats();
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.TryUnsubscribe<BpmRegionBeatsUpdatedSignal>(new Action(this.HandleBpmRegionBeatsUpdated));
      this._signalBus.TryUnsubscribe<PlayHeadZoomedSignal>(new Action(this.HandlePlayHeadZoomed));
      this._signalBus.TryUnsubscribe<BpmRegionsMovedSignal>(new Action(this.HandleBpmRegionsMoved));
      this._signalBus.TryUnsubscribe<BpmRegionsChangedSignal>(new Action(this.HandleBpmRegionsChanged));
      this._signalBus.TryUnsubscribe<BpmRegionSplitSignal>(new Action(this.HandleBpmRegionSplit));
      this._signalBus.TryUnsubscribe<BpmRegionsMergedSignal>(new Action(this.HandleBpmRegionsMerged));
      this._signalBus.TryUnsubscribe<BpmSubdivisionSwitchedSignal>(new Action(this.HandleBpmSubdivisionSwitched));
      foreach (BpmBeatMarkerView beatMarker in this._beatMarkers)
        this._bpmBeatMarkerViewPool.Despawn(beatMarker);
      this._beatMarkers.Clear();
    }

    private void HandlePlayHeadUpdated() => this.DisplayBeats();

    private void HandleBpmRegionBeatsUpdated() => this.DisplayBeats();

    private void HandlePlayHeadZoomed() => this.DisplayBeats();

    private void HandleBpmRegionsMoved() => this.DisplayBeats();

    private void HandleBpmRegionsChanged() => this.DisplayBeats();

    private void HandleBpmRegionsMerged() => this.DisplayBeats();

    private void HandleBpmRegionSplit() => this.DisplayBeats();

    private void HandleBpmSubdivisionSwitched() => this.DisplayBeats();

    private void DisplayBeats()
    {
      int num1 = this._bpmEditorState.sample - this._bpmEditorState.previewHalfSize;
      int num2 = this._bpmEditorState.sample + this._bpmEditorState.previewHalfSize;
      int num3 = Mathf.Max(0, num1);
      int b = Mathf.Min(num2, this._bpmEditorSongPreviewController.samplesCount);
      (int start, int end) = this._bpmEditorState.previewRegionWindow;
      int num4 = Mathf.RoundToInt(300f / (WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, num3 + 10, num1, num2) - WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, num3, num1, num2)));
      int windowIndex = 0;
      int quarterBeatIndex = 0;
      for (int index = start; index <= end; ++index)
      {
        BpmRegion region = this._bpmEditorDataModel.regions[index];
        float beat1 = region.SampleToBeat(Mathf.Max(region.startSampleIndex, num3));
        float beat2 = region.SampleToBeat(Mathf.Min(region.endSampleIndex, b));
        int num5 = Mathf.CeilToInt(beat1);
        int num6 = Mathf.FloorToInt(beat2);
        for (int beat3 = num5; beat3 <= num6; ++beat3)
          this.DisplayBeat(region, beat3, num1, num2, num4, ref windowIndex);
        int num7 = Mathf.FloorToInt(beat1);
        int num8 = Mathf.CeilToInt(beat2);
        if ((double) WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, region.startSampleIndex + region.samplesPerBeat, region.startSampleIndex, region.startSampleIndex + this._bpmEditorState.previewHalfSize * 2) > 300.0)
        {
          for (int beat4 = num7; beat4 <= num8; ++beat4)
            this.DisplayQuarterBeats(region, beat4, num1, num2, num4, ref quarterBeatIndex);
        }
      }
      for (int index = windowIndex; index < this._beatMarkers.Count; ++index)
        this._beatMarkers[index].gameObject.SetActive(false);
      for (int index = quarterBeatIndex; index < this._quarterBeatMarkers.Count; ++index)
        this._quarterBeatMarkers[index].gameObject.SetActive(false);
    }

    private void HandleBeatMarkerMouseDown(int sample, int beat)
    {
      Action<int, int> beatMouseDownEvent = this.beatMouseDownEvent;
      if (beatMouseDownEvent == null)
        return;
      beatMouseDownEvent(sample, beat);
    }

    private void HandleBeatMarkerMouseUp(int sample, int beat)
    {
      Action<int, int> beatMouseUpEvent = this.beatMouseUpEvent;
      if (beatMouseUpEvent == null)
        return;
      beatMouseUpEvent(sample, beat);
    }

    private void DisplayBeat(
      BpmRegion region,
      int beat,
      int previewStartSample,
      int previewEndSample,
      int samplesEpsilon,
      ref int windowIndex)
    {
      int sample = region.BeatToSample((float) beat);
      if (sample <= region.startSampleIndex || sample >= region.endSampleIndex - samplesEpsilon)
        return;
      if (windowIndex >= this._beatMarkers.Count)
      {
        BpmBeatMarkerView bpmBeatMarkerView = this._bpmBeatMarkerViewPool.Spawn();
        bpmBeatMarkerView.rectTransform.SetParent((Transform) this._containerTransform, false);
        this._beatMarkers.Add(bpmBeatMarkerView);
      }
      BpmBeatMarkerView beatMarker = this._beatMarkers[windowIndex];
      beatMarker.SetData(sample, beat, new Action<int, int>(this.HandleBeatMarkerMouseDown), new Action<int, int>(this.HandleBeatMarkerMouseUp));
      if (!beatMarker.gameObject.activeSelf)
        beatMarker.gameObject.SetActive(true);
      this.CalculatePosition(beatMarker.rectTransform, sample, previewStartSample, previewEndSample);
      ++windowIndex;
    }

    private void DisplayQuarterBeats(
      BpmRegion region,
      int beat,
      int previewStartSample,
      int previewEndSample,
      int sampleEpsilon,
      ref int quarterBeatIndex)
    {
      float num1 = this._bpmEditorState.bpmSubdivisionType == BpmSubdivisionType.Normal ? 0.25f : 0.33f;
      for (float num2 = num1; (double) num2 < 0.89999997615814209; num2 += num1)
      {
        int sample = region.BeatToSample((float) beat + num2);
        if (sample > region.startSampleIndex && sample < region.endSampleIndex - sampleEpsilon)
        {
          if (quarterBeatIndex >= this._quarterBeatMarkers.Count)
          {
            BpmQuarterBeatMarkerView quarterBeatMarkerView = this._bpmQuarterBeatMarkerViewPool.Spawn();
            quarterBeatMarkerView.rectTransform.SetParent((Transform) this._containerTransform, false);
            this._quarterBeatMarkers.Add(quarterBeatMarkerView);
          }
          BpmQuarterBeatMarkerView quarterBeatMarker = this._quarterBeatMarkers[quarterBeatIndex];
          if (!quarterBeatMarker.gameObject.activeSelf)
            quarterBeatMarker.gameObject.SetActive(true);
          this.CalculatePosition(quarterBeatMarker.rectTransform, sample, previewStartSample, previewEndSample);
          ++quarterBeatIndex;
        }
      }
    }

        private void CalculatePosition(RectTransform rectTransform, int sample, int previewStartSample, int previewEndSample)
        {
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, sample, previewStartSample, previewEndSample);
            rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
