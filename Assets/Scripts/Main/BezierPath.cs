// Decompiled with JetBrains decompiler
// Type: BezierPath
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BezierPath
{
  protected readonly List<Vector3> _points;
  protected BezierPath.ControlMode _controlMode;
  protected List<float> _perAnchorNormalsAngle;
  protected const float kAutoControlLength = 0.3f;
  protected float[] _neighbourDistances;

  public BezierPath.ControlMode controlPointMode
  {
    get => this._controlMode;
    set
    {
      if (this._controlMode == value)
        return;
      this._controlMode = value;
      if (this._controlMode != BezierPath.ControlMode.Automatic)
        return;
      this.AutoSetAllControlPoints();
      this.NotifyPathModified();
    }
  }

  public event System.Action bezierPathWasModifiedEvent;

  public int pointsCount => this._points.Count;

  public int anchorPointsCount => (this._points.Count + 2) / 3;

  public int segmentsCount => this._points.Count / 3;

  public BezierPath(Vector3 centre, bool initTwoSegments = false)
  {
    Vector3 up = Vector3.up;
    this._points = new List<Vector3>()
    {
      centre + Vector3.left * 2f,
      centre + Vector3.left * 1f + up * 0.5f,
      centre + Vector3.right * 1f - up * 0.5f,
      centre + Vector3.right * 2f
    };
    this._perAnchorNormalsAngle = new List<float>()
    {
      0.0f,
      0.0f
    };
    if (initTwoSegments)
    {
      CubicBezierHelper.SplitCurve(this._points, 0.5f);
      this._perAnchorNormalsAngle.Add(0.0f);
    }
    this._neighbourDistances = new float[2];
  }

  public virtual void UpdateByAnchorPoints(IReadOnlyList<Vector3> points)
  {
    if (points.Count < 2)
    {
      Debug.LogError((object) "Path requires at least 2 anchor points.");
    }
    else
    {
      this._points.Clear();
      this._points.Add(points[0]);
      this._points.Add(Vector3.zero);
      this._points.Add(Vector3.zero);
      this._points.Add(points[1]);
      this._perAnchorNormalsAngle.Clear();
      this._perAnchorNormalsAngle.Add(0.0f);
      this._perAnchorNormalsAngle.Add(0.0f);
      for (int index = 2; index < points.Count; ++index)
      {
        this.AddSegmentToEnd(points[index]);
        this._perAnchorNormalsAngle.Add(0.0f);
      }
    }
  }

  public virtual void UpdateControlPoints(IReadOnlyList<Vector3> points)
  {
    if (points.Count % 2 != 0)
    {
      Debug.LogError((object) "Between any anchors there need to be 2 control points.");
    }
    else
    {
      for (int index = 0; index < points.Count / 2; ++index)
      {
        this.SetPoint(1 + index * 3, points[index * 2]);
        this.SetPoint(2 + index * 3, points[index * 2 + 1]);
      }
    }
  }

  public Vector3 this[int i] => this.GetPoint(i);

  public virtual Vector3 GetPoint(int i) => this._points[i];

  public virtual void SetPoint(int i, Vector3 localPosition, bool suppressPathModified = false)
  {
    this._points[i] = localPosition;
    if (suppressPathModified)
      return;
    this.NotifyPathModified();
  }

  public virtual void AddSegmentToEnd(Vector3 anchorPos)
  {
    int index = this._points.Count - 1;
    Vector3 vector3_1 = this._points[index] - this._points[index - 1];
    if (this._controlMode != BezierPath.ControlMode.Mirrored && this._controlMode != BezierPath.ControlMode.Automatic)
    {
      float magnitude = (this._points[index] - anchorPos).magnitude;
      vector3_1 = (this._points[index] - this._points[index - 1]).normalized * magnitude * 0.5f;
    }
    Vector3 vector3_2 = this._points[index] + vector3_1;
    Vector3 vector3_3 = (anchorPos + vector3_2) * 0.5f;
    this._points.Add(vector3_2);
    this._points.Add(vector3_3);
    this._points.Add(anchorPos);
    this._perAnchorNormalsAngle.Add(this._perAnchorNormalsAngle[this._perAnchorNormalsAngle.Count - 1]);
    if (this._controlMode == BezierPath.ControlMode.Automatic)
      this.AutoSetAllAffectedControlPoints(this._points.Count - 1);
    this.NotifyPathModified();
  }

  public virtual void GetPointsInSegment(int segmentIndex, ref Vector3[] points)
  {
    segmentIndex = Mathf.Clamp(segmentIndex, 0, this.segmentsCount - 1);
    for (int index = 0; index < 4; ++index)
      points[index] = this[segmentIndex * 3 + index];
  }

  public virtual void GetPointsInSegment(
    int segmentIndex,
    out Vector3 p0,
    out Vector3 p1,
    out Vector3 p2,
    out Vector3 p3)
  {
    int index = 3 * Mathf.Clamp(segmentIndex, 0, this.segmentsCount - 1);
    p0 = this._points[index];
    p1 = this._points[index + 1];
    p2 = this._points[index + 2];
    p3 = this._points[index + 3];
  }

  public virtual float GetAnchorNormalAngle(int anchorIndex) => this._perAnchorNormalsAngle[anchorIndex] % 360f;

  public virtual void SetAnchorNormalAngle(int anchorIndex, float angle)
  {
    angle = (float) (((double) angle + 360.0) % 360.0);
    if (Mathf.Approximately(this._perAnchorNormalsAngle[anchorIndex], angle))
      return;
    this._perAnchorNormalsAngle[anchorIndex] = angle;
    this.NotifyPathModified();
  }

  public virtual void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
  {
    for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
    {
      if (i >= 0 && i < this._points.Count)
        this.AutoSetAnchorControlPoints(this.LoopIndex(i));
    }
    this.AutoSetStartAndEndControls();
  }

  public virtual void AutoSetAllControlPoints()
  {
    if (this.anchorPointsCount > 2)
    {
      for (int anchorIndex = 0; anchorIndex < this._points.Count; anchorIndex += 3)
        this.AutoSetAnchorControlPoints(anchorIndex);
    }
    this.AutoSetStartAndEndControls();
  }

  public virtual void AutoSetAnchorControlPoints(int anchorIndex)
  {
    Vector3 point = this._points[anchorIndex];
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < 2; ++index)
      this._neighbourDistances[index] = 0.0f;
    if (anchorIndex - 3 >= 0)
    {
      Vector3 vector3 = this._points[this.LoopIndex(anchorIndex - 3)] - point;
      zero += vector3.normalized;
      this._neighbourDistances[0] = vector3.magnitude;
    }
    if (anchorIndex + 3 >= 0)
    {
      Vector3 vector3 = this._points[this.LoopIndex(anchorIndex + 3)] - point;
      zero -= vector3.normalized;
      this._neighbourDistances[1] = -vector3.magnitude;
    }
    zero.Normalize();
    for (int index = 0; index < 2; ++index)
    {
      int i = anchorIndex + index * 2 - 1;
      if (i >= 0 && i < this._points.Count)
        this._points[this.LoopIndex(i)] = point + zero * this._neighbourDistances[index] * 0.3f;
    }
  }

  public virtual void AutoSetStartAndEndControls()
  {
    if (this.anchorPointsCount == 2)
    {
      this._points[1] = this._points[0] + (this._points[3] - this._points[0]) * 0.25f;
      this._points[2] = this._points[3] + (this._points[0] - this._points[3]) * 0.25f;
    }
    else
    {
      this._points[1] = (this._points[0] + this._points[2]) * 0.5f;
      this._points[this._points.Count - 2] = (this._points[this._points.Count - 1] + this._points[this._points.Count - 3]) * 0.5f;
    }
  }

  public virtual int LoopIndex(int i) => (i + this._points.Count) % this._points.Count;

  public virtual void NotifyPathModified()
  {
    System.Action wasModifiedEvent = this.bezierPathWasModifiedEvent;
    if (wasModifiedEvent == null)
      return;
    wasModifiedEvent();
  }

  public enum ControlMode
  {
    Aligned,
    Mirrored,
    Free,
    Automatic,
  }
}
