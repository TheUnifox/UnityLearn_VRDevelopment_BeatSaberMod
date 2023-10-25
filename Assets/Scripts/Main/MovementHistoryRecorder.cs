// Decompiled with JetBrains decompiler
// Type: MovementHistoryRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MovementHistoryRecorder
{
  protected AveragingValueRecorder _averagingValueRecorer;
  protected float _increaseSpeed;
  protected float _decreaseSpeed;
  protected float _accum;

  public AveragingValueRecorder averagingValueRecorer => this._averagingValueRecorer;

  public MovementHistoryRecorder(
    float averageWindowDuration,
    float historyValuesPerSecond,
    float increaseSpeed,
    float decreaseSpeed)
  {
    this._averagingValueRecorer = new AveragingValueRecorder(averageWindowDuration, -1f, historyValuesPerSecond);
    this._increaseSpeed = increaseSpeed;
    this._decreaseSpeed = decreaseSpeed;
  }

  public virtual void AddMovement(float distance) => this._accum += distance * this._increaseSpeed / Mathf.Max(1f, this._accum);

  public virtual void ManualUpdate(float deltaTime)
  {
    this._accum -= this._decreaseSpeed * deltaTime;
    if ((double) this._accum < 0.0)
      this._accum = 0.0f;
    this._averagingValueRecorer.Update(this._accum, deltaTime);
  }
}
