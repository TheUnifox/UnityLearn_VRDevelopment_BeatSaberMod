// Decompiled with JetBrains decompiler
// Type: OnlineServices.GetLeaderboardEntriesResult
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace OnlineServices
{
  public readonly struct GetLeaderboardEntriesResult
  {
    public readonly bool isError;
    public readonly LeaderboardEntryData[] leaderboardEntries;
    public readonly int referencePlayerScoreIndex;

    public GetLeaderboardEntriesResult(
      bool isError,
      LeaderboardEntryData[] leaderboardEntries,
      int referencePlayerScoreIndex)
    {
      this.isError = isError;
      this.leaderboardEntries = leaderboardEntries;
      this.referencePlayerScoreIndex = referencePlayerScoreIndex;
    }

    public static GetLeaderboardEntriesResult resultWithError => new GetLeaderboardEntriesResult(true, (LeaderboardEntryData[]) null, -1);
  }
}
