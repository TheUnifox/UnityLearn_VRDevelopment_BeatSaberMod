// Decompiled with JetBrains decompiler
// Type: AveragingValueRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class AveragingValueRecorder
{
  protected float _averageWindowDuration = 1f;
  protected float _historyValuesPerSecond = 1f;
  protected int _historyValuesCount = 10;
  protected Queue<AveragingValueRecorder.AverageValueData> _averageWindowValues;
  protected Queue<float> _historyValues;
  protected float _time;
  protected float _historyTime;
  protected float _averageValue;
  protected float _averageWindowValuesDuration;
  protected float _lastValue;

  public AveragingValueRecorder(
    float averageWindowDuration,
    float historyWindowDuration,
    float historyValuesPerSecond)
  {
    this._averageWindowDuration = averageWindowDuration;
    this._historyValuesPerSecond = historyValuesPerSecond;
    this._historyValuesCount = (int) ((double) historyWindowDuration * (double) historyValuesPerSecond);
    this._averageWindowValues = new Queue<AveragingValueRecorder.AverageValueData>(200);
    this._historyValues = new Queue<float>(this._historyValuesCount > 0 ? this._historyValuesCount : (int) ((double) historyValuesPerSecond * 300.0));
  }

  public virtual void Update(float value, float deltaTime)
  {
    this._lastValue = value;
    this._averageWindowValues.Enqueue(new AveragingValueRecorder.AverageValueData(value, deltaTime));
    this._averageWindowValuesDuration += deltaTime;
    while ((double) this._averageWindowValuesDuration > (double) this._averageWindowDuration)
    {
      this._averageWindowValuesDuration -= this._averageWindowValues.Peek().time;
      this._averageWindowValues.Dequeue();
    }
    this._averageValue = 0.0f;
    foreach (AveragingValueRecorder.AverageValueData averageWindowValue in this._averageWindowValues)
      this._averageValue += averageWindowValue.value * averageWindowValue.time / this._averageWindowValuesDuration;
    this._time += deltaTime;
    this._historyTime += deltaTime;
    if ((double) this._historyTime <= 1.0 / (double) this._historyValuesPerSecond)
      return;
    if (this._historyValues.Count == this._historyValuesCount && this._historyValuesCount > 0)
    {
      double num = (double) this._historyValues.Dequeue();
    }
    this._historyValues.Enqueue(this._averageValue);
    this._historyTime = 0.0f;
  }

  public virtual float GetAverageValue() => this._averageValue;

  public virtual float GetLastValue() => this._lastValue;

  public virtual Queue<float> GetHistoryValues() => this._historyValues;

  public struct AverageValueData
  {
    [CompilerGenerated]
    float m_Cvalue;
    [CompilerGenerated]
    float m_Ctime;

    public float value
    {
      get => this.m_Cvalue;
      private set => this.m_Cvalue = value;
    }

    public float time
    {
      get => this.m_Ctime;
      private set => this.m_Ctime = value;
    }

    public AverageValueData(float value, float time)
      : this()
    {
      this.value = value;
      this.time = time;
    }
  }
}
