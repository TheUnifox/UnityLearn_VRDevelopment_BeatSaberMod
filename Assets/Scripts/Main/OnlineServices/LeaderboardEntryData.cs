// Decompiled with JetBrains decompiler
// Type: OnlineServices.LeaderboardEntryData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace OnlineServices
{
  public class LeaderboardEntryData
  {
    public readonly int score;
    public readonly int rank;
    public string displayName;
    public readonly string playerId;
    public readonly GameplayModifiers gameplayModifiers;

    public LeaderboardEntryData(
      int score,
      int rank,
      string displayName,
      string playerId,
      GameplayModifiers gameplayModifiers)
    {
      this.score = score;
      this.rank = rank;
      this.displayName = displayName;
      this.playerId = playerId;
      this.gameplayModifiers = gameplayModifiers;
    }

    public override string ToString() => string.Format("LeaderboardEntry: score = {0}, rank = {1}, playerName = {2}, playerId = {3}, gameplayModifiers = {4}", (object) this.score, (object) this.rank, (object) this.displayName, (object) this.playerId, (object) this.gameplayModifiers);
  }
}
