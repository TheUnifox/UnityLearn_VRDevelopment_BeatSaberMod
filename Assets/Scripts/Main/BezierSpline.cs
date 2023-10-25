// Decompiled with JetBrains decompiler
// Type: BezierSpline
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierSpline
{
  protected readonly List<BezierCurve> _segments = new List<BezierCurve>();
  protected readonly List<Vector3> _sourceDataPoints = new List<Vector3>();

  public List<BezierCurve> segments => this._segments;

  public virtual void AddPoint(float distance, Vector2 point) => this._sourceDataPoints.Add(new Vector3(point.x, point.y, distance));

  public virtual void SortSourceData() => this._sourceDataPoints.Sort((Comparison<Vector3>) ((point1, point2) => point1.z.CompareTo(point2.z)));

  public virtual void AddArtificialStartAndFinishPoint()
  {
    if (this._sourceDataPoints.Count < 2)
    {
      Debug.LogError((object) "Cannot add artificial points with 0 or 1 ");
    }
    else
    {
      Vector3 sourceDataPoint1 = this._sourceDataPoints[0];
      Vector3 sourceDataPoint2 = this._sourceDataPoints[1];
      this._sourceDataPoints.Insert(0, new Vector3(sourceDataPoint1.x, sourceDataPoint1.y, sourceDataPoint1.z - (sourceDataPoint2.z - sourceDataPoint1.z)));
      Vector3 sourceDataPoint3 = this._sourceDataPoints[this._sourceDataPoints.Count - 1];
      Vector3 sourceDataPoint4 = this._sourceDataPoints[this._sourceDataPoints.Count - 2];
      this._sourceDataPoints.Add(new Vector3(sourceDataPoint3.x, sourceDataPoint3.y, sourceDataPoint3.z + sourceDataPoint3.z - sourceDataPoint4.z));
    }
  }

  public virtual void ComputeControlPoints()
  {
    this._segments.Clear();
    if (this._sourceDataPoints.Count == 0)
    {
      Debug.LogWarning((object) "Unable to calculate path with 0 waypoints, returning empty path");
    }
    else
    {
      BezierSpline.ComputeControlPointsResults controlPoints1 = this.ComputeControlPoints(this._sourceDataPoints.Select<Vector3, float>((Func<Vector3, float>) (p => p.x)).ToList<float>());
      BezierSpline.ComputeControlPointsResults controlPoints2 = this.ComputeControlPoints(this._sourceDataPoints.Select<Vector3, float>((Func<Vector3, float>) (p => p.y)).ToList<float>());
      BezierSpline.ComputeControlPointsResults controlPoints3 = this.ComputeControlPoints(this._sourceDataPoints.Select<Vector3, float>((Func<Vector3, float>) (p => p.z)).ToList<float>());
      for (int index = 0; index < this._sourceDataPoints.Count - 1; ++index)
        this._segments.Add(new BezierCurve(this._sourceDataPoints[index], new Vector3(controlPoints1.p1[index], controlPoints2.p1[index], controlPoints3.p1[index]), new Vector3(controlPoints1.p2[index], controlPoints2.p2[index], controlPoints3.p2[index]), this._sourceDataPoints[index + 1]));
    }
  }

  public virtual void Clear()
  {
    this._sourceDataPoints.Clear();
    this._segments.Clear();
  }

  public virtual BezierSpline.ComputeControlPointsResults ComputeControlPoints(List<float> k)
  {
    int index1 = k.Count - 1;
    float[] p1 = new float[index1];
    float[] p2 = new float[index1];
    float[] numArray1 = new float[index1];
    float[] numArray2 = new float[index1];
    float[] numArray3 = new float[index1];
    float[] numArray4 = new float[index1];
    numArray1[0] = 0.0f;
    numArray2[0] = 2f;
    numArray3[0] = 1f;
    numArray4[0] = k[0] + 2f * k[1];
    for (int index2 = 1; index2 < index1 - 1; ++index2)
    {
      numArray1[index2] = 1f;
      numArray2[index2] = 4f;
      numArray3[index2] = 1f;
      numArray4[index2] = (float) (4.0 * (double) k[index2] + 2.0 * (double) k[index2 + 1]);
    }
    numArray1[index1 - 1] = 2f;
    numArray2[index1 - 1] = 7f;
    numArray3[index1 - 1] = 0.0f;
    numArray4[index1 - 1] = 8f * k[index1 - 1] + k[index1];
    for (int index3 = 1; index3 < index1; ++index3)
    {
      float num = numArray1[index3] / numArray2[index3 - 1];
      numArray2[index3] = numArray2[index3] - num * numArray3[index3 - 1];
      numArray4[index3] = numArray4[index3] - num * numArray4[index3 - 1];
    }
    p1[index1 - 1] = numArray4[index1 - 1] / numArray2[index1 - 1];
    for (int index4 = index1 - 2; index4 >= 0; --index4)
      p1[index4] = (numArray4[index4] - numArray3[index4] * p1[index4 + 1]) / numArray2[index4];
    for (int index5 = 0; index5 < index1 - 1; ++index5)
      p2[index5] = 2f * k[index5 + 1] - p1[index5 + 1];
    p2[index1 - 1] = (float) (0.5 * ((double) k[index1] + (double) p1[index1 - 1]));
    return new BezierSpline.ComputeControlPointsResults(p1, p2);
  }

  public readonly struct ComputeControlPointsResults
  {
    public readonly float[] p1;
    public readonly float[] p2;

    public ComputeControlPointsResults(float[] p1, float[] p2)
    {
      this.p1 = p1;
      this.p2 = p2;
    }
  }
}
