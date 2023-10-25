// Decompiled with JetBrains decompiler
// Type: MainSystemInit
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BGNet.Core.GameLift;
using OnlineServices;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweening;
using UnityEngine;
using UnityEngine.XR;
using VRUIControls;
using Zenject;

public class MainSystemInit : MonoBehaviour
{
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [SerializeField]
  protected AudioManagerSO _audioManager;
  [SerializeField]
  protected PerceivedLoudnessPerLevelSO _perceivedLoudnessPerLevel;
  [SerializeField]
  protected RelativeSfxVolumePerLevelSO _relativeSfxVolumePerLevel;
  [SerializeField]
  protected AvatarDataModel _avatarDataModel;
  [SerializeField]
  protected AvatarPartsModelSO _avatarPartsModel;
  [SerializeField]
  protected SkinColorSetSO _skinColorSet;
  [SerializeField]
  protected MirrorRendererGraphicsSettingsPresets _mirrorRendererGraphicsSettingsPresets;
  [SerializeField]
  protected MainEffectGraphicsSettingsPresetsSO _mainEffectGraphicsSettingsPresets;
  [SerializeField]
  protected BloomPrePassGraphicsSettingsPresetsSO _bloomPrePassGraphicsSettingsPresets;
  [SerializeField]
  protected MirrorRendererSO _mirrorRenderer;
  [SerializeField]
  protected MainEffectContainerSO _mainEffectContainer;
  [SerializeField]
  protected BloomPrePassEffectContainerSO _bloomPrePassEffectContainer;
  [SerializeField]
  protected SongPackMaskModelSO _songPackMaskModel;
  [Space]
  [SerializeField]
  protected AppStaticSettingsSO _appStaticSettings;
  [Space]
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelScenesTransitionSetupData;
  [SerializeField]
  protected MultiplayerLevelScenesTransitionSetupDataSO _multiplayerLevelScenesTransitionSetupData;
  [Space]
  [SerializeField]
  protected TimeHelper _timeHelperPrefab;
  [SerializeField]
  protected PlayerDataModel _playerDataModelPrefab;
  [SerializeField]
  protected CampaignProgressModel _campaignProgressModelPrefab;
  [SerializeField]
  protected BeatmapLevelsModel _beatmapLevelsModelPrefab;
  [SerializeField]
  protected CustomLevelLoader _customLevelLoaderPrefab;
  [SerializeField]
  protected CachedMediaAsyncLoader _cachedMediaAsyncLoaderPrefab;
  [SerializeField]
  protected ExternalCamerasManager _externalCamerasManagerPrefab;
  [SerializeField]
  protected MultiplayerSessionManager _multiplayerSessionManagerPrefab;
  [SerializeField]
  protected VoipManager _voipManagerPrefab;
  [SerializeField]
  protected LocalNetworkPlayerModel _localNetworkPlayerModelPrefab;
  [SerializeField]
  protected GameLiftNetworkPlayerModel _gameLiftNetworkPlayerModelPrefab;
  [SerializeField]
  protected NetworkPlayerEntitlementChecker _networkPlayerEntitlementCheckerPrefab;
  [SerializeField]
  protected HapticFeedbackController _hapticFeedbackControllerPrefab;
  [SerializeField]
  protected TimeTweeningManager _tweeningManagerPrefab;
  [SerializeField]
  protected BloomPrePassLightsUpdateSystem _lightsUpdateSystemPrefab;
  [SerializeField]
  protected EnvironmentAudioEffectsPlayer _environmentAudioEffectsPlayerPrefab;
  [Space]
  [SerializeField]
  protected NodePoseSyncStateManager _nodePoseSyncStateManagerPrefab;
  [Space]
  [SerializeField]
  protected AlwaysOwnedContentContainerSO _alwaysOwnedContentContainer;
  [Space]
  [SerializeField]
  protected PSVRHelper _psVRHelperPrefab;
  [SerializeField]
  protected OculusVRHelper _oculusVRHelperPrefab;
  [SerializeField]
  protected OpenVRHelper _openVRHelperPrefab;
  [SerializeField]
  protected DevicelessVRHelper _devicelessVRHelperPrefab;
  [Space]
  [SerializeField]
  protected TestPlatformAdditionalContentModel _testPlatformAdditionalContentModelPrefab;
  [SerializeField]
  protected PS4PlatformAdditionalContentModel _ps4PlatformAdditionalContentModelPrefab;
  [SerializeField]
  protected OculusPlatformAdditionalContentModel _oculusPlatformAdditionalContentModelPrefab;
  [SerializeField]
  protected SteamPlatformAdditionalContentModel _steamPlatformAdditionalContentModelPrefab;
  [Space]
  [SerializeField]
  protected SteamLevelProductsModelSO _steamLevelProductsModel;
  [SerializeField]
  protected OculusLevelProductsModelSO _oculusLevelProducsModel;
  [SerializeField]
  protected PS4LevelProductsModelSO _ps4LevelProductsModel;
  [SerializeField]
  protected PS4LeaderboardIdsModelSO _ps4LeaderboardIdsModel;
  [SerializeField]
  protected LeaderboardIdsModelSO _riftLeaderboardIdsModel;
  [SerializeField]
  protected LeaderboardIdsModelSO _questLeaderboardIdsModel;
  [SerializeField]
  protected LeaderboardIdsModelSO _steamLeaderboardIdsModel;
  [SerializeField]
  protected ServerManager _onlineServicesServerManagerPrefab;
  [SerializeField]
  protected RichPresenceManager _richPresenceManagerPrefab;
  [SerializeField]
  protected DlcPromoPanelDataSO _dlcPromoPanelData;
  [SerializeField]
  protected BeatmapLevelsPromoDataSO _beatmapLevelsPromoData;
  [Space]
  [SerializeField]
  protected NetworkConfigSO _networkConfig;
  [SerializeField]
  protected SteamNetworkPlayerModel _steamNetworkPlayerModelPrefab;
  [SerializeField]
  protected OculusNetworkPlayerModel _oculusNetworkPlayerModelPrefab;
  [SerializeField]
  protected PS4NetworkPlayerModel _ps4NetworkPlayerModelPrefab;
  [Space]
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  [Space]
  [SerializeField]
  protected LeaderboardScoreUploader _leaderboardScoreUploader;
  [SerializeField]
  protected PlatformLeaderboardsModel _platformLeaderboardsModel;
  [SerializeField]
  protected BeatmapLevelSO _anyBeatmapLevelSO;
  [Space]
  [SerializeField]
  protected RecordingToolInstallerSO _recordingToolInstaller;
  protected readonly EnvironmentCommandLineArgsProvider _commandLineArgsProvider = new EnvironmentCommandLineArgsProvider();
  protected MockPlayersModel _mockPlayersModel;

  public virtual void Init()
  {
    this._mainSettingsModel.Load(false);
    Application.backgroundLoadingPriority = ThreadPriority.Low;
    this._audioManager.Init();
    this._audioManager.mainVolume = AudioHelpers.NormalizedVolumeToDB((float) (ObservableVariableSO<float>) this._mainSettingsModel.volume);
    Vector2Int windowResolution = (Vector2Int) (ObservableVariableSO<Vector2Int>) this._mainSettingsModel.windowResolution;
    if (Screen.width != windowResolution.x || Screen.height != windowResolution.y)
      Screen.SetResolution(windowResolution.x, windowResolution.y, false);
    Screen.fullScreen = (bool) (ObservableVariableSO<bool>) this._mainSettingsModel.fullscreen;
    XRSettings.renderViewportScale = 1f;
    XRSettings.eyeTextureResolutionScale = (float) (ObservableVariableSO<float>) this._mainSettingsModel.vrResolutionScale * (float) (ObservableVariableSO<float>) this._mainSettingsModel.menuVRResolutionScaleMultiplier;
    XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.mirrorGraphicsSettings >= this._mirrorRendererGraphicsSettingsPresets.presets.Length)
      this._mainSettingsModel.mirrorGraphicsSettings.value = this._mirrorRendererGraphicsSettingsPresets.presets.Length - 1;
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.mainEffectGraphicsSettings >= this._mainEffectGraphicsSettingsPresets.presets.Length)
      this._mainSettingsModel.mainEffectGraphicsSettings.value = this._mainEffectGraphicsSettingsPresets.presets.Length - 1;
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.bloomPrePassGraphicsSettings >= this._bloomPrePassGraphicsSettingsPresets.presets.Length)
      this._mainSettingsModel.bloomPrePassGraphicsSettings.value = this._bloomPrePassGraphicsSettingsPresets.presets.Length - 1;
    MirrorRendererGraphicsSettingsPresets.Preset preset1 = this._mirrorRendererGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.mirrorGraphicsSettings];
    MainEffectGraphicsSettingsPresetsSO.Preset preset2 = this._mainEffectGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.mainEffectGraphicsSettings];
    BloomPrePassGraphicsSettingsPresetsSO.Preset preset3 = this._bloomPrePassGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.bloomPrePassGraphicsSettings];
    BoolSO graphicsSettings = this._mainSettingsModel.smokeGraphicsSettings;
    Language selected = this._mainSettingsModel.language.value;
    this._mirrorRenderer.Init(preset1.reflectLayers, preset1.stereoTextureWidth, preset1.stereoTextureHeight, preset1.monoTextureWidth, preset1.monoTextureHeight, preset1.maxAntiAliasing, preset1.enableBloomPrePassFog);
    this._mainEffectContainer.Init(preset2.mainEffect);
    this._bloomPrePassEffectContainer.Init(preset3.bloomPrePassEffect);
    Localization.Instance.SelectLanguage(selected);
    Application.targetFrameRate = -1;
    Application.runInBackground = true;
    QualitySettings.maxQueuedFrames = -1;
    QualitySettings.vSyncCount = 0;
    QualitySettings.antiAliasing = (int) (ObservableVariableSO<int>) this._mainSettingsModel.antiAliasingLevel;
  }

  public virtual void PreInstall(MockPlayersModel mockPlayersModel) => this._mockPlayersModel = mockPlayersModel;

  public virtual void InstallBindings(DiContainer container)
  {
    container.Bind<TimeHelper>().FromComponentInNewPrefab((UnityEngine.Object) this._timeHelperPrefab).AsSingle().NonLazy();
    container.Bind<AppStaticSettingsSO>().FromInstance(this._appStaticSettings).AsSingle();
    container.Bind<SongPreviewPlayer.InitData>().FromInstance(new SongPreviewPlayer.InitData((float) (ObservableVariableSO<float>) this._mainSettingsModel.ambientVolumeScale)).AsSingle();
    container.Bind<AudioManagerSO>().FromInstance(this._audioManager).AsSingle();
    container.Bind<PerceivedLoudnessPerLevelModel>().FromInstance(new PerceivedLoudnessPerLevelModel(this._perceivedLoudnessPerLevel)).AsSingle();
    container.Bind<RelativeSfxVolumePerLevelModel>().FromInstance(new RelativeSfxVolumePerLevelModel(this._relativeSfxVolumePerLevel)).AsSingle();
    container.Bind<PlayerDataModel>().FromComponentInNewPrefab((UnityEngine.Object) this._playerDataModelPrefab).AsSingle();
    container.Bind<AvatarDataModel>().FromComponentInNewPrefab((UnityEngine.Object) this._avatarDataModel).AsSingle();
    container.Bind<AvatarPartsModel>().FromInstance(new AvatarPartsModel(this._avatarPartsModel, this._skinColorSet)).AsSingle();
    container.Bind<CampaignProgressModel>().FromComponentInNewPrefab((UnityEngine.Object) this._campaignProgressModelPrefab).AsSingle();
    container.Bind<BeatmapLevelsModel>().FromComponentInNewPrefab((UnityEngine.Object) this._beatmapLevelsModelPrefab).AsSingle();
    container.Bind<ExternalCamerasManager>().FromComponentInNewPrefab((UnityEngine.Object) this._externalCamerasManagerPrefab).AsSingle().NonLazy();
    container.Bind<CustomLevelLoader>().FromComponentInNewPrefab((UnityEngine.Object) this._customLevelLoaderPrefab).AsSingle();
    container.Bind<CachedMediaAsyncLoader>().FromComponentInNewPrefab((UnityEngine.Object) this._cachedMediaAsyncLoaderPrefab).AsSingle();
    container.Bind<BeatmapDataCache>().AsSingle().NonLazy();
    container.Bind<IReferenceCountingCache<int, Task<AudioClip>>>().To<AudioReferenceCountingCache>().AsTransient();
    container.Bind<IMediaAsyncLoader>().To<MediaAsyncLoader>().AsSingle();
    container.Bind<AudioClipAsyncLoader>().AsSingle();
    if (this._mockPlayersModel == null)
    {
      if ((bool) (ObservableVariableSO<bool>) this._mainSettingsModel.useCustomServerEnvironment)
      {
        string lower = this._mainSettingsModel.customServerHostName.value.ToLower();
        int result = this._networkConfig.masterServerEndPoint.port;
        if (lower.Contains(":"))
        {
          int.TryParse(lower.Split(':')[1], out result);
          lower = lower.Split(':')[0];
        }
        CustomNetworkConfig instance = new CustomNetworkConfig((INetworkConfig) this._networkConfig, lower, result, (bool) (ObservableVariableSO<bool>) this._mainSettingsModel.forceGameLiftServerEnvironment);
        container.Bind<INetworkConfig>().FromInstance((INetworkConfig) instance).AsSingle();
      }
      else
      {
        container.Bind<NetworkConfigSO>().FromScriptableObject((ScriptableObject) this._networkConfig).AsSingle();
        container.Bind<INetworkConfig>().To<NetworkConfigSO>().FromResolve();
      }
      container.Bind<IMultiplayerStatusModel>().To<MultiplayerStatusModel>().AsSingle();
      container.Bind<IQuickPlaySetupModel>().To<QuickPlaySetupModel>().AsSingle();
      container.Bind<IMultiplayerSessionManager>().FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerSessionManagerPrefab).AsSingle();
      container.Bind<LocalNetworkPlayerModel>().FromComponentInNewPrefab((UnityEngine.Object) this._localNetworkPlayerModelPrefab).AsSingle();
      container.Bind<IGameLiftPlayerSessionProvider>().To<GameLiftPlayerSessionProvider>().AsSingle();
      container.Bind<GameLiftNetworkPlayerModel>().FromComponentInNewPrefab((UnityEngine.Object) this._gameLiftNetworkPlayerModelPrefab).AsSingle();
      container.Bind(typeof (IUnifiedNetworkPlayerModel), typeof (IInitializable), typeof (IDisposable)).To<UnifiedNetworkPlayerModel>().AsSingle();
    }
    container.Bind(typeof (IMenuRpcManager), typeof (IDisposable)).To<MenuRpcManager>().AsSingle();
    container.Bind<LobbyGameStateModel>().AsSingle();
    container.Bind<LobbyPlayerPermissionsModel>().AsSingle();
    container.Bind(typeof (INodePoseSyncStateManager), typeof (NodePoseSyncStateManager)).FromComponentInNewPrefab((UnityEngine.Object) this._nodePoseSyncStateManagerPrefab).AsSingle();
    container.Bind<NetworkPlayerEntitlementChecker>().FromComponentInNewPrefab((UnityEngine.Object) this._networkPlayerEntitlementCheckerPrefab).AsSingle().NonLazy();
    container.Bind<SongPackMasksModel>().FromInstance(new SongPackMasksModel(this._songPackMaskModel)).AsSingle();
    container.Bind<HapticFeedbackController>().FromComponentInNewPrefab((UnityEngine.Object) this._hapticFeedbackControllerPrefab).AsSingle();
    container.Bind<TimeTweeningManager>().FromComponentInNewPrefab((UnityEngine.Object) this._tweeningManagerPrefab).AsSingle();
    container.Bind<BloomPrePassLightsUpdateSystem>().FromComponentInNewPrefab((UnityEngine.Object) this._lightsUpdateSystemPrefab).AsSingle().NonLazy();
    container.Bind<EnvironmentAudioEffectsPlayer>().FromComponentInNewPrefab((UnityEngine.Object) this._environmentAudioEffectsPlayerPrefab).AsSingle().NonLazy();
    container.Bind<VRControllersInputManager>().AsSingle();
    container.Bind<BeatmapCharacteristicCollectionSO>().FromScriptableObject((ScriptableObject) this._beatmapCharacteristicCollection).AsSingle();
    container.Bind<AlwaysOwnedContentContainerSO>().FromScriptableObject((ScriptableObject) this._alwaysOwnedContentContainer).AsSingle();
    container.Bind<DlcPromoPanelDataSO>().FromScriptableObject((ScriptableObject) this._dlcPromoPanelData).AsSingle().NonLazy();
    container.Bind<DlcPromoPanelModel>().AsSingle().NonLazy();
    container.Bind<BeatmapLevelsPromoDataSO>().FromScriptableObject((ScriptableObject) this._beatmapLevelsPromoData).AsSingle().NonLazy();
    container.Bind<StandardLevelScenesTransitionSetupDataSO>().FromScriptableObject((ScriptableObject) this._standardLevelScenesTransitionSetupData).AsSingle();
    container.Bind<MissionLevelScenesTransitionSetupDataSO>().FromScriptableObject((ScriptableObject) this._missionLevelScenesTransitionSetupData).AsSingle();
    container.Bind<MultiplayerLevelScenesTransitionSetupDataSO>().FromScriptableObject((ScriptableObject) this._multiplayerLevelScenesTransitionSetupData).AsSingle();
    container.Bind<GameplayLevelSceneTransitionEvents>().AsSingle().NonLazy();
    container.Bind<ResetPitchOnGameplayFinished>().AsSingle().NonLazy();
    if (this.IsRunningFromNUnit())
      container.Bind<IVRPlatformHelper>().FromComponentInNewPrefab((UnityEngine.Object) this._devicelessVRHelperPrefab).AsSingle();
    else if (XRSettings.loadedDeviceName.IndexOf("oculus", StringComparison.OrdinalIgnoreCase) >= 0)
      container.Bind<IVRPlatformHelper>().FromComponentInNewPrefab((UnityEngine.Object) this._oculusVRHelperPrefab).AsSingle();
    else if (XRSettings.loadedDeviceName.IndexOf("openvr", StringComparison.OrdinalIgnoreCase) >= 0)
    {
      container.Bind<IVRPlatformHelper>().FromComponentInNewPrefab((UnityEngine.Object) this._openVRHelperPrefab).AsSingle();
      if ((bool) (ObservableVariableSO<bool>) this._mainSettingsModel.openVrThreadedHaptics)
        container.Bind<IOpenVRHaptics>().To<ThreadedOpenVrOpenVrHaptics>().AsSingle();
      else
        container.Bind<IOpenVRHaptics>().To<SimpleOpenVrOpenVrHaptics>().AsSingle();
    }
    else
      container.Bind<IVRPlatformHelper>().FromComponentInNewPrefab((UnityEngine.Object) this._devicelessVRHelperPrefab).AsSingle();
    container.Bind<OculusLevelProductsModelSO>().FromScriptableObject((ScriptableObject) this._oculusLevelProducsModel).AsSingle();
    container.Bind<AdditionalContentModel>().FromComponentInNewPrefab((UnityEngine.Object) this._oculusPlatformAdditionalContentModelPrefab).AsSingle();
    container.Bind<IBeatmapDataAssetFileModel>().To<OculusBeatmapDataAssetFileModel>().AsSingle();
    container.Bind<IPlatformUserModel>().To<OculusPlatformUserModel>().AsSingle();
    container.Bind<PlatformNetworkPlayerModel>().FromComponentInNewPrefab((UnityEngine.Object) this._oculusNetworkPlayerModelPrefab).AsSingle();
    this.InstallOculusDestinationBindings(container);
    container.Bind<IInvitePlatformHandler>().To<OculusInvitePlatformHandler>().AsSingle();
    this.InstallRichPresence(container);
    this.InstallPlatformLeaderboardsModel(container);
    ProgramArguments instance1 = new ProgramArguments((IReadOnlyList<string>) this._commandLineArgsProvider.GetCommandLineArgs());
    container.Bind<ProgramArguments>().FromInstance(instance1).AsSingle();
  }

  public virtual void InstallRichPresence(DiContainer container)
  {
    if (this.IsRunningFromNUnit())
      container.Bind<IRichPresencePlatformHandler>().To<NoRichPresencePlatformHandler>().AsSingle();
    else
      container.Bind<IRichPresencePlatformHandler>().To<OculusRichPresencePlatformHandler>().AsSingle();
    container.Bind<RichPresenceManager>().FromComponentInNewPrefab((UnityEngine.Object) this._richPresenceManagerPrefab).AsSingle().NonLazy();
  }

  public virtual void InstallOculusDestinationBindings(DiContainer container)
  {
    container.BindInterfacesAndSelfTo<OculusDeeplinkManager>().AsSingle().NonLazy();
    container.Bind<IDestinationRequestManager>().To<DeeplinkManagerToDestinationRequestManagerAdapter>().AsSingle().NonLazy();
    container.BindInterfacesAndSelfTo<MainMenuDestinationRequestController>().AsSingle().NonLazy();
  }

  public virtual void InstallPlatformLeaderboardsModel(DiContainer container)
  {
    OculusPlatformLeaderboardsHandler instance = new OculusPlatformLeaderboardsHandler(this._riftLeaderboardIdsModel);
    container.Bind<PlatformLeaderboardsHandler>().FromInstance((PlatformLeaderboardsHandler) instance).AsSingle();
    container.Bind<LeaderboardScoreUploader>().FromComponentInNewPrefab((UnityEngine.Object) this._leaderboardScoreUploader).AsSingle();
    container.BindInterfacesAndSelfTo<PlatformLeaderboardsModel>().FromComponentInNewPrefab((UnityEngine.Object) this._platformLeaderboardsModel).AsSingle();
    container.Bind<ScreenCaptureCache>().AsSingle();
    container.Bind<PhysicsRaycasterWithCache>().AsSingle();
  }

  public virtual bool IsRunningFromNUnit() => false;
}
