// Decompiled with JetBrains decompiler
// Type: ScoringElement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using Zenject;

public abstract class ScoringElement : IComparable<ScoringElement>
{
  public NoteData noteData { get; protected set; }

  public int maxPossibleCutScore => ScoreModel.GetNoteScoreDefinition(this.noteData.scoringType).maxCutScore;

  public float time => this.noteData.time;

  public int multiplier { get; private set; }

  public int maxMultiplier { get; private set; }

  public abstract int cutScore { get; }

  public abstract ScoreMultiplierCounter.MultiplierEventType wouldBeCorrectCutBestPossibleMultiplierEventType { get; }

  public abstract ScoreMultiplierCounter.MultiplierEventType multiplierEventType { get; }

  public bool isFinished { get; protected set; }

  protected abstract int executionOrder { get; }

  public int CompareTo(ScoringElement other)
  {
    int num = this.time.CompareTo(other.time);
    return num == 0 ? this.executionOrder.CompareTo(other.executionOrder) : num;
  }

  public void SetMultipliers(int multiplier, int maxMultiplier)
  {
    this.multiplier = multiplier;
    this.maxMultiplier = maxMultiplier;
  }

  protected virtual void Reinitialize()
  {
  }

  public class Pool<T> : MemoryPool<T> where T : ScoringElement
  {
    protected override void Reinitialize(T scoringElement) => scoringElement.Reinitialize();
  }
}
