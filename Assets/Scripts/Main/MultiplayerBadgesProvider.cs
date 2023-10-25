// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgesProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class MultiplayerBadgesProvider
{
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly IDifficultyBeatmap _difficultyBeatmap;
  [Inject]
  protected readonly MultiplayerBadgesModelSO _multiplayerBadgesModel;
  protected const int kTargetPositiveBadgesCount = 2;
  protected const int kTargetNegativeBadgesCount = 1;
  protected const float kMinRandomMultiplierAmount = 0.8f;
  protected const float kMaxRandomMultiplierAmount = 1.2f;

  public virtual void SelectBadgesAndPutThemIntoResults(
    IReadOnlyList<MultiplayerPlayerResultsData> playerResults)
  {
    if (playerResults.Count<MultiplayerPlayerResultsData>((Func<MultiplayerPlayerResultsData, bool>) (result => result.multiplayerLevelCompletionResults.hasAnyResults)) == 0)
      return;
    Random random = new Random(playerResults.Select<MultiplayerPlayerResultsData, int>((Func<MultiplayerPlayerResultsData, int>) (p => p.multiplayerLevelCompletionResults.levelCompletionResults.modifiedScore)).Max());
    List<MultiplayerBadgeAwardData> multiplayerBadgeAwardDataList1 = new List<MultiplayerBadgeAwardData>();
    List<MultiplayerBadgeAwardData> multiplayerBadgeAwardDataList2 = new List<MultiplayerBadgeAwardData>();
    foreach (MultiplayerBadgeDataSO positiveBadge in (IEnumerable<MultiplayerBadgeDataSO>) this._multiplayerBadgesModel.positiveBadges)
    {
      float num = (float) random.Next(800, 1200) / 1000f;
      IReadOnlyList<MultiplayerPlayerResultsData> resultsData = playerResults;
      PlayerDataModel playerDataModel = this._playerDataModel;
      IDifficultyBeatmap difficultyBeatmap = this._difficultyBeatmap;
      double randomMultiplier = (double) num;
      MultiplayerBadgeAwardData badgeData = positiveBadge.CalculateBadgeData(resultsData, playerDataModel, difficultyBeatmap, (float) randomMultiplier);
      if (badgeData != null)
        multiplayerBadgeAwardDataList2.Add(badgeData);
    }
    multiplayerBadgeAwardDataList2.Sort();
    List<MultiplayerBadgeAwardData> multiplayerBadgeAwardDataList3 = new List<MultiplayerBadgeAwardData>();
    foreach (MultiplayerBadgeDataSO negativeBadge in (IEnumerable<MultiplayerBadgeDataSO>) this._multiplayerBadgesModel.negativeBadges)
    {
      float num = (float) random.Next(800, 1200) / 1000f;
      IReadOnlyList<MultiplayerPlayerResultsData> resultsData = playerResults;
      PlayerDataModel playerDataModel = this._playerDataModel;
      IDifficultyBeatmap difficultyBeatmap = this._difficultyBeatmap;
      double randomMultiplier = (double) num;
      MultiplayerBadgeAwardData badgeData = negativeBadge.CalculateBadgeData(resultsData, playerDataModel, difficultyBeatmap, (float) randomMultiplier);
      if (badgeData != null)
        multiplayerBadgeAwardDataList3.Add(badgeData);
    }
    multiplayerBadgeAwardDataList3.Sort();
    int num1 = 0;
    int num2 = 0;
    int num3 = 2;
    foreach (MultiplayerBadgeAwardData multiplayerBadgeAwardData in multiplayerBadgeAwardDataList2)
    {
      if (num1 < num3)
      {
        foreach (MultiplayerPlayerResultsData playerResult in (IEnumerable<MultiplayerPlayerResultsData>) playerResults)
        {
          if (playerResult.connectedPlayer == multiplayerBadgeAwardData.awardedPlayer && playerResult.badge == null)
          {
            multiplayerBadgeAwardDataList1.Add(multiplayerBadgeAwardData);
            playerResult.badge = multiplayerBadgeAwardData;
            ++num1;
            break;
          }
        }
      }
      else
        break;
    }
    int num4 = 1 + (2 - num1);
    foreach (MultiplayerBadgeAwardData multiplayerBadgeAwardData in multiplayerBadgeAwardDataList3)
    {
      if (num2 < num4)
      {
        for (int index = 0; index < playerResults.Count - 1; ++index)
        {
          MultiplayerPlayerResultsData playerResult = playerResults[index];
          if (playerResult.connectedPlayer == multiplayerBadgeAwardData.awardedPlayer && playerResult.badge == null)
          {
            multiplayerBadgeAwardDataList1.Add(multiplayerBadgeAwardData);
            playerResult.badge = multiplayerBadgeAwardData;
            ++num2;
            break;
          }
        }
      }
      else
        break;
    }
    int num5 = 2 + (1 - num2);
    foreach (MultiplayerBadgeAwardData multiplayerBadgeAwardData in multiplayerBadgeAwardDataList2)
    {
      if (num1 >= num5)
        break;
      foreach (MultiplayerPlayerResultsData playerResult in (IEnumerable<MultiplayerPlayerResultsData>) playerResults)
      {
        if (playerResult.connectedPlayer == multiplayerBadgeAwardData.awardedPlayer && playerResult.badge == null)
        {
          multiplayerBadgeAwardDataList1.Add(multiplayerBadgeAwardData);
          playerResult.badge = multiplayerBadgeAwardData;
          ++num1;
          break;
        }
      }
    }
  }
}
