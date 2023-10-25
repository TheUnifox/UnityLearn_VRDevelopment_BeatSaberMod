// Decompiled with JetBrains decompiler
// Type: VertexPath
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class VertexPath
{
  public readonly int vertexCount;
  protected readonly VertexPath.Vertex[] _localVertices;
  protected float _length;
  protected readonly float[] _cumulativeLengthAtEachVertex;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 _back = Vector3.back;
  protected readonly int[] _anchorVertexMap;

  public float length => this._length;

  public VertexPath(int numberOfPathSegments)
  {
    this.vertexCount = numberOfPathSegments + 1;
    this._localVertices = new VertexPath.Vertex[this.vertexCount];
    this._cumulativeLengthAtEachVertex = new float[this.vertexCount];
    this._anchorVertexMap = new int[2]
    {
      0,
      this.vertexCount - 1
    };
    this._length = 0.0f;
  }

  public virtual void UpdateByBezierPath(BezierPath bezierPath)
  {
    this.SplitBezierPathIntoFixNumberOfSegments(bezierPath, this.vertexCount - 1);
    this._length = this._cumulativeLengthAtEachVertex[this.vertexCount - 1];
    VertexPath.Vertex localVertex = this._localVertices[this._localVertices.Length - 1];
    Vector3 normalized = Vector3.Cross(VertexPath._back, localVertex.tangent).normalized;
    float num1 = Vector3.Angle(localVertex.normal, normalized);
    if ((double) this._localVertices[0].position.x < (double) localVertex.position.x)
      num1 = -num1;
    for (int anchorIndex = 0; anchorIndex < this._anchorVertexMap.Length - 1; ++anchorIndex)
    {
      float anchorNormalAngle = bezierPath.GetAnchorNormalAngle(anchorIndex);
      float target = bezierPath.GetAnchorNormalAngle(anchorIndex + 1) + num1;
      float num2 = Mathf.DeltaAngle(anchorNormalAngle, target);
      int anchorVertex = this._anchorVertexMap[anchorIndex];
      int num3 = this._anchorVertexMap[anchorIndex + 1] - anchorVertex;
      if (anchorIndex == this._anchorVertexMap.Length - 2)
        ++num3;
      for (int index1 = 0; index1 < num3; ++index1)
      {
        int index2 = anchorVertex + index1;
        float num4 = num3 == 1 ? 1f : (float) index1 / ((float) num3 - 1f);
        double angle = (double) anchorNormalAngle + (double) num2 * (double) num4;
        ref VertexPath.Vertex local = ref this._localVertices[index2];
        Vector3 tangent = local.tangent;
        Quaternion quaternion = Quaternion.AngleAxis((float) angle, tangent);
        local.normal = quaternion * local.normal;
      }
    }
  }

  public virtual float TimeAtPoint(int pointIndex) => this._cumulativeLengthAtEachVertex[pointIndex] / this._length;

  public virtual void GetVertex(
    int index,
    out Vector3 position,
    out Vector3 tangent,
    out Vector3 normal)
  {
    ref VertexPath.Vertex local = ref this._localVertices[index];
    position = local.position;
    tangent = local.tangent;
    normal = local.normal;
  }

  public virtual Vector3 GetPoint(int index) => this._localVertices[index].position;

  public virtual void SplitBezierPathIntoFixNumberOfSegments(
    BezierPath bezierPath,
    int numberOfVertexSegments)
  {
    int num1 = 0;
    Vector3 back = VertexPath._back;
    Vector3 p0;
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;
    bezierPath.GetPointsInSegment(0, out p0, out p1, out p2, out p3);
    VertexPath.Vertex lastVertex = new VertexPath.Vertex()
    {
      position = bezierPath[0],
      tangent = CubicBezierHelper.EvaluateCurveDerivative(in p0, in p1, in p2, in p3, 0.0f).normalized
    };
    lastVertex.normal = Vector3.Cross(back, lastVertex.tangent).normalized;
    this._localVertices[0] = lastVertex;
    this._cumulativeLengthAtEachVertex[0] = 0.0f;
    int vertCount = num1 + 1;
    float num2 = 0.0f;
    for (int segmentIndex = 0; segmentIndex < bezierPath.segmentsCount; ++segmentIndex)
    {
      bezierPath.GetPointsInSegment(segmentIndex, out p0, out p1, out p2, out p3);
      float num3 = CubicBezierHelper.EstimateCurveLength(in p0, in p1, in p2, in p3);
      num2 += num3;
    }
    float num4 = num2 / (float) numberOfVertexSegments;
    float num5 = 0.0f;
    float currentPathLength = 0.0f;
    for (int segmentIndex = 0; segmentIndex < bezierPath.segmentsCount; ++segmentIndex)
    {
      bezierPath.GetPointsInSegment(segmentIndex, out p0, out p1, out p2, out p3);
      float num6 = CubicBezierHelper.EstimateCurveLength(in p0, in p1, in p2, in p3);
      num5 += num6;
      while ((double) num5 > (double) num4)
      {
        num5 -= num4;
        float t = (float) (1.0 - (double) num5 / (double) num6);
        this.AddVertex(in p0, in p1, in p2, in p3, t, ref back, ref currentPathLength, ref lastVertex, ref vertCount);
      }
    }
    if (vertCount == numberOfVertexSegments)
    {
      this.AddVertex(in p0, in p1, in p2, in p3, 1f, ref back, ref currentPathLength, ref lastVertex, ref vertCount);
    }
    else
    {
      if ((double) num5 <= 0.0099999997764825821)
        return;
      Debug.LogError((object) string.Format("This should not happen, it will increase number of segments by one, remainingSegmentLength: {0}, lengthPerVertexSegment: {1}", (object) num5, (object) num4));
    }
  }

  public virtual void AddVertex(
    in Vector3 p0,
    in Vector3 p1,
    in Vector3 p2,
    in Vector3 p3,
    float t,
    ref Vector3 lastRotationAxis,
    ref float currentPathLength,
    ref VertexPath.Vertex lastVertex,
    ref int vertCount)
  {
    VertexPath.Vertex vertex = new VertexPath.Vertex()
    {
      position = CubicBezierHelper.EvaluateCurve(in p0, in p1, in p2, in p3, t),
      tangent = CubicBezierHelper.EvaluateCurveDerivative(in p0, in p1, in p2, in p3, t).normalized
    };
    Vector3 lhs = lastVertex.position - vertex.position;
    float sqrMagnitude = lhs.sqrMagnitude;
    Vector3 rhs = lastRotationAxis - lhs * (2f / sqrMagnitude * Vector3.Dot(lhs, lastRotationAxis));
    Vector3 vector3_1 = lastVertex.tangent - lhs * (2f / sqrMagnitude * Vector3.Dot(lhs, lastVertex.tangent));
    Vector3 vector3_2 = vertex.tangent - vector3_1;
    float num = Vector3.Dot(vector3_2, vector3_2);
    lastRotationAxis = rhs - vector3_2 * (2f / num * Vector3.Dot(vector3_2, rhs));
    vertex.normal = Vector3.Cross(lastRotationAxis, vertex.tangent).normalized;
    currentPathLength += Mathf.Sqrt(sqrMagnitude);
    this._cumulativeLengthAtEachVertex[vertCount] = currentPathLength;
    this._localVertices[vertCount] = vertex;
    lastVertex = vertex;
    ++vertCount;
  }

  public struct Vertex
  {
    public Vector3 position;
    public Vector3 tangent;
    public Vector3 normal;
  }
}
