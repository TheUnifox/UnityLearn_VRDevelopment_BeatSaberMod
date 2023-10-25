// Decompiled with JetBrains decompiler
// Type: PracticeViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PracticeViewController : ViewController
{
  [SerializeField]
  protected TimeSlider _songStartSlider;
  [SerializeField]
  protected PercentSlider _speedSlider;
  [SerializeField]
  protected LevelBar _levelBar;
  [Space]
  [SerializeField]
  protected Button _playButton;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  [Inject]
  protected readonly PerceivedLoudnessPerLevelModel _perceivedLoudnessPerLevelModel;
  protected const float kWaitBeforePlayPreviewAfterPreviewStartValueChanged = 1f;
  protected const float kMinValueChangeToInstantPlayPreview = 3f;
  protected PracticeSettings _practiceSettings;
  protected float _currentPlayingStartTime;
  protected float _maxStartSongTime;
  protected IBeatmapLevel _level;
  protected BeatmapCharacteristicSO _beatmapCharacteristic;
  protected BeatmapDifficulty _beatmapDifficulty;

  public event System.Action didPressPlayButtonEvent;

  public PracticeSettings practiceSettings => this._practiceSettings;

  public virtual void Init(
    IBeatmapLevel level,
    BeatmapCharacteristicSO beatmapCharacteristic,
    BeatmapDifficulty beatmapDifficulty)
  {
    this._level = level;
    this._beatmapCharacteristic = beatmapCharacteristic;
    this._beatmapDifficulty = beatmapDifficulty;
    this._practiceSettings = this._playerDataModel.playerData.practiceSettings;
    this._maxStartSongTime = Mathf.Max(this._level.beatmapLevelData.audioClip.length - 1f, 0.0f);
    this._songStartSlider.minValue = 0.0f;
    this._songStartSlider.maxValue = this._maxStartSongTime;
    this._practiceSettings.songSpeedMul = Mathf.Clamp(this._practiceSettings.songSpeedMul, this._speedSlider.minValue, this._speedSlider.maxValue);
    this._practiceSettings.startSongTime = 0.0f;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this.buttonBinder.AddBinding(this._playButton, new System.Action(this.PlayButtonPressed));
    if (!addedToHierarchy)
      return;
    this._speedSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandleSpeedSliderValueDidChange);
    this._songStartSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandleSongStartSliderValueDidChange);
    this._levelBar.Setup((IPreviewBeatmapLevel) this._level, this._beatmapCharacteristic, this._beatmapDifficulty);
    this.RefreshUI();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._speedSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandleSpeedSliderValueDidChange);
    this._songStartSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandleSongStartSliderValueDidChange);
  }

  public virtual void PlayPreview()
  {
    this._songPreviewPlayer.CrossfadeTo(this._level.beatmapLevelData.audioClip, this._perceivedLoudnessPerLevelModel.GetLoudnessCorrectionByLevelId(this._level.levelID), this._practiceSettings.startSongTime, 5f, (System.Action) null);
    this._currentPlayingStartTime = this._practiceSettings.startSongTime;
  }

  public virtual void RefreshUI()
  {
    this._songStartSlider.value = this._practiceSettings.startSongTime;
    this._speedSlider.value = this._practiceSettings.songSpeedMul;
  }

  public virtual void HandleSpeedSliderValueDidChange(RangeValuesTextSlider slider, float value) => this._practiceSettings.songSpeedMul = value;

  public virtual void HandleSongStartSliderValueDidChange(RangeValuesTextSlider slider, float value)
  {
    this._practiceSettings.startSongTime = value;
    if ((double) Mathf.Abs(this._currentPlayingStartTime - value) <= 3.0)
      return;
    this.PlayPreview();
  }

  public virtual void PlayButtonPressed()
  {
    this._practiceSettings.startSongTime = Mathf.Min(this._practiceSettings.startSongTime, this._maxStartSongTime);
    System.Action pressPlayButtonEvent = this.didPressPlayButtonEvent;
    if (pressPlayButtonEvent == null)
      return;
    pressPlayButtonEvent();
  }
}
