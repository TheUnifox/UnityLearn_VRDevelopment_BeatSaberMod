// Decompiled with JetBrains decompiler
// Type: Vector2Extensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public static class Vector2Extensions
{
  public static float SignedAngleToLine(this Vector2 vec, Vector2 line)
  {
    float f1 = Vector2.SignedAngle(vec, line);
    float f2 = Vector2.SignedAngle(vec, -line);
    return (double) Mathf.Abs(f1) >= (double) Mathf.Abs(f2) ? f2 : f1;
  }
}
