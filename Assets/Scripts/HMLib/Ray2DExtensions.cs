// Decompiled with JetBrains decompiler
// Type: Ray2DExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public static class Ray2DExtensions
{
  public static int CircleIntersections(
    this Ray2D ray,
    Vector2 circleCenter,
    float radius,
    float[] distances)
  {
    int index = 0;
    float num1 = (float) ((double) ray.direction.x * (double) ray.direction.x + (double) ray.direction.y * (double) ray.direction.y);
    float num2 = (float) (2.0 * ((double) ray.direction.x * ((double) ray.origin.x - (double) circleCenter.x) + (double) ray.direction.y * ((double) ray.origin.y - (double) circleCenter.y)));
    float num3 = (float) (((double) ray.origin.x - (double) circleCenter.x) * ((double) ray.origin.x - (double) circleCenter.x) + ((double) ray.origin.y - (double) circleCenter.y) * ((double) ray.origin.y - (double) circleCenter.y) - (double) radius * (double) radius);
    float f = (float) ((double) num2 * (double) num2 - 4.0 * (double) num1 * (double) num3);
    if ((double) num1 <= 1E-07 || (double) f < 0.0)
      index = 0;
    else if ((double) f == 0.0)
    {
      float num4 = (float) (-(double) num2 / (2.0 * (double) num1));
      if ((double) num4 >= 0.0)
      {
        distances[0] = num4;
        ++index;
      }
    }
    else
    {
      float num5 = (float) ((-(double) num2 + (double) Mathf.Sqrt(f)) / (2.0 * (double) num1));
      if ((double) num5 >= 0.0)
      {
        distances[index] = num5;
        ++index;
      }
      float num6 = (float) ((-(double) num2 - (double) Mathf.Sqrt(f)) / (2.0 * (double) num1));
      if ((double) num6 >= 0.0)
      {
        distances[index] = num6;
        ++index;
      }
    }
    return index;
  }
}
