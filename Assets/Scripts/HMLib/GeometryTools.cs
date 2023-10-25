// Decompiled with JetBrains decompiler
// Type: GeometryTools
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class GeometryTools
{
  public static bool ThreePointsToBox(
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    out Vector3 center,
    out Vector3 halfSize,
    out Quaternion orientation)
  {
    Vector3 normalized1 = Vector3.Cross(p1 - p2, p0 - p2).normalized;
    if ((double) normalized1.sqrMagnitude > 9.9999997473787516E-06)
    {
      Vector3 normalized2 = (p0 - p1).normalized;
      Vector3 inNormal = Vector3.Cross(normalized2, normalized1);
      orientation = new Quaternion();
      orientation.SetLookRotation(normalized2, normalized1);
      float num1 = Mathf.Abs(new Plane(inNormal, p0).GetDistanceToPoint(p2));
      float num2 = Vector3.Magnitude(p0 - p1);
      Vector3 vector3 = (p0 + p1) * 0.5f;
      center = vector3 - inNormal * num1 * 0.5f;
      halfSize = new Vector3(num1 * 0.5f, 0.0f, num2 * 0.5f);
      return true;
    }
    center = Vector3.zero;
    halfSize = Vector3.zero;
    orientation = Quaternion.identity;
    return false;
  }
}
