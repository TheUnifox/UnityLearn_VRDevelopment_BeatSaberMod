// Decompiled with JetBrains decompiler
// Type: SaberClashChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SaberClashChecker
{
  protected const float kMinDistanceToClash = 0.08f;
  protected const float kIgnoredTime = 0.1f;
  protected bool _sabersAreClashing;
  protected Vector3 _clashingPoint;
  protected Saber _leftSaber;
  protected Saber _rightSaber;
  protected int _prevGetFrameNum = -1;

  [Inject]
  public virtual void Init(SaberManager saberManager)
  {
    this._leftSaber = saberManager.leftSaber;
    this._rightSaber = saberManager.rightSaber;
  }

  public virtual bool AreSabersClashing(out Vector3 clashingPoint)
  {
    if ((double) this._leftSaber.movementData.lastAddedData.time < 0.10000000149011612)
    {
      clashingPoint = this._clashingPoint;
      return false;
    }
    if (this._prevGetFrameNum == Time.frameCount)
    {
      clashingPoint = this._clashingPoint;
      return this._sabersAreClashing;
    }
    this._prevGetFrameNum = Time.frameCount;
    Vector3 saberBladeTopPos1 = this._leftSaber.saberBladeTopPos;
    Vector3 saberBladeTopPos2 = this._rightSaber.saberBladeTopPos;
    Vector3 saberBladeBottomPos1 = this._leftSaber.saberBladeBottomPos;
    Vector3 saberBladeBottomPos2 = this._rightSaber.saberBladeBottomPos;
    Vector3 inbetweenPoint;
    if ((double) this.SegmentToSegmentDist(saberBladeBottomPos1, saberBladeTopPos1, saberBladeBottomPos2, saberBladeTopPos2, out inbetweenPoint) < 0.079999998211860657 && this._leftSaber.isActiveAndEnabled && this._rightSaber.isActiveAndEnabled)
    {
      this._clashingPoint = inbetweenPoint;
      this._sabersAreClashing = true;
    }
    else
      this._sabersAreClashing = false;
    clashingPoint = this._clashingPoint;
    return this._sabersAreClashing;
  }

  public virtual float SegmentToSegmentDist(
    Vector3 fromA,
    Vector3 toA,
    Vector3 fromB,
    Vector3 toB,
    out Vector3 inbetweenPoint)
  {
    float num1 = 1E-06f;
    Vector3 vector3_1 = toA - fromA;
    Vector3 vector3_2 = toB - fromB;
    Vector3 rhs = fromA - fromB;
    float num2 = Vector3.Dot(vector3_1, vector3_1);
    float num3 = Vector3.Dot(vector3_1, vector3_2);
    float num4 = Vector3.Dot(vector3_2, vector3_2);
    float num5 = Vector3.Dot(vector3_1, rhs);
    float num6 = Vector3.Dot(vector3_2, rhs);
    double num7;
    float num8 = (float) (num7 = (double) num2 * (double) num4 - (double) num3 * (double) num3);
    float num9 = (float) num7;
    float f1;
    float f2;
    if (num7 < (double) num1)
    {
      f1 = 0.0f;
      num8 = 1f;
      f2 = num6;
      num9 = num4;
    }
    else
    {
      f1 = (float) ((double) num3 * (double) num6 - (double) num4 * (double) num5);
      f2 = (float) ((double) num2 * (double) num6 - (double) num3 * (double) num5);
      if ((double) f1 < 0.0)
      {
        f1 = 0.0f;
        f2 = num6;
        num9 = num4;
      }
      else if ((double) f1 > (double) num8)
      {
        f1 = num8;
        f2 = num6 + num3;
        num9 = num4;
      }
    }
    if ((double) f2 < 0.0)
    {
      f2 = 0.0f;
      if (-(double) num5 < 0.0)
        f1 = 0.0f;
      else if (-(double) num5 > (double) num2)
      {
        f1 = num8;
      }
      else
      {
        f1 = -num5;
        num8 = num2;
      }
    }
    else if ((double) f2 > (double) num9)
    {
      f2 = num9;
      if (-(double) num5 + (double) num3 < 0.0)
        f1 = 0.0f;
      else if (-(double) num5 + (double) num3 > (double) num2)
      {
        f1 = num8;
      }
      else
      {
        f1 = -num5 + num3;
        num8 = num2;
      }
    }
    float num10 = (double) Mathf.Abs(f1) < (double) num1 ? 0.0f : f1 / num8;
    float num11 = (double) Mathf.Abs(f2) < (double) num1 ? 0.0f : f2 / num9;
    Vector3 vector3_3 = fromA + num10 * vector3_1;
    Vector3 vector3_4 = fromB + num11 * vector3_2;
    inbetweenPoint = (vector3_3 + vector3_4) * 0.5f;
    return (vector3_3 - vector3_4).magnitude;
  }
}
