// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgeDataFullComboSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerBadgeDataFullComboSO : MultiplayerBadgeDataSO
{
  [SerializeField]
  protected float _weight = float.MaxValue;

  public override MultiplayerBadgeAwardData CalculateBadgeData(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    PlayerDataModel playerDataModel,
    IDifficultyBeatmap difficultyBeatmap,
    float randomMultiplier)
  {
    IConnectedPlayer awardedPlayer = (IConnectedPlayer) null;
    int num = 0;
    foreach (MultiplayerPlayerResultsData playerResultsData in (IEnumerable<MultiplayerPlayerResultsData>) resultsData)
    {
      if (playerResultsData.multiplayerLevelCompletionResults.levelCompletionResults.fullCombo && playerResultsData.multiplayerLevelCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
      {
        if (awardedPlayer != null)
          return (MultiplayerBadgeAwardData) null;
        awardedPlayer = playerResultsData.connectedPlayer;
        num = playerResultsData.multiplayerLevelCompletionResults.levelCompletionResults.maxCombo;
      }
    }
    if (awardedPlayer == null)
      return (MultiplayerBadgeAwardData) null;
    string title = Localization.Get(this.titleLocalizationKey);
    string format = Localization.GetFormat(this.subtitleLocalizationKey, (object) num);
    return new MultiplayerBadgeAwardData(awardedPlayer, this._weight, title, format, (MultiplayerBadgeDataSO) this);
  }
}
