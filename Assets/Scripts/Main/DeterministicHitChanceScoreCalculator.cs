// Decompiled with JetBrains decompiler
// Type: DeterministicHitChanceScoreCalculator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class DeterministicHitChanceScoreCalculator : IMockPlayerScoreCalculator
{
  protected readonly float _hitChance;
  protected const int kScorePerHit = 105;
  protected float _chanceAggregated;

  public DeterministicHitChanceScoreCalculator(float hitChance) => this._hitChance = hitChance;

  public virtual int GetScoreForNote(MockNoteData noteData)
  {
    this._chanceAggregated += this._hitChance;
    if ((double) this._chanceAggregated < 1.0)
      return 0;
    --this._chanceAggregated;
    return 105;
  }
}
