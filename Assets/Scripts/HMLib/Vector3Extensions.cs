// Decompiled with JetBrains decompiler
// Type: Vector3Extensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public static class Vector3Extensions
{
  public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
  {
    Vector3 vector3 = b - a;
    return Vector3.Dot(value - a, vector3) / Vector3.Dot(vector3, vector3);
  }

  public static Vector3 RotatedAroundPivot(this Vector3 vector, Quaternion rotation, Vector3 pivot) => rotation * (vector - pivot) + pivot;
}
