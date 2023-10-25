// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupObjectsGroundView
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
  public class EventBoxGroupObjectsGroundView : MonoBehaviour
  {
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    private List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack> _currentPageSpawnableTracks;

    protected void Start()
    {
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._signalBus.Subscribe<EventBoxGroupsPageChangedSignal>(new Action(this.HandleEventBoxGroupsPageChanged));
      this._currentPageSpawnableTracks = this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage);
      this.UnsubscribeFromGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.EventBoxGroups)
        return;
      this.SubscribeToGroundEvents();
      this.UpdateGroundScale();
    }

    protected void OnDestroy()
    {
      this.UnsubscribeFromGroundViewEvents();
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._signalBus.TryUnsubscribe<EventBoxGroupsPageChangedSignal>(new Action(this.HandleEventBoxGroupsPageChanged));
    }

    private void HandleEventBoxGroupsPageChanged()
    {
      this._currentPageSpawnableTracks = this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage);
      this.UpdateGroundScale();
    }

    private void HandleBeatmapEditorGroundViewMouseMove(Vector2 position)
    {
      float beat = AudioTimeHelper.RoundToBeat(this._beatmapObjectPlacementHelper.PositionToBeat(position.y), this._beatmapState.subdivision);
      if (this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos.Length == 0)
        return;
      (int index, float startPosition) = TrackPlacementHelper.GetPageIndexAndStartPosition(position.x, (IReadOnlyList<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) this._currentPageSpawnableTracks);
      if (index == -1)
        return;
      EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack pageSpawnableTrack = this._currentPageSpawnableTracks[index];
      int trackIndex = TrackPlacementHelper.GetTrackIndex(position.x, startPosition, (float) pageSpawnableTrack.tracksCount);
      if (trackIndex >= pageSpawnableTrack.eventBoxGroupTracks.Count)
        return;
      int groupId = pageSpawnableTrack.lightGroup.groupId;
      EventBoxGroupEditorData.EventBoxGroupType trackType = pageSpawnableTrack.eventBoxGroupTracks[trackIndex].trackType;
      if (AudioTimeHelper.IsBeatSame(this._eventBoxGroupsState.currentHoverBeat, beat) && this._eventBoxGroupsState.currentHoverGroupId == groupId && this._eventBoxGroupsState.currentHoverGroupType == trackType)
        return;
      this._signalBus.Fire<ChangeHoverGroupIdSignal>(new ChangeHoverGroupIdSignal(beat, groupId, trackType));
    }

    private void HandleLevelEditorModeSwitched()
    {
      this._currentPageSpawnableTracks = this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage);
      this.UnsubscribeFromGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.EventBoxGroups)
        return;
      this.SubscribeToGroundEvents();
      this.UpdateGroundScale();
    }

    private void UpdateGroundScale()
    {
      if (this._currentPageSpawnableTracks == null || this._currentPageSpawnableTracks.Count == 0)
        return;
      this._beatmapEditorGroundView.UpdateGroundScale(TrackPlacementHelper.GetPageWidth((IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) this._currentPageSpawnableTracks) + 1f);
    }

    private void SubscribeToGroundEvents() => this._beatmapEditorGroundView.mouseMoveEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);

    private void UnsubscribeFromGroundViewEvents() => this._beatmapEditorGroundView.mouseMoveEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
  }
}
