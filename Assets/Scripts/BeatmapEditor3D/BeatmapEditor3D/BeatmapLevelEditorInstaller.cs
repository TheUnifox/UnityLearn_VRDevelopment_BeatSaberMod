// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapLevelEditorInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using BeatmapEditor3D.Visuals;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapLevelEditorInstaller : MonoInstaller
  {
    [Header("Containers")]
    [SerializeField]
    private BeatmapObjectsContainer _beatmapObjectsContainer;
    [SerializeField]
    private BasicEventObjectsContainer _basicEventObjectsContainer;
    [SerializeField]
    private EventBoxGroupObjectsContainer _eventBoxGroupsContainer;
    [Header("Beatmap Object Prefabs")]
    [SerializeField]
    private NormalNoteView _normalNoteViewPrefab;
    [SerializeField]
    private BombNoteView _bombNoteViewPrefab;
    [SerializeField]
    private ObstacleView _obstacleViewPrefab;
    [SerializeField]
    private ArcView _arcViewPrefab;
    [SerializeField]
    private ChainNoteView _chainNoteViewPrefab;
    [SerializeField]
    private ChainElementNoteView _chainElementNoteViewPrefab;
    [Header("Beatmap Object Grid FSM")]
    [SerializeField]
    private BeatmapObjectEditGridView _beatmapObjectEditGridView;
    [SerializeField]
    private BeatmapObjectGridHoverView _beatmapObjectGridHoverView;
    [Header("BPM Regions")]
    [SerializeField]
    private BeatmapObjectBeatLine _beatmapObjectBeatLinePrefab;
    [SerializeField]
    private BpmRegionBeatNumberView _bpmRegionBeatNumberViewPrefab;
    [Header("Event Objects")]
    [SerializeField]
    private EventObjectSelectionCellView _eventObjectSelectionCellViewPrefab;
    [SerializeField]
    private EventTrack _eventTrackPrefab;
    [SerializeField]
    private TextEventMarkerObject _textEventMarkerObjectPrefab;
    [SerializeField]
    private LightEventMarkerObject _lightEventMarkerObjectPrefab;
    [SerializeField]
    private DurationEventMarkerObject _durationEventMarkerObjectPrefab;
    [Header("Event Box Group Objects")]
    [SerializeField]
    private EventBoxGroupBackgroundTrackView _eventBoxGroupBackgroundTrackViewPrefab;
    [SerializeField]
    private ColorEventBoxGroupTrackView _colorEventBoxGroupTrackViewPrefab;
    [SerializeField]
    private RotationEventBoxGroupTrackView _rotationEventBoxGroupTrackViewPrefab;
    [SerializeField]
    private TranslationEventBoxGroupTrackView _translationEventBoxGroupTrackViewPrefab;
    [SerializeField]
    private LightColorEventBoxTrackView _lightColorEventBoxTrackViewPrefab;
    [SerializeField]
    private LightRotationEventBoxTrackView _lightRotationEventBoxTrackViewPrefab;
    [SerializeField]
    private LightTranslationEventBoxTrackView _lightTranslationEventBoxTrackViewPrefab;
    [Space]
    [SerializeField]
    private TextOnlyEventMarkerObject _textOnlyEventMarkerObjectPrefab;
    [SerializeField]
    private ColorEventMarkerObject _colorEventMarkerObjectPrefab;
    [Space]
    [SerializeField]
    private BeatmapEditorAutoExposureController _autoExposureControllerPrefab;

    public override void InstallBindings()
    {
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorLivePreviewManager>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorGameplayUiStateBridge>().AsSingle().NonLazy();
      this.Container.Bind<BeatmapObjectsContainer>().FromInstance(this._beatmapObjectsContainer).AsSingle();
      this.Container.Bind<BasicEventObjectsContainer>().FromInstance(this._basicEventObjectsContainer).AsSingle();
      this.Container.Bind<EventBoxGroupObjectsContainer>().FromInstance(this._eventBoxGroupsContainer).AsSingle();
      this.Container.BindMemoryPool<BeatmapObjectBeatLine, BeatmapObjectBeatLine.Pool>().WithInitialSize(21).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._beatmapObjectBeatLinePrefab);
      this.Container.BindMemoryPool<BpmRegionBeatNumberView, BpmRegionBeatNumberView.Pool>().WithInitialSize(21).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._bpmRegionBeatNumberViewPrefab);
      this.Container.Bind<BeatmapObjectGridFsm>().AsSingle();
      this.Container.Bind<BeatmapObjectGridFsmSharedData>().AsSingle();
      this.Container.BindFactory<BeatmapObjectGridFsmStateDefault, BeatmapObjectGridFsmStateDefault.Factory>();
      this.Container.BindFactory<BeatmapObjectGridFsmStateHidden, BeatmapObjectGridFsmStateHidden.Factory>();
      this.Container.BindFactory<MoveBeatmapObjectOnGridFsmState, MoveBeatmapObjectOnGridFsmState.Factory>();
      this.Container.BindFactory<PlaceNoteBeatmapObjectGridFsmState, PlaceNoteBeatmapObjectGridFsmState.Factory>();
      this.Container.BindFactory<PlaceBombBeatmapObjectGridFsmState, PlaceBombBeatmapObjectGridFsmState.Factory>();
      this.Container.BindFactory<NoteEditorData, int, int, float, PlaceChainBeatmapObjectGridFsmState, PlaceChainBeatmapObjectGridFsmState.Factory>();
      this.Container.BindFactory<PlaceObstacleBeatmapObjectGridFsmState, PlaceObstacleBeatmapObjectGridFsmState.Factory>();
      this.Container.BindFactory<PlaceArcBeatmapObjectGridFsmState, PlaceArcBeatmapObjectGridFsmState.Factory>();
      this.Container.Bind<BeatmapObjectEditGridView>().FromInstance(this._beatmapObjectEditGridView).AsSingle();
      this.Container.Bind<BeatmapObjectGridHoverView>().FromInstance(this._beatmapObjectGridHoverView).AsSingle();
      this.Container.BindMemoryPool<NormalNoteView, NormalNoteView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._normalNoteViewPrefab).UnderTransform(this._beatmapObjectsContainer.transform);
      this.Container.BindMemoryPool<BombNoteView, BombNoteView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._bombNoteViewPrefab).UnderTransform(this._beatmapObjectsContainer.transform);
      this.Container.BindMemoryPool<ObstacleView, ObstacleView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._obstacleViewPrefab).UnderTransform(this._beatmapObjectsContainer.transform);
      this.Container.BindMemoryPool<ArcView, ArcView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._arcViewPrefab).UnderTransform(this._beatmapObjectsContainer.transform);
      this.Container.BindMemoryPool<ChainNoteView, ChainNoteView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._chainNoteViewPrefab).UnderTransform(this._beatmapObjectsContainer.transform);
      this.Container.BindMemoryPool<ChainElementNoteView, ChainElementNoteView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._chainElementNoteViewPrefab).UnderTransform(this._beatmapObjectsContainer.transform);
      this.Container.Bind<EventMarkerSpawnerProvider>().AsSingle();
      this.Container.Bind<BasicEventMarkerSpawner>().AsSingle();
      this.Container.Bind<LightEventMarkerSpawner>().AsSingle();
      this.Container.Bind<DurationEventMarkerSpawner>().AsSingle();
      this.Container.BindMemoryPool<BasicEventMarker, BasicEventMarker.Pool>().ExpandByDoubling(false);
      this.Container.BindMemoryPool<EventObjectSelectionCellView, EventObjectSelectionCellView.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._eventObjectSelectionCellViewPrefab).UnderTransform(this._basicEventObjectsContainer.transform);
      this.Container.BindMemoryPool<EventTrack, EventTrack.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._eventTrackPrefab).UnderTransform(this._basicEventObjectsContainer.transform);
      this.Container.BindMemoryPool<TextEventMarkerObject, TextEventMarkerObject.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._textEventMarkerObjectPrefab).UnderTransform(this._basicEventObjectsContainer.transform);
      this.Container.BindMemoryPool<LightEventMarkerObject, LightEventMarkerObject.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._lightEventMarkerObjectPrefab).UnderTransform(this._basicEventObjectsContainer.transform);
      this.Container.BindMemoryPool<DurationEventMarkerObject, DurationEventMarkerObject.Pool>().ExpandByDoubling(false).FromComponentInNewPrefab((Object) this._durationEventMarkerObjectPrefab).UnderTransform(this._basicEventObjectsContainer.transform);
      this.Container.BindMemoryPool<EventBoxGroupBackgroundTrackView, EventBoxGroupBackgroundTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._eventBoxGroupBackgroundTrackViewPrefab);
      this.Container.BindMemoryPool<ColorEventBoxGroupTrackView, ColorEventBoxGroupTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._colorEventBoxGroupTrackViewPrefab);
      this.Container.BindMemoryPool<RotationEventBoxGroupTrackView, RotationEventBoxGroupTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._rotationEventBoxGroupTrackViewPrefab);
      this.Container.BindMemoryPool<TranslationEventBoxGroupTrackView, TranslationEventBoxGroupTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._translationEventBoxGroupTrackViewPrefab);
      this.Container.BindMemoryPool<LightColorEventBoxTrackView, LightColorEventBoxTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._lightColorEventBoxTrackViewPrefab);
      this.Container.BindMemoryPool<LightRotationEventBoxTrackView, LightRotationEventBoxTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._lightRotationEventBoxTrackViewPrefab);
      this.Container.BindMemoryPool<LightTranslationEventBoxTrackView, LightTranslationEventBoxTrackView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._lightTranslationEventBoxTrackViewPrefab);
      this.Container.BindMemoryPool<EventBoxGroupEventMarker, EventBoxGroupEventMarker.Pool>().WithInitialSize(32).ExpandByOneAtATime(false);
      this.Container.BindMemoryPool<TextOnlyEventMarkerObject, TextOnlyEventMarkerObject.Pool>().WithInitialSize(32).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._textOnlyEventMarkerObjectPrefab);
      this.Container.BindMemoryPool<ColorEventMarkerObject, ColorEventMarkerObject.Pool>().WithInitialSize(32).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._colorEventMarkerObjectPrefab);
      this.Container.Bind<BeatmapEditorAutoExposureController>().FromComponentInNewPrefab((Object) this._autoExposureControllerPrefab).AsSingle().NonLazy();
    }
  }
}
