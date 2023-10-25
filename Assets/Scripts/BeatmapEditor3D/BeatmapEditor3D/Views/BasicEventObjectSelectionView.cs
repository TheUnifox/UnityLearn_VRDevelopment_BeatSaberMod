// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BasicEventObjectSelectionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class BasicEventObjectSelectionView : MonoBehaviour
  {
    [SerializeField]
    private BasicEventObjectsGroundView _basicEventObjectsGroundView;
    [SerializeField]
    private StretchableObstacle _stretchableObstacle;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly IReadonlyEventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;

    protected void OnEnable()
    {
      this._signalBus.Subscribe<SelectMultipleEventsSignal>(new Action(this.HandleSelectMultipleEvents));
      this._stretchableObstacle.gameObject.SetActive(false);
      this._basicEventObjectsGroundView.mouseUpEvent += new Action(this.HandleBasicEventObjectsGroundViewMouseUp);
      this._basicEventObjectsGroundView.mouseStartDragEvent += new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseStartDrag);
      this._basicEventObjectsGroundView.mouseDragEvent += new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseDrag);
      this._basicEventObjectsGroundView.mouseEndDragEvent += new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseEndDrag);
      this._basicEventObjectsGroundView.mouseMoveEvent += new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseMove);
    }

    protected void OnDisable()
    {
      this._signalBus.TryUnsubscribe<SelectMultipleEventsSignal>(new Action(this.HandleSelectMultipleEvents));
      this._basicEventObjectsGroundView.mouseUpEvent -= new Action(this.HandleBasicEventObjectsGroundViewMouseUp);
      this._basicEventObjectsGroundView.mouseStartDragEvent -= new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseStartDrag);
      this._basicEventObjectsGroundView.mouseDragEvent -= new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseDrag);
      this._basicEventObjectsGroundView.mouseEndDragEvent -= new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseEndDrag);
      this._basicEventObjectsGroundView.mouseMoveEvent -= new Action<float, int, int>(this.HandleBasicEventObjectsGroundViewMouseMove);
    }

    private void HandleBasicEventObjectsGroundViewMouseUp()
    {
      if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) || !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<SelectAllEventsOnTrackSignal>(new SelectAllEventsOnTrackSignal()
      {
        visibleTrackId = this._basicEventsState.currentHoverVisibleTrackId
      });
    }

    private void HandleBasicEventObjectsGroundViewMouseStartDrag(
      float beat,
      int pageTrackId,
      int visibleTrackId)
    {
      this._signalBus.Fire<EventsChangeRectangleSelectionSignal>(new EventsChangeRectangleSelectionSignal()
      {
        changeType = EventsChangeRectangleSelectionSignal.ChangeType.Start,
        beat = this._beatmapState.beat,
        trackIndex = visibleTrackId
      });
      this.UpdateSelectionRectangle();
    }

    private void HandleBasicEventObjectsGroundViewMouseDrag(
      float beat,
      int pageTrackId,
      int visibleTrackId)
    {
      this._signalBus.Fire<EventsChangeRectangleSelectionSignal>(new EventsChangeRectangleSelectionSignal()
      {
        changeType = EventsChangeRectangleSelectionSignal.ChangeType.Drag,
        beat = beat,
        trackIndex = visibleTrackId
      });
      this.UpdateSelectionRectangle();
    }

    private void HandleBasicEventObjectsGroundViewMouseEndDrag(
      float beat,
      int pageTrackId,
      int visibleTrackId)
    {
      this._signalBus.Fire<EventsChangeRectangleSelectionSignal>(new EventsChangeRectangleSelectionSignal()
      {
        changeType = EventsChangeRectangleSelectionSignal.ChangeType.End,
        beat = beat,
        trackIndex = visibleTrackId
      });
      this._signalBus.Fire<SelectMultipleEventsSignal>(new SelectMultipleEventsSignal()
      {
        addToSelection = true
      });
    }

    private void HandleBasicEventObjectsGroundViewMouseMove(
      float beat,
      int pageTrackId,
      int visibleTrackId)
    {
      if (this._beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<EventsChangeRectangleSelectionSignal>(new EventsChangeRectangleSelectionSignal()
      {
        changeType = EventsChangeRectangleSelectionSignal.ChangeType.Drag,
        beat = beat,
        trackIndex = visibleTrackId
      });
      this.UpdateSelectionRectangle();
    }

    private void HandleSelectMultipleEvents() => this.HideSelectionRectangle();

    private void UpdateSelectionRectangle()
    {
      this._stretchableObstacle.gameObject.SetActive(true);
      int count1 = this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count;
      List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo> basicEventTrackInfoList = this._beatmapDataModel.environmentTrackDefinition[EnvironmentTracksDefinitionSO.BasicEventTrackPage.Page1];
      int count2 = this._basicEventsState.currentEventsPage == EnvironmentTracksDefinitionSO.BasicEventTrackPage.Page1 ? 0 : basicEventTrackInfoList.Count;
      int max = this._basicEventsState.currentEventsPage == EnvironmentTracksDefinitionSO.BasicEventTrackPage.Page1 ? basicEventTrackInfoList.Count - 1 : this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos.Length - 1;
      float num1 = TrackPlacementHelper.TrackToPosition(Mathf.Clamp(this._eventsSelectionState.startTrackIndex, count2, max) - count2, count1) - 0.25f;
      float num2 = TrackPlacementHelper.TrackToPosition(Mathf.Clamp(this._eventsSelectionState.endTrackIndex, count2, max) - count2, count1) + 0.25f;
      float position1 = this._beatmapObjectPlacementHelper.BeatToPosition(this._eventsSelectionState.startBeat);
      double position2 = (double) this._beatmapObjectPlacementHelper.BeatToPosition(this._eventsSelectionState.endBeat);
      float width = num2 - num1;
      double num3 = (double) position1;
      float length = (float) (position2 - num3);
      this._stretchableObstacle.SetSizeAndColor(width, 0.1f, length, Color.white);
      this._stretchableObstacle.transform.localPosition = new Vector3(num1 + width * 0.5f, 0.0f, position1);
    }

    private void HideSelectionRectangle() => this._stretchableObstacle.gameObject.SetActive(false);
  }
}
