// Decompiled with JetBrains decompiler
// Type: OnlineServicesInfoViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using OnlineServices;
using System;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class OnlineServicesInfoViewController : ViewController
{
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [SerializeField]
  protected BeatmapLevelSO _testBeatmapLevel;
  [SerializeField]
  protected BeatmapCharacteristicSO _testBeatmapCharacteristic;
  [SerializeField]
  protected Button _refreshButton;
  [Inject]
  protected readonly ServerManager _serverManager;
  [Inject]
  protected readonly IPlatformUserModel _platformUserModel;
  protected StringBuilder _sb = new StringBuilder();

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.GetServerStatus();
    this._refreshButton.onClick.AddListener(new UnityAction(this.GetServerStatus));
  }

  public virtual async void GetServerStatus()
  {
    this.AppendLine("Initializing Server Manager");
    this.AppendLine(string.Format("Server Manager initialized: {0}", (object) this._serverManager.initialized));
    this.AppendLine("User Info: " + (await this._platformUserModel.GetUserInfo()).userName);
    GetLeaderboardFilterData leaderboardFilterData = new GetLeaderboardFilterData(this._testBeatmapLevel.GetDifficultyBeatmap(this._testBeatmapCharacteristic, BeatmapDifficulty.Normal), 10, 1, OnlineServices.ScoresScope.Global, (GameplayModifiers) null);
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    try
    {
      CancellationToken cancellationToken = cancellationTokenSource.Token;
      this.AppendLine("Getting Leaderboards test data");
      LeaderboardEntriesResult leaderboardEntriesAsync = await this._serverManager.GetLeaderboardEntriesAsync(leaderboardFilterData, cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      if (leaderboardEntriesAsync.isError)
        this.AppendLine("Error: " + leaderboardEntriesAsync.localizedErrorMessage);
      else
        this.AppendLine(string.Format("Entries: {0}", (object) leaderboardEntriesAsync.leaderboardEntries.Length));
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException ex)
    {
      this.AppendLine("Canceled");
    }
  }

  public virtual void AppendLine(string line)
  {
    this._sb.AppendLine(line);
    this._textPageScrollView.SetText(this._sb.ToString());
  }
}
