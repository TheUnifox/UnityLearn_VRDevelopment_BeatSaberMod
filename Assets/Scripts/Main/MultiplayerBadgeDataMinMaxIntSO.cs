// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgeDataMinMaxIntSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiplayerBadgeDataMinMaxIntSO : MultiplayerBadgeDataSO
{
  [Space]
  [SerializeField]
  private MultiplayerBadgeMinMax _minMax;
  [SerializeField]
  private float _weightMultiplier = 1f;

  public override MultiplayerBadgeAwardData CalculateBadgeData(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    PlayerDataModel playerDataModel,
    IDifficultyBeatmap difficultyBeatmap,
    float randomMultiplier)
  {
    return this._minMax == MultiplayerBadgeMinMax.Max ? this.CalculateMax(resultsData, randomMultiplier) : this.CalculateMin(resultsData, randomMultiplier);
  }

  protected abstract int GetValue(MultiplayerPlayerResultsData result);

  private MultiplayerBadgeAwardData CalculateMax(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    float randomMultiplier)
  {
    IConnectedPlayer awardedPlayer = (IConnectedPlayer) null;
    int a = 0;
    int num1 = 0;
    foreach (MultiplayerPlayerResultsData result in (IEnumerable<MultiplayerPlayerResultsData>) resultsData)
    {
      int b = this.GetValue(result);
      num1 += b;
      if (b > a)
      {
        a = b;
        awardedPlayer = result.connectedPlayer;
      }
      else if (Mathf.Approximately((float) a, (float) b))
      {
        a = b;
        awardedPlayer = (IConnectedPlayer) null;
      }
    }
    if (awardedPlayer == null)
      return (MultiplayerBadgeAwardData) null;
    int num2 = num1 / resultsData.Count;
    if (num2 <= 0)
      num2 = 1;
    float weight = (float) a / (float) num2 * this._weightMultiplier * randomMultiplier;
    string title = Localization.Get(this.titleLocalizationKey);
    string format = Localization.GetFormat(this.subtitleLocalizationKey, (object) a);
    return new MultiplayerBadgeAwardData(awardedPlayer, weight, title, format, (MultiplayerBadgeDataSO) this);
  }

  private MultiplayerBadgeAwardData CalculateMin(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    float randomMultiplier)
  {
    IConnectedPlayer awardedPlayer = (IConnectedPlayer) null;
    int a = int.MaxValue;
    int num1 = 0;
    foreach (MultiplayerPlayerResultsData result in (IEnumerable<MultiplayerPlayerResultsData>) resultsData)
    {
      int b = this.GetValue(result);
      num1 += b;
      if (b < a)
      {
        a = b;
        awardedPlayer = result.connectedPlayer;
      }
      else if (Mathf.Approximately((float) a, (float) b))
      {
        a = b;
        awardedPlayer = (IConnectedPlayer) null;
      }
    }
    if (awardedPlayer == null)
      return (MultiplayerBadgeAwardData) null;
    int num2 = num1 / resultsData.Count;
    if (a <= 0)
      a = 1;
    float weight = (float) num2 / (float) a * this._weightMultiplier * randomMultiplier;
    string title = Localization.Get(this.titleLocalizationKey);
    string format = Localization.GetFormat(this.subtitleLocalizationKey, (object) a);
    return new MultiplayerBadgeAwardData(awardedPlayer, weight, title, format, (MultiplayerBadgeDataSO) this);
  }
}
