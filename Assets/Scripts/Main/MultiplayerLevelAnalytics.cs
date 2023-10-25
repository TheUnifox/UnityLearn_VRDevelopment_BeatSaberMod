// Decompiled with JetBrains decompiler
// Type: MultiplayerLevelAnalytics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerLevelAnalytics : MonoBehaviour
{
  [SerializeField]
  protected MultiplayerLevelScenesTransitionSetupDataSO _multiplayerLevelScenesTransitionSetupData;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;

  public virtual void Start() => this._multiplayerLevelScenesTransitionSetupData.didFinishEvent += new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);

  public virtual void OnDestroy() => this._multiplayerLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);

  public virtual void HandleMultiplayerLevelDidFinish(
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData,
    MultiplayerResultsData multiplayerResultsData)
  {
    if (!multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.hasAnyResults)
      return;
    IDifficultyBeatmap difficultyBeatmap = multiplayerLevelScenesTransitionSetupData.difficultyBeatmap;
    Dictionary<string, string> eventData = new Dictionary<string, string>(48);
    eventData.Add("game_id", multiplayerResultsData.gameId);
    eventData.Add("game_mode", multiplayerLevelScenesTransitionSetupData.gameMode);
    eventData.Add("level_id", difficultyBeatmap.level.levelID.ToString());
    eventData.Add("difficulty", difficultyBeatmap.difficulty.ToString());
    eventData.Add("characteristic", difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName);
    eventData.Add("player_count", (multiplayerResultsData.otherPlayersData.Count + 1).ToString());
    eventData.Add("using_override_color_scheme", multiplayerLevelScenesTransitionSetupData.usingOverrideColorScheme.ToString());
    eventData.Add("color_scheme_id", multiplayerLevelScenesTransitionSetupData.colorScheme.colorSchemeId);
    if (multiplayerResultsData.localPlayerResultData.badge != null)
      eventData.Add("badge", multiplayerResultsData.localPlayerResultData.badge.titleLocalizationKey);
    LevelCompletionResultsAnalyticsHelper.FillEventData(multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults, eventData);
    if (multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Incomplete)
      eventData["end_action"] = multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.playerLevelEndReason.ToString();
    this._analyticsModel.LogEvent("Multiplayer Level Ended", eventData);
  }
}
