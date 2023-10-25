// Decompiled with JetBrains decompiler
// Type: OnlineServices.LevelScoreResultsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace OnlineServices
{
  public readonly struct LevelScoreResultsData
  {
    public readonly IDifficultyBeatmap difficultyBeatmap;
    public readonly int multipliedScore;
    public readonly int modifiedScore;
    public readonly bool fullCombo;
    public readonly int goodCutsCount;
    public readonly int badCutsCount;
    public readonly int missedCount;
    public readonly int maxCombo;
    public readonly GameplayModifiers gameplayModifiers;

    public LevelScoreResultsData(
      IDifficultyBeatmap difficultyBeatmap,
      int multipliedScore,
      int modifiedScore,
      bool fullCombo,
      int goodCutsCount,
      int badCutsCount,
      int missedCount,
      int maxCombo,
      GameplayModifiers gameplayModifiers)
    {
      this.multipliedScore = multipliedScore;
      this.difficultyBeatmap = difficultyBeatmap;
      this.modifiedScore = modifiedScore;
      this.fullCombo = fullCombo;
      this.goodCutsCount = goodCutsCount;
      this.badCutsCount = badCutsCount;
      this.missedCount = missedCount;
      this.maxCombo = maxCombo;
      this.gameplayModifiers = gameplayModifiers;
    }

    public override string ToString() => string.Format("LevelScoreResultsData: difficultyBeatmap = {0}, multipliedScore = {1}, modifiedScore = {2}, gameplayModifiers = {3}", (object) this.difficultyBeatmap, (object) this.multipliedScore, (object) this.modifiedScore, (object) this.gameplayModifiers);
  }
}
