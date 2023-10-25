// Decompiled with JetBrains decompiler
// Type: MultiplayerResultsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerResultsViewController : ViewController
{
  [SerializeField]
  protected GameObject _levelClearedGO;
  [SerializeField]
  protected GameObject _levelFailedGO;
  [SerializeField]
  protected GameObject _levelResultsGO;
  [SerializeField]
  protected LevelBar _levelBar;
  [SerializeField]
  protected ResultsTableView _resultsTableView;
  [SerializeField]
  protected Button _backToLobbyButton;
  [SerializeField]
  protected Button _backToMenuButton;

  public event System.Action<MultiplayerResultsViewController> backToLobbyPressedEvent;

  public event System.Action<MultiplayerResultsViewController> backToMenuPressedEvent;

  public virtual void Init(
    MultiplayerResultsData multiplayerResultsData,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    bool showBackToLobbyButton,
    bool showBackToMenuButton)
  {
    if (multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.hasAnyResults)
    {
      LevelCompletionResults completionResults = multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults;
      this._levelClearedGO.SetActive(completionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared);
      this._levelFailedGO.SetActive(completionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed);
      this._levelResultsGO.SetActive(completionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared && completionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Failed);
    }
    else
    {
      this._levelClearedGO.SetActive(false);
      this._levelFailedGO.SetActive(false);
      this._levelResultsGO.SetActive(false);
    }
    this._backToLobbyButton.gameObject.SetActive(showBackToLobbyButton);
    this._backToMenuButton.gameObject.SetActive(showBackToMenuButton);
    this._levelBar.Setup(previewBeatmapLevel, beatmapCharacteristic, beatmapDifficulty);
    this._resultsTableView.SetData(multiplayerResultsData.allPlayersSortedData);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._backToLobbyButton, new System.Action(this.BackToLobbyPressed));
    this.buttonBinder.AddBinding(this._backToMenuButton, new System.Action(this.BackToMenuPressed));
  }

  public virtual void BackToLobbyPressed()
  {
    System.Action<MultiplayerResultsViewController> lobbyPressedEvent = this.backToLobbyPressedEvent;
    if (lobbyPressedEvent == null)
      return;
    lobbyPressedEvent(this);
  }

  public virtual void BackToMenuPressed()
  {
    System.Action<MultiplayerResultsViewController> menuPressedEvent = this.backToMenuPressedEvent;
    if (menuPressedEvent == null)
      return;
    menuPressedEvent(this);
  }
}
