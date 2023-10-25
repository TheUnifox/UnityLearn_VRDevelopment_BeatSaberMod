// Decompiled with JetBrains decompiler
// Type: BeatmapObjectAvoidanceTiltEvaluator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BeatmapObjectAvoidanceTiltEvaluator
{
  protected readonly IAudioTimeSource _audioTimeSource;
  protected readonly Vector2 _gravity;
  protected readonly Vector2 _normalizedGravity;
  protected readonly BezierSplineEvaluator _bezierSplineEvaluator;
  protected float _currentAcceleration;
  protected const float kLookAheadTime = 0.2f;

  public BeatmapObjectAvoidanceTiltEvaluator(
    IAudioTimeSource audioTimeSource,
    BezierSplineEvaluator bezierSplineEvaluator,
    Vector2 gravity)
  {
    this._audioTimeSource = audioTimeSource;
    this._bezierSplineEvaluator = bezierSplineEvaluator;
    this._gravity = gravity;
    this._normalizedGravity = gravity.normalized;
  }

  public virtual float GetTiltAngle()
  {
    float time = this._audioTimeSource.songTime + 0.2f;
    float x1 = this._bezierSplineEvaluator.EvaluatePosition(time - 0.1f).x;
    float x2 = this._bezierSplineEvaluator.EvaluatePosition(time).x;
    double x3 = (double) this._bezierSplineEvaluator.EvaluatePosition(time + 0.1f).x;
    float num1 = (float) (((double) x2 - (double) x1) / 0.10000000149011612);
    double num2 = (double) x2;
    this._currentAcceleration = (float) (((x3 - num2) / 0.10000000149011612 - (double) num1) / 0.10000000149011612);
    return -Vector2.SignedAngle(this._normalizedGravity, (new Vector2(this._currentAcceleration, 0.0f) + this._gravity).normalized);
  }
}
