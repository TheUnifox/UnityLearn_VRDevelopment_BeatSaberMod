// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapEditorDataModelsInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.SerializedData;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapEditorDataModelsInstaller : MonoInstaller
  {
    [SerializeField]
    private EnvironmentTypeSO _normalEnvironmentType;
    [SerializeField]
    private EnvironmentTypeSO _circleEnvironmentType;

    public override void InstallBindings()
    {
      this.Container.Bind<BeatmapProjectManager>().AsSingle();
      this.Container.Bind<BeatmapData>().FromInstance(new BeatmapData(4)).AsSingle();
      this.Container.Bind<EnvironmentTypeSO>().WithId((object) "Normal").FromScriptableObject((ScriptableObject) this._normalEnvironmentType).AsCached();
      this.Container.Bind<EnvironmentTypeSO>().WithId((object) "Circle").FromScriptableObject((ScriptableObject) this._circleEnvironmentType).AsCached();
      this.Container.BindInterfacesAndSelfTo<BeatmapState>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapObjectsState>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BasicEventsState>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<EventBoxGroupsState>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorSettingsDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapsCollectionDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapLevelDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapBasicEventsDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEventBoxGroupsDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapLivePreviewDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapObjectsSelectionState>().AsSingle();
      this.Container.DeclareSignal<BeatmapObjectsSelectionStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<BeatmapObjectsClipboardState>().AsSingle();
      this.Container.DeclareSignal<BeatmapObjectsClipboardStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<EventsSelectionState>().AsSingle();
      this.Container.DeclareSignal<EventsSelectionStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<EventsClipboardState>().AsSingle();
      this.Container.DeclareSignal<EventsClipboardStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<EventBoxGroupsSelectionState>().AsSingle();
      this.Container.DeclareSignal<EventBoxGroupsSelectionStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<EventBoxGroupsClipboardState>().AsSingle();
      this.Container.DeclareSignal<EventBoxGroupsClipboardStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<EventBoxGroupsClipboardHelper>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<EventBoxesSelectionState>().AsSingle();
      this.Container.DeclareSignal<EventBoxesSelectionStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<EventBoxesClipboardState>().AsSingle();
      this.Container.DeclareSignal<EventBoxesClipboardStateUpdatedSignal>().OptionalSubscriber();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorSettingsLoader>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorSettingsSaver>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapLevelDataModelVersionedLoader>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapLevelDataModelSaver>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapDataModelVersionedLoader>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapDataModelSaver>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapBpmDataVersionedLoader>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapBpmDataSaver>().AsSingle();
    }
  }
}
