// Decompiled with JetBrains decompiler
// Type: StandardLevelAnalytics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StandardLevelAnalytics : MonoBehaviour
{
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;

  public virtual void Start() => this._standardLevelScenesTransitionSetupData.didFinishEvent += new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleStandardLevelDidFinishEvent);

  public virtual void OnDestroy() => this._standardLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleStandardLevelDidFinishEvent);

  public virtual void HandleStandardLevelDidFinishEvent(
    StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData,
    LevelCompletionResults levelCompletionResults)
  {
    IDifficultyBeatmap difficultyBeatmap = standardLevelScenesTransitionSetupData.difficultyBeatmap;
    PracticeSettings practiceSettings = standardLevelScenesTransitionSetupData.practiceSettings;
    Dictionary<string, string> eventData = new Dictionary<string, string>(48);
    eventData.Add("game_mode", standardLevelScenesTransitionSetupData.gameMode);
    eventData.Add("level_id", difficultyBeatmap.level.levelID.ToString());
    eventData.Add("difficulty", difficultyBeatmap.difficulty.ToString());
    eventData.Add("characteristic", difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName);
    eventData.Add("using_override_environment", standardLevelScenesTransitionSetupData.usingOverrideEnvironment.ToString());
    eventData.Add("environment", standardLevelScenesTransitionSetupData.environmentInfo.serializedName);
    eventData.Add("using_override_color_scheme", standardLevelScenesTransitionSetupData.usingOverrideColorScheme.ToString());
    eventData.Add("color_scheme_id", standardLevelScenesTransitionSetupData.colorScheme.colorSchemeId);
    LevelCompletionResultsAnalyticsHelper.FillEventData(levelCompletionResults, eventData);
    if (practiceSettings != null)
    {
      Dictionary<string, string> dictionary1 = eventData;
      float num = practiceSettings.songSpeedMul;
      string str1 = num.ToString();
      dictionary1.Add("practice_song_speed_mul", str1);
      Dictionary<string, string> dictionary2 = eventData;
      num = practiceSettings.startSongTime;
      string str2 = num.ToString();
      dictionary2.Add("practice_start_song_time", str2);
      eventData.Add("practice_start_in_advance_and_clear_notes", practiceSettings.startInAdvanceAndClearNotes.ToString());
      this._analyticsModel.LogEvent("Practice Level Ended", eventData);
    }
    else
      this._analyticsModel.LogEvent("Standard Level Ended", eventData);
  }
}
