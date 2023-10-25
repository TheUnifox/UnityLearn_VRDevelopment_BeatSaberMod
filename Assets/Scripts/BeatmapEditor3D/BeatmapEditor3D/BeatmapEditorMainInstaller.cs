// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorMainInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.Logging;
using BeatmapEditor3D.Views;
using HMUI;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorMainInstaller : MonoInstaller
  {
    [SerializeField]
    private Transform _poolWrapperTransform;
    [Space]
    [SerializeField]
    private DevicelessVRHelper _devicelessVRHelperPrefab;
    [Space]
    [SerializeField]
    private BeatmapEditorScenesManager _beatmapEditorScenesManagerPrefab;
    [SerializeField]
    private MenuTransitionsHelper _menuTransitionsHelperPrefab;
    [SerializeField]
    private AudioClipLoader _audioClipLoaderPrefab;
    [Space]
    [SerializeField]
    private FlashMessageView _flashMessageViewPrefab;
    [Space]
    [SerializeField]
    private AudioSource _songPreviewAudioSource;
    [SerializeField]
    private EditorAudioFeedbackController _editorAudioFeedbackController;
    [SerializeField]
    private AudioManagerSO _audioManager;
    [Space]
    [SerializeField]
    private BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
    [Space]
    [SerializeField]
    private EnvironmentsListSO _environmentsList;
    [SerializeField]
    private EnvironmentDefinitionsListSO _environmentDefinitionsList;
    [Space]
    [SerializeField]
    private bool _enableRuntimeLogging;

    public override void InstallBindings()
    {
      if (this._enableRuntimeLogging)
        this.Container.BindInterfacesAndSelfTo<BeatmapEditorRuntimeLogger>().AsSingle();
      this.Container.Bind<VRControllersInputManager>().AsSingle();
      this.Container.Bind<KeyboardBinder>().AsTransient();
      this.Container.Bind<MouseBinder>().AsTransient();
      this.Container.Bind<IVRPlatformHelper>().FromComponentInNewPrefab((Object) this._devicelessVRHelperPrefab).AsSingle();
      this.Container.BindInterfacesAndSelfTo<ScrollInputController>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<EditBpmScrollInputController>().AsSingle();
      this.Container.BindInterfacesTo<SongPreviewController>().AsSingle();
      this.Container.Bind(typeof (AudioSource), typeof (BasicSpectrogramData)).FromComponentInNewPrefab((Object) this._songPreviewAudioSource).AsSingle();
      this.Container.Bind<EditorAudioFeedbackController>().FromComponentInNewPrefab((Object) this._editorAudioFeedbackController).AsSingle().NonLazy();
      this.Container.Bind<AudioManagerSO>().FromInstance(this._audioManager);
      this.Container.Bind<BeatmapEditorScenesManager>().FromComponentInNewPrefab((Object) this._beatmapEditorScenesManagerPrefab).AsSingle();
      this.Container.Bind<MenuTransitionsHelper>().FromComponentInNewPrefab((Object) this._menuTransitionsHelperPrefab).AsSingle();
      this.Container.Bind<AudioClipLoader>().FromComponentInNewPrefab((Object) this._audioClipLoaderPrefab).AsSingle();
      this.Container.Bind<WaveformDataModel>().AsSingle();
      this.Container.Bind<BeatmapLevelStarterController>().AsSingle();
      this.Container.Bind<BeatmapDataLoader>().AsSingle();
      this.Container.Bind<BeatmapObjectPlacementHelper>().AsSingle();
      this.Container.Bind<BeatmapCharacteristicCollectionSO>().FromScriptableObject((ScriptableObject) this._beatmapCharacteristicCollection).AsSingle();
      this.Container.Bind<EnvironmentsListSO>().FromScriptableObject((ScriptableObject) this._environmentsList).AsSingle();
      this.Container.Bind<EnvironmentDefinitionsListSO>().FromScriptableObject((ScriptableObject) this._environmentDefinitionsList).AsSingle();
      this.Container.Bind<BeatmapEditorLogger>().FromInstance(new BeatmapEditorLogger((ILogHandler) Debug.unityLogger, true, UnityEngine.LogType.Log));
      this.Container.Bind<DataTransformerProvider>().AsSingle();
      this.Container.BindMemoryPool<FlashMessageView, FlashMessageView.Pool>().ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._flashMessageViewPrefab).UnderTransform(this._poolWrapperTransform);
    }
  }
}
