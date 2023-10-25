// Decompiled with JetBrains decompiler
// Type: ResultsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResultsViewController : ViewController
{
  [SerializeField]
  protected Button _restartButton;
  [SerializeField]
  protected Button _continueButton;
  [Space]
  [SerializeField]
  protected GameObject _clearedPanel;
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected GameObject _newHighScoreText;
  [SerializeField]
  protected TextMeshProUGUI _rankText;
  [SerializeField]
  protected TextMeshProUGUI _goodCutsPercentageText;
  [SerializeField]
  protected TextMeshProUGUI _comboText;
  [Space]
  [SerializeField]
  protected GameObject _clearedBannerGo;
  [SerializeField]
  protected GameObject _failedBannerGo;
  [Space]
  [SerializeField]
  protected LevelBar _levelBar;
  [Space]
  [SerializeField]
  protected AudioClip _levelClearedAudioClip;
  [Inject]
  protected readonly FireworksController _fireworksController;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  [Inject]
  protected readonly ResultsEnvironmentManager _resultsEnvironmentManager;
  protected LevelCompletionResults _levelCompletionResults;
  protected IDifficultyBeatmap _difficultyBeatmap;
  protected IReadonlyBeatmapData _transformedBeatmapData;
  protected Coroutine _startFireworksAfterDelayCoroutine;
  protected bool _newHighScore;
  protected bool _practice;

  public event System.Action<ResultsViewController> continueButtonPressedEvent;

  public event System.Action<ResultsViewController> restartButtonPressedEvent;

  public bool practice => this._practice;

  public virtual void Init(
    LevelCompletionResults levelCompletionResults,
    IReadonlyBeatmapData transformedBeatmapData,
    IDifficultyBeatmap difficultyBeatmap,
    bool practice,
    bool newHighScore)
  {
    this._levelCompletionResults = levelCompletionResults;
    this._difficultyBeatmap = difficultyBeatmap;
    this._transformedBeatmapData = transformedBeatmapData;
    this._newHighScore = newHighScore;
    this._practice = practice;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._restartButton, new System.Action(this.RestartButtonPressed));
      this.buttonBinder.AddBinding(this._continueButton, new System.Action(this.ContinueButtonPressed));
    }
    if (!addedToHierarchy)
      return;
    this.SetDataToUI();
    if (this._levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
      this.EnableResultsEnvironmentController();
    if (this._levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared || !this._newHighScore)
      return;
    this._startFireworksAfterDelayCoroutine = this.StartCoroutine(this.StartFireworksAfterDelay(1.95f));
    this._songPreviewPlayer.CrossfadeTo(this._levelClearedAudioClip, -4f, 0.0f, this._levelClearedAudioClip.length, (System.Action) null);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (this._startFireworksAfterDelayCoroutine != null)
    {
      this.StopCoroutine(this._startFireworksAfterDelayCoroutine);
      this._startFireworksAfterDelayCoroutine = (Coroutine) null;
    }
    if (!((UnityEngine.Object) this._fireworksController != (UnityEngine.Object) null))
      return;
    this._fireworksController.enabled = false;
  }

  public virtual IEnumerator StartFireworksAfterDelay(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this._fireworksController.enabled = true;
  }

  public virtual void SetDataToUI()
  {
    this._clearedPanel.SetActive(this._levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared);
    IBeatmapLevel level = this._difficultyBeatmap.level;
    int difficulty = (int) this._difficultyBeatmap.difficulty;
    this._levelBar.Setup((IPreviewBeatmapLevel) level, this._difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, this._difficultyBeatmap.difficulty);
    if (this._levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed)
    {
      this._clearedBannerGo.SetActive(false);
      this._failedBannerGo.SetActive(true);
    }
    else
    {
      if (this._levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
        return;
      this._clearedBannerGo.SetActive(true);
      this._failedBannerGo.SetActive(false);
      this._scoreText.text = ScoreFormatter.Format(this._levelCompletionResults.modifiedScore);
      this._rankText.text = RankModel.GetRankName(this._levelCompletionResults.rank);
      this._goodCutsPercentageText.text = string.Format("{0}<size=50%> / {1}</size>", (object) this._levelCompletionResults.goodCutsCount, (object) (this._levelCompletionResults.goodCutsCount + this._levelCompletionResults.missedCount + this._levelCompletionResults.badCutsCount));
      this._comboText.text = this._levelCompletionResults.fullCombo ? Localization.Get("STATS_FULL_COMBO") : Localization.Get("STATS_MAX_COMBO") + " " + this._levelCompletionResults.maxCombo.ToString();
      this._newHighScoreText.SetActive(this._newHighScore);
    }
  }

  public virtual void EnableResultsEnvironmentController()
  {
    if (this._levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
      return;
    foreach (string beatmapEventKeyword in this._transformedBeatmapData.specialBasicBeatmapEventKeywords)
    {
      BaseResultsEnvironmentController controllerForKeyword = this._resultsEnvironmentManager.GetResultEnvironmentControllerForKeyword(beatmapEventKeyword);
      controllerForKeyword.gameObject.SetActive(true);
      if ((UnityEngine.Object) controllerForKeyword != (UnityEngine.Object) null)
      {
        controllerForKeyword.Setup(this._transformedBeatmapData);
        this._resultsEnvironmentManager.ShowResultForKeyword(beatmapEventKeyword);
      }
    }
  }

  public virtual void DisableResultEnvironmentController()
  {
    if (this._levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
      return;
    foreach (string beatmapEventKeyword in this._transformedBeatmapData.specialBasicBeatmapEventKeywords)
      this._resultsEnvironmentManager.HideResultForKeyword(beatmapEventKeyword);
  }

  public virtual void ContinueButtonPressed()
  {
    this.DisableResultEnvironmentController();
    System.Action<ResultsViewController> buttonPressedEvent = this.continueButtonPressedEvent;
    if (buttonPressedEvent == null)
      return;
    buttonPressedEvent(this);
  }

  public virtual void RestartButtonPressed()
  {
    this.DisableResultEnvironmentController();
    System.Action<ResultsViewController> buttonPressedEvent = this.restartButtonPressedEvent;
    if (buttonPressedEvent == null)
      return;
    buttonPressedEvent(this);
  }
}
