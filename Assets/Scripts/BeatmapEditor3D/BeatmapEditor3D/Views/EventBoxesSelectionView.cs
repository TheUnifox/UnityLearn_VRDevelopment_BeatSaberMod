// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxesSelectionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventBoxesSelectionView : MonoBehaviour
  {
    [SerializeField]
    private StretchableObstacle _stretchableObstacle;
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<EventBoxEditorData> _eventBoxes;

    protected void OnEnable()
    {
      this._stretchableObstacle.gameObject.SetActive(false);
      this._beatmapEditorGroundView.mouseUpEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseUp);
      this._beatmapEditorGroundView.mouseStartDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
      this._beatmapEditorGroundView.mouseMoveEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
      this._signalBus.Subscribe<EventBoxesUpdatedSignal>(new Action(this.HandleEventBoxesUpdated));
      this._signalBus.Subscribe<SelectMultipleEventBoxesEventsSignal>(new Action(this.HandleSelectMultipleEventBoxes));
    }

    protected void OnDisable()
    {
      this._beatmapEditorGroundView.mouseUpEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseUp);
      this._beatmapEditorGroundView.mouseStartDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
      this._beatmapEditorGroundView.mouseMoveEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
      this._signalBus.TryUnsubscribe<EventBoxesUpdatedSignal>(new Action(this.HandleEventBoxesUpdated));
      this._signalBus.TryUnsubscribe<SelectMultipleEventBoxesEventsSignal>(new Action(this.HandleSelectMultipleEventBoxes));
    }

    private void HandleBeatmapEditorGroundViewMouseUp(Vector2 position)
    {
      if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftControl))
        return;
      Input.GetKey(KeyCode.RightControl);
    }

    private void HandleBeatmapEditorGroundViewMouseStartDrag(Vector2 position)
    {
      (float, int) andEventBoxIndex = this.GetBeatAndEventBoxIndex(position);
      this._signalBus.Fire<EventBoxesChangeRectangleSelectionSignal>(new EventBoxesChangeRectangleSelectionSignal(andEventBoxIndex.Item2, andEventBoxIndex.Item1, EventBoxesChangeRectangleSelectionSignal.ChangeType.Start));
      this.UpdateSelectionRectangle();
    }

    private void HandleBeatmapEditorGroundViewMouseDrag(Vector2 position)
    {
      (float, int) andEventBoxIndex = this.GetBeatAndEventBoxIndex(position);
      this._signalBus.Fire<EventBoxesChangeRectangleSelectionSignal>(new EventBoxesChangeRectangleSelectionSignal(andEventBoxIndex.Item2, andEventBoxIndex.Item1, EventBoxesChangeRectangleSelectionSignal.ChangeType.Drag));
      this.UpdateSelectionRectangle();
    }

    private void HandleBeatmapEditorGroundViewMouseEndDrag(Vector2 position)
    {
      (float, int) andEventBoxIndex = this.GetBeatAndEventBoxIndex(position);
      this._signalBus.Fire<EventBoxesChangeRectangleSelectionSignal>(new EventBoxesChangeRectangleSelectionSignal(andEventBoxIndex.Item2, andEventBoxIndex.Item1, EventBoxesChangeRectangleSelectionSignal.ChangeType.End));
      this._signalBus.Fire<SelectMultipleEventBoxesEventsSignal>(new SelectMultipleEventBoxesEventsSignal(true, true));
    }

    private void HandleBeatmapEditorGroundViewMouseMove(Vector2 position)
    {
      if (this._beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      (float, int) andEventBoxIndex = this.GetBeatAndEventBoxIndex(position);
      this._signalBus.Fire<EventBoxesChangeRectangleSelectionSignal>(new EventBoxesChangeRectangleSelectionSignal(andEventBoxIndex.Item2, andEventBoxIndex.Item1, EventBoxesChangeRectangleSelectionSignal.ChangeType.Drag));
      this.UpdateSelectionRectangle();
    }

    private void HandleEventBoxesUpdated() => this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);

    private void HandleSelectMultipleEventBoxes() => this.HideSelectionRectangle();

    private (float, int) GetBeatAndEventBoxIndex(Vector2 position) => (this._beatmapObjectPlacementHelper.PositionToBeat(position.y), TrackPlacementHelper.PositionToTrackIndex(position.x, this._eventBoxes.Count));

    private void UpdateSelectionRectangle()
    {
      this._stretchableObstacle.gameObject.SetActive(true);
      int count = this._eventBoxes.Count;
      float num1 = TrackPlacementHelper.TrackToPosition(Mathf.Clamp(this._selectionState.startEventBoxIndex, 0, count), count) - 0.25f;
      float num2 = TrackPlacementHelper.TrackToPosition(Mathf.Clamp(this._selectionState.endEventBoxIndex, 0, count), count) + 0.25f;
      float position1 = this._beatmapObjectPlacementHelper.BeatToPosition(this._selectionState.startBeat);
      double position2 = (double) this._beatmapObjectPlacementHelper.BeatToPosition(this._selectionState.endBeat);
      float width = num2 - num1;
      double num3 = (double) position1;
      float length = (float) (position2 - num3);
      this._stretchableObstacle.SetSizeAndColor(width, 0.1f, length, Color.white);
      this._stretchableObstacle.transform.localPosition = new Vector3(num1 + width * 0.5f, 0.0f, position1);
    }

    private void HideSelectionRectangle() => this._stretchableObstacle.gameObject.SetActive(false);
  }
}
