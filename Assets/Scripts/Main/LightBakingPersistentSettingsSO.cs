// Decompiled with JetBrains decompiler
// Type: LightBakingPersistentSettingsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.XR;

public class LightBakingPersistentSettingsSO : PersistentScriptableObject
{
  [SerializeField]
  protected int _reflectionsCount = 1;
  [Space]
  [SerializeField]
  protected BloomPrePassEffectSO _bloomPrePassEffectToneMappingOff;
  [SerializeField]
  protected MainEffectSO _mainEffectForBaking;
  [Space]
  [SerializeField]
  protected float _colorFromSchemeAlpha = 0.75f;
  [SerializeField]
  protected Color[] _bakedLightEditorColors = new Color[6]
  {
    Color.white,
    Color.white,
    Color.white,
    Color.white,
    Color.white,
    Color.white
  };
  [Space]
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
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
  [Space]
  [SerializeField]
  protected BakedLightDataLoader _bakedLightDataLoaderPrefab;
  [SerializeField]
  protected BakedReflectionProbe _bakedReflectionProbePrefab;
  [SerializeField]
  protected LightmapLightsWithIds _lightmapLightsWithIds;
  [SerializeField]
  protected FakeMirrorObjectsInstaller _fakeMirrorObjectsInstallerPrefab;
  [SerializeField]
  protected FakeMirrorSettings _fakeMirrorSettingsPrefab;
  [SerializeField]
  protected Material _defaultDepthOnlyWriteMaterialForFakeMirror;
  protected const int kDefaultMirrorGraphicsSettings = 0;
  protected const int kDefaultMainEffectGraphicsSettings = 1;
  protected const int kDefaultAntiAliasingLevel = 8;
  protected const float kDefaultVrResolutionScale = 1f;
  protected const float kDefaultMenuVRResolutionScaleMultiplier = 1f;
  protected const bool kDefaultUseFixedFoveatedRenderingDuringGameplay = false;
  protected const bool kDefaultBurnMarkTrailsEnabled = true;
  protected const bool kDefaultScreenDisplacementEffectsEnabled = true;

  public int reflectionsCount
  {
    get => this._reflectionsCount;
    set => this._reflectionsCount = value;
  }

  public float colorFromSchemeAlpha
  {
    get => this._colorFromSchemeAlpha;
    set => this._colorFromSchemeAlpha = value;
  }

  public Color[] bakedLightEditorColors => this._bakedLightEditorColors;

  public Material defaultDepthOnlyWriteMaterialForFakeMirror => this._defaultDepthOnlyWriteMaterialForFakeMirror;

  public bool mainEffectForBakingIsOn
  {
    get => (Object) this._mainEffectContainer.mainEffect == (Object) this._mainEffectForBaking;
    set
    {
      if (value)
        this._mainEffectContainer.Init(this._mainEffectForBaking);
      else
        this._mainEffectContainer.Init(this._mainEffectGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.mainEffectGraphicsSettings].mainEffect);
    }
  }

  public BakedLightDataLoader bakedLightDataLoaderPrefab => this._bakedLightDataLoaderPrefab;

  public BakedReflectionProbe bakedReflectionProbePrefab => this._bakedReflectionProbePrefab;

  public LightmapLightsWithIds lightmapLightsWithIds => this._lightmapLightsWithIds;

  public FakeMirrorObjectsInstaller fakeMirrorObjectsInstallerPrefab => this._fakeMirrorObjectsInstallerPrefab;

  public FakeMirrorSettings fakeMirrorSettingsPrefab => this._fakeMirrorSettingsPrefab;

  public virtual void SetGraphicsSettingsForBaking()
  {
    this._mainSettingsModel.mirrorGraphicsSettings.value = 0;
    this._mainSettingsModel.mainEffectGraphicsSettings.value = 1;
    this._mainSettingsModel.antiAliasingLevel.value = 8;
    this._mainSettingsModel.vrResolutionScale.value = 1f;
    this._mainSettingsModel.menuVRResolutionScaleMultiplier.value = 1f;
    this._mainSettingsModel.burnMarkTrailsEnabled.value = true;
    this._mainSettingsModel.screenDisplacementEffectsEnabled.value = true;
    this._mainSettingsModel.useFixedFoveatedRenderingDuringGameplay.value = false;
    XRSettings.eyeTextureResolutionScale = (float) (ObservableVariableSO<float>) this._mainSettingsModel.vrResolutionScale * (float) (ObservableVariableSO<float>) this._mainSettingsModel.menuVRResolutionScaleMultiplier;
    MirrorRendererGraphicsSettingsPresets.Preset preset = this._mirrorRendererGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.mirrorGraphicsSettings];
    this._mirrorRenderer.Init(preset.reflectLayers, preset.stereoTextureWidth, preset.stereoTextureHeight, preset.monoTextureWidth, preset.monoTextureHeight, preset.maxAntiAliasing, preset.enableBloomPrePassFog);
    this._mainEffectContainer.Init(this._mainEffectForBaking);
    QualitySettings.antiAliasing = (int) (ObservableVariableSO<int>) this._mainSettingsModel.antiAliasingLevel;
    this.SetToneMappingOn(false);
  }

  public virtual void SetPlatformGraphics()
  {
    this._mainSettingsModel.Load(false);
    MainSettingsDefaultValues.SetFixedDefaultValues(this._mainSettingsModel);
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.mirrorGraphicsSettings >= this._mirrorRendererGraphicsSettingsPresets.presets.Length)
      this._mainSettingsModel.mirrorGraphicsSettings.value = this._mirrorRendererGraphicsSettingsPresets.presets.Length - 1;
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.mainEffectGraphicsSettings >= this._mainEffectGraphicsSettingsPresets.presets.Length)
      this._mainSettingsModel.mainEffectGraphicsSettings.value = this._mainEffectGraphicsSettingsPresets.presets.Length - 1;
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.bloomPrePassGraphicsSettings >= this._bloomPrePassGraphicsSettingsPresets.presets.Length)
      this._mainSettingsModel.bloomPrePassGraphicsSettings.value = this._bloomPrePassGraphicsSettingsPresets.presets.Length - 1;
    MirrorRendererGraphicsSettingsPresets.Preset preset1 = this._mirrorRendererGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.mirrorGraphicsSettings];
    MainEffectGraphicsSettingsPresetsSO.Preset preset2 = this._mainEffectGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.mainEffectGraphicsSettings];
    BloomPrePassGraphicsSettingsPresetsSO.Preset preset3 = this._bloomPrePassGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.bloomPrePassGraphicsSettings];
    this._mirrorRenderer.Init(preset1.reflectLayers, preset1.stereoTextureWidth, preset1.stereoTextureHeight, preset1.monoTextureWidth, preset1.monoTextureHeight, preset1.maxAntiAliasing, preset1.enableBloomPrePassFog);
    this._mainEffectContainer.Init(preset2.mainEffect);
    this._bloomPrePassEffectContainer.Init(preset3.bloomPrePassEffect);
  }

  public virtual void SetToneMappingOn(bool isOn)
  {
    if (isOn)
      this._bloomPrePassEffectContainer.Init(this._bloomPrePassGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mainSettingsModel.bloomPrePassGraphicsSettings].bloomPrePassEffect);
    else
      this._bloomPrePassEffectContainer.Init(this._bloomPrePassEffectToneMappingOff);
  }

  public virtual bool IsToneMappingOn() => this._bloomPrePassEffectContainer.bloomPrePassEffect.toneMapping == ToneMapping.Aces;
}
