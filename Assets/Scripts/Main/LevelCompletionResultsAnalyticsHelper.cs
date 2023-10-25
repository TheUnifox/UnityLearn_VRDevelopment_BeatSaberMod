// Decompiled with JetBrains decompiler
// Type: LevelCompletionResultsAnalyticsHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public abstract class LevelCompletionResultsAnalyticsHelper
{
  public static void FillEventData(
    LevelCompletionResults levelCompletionResults,
    Dictionary<string, string> eventData)
  {
    eventData.Add("end_action", levelCompletionResults.levelEndAction.ToString());
    eventData.Add("end_state", levelCompletionResults.levelEndStateType.ToString());
    eventData.Add("raw_score", levelCompletionResults.multipliedScore.ToString());
    eventData.Add("modified_score", levelCompletionResults.modifiedScore.ToString());
    eventData.Add("rank", levelCompletionResults.rank.ToString());
    eventData.Add("energy", levelCompletionResults.energy.ToString());
    eventData.Add("good_cut_count", levelCompletionResults.goodCutsCount.ToString());
    eventData.Add("bad_cut_count", levelCompletionResults.badCutsCount.ToString());
    eventData.Add("missed_count", levelCompletionResults.missedCount.ToString());
    eventData.Add("ok_count", levelCompletionResults.okCount.ToString());
    eventData.Add("not_good_count", levelCompletionResults.notGoodCount.ToString());
    eventData.Add("full_combo", levelCompletionResults.fullCombo.ToString());
    eventData.Add("modifier_no_fail_on_zero_energy", levelCompletionResults.gameplayModifiers.noFailOn0Energy.ToString());
    eventData.Add("modifier_disappearing_arrows", levelCompletionResults.gameplayModifiers.disappearingArrows.ToString());
    eventData.Add("modifier_fast_notes", levelCompletionResults.gameplayModifiers.fastNotes.ToString());
    eventData.Add("modifier_ghost_notes", levelCompletionResults.gameplayModifiers.ghostNotes.ToString());
    eventData.Add("modifier_insta_fail", levelCompletionResults.gameplayModifiers.instaFail.ToString());
    eventData.Add("modifier_no_arrows", levelCompletionResults.gameplayModifiers.noArrows.ToString());
    eventData.Add("modifier_no_bombs", levelCompletionResults.gameplayModifiers.noBombs.ToString());
    eventData.Add("modifier_fail_on_saber_clash", levelCompletionResults.gameplayModifiers.failOnSaberClash.ToString());
    eventData.Add("modifier_energy_type", levelCompletionResults.gameplayModifiers.energyType.ToString());
    eventData.Add("modifier_obstacle_type", levelCompletionResults.gameplayModifiers.enabledObstacleType.ToString());
    eventData.Add("modifier_song_speed", levelCompletionResults.gameplayModifiers.songSpeed.ToString());
    eventData.Add("modifier_pro_mode", levelCompletionResults.gameplayModifiers.proMode.ToString());
    eventData.Add("modifier_zen_mode", levelCompletionResults.gameplayModifiers.zenMode.ToString());
    eventData.Add("modifier_small_cubes", levelCompletionResults.gameplayModifiers.smallCubes.ToString());
    eventData.Add("modifier_strict_angles", levelCompletionResults.gameplayModifiers.strictAngles.ToString());
  }
}
