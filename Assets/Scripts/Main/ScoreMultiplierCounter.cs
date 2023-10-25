// Decompiled with JetBrains decompiler
// Type: ScoreMultiplierCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class ScoreMultiplierCounter
{
  protected int _multiplier = 1;
  protected int _multiplierIncreaseProgress;
  protected int _multiplierIncreaseMaxProgress = 2;

  public int multiplier => this._multiplier;

  public float normalizedProgress => (float) this._multiplierIncreaseProgress / (float) this._multiplierIncreaseMaxProgress;

  public virtual void Reset()
  {
    this._multiplier = 1;
    this._multiplierIncreaseProgress = 0;
    this._multiplierIncreaseMaxProgress = 2;
  }

  public virtual bool ProcessMultiplierEvent(
    ScoreMultiplierCounter.MultiplierEventType multiplierEventType)
  {
    bool flag = false;
    switch (multiplierEventType)
    {
      case ScoreMultiplierCounter.MultiplierEventType.Positive:
        if (this._multiplier < 8)
        {
          if (this._multiplierIncreaseProgress < this._multiplierIncreaseMaxProgress)
          {
            ++this._multiplierIncreaseProgress;
            flag = true;
          }
          if (this._multiplierIncreaseProgress >= this._multiplierIncreaseMaxProgress)
          {
            this._multiplier *= 2;
            this._multiplierIncreaseProgress = 0;
            this._multiplierIncreaseMaxProgress = this._multiplier * 2;
            flag = true;
            break;
          }
          break;
        }
        break;
      case ScoreMultiplierCounter.MultiplierEventType.Negative:
        if (this._multiplierIncreaseProgress > 0)
        {
          this._multiplierIncreaseProgress = 0;
          flag = true;
        }
        if (this._multiplier > 1)
        {
          this._multiplier /= 2;
          this._multiplierIncreaseMaxProgress = this._multiplier * 2;
          flag = true;
          break;
        }
        break;
    }
    return flag;
  }

  public enum MultiplierEventType
  {
    Positive,
    Neutral,
    Negative,
  }
}
