// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgeDataPersonalBestSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerBadgeDataPersonalBestSO : MultiplayerBadgeDataSO
{
  [SerializeField]
  protected float _weight = float.MaxValue;

  public override MultiplayerBadgeAwardData CalculateBadgeData(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    PlayerDataModel playerDataModel,
    IDifficultyBeatmap difficultyBeatmap,
    float randomMultiplier)
  {
    foreach (MultiplayerPlayerResultsData playerResultsData in (IEnumerable<MultiplayerPlayerResultsData>) resultsData)
    {
      if (playerResultsData.connectedPlayer.isMe && difficultyBeatmap != null)
      {
        if (playerDataModel.playerData.GetPlayerLevelStatsData(difficultyBeatmap).highScore < playerResultsData.multiplayerLevelCompletionResults.levelCompletionResults.modifiedScore)
        {
          string title = Localization.Get(this.titleLocalizationKey);
          string format = Localization.GetFormat(this.subtitleLocalizationKey, (object) playerResultsData.multiplayerLevelCompletionResults.levelCompletionResults.modifiedScore);
          return new MultiplayerBadgeAwardData(playerResultsData.connectedPlayer, this._weight, title, format, (MultiplayerBadgeDataSO) this);
        }
        break;
      }
    }
    return (MultiplayerBadgeAwardData) null;
  }
}
