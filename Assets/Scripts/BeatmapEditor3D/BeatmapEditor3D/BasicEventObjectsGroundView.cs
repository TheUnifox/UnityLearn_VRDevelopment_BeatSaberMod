// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BasicEventObjectsGroundView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BasicEventObjectsGroundView : MonoBehaviour
  {
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly SignalBus _signalBus;

    public event Action mouseEnterEvent;

    public event Action mouseExitEvent;

    public event Action mouseDownEvent;

    public event Action mouseUpEvent;

    public event Action<float, int, int> mouseStartDragEvent;

    public event Action<float, int, int> mouseDragEvent;

    public event Action<float, int, int> mouseEndDragEvent;

    public event Action<float, int, int> mouseMoveEvent;

    protected void Start()
    {
      this._signalBus.Subscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChanged));
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this.UnsubscribeToGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.BasicEvents)
        return;
      this.SubscribeToGroundViewEvents();
      this.UpdateGroundScale();
    }

    protected void OnDestroy()
    {
      this.UnsubscribeToGroundViewEvents();
      this._signalBus.TryUnsubscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChanged));
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
    }

    private void HandleBeatmapEditorGroundViewMouseEnter()
    {
      Action mouseEnterEvent = this.mouseEnterEvent;
      if (mouseEnterEvent == null)
        return;
      mouseEnterEvent();
    }

    private void HandleBeatmapEditorGroundViewMouseExit()
    {
      Action mouseExitEvent = this.mouseExitEvent;
      if (mouseExitEvent == null)
        return;
      mouseExitEvent();
    }

    private void HandleBeatmapEditorGroundViewMouseUp(Vector2 _)
    {
      Action mouseUpEvent = this.mouseUpEvent;
      if (mouseUpEvent == null)
        return;
      mouseUpEvent();
    }

    private void HandleBeatmapEditorGroundViewMouseDown(Vector2 _)
    {
      Action mouseDownEvent = this.mouseDownEvent;
      if (mouseDownEvent == null)
        return;
      mouseDownEvent();
    }

    private void HandleBeatmapEditorGroundViewMouseMove(Vector2 position)
    {
      (float beat1, int num1, int num2) = this.GetBeatAndHoverTrackFromPosition(position);
      float beat2 = AudioTimeHelper.RoundToBeat(beat1, this._beatmapState.subdivision);
      if (!AudioTimeHelper.IsBeatSame(this._basicEventsState.currentHoverBeat, beat2) || this._basicEventsState.currentHoverPageTrackId != num1 || this._basicEventsState.currentHoverVisibleTrackId != num2)
        this._signalBus.Fire<UpdateHoverBeatAndTrackSignal>(new UpdateHoverBeatAndTrackSignal()
        {
          hoverBeat = beat2,
          pageTrackId = num1,
          visibleTrackId = num2
        });
      Action<float, int, int> mouseMoveEvent = this.mouseMoveEvent;
      if (mouseMoveEvent == null)
        return;
      mouseMoveEvent(beat1, num1, num2);
    }

    private void HandleBeatmapEditorGroundViewMouseStartDrag(Vector2 position)
    {
      (float num1, int num2, int num3) = this.GetBeatAndHoverTrackFromPosition(position);
      Action<float, int, int> mouseStartDragEvent = this.mouseStartDragEvent;
      if (mouseStartDragEvent == null)
        return;
      mouseStartDragEvent(num1, num2, num3);
    }

    private void HandleBeatmapEditorGroundViewMouseDrag(Vector2 position)
    {
      (float num1, int num2, int num3) = this.GetBeatAndHoverTrackFromPosition(position);
      Action<float, int, int> mouseDragEvent = this.mouseDragEvent;
      if (mouseDragEvent == null)
        return;
      mouseDragEvent(num1, num2, num3);
    }

    private void HandleBeatmapEditorGroundViewMouseEndDrag(Vector2 position)
    {
      (float num1, int num2, int num3) = this.GetBeatAndHoverTrackFromPosition(position);
      Action<float, int, int> mouseEndDragEvent = this.mouseEndDragEvent;
      if (mouseEndDragEvent == null)
        return;
      mouseEndDragEvent(num1, num2, num3);
    }

    private void HandleLevelEditorModeSwitched()
    {
      this.UnsubscribeToGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.BasicEvents)
        return;
      this.SubscribeToGroundViewEvents();
      this.UpdateGroundScale();
    }

    private void HandleEventsPageChanged() => this.UpdateGroundScale();

    private (float, int, int) GetBeatAndHoverTrackFromPosition(Vector2 position)
    {
      float beat = this._beatmapObjectPlacementHelper.PositionToBeat(position.y);
      int trackIndex = TrackPlacementHelper.PositionToTrackIndex(position.x, this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count);
      int num1 = 0;
      for (EnvironmentTracksDefinitionSO.BasicEventTrackPage page = EnvironmentTracksDefinitionSO.BasicEventTrackPage.Page1; page < EnvironmentTracksDefinitionSO.BasicEventTrackPage.Count && page != this._basicEventsState.currentEventsPage; ++page)
        num1 += this._beatmapDataModel.environmentTrackDefinition[page].Count;
      int num2 = num1 + trackIndex;
      return (beat, trackIndex, num2);
    }

    private void UpdateGroundScale() => this._beatmapEditorGroundView.UpdateGroundScale(TrackPlacementHelper.TrackCountToScale(this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count) + 1f);

    private void SubscribeToGroundViewEvents()
    {
      this._beatmapEditorGroundView.mouseEnterEvent += new Action(this.HandleBeatmapEditorGroundViewMouseEnter);
      this._beatmapEditorGroundView.mouseExitEvent += new Action(this.HandleBeatmapEditorGroundViewMouseExit);
      this._beatmapEditorGroundView.mouseMoveEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
      this._beatmapEditorGroundView.mouseUpEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseUp);
      this._beatmapEditorGroundView.mouseDownEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDown);
      this._beatmapEditorGroundView.mouseStartDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
    }

    private void UnsubscribeToGroundViewEvents()
    {
      this._beatmapEditorGroundView.mouseEnterEvent -= new Action(this.HandleBeatmapEditorGroundViewMouseEnter);
      this._beatmapEditorGroundView.mouseExitEvent -= new Action(this.HandleBeatmapEditorGroundViewMouseExit);
      this._beatmapEditorGroundView.mouseMoveEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
      this._beatmapEditorGroundView.mouseUpEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseUp);
      this._beatmapEditorGroundView.mouseDownEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDown);
      this._beatmapEditorGroundView.mouseStartDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
    }
  }
}
