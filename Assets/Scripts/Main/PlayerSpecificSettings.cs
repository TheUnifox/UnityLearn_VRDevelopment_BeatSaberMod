// Decompiled with JetBrains decompiler
// Type: PlayerSpecificSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class PlayerSpecificSettings
{
  [SerializeField]
  protected bool _leftHanded;
  [SerializeField]
  protected float _playerHeight;
  [SerializeField]
  protected bool _automaticPlayerHeight;
  [SerializeField]
  protected float _sfxVolume;
  [SerializeField]
  protected bool _reduceDebris;
  [SerializeField]
  protected bool _noTextsAndHuds;
  [SerializeField]
  protected bool _noFailEffects;
  [SerializeField]
  protected bool _advancedHud;
  [SerializeField]
  protected bool _autoRestart;
  [SerializeField]
  protected float _saberTrailIntensity;
  [SerializeField]
  protected NoteJumpDurationTypeSettings _noteJumpDurationTypeSettings;
  [SerializeField]
  protected float _noteJumpFixedDuration;
  [SerializeField]
  protected float _noteJumpStartBeatOffset;
  [SerializeField]
  protected bool _hideNoteSpawnEffect;
  [SerializeField]
  protected bool _adaptiveSfx;
  [SerializeField]
  protected bool _arcsHapticFeedback;
  [SerializeField]
  protected ArcVisibilityType _arcsVisible;
  [SerializeField]
  protected EnvironmentEffectsFilterPreset _environmentEffectsFilterDefaultPreset;
  [SerializeField]
  protected EnvironmentEffectsFilterPreset _environmentEffectsFilterExpertPlusPreset;

  public bool leftHanded => this._leftHanded;

  public float playerHeight => this._playerHeight;

  public bool automaticPlayerHeight => this._automaticPlayerHeight;

  public float sfxVolume => this._sfxVolume;

  public bool reduceDebris => this._reduceDebris;

  public bool noTextsAndHuds => this._noTextsAndHuds;

  public bool noFailEffects => this._noFailEffects;

  public bool advancedHud => this._advancedHud;

  public bool autoRestart => this._autoRestart;

  public float saberTrailIntensity => this._saberTrailIntensity;

  public NoteJumpDurationTypeSettings noteJumpDurationTypeSettings => this._noteJumpDurationTypeSettings;

  public float noteJumpFixedDuration => this._noteJumpFixedDuration;

  public float noteJumpStartBeatOffset => this._noteJumpStartBeatOffset;

  public bool hideNoteSpawnEffect => this._hideNoteSpawnEffect;

  public bool adaptiveSfx => this._adaptiveSfx;

  public bool arcsHapticFeedback => this._arcsHapticFeedback;

  public ArcVisibilityType arcsVisible => this._arcsVisible;

  public EnvironmentEffectsFilterPreset environmentEffectsFilterDefaultPreset => this._environmentEffectsFilterDefaultPreset;

  public EnvironmentEffectsFilterPreset environmentEffectsFilterExpertPlusPreset => this._environmentEffectsFilterExpertPlusPreset;

  public PlayerSpecificSettings()
  {
    this._leftHanded = false;
    this._playerHeight = 1.7f;
    this._automaticPlayerHeight = true;
    this._sfxVolume = 0.7f;
    this._reduceDebris = false;
    this._noTextsAndHuds = false;
    this._noFailEffects = false;
    this._advancedHud = false;
    this._autoRestart = false;
    this._saberTrailIntensity = 0.5f;
    this._noteJumpDurationTypeSettings = NoteJumpDurationTypeSettings.Dynamic;
    this._noteJumpFixedDuration = 0.5f;
    this._noteJumpStartBeatOffset = 0.0f;
    this._hideNoteSpawnEffect = false;
    this._adaptiveSfx = true;
    this._arcsHapticFeedback = true;
    this._arcsVisible = ArcVisibilityType.Standard;
    this._environmentEffectsFilterDefaultPreset = EnvironmentEffectsFilterPreset.StrobeFilter;
    this._environmentEffectsFilterExpertPlusPreset = EnvironmentEffectsFilterPreset.AllEffects;
  }

  public PlayerSpecificSettings(
    bool leftHanded,
    float playerHeight,
    bool automaticPlayerHeight,
    float sfxVolume,
    bool reduceDebris,
    bool noTextsAndHuds,
    bool noFailEffects,
    bool advancedHud,
    bool autoRestart,
    float saberTrailIntensity,
    NoteJumpDurationTypeSettings noteJumpDurationTypeSettings,
    float noteJumpFixedDuration,
    float noteJumpStartBeatOffset,
    bool hideNoteSpawnEffect,
    bool adaptiveSfx,
    bool arcsHapticFeedback,
    ArcVisibilityType arcsVisible,
    EnvironmentEffectsFilterPreset environmentEffectsFilterDefaultPreset,
    EnvironmentEffectsFilterPreset environmentEffectsFilterExpertPlusPreset)
  {
    this._leftHanded = leftHanded;
    this._playerHeight = playerHeight;
    this._automaticPlayerHeight = automaticPlayerHeight;
    this._sfxVolume = sfxVolume;
    this._reduceDebris = reduceDebris;
    this._noTextsAndHuds = noTextsAndHuds;
    this._noFailEffects = noFailEffects;
    this._advancedHud = advancedHud;
    this._autoRestart = autoRestart;
    this._saberTrailIntensity = saberTrailIntensity;
    this._noteJumpDurationTypeSettings = noteJumpDurationTypeSettings;
    this._noteJumpFixedDuration = noteJumpFixedDuration;
    this._noteJumpStartBeatOffset = noteJumpStartBeatOffset;
    this._hideNoteSpawnEffect = hideNoteSpawnEffect;
    this._adaptiveSfx = adaptiveSfx;
    this._arcsHapticFeedback = arcsHapticFeedback;
    this._arcsVisible = arcsVisible;
    this._environmentEffectsFilterDefaultPreset = environmentEffectsFilterDefaultPreset;
    this._environmentEffectsFilterExpertPlusPreset = environmentEffectsFilterExpertPlusPreset;
  }

  public virtual PlayerSpecificSettings CopyWith(
    bool? leftHanded = null,
    float? playerHeight = null,
    bool? automaticPlayerHeight = null,
    float? sfxVolume = null,
    bool? reduceDebris = null,
    bool? noTextsAndHuds = null,
    bool? noFailEffects = null,
    bool? advancedHud = null,
    bool? autoRestart = null,
    float? saberTrailIntensity = null,
    NoteJumpDurationTypeSettings? noteJumpDurationTypeSettings = null,
    float? noteJumpFixedDuration = null,
    float? noteJumpStartBeatOffset = null,
    bool? hideNoteSpawnEffect = null,
    bool? adaptiveSfx = null,
    bool? arcsHapticFeedback = null,
    ArcVisibilityType? arcsVisible = null,
    EnvironmentEffectsFilterPreset? environmentEffectsFilterDefaultPreset = null,
    EnvironmentEffectsFilterPreset? environmentEffectsFilterExpertPlusPreset = null)
  {
    return new PlayerSpecificSettings(((int) leftHanded ?? (this._leftHanded ? 1 : 0)) != 0, (float) ((double) playerHeight ?? (double) this._playerHeight), ((int) automaticPlayerHeight ?? (this._automaticPlayerHeight ? 1 : 0)) != 0, (float) ((double) sfxVolume ?? (double) this._sfxVolume), ((int) reduceDebris ?? (this._reduceDebris ? 1 : 0)) != 0, ((int) noTextsAndHuds ?? (this._noTextsAndHuds ? 1 : 0)) != 0, ((int) noFailEffects ?? (this._noFailEffects ? 1 : 0)) != 0, ((int) advancedHud ?? (this._advancedHud ? 1 : 0)) != 0, ((int) autoRestart ?? (this._autoRestart ? 1 : 0)) != 0, (float) ((double) saberTrailIntensity ?? (double) this._saberTrailIntensity), (NoteJumpDurationTypeSettings) ((int) noteJumpDurationTypeSettings ?? (int) this._noteJumpDurationTypeSettings), (float) ((double) noteJumpFixedDuration ?? (double) this._noteJumpFixedDuration), (float) ((double) noteJumpStartBeatOffset ?? (double) this._noteJumpStartBeatOffset), ((int) hideNoteSpawnEffect ?? (this._hideNoteSpawnEffect ? 1 : 0)) != 0, ((int) adaptiveSfx ?? (this._adaptiveSfx ? 1 : 0)) != 0, ((int) arcsHapticFeedback ?? (this._arcsHapticFeedback ? 1 : 0)) != 0, (ArcVisibilityType) ((int) arcsVisible ?? (int) this._arcsVisible), (EnvironmentEffectsFilterPreset) ((int) environmentEffectsFilterDefaultPreset ?? (int) this._environmentEffectsFilterDefaultPreset), (EnvironmentEffectsFilterPreset) ((int) environmentEffectsFilterExpertPlusPreset ?? (int) this._environmentEffectsFilterExpertPlusPreset));
  }

  public virtual EnvironmentEffectsFilterPreset GetEnvironmentEffectsFilterPreset(
    BeatmapDifficulty difficulty)
  {
    return difficulty == BeatmapDifficulty.ExpertPlus ? this._environmentEffectsFilterExpertPlusPreset : this._environmentEffectsFilterDefaultPreset;
  }
}
