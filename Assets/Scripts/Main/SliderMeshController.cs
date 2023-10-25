// Decompiled with JetBrains decompiler
// Type: SliderMeshController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class SliderMeshController : MonoBehaviour
{
  [SerializeField]
  protected int _numberOfFixedVertexPathSegments = 50;
  [SerializeField]
  protected float _controlPointDistancePerSqrtNotesDistance = 1f;
  [SerializeField]
  protected float _middleAnchorPointOffsetAmount = 0.5f;
  [SerializeField]
  protected float _middleControlPointZDistanceModifier = 0.15f;
  [SerializeField]
  protected float _middleControlPointYDistanceModifier = 0.25f;
  [SerializeField]
  protected float _middleControlPointXDistanceModifier = 0.25f;
  [Space]
  [SerializeField]
  protected SliderMeshConstructor _sliderMeshConstructor;
  protected PathsHolder _pathsHolder;
  protected readonly List<Vector3> _reusableAnchorsList = new List<Vector3>(3);
  protected readonly Vector3[] _reusableControlPointsArray4 = new Vector3[4];
  protected readonly Vector3[] _reusableControlPointsArray2 = new Vector3[2];
  public const float kDefaultGameNoteSize = 0.45f;

  public float pathLength => this._pathsHolder.vertexPath.length;

  public Mesh mesh => this._sliderMeshConstructor.mesh;

  public PathsHolder pathsHolder => this._pathsHolder;

  public virtual void CreateBezierPathAndMesh(
    SliderData sliderData,
    Vector3 headNotePos,
    Vector3 tailNotePos,
    float jumpSpeed,
    float noteUniformScale)
  {
    if (this._pathsHolder == null)
    {
      this._pathsHolder = new PathsHolder(this._numberOfFixedVertexPathSegments, false);
      this._pathsHolder.bezierPath.controlPointMode = BezierPath.ControlMode.Aligned;
    }
    BezierPath bezierPath = this._pathsHolder.bezierPath;
    float z1 = (float) (0.44999998807907104 * (double) noteUniformScale * 0.5);
    float num1 = (sliderData.tailTime - sliderData.time) * jumpSpeed;
    Vector2 vector = sliderData.headCutDirection.Direction();
    Vector2 vector2_1 = sliderData.tailCutDirection.Direction();
    Vector2 vector2_2 = z1 * vector;
    Vector2 vector2_3 = z1 * -vector2_1;
    if (sliderData.sliderType == SliderData.Type.Burst)
    {
      vector2_2 *= 0.1f;
      vector2_3 *= 0.1f;
    }
    else
    {
      if (!sliderData.hasHeadNote)
        vector2_2 *= 0.1f;
      if (!sliderData.hasTailNote)
        vector2_3 *= 0.1f;
    }
    Vector3 a = new Vector3(headNotePos.x + vector2_2.x, headNotePos.y + vector2_2.y, z1);
    Vector3 b = new Vector3(tailNotePos.x + vector2_3.x, tailNotePos.y + vector2_3.y, num1 + z1);
    Vector3 vector3_1 = (a + b) * 0.5f;
    float rads = 0.0f;
    bool flag = sliderData.headCutDirection.IsOnSamePlane(sliderData.tailCutDirection) && sliderData.headLineIndex == sliderData.tailLineIndex;
    if (flag)
    {
      if (sliderData.midAnchorMode == SliderMidAnchorMode.Clockwise)
        rads = -1.57079637f;
      else if (sliderData.midAnchorMode == SliderMidAnchorMode.CounterClockwise)
        rads = 1.57079637f;
      else
        flag = false;
    }
    Vector2 vector2_4 = vector.Rotate(rads);
    Vector3 vector3_2 = new Vector3(vector2_4.x, vector2_4.y, 0.0f);
    Vector3 vector3_3 = vector3_1 + vector3_2 * this._middleAnchorPointOffsetAmount;
    this._reusableAnchorsList.Clear();
    this._reusableAnchorsList.Add(a);
    if (flag)
      this._reusableAnchorsList.Add(vector3_3);
    this._reusableAnchorsList.Add(b);
    bezierPath.UpdateByAnchorPoints((IReadOnlyList<Vector3>) this._reusableAnchorsList);
    float sqrtNotesDistance = this._controlPointDistancePerSqrtNotesDistance;
    Vector3 vector3_4 = SliderMeshController.CutDirectionToControlPointPosition(sliderData.headCutDirection) * (sqrtNotesDistance * sliderData.headControlPointLengthMultiplier);
    Vector3 vector3_5 = -SliderMeshController.CutDirectionToControlPointPosition(sliderData.tailCutDirection) * (sqrtNotesDistance * sliderData.tailControlPointLengthMultiplier);
    Vector3 vector3_6 = a + vector3_4;
    Vector3 vector3_7 = b + vector3_5;
    if (sliderData.sliderType == SliderData.Type.Burst)
    {
      vector3_6 = Vector3.Lerp(a, b, 0.3333f);
      vector3_7 = Vector3.Lerp(a, b, 0.6667f);
    }
    if (flag)
    {
      Vector3 vector3_8 = vector3_6 - vector3_3;
      Vector3 vector3_9 = vector3_7 - vector3_3;
      float z2 = (Mathf.Abs(vector3_8.z) + Mathf.Abs(vector3_9.z)) * this._middleControlPointZDistanceModifier;
      float num2 = (Mathf.Abs(vector3_8.y) + Mathf.Abs(vector3_9.y)) * this._middleControlPointYDistanceModifier;
      double num3 = ((double) Mathf.Abs(vector3_8.x) + (double) Mathf.Abs(vector3_9.x)) * (double) this._middleControlPointXDistanceModifier;
      float y1 = num2;
      float y2 = -num2;
      if ((double) vector3_6.y < (double) vector3_7.y)
      {
        y1 = -y1;
        y2 = -y2;
      }
      if (Mathf.Approximately(vector3_6.y, vector3_7.y))
      {
        y1 = 0.0f;
        y2 = 0.0f;
      }
      float x1 = (float) num3;
      float x2 = (float) -num3;
      if ((double) vector3_6.x < (double) vector3_7.x)
      {
        x1 = -x1;
        x2 = -x2;
      }
      if (Mathf.Approximately(vector3_6.x, vector3_7.x))
      {
        x1 = 0.0f;
        x2 = 0.0f;
      }
      Vector3 vector3_10 = vector3_3 + new Vector3(x1, y1, -z2);
      Vector3 vector3_11 = vector3_3 + new Vector3(x2, y2, z2);
      this._reusableControlPointsArray4[0] = vector3_6;
      this._reusableControlPointsArray4[1] = vector3_10;
      this._reusableControlPointsArray4[2] = vector3_11;
      this._reusableControlPointsArray4[3] = vector3_7;
      bezierPath.UpdateControlPoints((IReadOnlyList<Vector3>) this._reusableControlPointsArray4);
      bezierPath.SetAnchorNormalAngle(2, 0.0f);
    }
    else
    {
      this._reusableControlPointsArray2[0] = vector3_6;
      this._reusableControlPointsArray2[1] = vector3_7;
      bezierPath.UpdateControlPoints((IReadOnlyList<Vector3>) this._reusableControlPointsArray2);
    }
    bezierPath.SetAnchorNormalAngle(0, 0.0f);
    bezierPath.SetAnchorNormalAngle(1, 0.0f);
    this._pathsHolder.UpdateVertexPathByBezierPath();
    this._sliderMeshConstructor.CreateMeshIfNonExisting();
    this._sliderMeshConstructor.CreateSliderMesh(this._pathsHolder.vertexPath);
  }

  private static Vector3 CutDirectionToControlPointPosition(NoteCutDirection noteCutDirection)
  {
    switch (noteCutDirection)
    {
      case NoteCutDirection.Up:
        return new Vector3(0.0f, 1f, -1E-05f);
      case NoteCutDirection.Down:
        return new Vector3(0.0f, -1f, -1E-05f);
      case NoteCutDirection.Left:
        return new Vector3(-1f, 0.0f, -1E-05f);
      case NoteCutDirection.Right:
        return new Vector3(1f, 0.0f, -1E-05f);
      case NoteCutDirection.UpLeft:
        return new Vector3(-0.707106769f, 0.707106769f, -1E-05f);
      case NoteCutDirection.UpRight:
        return new Vector3(0.707106769f, 0.707106769f, -1E-05f);
      case NoteCutDirection.DownLeft:
        return new Vector3(-0.707106769f, -0.707106769f, -1E-05f);
      case NoteCutDirection.DownRight:
        return new Vector3(0.707106769f, -0.707106769f, -1E-05f);
      case NoteCutDirection.Any:
      case NoteCutDirection.None:
        return Vector3.zero;
      default:
        return Vector3.zero;
    }
  }
}
