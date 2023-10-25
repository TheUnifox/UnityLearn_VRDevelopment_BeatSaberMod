// Decompiled with JetBrains decompiler
// Type: GoodCutScoringElement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class GoodCutScoringElement : ScoringElement, ICutScoreBufferDidFinishReceiver
{
  protected readonly CutScoreBuffer _cutScoreBuffer = new CutScoreBuffer();
  protected ScoreMultiplierCounter.MultiplierEventType _multiplierEventType;
  protected ScoreMultiplierCounter.MultiplierEventType _wouldBeCorrectCutBestPossibleMultiplierEventType;

  public override int cutScore
  {
    get
    {
      CutScoreBuffer cutScoreBuffer = this._cutScoreBuffer;
      return cutScoreBuffer == null ? 0 : __nonvirtual (cutScoreBuffer.cutScore);
    }
  }

  public override ScoreMultiplierCounter.MultiplierEventType wouldBeCorrectCutBestPossibleMultiplierEventType => this._wouldBeCorrectCutBestPossibleMultiplierEventType;

  public override ScoreMultiplierCounter.MultiplierEventType multiplierEventType => this._multiplierEventType;

  public IReadonlyCutScoreBuffer cutScoreBuffer => (IReadonlyCutScoreBuffer) this._cutScoreBuffer;

  protected override int executionOrder => this._cutScoreBuffer.executionOrder;

  protected override void Reinitialize()
  {
    this.isFinished = false;
    this._cutScoreBuffer?.UnregisterDidFinishReceiver((ICutScoreBufferDidFinishReceiver) this);
  }

  public virtual void Init(NoteCutInfo noteCutInfo)
  {
    this.noteData = noteCutInfo.noteData;
    switch (this.noteData.scoringType)
    {
      case NoteData.ScoringType.Ignore:
        this._multiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        this._wouldBeCorrectCutBestPossibleMultiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        break;
      case NoteData.ScoringType.NoScore:
        this._multiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        this._wouldBeCorrectCutBestPossibleMultiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Neutral;
        break;
      case NoteData.ScoringType.Normal:
      case NoteData.ScoringType.SliderHead:
      case NoteData.ScoringType.SliderTail:
      case NoteData.ScoringType.BurstSliderHead:
      case NoteData.ScoringType.BurstSliderElement:
        this._multiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Positive;
        this._wouldBeCorrectCutBestPossibleMultiplierEventType = ScoreMultiplierCounter.MultiplierEventType.Positive;
        break;
    }
    if (this._cutScoreBuffer.Init(in noteCutInfo))
    {
      this._cutScoreBuffer.RegisterDidFinishReceiver((ICutScoreBufferDidFinishReceiver) this);
      this.isFinished = false;
    }
    else
      this.isFinished = true;
  }

  public virtual void HandleCutScoreBufferDidFinish(CutScoreBuffer cutScoreBuffer)
  {
    this.isFinished = true;
    this._cutScoreBuffer.UnregisterDidFinishReceiver((ICutScoreBufferDidFinishReceiver) this);
  }

  public class Pool : ScoringElement.Pool<GoodCutScoringElement>
  {
  }
}
