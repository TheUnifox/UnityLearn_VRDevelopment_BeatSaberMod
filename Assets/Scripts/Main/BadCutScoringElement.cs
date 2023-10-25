// Decompiled with JetBrains decompiler
// Type: BadCutScoringElement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class BadCutScoringElement : ScoringElement
{
  protected ScoreMultiplierCounter.MultiplierEventType _multiplierEventType;
  protected ScoreMultiplierCounter.MultiplierEventType _wouldBeCorrectCutBestPossibleMultiplierEventType;

  public override int cutScore => 0;

  public override ScoreMultiplierCounter.MultiplierEventType wouldBeCorrectCutBestPossibleMultiplierEventType => this._wouldBeCorrectCutBestPossibleMultiplierEventType;

  public override ScoreMultiplierCounter.MultiplierEventType multiplierEventType => this._multiplierEventType;

  protected override int executionOrder => 100000;

  public virtual void Init(NoteData noteData)
  {
    this.isFinished = true;
    this.noteData = noteData;
    switch (noteData.scoringType)
    {
      case NoteData.ScoringType.Ignore:
        this._multiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        this._wouldBeCorrectCutBestPossibleMultiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        break;
      case NoteData.ScoringType.NoScore:
        this._multiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Negative;
        this._wouldBeCorrectCutBestPossibleMultiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        break;
      case NoteData.ScoringType.Normal:
      case NoteData.ScoringType.SliderHead:
      case NoteData.ScoringType.SliderTail:
      case NoteData.ScoringType.BurstSliderHead:
      case NoteData.ScoringType.BurstSliderElement:
        this._multiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Negative;
        this._wouldBeCorrectCutBestPossibleMultiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Positive;
        break;
    }
  }

  public class Pool : ScoringElement.Pool<BadCutScoringElement>
  {
  }
}
