// Decompiled with JetBrains decompiler
// Type: PlayerSettingsPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsPanelController : MonoBehaviour, IRefreshable
{
  [SerializeField]
  protected Toggle _leftHandedToggle;
  [SerializeField]
  protected Toggle _reduceDebrisToggle;
  [SerializeField]
  protected Toggle _noTextsAndHudsToggle;
  [SerializeField]
  protected Toggle _advanceHudToggle;
  [SerializeField]
  protected Toggle _autoRestartToggle;
  [SerializeField]
  protected PlayerHeightSettingsController _playerHeightSettingsController;
  [SerializeField]
  protected CanvasGroup _playerHeightSettingsCanvasGroup;
  [SerializeField]
  protected Toggle _automaticPlayerHeightToggle;
  [SerializeField]
  protected FormattedFloatListSettingsController _sfxVolumeSettingsController;
  [SerializeField]
  protected FormattedFloatListSettingsController _saberTrailIntensitySettingsController;
  [SerializeField]
  protected NoteJumpDurationTypeSettingsDropdown _noteJumpDurationTypeSettingsDropdown;
  [SerializeField]
  protected FormattedFloatListSettingsController _noteJumpFixedDurationSettingsController;
  [SerializeField]
  protected CanvasGroup _noteJumpFixedDurationSettingsCanvasGroup;
  [SerializeField]
  protected NoteJumpStartBeatOffsetDropdown _noteJumpStartBeatOffsetDropdown;
  [SerializeField]
  protected CanvasGroup _noteJumpStartBeatOffsetCanvasGroup;
  [SerializeField]
  protected EnvironmentEffectsFilterPresetDropdown _environmentEffectsFilterDefaultPresetDropdown;
  [SerializeField]
  protected EnvironmentEffectsFilterPresetDropdown _environmentEffectsFilterExpertPlusPresetDropdown;
  [SerializeField]
  protected Toggle _hideNoteSpawnEffectToggle;
  [SerializeField]
  protected Toggle _adaptiveSfxToggle;
  [SerializeField]
  protected Toggle _arcsHapticFeedbackToggle;
  [SerializeField]
  protected ArcVisibilityTypeSettingsDropdown _arcsVisibilityTypeSettingsDropdown;
  [Space]
  [SerializeField]
  protected CanvasGroup _singleplayerOnlyCanvasGroup;
  [Space]
  [SerializeField]
  protected GameObject _arcVisibilityWarning;
  protected const float kDisabledSectionAlpha = 0.2f;
  protected ArcVisibilityType _currentArcType;
  protected PlayerSpecificSettings _playerSpecificSettings;
  protected ToggleBinder _toggleBinder;
  protected bool _dirty;
  protected bool _refreshed;
  protected readonly EventBinder _eventBinder = new EventBinder();

  public event System.Action didChangePlayerSettingsEvent;

  public PlayerSpecificSettings playerSpecificSettings
  {
    get
    {
      if (this._dirty)
      {
        PlayerSpecificSettings specificSettings = this._playerSpecificSettings;
        bool? leftHanded = new bool?(this._leftHandedToggle.isOn);
        bool? nullable1 = new bool?(this._reduceDebrisToggle.isOn);
        bool? nullable2 = new bool?(this._noTextsAndHudsToggle.isOn);
        bool? nullable3 = new bool?(this._advanceHudToggle.isOn);
        bool? nullable4 = new bool?(this._autoRestartToggle.isOn);
        bool? nullable5 = new bool?(this._automaticPlayerHeightToggle.isOn);
        float? playerHeight = new float?(this._playerHeightSettingsController.value);
        bool? automaticPlayerHeight = nullable5;
        float? sfxVolume = new float?(this._sfxVolumeSettingsController.value);
        bool? reduceDebris = nullable1;
        bool? noTextsAndHuds = nullable2;
        bool? noFailEffects = new bool?();
        bool? advancedHud = nullable3;
        bool? autoRestart = nullable4;
        float? saberTrailIntensity = new float?(this._saberTrailIntensitySettingsController.value);
        NoteJumpDurationTypeSettings? noteJumpDurationTypeSettings = new NoteJumpDurationTypeSettings?(this._noteJumpDurationTypeSettingsDropdown.GetSelectedItemValue());
        float? noteJumpFixedDuration = new float?(this._noteJumpFixedDurationSettingsController.value);
        float? noteJumpStartBeatOffset = new float?(this._noteJumpStartBeatOffsetDropdown.GetSelectedItemValue());
        bool? hideNoteSpawnEffect = new bool?(this._hideNoteSpawnEffectToggle.isOn);
        bool? adaptiveSfx = new bool?(this._adaptiveSfxToggle.isOn);
        bool? arcsHapticFeedback = new bool?(this._arcsHapticFeedbackToggle.isOn);
        ArcVisibilityType? arcsVisible = new ArcVisibilityType?(this._arcsVisibilityTypeSettingsDropdown.GetSelectedItemValue());
        EnvironmentEffectsFilterPreset? environmentEffectsFilterDefaultPreset = new EnvironmentEffectsFilterPreset?(this._environmentEffectsFilterDefaultPresetDropdown.GetSelectedItemValue());
        EnvironmentEffectsFilterPreset? environmentEffectsFilterExpertPlusPreset = new EnvironmentEffectsFilterPreset?(this._environmentEffectsFilterExpertPlusPresetDropdown.GetSelectedItemValue());
        this._playerSpecificSettings = specificSettings.CopyWith(leftHanded, playerHeight, automaticPlayerHeight, sfxVolume, reduceDebris, noTextsAndHuds, noFailEffects, advancedHud, autoRestart, saberTrailIntensity, noteJumpDurationTypeSettings, noteJumpFixedDuration, noteJumpStartBeatOffset, hideNoteSpawnEffect, adaptiveSfx, arcsHapticFeedback, arcsVisible, environmentEffectsFilterDefaultPreset, environmentEffectsFilterExpertPlusPreset);
      }
      this._dirty = false;
      return this._playerSpecificSettings;
    }
  }

  public virtual void SetData(PlayerSpecificSettings playerSpecificSettings)
  {
    this._playerSpecificSettings = playerSpecificSettings;
    this._dirty = false;
    this._refreshed = false;
    this.Refresh();
  }

  public virtual void SetLayout(
    PlayerSettingsPanelController.PlayerSettingsPanelLayout layout)
  {
    switch (layout)
    {
      case PlayerSettingsPanelController.PlayerSettingsPanelLayout.All:
        this.SetSectionDisabled(this._singleplayerOnlyCanvasGroup, false);
        break;
      case PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer:
        this.SetSectionDisabled(this._singleplayerOnlyCanvasGroup, false);
        break;
      case PlayerSettingsPanelController.PlayerSettingsPanelLayout.Multiplayer:
        this.SetSectionDisabled(this._singleplayerOnlyCanvasGroup, true);
        break;
    }
  }

  public virtual void Awake() => this._toggleBinder = new ToggleBinder();

  public virtual void OnEnable()
  {
    this._toggleBinder.AddBinding(this._leftHandedToggle, (System.Action<bool>) (on => this.SetIsDirty()));
    this._toggleBinder.AddBinding(this._reduceDebrisToggle, (System.Action<bool>) (on => this.SetIsDirty()));
    this._toggleBinder.AddBinding(this._noTextsAndHudsToggle, new System.Action<bool>(this.HandleNoTextsAndHudsToggleChanged));
    this._toggleBinder.AddBinding(this._advanceHudToggle, new System.Action<bool>(this.HandleAdvancedHudToggleChanged));
    this._toggleBinder.AddBinding(this._autoRestartToggle, (System.Action<bool>) (on => this.SetIsDirty()));
    this._toggleBinder.AddBinding(this._hideNoteSpawnEffectToggle, (System.Action<bool>) (on => this.SetIsDirty()));
    this._toggleBinder.AddBinding(this._adaptiveSfxToggle, (System.Action<bool>) (on => this.SetIsDirty()));
    this._toggleBinder.AddBinding(this._automaticPlayerHeightToggle, (System.Action<bool>) (on =>
    {
      this.SetSectionDisabled(this._playerHeightSettingsCanvasGroup, on);
      this.SetIsDirty();
    }));
    this._toggleBinder.AddBinding(this._arcsHapticFeedbackToggle, (System.Action<bool>) (on => this.SetIsDirty()));
    this._eventBinder.Bind((System.Action) (() =>
    {
      this._sfxVolumeSettingsController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.HandleSFXVolumeSettingsControllerValueDidChange);
      this._arcsVisibilityTypeSettingsDropdown.didSelectCellWithIdxEvent += new System.Action<int, ArcVisibilityType>(this.HandleArcVisibilityDropdownDidSelectCellWithIdx);
      this._saberTrailIntensitySettingsController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.HandleSaberTrailIntensitySettingsControllerValueDidChange);
      this._noteJumpDurationTypeSettingsDropdown.didSelectCellWithIdxEvent += new System.Action<int, NoteJumpDurationTypeSettings>(this.HandleNoteJumpDurationTypeSettingsDropdownDidSelectCellWithIdx);
      this._noteJumpFixedDurationSettingsController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.HandleNoteJumpFixedDurationSettingsControllerValueDidChange);
      this._playerHeightSettingsController.valueDidChangeEvent += new System.Action<float>(this.HandlePlayerHeightSettingsControllerValueDidChange);
      this._noteJumpStartBeatOffsetDropdown.didSelectCellWithIdxEvent += new System.Action<int, float>(this.HandleNoteJumpStartBeatOffsetPositionSelected);
      this._environmentEffectsFilterDefaultPresetDropdown.didSelectCellWithIdxEvent += new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
      this._environmentEffectsFilterExpertPlusPresetDropdown.didSelectCellWithIdxEvent += new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
    }), (System.Action) (() =>
    {
      if ((UnityEngine.Object) this._sfxVolumeSettingsController != (UnityEngine.Object) null)
        this._sfxVolumeSettingsController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.HandleSFXVolumeSettingsControllerValueDidChange);
      if ((UnityEngine.Object) this._arcsVisibilityTypeSettingsDropdown != (UnityEngine.Object) null)
        this._arcsVisibilityTypeSettingsDropdown.didSelectCellWithIdxEvent -= new System.Action<int, ArcVisibilityType>(this.HandleArcVisibilityDropdownDidSelectCellWithIdx);
      if ((UnityEngine.Object) this._saberTrailIntensitySettingsController != (UnityEngine.Object) null)
        this._saberTrailIntensitySettingsController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.HandleSaberTrailIntensitySettingsControllerValueDidChange);
      if ((UnityEngine.Object) this._noteJumpDurationTypeSettingsDropdown != (UnityEngine.Object) null)
        this._noteJumpDurationTypeSettingsDropdown.didSelectCellWithIdxEvent -= new System.Action<int, NoteJumpDurationTypeSettings>(this.HandleNoteJumpDurationTypeSettingsDropdownDidSelectCellWithIdx);
      if ((UnityEngine.Object) this._noteJumpFixedDurationSettingsController != (UnityEngine.Object) null)
        this._noteJumpFixedDurationSettingsController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.HandleNoteJumpFixedDurationSettingsControllerValueDidChange);
      if ((UnityEngine.Object) this._playerHeightSettingsController != (UnityEngine.Object) null)
        this._playerHeightSettingsController.valueDidChangeEvent -= new System.Action<float>(this.HandlePlayerHeightSettingsControllerValueDidChange);
      if ((UnityEngine.Object) this._environmentEffectsFilterDefaultPresetDropdown != (UnityEngine.Object) null)
        this._environmentEffectsFilterDefaultPresetDropdown.didSelectCellWithIdxEvent -= new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
      if (!((UnityEngine.Object) this._environmentEffectsFilterExpertPlusPresetDropdown != (UnityEngine.Object) null))
        return;
      this._environmentEffectsFilterExpertPlusPresetDropdown.didSelectCellWithIdxEvent -= new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
    }));
  }

  public virtual void OnDisable() => this.UnsubscribeAllUICallbacks();

  public virtual void OnDestroy() => this.UnsubscribeAllUICallbacks();

  public virtual void UnsubscribeAllUICallbacks()
  {
    this._toggleBinder?.ClearBindings();
    this._eventBinder?.ClearAllBindings();
  }

  public virtual void Refresh()
  {
    if (this._refreshed)
      return;
    this._refreshed = true;
    this._leftHandedToggle.isOn = this._playerSpecificSettings.leftHanded;
    this._reduceDebrisToggle.isOn = this._playerSpecificSettings.reduceDebris;
    this._noTextsAndHudsToggle.isOn = this._playerSpecificSettings.noTextsAndHuds;
    this._advanceHudToggle.isOn = this._playerSpecificSettings.advancedHud;
    this._autoRestartToggle.isOn = this._playerSpecificSettings.autoRestart;
    this._automaticPlayerHeightToggle.isOn = this._playerSpecificSettings.automaticPlayerHeight;
    this._hideNoteSpawnEffectToggle.isOn = this._playerSpecificSettings.hideNoteSpawnEffect;
    this._adaptiveSfxToggle.isOn = this._playerSpecificSettings.adaptiveSfx;
    this._playerHeightSettingsController.Init(this._playerSpecificSettings.playerHeight);
    this._sfxVolumeSettingsController.SetValue(this._playerSpecificSettings.sfxVolume);
    this._saberTrailIntensitySettingsController.SetValue(this._playerSpecificSettings.saberTrailIntensity);
    this._noteJumpDurationTypeSettingsDropdown.SelectCellWithValue(this._playerSpecificSettings.noteJumpDurationTypeSettings);
    this._noteJumpStartBeatOffsetDropdown.SelectCellWithValue(this._playerSpecificSettings.noteJumpStartBeatOffset);
    this._noteJumpFixedDurationSettingsController.SetValue(this._playerSpecificSettings.noteJumpFixedDuration);
    this._arcsHapticFeedbackToggle.isOn = this._playerSpecificSettings.arcsHapticFeedback;
    this._arcsVisibilityTypeSettingsDropdown.SelectCellWithValue(this._playerSpecificSettings.arcsVisible);
    this._environmentEffectsFilterDefaultPresetDropdown.SelectCellWithValue(this._playerSpecificSettings.environmentEffectsFilterDefaultPreset);
    this._environmentEffectsFilterExpertPlusPresetDropdown.SelectCellWithValue(this._playerSpecificSettings.environmentEffectsFilterExpertPlusPreset);
    this.SetSectionDisabled(this._playerHeightSettingsCanvasGroup, this._playerSpecificSettings.automaticPlayerHeight);
    this.RefreshNoteJumpUI(this._playerSpecificSettings.noteJumpDurationTypeSettings);
    this.RefreshArcsWarning(this._playerSpecificSettings.arcsVisible, true);
  }

  public virtual void HandleSFXVolumeSettingsControllerValueDidChange(
    FormattedFloatListSettingsController settingsController,
    float value)
  {
    this.SetIsDirty();
  }

  public virtual void HandleArcVisibilityDropdownDidSelectCellWithIdx(
    int idx,
    ArcVisibilityType arcVisibilityType)
  {
    this.RefreshArcsWarning(arcVisibilityType, false);
    this.SetIsDirty();
  }

  public virtual void HandleSaberTrailIntensitySettingsControllerValueDidChange(
    FormattedFloatListSettingsController settingsController,
    float value)
  {
    this.SetIsDirty();
  }

  public virtual void HandlePlayerHeightSettingsControllerValueDidChange(float value) => this.SetIsDirty();

  public virtual void HandleNoteJumpStartBeatOffsetPositionSelected(int idx, float startBeatOffset) => this.SetIsDirty();

  public virtual void HandleLightReductionAmountSelected(
    int obj,
    EnvironmentEffectsFilterPreset environmentEffectsFilterPreset)
  {
    this.SetIsDirty();
  }

  public virtual void HandleAdvancedHudToggleChanged(bool on)
  {
    if (on)
      this._noTextsAndHudsToggle.isOn = false;
    this.SetIsDirty();
  }

  public virtual void HandleNoteJumpDurationTypeSettingsDropdownDidSelectCellWithIdx(
    int idx,
    NoteJumpDurationTypeSettings noteJumpDurationTypeSettings)
  {
    this.RefreshNoteJumpUI(noteJumpDurationTypeSettings);
    this.SetIsDirty();
  }

  public virtual void HandleNoteJumpFixedDurationSettingsControllerValueDidChange(
    FormattedFloatListSettingsController formattedFloatListSettingsController,
    float value)
  {
    this.SetIsDirty();
  }

  public virtual void HandleNoTextsAndHudsToggleChanged(bool on)
  {
    if (on)
      this._advanceHudToggle.isOn = false;
    this.SetIsDirty();
  }

  public virtual void SetIsDirty()
  {
    this._dirty = true;
    System.Action playerSettingsEvent = this.didChangePlayerSettingsEvent;
    if (playerSettingsEvent == null)
      return;
    playerSettingsEvent();
  }

  public virtual void RefreshNoteJumpUI(
    NoteJumpDurationTypeSettings noteJumpDurationTypeSettings)
  {
    this.SetSectionDisabled(this._noteJumpFixedDurationSettingsCanvasGroup, noteJumpDurationTypeSettings == NoteJumpDurationTypeSettings.Dynamic);
    this.SetSectionDisabled(this._noteJumpStartBeatOffsetCanvasGroup, noteJumpDurationTypeSettings == NoteJumpDurationTypeSettings.Static);
  }

  public virtual void SetSectionDisabled(CanvasGroup sectionCanvasGroup, bool disable)
  {
    sectionCanvasGroup.alpha = disable ? 0.2f : 1f;
    sectionCanvasGroup.interactable = !disable;
    sectionCanvasGroup.blocksRaycasts = !disable;
  }

  public virtual void RefreshArcsWarning(ArcVisibilityType arcVisibilityType, bool forceRebuild)
  {
    this._arcVisibilityWarning.SetActive(arcVisibilityType == ArcVisibilityType.None);
    if (this._currentArcType != arcVisibilityType | forceRebuild)
      LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
    this._currentArcType = arcVisibilityType;
  }

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_0(bool on) => this.SetIsDirty();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_1(bool on) => this.SetIsDirty();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_2(bool on) => this.SetIsDirty();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_3(bool on) => this.SetIsDirty();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_4(bool on) => this.SetIsDirty();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_5(bool on)
  {
    this.SetSectionDisabled(this._playerHeightSettingsCanvasGroup, on);
    this.SetIsDirty();
  }

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_6(bool on) => this.SetIsDirty();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_7()
  {
    this._sfxVolumeSettingsController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.HandleSFXVolumeSettingsControllerValueDidChange);
    this._arcsVisibilityTypeSettingsDropdown.didSelectCellWithIdxEvent += new System.Action<int, ArcVisibilityType>(this.HandleArcVisibilityDropdownDidSelectCellWithIdx);
    this._saberTrailIntensitySettingsController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.HandleSaberTrailIntensitySettingsControllerValueDidChange);
    this._noteJumpDurationTypeSettingsDropdown.didSelectCellWithIdxEvent += new System.Action<int, NoteJumpDurationTypeSettings>(this.HandleNoteJumpDurationTypeSettingsDropdownDidSelectCellWithIdx);
    this._noteJumpFixedDurationSettingsController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.HandleNoteJumpFixedDurationSettingsControllerValueDidChange);
    this._playerHeightSettingsController.valueDidChangeEvent += new System.Action<float>(this.HandlePlayerHeightSettingsControllerValueDidChange);
    this._noteJumpStartBeatOffsetDropdown.didSelectCellWithIdxEvent += new System.Action<int, float>(this.HandleNoteJumpStartBeatOffsetPositionSelected);
    this._environmentEffectsFilterDefaultPresetDropdown.didSelectCellWithIdxEvent += new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
    this._environmentEffectsFilterExpertPlusPresetDropdown.didSelectCellWithIdxEvent += new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
  }

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__39_8()
  {
    if ((UnityEngine.Object) this._sfxVolumeSettingsController != (UnityEngine.Object) null)
      this._sfxVolumeSettingsController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.HandleSFXVolumeSettingsControllerValueDidChange);
    if ((UnityEngine.Object) this._arcsVisibilityTypeSettingsDropdown != (UnityEngine.Object) null)
      this._arcsVisibilityTypeSettingsDropdown.didSelectCellWithIdxEvent -= new System.Action<int, ArcVisibilityType>(this.HandleArcVisibilityDropdownDidSelectCellWithIdx);
    if ((UnityEngine.Object) this._saberTrailIntensitySettingsController != (UnityEngine.Object) null)
      this._saberTrailIntensitySettingsController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.HandleSaberTrailIntensitySettingsControllerValueDidChange);
    if ((UnityEngine.Object) this._noteJumpDurationTypeSettingsDropdown != (UnityEngine.Object) null)
      this._noteJumpDurationTypeSettingsDropdown.didSelectCellWithIdxEvent -= new System.Action<int, NoteJumpDurationTypeSettings>(this.HandleNoteJumpDurationTypeSettingsDropdownDidSelectCellWithIdx);
    if ((UnityEngine.Object) this._noteJumpFixedDurationSettingsController != (UnityEngine.Object) null)
      this._noteJumpFixedDurationSettingsController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.HandleNoteJumpFixedDurationSettingsControllerValueDidChange);
    if ((UnityEngine.Object) this._playerHeightSettingsController != (UnityEngine.Object) null)
      this._playerHeightSettingsController.valueDidChangeEvent -= new System.Action<float>(this.HandlePlayerHeightSettingsControllerValueDidChange);
    if ((UnityEngine.Object) this._environmentEffectsFilterDefaultPresetDropdown != (UnityEngine.Object) null)
      this._environmentEffectsFilterDefaultPresetDropdown.didSelectCellWithIdxEvent -= new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
    if (!((UnityEngine.Object) this._environmentEffectsFilterExpertPlusPresetDropdown != (UnityEngine.Object) null))
      return;
    this._environmentEffectsFilterExpertPlusPresetDropdown.didSelectCellWithIdxEvent -= new System.Action<int, EnvironmentEffectsFilterPreset>(this.HandleLightReductionAmountSelected);
  }

  public enum PlayerSettingsPanelLayout
  {
    All,
    Singleplayer,
    Multiplayer,
  }
}
