// Decompiled with JetBrains decompiler
// Type: HTTPLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using OnlineServices;
using OnlineServices.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.XR;
using Zenject;

public class HTTPLeaderboardsModel : ILeaderboardsModel
{
  [Inject]
  protected readonly IUserLoginDtoDataSource _userLoginDataSource;
  [Inject]
  protected readonly IApiLeaderboardsModel _apiLeaderboardsModel;
  protected readonly string _guid = Guid.NewGuid().ToString();
  protected string[] _friendsUserIds;
  protected string _platformUserId;

  public event System.Action<string> scoreForLeaderboardDidUploadEvent;

  public virtual void LogoutAsync() => this._apiLeaderboardsModel.LogoutAsync();

  public virtual string GetLeaderboardId(IDifficultyBeatmap difficultyBeatmap)
  {
    string str = "Unknown";
    switch (difficultyBeatmap.difficulty)
    {
      case BeatmapDifficulty.Easy:
        str = "Easy";
        break;
      case BeatmapDifficulty.Normal:
        str = "Normal";
        break;
      case BeatmapDifficulty.Hard:
        str = "Hard";
        break;
      case BeatmapDifficulty.Expert:
        str = "Expert";
        break;
      case BeatmapDifficulty.ExpertPlus:
        str = "ExpertPlus";
        break;
    }
    return difficultyBeatmap.level.levelID + difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.compoundIdPartName + str;
  }

  public virtual async Task<GetLeaderboardEntriesResult> GetLeaderboardEntriesAsync(
    GetLeaderboardFilterData leaderboardFilterData,
    CancellationToken cancellationToken)
  {
    string leaderboardId = this.GetLeaderboardId(leaderboardFilterData.beatmap);
    if (this._friendsUserIds == null)
      this._friendsUserIds = await this._userLoginDataSource.GetUserFriendsUserIds(cancellationToken);
    if (this._platformUserId == null)
      this._platformUserId = await this._userLoginDataSource.GetPlatformUserIdAsync(cancellationToken);
    BeatSaberAPI.DataTransferObjects.ScoresScope scoresScope;
    switch (leaderboardFilterData.scope)
    {
      case OnlineServices.ScoresScope.Global:
        scoresScope = BeatSaberAPI.DataTransferObjects.ScoresScope.Global;
        break;
      case OnlineServices.ScoresScope.Friends:
        scoresScope = BeatSaberAPI.DataTransferObjects.ScoresScope.Friends;
        break;
      default:
        scoresScope = BeatSaberAPI.DataTransferObjects.ScoresScope.Global;
        break;
    }
    ApiResponse<LeaderboardEntries> leaderboardEntriesAsync = await this._apiLeaderboardsModel.GetLeaderboardEntriesAsync(new LeaderboardQuery()
    {
      leaderboardId = leaderboardId,
      count = leaderboardFilterData.count,
      fromRank = leaderboardFilterData.fromRank,
      scope = scoresScope,
      friendsUserIds = this._friendsUserIds,
      onlyWithSpecificGameplayModifiers = leaderboardFilterData.gameplayModifiers != null,
      gameplayModifiers = GameplayModifiersHelper.ToDTO(leaderboardFilterData.gameplayModifiers)
    }, cancellationToken);
    if (leaderboardEntriesAsync.isError)
      return new GetLeaderboardEntriesResult(true, (LeaderboardEntryData[]) null, -1);
    string[] array = ((IEnumerable<BeatSaberAPI.DataTransferObjects.LeaderboardEntry>) leaderboardEntriesAsync.responseDto.entries).Select<BeatSaberAPI.DataTransferObjects.LeaderboardEntry, string>((Func<BeatSaberAPI.DataTransferObjects.LeaderboardEntry, string>) (x => x.userDisplayName)).ToArray<string>();
    List<LeaderboardEntryData> leaderboardEntryDataList = new List<LeaderboardEntryData>();
    BeatSaberAPI.DataTransferObjects.LeaderboardEntry[] entries = leaderboardEntriesAsync.responseDto.entries;
    int referencePlayerScoreIndex = -1;
    if (entries != null)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        BeatSaberAPI.DataTransferObjects.LeaderboardEntry leaderboardEntry = entries[index];
        GameplayModifiers gameplayModifiers = GameplayModifiersHelper.FromDTO(leaderboardEntry.gameplayModifiers);
        leaderboardEntryDataList.Add(new LeaderboardEntryData(leaderboardEntry.score, leaderboardEntry.rank, array[index], leaderboardEntry.platformUserId, gameplayModifiers));
        if (leaderboardEntry.platformUserId == this._platformUserId)
          referencePlayerScoreIndex = index;
      }
    }
    return new GetLeaderboardEntriesResult(leaderboardEntriesAsync.isError, leaderboardEntryDataList.ToArray(), referencePlayerScoreIndex);
  }

  public virtual async Task<SendLeaderboardEntryResult> SendLevelScoreResultAsync(
    LevelScoreResultsData levelResultsData,
    CancellationToken cancellationToken)
  {
    string leaderboardId = this.GetLeaderboardId(levelResultsData.difficultyBeatmap);
    LevelScoreResult.GameplayModifiers[] dto = GameplayModifiersHelper.ToDTO(levelResultsData.gameplayModifiers);
    int num = (int) await this._apiLeaderboardsModel.SendLevelScoreResultAsync(new LevelScoreResult()
    {
      leaderboardId = leaderboardId,
      multipliedScore = levelResultsData.multipliedScore,
      modifiedScore = levelResultsData.modifiedScore,
      fullCombo = levelResultsData.fullCombo,
      goodCutsCount = levelResultsData.goodCutsCount,
      badCutsCount = levelResultsData.badCutsCount,
      missedCount = levelResultsData.missedCount,
      maxCombo = levelResultsData.maxCombo,
      gameplayModifiers = dto,
      deviceModel = XRDevice.model
    }, cancellationToken);
    if (num == 0)
    {
      System.Action<string> leaderboardDidUploadEvent = this.scoreForLeaderboardDidUploadEvent;
      if (leaderboardDidUploadEvent != null)
        leaderboardDidUploadEvent(leaderboardId);
    }
    return num != 0 ? SendLeaderboardEntryResult.Failed : SendLeaderboardEntryResult.OK;
  }
}
