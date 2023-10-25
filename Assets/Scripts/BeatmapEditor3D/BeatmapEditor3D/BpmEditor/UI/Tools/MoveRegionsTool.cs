// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.Tools.MoveRegionsTool
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;

namespace BeatmapEditor3D.BpmEditor.UI.Tools
{
  public class MoveRegionsTool : BpmEditorTool
  {
    [SerializeField]
    private BpmRegionsInputController _inputController;
    [SerializeField]
    private BpmRegionsView _bpmRegionsView;
    private int _leftRegionIdx;
    private BpmRegion _leftRegion;
    private int _rightRegionIdx;
    private BpmRegion _rightRegion;

    public override void EnableTool() => this._bpmRegionsView.regionDividerMouseDownEvent += new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseDown);

    public override void DisableTool() => this._bpmRegionsView.regionDividerMouseDownEvent -= new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseDown);

    private void HandleBpmRegionsViewRegionDividerMouseDown(int leftRegionIdx, int rightRegionIdx)
    {
      this._leftRegionIdx = leftRegionIdx;
      this._rightRegionIdx = rightRegionIdx;
      this._leftRegion = new BpmRegion(this._bpmEditorDataModel.regions[this._leftRegionIdx]);
      this._rightRegion = new BpmRegion(this._bpmEditorDataModel.regions[this._rightRegionIdx]);
      this._bpmRegionsView.regionDividerMouseUpEvent += new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseUp);
      this._inputController.mouseBeginDragEvent += new Action<int>(this.HandleInputControllerBeginDrag);
      this._inputController.mouseDragEvent += new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent += new Action<int>(this.HandleInputControllerMouseEndDrag);
    }

    private void HandleBpmRegionsViewRegionDividerMouseUp(int leftRegionIdx, int rightRegionIdx) => this.RemoveEvents();

    private void HandleInputControllerBeginDrag(int obj) => this._bpmRegionsView.regionDividerMouseUpEvent -= new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseUp);

    private void HandleInputControllerMouseEndDrag(int sample)
    {
      this.RemoveEvents();
      this._signalBus.Fire<EndBpmMoveRegionsSignal>();
    }

    private void HandleInputControllerMouseDrag(int sample) => this._signalBus.Fire<BpmMoveRegionsSignal>(new BpmMoveRegionsSignal(this._leftRegionIdx, this._leftRegion, this._rightRegionIdx, this._rightRegion, sample));

    private void RemoveEvents()
    {
      this._bpmRegionsView.regionDividerMouseUpEvent -= new Action<int, int>(this.HandleBpmRegionsViewRegionDividerMouseUp);
      this._inputController.mouseBeginDragEvent -= new Action<int>(this.HandleInputControllerBeginDrag);
      this._inputController.mouseDragEvent -= new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent -= new Action<int>(this.HandleInputControllerMouseEndDrag);
    }
  }
}
