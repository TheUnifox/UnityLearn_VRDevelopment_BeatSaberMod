// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxesObjectsGroundView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventBoxesObjectsGroundView : MonoBehaviour
  {
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    private List<EventBoxEditorData> _eventBoxes;

    protected void Start()
    {
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._signalBus.Subscribe<EventBoxesUpdatedSignal>(new Action(this.HandleEventBoxesUpdated));
      this.UnsubscribeFromGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.EventBoxes)
        return;
      this.SubscribeToGroundEvents();
      this.UpdateGroundScale();
    }

    protected void OnDestroy()
    {
      this.UnsubscribeFromGroundViewEvents();
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._signalBus.TryUnsubscribe<EventBoxesUpdatedSignal>(new Action(this.HandleEventBoxesUpdated));
    }

    private void HandleBeatmapEditorGroundViewMouseMove(Vector2 position)
    {
      float beat = AudioTimeHelper.RoundToBeat(this._beatmapObjectPlacementHelper.PositionToBeat(position.y), this._beatmapState.subdivision);
      BeatmapEditorObjectId id = this._eventBoxes[TrackPlacementHelper.PositionToTrackIndex(position.x, this._eventBoxes.Count)].id;
      if (AudioTimeHelper.IsBeatSame(this._eventBoxGroupsState.currentHoverBeat, beat) && !(this._eventBoxGroupsState.currentHoverEventBoxId != id))
        return;
      this._signalBus.Fire<ChangeHoverEventBoxIdSignal>(new ChangeHoverEventBoxIdSignal(id));
    }

    private void HandleLevelEditorModeSwitched()
    {
      this.UnsubscribeFromGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.EventBoxes)
        return;
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
      this.SubscribeToGroundEvents();
      this.UpdateGroundScale();
    }

    private void HandleEventBoxesUpdated()
    {
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
      this.UpdateGroundScale();
    }

    private void UpdateGroundScale() => this._beatmapEditorGroundView.UpdateGroundScale(TrackPlacementHelper.TrackCountToScale(this._eventBoxes.Count) + 1f);

    private void SubscribeToGroundEvents() => this._beatmapEditorGroundView.mouseMoveEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);

    private void UnsubscribeFromGroundViewEvents() => this._beatmapEditorGroundView.mouseMoveEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
  }
}
