// Decompiled with JetBrains decompiler
// Type: BeatmapDifficultyMethods
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

public static class BeatmapDifficultyMethods
{
  public static string Name(this BeatmapDifficulty difficulty)
  {
    switch (difficulty)
    {
      case BeatmapDifficulty.Easy:
        return Localization.Get("DIFFICULTY_EASY");
      case BeatmapDifficulty.Normal:
        return Localization.Get("DIFFICULTY_NORMAL");
      case BeatmapDifficulty.Hard:
        return Localization.Get("DIFFICULTY_HARD");
      case BeatmapDifficulty.Expert:
        return Localization.Get("DIFFICULTY_EXPERT");
      case BeatmapDifficulty.ExpertPlus:
        return Localization.Get("DIFFICULTY_EXPERT_PLUS");
      default:
        return Localization.Get("DIFFICULTY_UNKNOWN");
    }
  }

  public static string ShortName(this BeatmapDifficulty difficulty)
  {
    switch (difficulty)
    {
      case BeatmapDifficulty.Easy:
        return Localization.Get("DIFFICULTY_EASY_SHORT");
      case BeatmapDifficulty.Normal:
        return Localization.Get("DIFFICULTY_NORMAL_SHORT");
      case BeatmapDifficulty.Hard:
        return Localization.Get("DIFFICULTY_HARD_SHORT");
      case BeatmapDifficulty.Expert:
        return Localization.Get("DIFFICULTY_EXPERT_SHORT");
      case BeatmapDifficulty.ExpertPlus:
        return Localization.Get("DIFFICULTY_EXPERT_PLUS_SHORT");
      default:
        return Localization.Get("DIFFICULTY_UNKNOWN_SHORT");
    }
  }

  public static int DefaultRating(this BeatmapDifficulty difficulty)
  {
    switch (difficulty)
    {
      case BeatmapDifficulty.Easy:
        return 1;
      case BeatmapDifficulty.Normal:
        return 3;
      case BeatmapDifficulty.Hard:
        return 5;
      case BeatmapDifficulty.Expert:
        return 7;
      case BeatmapDifficulty.ExpertPlus:
        return 9;
      default:
        return 5;
    }
  }

  public static float NoteJumpMovementSpeed(this BeatmapDifficulty difficulty)
  {
    switch (difficulty)
    {
      case BeatmapDifficulty.Easy:
        return 10f;
      case BeatmapDifficulty.Normal:
        return 10f;
      case BeatmapDifficulty.Hard:
        return 10f;
      case BeatmapDifficulty.Expert:
        return 12f;
      case BeatmapDifficulty.ExpertPlus:
        return 16f;
      default:
        return 5f;
    }
  }
}
