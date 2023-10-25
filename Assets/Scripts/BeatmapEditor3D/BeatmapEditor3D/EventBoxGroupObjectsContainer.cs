// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupObjectsContainer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using BeatmapEditor3D.Visuals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventBoxGroupObjectsContainer : MonoBehaviour
  {
    [SerializeField]
    private BeatGridContainer _beatGridContainer;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly EventBoxGroupBackgroundTrackView.Pool _eventBoxGroupBackgroundTrackViewPool;
    [Inject]
    private readonly ColorEventBoxGroupTrackView.Pool _colorEventBoxGroupTrackViewPool;
    [Inject]
    private readonly RotationEventBoxGroupTrackView.Pool _rotationEventBoxGroupTrackViewPool;
    [Inject]
    private readonly TranslationEventBoxGroupTrackView.Pool _translationEventBoxGroupTrackViewPool;
    private bool _initialized;
    private MemoryPoolContainer<EventBoxGroupBackgroundTrackView> _eventBoxGroupBackgroundTrackPoolContainer;
    private MemoryPoolContainer<ColorEventBoxGroupTrackView> _colorEventBoxGroupTrackPoolContainer;
    private MemoryPoolContainer<RotationEventBoxGroupTrackView> _rotationEventBoxGroupTrackContainer;
    private MemoryPoolContainer<TranslationEventBoxGroupTrackView> _translationEventBoxGroupTrackContainer;

    protected void OnEnable()
    {
      this._signalBus.Subscribe<EventBoxGroupsPageChangedSignal>(new Action(this.HandleEventBoxGroupsPageChanged));
      this.ClearEventTracks();
      this.SpawnEventTracks();
      this._beatGridContainer.SetDataToBeatContainers(1f, TrackPlacementHelper.GetPageWidth((IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage)));
      this._beatGridContainer.Enable();
    }

    protected void OnDisable()
    {
      this._signalBus.TryUnsubscribe<EventBoxGroupsPageChangedSignal>(new Action(this.HandleEventBoxGroupsPageChanged));
      this._beatGridContainer.Disable();
    }

    private void HandleEventBoxGroupsPageChanged()
    {
      this.ClearEventTracks();
      this.SpawnEventTracks();
      this._beatGridContainer.SetDataToBeatContainers(1f, TrackPlacementHelper.GetPageWidth((IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage)));
      this._beatGridContainer.ForceUpdate();
    }

    private void SpawnEventTracks()
    {
      if (this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos == null || this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos.Length == 0)
        return;
      if (!this._initialized)
      {
        this._eventBoxGroupBackgroundTrackPoolContainer = new MemoryPoolContainer<EventBoxGroupBackgroundTrackView>((IMemoryPool<EventBoxGroupBackgroundTrackView>) this._eventBoxGroupBackgroundTrackViewPool);
        this._colorEventBoxGroupTrackPoolContainer = new MemoryPoolContainer<ColorEventBoxGroupTrackView>((IMemoryPool<ColorEventBoxGroupTrackView>) this._colorEventBoxGroupTrackViewPool);
        this._rotationEventBoxGroupTrackContainer = new MemoryPoolContainer<RotationEventBoxGroupTrackView>((IMemoryPool<RotationEventBoxGroupTrackView>) this._rotationEventBoxGroupTrackViewPool);
        this._translationEventBoxGroupTrackContainer = new MemoryPoolContainer<TranslationEventBoxGroupTrackView>((IMemoryPool<TranslationEventBoxGroupTrackView>) this._translationEventBoxGroupTrackViewPool);
        this._initialized = true;
      }
      List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack> eventBoxGroupTracks = this._beatmapDataModel.environmentTrackDefinition.GetSpawnableEventBoxGroupTracks(this._eventBoxGroupsState.currentPage);
      float startPosition = (float) (-(double) TrackPlacementHelper.GetPageWidth((IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) eventBoxGroupTracks) * 0.5);
      foreach (EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack eventBoxGroupTrack1 in eventBoxGroupTracks)
      {
        float groupWidth = TrackPlacementHelper.GetGroupWidth(eventBoxGroupTrack1.tracksCount);
        EventBoxGroupBackgroundTrackView backgroundTrackView = this._eventBoxGroupBackgroundTrackPoolContainer.Spawn();
        backgroundTrackView.Initialize(eventBoxGroupTrack1.groupName, groupWidth);
        Transform transform = backgroundTrackView.transform;
        transform.SetParent(this.transform, false);
        transform.localPosition = TrackPlacementHelper.GetGroupPosition(startPosition, groupWidth);
        for (int index = 0; index < eventBoxGroupTrack1.eventBoxGroupTracks.Count; ++index)
        {
          EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack eventBoxGroupTrack2 = eventBoxGroupTrack1.eventBoxGroupTracks[index];
          float position = startPosition + TrackPlacementHelper.GetTrackPositionInGroup(index);
          switch (eventBoxGroupTrack2.trackType)
          {
            case EventBoxGroupEditorData.EventBoxGroupType.Color:
              this.CreateColorTrackView(eventBoxGroupTrack2.groupName, eventBoxGroupTrack2.lightGroup, position);
              break;
            case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
              this.CreateRotationTrackView(eventBoxGroupTrack2.groupName, eventBoxGroupTrack2.lightGroup, position);
              break;
            case EventBoxGroupEditorData.EventBoxGroupType.Translation:
              this.CreateTranslationTrackView(eventBoxGroupTrack2.groupName, eventBoxGroupTrack2.lightGroup, position);
              break;
          }
        }
        startPosition += groupWidth + 0.7f;
      }
    }

    private void ClearEventTracks()
    {
      if (!this._initialized)
        return;
      for (int index = this._eventBoxGroupBackgroundTrackPoolContainer.activeItems.Count - 1; index >= 0; --index)
        this._eventBoxGroupBackgroundTrackPoolContainer.Despawn(this._eventBoxGroupBackgroundTrackPoolContainer.activeItems[index]);
      for (int index = this._colorEventBoxGroupTrackPoolContainer.activeItems.Count - 1; index >= 0; --index)
        this._colorEventBoxGroupTrackPoolContainer.Despawn(this._colorEventBoxGroupTrackPoolContainer.activeItems[index]);
      for (int index = this._rotationEventBoxGroupTrackContainer.activeItems.Count - 1; index >= 0; --index)
        this._rotationEventBoxGroupTrackContainer.Despawn(this._rotationEventBoxGroupTrackContainer.activeItems[index]);
      for (int index = this._translationEventBoxGroupTrackContainer.activeItems.Count - 1; index >= 0; --index)
        this._translationEventBoxGroupTrackContainer.Despawn(this._translationEventBoxGroupTrackContainer.activeItems[index]);
    }

    private void CreateColorTrackView(string name, LightGroupSO lightGroup, float position)
    {
      ColorEventBoxGroupTrackView boxGroupTrackView = this._colorEventBoxGroupTrackPoolContainer.Spawn();
      boxGroupTrackView.Initialize(lightGroup, name);
      this.SetPosition(boxGroupTrackView.transform, position);
    }

    private void CreateRotationTrackView(string name, LightGroupSO lightGroup, float position)
    {
      RotationEventBoxGroupTrackView boxGroupTrackView = this._rotationEventBoxGroupTrackContainer.Spawn();
      boxGroupTrackView.Initialize(lightGroup, name);
      this.SetPosition(boxGroupTrackView.transform, position);
    }

    private void CreateTranslationTrackView(string name, LightGroupSO lightGroup, float position)
    {
      TranslationEventBoxGroupTrackView boxGroupTrackView = this._translationEventBoxGroupTrackContainer.Spawn();
      boxGroupTrackView.Initialize(lightGroup, name);
      this.SetPosition(boxGroupTrackView.transform, position);
    }

    private void SetPosition(Transform trackTransform, float position)
    {
      trackTransform.SetParent(this.transform, false);
      trackTransform.localPosition = Vector3.right * position;
    }
  }
}
