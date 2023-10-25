// Decompiled with JetBrains decompiler
// Type: SplineUtils
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class SplineUtils
{
  public static Vector3 Interpolate(Vector3 t0, Vector3 p0, Vector3 p1, Vector3 t1, float f)
  {
    double num1 = -0.5 * (double) t0.x + 1.5 * (double) p0.x + -1.5 * (double) p1.x + 0.5 * (double) t1.x;
    float num2 = (float) ((double) t0.x + -2.5 * (double) p0.x + 2.0 * (double) p1.x + -0.5 * (double) t1.x);
    float num3 = (float) (-0.5 * (double) t0.x + 0.5 * (double) p1.x);
    float x1 = p0.x;
    float num4 = (float) (-0.5 * (double) t0.y + 1.5 * (double) p0.y + -1.5 * (double) p1.y + 0.5 * (double) t1.y);
    float num5 = (float) ((double) t0.y + -2.5 * (double) p0.y + 2.0 * (double) p1.y + -0.5 * (double) t1.y);
    float num6 = (float) (-0.5 * (double) t0.y + 0.5 * (double) p1.y);
    float y1 = p0.y;
    float num7 = (float) (-0.5 * (double) t0.z + 1.5 * (double) p0.z + -1.5 * (double) p1.z + 0.5 * (double) t1.z);
    float num8 = (float) ((double) t0.z + -2.5 * (double) p0.z + 2.0 * (double) p1.z + -0.5 * (double) t1.z);
    float num9 = (float) (-0.5 * (double) t0.z + 0.5 * (double) p1.z);
    float z1 = p0.z;
    double num10 = (double) f;
    double x2 = ((num1 * num10 + (double) num2) * (double) f + (double) num3) * (double) f + (double) x1;
    float num11 = ((num4 * f + num5) * f + num6) * f + y1;
    float num12 = ((num7 * f + num8) * f + num9) * f + z1;
    double y2 = (double) num11;
    double z2 = (double) num12;
    return new Vector3((float) x2, (float) y2, (float) z2);
  }
}
