// Decompiled with JetBrains decompiler
// Type: MainSettingsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainSettingsModelSO : PersistentScriptableObject
{
  [SOVariable]
  public FloatSO vrResolutionScale;
  [SOVariable]
  public FloatSO menuVRResolutionScaleMultiplier;
  [SOVariable]
  public BoolSO useFixedFoveatedRenderingDuringGameplay;
  [SOVariable]
  public Vector2IntSO windowResolution;
  [SOVariable]
  public Vector2IntSO editorWindowResolution;
  [SOVariable]
  public BoolSO fullscreen;
  [SOVariable]
  public IntSO antiAliasingLevel;
  [SOVariable]
  public FloatSO volume;
  [SOVariable]
  public FloatSO ambientVolumeScale;
  [SOVariable]
  public BoolSO controllersRumbleEnabled;
  [SOVariable]
  public Vector3SO roomCenter;
  [SOVariable]
  public FloatSO roomRotation;
  [SOVariable]
  public Vector3SO controllerPosition;
  [SOVariable]
  public Vector3SO controllerRotation;
  [SOVariable]
  public IntSO mirrorGraphicsSettings;
  [SOVariable]
  public IntSO mainEffectGraphicsSettings;
  [SOVariable]
  public IntSO bloomPrePassGraphicsSettings;
  [SOVariable]
  public BoolSO smokeGraphicsSettings;
  [SOVariable]
  public BoolSO enableAlphaFeatures;
  [SOVariable]
  public IntSO pauseButtonPressDurationLevel;
  [SOVariable]
  public BoolSO burnMarkTrailsEnabled;
  [SOVariable]
  public BoolSO screenDisplacementEffectsEnabled;
  [SOVariable]
  public BoolSO smoothCameraEnabled;
  [SOVariable]
  public FloatSO smoothCameraFieldOfView;
  [SOVariable]
  public Vector3SO smoothCameraThirdPersonPosition;
  [SOVariable]
  public Vector3SO smoothCameraThirdPersonEulerAngles;
  [SOVariable]
  public BoolSO smoothCameraThirdPersonEnabled;
  [SOVariable]
  public FloatSO smoothCameraRotationSmooth;
  [SOVariable]
  public FloatSO smoothCameraPositionSmooth;
  [SOVariable]
  public BoolSO overrideAudioLatency;
  [SOVariable]
  public FloatSO audioLatency;
  [SOVariable]
  public IntSO maxShockwaveParticles;
  [SOVariable]
  public IntSO maxNumberOfCutSoundEffects;
  [SOVariable]
  public BoolSO onlineServicesEnabled;
  [SOVariable]
  public BoolSO openVrThreadedHaptics;
  [SOVariable]
  public LanguageSO language;
  [SOVariable]
  public BoolSO useCustomServerEnvironment;
  [SOVariable]
  public BoolSO forceGameLiftServerEnvironment;
  [SOVariable]
  public StringSO customServerHostName;
  [SOVariable]
  public BoolSO enableFPSCounter;
  [SOVariable]
  public BoolSO depthTextureEnabled;
  [CompilerGenerated]
  protected bool m_CcreateScreenshotDuringTheGame;
  public const float kDefaultPlayerHeight = 1.8f;
  public const float kHeadPosToPlayerHeightOffset = 0.1f;
  protected const string kFileName = "settings.cfg";
  protected const string kTempFileName = "settings.cfg.tmp";
  protected const string kBackupFileName = "settings.cfg.bak";
  protected const string kCurrentVersion = "2.0.0";
  public const float kControllersPositionOffsetLimit = 0.1f;
  public const float kControllersRotationOffsetLimit = 180f;
  [CompilerGenerated]
  protected bool m_CplayingForTheFirstTime;
  protected bool _playingForTheFirstTimeChecked;
  protected bool _isLoaded;

  public bool createScreenshotDuringTheGame
  {
    get => this.m_CcreateScreenshotDuringTheGame;
    private set => this.m_CcreateScreenshotDuringTheGame = value;
  }

  public bool playingForTheFirstTime
  {
    get => this.m_CplayingForTheFirstTime;
    private set => this.m_CplayingForTheFirstTime = value;
  }

  public virtual void Save() => FileHelpers.SaveToJSONFile((object) new MainSettingsModelSO.Config()
  {
    vrResolutionScale = (float) (ObservableVariableSO<float>) this.vrResolutionScale,
    menuVRResolutionScaleMultiplier = (float) (ObservableVariableSO<float>) this.menuVRResolutionScaleMultiplier,
    useFixedFoveatedRenderingDuringGameplay = (bool) (ObservableVariableSO<bool>) this.useFixedFoveatedRenderingDuringGameplay,
    windowResolutionWidth = this.windowResolution.value.x,
    windowResolutionHeight = this.windowResolution.value.y,
    editorResolutionWidth = this.editorWindowResolution.value.x,
    editorResolutionHeight = this.editorWindowResolution.value.y,
    windowMode = ((bool) (ObservableVariableSO<bool>) this.fullscreen ? MainSettingsModelSO.WindowMode.Fullscreen : MainSettingsModelSO.WindowMode.Windowed),
    antiAliasingLevel = (int) (ObservableVariableSO<int>) this.antiAliasingLevel,
    volume = (float) (ObservableVariableSO<float>) this.volume,
    ambientVolumeScale = (float) (ObservableVariableSO<float>) this.ambientVolumeScale,
    controllersRumbleEnabled = (bool) (ObservableVariableSO<bool>) this.controllersRumbleEnabled,
    roomCenterX = this.roomCenter.value.x,
    roomCenterY = this.roomCenter.value.y,
    roomCenterZ = this.roomCenter.value.z,
    roomRotation = (float) (ObservableVariableSO<float>) this.roomRotation,
    controllerPositionX = this.controllerPosition.value.x,
    controllerPositionY = this.controllerPosition.value.y,
    controllerPositionZ = this.controllerPosition.value.z,
    controllerRotationX = this.controllerRotation.value.x,
    controllerRotationY = this.controllerRotation.value.y,
    controllerRotationZ = this.controllerRotation.value.z,
    mirrorGraphicsSettings = (int) (ObservableVariableSO<int>) this.mirrorGraphicsSettings,
    mainEffectGraphicsSettings = (int) (ObservableVariableSO<int>) this.mainEffectGraphicsSettings,
    bloomGraphicsSettings = (int) (ObservableVariableSO<int>) this.bloomPrePassGraphicsSettings,
    smokeGraphicsSettings = (this.smokeGraphicsSettings.value ? 1 : 0),
    enableAlphaFeatures = (this.enableAlphaFeatures.value ? 1 : 0),
    pauseButtonPressDurationLevel = (int) (ObservableVariableSO<int>) this.pauseButtonPressDurationLevel,
    burnMarkTrailsEnabled = (bool) (ObservableVariableSO<bool>) this.burnMarkTrailsEnabled,
    screenDisplacementEffectsEnabled = (bool) (ObservableVariableSO<bool>) this.screenDisplacementEffectsEnabled,
    smoothCameraEnabled = (this.smoothCameraEnabled.value ? 1 : 0),
    smoothCameraFieldOfView = (float) (ObservableVariableSO<float>) this.smoothCameraFieldOfView,
    smoothCameraThirdPersonPositionX = this.smoothCameraThirdPersonPosition.value.x,
    smoothCameraThirdPersonPositionY = this.smoothCameraThirdPersonPosition.value.y,
    smoothCameraThirdPersonPositionZ = this.smoothCameraThirdPersonPosition.value.z,
    smoothCameraThirdPersonEulerAnglesX = this.smoothCameraThirdPersonEulerAngles.value.x,
    smoothCameraThirdPersonEulerAnglesY = this.smoothCameraThirdPersonEulerAngles.value.y,
    smoothCameraThirdPersonEulerAnglesZ = this.smoothCameraThirdPersonEulerAngles.value.z,
    smoothCameraThirdPersonEnabled = (this.smoothCameraThirdPersonEnabled.value ? 1 : 0),
    smoothCameraRotationSmooth = (float) (ObservableVariableSO<float>) this.smoothCameraRotationSmooth,
    smoothCameraPositionSmooth = (float) (ObservableVariableSO<float>) this.smoothCameraPositionSmooth,
    overrideAudioLatency = (bool) (ObservableVariableSO<bool>) this.overrideAudioLatency,
    audioLatency = (float) (ObservableVariableSO<float>) this.audioLatency,
    maxShockwaveParticles = (int) (ObservableVariableSO<int>) this.maxShockwaveParticles,
    maxNumberOfCutSoundEffects = (int) (ObservableVariableSO<int>) this.maxNumberOfCutSoundEffects,
    onlineServicesEnabled = (bool) (ObservableVariableSO<bool>) this.onlineServicesEnabled,
    openVrThreadedHaptics = (bool) (ObservableVariableSO<bool>) this.openVrThreadedHaptics,
    language = this.language.value.ToSerializedName(),
    useCustomServerEnvironment = (bool) (ObservableVariableSO<bool>) this.useCustomServerEnvironment,
    forceGameLiftServerEnvironment = (bool) (ObservableVariableSO<bool>) this.forceGameLiftServerEnvironment,
    customServerHostName = (string) (ObservableVariableSO<string>) this.customServerHostName,
    enableFPSCounter = (bool) (ObservableVariableSO<bool>) this.enableFPSCounter
  }, Application.persistentDataPath + "/settings.cfg", Application.persistentDataPath + "/settings.cfg.tmp", Application.persistentDataPath + "/settings.cfg.bak");

  public virtual void Load(bool forced)
  {
    if (this._isLoaded && !forced)
      return;
    MainSettingsModelSO.Config config = FileHelpers.LoadFromJSONFile<MainSettingsModelSO.Config>(Application.persistentDataPath + "/settings.cfg", Application.persistentDataPath + "/settings.cfg.bak");
    if (config == null)
      config = new MainSettingsModelSO.Config();
    else if (config.version != "2.0.0")
    {
      int majorVersionNumber1 = VersionStringHelper.GetMajorVersionNumber(config.version);
      int majorVersionNumber2 = VersionStringHelper.GetMajorVersionNumber("2.0.0");
      if (majorVersionNumber1 == 1 && majorVersionNumber2 > 1)
        ++config.mirrorGraphicsSettings;
      config.version = "2.0.0";
    }
    this.vrResolutionScale.value = config.vrResolutionScale;
    this.menuVRResolutionScaleMultiplier.value = config.menuVRResolutionScaleMultiplier;
    this.useFixedFoveatedRenderingDuringGameplay.value = config.useFixedFoveatedRenderingDuringGameplay;
    this.windowResolution.value = new Vector2Int(config.windowResolutionWidth, config.windowResolutionHeight);
    this.editorWindowResolution.value = new Vector2Int(config.editorResolutionWidth, config.editorResolutionHeight);
    this.fullscreen.value = config.windowMode == MainSettingsModelSO.WindowMode.Fullscreen;
    this.antiAliasingLevel.value = config.antiAliasingLevel;
    this.roomCenter.value = new Vector3(config.roomCenterX, config.roomCenterY, config.roomCenterZ);
    this.roomRotation.value = config.roomRotation;
    this.controllersRumbleEnabled.value = config.controllersRumbleEnabled;
    this.controllerPosition.value = new Vector3(Mathf.Clamp(config.controllerPositionX, -0.1f, 0.1f), Mathf.Clamp(config.controllerPositionY, -0.1f, 0.1f), Mathf.Clamp(config.controllerPositionZ, -0.1f, 0.1f));
    this.controllerRotation.value = new Vector3(Mathf.Clamp(config.controllerRotationX, -180f, 180f), Mathf.Clamp(config.controllerRotationY, -180f, 180f), Mathf.Clamp(config.controllerRotationZ, -180f, 180f));
    this.mirrorGraphicsSettings.value = config.mirrorGraphicsSettings;
    this.mainEffectGraphicsSettings.value = config.mainEffectGraphicsSettings;
    this.bloomPrePassGraphicsSettings.value = config.bloomGraphicsSettings;
    this.smokeGraphicsSettings.value = config.smokeGraphicsSettings == 1;
    this.enableAlphaFeatures.value = config.enableAlphaFeatures == 1;
    this.burnMarkTrailsEnabled.value = config.burnMarkTrailsEnabled;
    this.screenDisplacementEffectsEnabled.value = config.screenDisplacementEffectsEnabled;
    this.maxShockwaveParticles.value = config.maxShockwaveParticles;
    this.smoothCameraEnabled.value = config.smoothCameraEnabled > 0;
    this.smoothCameraFieldOfView.value = config.smoothCameraFieldOfView;
    this.smoothCameraThirdPersonPosition.value = new Vector3(config.smoothCameraThirdPersonPositionX, config.smoothCameraThirdPersonPositionY, config.smoothCameraThirdPersonPositionZ);
    this.smoothCameraThirdPersonEulerAngles.value = new Vector3(config.smoothCameraThirdPersonEulerAnglesX, config.smoothCameraThirdPersonEulerAnglesY, config.smoothCameraThirdPersonEulerAnglesZ);
    this.smoothCameraThirdPersonEnabled.value = config.smoothCameraThirdPersonEnabled > 0;
    this.smoothCameraRotationSmooth.value = config.smoothCameraRotationSmooth;
    this.smoothCameraPositionSmooth.value = config.smoothCameraPositionSmooth;
    this.volume.value = config.volume;
    this.ambientVolumeScale.value = config.ambientVolumeScale;
    this.overrideAudioLatency.value = config.overrideAudioLatency;
    this.audioLatency.value = config.audioLatency;
    this.maxNumberOfCutSoundEffects.value = config.maxNumberOfCutSoundEffects;
    this.pauseButtonPressDurationLevel.value = config.pauseButtonPressDurationLevel;
    this.onlineServicesEnabled.value = config.onlineServicesEnabled;
    this.openVrThreadedHaptics.value = config.openVrThreadedHaptics;
    this.language.value = config.language == "SystemLanguage" ? Localization.Instance.ConvertSystemLanguage(Application.systemLanguage) : config.language.ToLanguage();
    this.useCustomServerEnvironment.value = config.useCustomServerEnvironment;
    this.forceGameLiftServerEnvironment.value = config.forceGameLiftServerEnvironment;
    this.customServerHostName.value = config.customServerHostName;
    this.enableFPSCounter.value = config.enableFPSCounter;
    this.depthTextureEnabled.value = (bool) (ObservableVariableSO<bool>) this.smokeGraphicsSettings;
    this.createScreenshotDuringTheGame = false;
    this._isLoaded = true;
  }

  public virtual void __DeleteSettingsFiles()
  {
    string path1 = Path.Combine(Application.persistentDataPath, "settings.cfg");
    string path2 = Path.Combine(Application.persistentDataPath, "settings.cfg.bak");
    try
    {
      File.Delete(path1);
      File.Delete(path2);
    }
    catch
    {
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    if (!this._playingForTheFirstTimeChecked)
    {
      this._playingForTheFirstTimeChecked = true;
      this.playingForTheFirstTime = !File.Exists(Application.persistentDataPath + "/settings.cfg");
    }
    this.Load(true);
  }

  public virtual void OnDisable()
  {
    if (!this._isLoaded)
      return;
    this.Save();
  }

  public enum WindowMode
  {
    Windowed,
    Fullscreen,
  }

  public class Config
  {
    public string version = "2.0.0";
    public int windowResolutionWidth = 1280;
    public int windowResolutionHeight = 720;
    public int editorResolutionWidth = 1280;
    public int editorResolutionHeight = 720;
    public MainSettingsModelSO.WindowMode windowMode = MainSettingsModelSO.WindowMode.Fullscreen;
    public float vrResolutionScale = 1f;
    public float menuVRResolutionScaleMultiplier = 1f;
    public bool useFixedFoveatedRenderingDuringGameplay;
    public int antiAliasingLevel = 4;
    public int mirrorGraphicsSettings = 3;
    public int mainEffectGraphicsSettings = 1;
    public int bloomGraphicsSettings;
    public int smokeGraphicsSettings = 1;
    public bool burnMarkTrailsEnabled = true;
    public bool screenDisplacementEffectsEnabled = true;
    public float roomCenterX;
    public float roomCenterY;
    public float roomCenterZ;
    public float roomRotation;
    public float controllerPositionX;
    public float controllerPositionY;
    public float controllerPositionZ;
    public float controllerRotationX;
    public float controllerRotationY;
    public float controllerRotationZ;
    public int smoothCameraEnabled;
    public float smoothCameraFieldOfView = 70f;
    public float smoothCameraThirdPersonPositionX;
    public float smoothCameraThirdPersonPositionY = 1.5f;
    public float smoothCameraThirdPersonPositionZ = -1.5f;
    public float smoothCameraThirdPersonEulerAnglesX;
    public float smoothCameraThirdPersonEulerAnglesY;
    public float smoothCameraThirdPersonEulerAnglesZ;
    public int smoothCameraThirdPersonEnabled;
    public float smoothCameraRotationSmooth = 4f;
    public float smoothCameraPositionSmooth = 4f;
    public bool useCustomServerEnvironment;
    public bool forceGameLiftServerEnvironment;
    public string customServerHostName = "";
    public float volume = 1f;
    public float ambientVolumeScale = 0.8f;
    public bool controllersRumbleEnabled = true;
    public int enableAlphaFeatures;
    public int pauseButtonPressDurationLevel;
    public int maxShockwaveParticles = 1;
    public bool overrideAudioLatency;
    public float audioLatency;
    public int maxNumberOfCutSoundEffects = 24;
    public bool onlineServicesEnabled;
    public bool openVrThreadedHaptics;
    public string language = "SystemLanguage";
    public bool enableFPSCounter;
  }
}
