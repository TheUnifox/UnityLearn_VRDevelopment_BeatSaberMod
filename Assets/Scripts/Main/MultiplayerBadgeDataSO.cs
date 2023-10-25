// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgeDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class MultiplayerBadgeDataSO : ScriptableObject
{
  [SerializeField]
  [LocalizationKey]
  private string _titleLocalizationKey;
  [SerializeField]
  [LocalizationKey]
  private string _subtitleLocalizationKey;
  [SerializeField]
  private Sprite _icon;

  public Sprite icon => this._icon;

  public string titleLocalizationKey => this._titleLocalizationKey;

  public string subtitleLocalizationKey => this._subtitleLocalizationKey;

  public abstract MultiplayerBadgeAwardData CalculateBadgeData(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    PlayerDataModel playerDataModel,
    IDifficultyBeatmap difficultyBeatmap,
    float randomMultiplier);
}
