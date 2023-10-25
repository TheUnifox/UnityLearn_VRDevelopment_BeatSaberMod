// Decompiled with JetBrains decompiler
// Type: OnlineServices.GetLeaderboardFilterData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace OnlineServices
{
  public readonly struct GetLeaderboardFilterData
  {
    public readonly IDifficultyBeatmap beatmap;
    public readonly int count;
    public readonly int fromRank;
    public readonly ScoresScope scope;
    public readonly GameplayModifiers gameplayModifiers;

    public GetLeaderboardFilterData(
      IDifficultyBeatmap beatmap,
      int count,
      int fromRank,
      ScoresScope scope,
      GameplayModifiers gameplayModifiers)
    {
      this.beatmap = beatmap;
      this.count = count;
      this.fromRank = fromRank;
      this.scope = scope;
      this.gameplayModifiers = gameplayModifiers;
    }
  }
}
