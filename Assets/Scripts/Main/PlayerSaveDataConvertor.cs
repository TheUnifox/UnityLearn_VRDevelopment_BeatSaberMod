// Decompiled with JetBrains decompiler
// Type: PlayerSaveDataConvertor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class PlayerSaveDataConvertor
{
  public static EnvironmentEffectsFilterPreset GetRuntimeData(
    this PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData saveData)
  {
    if (saveData == PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.AllEffects)
      return EnvironmentEffectsFilterPreset.AllEffects;
    if (saveData == PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.StrobeFilter)
      return EnvironmentEffectsFilterPreset.StrobeFilter;
    return saveData == PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.NoEffects ? EnvironmentEffectsFilterPreset.NoEffects : EnvironmentEffectsFilterPreset.AllEffects;
  }

  public static PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData GetSaveData(
    this EnvironmentEffectsFilterPreset data)
  {
    if (data == EnvironmentEffectsFilterPreset.AllEffects)
      return PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.AllEffects;
    if (data == EnvironmentEffectsFilterPreset.StrobeFilter)
      return PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.StrobeFilter;
    return data == EnvironmentEffectsFilterPreset.NoEffects ? PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.NoEffects : PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.AllEffects;
  }

  public static NoteJumpDurationTypeSettings GetRuntimeData(
    this PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData saveData)
  {
    if (saveData == PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData.Dynamic)
      return NoteJumpDurationTypeSettings.Dynamic;
    return saveData == PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData.Static ? NoteJumpDurationTypeSettings.Static : NoteJumpDurationTypeSettings.Static;
  }

  public static PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData GetSaveData(
    this NoteJumpDurationTypeSettings data)
  {
    if (data == NoteJumpDurationTypeSettings.Dynamic)
      return PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData.Dynamic;
    return data == NoteJumpDurationTypeSettings.Static ? PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData.Static : PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData.Static;
  }

  public static ArcVisibilityType GetRuntimeData(
    this PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData saveData)
  {
    switch (saveData)
    {
      case PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.None:
        return ArcVisibilityType.None;
      case PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.Low:
        return ArcVisibilityType.Low;
      case PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.Standard:
        return ArcVisibilityType.Standard;
      case PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.High:
        return ArcVisibilityType.High;
      default:
        return ArcVisibilityType.Standard;
    }
  }

  public static PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData GetSaveData(
    this ArcVisibilityType data)
  {
    switch (data)
    {
      case ArcVisibilityType.None:
        return PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.None;
      case ArcVisibilityType.Low:
        return PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.Low;
      case ArcVisibilityType.Standard:
        return PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.Standard;
      case ArcVisibilityType.High:
        return PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.High;
      default:
        return PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData.Standard;
    }
  }
}
