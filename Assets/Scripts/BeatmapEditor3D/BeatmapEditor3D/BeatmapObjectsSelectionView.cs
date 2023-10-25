// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsSelectionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsSelectionView : MonoBehaviour
  {
    [SerializeField]
    private StretchableObstacle _stretchableObstacle;
    [SerializeField]
    private BeatmapObjectsGroundView _beatmapObjectsGroundView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    protected void Start()
    {
      this._stretchableObstacle.gameObject.SetActive(false);
      this._signalBus.Subscribe<SelectMultipleBeatmapObjectsSignal>(new Action(this.HandleSelectMultipleBeatmapObjects));
      this._beatmapObjectsGroundView.mouseStartDragEvent += new Action<float>(this.HandleBeatmapObjectGroundViewMouseStartDrag);
      this._beatmapObjectsGroundView.mouseDragEvent += new Action<float>(this.HandleBeatmapObjectGroundViewMouseDrag);
      this._beatmapObjectsGroundView.mouseEndDragEvent += new Action<float>(this.HandleBeatmapObjectGroundMouseEndDrag);
      this._beatmapObjectsGroundView.mouseMoveEvent += new Action<float>(this.HandleBeatmapObjectGroundMouseMove);
    }

    protected void OnDestroy()
    {
      this._signalBus.TryUnsubscribe<SelectMultipleBeatmapObjectsSignal>(new Action(this.HandleSelectMultipleBeatmapObjects));
      this._beatmapObjectsGroundView.mouseStartDragEvent -= new Action<float>(this.HandleBeatmapObjectGroundViewMouseStartDrag);
      this._beatmapObjectsGroundView.mouseDragEvent -= new Action<float>(this.HandleBeatmapObjectGroundViewMouseDrag);
      this._beatmapObjectsGroundView.mouseEndDragEvent -= new Action<float>(this.HandleBeatmapObjectGroundMouseEndDrag);
      this._beatmapObjectsGroundView.mouseMoveEvent -= new Action<float>(this.HandleBeatmapObjectGroundMouseMove);
    }

    private void HandleBeatmapObjectGroundViewMouseStartDrag(float beat)
    {
      this._signalBus.Fire<BeatmapObjectsChangeRectangleSelectionSignal>(new BeatmapObjectsChangeRectangleSelectionSignal(this._beatmapState.beat, BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.Start));
      this.UpdateSelectionRectangle();
    }

    private void HandleBeatmapObjectGroundViewMouseDrag(float beat)
    {
      this._signalBus.Fire<BeatmapObjectsChangeRectangleSelectionSignal>(new BeatmapObjectsChangeRectangleSelectionSignal(beat, BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.Drag));
      this.UpdateSelectionRectangle();
    }

    private void HandleBeatmapObjectGroundMouseEndDrag(float beat)
    {
      this._signalBus.Fire<BeatmapObjectsChangeRectangleSelectionSignal>(new BeatmapObjectsChangeRectangleSelectionSignal(beat, BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.End));
      this._signalBus.Fire<SelectMultipleBeatmapObjectsSignal>(new SelectMultipleBeatmapObjectsSignal(true));
    }

    private void HandleBeatmapObjectGroundMouseMove(float beat)
    {
      if (this._beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<BeatmapObjectsChangeRectangleSelectionSignal>(new BeatmapObjectsChangeRectangleSelectionSignal(beat, BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.Drag));
      this.UpdateSelectionRectangle();
    }

    private void HandleSelectMultipleBeatmapObjects() => this.HideSelectionRectangle();

    private void UpdateSelectionRectangle()
    {
      this._stretchableObstacle.gameObject.SetActive(true);
      float position = this._beatmapObjectPlacementHelper.BeatToPosition(this._beatmapObjectsSelectionState.startBeat);
      this._stretchableObstacle.SetSizeAndColor(3.5f, 0.1f, this._beatmapObjectPlacementHelper.BeatToPosition(this._beatmapObjectsSelectionState.endBeat) - position, Color.white);
      this._stretchableObstacle.transform.localPosition = new Vector3(0.0f, 0.0f, position);
    }

    private void HideSelectionRectangle() => this._stretchableObstacle.gameObject.SetActive(false);
  }
}
