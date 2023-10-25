// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxGroupsSelectionView
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
  public class EventBoxGroupsSelectionView : MonoBehaviour
  {
    [SerializeField]
    private StretchableObstacle _stretchableObstacle;
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected void OnEnable()
    {
      this._stretchableObstacle.gameObject.SetActive(false);
      this._signalBus.Subscribe<SelectMultipleEventBoxGroupsSignal>(new Action(this.HandleSelectMultipleEventBoxGroups));
      this._beatmapEditorGroundView.mouseUpEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseUp);
      this._beatmapEditorGroundView.mouseStartDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
      this._beatmapEditorGroundView.mouseMoveEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
    }

    protected void OnDisable()
    {
      this._signalBus.TryUnsubscribe<SelectMultipleEventBoxGroupsSignal>(new Action(this.HandleSelectMultipleEventBoxGroups));
      this._beatmapEditorGroundView.mouseUpEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseUp);
      this._beatmapEditorGroundView.mouseStartDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
      this._beatmapEditorGroundView.mouseMoveEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
    }

    private void HandleBeatmapEditorGroundViewMouseUp(Vector2 position)
    {
      if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftControl))
        return;
      Input.GetKey(KeyCode.RightControl);
    }

    private void HandleBeatmapEditorGroundViewMouseStartDrag(Vector2 position)
    {
      this._signalBus.Fire<EventBoxGroupChangeRectangleSelectionSignal>(new EventBoxGroupChangeRectangleSelectionSignal(this.GetBeatAndGroupId(position).Item2, this._beatmapState.beat, EventBoxGroupChangeRectangleSelectionSignal.ChangeType.Start));
      this.UpdateSelectionRectangle();
    }

    private void HandleBeatmapEditorGroundViewMouseDrag(Vector2 position)
    {
      (float, int) beatAndGroupId = this.GetBeatAndGroupId(position);
      this._signalBus.Fire<EventBoxGroupChangeRectangleSelectionSignal>(new EventBoxGroupChangeRectangleSelectionSignal(beatAndGroupId.Item2, beatAndGroupId.Item1, EventBoxGroupChangeRectangleSelectionSignal.ChangeType.Drag));
      this.UpdateSelectionRectangle();
    }

    private void HandleBeatmapEditorGroundViewMouseEndDrag(Vector2 position)
    {
      (float, int) beatAndGroupId = this.GetBeatAndGroupId(position);
      this._signalBus.Fire<EventBoxGroupChangeRectangleSelectionSignal>(new EventBoxGroupChangeRectangleSelectionSignal(beatAndGroupId.Item2, beatAndGroupId.Item1, EventBoxGroupChangeRectangleSelectionSignal.ChangeType.End));
      this._signalBus.Fire<SelectMultipleEventBoxGroupsSignal>(new SelectMultipleEventBoxGroupsSignal(true, true));
    }

    private void HandleBeatmapEditorGroundViewMouseMove(Vector2 position)
    {
      if (this._beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      (float, int) beatAndGroupId = this.GetBeatAndGroupId(position);
      this._signalBus.Fire<EventBoxGroupChangeRectangleSelectionSignal>(new EventBoxGroupChangeRectangleSelectionSignal(beatAndGroupId.Item2, beatAndGroupId.Item1, EventBoxGroupChangeRectangleSelectionSignal.ChangeType.Drag));
    }

    private void HandleSelectMultipleEventBoxGroups() => this.HideSelectionRectangle();

    private (float, int) GetBeatAndGroupId(Vector2 position) => (this._beatmapObjectPlacementHelper.PositionToBeat(position.y), this._eventBoxGroupsState.currentHoverGroupId);

    private void UpdateSelectionRectangle()
    {
      this._stretchableObstacle.gameObject.SetActive(true);
      float position = this._beatmapObjectPlacementHelper.BeatToPosition(this._selectionState.startBeat);
      float length = this._beatmapObjectPlacementHelper.BeatToPosition(this._selectionState.endBeat) - position;
      List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack> eventBoxGroupTracks = this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage);
      float num = (float) (-(double) TrackPlacementHelper.GetPageWidth((IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) eventBoxGroupTracks) * 0.5);
      for (int index = 0; index < this._selectionState.startPageIndex; ++index)
        num += TrackPlacementHelper.GetGroupWidth(eventBoxGroupTracks[index].tracksCount) + 0.7f;
      float width = -0.7f;
      for (int startPageIndex = this._selectionState.startPageIndex; startPageIndex <= this._selectionState.endPageIndex; ++startPageIndex)
        width += TrackPlacementHelper.GetGroupWidth(eventBoxGroupTracks[startPageIndex].tracksCount) + 0.7f;
      this._stretchableObstacle.SetSizeAndColor(width, 0.1f, length, Color.white);
      this._stretchableObstacle.transform.localPosition = new Vector3(num + width * 0.5f, 0.0f, position);
    }

    private void HideSelectionRectangle() => this._stretchableObstacle.gameObject.SetActive(false);
  }
}
