// Decompiled with JetBrains decompiler
// Type: LevelStatsView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using TMPro;
using UnityEngine;

public class LevelStatsView : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _highScoreText;
  [SerializeField]
  protected TextMeshProUGUI _maxComboText;
  [SerializeField]
  protected TextMeshProUGUI _maxRankText;

  public virtual void Hide() => this.gameObject.SetActive(false);

  public virtual void ShowStats(IDifficultyBeatmap difficultyBeatmap, PlayerData playerData)
  {
    this.gameObject.SetActive(true);
    PlayerLevelStatsData playerLevelStatsData = playerData.GetPlayerLevelStatsData(difficultyBeatmap.level.levelID, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
    TextMeshProUGUI highScoreText = this._highScoreText;
    int num;
    string str1;
    if (!playerLevelStatsData.validScore)
    {
      str1 = "-";
    }
    else
    {
      num = playerLevelStatsData.highScore;
      str1 = num.ToString();
    }
    highScoreText.text = str1;
    string upper;
    if (!playerLevelStatsData.fullCombo)
    {
      num = playerLevelStatsData.maxCombo;
      upper = num.ToString();
    }
    else
      upper = Localization.Get("STATS_FULL_COMBO").ToUpper();
    string str2 = upper;
    this._maxComboText.text = playerLevelStatsData.validScore ? str2 : "-";
    this._maxRankText.text = playerLevelStatsData.validScore ? RankModel.GetRankName(playerLevelStatsData.maxRank) : "-";
  }
}
