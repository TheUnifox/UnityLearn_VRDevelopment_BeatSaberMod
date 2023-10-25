// Decompiled with JetBrains decompiler
// Type: NoteExecutionRating
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class NoteExecutionRating : BeatmapObjectExecutionRating
{
  public readonly NoteExecutionRating.Rating rating;
  public readonly int cutScore;
  public readonly int beforeCutScore;
  public readonly int centerDistanceCutScore;
  public readonly int afterCutScore;
  public readonly NoteData.ScoringType scoringType;

  public NoteExecutionRating(
    float time,
    NoteData.ScoringType scoringType,
    NoteExecutionRating.Rating rating,
    int cutScore,
    int beforeCutScore,
    int centerDistanceCutScore,
    int afterCutScore)
    : base(time)
  {
    this.rating = rating;
    this.cutScore = cutScore;
    this.beforeCutScore = beforeCutScore;
    this.centerDistanceCutScore = centerDistanceCutScore;
    this.afterCutScore = afterCutScore;
    this.scoringType = scoringType;
  }

  public enum Rating
  {
    GoodCut,
    Miss,
    BadCut,
  }
}
