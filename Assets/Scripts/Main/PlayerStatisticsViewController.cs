// Decompiled with JetBrains decompiler
// Type: PlayerStatisticsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerStatisticsViewController : ViewController
{
  [SerializeField]
  protected TextSegmentedControl _statsScopeSegmentedControl;
  [Space]
  [SerializeField]
  protected TextMeshProUGUI _playedLevelsCountText;
  [SerializeField]
  protected TextMeshProUGUI _clearedLevelsCountText;
  [SerializeField]
  protected TextMeshProUGUI _failedLevelsCountText;
  [SerializeField]
  protected TextMeshProUGUI _timePlayedText;
  [SerializeField]
  protected TextMeshProUGUI _goodCutsCountText;
  [SerializeField]
  protected TextMeshProUGUI _badCutsCountCountText;
  [SerializeField]
  protected TextMeshProUGUI _missedCountText;
  [SerializeField]
  protected TextMeshProUGUI _totalScoreText;
  [SerializeField]
  protected TextMeshProUGUI _fullComboCountText;
  [SerializeField]
  protected TextMeshProUGUI _handDistanceTravelledText;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly AppStaticSettingsSO _appStaticSettings;
  protected PlayerStatisticsViewController.StatsScopeData[] _statsScopeDatas;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      List<PlayerStatisticsViewController.StatsScopeData> statsScopeDataList = new List<PlayerStatisticsViewController.StatsScopeData>()
      {
        new PlayerStatisticsViewController.StatsScopeData(Localization.Get("STATS_ALL"), (Func<PlayerAllOverallStatsData.PlayerOverallStatsData>) (() => this._playerDataModel.playerData.playerAllOverallStatsData.allOverallStatsData)),
        new PlayerStatisticsViewController.StatsScopeData(Localization.Get("TITLE_CAMPAIGN"), (Func<PlayerAllOverallStatsData.PlayerOverallStatsData>) (() => this._playerDataModel.playerData.playerAllOverallStatsData.campaignOverallStatsData)),
        new PlayerStatisticsViewController.StatsScopeData(Localization.Get("TITLE_SOLO"), (Func<PlayerAllOverallStatsData.PlayerOverallStatsData>) (() => this._playerDataModel.playerData.playerAllOverallStatsData.soloFreePlayOverallStatsData)),
        new PlayerStatisticsViewController.StatsScopeData(Localization.Get("TITLE_PARTY"), (Func<PlayerAllOverallStatsData.PlayerOverallStatsData>) (() => this._playerDataModel.playerData.playerAllOverallStatsData.partyFreePlayOverallStatsData))
      };
      if (!this._appStaticSettings.disableMultiplayer)
        statsScopeDataList.Add(new PlayerStatisticsViewController.StatsScopeData(Localization.Get("BUTTON_MULTIPLAYER"), (Func<PlayerAllOverallStatsData.PlayerOverallStatsData>) (() => this._playerDataModel.playerData.playerAllOverallStatsData.onlinePlayOverallStatsData)));
      this._statsScopeDatas = statsScopeDataList.ToArray();
      string[] texts = new string[this._statsScopeDatas.Length];
      for (int index = 0; index < texts.Length; ++index)
        texts[index] = this._statsScopeDatas[index].text;
      this._statsScopeSegmentedControl.SetTexts((IReadOnlyList<string>) texts);
    }
    if (addedToHierarchy)
      this._statsScopeSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleStatsScopeSegmentedControlDidSelectCell);
    this.UpdateView(this._statsScopeDatas[this._statsScopeSegmentedControl.selectedCellNumber].playerOverallStatsDataFunc());
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._statsScopeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleStatsScopeSegmentedControlDidSelectCell);
  }

  public virtual void UpdateView(
    PlayerAllOverallStatsData.PlayerOverallStatsData playerOverallStatsData)
  {
    this._playedLevelsCountText.text = playerOverallStatsData.playedLevelsCount.ToString("# ### ### ##0");
    this._clearedLevelsCountText.text = playerOverallStatsData.clearedLevelsCount.ToString("# ### ### ##0");
    this._failedLevelsCountText.text = playerOverallStatsData.failedLevelsCount.ToString("# ### ### ##0");
    double timePlayed = (double) playerOverallStatsData.timePlayed;
    int num1 = ((float) timePlayed).TotalHours();
    int num2 = ((float) timePlayed).Minutes();
    int num3 = ((float) timePlayed).Seconds();
    if (num1 > 0)
      this._timePlayedText.text = num1.ToString() + "h " + (object) num2 + "m";
    else
      this._timePlayedText.text = num2.ToString() + "m " + (object) num3 + "s";
    this._goodCutsCountText.text = playerOverallStatsData.goodCutsCount.ToString("# ### ### ##0");
    this._badCutsCountCountText.text = playerOverallStatsData.badCutsCount.ToString("# ### ### ##0");
    this._missedCountText.text = playerOverallStatsData.missedCutsCount.ToString("# ### ### ##0");
    this._totalScoreText.text = playerOverallStatsData.totalScore.ToString("# ### ### ##0");
    TextMeshProUGUI fullComboCountText = this._fullComboCountText;
    int num4 = playerOverallStatsData.fullComboCount;
    string str1 = num4.ToString("# ### ### ##0");
    fullComboCountText.text = str1;
    if (playerOverallStatsData.handDistanceTravelled > 1000)
    {
      this._handDistanceTravelledText.text = ((float) playerOverallStatsData.handDistanceTravelled / 1000f).ToString("### ### ###.0") + "km";
    }
    else
    {
      TextMeshProUGUI distanceTravelledText = this._handDistanceTravelledText;
      num4 = playerOverallStatsData.handDistanceTravelled;
      string str2 = num4.ToString() + "m";
      distanceTravelledText.text = str2;
    }
  }

  public virtual void HandleStatsScopeSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellIdx)
  {
    this.UpdateView(this._statsScopeDatas[cellIdx].playerOverallStatsDataFunc());
  }

  [CompilerGenerated]
  public virtual PlayerAllOverallStatsData.PlayerOverallStatsData m_CDidActivatem_Eb__15_1() => this._playerDataModel.playerData.playerAllOverallStatsData.allOverallStatsData;

  [CompilerGenerated]
  public virtual PlayerAllOverallStatsData.PlayerOverallStatsData m_CDidActivatem_Eb__15_2() => this._playerDataModel.playerData.playerAllOverallStatsData.campaignOverallStatsData;

  [CompilerGenerated]
  public virtual PlayerAllOverallStatsData.PlayerOverallStatsData m_CDidActivatem_Eb__15_3() => this._playerDataModel.playerData.playerAllOverallStatsData.soloFreePlayOverallStatsData;

  [CompilerGenerated]
  public virtual PlayerAllOverallStatsData.PlayerOverallStatsData m_CDidActivatem_Eb__15_4() => this._playerDataModel.playerData.playerAllOverallStatsData.partyFreePlayOverallStatsData;

  [CompilerGenerated]
  public virtual PlayerAllOverallStatsData.PlayerOverallStatsData m_CDidActivatem_Eb__15_0() => this._playerDataModel.playerData.playerAllOverallStatsData.onlinePlayOverallStatsData;

    public struct StatsScopeData
    {
        public string text { get; private set; }

        public Func<PlayerAllOverallStatsData.PlayerOverallStatsData> playerOverallStatsDataFunc { get; private set; }

        public StatsScopeData(string text, Func<PlayerAllOverallStatsData.PlayerOverallStatsData> playerOverallStatsDataFunc)
        {
            this.text = text;
            this.playerOverallStatsDataFunc = playerOverallStatsDataFunc;
        }
    }
}
