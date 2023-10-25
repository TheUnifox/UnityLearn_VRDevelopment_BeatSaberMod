// Decompiled with JetBrains decompiler
// Type: OnlineServices.LeaderboardEntriesResult
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

namespace OnlineServices
{
  public class LeaderboardEntriesResult
  {
    public readonly bool isError;
    public readonly string localizedErrorMessage;
    public readonly LeaderboardEntryData[] leaderboardEntries;
    public readonly int referencePlayerScoreIndex;

    private LeaderboardEntriesResult(
      LeaderboardEntryData[] leaderboardEntries,
      int referencePlayerScoreIndex,
      bool isError,
      string localizedErrorMessage)
    {
      this.isError = isError;
      this.localizedErrorMessage = localizedErrorMessage;
      this.leaderboardEntries = leaderboardEntries;
      this.referencePlayerScoreIndex = referencePlayerScoreIndex;
    }

    private static LeaderboardEntriesResult ErrorResult(string localizedErrorMessage) => new LeaderboardEntriesResult((LeaderboardEntryData[]) null, -1, true, localizedErrorMessage);

    public static LeaderboardEntriesResult notInicializedError => LeaderboardEntriesResult.ErrorResult(Localization.Get("LEADERBOARDS_NOT_INITIALIZED_ERROR"));

    public static LeaderboardEntriesResult somethingWentWrongError => LeaderboardEntriesResult.ErrorResult(Localization.Get("LEADERBOARDS_SOMETHING_WENT_WRONG_ERROR"));

    public static LeaderboardEntriesResult onlineServicesUnavailableError => LeaderboardEntriesResult.ErrorResult(Localization.Get("LEADERBOARDS_PLATFORM_SERVICES_ERROR"));

    public static LeaderboardEntriesResult FromGetLeaderboardEntriesResult(
      GetLeaderboardEntriesResult getLeaderboardEntriesResult)
    {
      return new LeaderboardEntriesResult(getLeaderboardEntriesResult.leaderboardEntries, getLeaderboardEntriesResult.referencePlayerScoreIndex, getLeaderboardEntriesResult.isError, (string) null);
    }
  }
}
