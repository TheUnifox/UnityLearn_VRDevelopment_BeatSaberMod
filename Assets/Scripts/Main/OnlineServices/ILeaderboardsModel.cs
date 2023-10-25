// Decompiled with JetBrains decompiler
// Type: OnlineServices.ILeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;

namespace OnlineServices
{
  public interface ILeaderboardsModel
  {
    string GetLeaderboardId(IDifficultyBeatmap difficultyBeatmap);

    Task<GetLeaderboardEntriesResult> GetLeaderboardEntriesAsync(
      GetLeaderboardFilterData leaderboardFilterData,
      CancellationToken cancellationToken);

    Task<SendLeaderboardEntryResult> SendLevelScoreResultAsync(
      LevelScoreResultsData levelScoreResultsData,
      CancellationToken cancellationToken);
  }
}
