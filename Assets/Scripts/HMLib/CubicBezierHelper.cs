// Decompiled with JetBrains decompiler
// Type: CubicBezierHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class CubicBezierHelper
{
  public static Vector3 EvaluateCurve(
    in Vector3 a1,
    in Vector3 c1,
    in Vector3 c2,
    in Vector3 a2,
    float t)
  {
    return (float) ((1.0 - (double) t) * (1.0 - (double) t) * (1.0 - (double) t)) * a1 + (float) (3.0 * (1.0 - (double) t) * (1.0 - (double) t)) * t * c1 + (float) (3.0 * (1.0 - (double) t)) * t * t * c2 + t * t * t * a2;
  }

  public static Vector3 EvaluateCurveDerivative(
    in Vector3 a1,
    in Vector3 c1,
    in Vector3 c2,
    in Vector3 a2,
    float t)
  {
    return (float) (3.0 * (1.0 - (double) t) * (1.0 - (double) t)) * (c1 - a1) + (float) (6.0 * (1.0 - (double) t)) * t * (c2 - c1) + 3f * t * t * (a2 - c2);
  }

  public static Vector3 EvaluateCurveSecondDerivative(
    in Vector3 a1,
    in Vector3 c1,
    in Vector3 c2,
    in Vector3 a2,
    float t)
  {
    return (float) (6.0 * (1.0 - (double) t)) * (c2 - 2f * c1 + a1) + 6f * t * (a2 - 2f * c2 + c1);
  }

  public static Vector3 Normal(
    in Vector3 a1,
    in Vector3 c1,
    in Vector3 c2,
    in Vector3 a2,
    float t)
  {
    Vector3 curveDerivative = CubicBezierHelper.EvaluateCurveDerivative(in a1, in c1, in c2, in a2, t);
    return Vector3.Cross(Vector3.Cross(CubicBezierHelper.EvaluateCurveSecondDerivative(in a1, in c1, in c2, in a2, t), curveDerivative), curveDerivative).normalized;
  }

  public static void SplitCurve(List<Vector3> points, float t)
  {
    Vector3 point1 = points[0];
    Vector3 point2 = points[1];
    Vector3 point3 = points[2];
    Vector3 point4 = points[3];
    Vector3 a1 = Vector3.Lerp(point1, point2, t);
    Vector3 vector3_1 = Vector3.Lerp(point2, point3, t);
    Vector3 b1 = Vector3.Lerp(point3, point4, t);
    Vector3 a2 = Vector3.Lerp(a1, vector3_1, t);
    Vector3 b2 = Vector3.Lerp(vector3_1, b1, t);
    Vector3 vector3_2 = Vector3.Lerp(a2, b2, t);
    points.Clear();
    points.Add(point1);
    points.Add(a1);
    points.Add(a2);
    points.Add(vector3_2);
    points.Add(b2);
    points.Add(b1);
    points.Add(point4);
  }

  public static float EstimateCurveLength(
    in Vector3 p0,
    in Vector3 p1,
    in Vector3 p2,
    in Vector3 p3)
  {
    Vector3 vector3 = p0 - p1;
    double magnitude1 = (double) vector3.magnitude;
    vector3 = p1 - p2;
    double magnitude2 = (double) vector3.magnitude;
    double num1 = magnitude1 + magnitude2;
    vector3 = p2 - p3;
    double magnitude3 = (double) vector3.magnitude;
    float num2 = (float) (num1 + magnitude3);
    return (p0 - p3).magnitude + num2 / 2f;
  }
}
