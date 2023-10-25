// Decompiled with JetBrains decompiler
// Type: MainSettingsDefaultValues
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class MainSettingsDefaultValues
{
  public const float kDefaultRoomCenterX = 0.0f;
  public const float kDefaultRoomCenterY = 0.0f;
  public const float kDefaultRoomCenterZ = 0.0f;
  public const float kDefaultControllerPositionX = 0.0f;
  public const float kDefaultControllerPositionY = 0.0f;
  public const float kDefaultControllerPositionZ = 0.0f;
  public const float kDefaultControllerRotationX = 0.0f;
  public const float kDefaultControllerRotationY = 0.0f;
  public const float kDefaultControllerRotationZ = 0.0f;
  public const int kDefaultWindowResolutionWidth = 1280;
  public const int kDefaultWindowResolutionHeight = 720;
  public const int kDefaultMirrorGraphicsSettings = 3;
  public const int kDefaultMainEffectGraphicsSettings = 1;
  public const int kDefaultBloomGraphicsSettings = 0;
  public const bool kDefaultSmokeGraphicsSettings = true;
  public const int kDefaultAntiAliasingLevel = 4;
  public const float kDefaultVrResolutionScale = 1f;
  public const float kDefaultMenuVRResolutionScaleMultiplier = 1f;
  public const bool kDefaultUseFixedFoveatedRenderingDuringGameplay = false;
  public const bool kDefaultBurnMarkTrailsEnabled = true;
  public const bool kDefaultScreenDisplacementEffectsEnabled = true;
  public const float kDefaultAudioLatency = 0.0f;
  public const int kMaxShockwaveParticles = 1;
  public const int kMaxNumberOfCutSoundEffects = 24;
  public const bool kCreateScreenshotDuringTheGame = false;
  public const string kSystemLanguageSerializedName = "SystemLanguage";
  public const int kDefaultPauseButtonPressDurationLevel = 0;

  public static void SetFixedDefaultValues(MainSettingsModelSO mainSettingsModel)
  {
    mainSettingsModel.burnMarkTrailsEnabled.value = true;
    if ((bool) (ObservableVariableSO<bool>) mainSettingsModel.overrideAudioLatency)
      return;
    mainSettingsModel.audioLatency.value = 0.0f;
  }
}
