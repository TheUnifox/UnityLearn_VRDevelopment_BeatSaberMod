// Decompiled with JetBrains decompiler
// Type: BezierSplineEvaluator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BezierSplineEvaluator
{
  protected readonly List<BezierCurve> _segments;
  protected int _currentSegmentIndex;
  protected const float kSlightAboveOne = 1.0005f;
  protected const float kSlightBelowZero = -0.0005f;

  public BezierSplineEvaluator(BezierSpline spline) => this._segments = spline.segments;

  public virtual Vector3 EvaluatePosition(float time) => this.Evaluate(this.OffsetSegmentAndGetT(time));

  public virtual Vector3 Evaluate(float t)
  {
    if (this._segments.Count == 0)
      return Vector3.zero;
    BezierCurve segment = this._segments[this._currentSegmentIndex];
    float num = 1f - t;
    return num * num * num * segment.p0 + 3f * num * num * t * segment.p1 + 3f * num * t * t * segment.p2 + t * t * t * segment.p3;
  }

  public virtual Vector3 EvaluateFirstDerivation(float t)
  {
    if (this._segments.Count == 0)
      return Vector3.zero;
    BezierCurve segment = this._segments[this._currentSegmentIndex];
    float num = 1f - t;
    return -3f * num * num * segment.p0 + (float) (3.0 * (1.0 - 4.0 * (double) t + 3.0 * (double) t * (double) t)) * segment.p1 + (float) (3.0 * (2.0 * (double) t - 3.0 * (double) t * (double) t)) * segment.p2 + 3f * t * t * segment.p3;
  }

  public virtual Vector3 EvaluateSecondDerivation(float t)
  {
    if (this._segments.Count == 0)
      return Vector3.zero;
    BezierCurve segment = this._segments[this._currentSegmentIndex];
    return 6f * (1f - t) * segment.p0 + (float) (3.0 * (6.0 * (double) t - 4.0)) * segment.p1 + (float) (3.0 * (2.0 - 6.0 * (double) t)) * segment.p2 + 6f * t * segment.p3;
  }

  public virtual float OffsetSegmentAndGetT(float time)
  {
    this.OffsetStartIndexToDistance(time);
    if (this._currentSegmentIndex == 0 && (double) time < (double) this._segments[this._currentSegmentIndex].p0.z)
      return 0.0f;
    if (this._currentSegmentIndex == this._segments.Count - 1)
      return 1f;
    float f = this.GetTForSegment(this._currentSegmentIndex, time);
    if (float.IsInfinity(f))
    {
      BezierCurve segment = this._segments[this._currentSegmentIndex];
      f = (float) (((double) time - (double) segment.p0.z) / ((double) segment.p3.z - (double) segment.p0.z));
    }
    return f;
  }

  public virtual float GetTForSegment(int segmentIndex, float time)
  {
    BezierCurve segment = this._segments[segmentIndex];
    float z1 = segment.p0.z;
    float z2 = segment.p1.z;
    float z3 = segment.p2.z;
    float z4 = segment.p3.z;
    float num1 = time;
    double a = -(double) z1 + 3.0 * (double) z2 - 3.0 * (double) z3 + (double) z4;
    float num2 = (float) (3.0 * (double) z1 - 6.0 * (double) z2 + 3.0 * (double) z3);
    float num3 = (float) (-3.0 * (double) z1 + 3.0 * (double) z2);
    float num4 = z1 - num1;
    double b = (double) num2;
    double c = (double) num3;
    double d = (double) num4;
    BezierSplineEvaluator.CubicSolveResult cubicSolveResult = BezierSplineEvaluator.SolveCubic((float) a, (float) b, (float) c, (float) d);
    if (cubicSolveResult.numberOfSolutions == 0)
      return 0.0f;
    if (cubicSolveResult.numberOfSolutions == 1)
      return cubicSolveResult.solution1;
    if (cubicSolveResult.numberOfSolutions == 2)
      return (double) cubicSolveResult.solution1 >= -0.00050000002374872565 && (double) cubicSolveResult.solution1 <= 1.000499963760376 ? cubicSolveResult.solution1 : cubicSolveResult.solution2;
    if (cubicSolveResult.numberOfSolutions != 3)
      return 0.0f;
    if ((double) cubicSolveResult.solution1 >= -0.00050000002374872565 && (double) cubicSolveResult.solution1 <= 1.000499963760376)
      return cubicSolveResult.solution1;
    return (double) cubicSolveResult.solution2 >= -0.00050000002374872565 && (double) cubicSolveResult.solution2 <= 1.000499963760376 ? cubicSolveResult.solution2 : cubicSolveResult.solution3;
  }

  public virtual void GetTimeValuesForSegment(
    int segmentIndex,
    out float t0Value,
    out float t1Value)
  {
    t0Value = this.GetTForSegment(segmentIndex, this._segments[segmentIndex].p0.z);
    t1Value = this.GetTForSegment(segmentIndex, this._segments[segmentIndex].p3.z);
  }

  public virtual void OffsetStartIndexToDistance(float time)
  {
    if (this._currentSegmentIndex == 0 && (double) time < (double) this._segments[this._currentSegmentIndex].p0.z)
      return;
    while (this._currentSegmentIndex > 0 && (double) time < (double) this._segments[this._currentSegmentIndex].p0.z)
      --this._currentSegmentIndex;
    while (this._currentSegmentIndex < this._segments.Count && (double) time > (double) this._segments[this._currentSegmentIndex].p0.z)
      ++this._currentSegmentIndex;
    if (this._currentSegmentIndex <= 0)
      return;
    --this._currentSegmentIndex;
  }

  private static float CubeRoot(float x)
  {
    float num = Mathf.Pow(Mathf.Abs(x), 0.333333343f);
    return (double) x >= 0.0 ? num : -num;
  }

  private static BezierSplineEvaluator.CubicSolveResult SolveCubic(
    float a,
    float b,
    float c,
    float d)
  {
    if ((double) Mathf.Abs(a) < (double) Mathf.Epsilon)
    {
      a = b;
      b = c;
      c = d;
      if ((double) Mathf.Abs(a) < (double) Mathf.Epsilon)
      {
        a = b;
        b = c;
        return (double) Mathf.Abs(a) >= (double) Mathf.Epsilon ? new BezierSplineEvaluator.CubicSolveResult(-b / a) : new BezierSplineEvaluator.CubicSolveResult();
      }
      float f = (float) ((double) b * (double) b - 4.0 * (double) a * (double) c);
      if ((double) Mathf.Abs(f) < (double) Mathf.Epsilon)
        return new BezierSplineEvaluator.CubicSolveResult((float) (-(double) b / (2.0 * (double) a)));
      return (double) f > 0.0 ? new BezierSplineEvaluator.CubicSolveResult((float) ((-(double) b + (double) Mathf.Sqrt(f)) / (2.0 * (double) a)), (float) ((-(double) b - (double) Mathf.Sqrt(f)) / (2.0 * (double) a))) : new BezierSplineEvaluator.CubicSolveResult();
    }
    float num1 = a * a;
    float num2 = b * b;
    float f1 = (float) ((3.0 * (double) a * (double) c - (double) num2) / (3.0 * (double) num1));
    float f2 = (float) ((2.0 * (double) num2 * (double) b - 9.0 * (double) a * (double) b * (double) c + 27.0 * (double) num1 * (double) d) / (27.0 * (double) num1 * (double) a));
    BezierSplineEvaluator.CubicSolveResult cubicSolveResult;
    if ((double) Mathf.Abs(f1) < (double) Mathf.Epsilon)
      cubicSolveResult = new BezierSplineEvaluator.CubicSolveResult(BezierSplineEvaluator.CubeRoot(-f2));
    else if ((double) Mathf.Abs(f2) < (double) Mathf.Epsilon)
    {
      cubicSolveResult = (double) f1 < 0.0 ? new BezierSplineEvaluator.CubicSolveResult(0.0f, Mathf.Sqrt(-f1), -Mathf.Sqrt(-f1)) : new BezierSplineEvaluator.CubicSolveResult(0.0f);
    }
    else
    {
      float f3 = (float) ((double) f2 * (double) f2 * 0.25 + (double) f1 * (double) f1 * (double) f1 / 27.0);
      if ((double) Mathf.Abs(f3) < (double) Mathf.Epsilon)
        cubicSolveResult = new BezierSplineEvaluator.CubicSolveResult(-1.5f * f2 / f1, 3f * f2 / f1);
      else if ((double) f3 > 0.0)
      {
        float num3 = BezierSplineEvaluator.CubeRoot((float) (-(double) f2 / 2.0) - Mathf.Sqrt(f3));
        cubicSolveResult = new BezierSplineEvaluator.CubicSolveResult(num3 - f1 / (3f * num3));
      }
      else
      {
        float num4 = 2f * Mathf.Sqrt((float) (-(double) f1 / 3.0));
        float f4 = Mathf.Acos(3f * f2 / f1 / num4) / 3f;
        cubicSolveResult = new BezierSplineEvaluator.CubicSolveResult(num4 * Mathf.Cos(f4), num4 * Mathf.Cos(f4 - 2.09439516f), num4 * Mathf.Cos(f4 - 4.18879032f));
      }
    }
    float num5 = b / (3f * a);
    cubicSolveResult.solution1 -= num5;
    cubicSolveResult.solution2 -= num5;
    cubicSolveResult.solution3 -= num5;
    return cubicSolveResult;
  }

  public struct CubicSolveResult
  {
    public readonly int numberOfSolutions;
    public float solution1;
    public float solution2;
    public float solution3;

    public CubicSolveResult(float solution1)
    {
      this.numberOfSolutions = 1;
      this.solution1 = solution1;
      this.solution2 = 0.0f;
      this.solution3 = 0.0f;
    }

    public CubicSolveResult(float solution1, float solution2)
    {
      this.numberOfSolutions = 2;
      this.solution1 = solution1;
      this.solution2 = solution2;
      this.solution3 = 0.0f;
    }

    public CubicSolveResult(float solution1, float solution2, float solution3)
    {
      this.numberOfSolutions = 3;
      this.solution1 = solution1;
      this.solution2 = solution2;
      this.solution3 = solution3;
    }
  }
}
