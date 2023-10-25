// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmRegionsView
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
  public abstract class BpmRegionsView : BeatmapEditorView
  {
    [SerializeField]
    private RectTransform _containerTransform;
    [SerializeField]
    protected BpmRegionsInputController _bpmRegionsInputController;
    [Space]
    [SerializeField]
    protected bool _showSamplesInsteadOfBeats;
    [Inject]
    protected readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    protected readonly BpmEditorSongPreviewController bpmEditorSongPreviewController;
    [Inject]
    protected readonly BpmEditorState bpmEditorState;
    [Inject]
    protected readonly SignalBus _signalBus;
    [Inject]
    private readonly BpmRegionView.Pool _bpmRegionViewPool;
    [Inject]
    private readonly BpmRegionDividerView.Pool _bpmRegionDividerViewPool;
    protected readonly List<BpmRegionView> _bpmRegions = new List<BpmRegionView>();
    protected readonly List<BpmRegionDividerView> _bpmRegionDividers = new List<BpmRegionDividerView>();
    private int _previewStartSample;
    private int _previewEndSample;

    public event Action<int, int> regionDividerMouseUpEvent;

    public event Action<int, int> regionDividerMouseDownEvent;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.Subscribe<BpmRegionBeatsUpdatedSignal>(new Action<BpmRegionBeatsUpdatedSignal>(this.HandleBpmRegionBeatsUpdated));
      this._signalBus.Subscribe<BpmRegionsMovedSignal>(new Action<BpmRegionsMovedSignal>(this.HandleBpmRegionsMoved));
      this._signalBus.Subscribe<BpmRegionSplitSignal>(new Action<BpmRegionSplitSignal>(this.HandleBpmRegionSplit));
      this._signalBus.Subscribe<BpmRegionsMergedSignal>(new Action(this.HandleBpmRegionsMerged));
      this._signalBus.Subscribe<BpmRegionsChangedSignal>(new Action(this.HandleBpmRegionsChanged));
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));
      this._signalBus.TryUnsubscribe<BpmRegionBeatsUpdatedSignal>(new Action<BpmRegionBeatsUpdatedSignal>(this.HandleBpmRegionBeatsUpdated));
      this._signalBus.TryUnsubscribe<BpmRegionsMovedSignal>(new Action<BpmRegionsMovedSignal>(this.HandleBpmRegionsMoved));
      this._signalBus.TryUnsubscribe<BpmRegionSplitSignal>(new Action<BpmRegionSplitSignal>(this.HandleBpmRegionSplit));
      this._signalBus.TryUnsubscribe<BpmRegionsMergedSignal>(new Action(this.HandleBpmRegionsMerged));
      this._signalBus.TryUnsubscribe<BpmRegionsChangedSignal>(new Action(this.HandleBpmRegionsChanged));
      foreach (BpmRegionView bpmRegion in this._bpmRegions)
        this._bpmRegionViewPool.Despawn(bpmRegion);
      this._bpmRegions.Clear();
      foreach (BpmRegionDividerView bpmRegionDivider in this._bpmRegionDividers)
        this._bpmRegionDividerViewPool.Despawn(bpmRegionDivider);
      this._bpmRegionDividers.Clear();
    }

    protected virtual void HandlePlayHeadUpdated()
    {
    }

    protected virtual void HandleBpmRegionBeatsUpdated(BpmRegionBeatsUpdatedSignal signal)
    {
    }

    protected virtual void HandleBpmRegionsMoved(BpmRegionsMovedSignal signal)
    {
    }

    protected virtual void HandleBpmRegionSplit(BpmRegionSplitSignal signal)
    {
    }

    protected virtual void HandleBpmRegionsMerged()
    {
    }

    protected virtual void HandleBpmRegionsChanged()
    {
    }

    protected void DisplayBpmGrid(
      int startIndex,
      int endIndex,
      int previewStartSample,
      int previewEndSample,
      bool overrideInteractable)
    {
      this._previewStartSample = previewStartSample;
      this._previewEndSample = previewEndSample;
      this.DisplayBpmRegions(startIndex, endIndex);
      this.DisplayBpmRegionDividers(startIndex, endIndex, overrideInteractable);
    }

    private void DisplayBpmRegions(int startIndex, int endIndex)
    {
      int windowIndex = 0;
      for (int index = startIndex; index <= endIndex; ++index)
      {
        BpmRegion region = this._bpmEditorDataModel.regions[index];
        BpmRegionView bpmRegionView = this.GetBpmRegionView(windowIndex);
        this.CalculateAndSetRegionSizeAndPosition(bpmRegionView, region);
        if (this._showSamplesInsteadOfBeats)
          bpmRegionView.SetSampleData(region.startSampleIndex, region.endSampleIndex);
        else
          bpmRegionView.SetBeatData(region.startBeat, AudioTimeHelper.SamplesToBPM(region.sampleCount, region.sampleFrequency, region.beats), region.beats);
        bpmRegionView.SetState(this.bpmEditorState.sample < region.startSampleIndex || this.bpmEditorState.sample > region.endSampleIndex);
        ++windowIndex;
      }
      for (int index = this._bpmRegions.Count - 1; index >= windowIndex; --index)
      {
        this._bpmRegionViewPool.Despawn(this._bpmRegions[index]);
        this._bpmRegions.RemoveAt(index);
      }
    }

    private void DisplayBpmRegionDividers(int startIndex, int endIndex, bool overrideInteractable)
    {
      int windowIndex1 = 0;
      for (int index = startIndex; index <= endIndex; ++index)
      {
        BpmRegion region = this._bpmEditorDataModel.regions[index];
        this.CalculateAndSetRegionDividerPosition(this.GetBpmRegionDividerView(windowIndex1, index, index - startIndex, overrideInteractable), region.startSampleIndex);
        ++windowIndex1;
      }
      BpmRegion region1 = this._bpmEditorDataModel.regions[this._bpmEditorDataModel.regions.Count - 1];
      int windowIndex2 = windowIndex1;
      int num = windowIndex2 + 1;
      RectTransform rectTransform = this.GetBpmRegionDividerView(windowIndex2, 0, endIndex + 1, false).rectTransform;
      rectTransform.anchoredPosition = rectTransform.anchoredPosition with
      {
        x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, region1.endSampleIndex, this._previewStartSample, this._previewEndSample) - 2f
      };
      for (int index = this._bpmRegionDividers.Count - 1; index >= num; --index)
      {
        this._bpmRegionDividerViewPool.Despawn(this._bpmRegionDividers[index]);
        this._bpmRegionDividers.RemoveAt(index);
      }
    }

    protected void CalculateAndSetRegionSizeAndPosition(BpmRegionView regionItem, BpmRegion region)
    {
      RectTransform rectTransform = regionItem.rectTransform;
      rectTransform.anchoredPosition = rectTransform.anchoredPosition with
      {
        x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, region.startSampleIndex, this._previewStartSample, this._previewEndSample)
      };
      rectTransform.sizeDelta = rectTransform.sizeDelta with
      {
        x = WaveformPlacementHelper.CalculateRegionWidth(this._containerTransform, region.startSampleIndex, region.endSampleIndex, this._previewStartSample, this._previewEndSample)
      };
    }

    protected void CalculateAndSetRegionDividerPosition(
      BpmRegionDividerView dividerItem,
      int sample)
    {
      RectTransform rectTransform = dividerItem.rectTransform;
      rectTransform.anchoredPosition = rectTransform.anchoredPosition with
      {
        x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, sample, this._previewStartSample, this._previewEndSample)
      };
    }

    private BpmRegionView GetBpmRegionView(int windowIndex)
    {
      if (windowIndex >= this._bpmRegions.Count)
      {
        BpmRegionView bpmRegionView = this._bpmRegionViewPool.Spawn();
        this._bpmRegions.Add(bpmRegionView);
        bpmRegionView.transform.SetParent((Transform) this._containerTransform, false);
      }
      return this._bpmRegions[windowIndex];
    }

    private BpmRegionDividerView GetBpmRegionDividerView(
      int windowIndex,
      int regionIndex,
      int i,
      bool defaultInteractable)
    {
      if (windowIndex >= this._bpmRegionDividers.Count)
      {
        BpmRegionDividerView regionDividerView = this._bpmRegionDividerViewPool.Spawn();
        this._bpmRegionDividers.Add(regionDividerView);
        regionDividerView.transform.SetParent((Transform) this._containerTransform, false);
      }
      BpmRegionDividerView bpmRegionDivider = this._bpmRegionDividers[windowIndex];
      bpmRegionDivider.SetData(regionIndex - 1, regionIndex, new Action<int, int>(this.HandleBpmRegionDividerMouseUp), new Action<int, int>(this.HandleBpmRegionDividerMouseDown));
      bpmRegionDivider.SetState(i != 0 & defaultInteractable);
      return bpmRegionDivider;
    }

    private void HandleBpmRegionDividerMouseUp(int endRegionIndex, int startRegionIndex)
    {
      Action<int, int> dividerMouseUpEvent = this.regionDividerMouseUpEvent;
      if (dividerMouseUpEvent == null)
        return;
      dividerMouseUpEvent(endRegionIndex, startRegionIndex);
    }

    private void HandleBpmRegionDividerMouseDown(int endRegionIndex, int startRegionIndex)
    {
      Action<int, int> dividerMouseDownEvent = this.regionDividerMouseDownEvent;
      if (dividerMouseDownEvent == null)
        return;
      dividerMouseDownEvent(endRegionIndex, startRegionIndex);
    }
  }
}
