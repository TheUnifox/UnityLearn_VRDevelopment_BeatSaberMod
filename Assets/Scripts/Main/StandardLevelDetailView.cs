// Decompiled with JetBrains decompiler
// Type: StandardLevelDetailView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StandardLevelDetailView : MonoBehaviour
{
  [SerializeField]
  protected Button _actionButton;
  [SerializeField]
  protected TextMeshProUGUI _actionButtonText;
  [SerializeField]
  protected Button _practiceButton;
  [SerializeField]
  protected LevelBar _levelBar;
  [SerializeField]
  protected LevelParamsPanel _levelParamsPanel;
  [SerializeField]
  protected BeatmapDifficultySegmentedControlController _beatmapDifficultySegmentedControlController;
  [SerializeField]
  protected BeatmapCharacteristicSegmentedControlController _beatmapCharacteristicSegmentedControlController;
  [SerializeField]
  protected Toggle _favoriteToggle;
  protected IBeatmapLevel _level;
  protected PlayerData _playerData;
  protected IDifficultyBeatmap _selectedDifficultyBeatmap;
  protected ToggleBinder _toggleBinder;

  public event System.Action<StandardLevelDetailView, IDifficultyBeatmap> didChangeDifficultyBeatmapEvent;

  public event System.Action<StandardLevelDetailView, Toggle> didFavoriteToggleChangeEvent;

  public IDifficultyBeatmap selectedDifficultyBeatmap => this._selectedDifficultyBeatmap;

  public Button actionButton => this._actionButton;

  public string actionButtonText
  {
    set => this._actionButtonText.text = value;
  }

  public Button practiceButton => this._practiceButton;

  public bool hidePracticeButton
  {
    set => this._practiceButton.gameObject.SetActive(!value);
  }

  public virtual void SetContent(
    IBeatmapLevel level,
    BeatmapDifficulty defaultDifficulty,
    BeatmapCharacteristicSO defaultBeatmapCharacteristic,
    PlayerData playerData)
  {
    this._level = level;
    this._playerData = playerData;
    if (this._level != null && this._level.beatmapLevelData != null)
    {
      this._beatmapCharacteristicSegmentedControlController.SetData(level.beatmapLevelData.difficultyBeatmapSets, (bool) (UnityEngine.Object) defaultBeatmapCharacteristic ? defaultBeatmapCharacteristic : this._beatmapCharacteristicSegmentedControlController.selectedBeatmapCharacteristic);
      this._beatmapDifficultySegmentedControlController.SetData(level.beatmapLevelData.GetDifficultyBeatmapSet(this._beatmapCharacteristicSegmentedControlController.selectedBeatmapCharacteristic).difficultyBeatmaps, defaultDifficulty);
    }
    this.RefreshContent();
  }

  public virtual void Awake()
  {
    this._beatmapDifficultySegmentedControlController.didSelectDifficultyEvent += new System.Action<BeatmapDifficultySegmentedControlController, BeatmapDifficulty>(this.HandleBeatmapDifficultySegmentedControlControllerDidSelectDifficulty);
    this._beatmapCharacteristicSegmentedControlController.didSelectBeatmapCharacteristicEvent += new System.Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO>(this.HandleBeatmapCharacteristicSegmentedControlControllerDidSelectBeatmapCharacteristic);
    this._toggleBinder = new ToggleBinder();
    this._toggleBinder.AddBinding(this._favoriteToggle, (System.Action<bool>) (on =>
    {
      System.Action<StandardLevelDetailView, Toggle> toggleChangeEvent = this.didFavoriteToggleChangeEvent;
      if (toggleChangeEvent == null)
        return;
      toggleChangeEvent(this, this._favoriteToggle);
    }));
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._beatmapDifficultySegmentedControlController != (UnityEngine.Object) null)
      this._beatmapDifficultySegmentedControlController.didSelectDifficultyEvent -= new System.Action<BeatmapDifficultySegmentedControlController, BeatmapDifficulty>(this.HandleBeatmapDifficultySegmentedControlControllerDidSelectDifficulty);
    if ((UnityEngine.Object) this._beatmapCharacteristicSegmentedControlController != (UnityEngine.Object) null)
      this._beatmapCharacteristicSegmentedControlController.didSelectBeatmapCharacteristicEvent -= new System.Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO>(this.HandleBeatmapCharacteristicSegmentedControlControllerDidSelectBeatmapCharacteristic);
    this._toggleBinder?.ClearBindings();
  }

  public virtual void HandleBeatmapDifficultySegmentedControlControllerDidSelectDifficulty(
    BeatmapDifficultySegmentedControlController controller,
    BeatmapDifficulty difficulty)
  {
    this.RefreshContent();
    System.Action<StandardLevelDetailView, IDifficultyBeatmap> difficultyBeatmapEvent = this.didChangeDifficultyBeatmapEvent;
    if (difficultyBeatmapEvent == null)
      return;
    difficultyBeatmapEvent(this, this._selectedDifficultyBeatmap);
  }

  public virtual void HandleBeatmapCharacteristicSegmentedControlControllerDidSelectBeatmapCharacteristic(
    BeatmapCharacteristicSegmentedControlController controller,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    this._beatmapDifficultySegmentedControlController.SetData(this._level.beatmapLevelData.GetDifficultyBeatmapSet(this._beatmapCharacteristicSegmentedControlController.selectedBeatmapCharacteristic).difficultyBeatmaps, this._beatmapDifficultySegmentedControlController.selectedDifficulty);
    this.RefreshContent();
    System.Action<StandardLevelDetailView, IDifficultyBeatmap> difficultyBeatmapEvent = this.didChangeDifficultyBeatmapEvent;
    if (difficultyBeatmapEvent == null)
      return;
    difficultyBeatmapEvent(this, this._selectedDifficultyBeatmap);
  }

  public virtual void RefreshContent()
  {
    if (this._level == null || this._level.beatmapLevelData == null)
      return;
    this._favoriteToggle.SetIsOnWithoutNotify(this._playerData.IsLevelUserFavorite((IPreviewBeatmapLevel) this._level));
    this._selectedDifficultyBeatmap = this._level.beatmapLevelData.GetDifficultyBeatmap(this._beatmapCharacteristicSegmentedControlController.selectedBeatmapCharacteristic, this._beatmapDifficultySegmentedControlController.selectedDifficulty);
    this._levelBar.Setup((IPreviewBeatmapLevel) this._level);
    this.SetContentForBeatmapDataAsync(this._selectedDifficultyBeatmap);
  }

  public virtual async void SetContentForBeatmapDataAsync(
    IDifficultyBeatmap selectedDifficultyBeatmap)
  {
    IBeatmapDataBasicInfo dataBasicInfoAsync = await this._selectedDifficultyBeatmap.GetBeatmapDataBasicInfoAsync();
    if (selectedDifficultyBeatmap != this._selectedDifficultyBeatmap)
      return;
    this._levelParamsPanel.notesPerSecond = (float) dataBasicInfoAsync.cuttableNotesCount / this._level.beatmapLevelData.audioClip.length;
    this._levelParamsPanel.notesCount = dataBasicInfoAsync.cuttableNotesCount;
    this._levelParamsPanel.obstaclesCount = dataBasicInfoAsync.obstaclesCount;
    this._levelParamsPanel.bombsCount = dataBasicInfoAsync.bombsCount;
  }

  public virtual void ClearContent() => this._selectedDifficultyBeatmap = (IDifficultyBeatmap) null;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__29_0(bool on)
  {
    System.Action<StandardLevelDetailView, Toggle> toggleChangeEvent = this.didFavoriteToggleChangeEvent;
    if (toggleChangeEvent == null)
      return;
    toggleChangeEvent(this, this._favoriteToggle);
  }
}
