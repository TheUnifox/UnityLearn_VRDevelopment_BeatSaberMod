// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.Tools.MoveBeatsTool
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;

namespace BeatmapEditor3D.BpmEditor.UI.Tools
{
  public class MoveBeatsTool : BpmEditorTool
  {
    [SerializeField]
    private BpmRegionsInputController _inputController;
    [SerializeField]
    private BpmBeatsView _bpmBeatsView;
    private int _regionIdx;
    private BpmRegion _region;
    private int _startSample;
    private int _beat;

    public override void EnableTool() => this._bpmBeatsView.beatMouseDownEvent += new Action<int, int>(this.HandleBpmBeatsViewMouseDown);

    public override void DisableTool() => this._bpmBeatsView.beatMouseDownEvent -= new Action<int, int>(this.HandleBpmBeatsViewMouseDown);

    private void HandleBpmBeatsViewMouseDown(int sample, int beat)
    {
      this._regionIdx = this._bpmEditorState.hoverBpmRegionIdx;
      this._region = new BpmRegion(this._bpmEditorDataModel.regions[this._regionIdx]);
      this._beat = beat;
      this._signalBus.Fire<BpmMoveBeatSignal>(new BpmMoveBeatSignal(this._regionIdx, this._region, this._beat, sample));
      this._bpmBeatsView.beatMouseUpEvent += new Action<int, int>(this.HandleBpmBeatsViewMouseUp);
      this._inputController.mouseBeginDragEvent += new Action<int>(this.HandleInputControllerMouseBeginDrag);
      this._inputController.mouseDragEvent += new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent += new Action<int>(this.HandleInputControllerMouseEndDrag);
    }

    private void HandleBpmBeatsViewMouseUp(int sample, int beat) => this.RemoveEvents();

    private void HandleInputControllerMouseBeginDrag(int sample) => this._bpmBeatsView.beatMouseUpEvent -= new Action<int, int>(this.HandleBpmBeatsViewMouseUp);

    private void HandleInputControllerMouseEndDrag(int sample)
    {
      this.RemoveEvents();
      this._signalBus.Fire<EndBpmMoveBeatSignal>();
    }

    private void HandleInputControllerMouseDrag(int sample) => this._signalBus.Fire<BpmMoveBeatSignal>(new BpmMoveBeatSignal(this._regionIdx, this._region, this._beat, sample));

    private void RemoveEvents()
    {
      this._bpmBeatsView.beatMouseUpEvent -= new Action<int, int>(this.HandleBpmBeatsViewMouseUp);
      this._inputController.mouseDragEvent -= new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent -= new Action<int>(this.HandleInputControllerMouseEndDrag);
    }
  }
}
