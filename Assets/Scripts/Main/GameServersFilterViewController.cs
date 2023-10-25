// Decompiled with JetBrains decompiler
// Type: GameServersFilterViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameServersFilterViewController : ViewController
{
  [Header("Filter By Difficulty")]
  [SerializeField]
  protected Toggle _filterByDifficultyToggle;
  [SerializeField]
  protected Button _filterByDifficultyButton;
  [SerializeField]
  protected BeatmapDifficultyDropdown _beatmapDifficultyDropdown;
  [Header("Filter By Modifiers")]
  [SerializeField]
  protected Toggle _filterByModifiersToggle;
  [SerializeField]
  protected Button _filterByModifiersButton;
  [SerializeField]
  protected GameplayModifiersDropdown _gameplayModifiersDropdown;
  [Header("Filter By Songs")]
  [SerializeField]
  protected Toggle _filterBySongsToggle;
  [SerializeField]
  protected Button _filterBySongsButton;
  [SerializeField]
  protected SongPacksDropdown _songPacksDropdown;
  [Header("Others")]
  [SerializeField]
  protected Toggle _showFullToggle;
  [SerializeField]
  protected Toggle _showPasswordProtectedToggle;
  protected readonly ToggleBinder _toggleBinder = new ToggleBinder();
  protected bool _showInternetGames;

  public GameServersFilter gameServersFilter => new GameServersFilter()
  {
    filterByDifficulty = this._filterByDifficultyToggle.isOn,
    filteredDifficulty = this._beatmapDifficultyDropdown.GetSelectedBeatmapDifficultyMask(),
    filterByModifiers = this._filterByModifiersToggle.isOn,
    filteredModifiers = this._gameplayModifiersDropdown.GetSelectedGameplayModifierMask(),
    filterBySongPacks = this._filterBySongsToggle.isOn,
    filteredSongPacks = this._songPacksDropdown.GetSelectedSongPackMask(),
    showFull = this._showFullToggle.isOn,
    showProtected = this._showPasswordProtectedToggle.isOn,
    showInternetGames = this._showInternetGames
  };

  public virtual void SetupGameServersFilter(GameServersFilter gameServersFilter) => this.Refresh(gameServersFilter);

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this._toggleBinder.AddBinding(this._filterByDifficultyToggle, (System.Action<bool>) (isOn => this._filterByDifficultyButton.interactable = isOn));
    this._toggleBinder.AddBinding(this._filterByModifiersToggle, (System.Action<bool>) (isOn => this._filterByModifiersButton.interactable = isOn));
    this._toggleBinder.AddBinding(this._filterBySongsToggle, (System.Action<bool>) (isOn => this._filterBySongsButton.interactable = isOn));
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    this._toggleBinder.ClearBindings();
  }

  public virtual void Refresh(GameServersFilter currentFilter)
  {
    this._filterByDifficultyToggle.isOn = currentFilter.filterByDifficulty;
    this._filterByModifiersToggle.isOn = currentFilter.filterByModifiers;
    this._filterBySongsToggle.isOn = currentFilter.filterBySongPacks;
    this._showFullToggle.isOn = currentFilter.showFull;
    this._showPasswordProtectedToggle.isOn = currentFilter.showProtected;
    this._filterByDifficultyButton.interactable = currentFilter.filterByDifficulty;
    this._filterByModifiersButton.interactable = currentFilter.filterByModifiers;
    this._filterBySongsButton.interactable = currentFilter.filterBySongPacks;
    this._beatmapDifficultyDropdown.SelectCellWithBeatmapDifficultyMask(currentFilter.filteredDifficulty);
    this._gameplayModifiersDropdown.SelectCellWithGameplayModifierMask(currentFilter.filteredModifiers);
    this._songPacksDropdown.SelectCellWithSongPackMask(currentFilter.filteredSongPacks);
    this._showInternetGames = currentFilter.showInternetGames;
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__16_0(bool isOn) => this._filterByDifficultyButton.interactable = isOn;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__16_1(bool isOn) => this._filterByModifiersButton.interactable = isOn;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__16_2(bool isOn) => this._filterBySongsButton.interactable = isOn;
}
