// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.Tools.MergeRegionsTool
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using UnityEngine;

namespace BeatmapEditor3D.BpmEditor.UI.Tools
{
  public class MergeRegionsTool : BpmEditorTool
  {
    private const int kMinDragDeltaInSamples = 1000;
    [SerializeField]
    private BpmRegionsInputController _inputController;
    [SerializeField]
    private BpmRegionsView _bpmRegionsView;
    [Space]
    [SerializeField]
    private RectTransform _bpmMergeVisualGuideTransform;
    [SerializeField]
    private GameObject _bpmMergeVisualGuideGo;
    private int _leftRegionIdx;
    private int _rightRegionIdx;
    private BpmRegion _leftRegion;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    protected void Update() => this._keyboardBinder.ManualUpdate();

    public override void EnableTool()
    {
      this.enabled = true;
      this._bpmRegionsView.regionDividerMouseDownEvent += new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseDown);
      this._keyboardBinder.AddBinding(KeyCode.Escape, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleEscapePressed));
      this._bpmMergeVisualGuideGo.SetActive(false);
    }

    public override void DisableTool()
    {
      this.enabled = true;
      this._bpmRegionsView.regionDividerMouseDownEvent -= new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseDown);
      this._keyboardBinder.ClearBindings();
      this._bpmMergeVisualGuideGo.SetActive(false);
    }

    private void HandleEscapePressed(bool pressed)
    {
      this._bpmMergeVisualGuideGo.SetActive(false);
      this.RemoveHandlers();
    }

    private void HandleBpmRegionsViewRegionDividerMouseDown(int leftRegionIdx, int rightRegionIdx)
    {
      if (leftRegionIdx == -1 || rightRegionIdx == -1)
        return;
      this._leftRegionIdx = leftRegionIdx;
      this._rightRegionIdx = rightRegionIdx;
      this._leftRegion = this._bpmEditorDataModel.regions[this._leftRegionIdx];
      this._bpmRegionsView.regionDividerMouseUpEvent += new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseUp);
      this._inputController.mouseBeginDragEvent += new Action<int>(this.HandleInputControllerMouseBeginDrag);
      this._inputController.mouseDragEvent += new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent += new Action<int>(this.HandleInputControllerMouseEndDrag);
    }

    private void HandleInputControllerMouseEndDrag(int sample)
    {
      this.RemoveHandlers();
      if (this._leftRegion == null || Mathf.Abs(this._leftRegion.endSampleIndex - sample) <= 1000)
        return;
      this._bpmMergeVisualGuideGo.SetActive(false);
      this._signalBus.Fire<BpmMergeRegionsSignal>(new BpmMergeRegionsSignal(this._leftRegionIdx, this._rightRegionIdx, sample <= this._leftRegion.endSampleIndex ? this._rightRegionIdx : this._leftRegionIdx));
      this._signalBus.Fire<EndBpmMergeRegionsSignal>();
    }

    private void HandleBpmRegionsViewRegionDividerMouseUp(int leftRegionIdx, int rightRegionIdx) => this.RemoveHandlers();

    private void HandleInputControllerMouseBeginDrag(int sample) => this._bpmRegionsView.regionDividerMouseUpEvent -= new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseUp);

    private void HandleInputControllerMouseDrag(int sample)
    {
      if (this._leftRegion == null)
        return;
      this._bpmMergeVisualGuideGo.SetActive(Mathf.Abs(this._leftRegion.endSampleIndex - sample) > 1000);
      this._bpmMergeVisualGuideTransform.localScale = this._bpmMergeVisualGuideTransform.localScale with
      {
        x = sample <= this._leftRegion.endSampleIndex ? -1f : 1f
      };
      this._bpmMergeVisualGuideTransform.anchoredPosition = this._bpmMergeVisualGuideTransform.anchoredPosition with
      {
        x = WaveformPlacementHelper.CalculateRegionPosition(this._inputController.containerTransform, this._leftRegion.endSampleIndex + 1, this._inputController.startSample, this._inputController.endSample) + 1f
      };
    }

    private void RemoveHandlers()
    {
      this._bpmRegionsView.regionDividerMouseUpEvent -= new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseUp);
      this._inputController.mouseBeginDragEvent -= new Action<int>(this.HandleInputControllerMouseBeginDrag);
      this._inputController.mouseDragEvent -= new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent -= new Action<int>(this.HandleInputControllerMouseEndDrag);
    }
  }
}
