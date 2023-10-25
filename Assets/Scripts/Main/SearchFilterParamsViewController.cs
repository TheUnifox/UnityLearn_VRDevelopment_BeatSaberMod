// Decompiled with JetBrains decompiler
// Type: SearchFilterParamsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SearchFilterParamsViewController : ViewController
{
  [SerializeField]
  protected Toggle _filterByOwnedToggle;
  [SerializeField]
  protected Toggle _filterByNotOwnedToggle;
  [SerializeField]
  protected Toggle _filterByCharacteristicToggle;
  [SerializeField]
  protected BeatmapCharacteristicsDropdown _beatmapCharacteristicsDropdown;
  [SerializeField]
  protected Toggle _filterByDifficultyToggle;
  [SerializeField]
  protected BeatmapDifficultyDropdown _beatmapDifficultyDropdown;
  [SerializeField]
  protected Toggle _filterBySongPacksToggle;
  [SerializeField]
  protected SongPacksDropdown _songPacksDropdown;
  [SerializeField]
  protected Toggle _filterByNotPlayedYetToggle;
  [SerializeField]
  protected Toggle _filterByMinBpmToggle;
  [SerializeField]
  protected FormattedFloatListSettingsController _minBpmController;
  [SerializeField]
  protected Toggle _filterByMaxBpmToggle;
  [SerializeField]
  protected FormattedFloatListSettingsController _maxBpmController;
  [SerializeField]
  protected Button _okButton;
  protected LevelFilterParams _currentLevelFilterParams;

  public event System.Action<SearchFilterParamsViewController, LevelFilterParams> didFinishEvent;

  public virtual void Setup(LevelFilterParams levelFilterParams)
  {
    this._currentLevelFilterParams = levelFilterParams;
    this.Refresh(this._currentLevelFilterParams);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    if (addedToHierarchy)
    {
      this._minBpmController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.MinBpmControllerValueDidChange);
      this._maxBpmController.valueDidChangeEvent += new System.Action<FormattedFloatListSettingsController, float>(this.MaxBpmControllerValueDidChange);
      this._filterByOwnedToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleFilterByOwnedValueValueChanged));
      this._filterByNotOwnedToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleFilterByNotOwnedValueValueChanged));
    }
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._okButton, new System.Action(this.OkButtonPressed));
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._minBpmController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.MinBpmControllerValueDidChange);
    this._maxBpmController.valueDidChangeEvent -= new System.Action<FormattedFloatListSettingsController, float>(this.MaxBpmControllerValueDidChange);
    this._filterByOwnedToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleFilterByOwnedValueValueChanged));
    this._filterByNotOwnedToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleFilterByNotOwnedValueValueChanged));
  }

  public virtual void OkButtonPressed()
  {
    LevelFilterParams levelFilterParams = new LevelFilterParams(this._currentLevelFilterParams.filterByLevelIds, this._currentLevelFilterParams.beatmapLevelIds, this._currentLevelFilterParams.searchText, this._filterByOwnedToggle.isOn, this._filterByNotOwnedToggle.isOn, this._filterByDifficultyToggle.isOn, this._beatmapDifficultyDropdown.GetSelectedBeatmapDifficultyMask(), this._filterByCharacteristicToggle.isOn, this._beatmapCharacteristicsDropdown.GetSelectedBeatmapCharacteristic(), this._filterBySongPacksToggle.isOn, this._songPacksDropdown.GetSelectedSongPackMask(), this._filterByNotPlayedYetToggle.isOn, this._filterByMinBpmToggle.isOn, this._minBpmController.value, this._filterByMaxBpmToggle.isOn, this._maxBpmController.value);
    System.Action<SearchFilterParamsViewController, LevelFilterParams> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, levelFilterParams);
  }

  public virtual void MinBpmControllerValueDidChange(
    FormattedFloatListSettingsController minBpmController,
    float value)
  {
    this._maxBpmController.SetValue(Math.Max(this._maxBpmController.value, value));
  }

  public virtual void MaxBpmControllerValueDidChange(
    FormattedFloatListSettingsController maxBpmController,
    float value)
  {
    this._minBpmController.SetValue(Math.Min(this._minBpmController.value, value));
  }

  public virtual void HandleFilterByOwnedValueValueChanged(bool isOn)
  {
    if (!isOn)
      return;
    this._filterByNotOwnedToggle.isOn = false;
  }

  public virtual void HandleFilterByNotOwnedValueValueChanged(bool isOn)
  {
    if (!isOn)
      return;
    this._filterByOwnedToggle.isOn = false;
  }

  public virtual void Refresh(LevelFilterParams levelFilterParams)
  {
    this._filterByOwnedToggle.isOn = levelFilterParams.filterByOwned;
    this._filterByNotOwnedToggle.isOn = levelFilterParams.filterByNotOwned;
    this._filterByCharacteristicToggle.isOn = levelFilterParams.filterByCharacteristic;
    this._beatmapCharacteristicsDropdown.SelectCellWithBeatmapCharacteristic(levelFilterParams.filteredCharacteristic);
    this._filterByDifficultyToggle.isOn = levelFilterParams.filterByDifficulty;
    this._beatmapDifficultyDropdown.SelectCellWithBeatmapDifficultyMask(levelFilterParams.filteredDifficulty);
    this._filterBySongPacksToggle.isOn = levelFilterParams.filterBySongPacks;
    this._songPacksDropdown.SelectCellWithSongPackMask(levelFilterParams.filteredSongPacks);
    this._filterByNotPlayedYetToggle.isOn = levelFilterParams.filterByNotPlayedYet;
    this._filterByMinBpmToggle.isOn = levelFilterParams.filterByMinBpm;
    this._minBpmController.values = LevelFilterParams.bpmValues;
    this._minBpmController.SetValue(levelFilterParams.filteredMinBpm);
    this._filterByMaxBpmToggle.isOn = levelFilterParams.filterByMaxBpm;
    this._maxBpmController.values = LevelFilterParams.bpmValues;
    this._maxBpmController.SetValue(levelFilterParams.filteredMaxBpm);
  }
}
