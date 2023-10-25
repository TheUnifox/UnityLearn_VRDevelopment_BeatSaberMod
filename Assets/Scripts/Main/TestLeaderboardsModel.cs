// Decompiled with JetBrains decompiler
// Type: TestLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using OnlineServices;
using System.Threading;
using System.Threading.Tasks;

public class TestLeaderboardsModel : ILeaderboardsModel
{
  public event System.Action<string> scoreForLeaderboardDidUploadEvent;

  public virtual string GetLeaderboardId(IDifficultyBeatmap difficultyBeatmap) => difficultyBeatmap.SerializedName();

  public virtual async Task<GetLeaderboardEntriesResult> GetLeaderboardEntriesAsync(
    GetLeaderboardFilterData leaderboardFilterData,
    CancellationToken cancellationToken)
  {
    await Task.Delay(200);
    LeaderboardEntryData[] leaderboardEntries = new LeaderboardEntryData[10];
    int referencePlayerScoreIndex = UnityEngine.Random.Range(0, 10);
    for (int index = 0; index < leaderboardEntries.Length; ++index)
    {
      string displayName = referencePlayerScoreIndex != index ? leaderboardFilterData.beatmap.SerializedName() + " " + (object) UnityEngine.Random.Range(100000, 999999) : string.Format("YOU - {0}", (object) leaderboardFilterData.scope);
      leaderboardEntries[index] = new LeaderboardEntryData(10000 / (index + 1) + UnityEngine.Random.Range(0, 100), leaderboardFilterData.fromRank + index, displayName, (string) null, (GameplayModifiers) null);
    }
    return new GetLeaderboardEntriesResult(false, leaderboardEntries, referencePlayerScoreIndex);
  }

  public virtual async Task<SendLeaderboardEntryResult> SendLevelScoreResultAsync(
    LevelScoreResultsData levelScoreResult,
    CancellationToken cancellationToken)
  {
    await Task.Delay(200);
    System.Action<string> leaderboardDidUploadEvent = this.scoreForLeaderboardDidUploadEvent;
    if (leaderboardDidUploadEvent != null)
      leaderboardDidUploadEvent(this.GetLeaderboardId(levelScoreResult.difficultyBeatmap));
    return SendLeaderboardEntryResult.OK;
  }
}
